using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using File = Filesystem.File;
using Rank = GameStageRank.Rank;
using GameStageService = GameStage.Service;
using GameStageRankService = GameStageRank.GameStageRankService;
using EnemyEntity = sgffu.Characters.Enemy.Entity;
using EnemyType = sgffu.Characters.Enemy.Type;

namespace sgffu.Characters.Enemy
{
    public class Factory
    {
        public static int entity_id = 0;

        private static Dictionary<Rank, Dictionary<EnemyType, EnemyEntity>> base_resources;

        private static Dictionary<Rank, Tuple<int, int>> base_random;

        private static List<GameObject> enemy_prefabs;

        private static float strength_scale = 0.9f;

        private static float exp_scale = 0.2f;

        private static float hp_scale = 0.333f;

        public static void init(List<GameObject> enemyPrefabs)
        {
            enemy_prefabs = enemyPrefabs;

            var d_bat = new EnemyEntity {
                name = "バット",
                type = EnemyType.D_Bat,
                //prefab_path = "Character/Enemy/D_Bat/D_Bat",
                prefab_id = 0,
                attack_voice = "Monster 3",
                damage_voice = "Beast Hit 3",
                dead_voice = "Beast Death 1",
            };

            var d_rabbit = new EnemyEntity {
                name = "ラビット",
                type = EnemyType.D_Rabbit,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 1,
                attack_voice = "Monster 1",
                damage_voice = "Beast Hit 1",
                dead_voice = "Beast Death 2",
            };

            var d_ghost = new EnemyEntity {
                name = "ゴースト",
                type = EnemyType.D_Ghost,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 2,
                attack_voice = "Monster 2",
                damage_voice = "Beast Hit 2",
                dead_voice = "Beast Death 3",
            };

            var d_rhino = new EnemyEntity {
                name = "リノ",
                type = EnemyType.D_Rhino,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 3,
                attack_distance = 2f,
                attack_voice = "Beast 4",
                damage_voice = "Beast Hit 4",
                dead_voice = "Beast Death 3",
            };

            var d_reptile = new EnemyEntity {
                name = "レプタイル",
                type = EnemyType.D_Reptile,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 4,
                attack_voice = "Beast 2",
                damage_voice = "Beast Hit 3",
                dead_voice = "Beast Death 2",
            };

            var c_wolf = new EnemyEntity {
                name = "ウルフ",
                type = EnemyType.C_Wolf,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 5,
                attack_distance = 1.5f,
                attack_voice = "Beast 3",
                damage_voice = "Beast Hit 1",
                dead_voice = "Beast Death 1",
            };

            var c_slime = new EnemyEntity {
                name = "スライム",
                type = EnemyType.C_Slime,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 6,
                attack_voice = "Monster 4",
                damage_voice = "Beast Hit 4",
                dead_voice = "Beast Death 4",
            };

            var d_crab = new EnemyEntity {
                name = "シェルクラブ",
                type = EnemyType.D_Crab,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 7,
                attack_voice = "Monster 1",
                damage_voice = "Beast Hit 1",
                dead_voice = "Beast Death 2",
            };

            var green_dragon = new EnemyEntity {
                name = "グリーンドラゴン",
                type = EnemyType.GreenDragon,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 8,
                attack_voice = "Monster 5",
                damage_voice = "Beast Hit 1",
                dead_voice = "Beast Death 2",
            };

            var grey_dragon = new EnemyEntity {
                name = "グレイドラゴン",
                type = EnemyType.GreyDragon,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 9,
                attack_voice = "Monster 6",
                damage_voice = "Beast Hit 1",
                dead_voice = "Beast Death 3",
            };

            var red_dragon = new EnemyEntity {
                name = "レッドドラゴン",
                type = EnemyType.RedDragon,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 10,
                attack_voice = "Monster 5",
                damage_voice = "Beast Hit 1",
                dead_voice = "Beast Death 1",
            };

            var albino_dragon = new EnemyEntity {
                name = "アルビノドラゴン",
                type = EnemyType.AlbinoDragon,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 11,
                attack_voice = "Monster 6",
                damage_voice = "Beast Hit 1",
                dead_voice = "Beast Death 4",
            };

            var skelton = new EnemyEntity {
                name = "スケルトン",
                type = EnemyType.Skelton,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 12,
                attack_voice = "Monster 2",
                damage_voice = "Beast Hit 2",
                dead_voice = "Beast Death 2",
            };

            var goblin = new EnemyEntity {
                name = "ゴブリン",
                type = EnemyType.Goblin,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 13,
                attack_voice = "Monster 4",
                damage_voice = "Beast Hit 3",
                dead_voice = "Beast Death 4",
            };

            var hobgoblin = new EnemyEntity {
                name = "ホブゴブリン",
                type = EnemyType.Hobgoblin,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 14,
                attack_voice = "Monster 4",
                damage_voice = "Beast Hit 3",
                dead_voice = "Beast Death 4",
            };

            var troll = new EnemyEntity {
                name = "トロール",
                type = EnemyType.Troll,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 15,
                attack_voice = "Monster 3",
                damage_voice = "Beast Hit 4",
                dead_voice = "Beast Death 3",
            };

            var mushroom = new EnemyEntity {
                name = "マッシュルーム",
                type = EnemyType.Mushroom,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 16,
                attack_voice = "Monster 3",
                damage_voice = "Beast Hit 3",
                dead_voice = "Beast Death 1",
            };
/*
            var spider = new EnemyEntity {
                name = "スパイダー",
                type = EnemyType.Spider,
                //prefab_path = "Character/Enemy/D_Rabbit/D_Rabbit",
                prefab_id = 17,
                attack_voice = "Monster 1",
                damage_voice = "Beast Hit 1",
                dead_voice = "Beast Death 2",
            };
*/
            base_resources = new Dictionary<Rank, Dictionary<EnemyType, EnemyEntity>> {
                { Rank.D, new Dictionary<EnemyType, EnemyEntity> {
                    { EnemyType.D_Crab, d_crab },
                    { EnemyType.D_Bat, d_bat },
                    { EnemyType.D_Rabbit, d_rabbit },
                    { EnemyType.D_Ghost, d_ghost },
                }},

                { Rank.C, new Dictionary<EnemyType, EnemyEntity> {
                    { EnemyType.C_Slime, c_slime },
                    { EnemyType.C_Wolf, c_wolf },
                    { EnemyType.Mushroom, mushroom },
                    //{ EnemyType.Spider, spider },
                }},

                { Rank.B, new Dictionary<EnemyType, EnemyEntity> {
                    { EnemyType.Goblin, goblin },
                    { EnemyType.Hobgoblin, hobgoblin },
                    { EnemyType.D_Bat, d_bat },
                }},

                { Rank.A, new Dictionary<EnemyType, EnemyEntity> {
                    { EnemyType.D_Rhino, d_rhino },
                    { EnemyType.D_Reptile, d_reptile },
                    { EnemyType.Troll, troll },
                }},

                { Rank.S, new Dictionary<EnemyType, EnemyEntity> {
                    { EnemyType.GreenDragon, green_dragon },
                    { EnemyType.GreyDragon, grey_dragon },
                    { EnemyType.RedDragon, red_dragon },
                    { EnemyType.AlbinoDragon, albino_dragon },
                }},
            };

            base_random = new Dictionary<Rank, Tuple<int, int>> {
                { Rank.D, new Tuple<int, int>(0, 8)},
                { Rank.C, new Tuple<int, int>(8, 16)},
                { Rank.B, new Tuple<int, int>(16, 32)},
                { Rank.A, new Tuple<int, int>(32, 64)},
                { Rank.S, new Tuple<int, int>(64, 128)}
            };

        }

        public static void rankLog(Rank rank)
        {
            switch(rank) {
                case Rank.None: Debug.Log("Rank.None"); break;
                case Rank.D: Debug.Log("Rank.D"); break;
                case Rank.C: Debug.Log("Rank.C"); break;
                case Rank.B: Debug.Log("Rank.B"); break;
                case Rank.A: Debug.Log("Rank.A"); break;
                case Rank.S: Debug.Log("Rank.S"); break;
                default: Debug.Log("None"); break;
            }
        }

        public static EnemyEntity createRandomEntity()
        {
            var rank = GameStageRankService.enemy();
            //rankLog(rank);

            if (!base_resources.ContainsKey(rank)) {
                throw new KeyNotFoundException();
            }

            var resouces = base_resources[rank];
            var rand = resouces.RandomAt();

            //var entity = (Entity)ObjectPoolService.takeOut(sgffu.ObjectPool.Type.Item, rand.Key);
            //if (entity == null) { entity = createEntity(rank, rand.Key); }
            var entity = createEntity(rank, rand.Key);
            entity.rank = rank;

            entity.game_object = createGameObject(entity);
            entity.game_object.SetActive(false);

            EnemyController con = entity.game_object.GetComponent<EnemyController>();
            con.id = entity.id;
            con.entity = entity;

            return entity;
        }

        public static EnemyEntity createEntity(Rank rank, EnemyType id)
        {
            if (!base_resources.ContainsKey(rank) || !base_resources[rank].ContainsKey(id)) {
                throw new KeyNotFoundException();
            }

            var enemy_strength = GameStageService.status("enemy_strength");
            var entity = base_resources[rank][id].clone();
            entity.id = entity_id;
            entity_id += 1;

            var strength = Mathf.RoundToInt(enemy_strength * strength_scale);
            //var exp = Mathf.RoundToInt(GameStageService.status("clear_count") * exp_scale);
            var exp = Mathf.RoundToInt(enemy_strength * exp_scale);
            if (exp < 1) exp = 1;

            //Debug.Log("EnemyFactory : enemy_strength * exp_scale : " + (enemy_strength * exp_scale).ToString());
            //Debug.Log("EnemyFactory : exp : " + exp.ToString());

            entity.lv  = GameStageService.status("clear_count") + 1;
            entity.exp = exp;

            var hp = entity.hp + Mathf.RoundToInt(entity.lv * hp_scale);
            entity.hp  = hp;
            entity.hp_max = hp;

            entity.atk = Random.Range(base_random[rank].Item1, base_random[rank].Item2) + strength;
            entity.def = Random.Range(base_random[rank].Item1, base_random[rank].Item2) + strength;
            entity.spd = Random.Range(base_random[rank].Item1, base_random[rank].Item2) + strength;
            //Debug.Log("atk: " + entity.atk);
            //Debug.Log("def: " + entity.def);
            //Debug.Log("spd: " + entity.spd);

/*
            Debug.Log("Enemy spawn; " + 
                entity.lv.ToString() + " / " +
                entity.exp.ToString() + " / " +
                entity.hp_max.ToString() + " / " +
                entity.atk.ToString() + " / " +
                entity.def.ToString() + " / " +
                entity.spd.ToString()
            );
*/

            return entity;
        }

        public static GameObject createGameObject(EnemyEntity entity)
        {
            var go = enemy_prefabs[entity.prefab_id];
            return Object.Instantiate(go);
        }
    }
}