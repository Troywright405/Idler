using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System;
using Unity.VisualScripting;

[System.Serializable]
public class UrlResponse
{
    public string url;
}

[System.Serializable]
public class MessageResponse
{
    public string message;
}

[System.Serializable]
public class RegistryResponse //Based on each http method used here
{
    public string api_base_url;
    public string upload_path;
    public string download_path;
    public string delete_path;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private string fileName = "master-save.dat";
    private string filePath;

    private string uploadUrl = "";
    private string downloadUrl = "";
    private string deleteUrl = "";

    public event System.Action<string, bool> OnStatusUpdate;
    public event System.Action<string, bool> OnEndpointResolved;
    public event System.Action<string, bool> OnError;
    public string ResolvedApiBaseUrl { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Optional: prevent duplicates
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        StartCoroutine(FetchApiRegistry("https://unity-api-registry.s3.us-east-1.amazonaws.com/default/config.json"));//could be moved to a local config file
    }

    private IEnumerator FetchApiRegistry(string registryUrl)
    {
        UnityWebRequest request = UnityWebRequest.Get(registryUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            RegistryResponse registry = JsonUtility.FromJson<RegistryResponse>(request.downloadHandler.text);
            uploadUrl = registry.api_base_url + registry.upload_path;
            downloadUrl = registry.api_base_url + registry.download_path;
            deleteUrl = registry.api_base_url + registry.delete_path;
            ResolvedApiBaseUrl = registry.api_base_url; //Stored here in case the SaveLoad window isn't active for direct invoke when a connect is made
            OnEndpointResolved?.Invoke(registry.api_base_url, false);//Try anyway, if a reconnect is on timer and the saveload window is open, this will work
        }
        else
        {
            Debug.LogWarning("Failed to load registry: " + request.error);
        }
    }
    public void SaveAndUpload()
    {
        SaveGame();
        UploadSaveFile();
    }

    // Use this new method to upload with real Firebase token
    public void UploadSaveFile()
    {
        StartCoroutine(UploadWithFirebaseAuthCoroutine());
    }

    private IEnumerator UploadWithFirebaseAuthCoroutine()
    {
        bool gotToken = false;
        string token = null;
        FirebaseManager.Instance.GetIdTokenForCurrentUser(t =>
        {
            token = t;
            gotToken = true;
        });
        while (!gotToken) yield return null;

        if (string.IsNullOrEmpty(token))
        {
            Debug.LogWarning("No Firebase ID token, cannot upload.");
            yield break;
        }

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("No file to upload.");
            yield break;
        }
        Debug.Log("ID Token: " + token);

        byte[] fileData = null;
        try
        {
            fileData = File.ReadAllBytes(filePath);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Could not read file: " + ex);
            RaiseStatusUpdate("Could not read local save file: " + ex.Message, true);
            yield break;
        }

        UploadHandlerRaw handler = new UploadHandlerRaw(fileData);
        handler.contentType = "application/octet-stream";
        Debug.Log("[UPLOAD] Attempting upload to: " + uploadUrl);

        UnityWebRequest request = new UnityWebRequest(uploadUrl, "POST");
        request.uploadHandler = handler;
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.SetRequestHeader("Content-Type", "application/octet-stream");
        request.timeout = 15;

        yield return request.SendWebRequest();

        // Robust API/malformed endpoint check
        if (request.result != UnityWebRequest.Result.Success)
        {
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                string.IsNullOrEmpty(request.downloadHandler.text) ||
                request.error.ToLower().Contains("resolve host") ||
                request.error.ToLower().Contains("not found") ||
                request.responseCode == 404)
            {
                Debug.LogWarning("Cloud error: Could not contact API Gateway or endpoint missing. Error: " + request.error);
                RaiseStatusUpdate("Cloud error: API endpoint unreachable or not deployed.", true);
            }
            else
            {
                Debug.LogWarning("Upload failed: " + request.error + "\n" + request.downloadHandler.text);
            }
            handler.Dispose();
            request.Dispose();
            yield break;
        }

        // Handle malformed or missing JSON response
        MessageResponse resp = null;
        try
        {
            resp = JsonUtility.FromJson<MessageResponse>(request.downloadHandler.text);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Malformed upload server response: " + request.downloadHandler.text + " (" + ex.Message + ")");
            RaiseStatusUpdate("Cloud error: Upload response was malformed. (Server/bucket misconfigured?)", true);
            handler.Dispose();
            request.Dispose();
            yield break;
        }
        if (resp == null || string.IsNullOrEmpty(resp.message))
        {
            Debug.LogWarning("Malformed or empty upload server response: " + request.downloadHandler.text);
            RaiseStatusUpdate("Cloud error: No upload confirmation from server (server/bucket misconfigured?)", true);
            handler.Dispose();
            request.Dispose();
            yield break;
        }
        RaiseStatusUpdate("Upload complete: " + resp.message, true);

        handler.Dispose();
        request.Dispose();
    }


    public void DownloadSaveFile()
    {
        StartCoroutine(DownloadWithFirebaseAuthCoroutine());
    }

    private IEnumerator DownloadWithFirebaseAuthCoroutine()
    {
        bool gotToken = false;
        string token = null;
        FirebaseManager.Instance.GetIdTokenForCurrentUser(t =>
        {
            token = t;
            gotToken = true;
        });
        while (!gotToken) yield return null;

        if (string.IsNullOrEmpty(token))
        {
            Debug.LogWarning("No Firebase ID token, cannot download.");
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get(downloadUrl);
        request.SetRequestHeader("Authorization", "Bearer " + token);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Failed to get download URL: " + request.error);
            yield break;
        }
        UrlResponse response = JsonUtility.FromJson<UrlResponse>(request.downloadHandler.text);
        string presignedUrl = response.url; // Get the config json from the 'server'

        if (string.IsNullOrEmpty(presignedUrl) ||
            !presignedUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            RaiseStatusUpdate("Cloud error: Failed to generate a valid download URL (server/bucket misconfigured or offline?)", true);
            yield break;
        }

        UnityWebRequest fileRequest = UnityWebRequest.Get(presignedUrl);
        yield return fileRequest.SendWebRequest();
        if (fileRequest.result == UnityWebRequest.Result.Success)
        {
            File.WriteAllBytes(filePath, fileRequest.downloadHandler.data);
            RaiseStatusUpdate("Downloaded and saved file to: " + filePath, true);
        }
        else
        {
            Debug.LogWarning("File download from S3 failed: " + fileRequest.error);
        }
        LoadGame();
    }

    public void DeleteSaveFile()
    {
        DeleteLocalSave();
        StartCoroutine(DeleteWithFirebaseAuthCoroutine());
    }

    private IEnumerator DeleteWithFirebaseAuthCoroutine()
    {
        bool gotToken = false;
        string token = null;
        FirebaseManager.Instance.GetIdTokenForCurrentUser(t =>
        {
            token = t;
            gotToken = true;
        });
        while (!gotToken) yield return null;

        if (string.IsNullOrEmpty(token))
        {
            Debug.LogWarning("No Firebase ID token, cannot delete.");
            yield break;
        }

        if (string.IsNullOrEmpty(deleteUrl))
        {
            Debug.LogWarning("Delete URL not set from registry.");
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Delete(deleteUrl);
        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string body = request.downloadHandler.text;
            Debug.Log($"[DELETE] Response body: {body}");

            if (!string.IsNullOrWhiteSpace(body))
            {
                MessageResponse response = JsonUtility.FromJson<MessageResponse>(body);
                RaiseStatusUpdate(response.message, true);
            }
            else
            {
                RaiseStatusUpdate("Delete succeeded, but response body was empty.", true);
            }
        }
        else
        {
            RaiseStatusUpdate("Delete from cloud failed: " + request.error, true);
        }
    }

    public void SaveGame()
    {
        string data = "This is a test save at " + System.DateTime.Now;
        try
        {
            File.WriteAllText(filePath, data);
            RaiseStatusUpdate("Game saved: " + filePath);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Failed to write save file: " + ex);
        }
    }

    public void LoadGame()
    {
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            RaiseStatusUpdate("Game loaded: " + filePath);
        }
        else
        {
            Debug.LogWarning("Save file not found: " + filePath);
        }
    }

    private void DeleteLocalSave()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            RaiseStatusUpdate("Local file deleted: " + filePath);
        }
        else
        {
            Debug.Log("Failed to delete: Local file not found");
        }
    }

    public void ShowLocalSaveInfo()
    {
        if (File.Exists(filePath))
        {
            FileInfo fileInfo = new FileInfo(filePath);
            RaiseStatusUpdate($"Save found:\n{filePath}\nSize: {fileInfo.Length / 1024f:F2} KB\nLast Modified: {fileInfo.LastWriteTime}");
        }
        else
        {
            RaiseStatusUpdate("No local save found.");
        }
    }

    public void RaiseStatusUpdate(string message, bool append = false)//Wrapper method for the default bool value, because system.action doesn't support defaults directly
    {
        Debug.Log($"{message}");
        OnStatusUpdate?.Invoke(message, append);
    }
}
