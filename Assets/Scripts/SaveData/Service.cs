using System;
using UnityEngine;

using PlayerEntity = sgffu.Characters.Player.PlayerEntity;
using PlayerService = sgffu.Characters.Player.PlayerService;
using EquipEntity = Item.Equip.EquipEntity;
using EqiupService = Item.Equip.Service;
using GameStageService = GameStage.Service;
using StorageService = sgffu.Storage.Service;

namespace SaveData
{

    public class Service
    {

        private static int _use_slot = 0;

        private static int _use_slot_min = 1;
        private static int _use_slot_max = 3;

        public static int use_slot {
            get { return _use_slot; }
            set {
                if (value < _use_slot_min || _use_slot_max < value)
                    throw new Exception("Invalid Slot Number : " + value.ToString());

                _use_slot = value;
            }
        }



        private static string player_key = "Player";
        private static string equip_key = "Equip";
        private static string stage_key = "Stage";


        public static void savePlayer(int slot, PlayerEntity obj)
        {
            use_slot = slot;

            if (obj == null || obj.GetType() != typeof(PlayerEntity)) {
                throw new Exception("SaveData.Service.savePlayer : Invalid type, not PlayerEntity.");
            }

            Debug.Log("SaveDataService.savePlayer : slot : " + slot.ToString());
            //savePlayerEquip(slot, obj.equip);
            savePlayerEquip(use_slot, EqiupService.entity);
            StorageService.save<PlayerEntity>(slot, player_key, obj);
        }

        public static PlayerEntity loadPlayer(int slot)
        {
            use_slot = slot;
            Debug.Log("SaveDataService.loadPlayer : slot : " + slot.ToString());

            var player = StorageService.load<PlayerEntity>(slot, player_key);
            if (player == null) return null;

            player.equip = loadPlayerEquip(slot);
            return player;
        }



        public static void savePlayerEquip(int slot, EquipEntity obj)
        {
            use_slot = slot;

            if (obj.GetType() != typeof(EquipEntity)) {
                throw new Exception("SaveData.Service.savePlayerEquip : Invalid type, not EquipEntity.");
            }

            Debug.Log("SaveDataService.savePlayerEquip : slot : " + slot.ToString());
            Debug.Log("SaveDataService.savePlayerEquip : " + obj.name);
            StorageService.save<EquipEntity>(slot, equip_key, obj);
        }

        public static EquipEntity loadPlayerEquip(int slot)
        {
            use_slot = slot;
            Debug.Log("SaveDataService.loadPlayerEquip : slot : " + slot.ToString());
            var entity = StorageService.load<EquipEntity>(slot, equip_key);
            Debug.Log("SaveDataService.loadPlayerEquip : " + entity.name);
            EqiupService.entity = entity;
            return entity;
        }



        public static void saveStage(int slot, StageSaveData obj)
        {
            use_slot = slot;

            if (obj.GetType() != typeof(StageSaveData)) {
                throw new Exception("SaveData.Service.saveStage : Invalid type, not StageSaveData.");
            }

            Debug.Log("SaveDataService.saveStage : slot : " + slot.ToString());
            StorageService.save<StageSaveData>(slot, stage_key, obj);
        }

        public static StageSaveData loadStage(int slot)
        {
            use_slot = slot;
            Debug.Log("SaveDataService.loadStage : slot : " + slot.ToString());
            return StorageService.load<StageSaveData>(slot, stage_key);
        }



        public static void save()
        {
            Debug.Log("SaveDataService.save()");

            // 装備データ保存
            // savePlayerEquip(_use_slot, EqiupService.entity);

            // プレイヤーデータ保存
            savePlayer(use_slot, PlayerService.entity);

            // ステージデータ保存
            StageSaveData stage_save_data = new StageSaveData {
                room_clear_count = GameStageService.status("clear_count"),
                enemy_strength = GameStageService.status("enemy_strength"),
                enemy_num = GameStageService.status("enemy_num"),
            };

            saveStage(use_slot, stage_save_data);
        }

        public static Tuple<PlayerEntity, StageSaveData> load()
        {
            Debug.Log("SaveDataService.load()");
            // ステージデータ取得
            StageSaveData stage_save_data = loadStage(use_slot);

            return new Tuple<PlayerEntity, StageSaveData>(
                // プレイヤーデータ取得
                loadPlayer(use_slot),
                stage_save_data
            );
        }
    }
}