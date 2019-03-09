using System.Collections;
using UnityEngine;
using UniRx;
using GameStageRank;
using sgffu.EventMessage;
//using MessageService = sgffu.Message.Service;
using LogService = sgffu.Log.Service;

public class GameStageRankManager : MonoBehaviour
{
    void Awake()
    {
        GameStageRankService.init();
    }

    void Start()
    {
        //GameStageRankService.init();
        //LogService.write("GameStageRankManager::Start()::Publish<CreatedGameStageRank>");
        MessageBroker.Default.Publish<CreatedGameStageRank>(new CreatedGameStageRank());
    }

}
