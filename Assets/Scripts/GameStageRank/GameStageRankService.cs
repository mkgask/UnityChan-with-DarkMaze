using System.Collections.Generic;
using UnityEngine;
using PlayerService = sgffu.Characters.Player.PlayerService;
using EnemyService = sgffu.Characters.Enemy.Service;
using GameStageService = GameStage.Service;
using Rank = GameStageRank.Rank;

namespace GameStageRank
{
    public class GameStageRankService
    {
        private static GameStageRankEntity entity;

/*
        public static int rank_min = 1;
        public static int rank_max = 20;
*/

        public static void init()
        {
            entity = GameStageRankEntityFactory.create();
        }



        public static float item_rank_scale = 0.1f;

        public static Rank item()
        {
/*
            if (entity != null) {
                Debug.Log("item():30 entity enable");
            } else {
                Debug.Log("item():32 entity disable");
            }
*/
            return entity.rand(Mathf.FloorToInt(GameStageService.status("clear_count") * item_rank_scale));
        }



        public static float enemy_rank_scale = 0.1f;

        public static Rank enemy()
        {
            return entity.rand(Mathf.FloorToInt(GameStageService.status("enemy_strength") * enemy_rank_scale));
        }



        public static float props_rank_scale = 0.1f;

        public static Rank props()
        {
            return entity.rand(Mathf.FloorToInt(PlayerService.lv() * props_rank_scale));
        }



        private static float bgm_rank_scale = 0.05f;

        public static Rank bgm()
        {
            var bgm_rank = Mathf.FloorToInt(GameStageService.status("clear_count") * bgm_rank_scale);
            //var bgm_rank = GameStageService.room_clear_count;

            switch (bgm_rank) {
                case 0: return Rank.D;
                case 1: return Rank.C;
                case 2: return Rank.B;
                case 3: return Rank.A;
                default: return Rank.S;
            }
        }
    }

}

