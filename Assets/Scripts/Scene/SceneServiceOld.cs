/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SceneManagement = UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;
using Scene = UnityEngine.SceneManagement.Scene;
using UniRx;
using sgffu.EventMessage;

namespace scenes
{
    public class SceneServiceOld
    {



        private List<string> loaded_scenes = new List<string>();
        private List<string> post_unload_scenes = new List<string>();



        public void init(string bootstrap_scene_name)
        {
            loaded_scenes.Add(bootstrap_scene_name);
        }

        public void sceneChangeSingle(
            SceneChangeSingle scene_change
        ) {
            if (scene_change.pre_action != null) scene_change.pre_action();
            if (scene_change.add_scene != "") sceneLoad(scene_change.add_scene);
            if (scene_change.intermediate_action != null) scene_change.intermediate_action();
            if (scene_change.post_action != null) scene_change.post_action();
        }

        public void sceneChangeSimple(
            SceneChangeSimple scene_change
        ) {
            if (scene_change.pre_action != null) scene_change.pre_action();
            if (scene_change.add_scene != "") sceneLoad(scene_change.add_scene, scene_change.active_scene, SceneManagement.LoadSceneMode.Additive);
            if (scene_change.intermediate_action != null) scene_change.intermediate_action();
            if (scene_change.del_scene != "") sceneUnload(scene_change.del_scene);
            if (scene_change.post_action != null) scene_change.post_action();
        }

        public void sceneChangeComplexity(
            SceneChangeComplexity scene_change
        ) {
            if (scene_change.pre_action != null) scene_change.pre_action();
            
            if (scene_change.add_scenes != null && 0 < scene_change.add_scenes.Count) {
                foreach (var scene_name in scene_change.add_scenes) {
                    sceneLoad(scene_name, "", SceneManagement.LoadSceneMode.Additive);
                }
            }

            if (scene_change.active_scene != "") {
                setActiveScene(scene_change.active_scene);
            }

            if (scene_change.intermediate_action != null) scene_change.intermediate_action();

            if (scene_change.del_scenes != null && 0 < scene_change.del_scenes.Count) {
                foreach (var scene_name in scene_change.del_scenes) {
                    sceneUnload(scene_name);
                }
            }

            if (scene_change.post_action != null) scene_change.post_action();
        }

        public void setActiveScene(string scene_name)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene_name));
        }

        //async public void sceneLoad(string scene_name, string active_scene = "")
        async public void sceneLoad(
            string scene_name,
            string active_scene = "",
            SceneManagement.LoadSceneMode load_scene_mode = SceneManagement.LoadSceneMode.Single
        ) {
            var scene = SceneManager.GetSceneByName(scene_name);
            if (scene.isLoaded) return;

            await SceneManager.LoadSceneAsync(scene_name, load_scene_mode);

            if (active_scene != "") {
                setActiveScene(active_scene);
            }
        }

        async public void sceneUnload(string scene_name)
        {
            var scene = SceneManager.GetSceneByName(scene_name);
            if (scene == null || !scene.isLoaded) return;

            if (loaded_scenes.Count == 1 &&
                scene_name == SceneManager.GetActiveScene().name
            ) {
                post_unload_scenes.Add(scene_name);
                return;
            }

            await SceneManager.UnloadSceneAsync(scene_name);
        }



        public void OnLoadedScene(Scene scene, SceneManagement.LoadSceneMode scene_mode)
        {
            if (loaded_scenes.Contains(scene.name)) return;
            loaded_scenes.Add(scene.name);
            
            if (0 < post_unload_scenes.Count) {
                foreach (var unload_scene in post_unload_scenes) {
                    sceneUnload(unload_scene);
                }
            }
        }

        public void OnUnloadedScene(Scene scene)
        {
            if (!loaded_scenes.Contains(scene.name)) return;
            loaded_scenes.Remove(scene.name);
        }

    }

}
*/