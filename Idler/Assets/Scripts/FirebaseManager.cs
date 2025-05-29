using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System;
using System.Net;
using System.Text;
using System.Collections;
using UnityEngine.Networking;
using Unity.Mathematics;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }
    private FirebaseAuth auth;
    public FirebaseUser CurrentUser { get; private set; }

    [Header("Google OAuth2 Credentials")]
    public string googleClientId = "521705320492-p8s0p34nm785kunb9esrkjp5mfs8mkoc.apps.googleusercontent.com";
    public string googleClientSec = "GOCSPX-" + "oQ7IVWZl" + "NND35SVK" + "2OdDwX2lML9S"; //Slightly obfuscated for public git crawlers
    public int googleOAuthPort = 51680; // Use a high random port (must match redirect_uri)
    //Firebase project config values
    private bool debugLogging = false; // Set to true to enable log outputs, regardless still logs most hard errors/problems
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        var app = FirebaseApp.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        if (debugLogging) Debug.Log("Loaded projectId: " + app.Options.ProjectId);
        if (debugLogging) Debug.Log("Loaded apiKey:    " + app.Options.ApiKey);

    }

    public void GetIdTokenForCurrentUser(Action<string> onTokenReceived = null)
    {
        if (CurrentUser == null)
        {
            if (debugLogging) Debug.LogWarning("No user is currently signed in.");
            onTokenReceived?.Invoke(null);
            return;
        }

        CurrentUser.TokenAsync(false).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                string token = task.Result;
                if (debugLogging) Debug.Log($"Firebase ID Token: {token}");
                onTokenReceived?.Invoke(token);
            }
            else
            {
                Debug.LogWarning("Failed to get Firebase ID Token: " + task.Exception);
                onTokenReceived?.Invoke(null);
            }
        });
    }


    // ================== GOOGLE SIGN-IN BUTTON (call from UI) ==================
    public void SignInWithGoogleButton()
    {
        StartCoroutine(GoogleSignInFlow());
    }

    IEnumerator GoogleSignInFlow()
    {
        string redirectUri = $"http://localhost:{googleOAuthPort}/";
        string scope = "openid email profile";
        string authUrl =
            "https://accounts.google.com/o/oauth2/v2/auth" +
            "?response_type=code" +
            $"&client_id={googleClientId}" +
            $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
            $"&scope={Uri.EscapeDataString(scope)}" +
            "&access_type=offline" +
            "&prompt=select_account";
        Debug.Log("[GoogleSignIn] Browser URL: " + authUrl);
        // Start listening for redirect before opening the browser!
        string authCode = null;
        bool done = false;
        Exception listenException = null;

        // Start HTTP listener coroutine
        StartCoroutine(ListenForAuthCode(googleOAuthPort, code =>
        {
            authCode = code;
            done = true;
        }, ex =>
        {
            listenException = ex;
            done = true;
        }));

        // Launch browser
        Application.OpenURL(authUrl);

        // Wait for code or error
        while (!done)
            yield return null;

        if (listenException != null || string.IsNullOrEmpty(authCode))
        {
            Debug.LogWarning("Google OAuth failed: " + listenException);
            yield break;
        }
        if (debugLogging)
        {
            Debug.Log("[Google OAuth] Received code, exchanging for tokens...");
        }

        // Exchange code for tokens
        yield return StartCoroutine(ExchangeCodeForToken(authCode, googleClientId, googleClientSec, redirectUri, (idToken, accessToken) =>
        {
            if (!string.IsNullOrEmpty(idToken) && !string.IsNullOrEmpty(accessToken))
            {
                if (debugLogging) Debug.Log("[Google OAuth] Token exchange successful!");
                // Sign in to Firebase
                SignInToFirebaseWithGoogle(idToken, accessToken);
            }
            else
            {
                Debug.LogWarning("[Google OAuth] Failed to exchange code for tokens.");
            }
        }));
    }

    // ========== LISTEN FOR OAUTH CODE ==========
    IEnumerator ListenForAuthCode(int port, Action<string> onCode, Action<Exception> onError)
    {
        HttpListener listener = new HttpListener();
        string prefix = $"http://localhost:{port}/";
        listener.Prefixes.Add(prefix);
        try
        {
            listener.Start();
            if (debugLogging) Debug.Log("[Google OAuth] Listening on " + prefix);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Failed to start HttpListener: " + ex);
            onError?.Invoke(ex);
            yield break;
        }

        var contextAsync = listener.GetContextAsync();
        while (!contextAsync.IsCompleted)
            yield return null;

        try
        {
            var context = contextAsync.GetAwaiter().GetResult();
            string code = context.Request.QueryString["code"];
            string error = context.Request.QueryString["error"];

            string responseString = "<html><body>You may now close this window and return to the app.</body></html>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
            listener.Stop();

            if (!string.IsNullOrEmpty(error))
            {
                onError?.Invoke(new Exception("OAuth error: " + error));
            }
            else
            {
                onCode?.Invoke(code);
            }
        }
        catch (Exception ex)
        {
            onError?.Invoke(ex);
        }
    }

    // ========== EXCHANGE AUTH CODE FOR TOKENS ==========
    IEnumerator ExchangeCodeForToken(string code, string clientId, string clientSecret, string redirectUri, Action<string, string> onTokens)
    {
        WWWForm form = new WWWForm();
        form.AddField("code", code);
        form.AddField("client_id", clientId);
        form.AddField("client_secret", clientSecret);
        form.AddField("redirect_uri", redirectUri);
        form.AddField("grant_type", "authorization_code");

        using (UnityWebRequest www = UnityWebRequest.Post("https://oauth2.googleapis.com/token", form))
        {
#if UNITY_2020_2_OR_NEWER
            yield return www.SendWebRequest();
#else
            yield return www.Send();
#endif
            if (www.result == UnityWebRequest.Result.Success)
            {
                var json = www.downloadHandler.text;
                var data = JsonUtility.FromJson<GoogleTokenResponse>(json);
                onTokens?.Invoke(data.id_token, data.access_token);
            }
            else
            {
                Debug.LogWarning("Token exchange failed: " + www.error);
                onTokens?.Invoke(null, null);
            }
        }
    }

    [Serializable]
    public class GoogleTokenResponse
    {
        public string id_token;
        public string access_token;
    }

    // ========== SIGN IN TO FIREBASE ==========
    void SignInToFirebaseWithGoogle(string idToken, string accessToken)
    {
        var credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogWarning("Firebase Google sign-in failed: " + task.Exception);
                return;
            }
            CurrentUser = task.Result;
            Debug.Log($"Google sign-in successful! User: {CurrentUser.Email}");
        });
    }

    // ========== (OPTIONAL: Add your Email/Password and other methods below as needed) ==========
    // ...

}
