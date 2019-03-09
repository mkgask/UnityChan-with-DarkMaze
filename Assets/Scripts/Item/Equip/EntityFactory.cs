using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using File = Filesystem.File;
using Rank = GameStageRank.Rank;
using GameStageRankService = GameStageRank.GameStageRankService;
using EquipEntity = Item.Equip.EquipEntity;
using EquipService = Item.Equip.Service;
using ItemID = Item.ID;
using sgffu.Config;

namespace Item.Equip
{
    class Factory
    {
        public static int entity_id = 1;

        private static Dictionary<Rank, Dictionary<ItemID, EquipEntity>> base_resources;

        private static Dictionary<Rank, Tuple<int, int>> base_random;

        private static List<GameObject> _prefabs;

        public static void init(List<GameObject> equip_prefabs)
        {
            //EquipEntity[] equips = ConfigFile.load(ConfigFile.equipConfigFilename, new EquipEntity[1]);
            _prefabs = equip_prefabs;

            // Dランク武器

            var d_knife = new EquipEntity {
                name = "アイアンナイフ",
                type = Type.Knife,
                rank = Rank.D,
                prefab_id = 0,
                main_body_gameobject_name = "Knife00_mesh",
                equip_position_offset = new Vector3(-0.023f, 0.03f, -0.05f),
                equip_rotation_offset = new Quaternion(0f, -135f, -133f, 0f),
                atk = 3,
                def = 7,
                spd = 11,
                attack_speed_adjust = 1f,
            };

            var d_staff = new EquipEntity {
                name = "アイアンスタッフ",
                type = Type.Staff,
                rank = Rank.D,
                prefab_id = 3,
                main_body_gameobject_name = "Staff00_mesh",
                equip_position_offset = new Vector3(-0.023f, 0.03f, -0.05f),
                equip_rotation_offset = new Quaternion(0f, -135f, -133f, 0f),
                atk = 5,
                def = 11,
                spd = 5,
                attack_speed_adjust = 1f,
            };

            var d_sword = new EquipEntity {
                name = "アイアンソード",
                type = Type.Sword,
                rank = Rank.D,
                prefab_id = 1,
                main_body_gameobject_name = "Sword00_mesh",
                equip_position_offset = new Vector3(-0.023f, 0.03f, -0.05f),
                equip_rotation_offset = new Quaternion(0f, -135f, -133f, 0f),
                atk = 7,
                def = 7,
                spd = 7,
                attack_speed_adjust = 1f,
            };

            var d_mace = new EquipEntity {
                name = "アイアンメイス",
                type = Type.Mace,
                rank = Rank.D,
                prefab_id = 2,
                main_body_gameobject_name = "Mace00_mesh",
                equip_position_offset = new Vector3(-0.023f, 0.03f, -0.05f),
                equip_rotation_offset = new Quaternion(0f, -135f, -133f, 0f),
                atk = 9,
                def = 5,
                spd = 5,
                attack_speed_adjust = 1f,
            };

            var d_axe = new EquipEntity {
                name = "アイアンアクス",
                type = Type.Axe,
                rank = Rank.D,
                prefab_id = 4,
                main_body_gameobject_name = "Axe00_mesh",
                equip_position_offset = new Vector3(-0.04f, 0.01f, -0.03f),
                equip_rotation_offset = Quaternion.Euler(0f, -70f, -85f),
                atk = 11,
                def = 3,
                spd = 3,
                attack_speed_adjust = 1f,
            };

            // Cランク武器

            var c_knife = new EquipEntity {
                name = "スチールダガー",
                type = Type.Knife,
                rank = Rank.C,
                prefab_id = 5,
                main_body_gameobject_name = "dagger",
                equip_position_offset = new Vector3(-0.05f, 0.03f, -0.02f),
                equip_rotation_offset = Quaternion.Euler(76.5f, -180f, -160f),
                atk = 13,
                def = 19,
                spd = 24,
                attack_speed_adjust = 1f,
            };

            var c_staff = new EquipEntity {
                name = "ライトスタッフ",
                type = Type.Staff,
                rank = Rank.C,
                prefab_id = 9,
                main_body_gameobject_name = "Metalstaff01",
                equip_position_offset = new Vector3(-0.053f, -0.019f, -0.181f),
                equip_rotation_offset = Quaternion.Euler(78f, 0f, 0f),
                atk = 16,
                def = 24,
                spd = 16,
                attack_speed_adjust = 1f,
            };

            var c_sword = new EquipEntity {
                name = "スチールソード",
                type = Type.Sword,
                rank = Rank.C,
                prefab_id = 6,
                main_body_gameobject_name = "sword",
                equip_position_offset = new Vector3(-0.05f, 0.03f, -0.02f),
                equip_rotation_offset = Quaternion.Euler(76.5f, -180f, -160f),
                atk = 19,
                def = 19,
                spd = 19,
                attack_speed_adjust = 1f,
            };

            var c_mace = new EquipEntity {
                name = "スチールメイス",
                type = Type.Mace,
                rank = Rank.C,
                prefab_id = 7,
                main_body_gameobject_name = "mace",
                equip_position_offset = new Vector3(-0.05f, 0.03f, -0.02f),
                equip_rotation_offset = Quaternion.Euler(76.5f, -180f, -160f),
                atk = 21,
                def = 16,
                spd = 16,
                attack_speed_adjust = 1f,
            };

            var c_axe = new EquipEntity {
                name = "スチールアクス",
                type = Type.Mace,
                rank = Rank.C,
                prefab_id = 8,
                main_body_gameobject_name = "axe",
                equip_position_offset = new Vector3(-0.064f, 0.03f, -0.008f),
                equip_rotation_offset = Quaternion.Euler(295f, 0f, 0f),
                atk = 24,
                def = 13,
                spd = 13,
                attack_speed_adjust = 1f,
            };

            // Bランク武器

            var b_kukri_knife = new EquipEntity {
                name = "ククリナイフ",
                type = Type.Knife,
                rank = Rank.B,
                prefab_id = 10,
                main_body_gameobject_name = "kukri_Normal",
                equip_position_offset = new Vector3(-0.054f, 0.017f, 0.19f),
                equip_rotation_offset = Quaternion.Euler(-0.39f, 12.3f, 270f),
                atk = 33,
                def = 45,
                spd = 51,
                attack_speed_adjust = 1f,
            };

            var b_staff_of_pain = new EquipEntity {
                name = "スタッフオブペイン",
                type = Type.Staff,
                rank = Rank.B,
                prefab_id = 15,
                main_body_gameobject_name = "StaffOfPain",
                equip_position_offset = new Vector3(-0.06f, 0.05f, 0.3f),
                equip_rotation_offset = Quaternion.Euler(90f, 0f, 0f),
                atk = 37,
                def = 51,
                spd = 37,
                attack_speed_adjust = 1f,
            };

            var b_heavy_sword = new EquipEntity {
                name = "ヘヴィソード",
                type = Type.Sword,
                rank = Rank.B,
                prefab_id = 11,
                main_body_gameobject_name = "dark_sword",
                equip_position_offset = new Vector3(-0.058f, 0.022f, 0.011f),
                equip_rotation_offset = Quaternion.Euler(90f, 0f, 0f),
                atk = 41,
                def = 41,
                spd = 41,
                attack_speed_adjust = 1f,
            };

            var b_heavy_mace = new EquipEntity {
                name = "ヘヴィメイス",
                type = Type.Mace,
                rank = Rank.B,
                prefab_id = 13,
                main_body_gameobject_name = "mace",
                equip_position_offset = new Vector3(-0.058f, 0.022f, 0.011f),
                equip_rotation_offset = Quaternion.Euler(90f, 0f, 0f),
                atk = 45,
                def = 37,
                spd = 37,
                attack_speed_adjust = 1f,
            };

            var b_heavy_axe = new EquipEntity {
                name = "ヘヴィアクス",
                type = Type.Axe,
                rank = Rank.B,
                prefab_id = 12,
                main_body_gameobject_name = "axe",
                equip_position_offset = new Vector3(-0.058f, 0.022f, 0.011f),
                equip_rotation_offset = Quaternion.Euler(90f, 0f, 0f),
                atk = 49,
                def = 33,
                spd = 33,
                attack_speed_adjust = 1f,
            };

            var b_heavy_hammer = new EquipEntity {
                name = "ヘヴィハンマー",
                type = Type.Hammer,
                rank = Rank.B,
                prefab_id = 14,
                main_body_gameobject_name = "hammer",
                equip_position_offset = new Vector3(-0.059f, 0.011f, 0.006f),
                equip_rotation_offset = Quaternion.Euler(90f, 0f, 0f),
                atk = 53,
                def = 30,
                spd = 30,
                attack_speed_adjust = 1f,
            };

            // Aランク武器

            var a_demon_dagger = new EquipEntity {
                name = "デーモンダガー",
                type = Type.Knife,
                rank = Rank.A,
                prefab_id = 16,
                main_body_gameobject_name = "dagger",
                equip_position_offset = new Vector3(-0.052f, -0.028f, 0.153f),
                equip_rotation_offset = Quaternion.Euler(-20f, 180f, 10f),
                atk = 64,
                def = 74,
                spd = 84,
                attack_speed_adjust = 1f,
            };

            var a_elven_staff = new EquipEntity {
                name = "エルブンスタッフ",
                type = Type.Staff,
                rank = Rank.D,
                prefab_id = 20,
                main_body_gameobject_name = "staff",
                equip_position_offset = new Vector3(0.026f, 0.051f, 0.33f),
                equip_rotation_offset = Quaternion.Euler(-247f, -246f, -260f),
                atk = 69,
                def = 84,
                spd = 69,
                attack_speed_adjust = 1f,
            };

            var a_elven_sword = new EquipEntity {
                name = "エルブンソード",
                type = Type.Sword,
                rank = Rank.A,
                prefab_id = 17,
                main_body_gameobject_name = "sword",
                equip_position_offset = new Vector3(-0.056f, 0.016f, 0f),
                equip_rotation_offset = Quaternion.Euler(-104f, -62f, -113f),
                atk = 74,
                def = 74,
                spd = 74,
                attack_speed_adjust = 1f,
            };

            var a_elven_axe = new EquipEntity {
                name = "エルブンアクス",
                type = Type.Axe,
                rank = Rank.A,
                prefab_id = 18,
                main_body_gameobject_name = "axe",
                equip_position_offset = new Vector3(-0.05f, 0.029f, 0.094f),
                equip_rotation_offset = Quaternion.Euler(-76f, 118f, 67f),
                atk = 79,
                def = 69,
                spd = 69,
                attack_speed_adjust = 1f,
            };

            var a_elven_hammer = new EquipEntity {
                name = "エルブンハンマー",
                type = Type.Hammer,
                rank = Rank.A,
                prefab_id = 19,
                main_body_gameobject_name = "hammer",
                equip_position_offset = new Vector3(-0.04f, 0.03f, 0.12f),
                equip_rotation_offset = Quaternion.Euler(-76f, 118f, 67f),
                atk = 84,
                def = 64,
                spd = 64,
                attack_speed_adjust = 1f,
            };



            // Sランク武器

            var s_dragon_knife = new EquipEntity {
                name = "ドラゴンナイフ",
                type = Type.Knife,
                rank = Rank.S,
                prefab_id = 21,
                main_body_gameobject_name = "DragonKnife",
                equip_position_offset = new Vector3(-0.045f, 0.026f, -0.15f),
                equip_rotation_offset = Quaternion.Euler(-90f, 0f, 180f),
                atk = 103,
                def = 127,
                spd = 145,
                attack_speed_adjust = 1f,
            };

            var s_bahamut_staff = new EquipEntity {
                name = "バハムートスタッフ",
                type = Type.Staff,
                rank = Rank.S,
                prefab_id = 27,
                main_body_gameobject_name = "Staff03",
                equip_position_offset = new Vector3(-0.05f, 0.02f, 0.2f),
                equip_rotation_offset = Quaternion.Euler(90f, 0f, 0f),
                atk = 109,
                def = 145,
                spd = 115,
                attack_speed_adjust = 1f,
            };

            var s_rune_sword = new EquipEntity {
                name = "ルーンソード",
                type = Type.Sword,
                rank = Rank.S,
                prefab_id = 28,
                main_body_gameobject_name = "Rune Sword",
                equip_position_offset = new Vector3(-0.16f, 0.103f, 0.101f),
                equip_rotation_offset = Quaternion.Euler(90f, 0f, 0f),
                atk = 115,
                def = 115,
                spd = 127,
                attack_speed_adjust = 1f,
            };

            var s_frozen_knite = new EquipEntity {
                name = "フローズンナイトソード",
                type = Type.Sword,
                rank = Rank.S,
                prefab_id = 22,
                main_body_gameobject_name = "greatsword_of_fn",
                equip_position_offset = new Vector3(-0.082f, -0.12f, 0.14f),
                equip_rotation_offset = Quaternion.Euler(-357f, 103f, 71f),
                atk = 121,
                def = 121,
                spd = 121,
                attack_speed_adjust = 1f,
            };

            var s_dragon_blade = new EquipEntity {
                name = "ドラゴンブレード",
                type = Type.Sword,
                rank = Rank.S,
                prefab_id = 23,
                main_body_gameobject_name = "Dragonblade",
                equip_position_offset = new Vector3(-0.15f, -0.03f, -0.21f),
                equip_rotation_offset = Quaternion.Euler(-65f, 145f, 56f),
                atk = 127,
                def = 127,
                spd = 115,
                attack_speed_adjust = 1f,
            };

            var s_runic_axe = new EquipEntity {
                name = "ルーニックアクス",
                type = Type.Axe,
                rank = Rank.S,
                prefab_id = 24,
                main_body_gameobject_name = "hacha_malla_base_triangulada",
                equip_position_offset = new Vector3(-0.16f, 0.103f, 0.101f),
                equip_rotation_offset = Quaternion.Euler(90f, 0f, 0f),
                atk = 133,
                def = 115,
                spd = 115,
                attack_speed_adjust = 1f,
            };

            var s_mystic_hammer = new EquipEntity {
                name = "ミスティックハンマー",
                type = Type.Hammer,
                rank = Rank.S,
                prefab_id = 26,
                main_body_gameobject_name = "Mystic_hammer",
                equip_position_offset = new Vector3(-0.04f, 0.03f, 0.09f),
                equip_rotation_offset = Quaternion.Euler(-270f, -180f, 180f),
                atk = 139,
                def = 109,
                spd = 109,
                attack_speed_adjust = 1f,
            };

            var s_sol_hammer = new EquipEntity {
                name = "ソルハンマー",
                type = Type.Hammer,
                rank = Rank.S,
                prefab_id = 25,
                main_body_gameobject_name = "FantasyHammer",
                equip_position_offset = new Vector3(-0.057f, 0.028f, -0.236f),
                equip_rotation_offset = Quaternion.Euler(-270f, 180f, -180f),
                atk = 145,
                def = 103,
                spd = 103,
                attack_speed_adjust = 1f,
            };

            // 武器配列生成

            base_resources = new Dictionary<Rank, Dictionary<ItemID, EquipEntity>> {
                { Rank.D, new Dictionary<ItemID, EquipEntity> {
                    { ItemID.D_Knife, d_knife },
                    { ItemID.D_Axe, d_axe },
                    { ItemID.D_Mace, d_mace },
                    { ItemID.D_Staff, d_staff },
                    { ItemID.D_Sword, d_sword },
                }},

                { Rank.C, new Dictionary<ItemID, EquipEntity> {
                    { ItemID.C_Knife, c_knife },
                    { ItemID.C_Sword, c_sword },
                    { ItemID.C_Axe, c_axe },
                    { ItemID.C_Mace, c_mace },
                    { ItemID.C_Staff, c_staff },
                }},

                { Rank.B, new Dictionary<ItemID, EquipEntity> {
                    { ItemID.B_KukriKnife, b_kukri_knife },
                    { ItemID.B_HeavySwrod, b_heavy_sword },
                    { ItemID.B_HeavyAxe, b_heavy_axe },
                    { ItemID.B_HeavyMace, b_heavy_mace },
                    { ItemID.B_HeavyHammer, b_heavy_hammer },
                    { ItemID.B_StaffOfPain, b_staff_of_pain },
                }},

                { Rank.A, new Dictionary<ItemID, EquipEntity> {
                    { ItemID.A_DemonDagger, a_demon_dagger },
                    { ItemID.A_ElvenSword, a_elven_sword },
                    { ItemID.A_ElvenAxe, a_elven_axe },
                    { ItemID.A_ElvenHammer, a_elven_hammer },
                    { ItemID.A_ElvenStaff, a_elven_staff },
                }},

                { Rank.S, new Dictionary<ItemID, EquipEntity> {
                    { ItemID.S_DragonKnife, s_dragon_knife },
                    { ItemID.S_FrozeKniteSword, s_frozen_knite },
                    { ItemID.S_DragonBlade, s_dragon_blade },
                    { ItemID.S_RuneSword, s_rune_sword },
                    { ItemID.S_RunicAxe, s_runic_axe },
                    { ItemID.S_Solhammer, s_sol_hammer },
                    { ItemID.S_MysticHammer, s_mystic_hammer },
                    { ItemID.S_BahamutStaff, s_bahamut_staff },
                }},
            };

            base_random = new Dictionary<Rank, Tuple<int, int>> {
                { Rank.D, new Tuple<int, int>(0, 10)},
                { Rank.C, new Tuple<int, int>(10, 30)},
                { Rank.B, new Tuple<int, int>(20, 50)},
                { Rank.A, new Tuple<int, int>(30, 70)},
                { Rank.S, new Tuple<int, int>(50, 100)}
            };

        }

        public static EquipEntity createRandomEntity()
        {
            var rank = GameStageRankService.item();

            if (!base_resources.ContainsKey(rank)) {
                throw new KeyNotFoundException();
            }

            var resouces = base_resources[rank];
            //var rand = resouces.RandomAt();
            var rand = resouces.RandomAt();

            //var entity = (Entity)ObjectPoolService.takeOut(sgffu.ObjectPool.Type.Item, rand.Key);
            //if (entity == null) { entity = createEntity(rank, rand.Key); }
            var entity = createEntity(rank, rand.Key);
            entity.game_object = createGameObject(entity, Vector3.zero);
            entity.game_object.SetActive(false);

            return entity;
        }

        public static EquipEntity createEntity(Rank rank, ItemID id)
        {
            if (!base_resources.ContainsKey(rank) || !base_resources[rank].ContainsKey(id)) {
                throw new KeyNotFoundException();
            }

            EquipEntity entity = base_resources[rank][id].clone();
            entity.id = entity_id;
            entity.atk_op = Random.Range(base_random[rank].Item1, base_random[rank].Item2);
            entity.def_op = Random.Range(base_random[rank].Item1, base_random[rank].Item2);
            entity.spd_op = Random.Range(base_random[rank].Item1, base_random[rank].Item2);

/*
            Debug.Log("Equip spawn; " + 
                entity.name + " / " +
                entity.rank.ToString() + " / " +
                (entity.atk + entity.atk_op).ToString() + " / " +
                (entity.def + entity.def_op).ToString() + " / " +
                (entity.spd + entity.spd_op).ToString()
            );
*/

            entity_id += 1;
            return entity;
        }

        public static EquipEntity createEntity(EquipEntity entity)
        {
            entity.id = entity_id;
            entity_id += 1;
            return entity;
        }

        public static GameObject createGameObject(EquipEntity entity, Vector3 position)
        {
            Debug.Log("Item.Equip.Factory.createGameObject() : entity.prefab_id : " + entity.prefab_id.ToString());
            Debug.Log("Item.Equip.Factory.createGameObject() : _prefabs.Count : " + _prefabs.Count.ToString());

            var go_base = _prefabs[entity.prefab_id];
            var go = Object.Instantiate(go_base);

            ItemEquipController iec = go.GetComponentInChildren<ItemEquipController>();
            if (iec != null) { iec.entity = entity; }

            // 出現位置の設定
            go.transform.position = position;
            EquipService.set(go);

            return go;
        }
    }
}