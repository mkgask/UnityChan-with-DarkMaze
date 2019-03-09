using System.Collections.Generic;
using UnityEngine;
using Rank = GameStageRank.Rank;

namespace sgffu.Bgm
{


    public class Factory
    {
        
        //private static List<GameObject> bgm_prefabs;

        //private static AudioClip[] bgm_list;

        private static Dictionary<Rank, Entity> base_resources;

        private static Entity title_bgm;

        //public static void init(List<GameObject> bgmPrefabs, AudioClip[] bgm_list)
        public static void init(Dictionary<string, AudioClip> bgm_list)
        {
            //bgm_prefabs = bgmPrefabs;

            title_bgm = new Entity {
                name = "The Green Fields",
                credit = "Medieval Fantasy Audio Bundle",
                prefab_id = 0,
                clip = bgm_list["Title_The Green Fields"],
            };

            var d_bgm = new Entity {
                name = "A Knights Tale",
                credit = "Medieval Fantasy Audio Bundle",
                prefab_id = 0,
                clip = bgm_list["D_A Knights Tale"],
            };

            var c_bgm = new Entity {
                name = "A Night and A Cave",
                credit = "Medieval Fantasy Audio Bundle",
                prefab_id = 1,
                clip = bgm_list["C_A Night and A Cave"],
            };

            var b_bgm = new Entity {
                name = "Monsters and Beasts",
                credit = "Medieval Fantasy Audio Bundle",
                prefab_id = 2,
                clip = bgm_list["B_Monsters and Beasts"],
            };

            var a_bgm = new Entity {
                name = "Path of Heroes",
                credit = "Medieval Fantasy Audio Bundle",
                prefab_id = 3,
                clip = bgm_list["A_Path of Heroes"],
            };

            var s_bgm = new Entity {
                name = "Bloody Battlefield",
                credit = "S_Bloody Battlefield",
                prefab_id = 4,
                clip = bgm_list["S_Bloody Battlefield"],
            };

            base_resources = new Dictionary<Rank, Entity> {
                { Rank.D, d_bgm},
                { Rank.C, c_bgm},
                { Rank.B, b_bgm},
                { Rank.A, a_bgm},
                { Rank.S, s_bgm},
            };

        }


        public static Entity get(Rank rank)
        {
            if (!base_resources.ContainsKey(rank)) {
                throw new KeyNotFoundException();
            }

            return base_resources[rank];
        }

        public static Entity getTitleBgm()
        {
            return title_bgm;
        }

    }

}