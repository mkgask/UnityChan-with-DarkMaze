using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using UniRx;

using Item.Equip;
using sgffu.EventMessage;

using Random = UnityEngine.Random;

using EquipEntity = Item.Equip.EquipEntity;
using EquipFactory = Item.Equip.Factory;
using EquipService = Item.Equip.Service;

using EffectService = Effect.Service;

using GameStageService = GameStage.Service;



namespace sgffu.Characters.Player
{

    public class PlayerService
    {
        private static PlayerEntity _entity;

        public static PlayerEntity entity {
            get { return _entity; }
        }

        private static GameObject go;

        private static float next_level_offset = 1.0f;

        private static float next_level_scale = 1.0f;

        private static Dictionary<int, int> exp_table = new Dictionary<int, int>();

        public static int init(GameObject game_object = null, PlayerEntity player_entity = null)
        {
            // プレイヤーGameObject取得
            if (game_object != null) {
                go = game_object;
            } else {
                //Debug.Log("PlayerService.init : PlayerFactory.createGameObject()");
                go = PlayerFactory.createGameObject();
            }

            // Entity生成
            if (player_entity != null) {
                _entity = player_entity;
                return _entity.equip.id;
            } else {
                //Debug.Log("PlayerService.init : PlayerFactory.createEntity()");
                _entity = PlayerFactory.createEntity();
            }

            // Lv 1以上のデータなら next_level更新
            if (1 < _entity.lv) {
                nextLevel(_entity.lv - 1);
                nextLevel(_entity.lv);
                nextLevel(_entity.lv + 1);
            }


            EquipEntity equip = _entity.equip;

            if (equip == null) {
                // 初期装備
                //Debug.Log("PlayerService.init : EquipFactory.createRandomEntity()");
                equip = EquipFactory.createRandomEntity();
                //Debug.Log("equip.name : " + equip.name);
                ItemEquipController iec = equip.game_object.GetComponentInChildren<ItemEquipController>();
                iec.OnTarget();
            } else {
                // EquipEntityの通りに武器を作ってIDを再設定
            }

            //nextLevel(1);
            //nextLevel(2);
            return equip.id;
        }

        public static void reset()
        {
            //Debug.Log("PlayerService.reset()");

            _entity.hp = _entity.hp_max;
            go.GetComponent<PlayerController>().positionReset();
        }



        public static IEnumerator respawn()
        {
            //Debug.Log("PlayerService.respawn()");

            reset();
            yield return new WaitForEndOfFrame();
            go.GetComponent<PlayerController>().respawn();
            yield return new WaitForSeconds(1);
        }



        public static void game_over()
        {
            Observable.Timer(TimeSpan.FromSeconds(5)).Subscribe(x => {
                GameStageService.game_over();
            });
        }



        /*
            Position
        */
        public static Vector3 position()
        {
            return go.transform.position;
        }



        /*
            Status
        */

        public static int status(string status_name)
        {
            switch(status_name) {
                case "Lv": return _entity.lv;
                case "Exp": return _entity.exp;
                case "HP": return _entity.hp;
                case "MaxHP": return _entity.hp_max;
                case "Atk": return _entity.atk;
                case "Def": return _entity.def;
                case "Spd": return _entity.spd;
                default: break;
            }

            return 0;
        }

        public static string statuString(string status_name)
        {
            return status(status_name).ToString();
        }

        public static int atk_total()
        {
            int atk = _entity.atk + EquipService.status("AtkTotal");
            return atk + Mathf.RoundToInt(Random.Range(atk * -0.1f, atk * 0.1f));
/*
            int rand = 0;

            while (rand == 0) {
                rand = Random.Range(-1, 1);
            }

            return atk + (Mathf.RoundToInt(Random.Range(atk * 0.01f, atk * 0.09f)) * rand);
*/
        }

        public static int def_total()
        {
            return _entity.def + EquipService.status("DefTotal");
        }

        public static int spd_total()
        {
            return _entity.spd + EquipService.status("SpdTotal");
        }



        /*
            Lv
        */

        public static int lv()
        {
            if (_entity == null) return 1;
            return _entity.lv;
        }


        public static int nextLevel(int lv)
        {
            if (exp_table.ContainsKey(lv)) return exp_table[lv];
            //return Mathf.RoundToInt(Mathf.Floor(lv * Mathf.Pow(2, Mathf.Sqrt(lv)) / 2 * next_level_scale));
            var exp = Mathf.RoundToInt(Mathf.Floor(lv * Mathf.Pow(2, Mathf.Sqrt(lv)) / 2 * next_level_scale));
            exp_table.Add(lv, exp);
            return exp;
        }

        public static int next_exp()
        {
            if (exp_table.ContainsKey(_entity.lv - 2)) exp_table.Remove(_entity.lv - 2);
            if (!exp_table.ContainsKey(_entity.lv - 1)) return 1;
            if (!exp_table.ContainsKey(_entity.lv)) nextLevel(_entity.lv);
            return exp_table[_entity.lv] - exp_table[_entity.lv - 1];
        }

        public static int now_exp()
        {
            if (!exp_table.ContainsKey(_entity.lv - 1)) return 0;
            return _entity.exp - exp_table[_entity.lv - 1];
        }


        public static void add_exp(int exp)
        {
            _entity.exp += exp;

            //Debug.Log("PlayerService : add_exp : " + exp.ToString());
            //Debug.Log("PlayerService : _entity.exp : " + _entity.exp.ToString());
            //Debug.Log("PlayerService : nextLevel : " + nextLevel(_entity.lv).ToString());

            if (nextLevel(_entity.lv) <= _entity.exp) {
                lvup();

                Observable.Timer(TimeSpan.FromMilliseconds(500)).Subscribe(x => {
                    add_exp(0);
                });
            }
        }

        public static void lvup()
        {
            var hp_up = Mathf.RoundToInt(Random.Range(0f, 2f));

            _entity.lv += 1;
            _entity.hp += hp_up;
            _entity.hp_max += hp_up;
            _entity.atk += Mathf.RoundToInt(Random.Range(0f, 2f));
            _entity.def += Mathf.RoundToInt(Random.Range(0f, 2f));
            _entity.spd += Mathf.RoundToInt(Random.Range(0f, 2f));

            //Debug.Log("PlayerService : Lvup : " + _entity.lv.ToString());

            nextLevel(_entity.lv);
            OnLvUP();
            MessageBroker.Default.Publish<PlaySe>(new PlaySe{ name = "LvUP" });
        }



        /*
            Battle
        */

        public static int getDamage(int enemy_atk)
        {
            // 基準ダメージ値
            int damage = enemy_atk - def_total();

            // ダメージ変動
            int damage_variation = Mathf.RoundToInt(Random.Range(0f, (damage * 0.05f) + 1f));

            // 変動を加味した実ダメージ値
            if (Random.Range(0, 2) < 1) {
                damage += damage_variation;
            } else {
                damage -= damage_variation;
            }

            // ダメージがゼロまたはマイナスなら、確率で1ダメージ発生
            if (damage < 1) {
                int diff = _entity.def - enemy_atk;

                if (0 < diff) {
                    // 防御力が敵の攻撃力を上回っていたら、上回り分ダメージの発生確率を下げる
                    int lucky_hit = Random.Range(0, diff + 1);
                    if (lucky_hit == 1) return 1;
                }

                return Random.Range(0, 2);
            }

            return damage;
        }

        public static int OnDamage(int damage)
        {
            var remain = _entity.hp -= damage;
            //Debug.Log("remain: " + remain);

            if (remain < 1) { _entity.hp = 0; }
            return _entity.hp;
        }


/*
        public static int OnDamage(int enemy_atk)
        {
            int damage = enemy_atk - (_entity.def + def_total());
            //Debug.Log("enemy_atk: " + enemy_atk);
            //Debug.Log("_entity.def: " + _entity.def);
            //Debug.Log("def_total(): " + def_total());
            //Debug.Log("damage: " + damage);

            if (damage < 1) {
                damage = Mathf.RoundToInt(Random.Range(0f, 1f));
            }
            //Debug.Log("damage: " + damage);

            EffectService.damage(damage, go.transform.position);
            var remain = _entity.hp -= damage;
            //Debug.Log("remain: " + remain);

            if (remain < 1) { _entity.hp = 0; }
            return _entity.hp;
        }
*/
        public static void OnMiss()
        {
            go.SendMessage("OnMiss");
        }

        public static void OnEnemyDead()
        {
            go.SendMessage("OnEnemyDead");
        }

        public static void OnLvUP()
        {
            go.SendMessage("OnLvUP");
        }

        /*
            Equip
        */
/*
        public static void changeEquip()
        {
        }
*/
    }
}
