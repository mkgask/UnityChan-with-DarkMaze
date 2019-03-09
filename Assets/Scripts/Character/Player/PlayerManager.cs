using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using sgffu.EventMessage;
using PlayerFactory = sgffu.Characters.Player.PlayerFactory;
using PlayerService = sgffu.Characters.Player.PlayerService;
using LogService = sgffu.Log.Service;

public class PlayerManager : MonoBehaviour
{
/*
    void Awake()
    {
        //PlayerService.init();

        Observable.Zip(
            MessageBroker.Default.Receive<CreatedGameStageRank>(),
            MessageBroker.Default.Receive<CreatedEquipBaseResouces>(),
            MessageBroker.Default.Receive<InputInitialized>(),
            MessageBroker.Default.Receive<LogInitialized>(),
            Tuple.Create
        ).Subscribe(x => {
            LogService.write("PlayerManager::Awake()::Receive<3>");
            var equip_id = PlayerService.init();

            LogService.write("PlayerManager::Awake()::Publish<ChangeEquipEvent>");
            MessageBroker.Default.Publish<ChangeEquipEvent>(new ChangeEquipEvent {
                id = equip_id,
            });

            MessageBroker.Default.Publish<CreatedPlayerService>(new CreatedPlayerService());
        });
    }
*/

    [SerializeField] private GameObject player_prefab;

    void Start()
    {
        PlayerFactory.init(player_prefab);

/*
        var equip_id = PlayerService.init();
        
        MessageBroker.Default.Publish<ChangeEquipEvent>(new ChangeEquipEvent {
            id = equip_id,
        });
*/

        MessageBroker.Default.Publish<CreatedPlayerService>(new CreatedPlayerService());

/*
        Observable.Zip(
            MessageBroker.Default.Receive<CreatedGameStageRank>(),
            MessageBroker.Default.Receive<CreatedEquipBaseResouces>(),
            MessageBroker.Default.Receive<InputInitialized>(),
            MessageBroker.Default.Receive<LogInitialized>(),
            Tuple.Create
        ).Subscribe(x => {
            LogService.write("PlayerManager::Awake()::Receive<3>");
            var equip_id = PlayerService.init();

            LogService.write("PlayerManager::Awake()::Publish<ChangeEquipEvent>");
            MessageBroker.Default.Publish<ChangeEquipEvent>(new ChangeEquipEvent {
                id = equip_id,
            });

            MessageBroker.Default.Publish<CreatedPlayerService>(new CreatedPlayerService());
        });

        //LogService.write("PlayerManager::Start()::Publish<CreatedPlayerService>");
        //MessageBroker.Default.Publish<CreatedPlayerService>(new CreatedPlayerService());
*/
    }
}
