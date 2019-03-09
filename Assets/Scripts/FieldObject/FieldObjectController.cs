using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Item.Inventory;
using sgffu.FieldObject;
using sgffu.EventMessage;

public class FieldObjectController : MonoBehaviour
{

    [SerializeField]
    private GameObject open;

    [SerializeField]
    private GameObject close;

    private bool open_close = false;

    public int id;

    private int current = 0;

    public void OnCollisionEnter(Collision target)
    {
        if (target.gameObject == null || target.gameObject.tag != "Player") { return; }
        if (open.activeSelf) { return; }

        open.SetActive(true);
        close.SetActive(false);

        FieldObjectService.OnCollisionEnter(id, gameObject);
        MessageBroker.Default.Publish<PlaySe>(new PlaySe{ name = "ChestOpen" });
    }
}
