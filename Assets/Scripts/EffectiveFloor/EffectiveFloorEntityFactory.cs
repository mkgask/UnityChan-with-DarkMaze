using UnityEngine;
using sgffu.FieldObject;

namespace sgffu.EffectiveFloor
{
    public class EffectiveFloorEntityFactory
    {
        public static EffectiveFloorEntity create(
                Vector3? position = null,
                /*
                float pos_x = 0,
                float pos_z = 0,
                */
                Vector3? rotation = null,
                int effective_size = 0,
                FieldObjectEntity entity = null)
        {
            return new EffectiveFloorEntity {
                position = position ?? new Vector3(float.MinValue, float.MinValue, float.MinValue),
                /*
                pos_x = pos_x,
                pos_z = pos_z,
                */
                effective_size = effective_size,
                rotation = rotation ?? Vector3.zero,
                entity = entity,
            };
        }
    }
}
