using UnityEngine.Events;

namespace sgffu.EventMessage
{
    public class SceneChangeSingle
    {
        public string add_scene = "";

        public UnityAction pre_action = null;
        //public UnityAction intermediate_action = null;
        public UnityAction post_action = null;
    }
}