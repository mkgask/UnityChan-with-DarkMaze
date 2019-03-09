using InventoryEntity = Item.Inventory.Entity;
using IInventory = Item.Inventory.IInventory;


namespace Item {
    public class Factory
    {
        public static IInventory createInventory(int max_size)
        {
            return new InventoryEntity(max_size);
        }
    }
}