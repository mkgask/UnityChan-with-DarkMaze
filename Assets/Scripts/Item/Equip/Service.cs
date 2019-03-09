using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using sgffu.EventMessage;
using cakeslice;
using EquipEntity = Item.Equip.EquipEntity;
using Rank = GameStageRank.Rank;
using Type = Item.Equip.Type;

namespace Item.Equip
{
    public class Service
    {

        private static float equiped_force = 3f;

/*
        public static void init() {
            MessageBroker.Default.Receive<ChangeEquipEvent>().Subscribe(x => {
                equipChange(x.id);
            });
        }
*/


        private static EquipEntity now_quiped_entity;

        public static EquipEntity entity {
            get { return now_quiped_entity; }
            set { now_quiped_entity = value; }
        }



        public static int status(string status_name)
        {
            switch(status_name) {
                case "Atk": return now_quiped_entity.atk;
                case "Def": return now_quiped_entity.def;
                case "Spd": return now_quiped_entity.spd;
                case "AtkOp": return now_quiped_entity.atk_op;
                case "DefOp": return now_quiped_entity.def_op;
                case "SpdOp": return now_quiped_entity.spd_op;
                case "AtkTotal": return now_quiped_entity.atk + now_quiped_entity.atk_op;
                case "DefTotal": return now_quiped_entity.def + now_quiped_entity.def_op;
                case "SpdTotal": return now_quiped_entity.spd + now_quiped_entity.spd_op;
                default: break;
            }

            return 0;
        }

        public static Type type()
        {
            return now_quiped_entity.type;
        }

        public static Rank rank()
        {
            return now_quiped_entity.rank;
        }

        public static string weapon_name()
        {
            if (now_quiped_entity == null) { return "Hand"; }
            return now_quiped_entity.name;
        }

        public static float attack_speed_adjust()
        {
            return now_quiped_entity.attack_speed_adjust;
        }



        private static Dictionary<int, GameObject> equip_item_list = new Dictionary<int, GameObject>();

        public static GameObject get(int id)
        {
            if (!equip_item_list.ContainsKey(id)) {
                throw new KeyNotFoundException();
            }

            //Debug.Log("EquipService::get() : id : " + id);
            return equip_item_list[id];
        }

        public static void set(GameObject equip)
        {
            ItemEquipController iec = equip.GetComponentInChildren<ItemEquipController>();

            if (iec == null) {
                throw new Exception();
            }

            int id = iec.entity.id;
            Debug.Log("EquipService::set() : id : " + id + " : " + iec.entity.name);

            if (equip_item_list.ContainsKey(id)) {
                equip_item_list[id] = equip;
            } else {
                equip_item_list.Add(id, equip);
            }
        }

        public static void del(int id)
        {
            //if (equip_item_list[key].GetComponent<ItemEquipController>().entity == now_quiped_entity) continue;
            if (equip_item_list[id] != null) {
                if (equip_item_list[id].tag == "Equiped") return;
                GameObject.Destroy(equip_item_list[id]);
            }

            equip_item_list.Remove(id);
        }

        public static void clear()
        {
            now_quiped_entity = null;
            equip_item_list.Clear();
        }



        public static void onlyEquip(int id, GameObject player_gameobject = null, bool update_service_data = true)
        {
            Debug.Log("EquipService.onlyEquip() : start");
            //if (now_quiped_entity != null && now_quiped_entity.id == id) return;

            GameObject go = get(id);
            Rigidbody rb = go.GetComponentInChildren<Rigidbody>();
            rb.isKinematic = true;

            GameObject player_right_hand;

            if (player_gameobject != null) {
                player_right_hand = player_gameobject.transform.Find("_root/center/Character1_Hips/Character1_Spine/Character1_Spine1/Character1_Spine2/Character1_RightShoulder/Character1_RightArm/Character1_RightForeArm/Character1_RightHand").gameObject;
            } else {
                player_right_hand = GameObject.FindGameObjectWithTag("PlayerRightHand");
                //Debug.Log("player_right_hand.name" + player_right_hand.name);
            }

            if (player_right_hand == null) {
                throw new Exception();
            }

            BoxCollider collider = go.GetComponentInChildren<BoxCollider>();
            //collider.isTrigger = true;
            collider.enabled = false;

            go.transform.SetParent(player_right_hand.transform);

            ItemEquipController iec = go.GetComponentInChildren<ItemEquipController>();
            Debug.Log("EquipService.onlyEquip() : name : " + iec.entity.name);
            Debug.Log("EquipService.onlyEquip() : type : " + iec.entity.type);
            Debug.Log("EquipService.onlyEquip() : prefab : " + iec.entity.prefab_id);
            iec.enabled = false;

            Debug.Log("EquipService.onlyEquip() : before : go.transform.localPosition " + go.transform.localPosition.ToString());
            Debug.Log("EquipService.onlyEquip() : before : go.transform.localRotation " + go.transform.localRotation.ToString());

            go.transform.position = Vector3.zero;
            go.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            go.transform.localPosition = iec.entity.equip_position_offset;
            go.transform.localRotation = iec.entity.equip_rotation_offset;

            Debug.Log("EquipService.onlyEquip() : after : iec.entity.equip_position_offset " + iec.entity.equip_position_offset.ToString());
            Debug.Log("EquipService.onlyEquip() : after : iec.entity.equip_rotation_offset " + iec.entity.equip_rotation_offset.ToString());

            Debug.Log("EquipService.onlyEquip() : after : go.transform.localPosition " + go.transform.localPosition.ToString());
            Debug.Log("EquipService.onlyEquip() : after : go.transform.localRotation " + go.transform.localRotation.ToString());

            go.tag = "Equiped";

            foreach (Transform child in go.transform)
            {
                child.tag = "Equiped";
            }

            Debug.Log("EquipService.onlyEquip() : Outline.enabled = false");
            foreach (var outline in go.GetComponentsInChildren<Outline>()) {
                outline.enabled = false;
            }

            string main_body_gameobject_name = iec.entity.main_body_gameobject_name;
            Transform main_body = go.transform.Find(main_body_gameobject_name);

            if (main_body != null) {
                main_body.localPosition = Vector3.zero;
                main_body.localRotation = new Quaternion(0f, 0f, 0f, 0f);
            }

            go.SetActive(true);
            if (update_service_data) now_quiped_entity = iec.entity;
        }



        public static void equipChange(int id, GameObject player_gameobject = null)
        {
            Debug.Log("EquipService::equipChange() : id : " + id);

            if (now_quiped_entity != null && now_quiped_entity.id == id) return;

            GameObject go = get(id);

            Rigidbody rb = go.GetComponentInChildren<Rigidbody>();
            rb.isKinematic = true;

            bool exist_eqiuped = true;
            GameObject[] equiped = new GameObject[0];

            try {
                equiped = GameObject.FindGameObjectsWithTag("Equiped");
            } catch(UnityException e) {
                exist_eqiuped = false;
            }

            if (equiped.Length < 1) exist_eqiuped = false;

            //Debug.Log("EquipService::equipChange() : equiped.Length : " + equiped.Length);
            //Debug.Log("EquipService::equipChange() : exist_eqiuped : " + exist_eqiuped);

            GameObject player_right_hand;

            if (player_gameobject != null) {
                player_right_hand = player_gameobject.transform.Find("_root/center/Character1_Hips/Character1_Spine/Character1_Spine1/Character1_Spine2/Character1_RightShoulder/Character1_RightArm/Character1_RightForeArm/Character1_RightHand").gameObject;
            } else {
                player_right_hand = GameObject.FindGameObjectWithTag("PlayerRightHand");
                //Debug.Log("player_right_hand.name" + player_right_hand.name);
            }

            //Debug.Log("EquipService::equipChange() : player_right_hand : " + player_right_hand);

            if (player_right_hand == null) {
                throw new Exception();
            }

            Debug.Log("EquipService::equipChange() : equiped.Length : " + equiped.Length.ToString());

            if (exist_eqiuped && 0 < equiped.Length) {
                foreach (var x in equiped) {
                    Debug.Log("EquipService::equipChange() : equiped : x.name : " + x.name.ToString());
                    if (x.layer == 8) continue;

                    x.tag = "ItemEquip";

                    foreach (Transform child in x.transform)
                    {
                        child.tag = "ItemEquip";
                    }

                    x.transform.parent = null;
                    //x.transform.SetParent(null);
                    //x.transform.position = player_right_hand.transform.position;

                    ItemEquipController ie = x.GetComponentInChildren<ItemEquipController>();
                    ie.TargetDisable();

                    BoxCollider col = x.GetComponentInChildren<BoxCollider>();
                    col.enabled = true;
                    col.isTrigger = false;

                    Debug.Log("EquipService.equipChange() : Outline.enabled = true");
                    foreach (var o in x.GetComponentsInChildren<Outline>()) {
                        o.enabled = true;
                    }

                    Rigidbody equped_rb = x.GetComponentInChildren<Rigidbody>();

                    if (equped_rb != null) {
                        equped_rb.isKinematic = false;
                        equped_rb.AddRelativeForce(new Vector3(0f, equped_rb.mass * equiped_force, equped_rb.mass * equiped_force), ForceMode.Impulse);
                    }
                }
            }

            //Debug.Log("player_right_hand: " + player_right_hand);
            //Debug.Log("player_right_hand.transform: " + player_right_hand.transform);

            BoxCollider collider = go.GetComponentInChildren<BoxCollider>();
            collider.isTrigger = true;
            //collider.enabled = false;

            go.transform.SetParent(player_right_hand.transform);

            ItemEquipController iec = go.GetComponentInChildren<ItemEquipController>();
            Debug.Log("EquipService.equipChange() : " + iec.entity.name);

            go.transform.position = Vector3.zero;
            go.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            go.transform.localPosition = iec.entity.equip_position_offset;
            go.transform.localRotation = iec.entity.equip_rotation_offset;

            go.tag = "Equiped";

            foreach (Transform child in go.transform)
            {
                child.tag = "Equiped";
            }

            Debug.Log("EquipService.equipChange() : Outline.enabled = false");
            foreach (var outline in go.GetComponentsInChildren<Outline>()) {
                outline.enabled = false;
            }

            string main_body_gameobject_name = iec.entity.main_body_gameobject_name;
            Transform main_body = go.transform.Find(main_body_gameobject_name);

            if (main_body != null) {
                main_body.localPosition = Vector3.zero;
                main_body.localRotation = new Quaternion(0f, 0f, 0f, 0f);
            }

            go.SetActive(true);
            now_quiped_entity = iec.entity;

            if (exist_eqiuped) {
                MessageBroker.Default.Publish<PlaySe>(new PlaySe{ name = "Sword Drop" });
            }
        }



        private static List<GameObject> target_list = new List<GameObject>();

        public static void OnTarget(GameObject target)
        {
            bool found = false;

            //Debug.Log("Item.Equip.Service::OnTarget()::target.name" + target.name);

            target_list.ForEach((x) => {
                if (x == target) {
                    x.SendMessage("OnTarget");
                    found = true;
                } else { x.SendMessage("TargetDisable"); }
            });

            if (!found) {
                target_list.Add(target);
                target.SendMessage("OnTarget");
            }
        }

        public static void ClearTarget()
        {
            target_list.Clear();
        }

        public static Entity randomCreate()
        {
            return new Entity();
        }

        public static void destroyEquipItem()
        {
/*
            if (0 < equip_item_list.Count) {
                foreach (var g in equip_item_list) {
                    g.Value.SetActive(true);
                }
            }

            GameObject[] gos = GameObject.FindGameObjectsWithTag("ItemEquip");

            if (0 < gos.Length) {
                foreach (var go in gos) {
                    if (go.transform.parent && go.transform.parent.tag == "Player") { continue; }
                    GameObject.Destroy(go);
                }
            }
*/
            Debug.Log("Item.Equip.Service::destroyEquipItem() : equip_item_list.Count : " + equip_item_list.Count);

            if (0 < equip_item_list.Count) {
/*
                foreach (var g in equip_item_list) {
                    if (g.Value.tag == "Equiped") continue;
                    //if (g.Value.GetComponent<ItemEquipController>().entity == now_quiped_entity) continue;
                    equip_item_list.Remove(g.Key);
                    //GameObject.Destroy(g.Value);
                }
*/
                foreach (var key in equip_item_list.Keys.ToArray()) {
                    del(key);
/*
                    if (equip_item_list[key].tag == "Equiped") continue;
                    //if (equip_item_list[key].GetComponent<ItemEquipController>().entity == now_quiped_entity) continue;
                    GameObject.Destroy(equip_item_list[key]);
                    equip_item_list.Remove(key);
*/
                }
            }

        }
    }
}