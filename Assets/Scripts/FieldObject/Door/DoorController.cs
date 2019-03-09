using System;
using UnityEngine;
using UniRx;

public class DoorController : MonoBehaviour
{
    Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();

        Observable.Timer(new TimeSpan(0, 0, 1)).First().Subscribe(x => {
            //Debug.Log("Observable.Timer: anim.SetBool : open : true");
            anim.SetBool("open", true);
        });
    }
}
