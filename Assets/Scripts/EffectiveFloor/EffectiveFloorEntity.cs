using UnityEngine;
using sgffu.FieldObject;

namespace sgffu.EffectiveFloor
{
    public class EffectiveFloorEntity
    {
        public Vector3 position = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        /*
        public float pos_x = 0;
        public float pos_z = 0;
        */
        public Vector3 rotation = new Vector3(0f, 0f, 0f);
        
        public bool enabled = true;

        public int effective_size = 0;

        public FieldObjectEntity entity = null;
    }
}
