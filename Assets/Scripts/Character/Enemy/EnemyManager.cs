using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using sgffu.EventMessage;
using GameStageRank;
using GameStageService = GameStage.Service;
using EnemyFactory = sgffu.Characters.Enemy.Factory;
using EnemyEntity = sgffu.Characters.Enemy.Entity;
using EnemyService = sgffu.Characters.Enemy.Service;
using SpawnerPoint = sgffu.FieldObject.Spawner.Point;
using LogService = sgffu.Log.Service;

public class EnemyManager : MonoBehaviour
{

    public int enemy_max = 10;

    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();

    void Awake()
    {
        EnemyFactory.init(prefabs);
/*
        Observable.Zip(
            MessageBroker.Default.Receive<CreatedGameStageRank>(),
            MessageBroker.Default.Receive<CreatedEquipBaseResouces>(),
            MessageBroker.Default.Receive<LogInitialized>(),
            Tuple.Create
        ).Subscribe(x => {
            //LogService.write("EnemyManager::Awake()::Receive<3>");
            EnemyService.startSpawn();
        });
*/

/*
            // ゲームステージサービスから敵の数もらう
            int enemy_num = GameStageService.status("enemy_num");
            // 1秒に1回、x回まで処理する（敵の出現数が上限を超えてたら待つ）
            Observable.Interval(System.TimeSpan.FromSeconds(1)).Take(enemy_num).Subscribe(y => {
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
    }

    void Start()
    {
        EnemyService.startSpawn();
        MessageBroker.Default.Publish<EnemySpawnStart>(new EnemySpawnStart());
/*
        EnemyFactory.init(prefabs);

        Observable.Zip(
            MessageBroker.Default.Receive<CreatedGameStageRank>(),
            MessageBroker.Default.Receive<CreatedEquipBaseResouces>(),
            MessageBroker.Default.Receive<LogInitialized>(),
            Tuple.Create
        ).Subscribe(x => {
            LogService.write("EnemyManager::Awake()::Receive<3>");
            EnemyService.startSpawn();

            MessageBroker.Default.Publish<EnemySpawnStart>(new EnemySpawnStart());
        });
*/
    }

    void Update()
    {
        //MessageBroker.Default.Publish<EnemyServiceInitialized>();
    }

}
