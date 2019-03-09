using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using sgffu.EventMessage;
using sgffu.FieldObject;
using sgffu.ObjectPool;
//using MessageService = sgffu.Message.Service;
using LogService = sgffu.Log.Service;

public class FieldObjectManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();

    void Awake()
    {
        FieldObjectEntityFactory.init(prefabs);
        FieldObjectService.init();
/*
        Observable.Zip(
            //MessageBroker.Default.Receive<CreatedEffectiveFloor>(),
            //MessageBroker.Default.Receive<CreatedGameStageRank>(),
            //MessageBroker.Default.Receive<CreatedEquipBaseResouces>(),
            //MessageBroker.Default.Receive<CreatedFieldObject>(),
            MessageBroker.Default.Receive<CreatedPlayerService>(),
            //MessageBroker.Default.Receive<LogInitialized>(),
            Tuple.Create
        ).Subscribe(x => {
            //LogService.write("FieldObjectManager::Awake()::Receive<6>");
            //FieldObjectService.init(x.Item5.player_service);
            FieldObjectService.createRandomEntities();

            MessageBroker.Default.Publish<CreatedFieldObject>(new CreatedFieldObject());
        });
*/
        //FieldObjectService.reset();

        MessageBroker.Default.Receive<CreatedEffectiveFloor>().Subscribe(x => {
            Debug.Log("MessageBroker.Default.Receive<CreatedPlayerService>().Subscribe()");
            //FieldObjectService.createRandomEntities();
            FieldObjectService.reset();
            MessageBroker.Default.Publish<CreatedFieldObject>(new CreatedFieldObject());
        });
    }

/*
    void Start()
    {
        FieldObjectEntityFactory.init(prefabs);
        FieldObjectService.init();

        Observable.Zip(
            MessageBroker.Default.Receive<CreatedEffectiveFloor>(),
            MessageBroker.Default.Receive<CreatedGameStageRank>(),
            MessageBroker.Default.Receive<CreatedEquipBaseResouces>(),
            MessageBroker.Default.Receive<CreatedFieldObject>(),
            MessageBroker.Default.Receive<CreatedPlayerService>(),
            MessageBroker.Default.Receive<LogInitialized>(),
            Tuple.Create
        ).Subscribe(x => {
            LogService.write("FieldObjectManager::Awake()::Receive<6>");
            //FieldObjectService.init(x.Item5.player_service);
            FieldObjectService.createRandomEntities();

            MessageBroker.Default.Publish<CreatedFieldObject>(new CreatedFieldObject());
        });

        //LogService.write("FieldObjectManager::Start()::Publish<CreatedFieldObject>");
        //MessageBroker.Default.Publish<CreatedFieldObject>(new CreatedFieldObject());
    }
*/

}
