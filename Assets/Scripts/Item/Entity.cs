using UnityEngine;
using sgffu.ObjectPool;
using ItemID = Item.ID;
using MessagePack;

namespace Item
{
    [MessagePackObject]
    public class Entity
    {
        [Key(0)]
        public int id = int.MinValue;

        [Key(1)]
        public ItemID item_id;

        [IgnoreMember]
        public GameObject game_object;

        //public string prefab_path;

        [Key(2)]
        public int prefab_id = int.MinValue;

        [Key(3)]
        public string name = "";

    }
}