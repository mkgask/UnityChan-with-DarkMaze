using System.Collections.Generic;
using UnityEngine;
using UniRx;
using sgffu.EventMessage;

namespace sgffu.Se
{


    public class SeManager : MonoBehaviour
    {
        [SerializeField] private AudioSource src;

        void Start ()
        {
            Dictionary<string, AudioClip> se_list = new Dictionary<string, AudioClip>();

            foreach (var se in Resources.LoadAll<AudioClip>("SE")) {
                se_list.Add(se.name, se);
            }

            Service.init(se_list, src);
            MessageBroker.Default.Receive<PlaySe>().Subscribe(se => {
                Service.play(se.name);
                //Debug.Log("Recieve SE: ChestOpen");
            });
        }

    }
}
