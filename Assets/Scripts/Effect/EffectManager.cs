using System.Collections.Generic;
using UnityEngine;
using UniRx;
using EffectFactory = Effect.Factory;
using EffectService = Effect.Service;
using EnemyService = sgffu.Characters.Enemy.Service;
using sgffu.EventMessage;

namespace Effect
{
    class EffectManager : MonoBehaviour
    {
        private ReactiveProperty<int> enemy_remaining = new ReactiveProperty<int>();

        private bool enemy_remaining_enabled = false;

        [SerializeField] private List<GameObject> prefabs = new List<GameObject>();

        private bool initialized = false;

        void Awake()
        {
            EffectFactory.init(prefabs);

            MessageBroker.Default.Receive<EnemySpawnStart>().Subscribe(_ => {
                initialized = true;
            });
        }

        void Start()
        {
            EffectService.init();
            //EffectService.nextRoomOff();

            enemy_remaining.Where(x => 0 < x).Subscribe(x => {
                enemy_remaining_enabled = true;
            });

            enemy_remaining.Where(x => x == 0 && enemy_remaining_enabled).Subscribe(x => {
                EffectService.nextRoomOn();
            });
/*
            enemy_remaining.Subscribe(remain => {
                if (0 < remain) { EffectService.nextRoomOff(); } else { EffectService.nextRoomOn(); }
            });
*/
            MessageBroker.Default.Publish<EffectInitialized>(new EffectInitialized());
        }

        void Update()
        {
            if (!initialized) return;
            enemy_remaining.Value = EnemyService.remainig();
            //Debug.Log("enemy_remaining.Value" + enemy_remaining.Value.ToString());
        }
    }
}