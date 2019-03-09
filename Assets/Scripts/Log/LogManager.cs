using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using sgffu.EventMessage;

namespace sgffu.Log
{
    public class LogManager : MonoBehaviour
    {
        void Awake()
        {
            MessageBroker.Default.Receive<UniCommonInitialised>().Subscribe(x => {
                Service.init();
                Service.write("sgffu.Log.LogManager::Awake()::UniCommonInitialised.Subscribe()");

                MessageBroker.Default.Publish<LogInitialized>(new LogInitialized());
            });
/*
            Observable.Timer(TimeSpan.FromSeconds(30)).Subscribe(x => {
                Debug.Log("Observable.Timeout 30 seconds");
            });
*/
        }

        void Start()
        {
            //MessageBroker.Default.Publish<LogInitialized>(new LogInitialized());
        }

    }
}
