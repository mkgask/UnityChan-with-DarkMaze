using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using sgffu.EventMessage;

public class TreasureController : MonoBehaviour
{

    [SerializeField]
    private GameObject go_open;

    [SerializeField]
    private GameObject go_close;

    private bool on_target = false;

    private bool opened = false;

    public void OnTarget()
    {
        on_target = true;
    }

    public void OnTriggerEnter(Collider target)
    {
        if (target.gameObject == null || target.gameObject.tag != "Player") { return; }
        if (go_open == null || go_close == null) { return; }
        if (!on_target || opened) { return; }
        
        go_open.SetActive(true);
        go_close.SetActive(false);

        Item.Equip.Service.randomCreate();
        opened = true;

        Debug.Log("Publish SE: ChestOpen");

/*
        if (go_open.activeSelf) {
            go_close.SetActive(true);
            go_open.SetActive(false);
        } else {
            go_open.SetActive(true);
            go_close.SetActive(false);

            Item.Equip.Service.randomCreate();
            opened = true;
        }
*/
    }
}
