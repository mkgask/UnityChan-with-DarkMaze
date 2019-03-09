using System.Collections.Generic;
using UnityEngine;

using DamageLabelPool = sgffu.ObjectPool.DamageLabel.DamageLabelPool;
using TextMeshPro = TMPro.TMP_Text;

namespace sgffu.ObjectPool
{
    class ObjectPoolManager : MonoBehaviour
    {


        [SerializeField] private Transform parentTransform;

        private DamageLabelPool damage_label_pool;

        void Start()
        {
            //damage_label_pool = new DamageLabelPool(parentTransform, DamageLabel);
        }
    }
}