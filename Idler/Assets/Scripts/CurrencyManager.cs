using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public delegate void OnCurrencyChanged(int newAmount);
    public event OnCurrencyChanged CurrencyChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int GetTotalGold()
    {
        return Inventory.Instance.GetItemCount("Gold");
    }

    public void AddCurrency(int amount)
    {
        Inventory.Instance.AddItem("Gold", amount);
    }

    public bool SpendCurrency(int amount)
    {
        if (Inventory.Instance.HasItem("Gold", amount))
        {
            Inventory.Instance.RemoveItem("Gold", amount);
            return true;
        }

        Debug.LogWarning($"Not enough gold! Required: {amount}, Available: {GetTotalGold()}");
        return false;
    }

    public void UpdateCurrencyDisplay()
    {
        CurrencyChanged?.Invoke(GetTotalGold());
    }
}
