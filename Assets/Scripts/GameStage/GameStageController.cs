using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using GameStageService = GameStage.Service;
using sgffu.EventMessage;

public class GameStageController : MonoBehaviour
{
    public int strength = 0;
    public int num = 0;

    void OnTriggerEnter(Collider c)
    {
        if (c.transform.tag != "Player") return;

        //Debug.Log("GameStageService.next : " + strength + " : " + num);
        GameStageService.next(strength, num);
    }
}
