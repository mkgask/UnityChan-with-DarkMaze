using UnityEngine;
using UniRx;
using sgffu.EventMessage;

public class TitleSceneManager : MonoBehaviour
{

    private bool click_enabled = true;

    void Update ()
    {
        if (!click_enabled) return;
        if (!Input.GetMouseButton(0)) return;

        click_enabled = false;

        MessageBroker.Default.Publish<SceneChangeSimple>(new SceneChangeSimple {
            add_scene = "Select",
            del_scene = "Title",
            active_scene = "Select",
            // pre_action = null,
            // post_action = null,
        });
    }
}
