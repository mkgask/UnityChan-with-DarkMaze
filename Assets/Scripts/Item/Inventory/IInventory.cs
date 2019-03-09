using ItemEntity = Item.Entity;

namespace Item.Inventory
{
    public interface IInventory {

        ItemEntity takeOut(int idx = 0);

        bool putIn(ItemEntity entity);
    }
}