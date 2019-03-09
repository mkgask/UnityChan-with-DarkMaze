using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using StrOpe = StringOperationUtil.OptimizedStringOperation;

using sgffu.Characters.Player;
using GameStageService = GameStage.Service;
using EquipService = Item.Equip.Service;
using GuiState = Gui.State;


namespace Gui
{

    public class Service
    {
        private static GameObject parent;

        private static Transform fade_background;

        private static Transform room_clear;

        private static TMP_Text room_clear_text;

        private static Transform game_over;

        // private static TMP_Text game_over_text;

        private static Dictionary<string, Transform> ui_block;

        private static Dictionary<string, TextMeshProUGUI> ui_text;
        private static Dictionary<string, Slider> ui_bar;

        private static GuiState _state = GuiState.Title;


        public static void test() {}

        public static void init(GameObject go)
        //public static void init(GuiManager gui_manager)
        {
            Transform parent_transform = go.transform;
            //Transform parent_transform = gui_manager.gameObject.transform;

            ui_block = new Dictionary<string, Transform> {
                { "LeftTop", parent_transform.Find("LeftTop") },
                { "RightTop", parent_transform.Find("RightTop") },
                { "LeftBottom", parent_transform.Find("LeftBottom") },
                { "RightBottom", parent_transform.Find("RightBottom") },
            };

            ui_text = new Dictionary<string, TextMeshProUGUI> {
                { "Lv", ui_block["LeftTop"].Find("Lv").GetComponent<TextMeshProUGUI>() },
                { "HP", ui_block["LeftTop"].Find("HP").GetComponent<TextMeshProUGUI>() },

                { "RoomClear", ui_block["RightTop"].Find("RoomClear").GetComponent<TextMeshProUGUI>() },
                { "EnemyStrength", ui_block["RightTop"].Find("EnemyStrength").GetComponent<TextMeshProUGUI>() },
                { "EnemyNum", ui_block["RightTop"].Find("EnemyNum").GetComponent<TextMeshProUGUI>() },
                { "LabelRoomClear", ui_block["RightTop"].Find("LabelRoomClear").GetComponent<TextMeshProUGUI>() },
                { "LabelEnemyStrength", ui_block["RightTop"].Find("LabelEnemyStrength").GetComponent<TextMeshProUGUI>() },
                { "LabelEnemyNum", ui_block["RightTop"].Find("LabelEnemyNum").GetComponent<TextMeshProUGUI>() },

                { "PlayerAtk", ui_block["LeftBottom"].Find("Atk").GetComponent<TextMeshProUGUI>() },
                { "PlayerDef", ui_block["LeftBottom"].Find("Def").GetComponent<TextMeshProUGUI>() },
                { "PlayerSpd", ui_block["LeftBottom"].Find("Spd").GetComponent<TextMeshProUGUI>() },

                { "WeaponName", ui_block["RightBottom"].Find("WeaponName").GetComponent<TextMeshProUGUI>() },
                { "WeaponAtkTotal", ui_block["RightBottom"].Find("WeaponAtkTotal").GetComponent<TextMeshProUGUI>() },
                { "WeaponAtkBaseOp", ui_block["RightBottom"].Find("WeaponAtkBaseOp").GetComponent<TextMeshProUGUI>() },
                { "WeaponDefTotal", ui_block["RightBottom"].Find("WeaponDefTotal").GetComponent<TextMeshProUGUI>() },
                { "WeaponDefBaseOp", ui_block["RightBottom"].Find("WeaponDefBaseOp").GetComponent<TextMeshProUGUI>() },
                { "WeaponSpdTotal", ui_block["RightBottom"].Find("WeaponSpdTotal").GetComponent<TextMeshProUGUI>() },
                { "WeaponSpdBaseOp", ui_block["RightBottom"].Find("WeaponSpdBaseOp").GetComponent<TextMeshProUGUI>() },
            };

            ui_bar = new Dictionary<string, Slider> {
                { "ExpBar", ui_block["LeftTop"].Find("ExpBar").GetComponent<Slider>() },
                { "HPBar", ui_block["LeftTop"].Find("HPBar").GetComponent<Slider>() },
            };

            //Debug.Log("GuiService.init: ui_bar[HPBar]" + ui_bar["HPBar"].name);
            //Debug.Log("GuiService.init: ui_bar[ExpBar]" + ui_bar["ExpBar"].name);
            //Debug.Log("ui_text[Lv]" + ui_text["Lv"]);

            fade_background = parent_transform.Find("CenterPanel/FadeBackground");

            room_clear = parent_transform.Find("CenterPanel/RoomClear");
            room_clear_text = parent_transform.Find("CenterPanel/RoomClear/RoomClearText").GetComponent<TextMeshProUGUI>();
            //Debug.Log("room_clear_text: "+ room_clear_text.name);
            
            game_over = parent_transform.Find("CenterPanel/GameOver");
            //room_clear_text = parent_transform.Find("CenterPanel/GameOver/GameOver").GetComponent<TextMeshProUGUI>();
        }



        public static void modeChange(GuiState state)
        {
            _state = state;

            switch (state) {
                case GuiState.Title:
                    ui_block["LeftTop"].gameObject.SetActive(false);
                    ui_text["RoomClear"].gameObject.SetActive(false);
                    ui_text["EnemyStrength"].gameObject.SetActive(false);
                    ui_text["EnemyNum"].gameObject.SetActive(false);
                    ui_text["LabelRoomClear"].gameObject.SetActive(false);
                    ui_text["LabelEnemyStrength"].gameObject.SetActive(false);
                    ui_text["LabelEnemyNum"].gameObject.SetActive(false);
                    ui_block["LeftBottom"].gameObject.SetActive(false);
                    ui_block["RightBottom"].gameObject.SetActive(false);
                    break;
                case GuiState.Room:
                    ui_block["LeftTop"].gameObject.SetActive(true);
                    ui_text["RoomClear"].gameObject.SetActive(true);
                    ui_text["EnemyStrength"].gameObject.SetActive(true);
                    ui_text["EnemyNum"].gameObject.SetActive(true);
                    ui_text["LabelRoomClear"].gameObject.SetActive(true);
                    ui_text["LabelEnemyStrength"].gameObject.SetActive(true);
                    ui_text["LabelEnemyNum"].gameObject.SetActive(true);
                    ui_block["LeftBottom"].gameObject.SetActive(true);
                    ui_block["RightBottom"].gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }



        public static IEnumerator fadeBacground(float target, float scale = 0.01f)
        {
            var image = fade_background.GetComponent<Image>();
            var base_color = image.color;
            var fade_from = base_color.a;
            yield return null;

            if (fade_from != target) {
                if (fade_from < target) {
                    for (var f = fade_from; f < target; f += scale) {
                        base_color.a = f;
                        image.color = base_color;
                        yield return null;
                    }
                } else {
                    for (var f = fade_from; target < f; f -= scale) {
                        base_color.a = f;
                        image.color = base_color;
                        yield return null;
                    }
                }
            }
        }

        public static void fadeEnable()
        {
            fade_background.gameObject.SetActive(true);
        }

        public static void fadeDisable()
        {
            fade_background.gameObject.SetActive(false);
        }

/*
        public static void backgroundFade(float rate, float time = 0.5f)
        {
            var image = fade_background.GetComponent<Image>();
            image.color = new Color(0, 0, 0, rate);
        }
*/


        public static void update()
        {
            if (_state != GuiState.Room) return;

            // *左上*
            // Lv表示
            string lv_str = PlayerService.statuString("Lv");
            ui_text["Lv"].text = lv_str;

            // 経験値バー
            int lv = PlayerService.status("Lv");
            int exp = PlayerService.now_exp();
            int next_level = PlayerService.next_exp();
            //Debug.Log("exp: " + exp.ToString());
            //Debug.Log("next_level: " + next_level.ToString());
            ui_bar["ExpBar"].value = exp;
            ui_bar["ExpBar"].maxValue = next_level;
            /*
            ui_bar["ExpBar"].value = PlayerService.status("Exp");
            ui_bar["ExpBar"].maxValue = PlayerService.nextLevel(lv);
            */

            // HP表示
            string hp_str = PlayerService.statuString("HP");
            string max_hp_str = PlayerService.statuString("MaxHP");
            ui_text["HP"].text = (hp_str + " / " + max_hp_str);

            // HPバー
            //int hp = PlayerService.status("HP");
            //int max_hp = PlayerService.status("MaxHP");
            ui_bar["HPBar"].value = PlayerService.status("HP");
            ui_bar["HPBar"].maxValue = PlayerService.status("MaxHP");

            // *右上*
            // クリアした部屋の数
            ui_text["RoomClear"].text = GameStageService.status("clear_count").ToString();
            // 敵の強さ
            ui_text["EnemyStrength"].text = GameStageService.status("enemy_strength").ToString();
            // 敵数
            ui_text["EnemyNum"].text = GameStageService.status("enemy_num").ToString();

            // *左下*
            int p_atk = PlayerService.status("Atk");
            int p_def = PlayerService.status("Def");
            int p_spd = PlayerService.status("Spd");
            ui_text["PlayerAtk"].text = p_atk.ToString();
            ui_text["PlayerDef"].text = p_def.ToString();
            ui_text["PlayerSpd"].text = p_spd.ToString();

            // *右下*
            int atk = EquipService.status("Atk");
            int def = EquipService.status("Def");
            int spd = EquipService.status("Spd");
            int atk_op = EquipService.status("AtkOp");
            int def_op = EquipService.status("DefOp");
            int spd_op = EquipService.status("SpdOp");

            ui_text["WeaponName"].text = EquipService.weapon_name();
            ui_text["WeaponAtkTotal"].text = (atk + atk_op).ToString();
            ui_text["WeaponDefTotal"].text = (def + def_op).ToString();
            ui_text["WeaponSpdTotal"].text = (spd + spd_op).ToString();
            ui_text["WeaponAtkBaseOp"].text = $"{atk} + {atk_op}";
            ui_text["WeaponDefBaseOp"].text = $"{def} + {def_op}";
            ui_text["WeaponSpdBaseOp"].text = $"{spd} + {spd_op}";
        }

        public static void display()
        {
            foreach(KeyValuePair<string, Transform> pair in ui_block) {
                pair.Value.gameObject.SetActive(true);
            }
        }

        public static void unDisplay()
        {
            foreach(KeyValuePair<string, Transform> pair in ui_block) {
                pair.Value.gameObject.SetActive(false);
            }
        }



        public static void showStageClear(int clearCount)
        {
            room_clear_text.text = StrOpe.i + "room #" + (clearCount + 1).ToString() + " clear";
            //room_clear_text.text = string.Format("room #{0} clear", clearCount.ToString());
            room_clear.gameObject.SetActive(true);
        }

        public static void hideStageClear()
        {
            room_clear.gameObject.SetActive(false);
        }



        public static void showGameOver()
        {
            //game_over_text.text = "Game Over";
            game_over.gameObject.SetActive(true);
        }

        public static void hideGameOver()
        {
            game_over.gameObject.SetActive(false);
        }

    }

}