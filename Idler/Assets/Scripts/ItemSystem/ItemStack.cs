[System.Serializable]
public class ItemStack
{
    public Item item;
    public int quantity;

    public ItemStack(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public void Use()
    {
        if (item.itemFlags.HasFlag(ItemFlag.Consumable) && quantity > 0)
        {
            item.UseItem(); // Still triggers effect
            quantity--;
        }
    }
}
