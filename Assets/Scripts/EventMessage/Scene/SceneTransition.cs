using System.Collections.Generic;

namespace sgffu.EventMessage
{
    public class SceneTransition
    {
        public string add_scene;

        public string del_scene;

        public List<string> add_scenes = new List<string>();

        public List<string> del_scenes = new List<string>();

        public string active_scene;
    }
}