using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private string fileName = "save_test.json";
    private string filePath;

    // Replace these with actual API endpoints once I figure them out
    private string uploadUrl = "https://abc123xyz.execute-api.us-east-1.amazonaws.com/upload"; //POST
    private string downloadUrl = "https://abc123xyz.execute-api.us-east-1.amazonaws.com/download"; //GET

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);
    }

    public void SaveGame()
    {
        string data = "This is a test save at " + System.DateTime.Now; //THIS NEEDS TO ULTIMATELY BE THE SAVE DATA
        File.WriteAllText(filePath, data);
        Debug.Log("Game saved: " + data);
    }

    public void LoadGame()
    {
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath); //THIS NEEDS TO ULTIMATELY BE THE SAVE DATA
            Debug.Log("Game loaded: " + data);
        }
        else
        {
            Debug.LogWarning("Save file not found: " + filePath);
        }
    }

    public void UploadSaveFile()
    {
        if (File.Exists(filePath))
            StartCoroutine(UploadCoroutine());
        else
            Debug.LogWarning("No file to upload.");
    }

    private IEnumerator UploadCoroutine()
    {
        byte[] fileData = File.ReadAllBytes(filePath);

        UnityWebRequest request = UnityWebRequest.Put(uploadUrl, fileData);
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.SetRequestHeader("Content-Type", "application/octet-stream");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Upload successful.");
        }
        else
        {
            Debug.LogError("Upload failed: " + request.error);
        }
    }

    public void DownloadSaveFile()
    {
        StartCoroutine(DownloadCoroutine());
    }

    private IEnumerator DownloadCoroutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(downloadUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            File.WriteAllBytes(filePath, request.downloadHandler.data);
            Debug.Log("Downloaded and saved file to: " + filePath);
        }
        else
        {
            Debug.LogError("Download failed: " + request.error);
        }
    }
}
