using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UniRx;
using sgffu.EventMessage;
using TMPro;

using GuiState = Gui.State;
using GuiService = Gui.Service;
using LogService = sgffu.Log.Service;
using EquipService = Item.Equip.Service;
using EffectService = Effect.Service;
using EffectiveFloorService = sgffu.EffectiveFloor.EffectiveFloorService;
using FieldObjectService = sgffu.FieldObject.FieldObjectService;
using EnemyService = sgffu.Characters.Enemy.Service;

public class GuiManager : MonoBehaviour
{

    public bool gui_enabled = false;

    //TextMeshPro w_name;

    [SerializeField] private GameObject settings_dialog;

    [SerializeField] private AudioMixer audio_mixer;



    // Use this for initialization
    void Awake()
    {
        /*
        GuiService.init(gameObject);

        Observable.Zip(
            //MessageBroker.Default.Receive<CreatedPlayerService>(),
            //MessageBroker.Default.Receive<CreatedEnemyService>(),
            MessageBroker.Default.Receive<CreatedPlayerService>(),
            MessageBroker.Default.Receive<CreatedEquipBaseResouces>(),
            MessageBroker.Default.Receive<LogInitialized>(),
            Tuple.Create
        ).Subscribe(x => {
            LogService.write("GuiManager::Awake()::Receive<3>");
            enable();

            MessageBroker.Default.Publish<CreatedGUI>(new CreatedGUI());
        });
        */
    }

    void Start()
    {
        GuiService.init(gameObject);

        initBgmSlider();
        initSeSlider();
/*
        Observable.Zip(
            //MessageBroker.Default.Receive<CreatedPlayerService>(),
            //MessageBroker.Default.Receive<CreatedEnemyService>(),
            MessageBroker.Default.Receive<CreatedPlayerService>(),
            MessageBroker.Default.Receive<CreatedEquipBaseResouces>(),
            MessageBroker.Default.Receive<LogInitialized>(),
            Tuple.Create
        ).Subscribe(x => {
            //LogService.write("GuiManager::Awake()::Receive<3>");
            enable();

            MessageBroker.Default.Publish<CreatedGUI>(new CreatedGUI());
        });
*/
        //LogService.write("GuiManager::Start()::Publish<CreatedGUI>");
        //MessageBroker.Default.Publish<CreatedGUI>(new CreatedGUI());

        enable();
        GuiService.modeChange(GuiState.Title);

        MessageBroker.Default.Receive<ChangeGuiMode>().Subscribe(x => {
            GuiService.modeChange(x.mode);
        });

        MessageBroker.Default.Receive<EnableGui>().Subscribe(x => {
            if (x.enable) enable();
            else disable();
        });

        MessageBroker.Default.Receive<SaveEnd>().Subscribe(x => {
            if (!scene_change_title) return;
            scene_change_title = false;
            applySceneChangeTitle();
        });

    }


    private bool scene_change_title = false;


    void OnGUI()
    {
        if (!gui_enabled) {
            return;
        }

        GuiService.update();
    }



    public void enable()
    {
        //Debug.Log("GuiManager::enable() : start");
        GuiService.display();
        gui_enabled = true;
    }

    public void disable()
    {
        //Debug.Log("GuiManager::disable() : start");
        GuiService.unDisplay();
        gui_enabled = false;
    }

/*
    public void fade()
    {
    }
*/

    public void toggle_settings_dialog()
    {
        //Debug.Log("GuiManager::settings_dialog: settings_dialog.activeSelf: " + settings_dialog.activeSelf.ToString());

        MessageBroker.Default.Publish<PlaySe>(new PlaySe{ name = "Button_18_Pack2" });

        if (settings_dialog.activeSelf) {
            Time.timeScale = 1f;
            settings_dialog.SetActive(false);
            return;
        }

        Time.timeScale = 0f;
        settings_dialog.SetActive(true);
        return;
    }



    public float getBgmVolume()
    {
        float bgm_volume;
        audio_mixer.GetFloat("BgmVolume", out bgm_volume);
        return bgm_volume;
    }

    public void setBgmVolume(float vol)
    {
        audio_mixer.SetFloat("BgmVolume", vol);
    }

    private void initBgmSlider()
    {
        var slider = transform.Find("CenterPanel/SettingsDialog/bgm_bar").GetComponent<Slider>();
        slider.value = getBgmVolume();
    }



    public float getSeVolume()
    {
        float se_volume;
        audio_mixer.GetFloat("SeVolume", out se_volume);
        return se_volume;
    }

    public void setSeVolume(float vol)
    {
        audio_mixer.SetFloat("SeVolume", vol);
        MessageBroker.Default.Publish<PlaySe>(new PlaySe{ name = "Button_20_Pack2" });
    }

    private void initSeSlider()
    {
        var slider = transform.Find("CenterPanel/SettingsDialog/se_bar").GetComponent<Slider>();
        slider.value = getSeVolume();
    }



    private bool enable_scene_change = false;

    public void sceneChangeTitle()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Select") {
            applySceneChangeTitle();
            return;
        }

        scene_change_title = true;
        MessageBroker.Default.Publish<SaveDataSave>(new SaveDataSave());
        // Continues reserve SaveEnd event on Start()
    }

    public void applySceneChangeTitle()
    {
        Debug.Log("MessageBroker.Default.Publish<SceneChangeSingle>()");
        Time.timeScale = 0f;

        MessageBroker.Default.Publish<SceneChangeSimple>(new SceneChangeSimple {
            add_scene = "Title",
            del_scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,

            pre_action = () => {
                // GUI表示をタイトル画面に切り替え
                GuiService.modeChange(GuiState.Title);

                // 設定画面表示を消去
                toggle_settings_dialog();

                // NextRoomを消去
                EffectService.nextRoomClear();

                // 地面に落ちてる装備品を削除
                EquipService.ClearTarget();
                EquipService.destroyEquipItem();

                // 装備品リストをクリア
                EquipService.clear();

                // 家具配置範囲再初期化
                EffectiveFloorService.reset();

                // 家具再初期化
                FieldObjectService.clear();

                // 敵削除
                EnemyService.destroyEnemyDead();
                EnemyService.destroyEnemy();
                EnemyService.stopSpawn();

            },

            post_action = () => {
                Time.timeScale = 1f;
            }
        });
    }



    public void exitGame()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Select") {
            Debug.Log("Application.Quit()");
            Application.Quit();
            return;
        }

        MessageBroker.Default.Receive<SaveEnd>().Subscribe(x => {
            Debug.Log("Application.Quit()");
            Application.Quit();
        });

        Debug.Log("MessageBroker.Default.Publish<SaveDataSave>()");
        MessageBroker.Default.Publish<SaveDataSave>(new SaveDataSave());
    }

}
