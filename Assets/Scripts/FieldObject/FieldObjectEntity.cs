using UnityEngine;
using sgffu.ObjectPool;
using FieldObjectID = sgffu.FieldObject.ID;

namespace sgffu.FieldObject
{
    public class FieldObjectEntity : IPoolObject
    {
        public int id = int.MinValue;

        public FieldObjectID type = FieldObjectID.None;
        /*
        public float pos_x;
        public float pos_z;
        */
        public Vector3 position = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        public Vector3 rotation = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        public int size = int.MaxValue;

        public GameObject game_object = null;

        //public string prefab_path = "";

        public int prefab_id = int.MinValue;

        public int inventory_size = 0;

        public void init()
        {
            
        }

        public void destroy()
        {
            GameObject.Destroy(game_object);
        }

        public FieldObjectEntity clone()
        {
            return (FieldObjectEntity)MemberwiseClone();
        }

    }
}