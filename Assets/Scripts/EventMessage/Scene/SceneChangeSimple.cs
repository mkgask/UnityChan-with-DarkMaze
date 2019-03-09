using UnityEngine.Events;

namespace sgffu.EventMessage
{
    public class SceneChangeSimple
    {
        public string add_scene = "";
        public string del_scene = "";
        public string active_scene = "";

        public UnityAction pre_action = null;
        //public UnityAction intermediate_action = null;
        public UnityAction post_action = null;
    }
}