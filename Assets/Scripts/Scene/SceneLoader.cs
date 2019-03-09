using UnityEngine;
using UniRx;
using sgffu.EventMessage;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace scenes
{
    public class SceneLoader : MonoBehaviour
    {

        //private SceneService scene_service = new SceneService();

        [SerializeField] private string _loading_scene_name = "";

        void Start()
        {
            var scene_service = new SceneService();
            scene_service.init(_loading_scene_name);

            SceneManager.sceneLoaded += scene_service.OnLoadedScene;
            SceneManager.sceneUnloaded += scene_service.OnUnloadedScene;

            // 自分をDontDestroyに登録
            DontDestroyOnLoad(gameObject);

            // シーン遷移待ち受け
            MessageBroker.Default.Receive<SceneChangeSingle>().Subscribe(x => {
                scene_service.sceneChangeSingle(x);
            });

            MessageBroker.Default.Receive<SceneChangeSimple>().Subscribe(x => {
                scene_service.sceneChangeSimple(x);
            });

            MessageBroker.Default.Receive<SceneChangeComplexity>().Subscribe(x => {
                scene_service.sceneChangeComplexity(x);
            });

            // Titleシーンをロード
            //scene_service.sceneChange("Title", "BootStrap", "Title");
            MessageBroker.Default.Publish<SceneChangeSimple>(new SceneChangeSimple {
                add_scene = "Title",
                del_scene = "BootStrap",
                active_scene = "Title",
                // pre_action = null,
                // post_action = null,
            });
        }
    }
}