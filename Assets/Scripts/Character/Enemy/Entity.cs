using UnityEngine;
using CharacterEntity = sgffu.Characters.CharacterEntity;
using EnemyType = sgffu.Characters.Enemy.Type;
using Rank = GameStageRank.Rank;

namespace sgffu.Characters.Enemy
{
    public class Entity : CharacterEntity
    {
        public int id = 0;

        public GameObject game_object = null;

        public string name = "";

        public Rank rank = Rank.D;

        public EnemyType type = EnemyType.None;

        //public string prefab_path = "";
        public int prefab_id = int.MinValue;

        public float attack_distance = 1f;

        public string attack_voice;

        public string damage_voice;

        public string dead_voice;

        public Entity clone()
        {
            return (Entity)MemberwiseClone();
        }

    }
}