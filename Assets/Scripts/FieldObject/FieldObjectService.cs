using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

//using sgffu.ObjectPool;
using Type = sgffu.ObjectPool.Type;
using Random = System.Random;
using IInventory = Item.Inventory.IInventory;
using ItemEntity = Item.Entity;
using Rank = GameStageRank.Rank;
using GameStageRankService = GameStageRank.GameStageRankService;
using ItemID = Item.ID;
using EquipEntity = Item.Equip.EquipEntity;
using FieldObjectID = sgffu.FieldObject.ID;

namespace sgffu.FieldObject
{
    using Exception = System.Exception;

    public class FieldObjectService
    {
        private static Dictionary<int, FieldObjectEntity> entity_list = new Dictionary<int, FieldObjectEntity>();

        private static Dictionary<int, IInventory> inventory_list = new Dictionary<int, IInventory>();

        private static float open_inventory_force = 1f;

        private static Dictionary<Rank, Tuple<int, int, int, int>> resouce_num;

        public static void init()
        {
            resouce_num = new Dictionary<Rank, Tuple<int, int, int, int>> {
                //{ Rank.D, new Tuple<int, int, int, int>(0, 2, 3, 7) },
                { Rank.D, new Tuple<int, int, int, int>(0, 2, 0, 1) },
                { Rank.C, new Tuple<int, int, int, int>(1, 3, 1, 1) },
                { Rank.B, new Tuple<int, int, int, int>(2, 4, 1, 2) },
                { Rank.A, new Tuple<int, int, int, int>(3, 5, 1, 2) },
                { Rank.S, new Tuple<int, int, int, int>(4, 6, 1, 3) },
            };
        }

        public static void clear()
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("FieldObject");

            if (0 < gos.Length) {
                foreach (var go in gos) {
                    GameObject.Destroy(go);
                }
            }

            entity_list.Clear();
        }

        public static void reset()
        {
            clear();
            createRandomEntities();
        }


        public static FieldObjectEntity createEntity(FieldObjectID id)
        {
            //FieldObjectEntity entity = (FieldObjectEntity)ObjectPoolService.takeOut(Type.Props, id);
            //FieldObjectEntity entity = FieldObjectEntityFactory.createBlank(id);
            /*
            FieldObjectEntity entity = (FieldObjectEntity)ObjectPoolService.takeOut(Type.Props, id);
            if (entity == null) { entity = FieldObjectEntityFactory.createBlank(id); }
            */
            FieldObjectEntity entity = FieldObjectEntityFactory.createBlank(id);
            FieldObjectEntityFactory.setupInventory(entity);
            FieldObjectEntityFactory.setupRandomPosition(entity);
/*
            if (r == null) {
                ObjectPoolService.ret(Type.Props, entity.type, entity);
            }
*/
            entity_list.Add(entity.id, entity);
            return entity;
        }

        public static FieldObjectEntity createRandomEntity()
        {
            var array = Enum.GetValues(typeof(FieldObjectID));
            var id = getRandomObjectID(array);
            //FieldObjectIDDebugLog(id);
            return createEntity(id);
        }

        public static FieldObjectID getRandomObjectID(Array array)
        {
            FieldObjectID id = FieldObjectID.None;

            do {
                id = (FieldObjectID)array.GetValue((new Random()).Next(array.Length));
            } while (id == FieldObjectID.None);

            return id;
        }

        public static void createRandomEntities()
        {
            var rank = GameStageRankService.props();

            if (!resouce_num.ContainsKey(rank)) {
                throw new KeyNotFoundException();
            }

            var resouces = resouce_num[rank];
            var object_num = (new Random()).Next(resouces.Item1, resouces.Item2 + 1);
            var tresure_num = (new Random()).Next(resouces.Item3, resouces.Item4 + 1);
/*
            Debug.Log("FieldObjectService.createRandomEntities : resouces :" + resouces);
            Debug.Log("FieldObjectService.createRandomEntities : object_num :" + object_num);
            Debug.Log("FieldObjectService.createRandomEntities : tresure_num :" + tresure_num);
*/
            Debug.Log("FieldObjectService.createRandomEntities : tresure_num :" + tresure_num);

            for (var i = 1; i <= object_num; i += 1) {
                //Debug.Log("FieldObjectService.createRandomEntities : object_num :" + i);
                FieldObjectEntityFactory.createRandomEntity();
            }

            for (var i = 1; i <= tresure_num; i += 1) {
                //Debug.Log("FieldObjectService.createRandomEntities : tresure_num :" + i);
                createEntity(FieldObjectID.TreasureBox);
            }

        }

        public static void FieldObjectIDDebugLog(FieldObjectID id)
        {
            switch(id) {
                case FieldObjectID.None: Debug.Log("FieldObjectIDDebugLog: None"); break;
                case FieldObjectID.Torch: Debug.Log("FieldObjectIDDebugLog: Torch"); break;
                case FieldObjectID.TreasureBox: Debug.Log("FieldObjectIDDebugLog: TreasureBox"); break;
                default: Debug.Log("FieldObjectIDDebugLog: default"); break;
            }
        }

        public static void debugLogItemID(ItemID id)
        {
            switch (id) {
                case ItemID.None: Debug.Log("debugLogItemID:72 : ItemID.None"); break;
                case ItemID.D_Sword: Debug.Log("debugLogItemID:73 : ItemID.D_Sword"); break;
            }
        }

        public static void setupInventory(int entity_id, int inventory_size)
        {
            if (inventory_list.ContainsKey(entity_id)) {
                throw new Exception("Found contains key: " + entity_id);
            }

            //Debug.Log("setupInventory:83 : entity_id : " + entity_id);
            //Debug.Log("setupInventory:84 : inventory_size : " + inventory_size);
            if (inventory_size < 1) return;

            inventory_list.Add(entity_id, Item.Factory.createInventory(inventory_size));
        }

        public static void setInventory(int entity_id, ItemEntity item)
        {
            if (!inventory_list.ContainsKey(entity_id)) {
                //Debug.Log("inventory_list.Count: " + inventory_list.Count);
                throw new KeyNotFoundException("Key not found: " + entity_id);
            }

            inventory_list[entity_id].putIn(item);
        }

        public static ItemEntity getInventory(int entity_id)
        {
            if (!inventory_list.ContainsKey(entity_id)) {
                //Debug.Log("inventory_list.Count: " + inventory_list.Count);
                throw new KeyNotFoundException("Entity id: " + entity_id);
            }

            return inventory_list[entity_id].takeOut();
        }

        public static void OnCollisionEnter(int entity_id, GameObject current)
        {
            //Debug.Log("OnCollisionEnter:111 : entity_id : " + entity_id);
/*
            if (!inventory_list.ContainsKey(entity_id)) {
                Debug.Log("inventory_list.Count: " + inventory_list.Count);
                //throw new KeyNotFoundException("Key not found: " + entity_id);
                return;
            }
*/
            ItemEntity item;

            try {
                item = getInventory(entity_id);
            } catch(KeyNotFoundException e) {
                //Debug.Log("KeyNotFoundException: " + e.Message);
                return;
            }

            //debugLogItemID(item.type);

            //item.game_object.transform.SetParent(current.transform);
            item.game_object.transform.position += current.transform.position + new Vector3(0, 0.5f, 0);
            //item.game_object.transform.rotation = current.transform.rotation;
            //Debug.Log(item.game_object.transform.position);
/*
            BoxCollider b_col = item.game_object.GetComponentInChildren<BoxCollider>();
            b_col.isTrigger = true;
*/
            item.game_object.SetActive(true);
            Rigidbody rb = item.game_object.GetComponentInChildren<Rigidbody>();
            //Vector3 jump_force = current.transform.rotation.eulerAngles * open_inventory_force;

            FieldObjectEntity entity = entity_list[entity_id];

            Vector3 jump_force = getItemJumpForce(Mathf.RoundToInt(entity.rotation.y)) * open_inventory_force;
            //Debug.Log("FieldObject::OnCollisionEnter() : entity.rotation : " + entity.rotation.ToString());
            //Debug.Log("FieldObject::OnCollisionEnter() : jump_force : " + jump_force.ToString());

            var original_scale = item.game_object.transform.localScale[0];
            var local_scale = 0.1f;
            var tmp_scale = original_scale * local_scale;
            item.game_object.transform.localScale = new Vector3(tmp_scale, tmp_scale, tmp_scale);

            //Debug.Log("FieldObject::OnCollisionEnter() : item.game_object.name : " + item.game_object.name);
            //Debug.Log("FieldObject::OnCollisionEnter() : original_scale : " + original_scale.ToString());

            Observable.Interval(TimeSpan.FromMilliseconds(50)).Take(3).Subscribe(x => {
                local_scale += 0.3f;
                tmp_scale = original_scale * local_scale;
                item.game_object.transform.localScale = new Vector3(tmp_scale, tmp_scale, tmp_scale);
            });
/*
            Observable.Interval(TimeSpan.FromMilliseconds(50)).Take(9).Subscribe(x => {
                local_scale += 0.1f;
                tmp_scale = original_scale * local_scale;
                item.game_object.transform.localScale = new Vector3(tmp_scale, tmp_scale, tmp_scale);
            });
*/
            if (rb != null) {
                //Debug.Log("rb.AddRelativeForce");
                //rb.AddRelativeForce(new Vector3(0f, rb.mass * open_inventory_force, rb.mass * open_inventory_force), ForceMode.Impulse);
                //rb.AddRelativeForce(jump_force, ForceMode.Impulse);
                rb.AddForce(jump_force, ForceMode.Impulse);
            }
        }

        public static Vector3 getItemJumpForce(int current_y)
        {
            //Debug.Log("FieldObjectService.getItemJumpForce() : current_y : " + current_y.ToString());

            if (current_y == 90) return new Vector3(0f, 0.5f, -1f);
            return new Vector3(1f, 0.5f, 0f);
        }

    }
}
