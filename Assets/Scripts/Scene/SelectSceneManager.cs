using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.UI;
using UniRx;
using TMPro;

using PlayerService = sgffu.Characters.Player.PlayerService;
using PlayerEntity = sgffu.Characters.Player.PlayerEntity;
using PlayerFactory = sgffu.Characters.Player.PlayerFactory;
using FieldObjectService = sgffu.FieldObject.FieldObjectService;
using GameStageService = GameStage.Service;
using StorageService = sgffu.Storage.Service;
using SaveDataService = SaveData.Service;
using EquipFactory = Item.Equip.Factory;
using EquipService = Item.Equip.Service;
using sgffu.EventMessage;
using scenes;
using GuiState = Gui.State;

public class SelectSceneManager : MonoBehaviour
{

    private Vector3 player_default_position = new Vector3(
        1.5f, 0.5f, -3.5f
    );

    public GameObject dialog;
    public Button dialog_yes_button;
    public Button dialog_no_button;
    public TextMeshProUGUI dialog_text;



    public Vector3 s1_position;
    public Vector3 s1_rotation;
    private PlayerEntity s1p;
    private GameObject s1go;

    public GameObject s1_new_button;
    public GameObject s1_load_button;
    public GameObject s1_del_button;

    public Vector3 s2_position;
    public Vector3 s2_rotation;
    private PlayerEntity s2p;
    private GameObject s2go;

    public GameObject s2_new_button;
    public GameObject s2_load_button;
    public GameObject s2_del_button;

    public Vector3 s3_position;
    public Vector3 s3_rotation;
    private PlayerEntity s3p;
    private GameObject s3go;
    
    public GameObject s3_new_button;
    public GameObject s3_load_button;
    public GameObject s3_del_button;


    public int room_clear_count_default = 0;
    public int enemy_strength_default = 1;
    public int enemy_num_default = 1;

    public void Start()
    {
        // セーブデータ3つ読み込み
        s1p = SaveDataService.loadPlayer(1);
        s2p = SaveDataService.loadPlayer(2);
        s3p = SaveDataService.loadPlayer(3);
/*
        s1p = StorageService.load<PlayerEntity>(1, "Player");
        s2p = StorageService.load<PlayerEntity>(2, "Player");
        s3p = StorageService.load<PlayerEntity>(3, "Player");
*/
        // スロット1処理（データあればキャラ表示、ボタン表示変更）
        if (s1p != null) {
            // データあるのでキャラ表示、ボタン表示変更
            Debug.Log("SelectSceneManager.Start : s1p exist.");
            Debug.Log("SelectSceneManager.Start : s1p : " + s1p.ToString());
            Debug.Log("SelectSceneManager.Start : s1p.equip : " + s1p.equip.name);
            Debug.Log("SelectSceneManager.Start : s1p.lv : " + s1p.lv);
            Debug.Log("SelectSceneManager.Start : s1p.exp : " + s1p.exp);

            s1p.equip = EquipFactory.createEntity(s1p.equip);
            var s1e = EquipFactory.createGameObject(s1p.equip, Vector3.zero);
            s1e.GetComponentInChildren<BoxCollider>().enabled = false;
            s1e.GetComponentInChildren<ItemEquipController>().enabled = false;
            Debug.Log("SelectSceneManager.Start : s1e : " + s1e.name);

            s1go = PlayerFactory.createGameObject();
            //Debug.Log("SelectSceneManager.Start : s1go.transform.SetPositionAndRotation : " + s1_position.ToString());
            s1go.transform.SetPositionAndRotation(s1_position, Quaternion.Euler(s1_rotation));
            s1go.GetComponent<Animator>().SetInteger("hp", 10);
            s1go.GetComponent<Animator>().enabled = true;
            s1go.GetComponent<BoxCollider>().enabled = false;
            s1go.GetComponent<NavMeshAgent>().enabled = false;
            s1go.GetComponent<PlayerController>().enabled = false;

            Debug.Log("MessageBroker.Default.Publish<EquipEvent>() : s1");
            MessageBroker.Default.Publish<EquipEvent>(new EquipEvent {
                id = s1p.equip.id,
                go = s1go,
                update_service_data = false,
            });

            s1_new_button.SetActive(false);
            s1_load_button.SetActive(true);
            s1_del_button.SetActive(true);

        } else {
            // データなしはそのまま
        }

        // スロット2処理（データあればキャラ表示、ボタン表示変更）
        if (s2p != null) {
            // データあるのでキャラ表示、ボタン表示変更
            Debug.Log("SelectSceneManager.Start : s2p exist.");
            Debug.Log("SelectSceneManager.Start : s2p : " + s2p.ToString());
            Debug.Log("SelectSceneManager.Start : s2p.equip : " + s2p.equip.name);

            s2p.equip = EquipFactory.createEntity(s2p.equip);
            var s2e = EquipFactory.createGameObject(s2p.equip, Vector3.zero);
            s2e.GetComponentInChildren<BoxCollider>().enabled = false;
            s2e.GetComponentInChildren<ItemEquipController>().enabled = false;
            Debug.Log("SelectSceneManager.Start : s2e : " + s2e.name);

            s2go = PlayerFactory.createGameObject();
            //Debug.Log("SelectSceneManager.Start : s2go.transform.SetPositionAndRotation : " + s2_position.ToString());
            s2go.transform.SetPositionAndRotation(s2_position, Quaternion.Euler(s2_rotation));
            s2go.GetComponent<Animator>().SetInteger("hp", 10);
            s2go.GetComponent<Animator>().enabled = true;
            s2go.GetComponent<BoxCollider>().enabled = false;
            s2go.GetComponent<NavMeshAgent>().enabled = false;
            s2go.GetComponent<PlayerController>().enabled = false;

            Debug.Log("MessageBroker.Default.Publish<EquipEvent>() : s2");
            MessageBroker.Default.Publish<EquipEvent>(new EquipEvent {
                id = s2p.equip.id,
                go = s2go,
                update_service_data = false,
            });

            s2_new_button.SetActive(false);
            s2_load_button.SetActive(true);
            s2_del_button.SetActive(true);
        } else {
            // データなしはそのまま
        }

        // スロット3処理（データあればキャラ表示、ボタン表示変更）
        if (s3p != null) {
            // データあるのでキャラ表示、ボタン表示変更
            Debug.Log("SelectSceneManager.Start : s3p exist.");

            s3p.equip = EquipFactory.createEntity(s3p.equip);
            var s3e = EquipFactory.createGameObject(s3p.equip, Vector3.zero);
            s3e.GetComponentInChildren<BoxCollider>().enabled = false;
            s3e.GetComponentInChildren<ItemEquipController>().enabled = false;
            Debug.Log("SelectSceneManager.Start : s3e : " + s3e.name);

            s3go = PlayerFactory.createGameObject();
            s3go.transform.SetPositionAndRotation(s3_position, Quaternion.Euler(s3_rotation));
            s3go.GetComponent<Animator>().SetInteger("hp", 10);
            s3go.GetComponent<Animator>().enabled = true;
            s3go.GetComponent<BoxCollider>().enabled = false;
            s3go.GetComponent<NavMeshAgent>().enabled = false;
            s3go.GetComponent<PlayerController>().enabled = false;

            MessageBroker.Default.Publish<EquipEvent>(new EquipEvent {
                id = s3p.equip.id,
                go = s3go,
                update_service_data = false,
            });

            s3_new_button.SetActive(false);
            s3_load_button.SetActive(true);
            s3_del_button.SetActive(true);
        } else {
            // データなしはそのまま
        }
    }



    private void loadGame(
        int slot,
        GameObject player_game_object = null,
        PlayerEntity player_entity = null
    ) {
        SaveDataService.use_slot = slot;
        //var game_stage_service = StorageService.load<GameStageService>(slot, "Stage");
        var stage_save_data = SaveDataService.loadStage(slot);

        if (player_game_object != null &&
            player_entity != null &&
            stage_save_data != null
        ) {
            DontDestroyOnLoad(player_game_object);

            MessageBroker.Default.Publish<SceneChangeSimple>(new SceneChangeSimple {
                add_scene = "Maze",
                del_scene = "Select",
                active_scene = "Maze",
                post_action = () => {
                    SceneService.EnableAllGameObjects("Maze", new List<string>());

                    // キャラデータを新シーンで展開
                    UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(
                        player_game_object,
                        UnityEngine.SceneManagement.SceneManager.GetSceneByName("Maze")
                    );

                    Debug.Log("SelectSceneManager.loadGame()");
                    Debug.Log("SelectSceneManager.loadGame() : player_entity.lv : " + player_entity.lv);
                    Debug.Log("SelectSceneManager.loadGame() : player_entity.exp : " + player_entity.exp);
                    PlayerService.nextLevel(player_entity.lv - 1);
                    PlayerService.nextLevel(player_entity.lv);
                    PlayerService.nextLevel(player_entity.lv + 1);

                    player_game_object.transform.position = player_default_position;
                    player_game_object.GetComponent<Animator>().enabled = true;
                    player_game_object.GetComponent<BoxCollider>().enabled = true;
                    player_game_object.GetComponent<NavMeshAgent>().enabled = true;
                    player_game_object.GetComponent<PlayerController>().enabled = true;
                    PlayerService.init(player_game_object, player_entity);

                    Debug.Log("SelectSceneManager.loadGame() : player_entity.equip : " + player_entity.equip.name);

                    // 装備しなおし
                    MessageBroker.Default.Publish<EquipEvent>(new EquipEvent {
                        id = player_entity.equip.id,
                        go = player_game_object,
                        update_service_data = true,
                    });

                    // ステージデータを新シーンで展開
                    GameStageService.room_clear_count = stage_save_data.room_clear_count;
                    GameStageService.enemy_strength = stage_save_data.enemy_strength;
                    GameStageService.enemy_num = stage_save_data.enemy_num;

                    // GUI起動
                    MessageBroker.Default.Publish<ChangeGuiMode>(new ChangeGuiMode { mode = GuiState.Room });
                },
            });

            return;
        }

        MessageBroker.Default.Publish<SceneChangeSimple>(new SceneChangeSimple {
            add_scene = "Maze",
            del_scene = "Select",
            active_scene = "Maze",
            post_action = () => {
                SceneService.EnableAllGameObjects("Maze", new List<string>());

                // 新しいプレイヤーを生成、初期武器ID取得
                int equip_id = PlayerService.init(null, null);

                // 初期武器を装備
                MessageBroker.Default.Publish<ChangeEquipEvent>(new ChangeEquipEvent {
                    id = equip_id,
                });

                // 新規ステージデータ設定
                GameStageService.room_clear_count = room_clear_count_default;
                GameStageService.enemy_strength = enemy_strength_default;
                GameStageService.enemy_num = enemy_num_default;

                // GUI起動
                MessageBroker.Default.Publish<ChangeGuiMode>(new ChangeGuiMode { mode = GuiState.Room });

            },
        });
    }



    public void slot1NewGame()
    {
        loadGame(1);
    }

    public void slot1LoadGame()
    {
        loadGame(1, s1go, s1p);
        if (s2go != null) EquipService.del(s2p.equip.id);
        if (s3go != null) EquipService.del(s3p.equip.id);
    }

    public void slot1Delete()
    {
        confirm_dialog("スロット 1 のデータを削除してよろしいですか？",
            () => {
                s1go.SetActive(false);
                s1_new_button.SetActive(true);
                s1_load_button.SetActive(false);
                s1_del_button.SetActive(false);
            },
            null
        );

        // 確認ダイアログ表示「Slot1のデータを削除して良いですか？」Yes/No
        // Noならダイアログ消して処理終了
        // Yesならダイアログ消して以下の処理
        // スロット1のボタン表示を切り替え
        // LoadGameボタン、Deleteボタン非表示
        // NewGameボタン表示
    }



    public void slot2NewGame()
    {
        loadGame(2);

        // シーン遷移保持データ"スロット2"保存
        // シーン遷移保持データ"キャラデータ"持たせない
        // シーン遷移保持データ"ステージデータ"持たせない
        // 遷移先で新規キャラ作成して1からゲームスタート
    }

    public void slot2LoadGame()
    {
        loadGame(2, s2go, s2p);
        if (s1go != null) EquipService.del(s1p.equip.id);
        if (s3go != null) EquipService.del(s3p.equip.id);

        // シーン遷移保持データ"スロット2"保存
        // シーン遷移保持データ"キャラデータ"スロット2のキャラデータ保存
        // シーン遷移保持データ"ステージデータ"スロット2のステージデータ保存
        // シーン遷移
        // 遷移先でキャラデータ、ステージデータ展開してからゲーム開始
    }

    public void slot2Delete()
    {
        confirm_dialog("スロット 2 のデータを削除してよろしいですか？",
            () => {
                s2go.SetActive(false);
                s2_new_button.SetActive(true);
                s2_load_button.SetActive(false);
                s2_del_button.SetActive(false);
            },
            null
        );

        // 確認ダイアログ表示「Slot2のデータを削除して良いですか？」Yes/No
        // Noならダイアログ消して処理終了
        // Yesならダイアログ消して以下の処理
        // スロット2のボタン表示を切り替え
        // LoadGameボタン、Deleteボタン非表示
        // NewGameボタン表示
    }



    public void slot3NewGame()
    {
        loadGame(3);

        // シーン遷移保持データ"スロット3"保存
        // シーン遷移保持データ"キャラデータ"持たせない
        // シーン遷移保持データ"ステージデータ"持たせない
        // 遷移先で新規キャラ作成して1からゲームスタート
    }

    public void slot3LoadGame()
    {
        loadGame(3, s3go, s3p);
        if (s1go != null) EquipService.del(s1p.equip.id);
        if (s2go != null) EquipService.del(s2p.equip.id);

        // シーン遷移保持データ"スロット3"保存
        // シーン遷移保持データ"キャラデータ"スロット3のキャラデータ保存
        // シーン遷移保持データ"ステージデータ"スロット3のステージデータ保存
        // シーン遷移
        // 遷移先でキャラデータ、ステージデータ展開してからゲーム開始
    }

    public void slot3Delete()
    {
        confirm_dialog("スロット 3 のデータを削除してよろしいですか？",
            () => {
                s3go.SetActive(false);
                s3_new_button.SetActive(true);
                s3_load_button.SetActive(false);
                s3_del_button.SetActive(false);
            },
            null
        );

        // 確認ダイアログ表示「Slot3のデータを削除して良いですか？」Yes/No
        // Noならダイアログ消して処理終了
        // Yesならダイアログ消して以下の処理
        // スロット3のボタン表示を切り替え
        // LoadGameボタン、Deleteボタン非表示
        // NewGameボタン表示
    }



    public void confirm_dialog(string msg, UnityAction yes, UnityAction no)
    {
        dialog_text.text = msg;

        dialog_yes_button.onClick.AddListener(() => {
            if (yes != null) yes();
            dialog.SetActive(false);
        });

        dialog_no_button.onClick.AddListener(() => {
            if (no != null) no();
            dialog.SetActive(false);
        });

        dialog.SetActive(true);
    }



}
