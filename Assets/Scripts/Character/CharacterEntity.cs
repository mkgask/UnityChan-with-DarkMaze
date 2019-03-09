using EquipEntity = Item.Equip.EquipEntity;
using MessagePack;

namespace sgffu.Characters
{
    [MessagePackObject]
    public class CharacterEntity
    {
        [Key(0)]
        public int lv = 1;

        [Key(1)]
        public int exp = 0;

        [Key(2)]
        public int hp = 10;

        [Key(3)]
        public int hp_max = 10;

        [Key(4)]
        public int atk = 1;

        [Key(5)]
        public int def = 1;

        [Key(6)]
        public int spd = 1;

        [Key(7)]
        public EquipEntity equip;
    }
}