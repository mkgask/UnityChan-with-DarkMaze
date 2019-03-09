using UnityEngine;
using UniRx;
using sgffu.EventMessage;
using EquipEntity = Item.Equip.EquipEntity;
using EquipService = Item.Equip.Service;

public class ItemEquipController : MonoBehaviour
{
    public EquipEntity entity;

    private bool on_target = false;



    public void Start() {}
    public void Update() {}


    public void OnTarget()
    {
        Debug.Log("ItemEquipController::OnTarget() : " + entity.name);

        on_target = true;
        //EquipService.OnTarget(this.gameObject);
    }

    public void TargetDisable()
    {
        on_target = false;
    }

    public void OnCollisionEnter(Collision target)
    {
        collisionCheck(target);
    }

    public void OnCollisionStay(Collision target)
    {
        collisionCheck(target);
    }

    private void collisionCheck(Collision target)
    {
        //Debug.Log("ItemEquipController::collisionCheck() : target.gameObject : " + target.gameObject.ToString());
        //Debug.Log("ItemEquipController::collisionCheck() : target.gameObject.tag : " + target.gameObject.tag);

        if (on_target && target.gameObject.tag == "Player") {
            //Debug.Log("ItemEquipController::collisionCheck() : on_target : true");
            MessageBroker.Default.Publish<ChangeEquipEvent>(new ChangeEquipEvent {
                id = entity.id,
            });
            //Debug.Log("ItemEquipController : MessageBroker.Default.Publish<ChangeEquipEvent>(new ChangeEquipEvent());");
        } else {
            //Debug.Log("ItemEquipController::collisionCheck() : on_target : false");
        }
    }

}