using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using sgffu.EventMessage;
using EffectiveFloorService = sgffu.EffectiveFloor.EffectiveFloorService;
using FieldObjectService = sgffu.FieldObject.FieldObjectService;
using PlayerService = sgffu.Characters.Player.PlayerService;
using EnemyService = sgffu.Characters.Enemy.Service;
using ItemEquipService = Item.Equip.Service;
using EffectService = Effect.Service;
using GuiService = Gui.Service;
using MessagePack;

namespace GameStage
{
    public class Service
    {
        private static int _room_clear_count = 0;

        private static int _enemy_strength = 1;

        private static int _enemy_num = 1;



        public static int room_clear_count {
            get { return _room_clear_count; }
            set { _room_clear_count = value; }
        }
    
        public static int enemy_strength {
            get { return _enemy_strength; }
            set { _enemy_strength = value; }
        }

        public static int enemy_num {
            get { return _enemy_num; }
            set { _enemy_num = value; }
        }


        private static int default_clear_add = 1;

        private static int game_over_room_adjust = -3;

        private static int game_over_strength_adjust = -4;

        private static int game_over_nums_adjust = -2;


        public static int status(string status_name)
        {
            switch(status_name) {
                case "clear_count": return _room_clear_count;
                case "enemy_strength": return _enemy_strength;
                case "enemy_num": return _enemy_num;
            }

            return 0;
        }


        async public static void game_over()
        {
            // プレイヤー操作禁止
            Time.timeScale = 0f;

            // 画面フェードを有効化（有効な間はuGUIのボタンが押せないので注意）
            GuiService.fadeEnable();

            // 画面を半フェード
            await GuiService.fadeBacground(0.5f);

            // ゲームオーバー表示
            GuiService.showGameOver();

            MessageBroker.Default.Publish<PlaySe>(new PlaySe{ name = "GameOver" });

            await in_fade(game_over_room_adjust, game_over_strength_adjust, game_over_nums_adjust);

            // 敵を全部消す
            EnemyService.destroyEnemy();

            // プレイヤー復帰
            Time.timeScale = 1f;
            await PlayerService.respawn();
            Time.timeScale = 0f;

            // GameOver表示クリア
            GuiService.hideGameOver();

            // 画面のフェードを戻す
            await GuiService.fadeBacground(0f);

            // 画面フェードを無効化
            GuiService.fadeDisable();

            // ドア開ける

            // 敵スポーン開始
            EnemyService.startSpawn();

            // プレイヤー操作再開
            Time.timeScale = 1f;
        }



        async public static void next(int add_strength, int add_num) 
        {
            // プレイヤー操作禁止
            Time.timeScale = 0f;

            // 画面フェードを有効化（有効な間はuGUIのボタンが押せないので注意）
            GuiService.fadeEnable();

            // 画面を半フェード
            await GuiService.fadeBacground(0.5f);

            // ステージクリア表示
            GuiService.showStageClear(_room_clear_count);

            MessageBroker.Default.Publish<PlaySe>(new PlaySe{ name = "RoomClear" });

            await in_fade(default_clear_add, add_strength, add_num);

            // RoomClear表示クリア
            GuiService.hideStageClear();

            // 画面のフェードを戻す
            await GuiService.fadeBacground(0f);

            // 画面フェードを無効化
            GuiService.fadeDisable();

            // ドア開ける

            // 敵スポーン開始
            EnemyService.startSpawn();

            // プレイヤー操作再開
            Time.timeScale = 1f;
        }


        public static IEnumerator in_fade(int add_clear_count, int add_strength, int add_num)
        {
            // 操作があるまで待つ
            while(!Input.GetMouseButtonUp(0) && !Input.GetKeyUp(KeyCode.Space) && !Input.GetKeyUp(KeyCode.Return)) {
                yield return null;
            }

            // 画面をフルフェード
            yield return GuiService.fadeBacground(1f);

            Time.timeScale = 1f;

            // プレイヤー再初期化
            PlayerService.reset();
            yield return new WaitForEndOfFrame();

            // BGMチェック
            MessageBroker.Default.Publish<BgmCheck>(new BgmCheck());
            yield return null;

            // 次のステージ情報更新
            _room_clear_count += add_clear_count;
            if (_room_clear_count < 1) _room_clear_count = 1;

            _enemy_strength += add_strength;
            if (_enemy_strength < 1) _enemy_strength = 1;

            _enemy_num += add_num;
            if (_enemy_num < 1) _enemy_num = 1;

            // ドア閉める

            // NextRoomのエフェクトを消去
            EffectService.nextRoomOff();
            yield return new WaitForEndOfFrame();

            // 地面に落ちてる装備品を削除
            ItemEquipService.ClearTarget();
            ItemEquipService.destroyEquipItem();
            yield return new WaitForEndOfFrame();

            // 家具配置範囲再初期化
            EffectiveFloorService.reset();

            // 家具再初期化
            FieldObjectService.reset();
            yield return new WaitForEndOfFrame();

            // 敵削除
            EnemyService.destroyEnemyDead();
            yield return new WaitForEndOfFrame();

            // リセット後の状態を保存
            MessageBroker.Default.Publish<SaveDataSave>(new SaveDataSave());
            yield return new WaitForEndOfFrame();

            Time.timeScale = 0f;
        }

    }
}