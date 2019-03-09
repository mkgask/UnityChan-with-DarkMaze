using UnityEngine;
using UniCommon;
using UniRx;
using sgffu.EventMessage;

public class UniCommonWrapperManager : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        //UniCommon.UniCommon.Initialize();
    }

    void Start()
    {
        UniCommon.UniCommon.Initialize();
        MessageBroker.Default.Publish<UniCommonInitialised>(new UniCommonInitialised());
    }

}
