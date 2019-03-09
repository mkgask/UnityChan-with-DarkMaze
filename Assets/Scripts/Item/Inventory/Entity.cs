using System.Collections.Generic;
using ItemEntity = Item.Entity;

namespace Item.Inventory
{
    public class Entity : IInventory
    {
        private List<ItemEntity> entities = new List<ItemEntity>();

        public int max {
            get;
            private set;
        }

        public Entity(int max_num) {
            max = max_num;
        }

        public ItemEntity takeOut(int idx = 0) {
            if (max < 1 || entities.Count < 1) { return null; }
            return entities[idx];
        }

        public bool putIn(ItemEntity entity) {
            if (max <= entities.Count) { return false; }
            entities.Add(entity);
            return true;
        }
    }
}