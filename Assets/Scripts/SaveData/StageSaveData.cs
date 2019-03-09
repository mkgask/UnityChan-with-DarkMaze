using MessagePack;

namespace SaveData
{
    [MessagePackObject]
    public class StageSaveData
    {
        [Key(0)]
        public int room_clear_count = 0;
        [Key(1)]
        public int enemy_strength = 1;
        [Key(2)]
        public int enemy_num = 1;
    }
}