using UnityEngine;
using UniRx;

using PlayerEntity = sgffu.Characters.Player.PlayerEntity;
using PlayerService = sgffu.Characters.Player.PlayerService;
using GameStageService = GameStage.Service;
using StorageService = sgffu.Storage.Service;
using SaveData;

using sgffu.EventMessage;
using SaveDataService = SaveData.Service;

public class SaveDataManager : MonoBehaviour
{

    void Start()
    {
        MessageBroker.Default.Receive<SaveDataSave>().Subscribe(x => {
            Debug.Log("MessageBroker.Default.Receive<SaveDataSave>().Subscribe()");
            SaveDataService.save();
            MessageBroker.Default.Publish<SaveEnd>(new SaveEnd {});

            // Saveされるタイミングは４つ
            // 敵を倒した時
            // 装備を入れ替えた時
            // クリアした時
            // 死んだ時
        });
/*
        ロードはSelectシーンで行われるが、Mazeシーン用なのでロード不要

        MessageBroker.Default.Receive<SaveDataLoad>().Subscribe(x => {
            SaveDataService.use_slot = x.slot;
            SaveDataService.load();
            // プレイヤーデータ取得（SelectSceneManagerで取ってるのでここでは不要）
            // （ではなく処理をこっちに移す必要があるでしょう）
            // PlayerEntity player_entity = StorageService.load<PlayerEntity>(x.slot);
            StageSaveData stage_save_data = StorageService.load<StageSaveData>(x.slot, "Stage");
            GameStageService.room_clear_count = stage_save_data.room_clear_count;
            GameStageService.enemy_strength = stage_save_data.enemy_strength;
            GameStageService.enemy_num = stage_save_data.enemy_num;
        });
*/
    }


}
