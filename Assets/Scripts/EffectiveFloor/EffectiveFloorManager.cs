using System.Collections;
using UnityEngine;
using UniRx;
using sgffu.EffectiveFloor;
using sgffu.EventMessage;
//using MessageService = sgffu.Message.Service;
using LogService = sgffu.Log.Service;

public class EffectiveFloorManager : MonoBehaviour
{
    void Awake()
    {
        //EffectiveFloorService.init();
/*
        Observable.FromCoroutine(waitForEffectiveFloorServiceInit).Subscribe(x => {
            //Debug.Log("Published at CreatedEffectiveFloor");
            //MessageBroker.Default.Publish<CreatedEffectiveFloor>(new CreatedEffectiveFloor());
            //MessageService.a.Publish<CreatedEffectiveFloor>(new CreatedEffectiveFloor());
            //MessageService.a.Dispose();
        });
*/
    }

/*
    IEnumerator waitForEffectiveFloorServiceInit() {
        EffectiveFloorService.init();
        yield return null;
    }
*/

    void Start()
    {
        EffectiveFloorService.init();
        //LogService.write("EffectiveFloorManager::Start()::Publish<CreatedEffectiveFloor>");
        MessageBroker.Default.Publish<CreatedEffectiveFloor>(new CreatedEffectiveFloor());
    }
}
