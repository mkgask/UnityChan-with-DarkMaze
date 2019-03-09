using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Rank = GameStageRank.Rank;
using Type = Item.Equip.Type;


namespace sgffu.Se
{
    using Exception = System.Exception;

    public class Service
    {

        private static Dictionary<string, AudioClip> se_list;

        private static AudioSource src;

        private static int play_count = 0;
        private static int play_max = 10;

        private static float play_volume_default = 0.5f;


        private static Dictionary<string, string> player_attack_se;

        private static List<string> player_attack_voice;

        private static List<string> player_damage_voice;

        // private static List<string> player_dead_voice;
        private static string player_dead_voice;


        public static void init(Dictionary<string, AudioClip> seList, AudioSource source)
        {
            se_list = seList;
            src = source;

            player_attack_voice = new List<string> {
                "univ0001",
                "univ0002",
                "univ1101",
            };

            player_damage_voice = new List<string> {
                "univ1091",
                "univ1092",
                "univ1094",
            };

            player_dead_voice = "univ1093";
/*
            player_dead_voice = new List<string> {
                "univ1093",
            };
*/
            player_attack_se = new Dictionary<string, string> {
                { "KnifeD", "Sword 1"},
                { "KnifeC", "Sword 2"},
                { "KnifeB", "Sword 3"},
                { "KnifeA", "Sword 4"},
                { "KnifeS", "Sword 5"},
                
                { "SwordD", "Sword 1"},
                { "SwordC", "Sword 2"},
                { "SwordB", "Sword 3"},
                { "SwordA", "Sword 4"},
                { "SwordS", "Sword 5"},
                
                { "MaceD", "Impact 1"},
                { "MaceC", "Impact 1"},
                { "MaceB", "Hammer 1"},
                { "MaceA", "Hammer 1"},
                { "MaceS", "Hammer 2"},

                { "HammerD", "Impact 1"},
                { "HammerC", "Impact 1"},
                { "HammerB", "Hammer 1"},
                { "HammerA", "Hammer 1"},
                { "HammerS", "Hammer 2"},

                { "StaffD", "Stick 2"},
                { "StaffC", "Stick 2"},
                { "StaffB", "Stick 2"},
                { "StaffA", "Stick 1"},
                { "StaffS", "Stick 1"},
                
                { "AxeD", "Axe 1"},
                { "AxeC", "Axe 1"},
                { "AxeB", "Axe 2"},
                { "AxeA", "Axe 4"},
                { "AxeS", "Axe 3"},
            };
        }

        public static void play(string se_name)
        {
            if (!se_list.ContainsKey(se_name)) {
                throw new Exception("SE not found: " + se_name);
            }

            if (play_max <= play_count) { return; }

            float length = se_list[se_name].length;
            play_count += 1;

            //Debug.Log("SE.PlayOneShot: " + se_name);
            src.PlayOneShot(se_list[se_name], play_volume_default);

            Observable.Timer(TimeSpan.FromMilliseconds((length * 1000f) + 100f)).Subscribe(x => {
                play_count -= 1;
                if (play_count < 0) play_count = 0;
            });

        }




        public static string getEnemyAttack(Rank rank)
        {
            switch (rank) {
                case Rank.D: return "Hit_Hurt1";
                case Rank.C: return "Hit_Hurt2";
                case Rank.B: return "Hit_Hurt3";
                case Rank.A: return "Hit_Hurt4";
                case Rank.S: return "Hit_Hurt5";
                default: return "Hit_Hurt1";
            }
        }



        public static string getPlayerAttack(Rank rank, Type type)
        {
            var key = type.ToString() + rank.ToString();

            if (!player_attack_se.ContainsKey(key)) {
                throw new Exception();
            }

            return player_attack_se[key];

        }

        public static string getPlayerAttackVoice()
        {
            return player_attack_voice.RandomAt();
        }

        public static string getPlayerDamageVoice()
        {
            return player_damage_voice.RandomAt();
        }

        public static string getPlayerDeadVoice()
        {
            //return player_dead_voice.RandomAt();
            return player_dead_voice;
        }


    }
}
