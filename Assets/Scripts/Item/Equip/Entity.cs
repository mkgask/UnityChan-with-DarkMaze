using UnityEngine;
using ItemEntity = Item.Entity;
using Rank = GameStageRank.Rank;
using MessagePack;

namespace Item.Equip
{
    [MessagePackObject]
    public class EquipEntity : ItemEntity
    {
        [Key(4)]
        public string main_body_gameobject_name = "";

        [Key(5)]
        public Vector3 equip_position_offset = new Vector3(0f, 0f, 0f);

        [Key(6)]
        public Quaternion equip_rotation_offset = new Quaternion(0f, 0f, 0f, 0f);

        [Key(7)]
        public float attack_speed_adjust = 1f;

        [Key(8)]
        public Type type = Type.Sword;

        [Key(9)]
        public Rank rank = Rank.D;

        [Key(10)]
        public int atk = 0;

        [Key(11)]
        public int def = 0;

        [Key(12)]
        public int spd = 0;

        [Key(13)]
        public int atk_op = 0;

        [Key(14)]
        public int def_op = 0;

        [Key(15)]
        public int spd_op = 0;

        [IgnoreMember]
        public int attack {
            get { return atk + atk_op; }
            private set { }
        }

        [IgnoreMember]
        public int defence {
            get { return def + def_op; }
            private set { }
        }

        [IgnoreMember]
        public int speed {
            get { return spd + spd_op; }
            private set { }
        }

        public EquipEntity clone()
        {
            return (EquipEntity)MemberwiseClone();
        }

    }
}