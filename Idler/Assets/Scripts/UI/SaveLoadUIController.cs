// SaveLoadUIController.cs
using UnityEngine;
using TMPro;

public class SaveLoadUIController : MonoBehaviour
{
    private GameObject PanelSaveLoad;

    public TMP_Text statusText; // Display status updates (e.g., save/load success)
    public TMP_Text endpointText; // Display the resolved API endpoint

    private void Awake()
{
    var canvas = GameObject.Find("Canvas");
    if (canvas != null)
    {
        PanelSaveLoad = canvas.transform.Find("PanelSaveLoad")?.gameObject;
    }

    if (PanelSaveLoad == null)
    {
        Debug.LogError("PanelSaveLoad not found — check hierarchy and spelling.");
    }
    else
    {
        statusText = PanelSaveLoad.transform.Find("TextSaveLoadFileDetails")?.GetComponent<TMP_Text>();
        endpointText = PanelSaveLoad.transform.Find("TextCloudEndpoint")?.GetComponent<TMP_Text>();
    }

    if (SaveManager.Instance != null)
    {
        SaveManager.Instance.OnStatusUpdate += UpdateStatus;
        SaveManager.Instance.OnEndpointResolved += UpdateEndpoint;
    }

    if (statusText != null) statusText.text = "";
    if (endpointText != null) endpointText.text = "";
}

    private void Start()
    {
        if (!string.IsNullOrEmpty(SaveManager.Instance.ResolvedApiBaseUrl))
        {
            HandleEndpointResolved(SaveManager.Instance.ResolvedApiBaseUrl);
        }
    }
    private void HandleEndpointResolved(string url)
    {
        UpdateEndpoint(url);
    }

    private void OnDestroy()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.OnStatusUpdate -= UpdateStatus;
            SaveManager.Instance.OnEndpointResolved -= UpdateEndpoint;
        }
    }

    public void ToggleSaveLoadPopup()
    {
        if (PanelSaveLoad != null)
        {
            PanelSaveLoad.SetActive(!PanelSaveLoad.activeSelf);
        }
    }

    public void CloseSaveLoadPopup()
    {
        if (PanelSaveLoad != null)
        {
            PanelSaveLoad.SetActive(false);
        }
    }
    private void UpdateStatus(string message, bool append = false)
    {
        if (statusText != null)
        {
            if (append && !string.IsNullOrEmpty(statusText.text))
                statusText.text += "\n" + message;
            else
                statusText.text = message;
        }
    }

    private void UpdateEndpoint(string message, bool append = false)
    {
        if (endpointText != null)
            endpointText.text = message;
    }

} 
