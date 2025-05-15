using UnityEngine;

public class SaveLoadUIController : MonoBehaviour
{
    private GameObject PanelSaveLoad;

    private void Awake()
    {
        var canvas = GameObject.Find("Canvas"); // using this method because the gameobject starts inactive by default
        if (canvas != null)
        {
            PanelSaveLoad = canvas.transform.Find("PanelSaveLoad")?.gameObject;
        }

        if (PanelSaveLoad == null)
        {
            Debug.LogError("PanelSaveLoad not found — check hierarchy and spelling.");
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
}
