using UnityEngine;
using EffectType = Effect.Type;

namespace Effect
{
    class Entity
    {
        public int id = 0;

        public EffectType type = EffectType.None;

        public GameObject game_object = null;

        public string name = "";

        //public string prefab_path = "";

        public int prefab_id = int.MinValue;

        public Entity clone()
        {
            return (Entity)MemberwiseClone();
        }
    }
}