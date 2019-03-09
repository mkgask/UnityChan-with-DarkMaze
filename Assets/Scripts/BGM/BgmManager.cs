using System.Collections.Generic;
using UnityEngine;
using UniRx;
using sgffu.EventMessage;

namespace sgffu.Bgm
{

    public class BgmManager : MonoBehaviour
    {
        //[SerializeField] private List<GameObject> bgmPrefabs = new List<GameObject>();

        [SerializeField] private AudioSource[] audioSources;

        [SerializeField] private float fade_duration;

        [SerializeField] private float bgm_scale;

        void Start()
        {
            Dictionary<string, AudioClip> bgm_list = new Dictionary<string, AudioClip>();

            foreach (var bgm in Resources.LoadAll<AudioClip>("BGM")) {
                bgm_list.Add(bgm.name, bgm);
            }

            Factory.init(bgm_list);
            Service.init(audioSources, fade_duration, bgm_scale);

            MessageBroker.Default.Receive<BgmCheck>().Subscribe(x => Service.bgmCheck());

            Service.bgmCheck();
        }

    }

}