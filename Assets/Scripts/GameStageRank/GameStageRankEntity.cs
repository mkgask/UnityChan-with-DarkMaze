using Random = System.Random;

namespace GameStageRank
{

    class GameStageRankEntity
    {
        public const int rank_list_length = 10;

        public static Rank[,] rank_list = new Rank[,] {
            { Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.D },
            { Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.C },
            { Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.C, Rank.C },
            { Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.C, Rank.C, Rank.C },
            { Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.C, Rank.C, Rank.C, Rank.C },
            { Rank.D, Rank.D, Rank.D, Rank.D, Rank.D, Rank.C, Rank.C, Rank.C, Rank.C, Rank.C },
            { Rank.D, Rank.D, Rank.D, Rank.D, Rank.C, Rank.C, Rank.C, Rank.C, Rank.C, Rank.B },
            { Rank.D, Rank.D, Rank.D, Rank.C, Rank.C, Rank.C, Rank.C, Rank.C, Rank.B, Rank.B },
            { Rank.D, Rank.D, Rank.C, Rank.C, Rank.C, Rank.C, Rank.C, Rank.B, Rank.B, Rank.B },
            { Rank.D, Rank.C, Rank.C, Rank.C, Rank.C, Rank.C, Rank.B, Rank.B, Rank.B, Rank.B },
            { Rank.C, Rank.C, Rank.C, Rank.C, Rank.B, Rank.B, Rank.B, Rank.B, Rank.B, Rank.A },
            { Rank.C, Rank.C, Rank.C, Rank.B, Rank.B, Rank.B, Rank.B, Rank.B, Rank.A, Rank.A },
            { Rank.C, Rank.C, Rank.B, Rank.B, Rank.B, Rank.B, Rank.B, Rank.A, Rank.A, Rank.A },
            { Rank.C, Rank.B, Rank.B, Rank.B, Rank.B, Rank.B, Rank.A, Rank.A, Rank.A, Rank.A },
            { Rank.B, Rank.B, Rank.B, Rank.B, Rank.A, Rank.A, Rank.A, Rank.A, Rank.A, Rank.S },
            { Rank.B, Rank.B, Rank.B, Rank.A, Rank.A, Rank.A, Rank.A, Rank.A, Rank.S, Rank.S },
            { Rank.B, Rank.B, Rank.A, Rank.A, Rank.A, Rank.A, Rank.A, Rank.S, Rank.S, Rank.S },
            { Rank.B, Rank.A, Rank.A, Rank.A, Rank.A, Rank.A, Rank.S, Rank.S, Rank.S, Rank.S },
        };

        public Rank rand(int rank) {
            if (rank < 0) { rank = 0; }
            else if (rank_list.GetLength(0) < rank) { rank = rank_list.GetLength(0); }
            return rank_list[rank, (new Random()).Next(rank_list.GetLength(1))];
        }
    }

}

