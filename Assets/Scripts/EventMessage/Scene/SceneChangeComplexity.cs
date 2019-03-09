using System.Collections.Generic;
using UnityEngine.Events;

namespace sgffu.EventMessage
{
    public class SceneChangeComplexity
    {
        public List<string> add_scenes = null;
        public List<string> del_scenes = null;

        public string active_scene = "";

        public UnityAction pre_action = null;
        //public UnityAction intermediate_action = null;
        public UnityAction post_action = null;
    }
}