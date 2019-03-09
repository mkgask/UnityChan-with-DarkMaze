using System;

namespace sgffu.ObjectPool
{

    public enum ObjectID
    {
        None,

        // Item
        D_Sword,

        // Monster

        // Props

        TreasureBox,

        Torch,

    }

    public static partial class ObjectIdExtentions {
        public static int Length(this ObjectID param) {
            return 3;
        }
    }

}