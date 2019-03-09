using System;
using System.Collections.Generic;
using UnityEngine;
using sgffu.EffectiveFloor;
using sgffu.ObjectPool;
using Object = UnityEngine.Object;
using Rank = GameStageRank.Rank;
using GameStageRankService = GameStageRank.GameStageRankService;
using EquipEntity = Item.Equip.EquipEntity;
using EquipFactory = Item.Equip.Factory;
using FieldObjectID = sgffu.FieldObject.ID;

namespace sgffu.FieldObject 
{
    using Exception = System.Exception;

    public class FieldObjectEntityFactory
    {
        public static int entity_id = 1;

        private static Dictionary<Rank, Dictionary<FieldObjectID, FieldObjectEntity>> base_resources;

        private static List<GameObject> prefabs;

        public static void init(List<GameObject> foPrefabs)
        {
            prefabs = foPrefabs;

            var tresure_box = new FieldObjectEntity {
                type = FieldObjectID.TreasureBox,
                size = 1,
                //prefab_path = "Props/Chest",
                prefab_id = 0,
                inventory_size = 1
            };

            var torch = new FieldObjectEntity {
                type = FieldObjectID.Torch,
                size = 1,
                //prefab_path = "Props/Torch",
                prefab_id = 1,
                inventory_size = 0,
            };

            var candle = new FieldObjectEntity {
                type = FieldObjectID.Candle,
                size = 1,
                //prefab_path = "Props/Candle",
                prefab_id = 2,
                inventory_size = 0,
            };

            var small_keg = new FieldObjectEntity {
                type = FieldObjectID.SmallKeg,
                size = 1,
                //prefab_path = "Props/Small Keg",
                prefab_id = 3,
                inventory_size = 0,
            };

            var large_keg = new FieldObjectEntity {
                type = FieldObjectID.LargeKeg,
                size = 1,
                //prefab_path = "Props/Large Keg",
                prefab_id = 4,
                inventory_size = 0,
            };

            var barrel = new FieldObjectEntity {
                type = FieldObjectID.Barrel,
                size = 1,
                //prefab_path = "Props/Barrel",
                prefab_id = 5,
                inventory_size = 0,
            };

            base_resources = new Dictionary<Rank, Dictionary<FieldObjectID, FieldObjectEntity>> {
                { Rank.D, new Dictionary<FieldObjectID, FieldObjectEntity> {
                    { FieldObjectID.SmallKeg, small_keg },
                    { FieldObjectID.LargeKeg, large_keg },
                    { FieldObjectID.Barrel, barrel },
                    { FieldObjectID.Torch, torch },
                    { FieldObjectID.Candle, candle },
                } },

                { Rank.C, new Dictionary<FieldObjectID, FieldObjectEntity> {
                    { FieldObjectID.SmallKeg, small_keg },
                    { FieldObjectID.Barrel, barrel },
                    { FieldObjectID.Torch, torch },
                    { FieldObjectID.Candle, candle },
                } },

                { Rank.B, new Dictionary<FieldObjectID, FieldObjectEntity> {
                    { FieldObjectID.SmallKeg, small_keg },
                    { FieldObjectID.Barrel, barrel },
                    { FieldObjectID.Candle, candle },
                } },

                { Rank.A, new Dictionary<FieldObjectID, FieldObjectEntity> {
                    { FieldObjectID.SmallKeg, small_keg },
                    { FieldObjectID.Barrel, barrel },
                } },

                { Rank.S, new Dictionary<FieldObjectID, FieldObjectEntity> {
                    { FieldObjectID.SmallKeg, small_keg },
                    { FieldObjectID.Barrel, barrel },
                } },

                { Rank.All, new Dictionary<FieldObjectID, FieldObjectEntity> {
                    { FieldObjectID.TreasureBox, tresure_box },
                } },

            };

        }

/*
        public static FieldObjectEntity create(
            FieldObjectID entity_type = FieldObjectID.None,
            int size = 0,
            Vector3? offset_position = null,
            string prefab_path = ""
        ) {
            EffectiveFloorEntity pos = EffectiveFloorService.rand(size);
            if (pos == null) { return null; }

            //Debug.Log("FieldObjectEntityFactory.create: id: " + id);
            FieldObjectEntity entity = new FieldObjectEntity {
                id = entity_id,
                type = entity_type,
                position = pos.position + (offset_position ?? Vector3.zero),
                //pos_x = pos.pos_x,
                //pos_z = pos.pos_z,
                size = size,
                prefab_path = prefab_path,
            };
            //Debug.Log("FieldObjectEntityFactory.create: entity.pos: " + entity.pos_x + ", " + entity.pos_z);

            entity.game_object = Object.Instantiate(Resources.Load(entity.prefab_path, typeof(GameObject))) as GameObject;
            //entity.game_object.transform.position = new Vector3(entity.pos_x, 0.5f, entity.pos_z);
            entity.game_object.transform.position = entity.position;
            entity.game_object.transform.Rotate(pos.rotation);

            entity_id += 1;

            setupInventory(entity);
            return entity;
        }
*/

        public static FieldObjectEntity createRandomEntity()
        {
            var rank = GameStageRankService.props();

            if (!base_resources.ContainsKey(rank)) {
                throw new KeyNotFoundException();
            }

            var resouces = base_resources[rank];
            var rand = resouces.RandomAt();

            //var entity = (Entity)ObjectPoolService.takeOut(sgffu.ObjectPool.Type.Item, rand.Key);
            //if (entity == null) { entity = createEntity(rank, rand.Key); }
            var entity = createBlank(rand.Key);
            setupInventory(entity);
            setupRandomPosition(entity);
            return entity;
        }

        public static FieldObjectEntity createEntity(Rank rank, FieldObjectID id)
        {
            if (!base_resources.ContainsKey(rank) || !base_resources[rank].ContainsKey(id)) {
                throw new KeyNotFoundException();
            }

            var entity = base_resources[rank][id].clone();
            entity.id = entity_id;

            entity_id += 1;
            return entity;
        }

        public static FieldObjectEntity createBlank(FieldObjectID id)
        {
            var exist = Rank.None;
            //debugLogFirldObjectID(id);

            foreach (Rank rank in Enum.GetValues(typeof(Rank))) {
                //debugLogRank(rank);
                if (rank == Rank.None) { continue; }

                if (!base_resources.ContainsKey(rank)) {
                    throw new Exception("rank Not found in base_resources" + Enum.GetName(typeof(Rank), rank));
                }

                if (base_resources[rank].ContainsKey(id)) { exist = rank; }
            }

            if (exist == Rank.None) {
                throw new Exception("Key not found in FieldObjectEntity base resouces.");
            }

            var resource = base_resources[exist][id];

            var entity = new FieldObjectEntity {
                id = entity_id,
                type = resource.type,
                size = resource.size,
                //prefab_path = resource.prefab_path,
                prefab_id = resource.prefab_id,
                inventory_size = resource.inventory_size,
            };

            var go_base = prefabs[entity.prefab_id];
            entity.game_object = Object.Instantiate(go_base);
            var foc = entity.game_object.GetComponent<FieldObjectController>();
            if (foc != null) { foc.id = entity_id; }
            entity.game_object.SetActive(false);

            entity_id += 1;
            return entity;
        }

/*
        public static void debugLogRank(Rank rank) {
            switch(rank) {
                case Rank.None: Debug.Log("Rank.None"); break;
                case Rank.D: Debug.Log("Rank.D"); break;
                case Rank.C: Debug.Log("Rank.C"); break;
                case Rank.B: Debug.Log("Rank.B"); break;
                case Rank.A: Debug.Log("Rank.A"); break;
                case Rank.S: Debug.Log("Rank.S"); break;
            }
        }
*/
/*
        public static void debugLogFirldObjectID(FieldObjectID id) {
            switch(id) {
                case FieldObjectID.None: Debug.Log("FieldObjectID.None"); break;
                case FieldObjectID.TreasureBox: Debug.Log("FieldObjectID.TreasureBox"); break;
                case FieldObjectID.Torch: Debug.Log("FieldObjectID.Torch"); break;
            }
        }
*/

        public static FieldObjectEntity setupRandomPosition(FieldObjectEntity entity)
        {
            var pos = EffectiveFloorService.rand(entity.size);
            if (pos == null) { return null; }

            entity.position = pos.position;
            entity.rotation = pos.rotation;
            entity.game_object.transform.position = pos.position;
            entity.game_object.transform.Rotate(pos.rotation);
            entity.game_object.SetActive(true);
            return entity;
        }

        public static FieldObjectEntity setupInventory(FieldObjectEntity entity)
        {
            //Debug.Log("entity.id: " + entity.id);
            //Debug.Log("entity.inventory_size: " + entity.inventory_size);

            FieldObjectService.setupInventory(entity.id, entity.inventory_size);
            if (entity.inventory_size < 1) { return entity; }

            var equip = EquipFactory.createRandomEntity();
            FieldObjectService.setInventory(entity.id, equip);
            return entity;
        }
    }
}