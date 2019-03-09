using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using sgffu.EventMessage;
using UniRx;



namespace scenes
{
    class SceneService
    {

        //--
        //    Properties
        //--

        private string _loading_scene_name;

        private string _active_scene_name;

        private List<UnityAction> _post_actions = new List<UnityAction>();
        private Dictionary<string, UnityAction> _post_action = new Dictionary<string, UnityAction>();

        private Dictionary<string, SceneState> _scene_state = new Dictionary<string, SceneState>();



        //--
        //    Foundation
        //--

        public void init(string loading_scene_name)
        {
            //Debug.Log("SceneService.init : loading_scene_name : " + loading_scene_name);
            _loading_scene_name = loading_scene_name;
        }



        //--
        //    Modules (Scene Loader)
        //--

        async public void sceneChangeSingle(
            SceneChangeSingle scene_change
        ) {
            //Debug.Log("SceneService.sceneChangeSingle : scene_change : " + scene_change.ToString());
            await loadingSceneLoad();

            if (scene_change.pre_action != null) scene_change.pre_action();
            if (scene_change.post_action != null) _post_actions.Add(scene_change.post_action);
            _active_scene_name = scene_change.add_scene;

            if (scene_change.add_scene != "") {
                sceneLoad(scene_change.add_scene);
            }
        }

        public void sceneChangeSimple(
            SceneChangeSimple scene_change
        ) {
            //Debug.Log("SceneService.sceneChangeSimple : scene_change : " + scene_change.ToString());
            loadingSceneLoad();

            if (scene_change.pre_action != null) scene_change.pre_action();
            if (scene_change.post_action != null) _post_actions.Add(scene_change.post_action);
            if (scene_change.active_scene != "") _active_scene_name = scene_change.active_scene;

            if (scene_change.add_scene != "") {
                //Debug.Log("SceneService.sceneChangeSimple : sceneLoad : scene_change.add_scene : " + scene_change.add_scene);
                sceneLoad(scene_change.add_scene, LoadSceneMode.Additive);
            }

            if (scene_change.del_scene != "") {
                //Debug.Log("SceneService.sceneChangeSimple : sceneUnload : scene_change.del_scene : " + scene_change.del_scene);
                sceneUnload(scene_change.del_scene);
            }
        }

        async public void sceneChangeComplexity(
            SceneChangeComplexity scene_change
        ) {
            //Debug.Log("SceneService.sceneChangeComplexity : scene_change : " + scene_change.ToString());
            await loadingSceneLoad();

            if (scene_change.pre_action != null) scene_change.pre_action();
            if (scene_change.post_action != null) _post_actions.Add(scene_change.post_action);
            if (scene_change.active_scene != "") _active_scene_name = scene_change.active_scene;

            if (scene_change.add_scenes != null && 0 < scene_change.add_scenes.Count) {
                foreach (var scene_name in scene_change.add_scenes) {
                    sceneLoad(scene_name, LoadSceneMode.Additive);
                }
            }

            if (scene_change.del_scenes != null && 0 < scene_change.del_scenes.Count) {
                foreach (var scene_name in scene_change.del_scenes) {
                    sceneUnload(scene_name);
                }
            }
        }



        public void setActiveScene(string scene_name)
        {
            //Debug.Log("SceneService.setActiveScene : scene_name : " + scene_name);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene_name));
        }



        //--
        //    Utilities (Scene Load / Unload)
        //--

        public void sceneLoad(
            string scene_name,
            LoadSceneMode load_scene_mode = LoadSceneMode.Single,
            bool allow_reset = false
        ) {
            //Debug.Log("SceneService.sceneLoad : scene_name : " + scene_name);

            if (_scene_state.ContainsKey(scene_name) && ((
                allow_reset &&
                    _scene_state[scene_name] != SceneState.LOADED
            ) || (
                !allow_reset && (
                    _scene_state[scene_name] == SceneState.LOADED ||
                    _scene_state[scene_name] == SceneState.LOADING
                )
            ))) {
                //Debug.Log("SceneService.sceneLoad : if (contain && ～ : true");
                return;
            }

            if (_scene_state.ContainsKey(_loading_scene_name) &&
                    _scene_state[_loading_scene_name] != SceneState.LOADED) {
                //Debug.Log("SceneService.sceneLoad : SceneState.LOADWAIT : " + scene_name);
                _scene_state[scene_name] = SceneState.LOADWAIT;
                return;
            }

            _scene_state[scene_name] = SceneState.LOADING;
            //Debug.Log("SceneService.sceneLoad : SceneState.LOADING : " + scene_name);
            SceneManager.LoadSceneAsync(scene_name, load_scene_mode);
        }

        public void sceneUnload(string scene_name)
        {
            //Debug.Log("SceneService.sceneUnload : scene_name : " + scene_name);

            if (_scene_state.ContainsKey(scene_name) && (
                    _scene_state[scene_name] == SceneState.UNLOADING ||
                    _scene_state[scene_name] == SceneState.UNLOADED)) {
                //Debug.Log("SceneService.sceneUnload : if (contain && ～ : true");
                return;
            }

            if (_scene_state.Where(x => x.Value == SceneState.LOADING).Any()) {
                //Debug.Log("SceneService.sceneUnload : SceneState.UNLOADWAIT : " + scene_name);
                _scene_state[scene_name] = SceneState.UNLOADWAIT;
                return;
            }

            _scene_state[scene_name] = SceneState.UNLOADING;

            //Debug.Log("SceneService.sceneUnload : SceneState.UNLOADING : " + scene_name);
            SceneManager.UnloadSceneAsync(scene_name);
        }



        //--
        //    Utilities (Events Callback)
        //--

        public void OnLoadedScene(Scene scene, LoadSceneMode scene_mode)
        {
            //Debug.Log("SceneService.OnLoadedScene : scene.name : " + scene.name);
            //Debug.Log("SceneService.OnLoadedScene : _scene_state : " + _scene_state.ToString());

            var scene_name = scene.name;
            var contain = _scene_state.ContainsKey(scene_name);

            _scene_state[scene_name] = SceneState.LOADED;

            if (scene.name == _active_scene_name) {
                SceneManager.SetActiveScene(scene);
                _active_scene_name = "";
            }

            foreach (var s in _scene_state.Where(x => x.Value == SceneState.LOADWAIT).ToArray()) {
                sceneLoad(s.Key);
            }

            if (
                !_scene_state.Any(x => x.Value == SceneState.LOADWAIT) &&
                !_scene_state.Any(x => x.Value == SceneState.LOADING)
            ) {
                foreach (var s in _scene_state.Where(x => x.Value == SceneState.UNLOADWAIT).ToArray()) {
                    sceneUnload(s.Key);
                }
            }

            postAction();
        }

        public void OnUnloadedScene(Scene scene)
        {
            //Debug.Log("SceneService.OnUnloadedScene : scene.name : " + scene.name);
            //Debug.Log("SceneService.OnUnloadedScene : _scene_state : " + _scene_state.ToString());

            var scene_name = scene.name;
            var contain = _scene_state.ContainsKey(scene_name);

            _scene_state[scene_name] = SceneState.UNLOADED;

            //Debug.Log("SceneService.OnUnloadedScene : _scene_state.Count : " + _scene_state.Count.ToString());
            //Debug.Log("SceneService.OnUnloadedScene : _scene_state.Count(x => x.Value == SceneState.LOADED) : " + _scene_state.Count(x => x.Value == SceneState.LOADED).ToString());
            //Debug.Log("SceneService.OnUnloadedScene : _scene_state.Count(x => x.Value == SceneState.UNLOADED) : " + _scene_state.Count(x => x.Value == SceneState.UNLOADED).ToString());

            postAction();
        }



        public void postAction()
        {

            //foreach (var pair in _scene_state) Debug.Log("SceneService.postAction : " + pair.Key + " : " + pair.Value);

            if (_scene_state.Count ==
                    _scene_state.Count(x => x.Value == SceneState.LOADED) +
                    _scene_state.Count(x => x.Value == SceneState.UNLOADED)) {
                
                //Debug.Log("SceneService.postAction : _post_actions.Count : " + _post_actions.Count.ToString());
                foreach (var action in _post_actions.ToArray()) {
                    action();
                }

                _post_actions.Clear();
                loadingSceneUnload();
            }
        }


        //--
        //    Utilities (Game Objects)
        //--

        public static void EnableAllGameObjects(string scene_name, List<string> ignores)
        {
            //Debug.Log("EnableAllGameObjects : " + scene_name);

            var scene = SceneManager.GetSceneByName(scene_name);
            var gos = scene.GetRootGameObjects();

            //Debug.Log("EnableAllGameObjects : GetSceneByName : " + scene.name);
            //Debug.Log("EnableAllGameObjects : gos.Length : " + gos.Length.ToString());

            foreach (var go in gos) {
                if (ignores.Contains(go.name)) continue;
                go.SetActive(true);
            }
        }



        //--
        //    Utilities (Loading Scene)
        //--

        public AsyncOperation loadingSceneLoad()
        {
            if (_loading_scene_name == "") throw new System.Exception("Loading scene name not found.");
            
            var contain = _scene_state.ContainsKey(_loading_scene_name);

            if (contain && (
                    _scene_state[_loading_scene_name] == SceneState.LOADED ||
                    _scene_state[_loading_scene_name] == SceneState.LOADING)) {
                return default(AsyncOperation);
            }

            _scene_state[_loading_scene_name] = SceneState.LOADING;

            //Debug.Log("SceneService.loadingSceneLoad : _loading_scene_name : " + _loading_scene_name);
            return SceneManager.LoadSceneAsync(_loading_scene_name, LoadSceneMode.Additive);
        }

        public AsyncOperation loadingSceneUnload()
        {
            if (_loading_scene_name == "") throw new System.Exception("Loading scene name not found.");
            
            var contain = _scene_state.ContainsKey(_loading_scene_name);

            if (contain && (
                    _scene_state[_loading_scene_name] == SceneState.UNLOADED ||
                    _scene_state[_loading_scene_name] == SceneState.UNLOADING)) {
                return default(AsyncOperation);
            }

            _scene_state[_loading_scene_name] = SceneState.UNLOADING;

            //Debug.Log("SceneService.loadingSceneUnload : _loading_scene_name : " + _loading_scene_name);
            return SceneManager.UnloadSceneAsync(_loading_scene_name);
        }

    }
}