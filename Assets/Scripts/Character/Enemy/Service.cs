using UnityEngine;

using System;
using System.Collections.Generic;

using UniRx;

using Random = UnityEngine.Random;

using GameStageService = GameStage.Service;

using SpawnerPoint = sgffu.FieldObject.Spawner.Point;
using SpawnerList = sgffu.FieldObject.Spawner.List;

using EnemyEntity = sgffu.Characters.Enemy.Entity;
using EnemyFactory = sgffu.Characters.Enemy.Factory;
using EnemyService = sgffu.Characters.Enemy.Service;

using Rank = GameStageRank.Rank;

namespace sgffu.Characters.Enemy {
    public class Service {

        public static int enemy_rank = 0;

        public static IDisposable spawn_disposable = null;

        public const int spawn_limit = 32;

        public static Dictionary<Rank, int> player_detect_interval = new Dictionary<Rank, int> {
            { Rank.D, 1024 },
            { Rank.C, 512 },
            { Rank.B, 256 },
            { Rank.A, 128 },
            { Rank.S, 64 },
        };

        public static void destroyEnemyDead()
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("EnemyDead");

            foreach (var go in gos) {
                GameObject.Destroy(go);
            }
        }

        public static void destroyEnemy()
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (var go in gos) {
                GameObject.Destroy(go);
            }
        }

        public static void startSpawn()
        {
            // ゲームステージサービスから敵の数もらう
            int enemy_num = GameStageService.status("enemy_num");

            // スポーン中なら現在のスポーンを中止
            stopSpawn();

            Debug.Log("EnemyService.startSpawn()");
/*
            // 1秒に1回、x回まで処理する（敵の出現数が上限を超えてたら待つ）
            spawn_disposable = Observable.Interval(System.TimeSpan.FromSeconds(1)).Take(enemy_num).Subscribe(y => {
                // ランクに合わせて敵を生成
                EnemyEntity enemy = EnemyService.create();
                // 左から出るか前から出るかを決定
                SpawnerPoint point = EnemyService.random_pos();
                // 敵オブジェクトに設定
                EnemyService.set_direction(enemy, point);
                // 出撃
                EnemyService.activate(enemy);
            });
*/
            int spawn_count = 0;

            // （敵の出現数が上限を超えてたら待つ）が出来ていないので修正必要
            // スポーンタイマーは基本的に無制限に動作するようにし、動作内で敵を上限まで出現させたかどうか判定させる
            // 出現済みで生きてる敵の数を数えて、出現上限に達していたら次まで待つ
            // 出現済みで生きてる敵の数が出現上限数以下で、かつ生成上限にまだ達していなければ敵を生成
            spawn_disposable = Observable.Interval(System.TimeSpan.FromSeconds(1)).Subscribe(x => {
                // 生成上限に達していたら終了
                if (enemy_num <= spawn_count) return;
                spawn_count += 1;
                
                // アクティブな敵が出現上限に達していたら終了
                if (spawn_limit <= GameObject.FindGameObjectsWithTag("Enemy").Length) return;

                // ランクに合わせて敵を生成
                EnemyEntity enemy = EnemyService.create();
                // 左から出るか前から出るかを決定
                SpawnerPoint point = EnemyService.random_pos();
                // 敵オブジェクトに設定
                EnemyService.set_direction(enemy, point);
                // 出撃
                EnemyService.activate(enemy);
            });


        }

        public static void stopSpawn()
        {
            if (spawn_disposable == null) return;
            Debug.Log("EnemyService.stopSpawn() : spawn_disposable.Dispose()");
            spawn_disposable.Dispose();
        }

        public static EnemyEntity create()
        {
            EnemyEntity enemy = EnemyFactory.createRandomEntity();
            return enemy;
        }

        public static SpawnerPoint random_pos()
        {
            return SpawnerList.all.RandomAt().Value;
        }

        public static void set_direction(EnemyEntity enemy, SpawnerPoint spawner)
        {
            var pos = new Vector3(spawner.position.x, enemy.game_object.transform.position.y, spawner.position.z);
            //enemy.game_object.transform.position = spawner.position;
            enemy.game_object.transform.position = pos;
            enemy.game_object.transform.Rotate(spawner.direction);
        }

        public static void activate(EnemyEntity enemy)
        {
            enemy.game_object.SetActive(true);
        }

        public static int get_detect_interval(Rank rank) {
            if (!player_detect_interval.ContainsKey(rank)) {
                throw new System.Exception("player_detect_interval is not contains key: " + rank);
            }

            return player_detect_interval[rank];
        }

        public static int remainig()
        {
            return GameObject.FindGameObjectsWithTag("Enemy").Length;
        }



        public static int getDamage(int at_atk, int de_def)
        {
            // 基準ダメージ値
            int damage = at_atk - de_def;

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
                int diff = de_def - at_atk;

                if (0 < diff) {
                    // 防御力が敵の攻撃力を上回っていたら、上回り分ダメージの発生確率を下げる
                    int lucky_hit = Random.Range(0, diff + 1);
                    if (lucky_hit == 1) return 1;
                }

                return Random.Range(0, 2);
            }

            return damage;
        }

    }
}
