using System.Collections.Generic;
using UnityEngine;
using EffectFactory = Effect.Factory;
using EffectType = Effect.Type;

using TMPro;

namespace Effect
{
    public class Service
    {

        /*
            DamageLabel Effect
        */
        private static Camera main_camera;

        private static GameObject center_panel;

        private static float damege_label_pos_y_offset = 50f;



        /*
            NextRoom Effect
        */
        //private static GameObject[] next_rooms;

        private static List<Transform> next_rooms = new List<Transform>();


        /*
            Foundation
        */
        public static void init()
        {
            main_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            center_panel = GameObject.FindGameObjectWithTag("CenterPanel");
            
            var next_room_parents = GameObject.FindGameObjectsWithTag("NextRoom");
            var i = 0;

            foreach (var go in next_room_parents) {
                //Debug.Log(go.transform.Find("NextRoomEffect").name);

                next_rooms.Insert(i, go.transform.Find("NextRoomEffect"));
                //next_rooms[i] = go.transform.Find("NextRoomEffect");
                i += 1;
            }

            //next_rooms = GameObject.FindGameObjectsWithTag("NextRoom");
            nextRoomOff();
        }



        /*
            DamageLabel Effect
        */
        public static void damage(int damage, Vector3 world_position)
        {
            var damage_label = EffectFactory.create(EffectType.DamageLabel);
            GameObject go = damage_label.game_object;
            go.transform.SetParent(center_panel.transform);

            RectTransform rect_transform = damage_label.game_object.GetComponent<RectTransform>();
            rect_transform.position = RectTransformUtility.WorldToScreenPoint(main_camera, world_position);
            rect_transform.position = new Vector3(
                rect_transform.position.x,
                rect_transform.position.y + damege_label_pos_y_offset,
                rect_transform.position.z
            );

            DamageLabel label = go.GetComponent<DamageLabel>();
            label.id = damage_label.id;
            label.setDamage(damage);
            go.SetActive(true);
        }

        public static void miss(Vector3 world_position)
        {
            var damage_label = EffectFactory.create(EffectType.DamageLabel);
            GameObject go = damage_label.game_object;
            go.transform.SetParent(center_panel.transform);

            RectTransform rect_transform = damage_label.game_object.GetComponent<RectTransform>();
            rect_transform.position = RectTransformUtility.WorldToScreenPoint(main_camera, world_position);
            rect_transform.position = new Vector3(
                rect_transform.position.x,
                rect_transform.position.y + damege_label_pos_y_offset,
                rect_transform.position.z
            );

            DamageLabel label = go.GetComponent<DamageLabel>();
            label.setMiss();
            go.SetActive(true);
        }


        public static void lvup(Vector3 world_position)
        {
            var damage_label = EffectFactory.create(EffectType.DamageLabel);
            GameObject go = damage_label.game_object;
            go.transform.SetParent(center_panel.transform);

            RectTransform rect_transform = damage_label.game_object.GetComponent<RectTransform>();
            rect_transform.position = RectTransformUtility.WorldToScreenPoint(main_camera, world_position);
            rect_transform.position = new Vector3(
                rect_transform.position.x,
                rect_transform.position.y + damege_label_pos_y_offset,
                rect_transform.position.z
            );

            DamageLabel label = go.GetComponent<DamageLabel>();
            label.setLvUP();
            go.SetActive(true);
        }



        /*
            NextRoom Effect
        */
        public static void nextRoomOn()
        {
            //GameObject[] next_rooms = GameObject.FindGameObjectsWithTag("NextRoom");
            if (next_rooms == null || next_rooms.Count < 1) return;

            foreach (var t in next_rooms)
            {
                //Debug.Log("EffectService : nextRoomOn()");
                t.gameObject.SetActive(true);
            }
        }

        public static void nextRoomOff()
        {
            //GameObject[] next_rooms = GameObject.FindGameObjectsWithTag("NextRoom");
            if (next_rooms == null || next_rooms.Count < 1) return;

            foreach (var t in next_rooms)
            {
                //Debug.Log("EffectService : nextRoomOff()");
                t.gameObject.SetActive(false);
            }
        }

        public static void nextRoomClear()
        {
            //GameObject[] next_rooms = GameObject.FindGameObjectsWithTag("NextRoom");
            if (next_rooms == null || next_rooms.Count < 1) return;
            next_rooms.Clear();
        }
    }
}