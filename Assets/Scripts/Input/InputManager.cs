using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using sgffu.Input;
using sgffu.EventMessage;
using LogService = sgffu.Log.Service;

public class InputManager : MonoBehaviour
{
    void Awake()
    {
        Service.init();
    }

    void Start()
    {
        //Service.init();
        //LogService.write("InputManager::Start()::Publish<InputInitialized>");
        MessageBroker.Default.Publish<InputInitialized>(new InputInitialized());
    }

    void Update()
    {
        Service.inputCheck();
    }
}
