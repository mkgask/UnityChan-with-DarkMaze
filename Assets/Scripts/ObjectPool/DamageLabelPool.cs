using UnityEngine;
using TMPro;
using UniRx;

using TextMeshPro = TMPro.TMP_Text;



namespace sgffu.ObjectPool.DamageLabel
{
    class DamageLabelPool : UniRx.Toolkit.ObjectPool<TextMeshPro>
    {
        private readonly TextMeshPro _prefab;
        private readonly Transform _parentTransform;

        //コンストラクタ
        public DamageLabelPool(Transform parentTransform, TextMeshPro prefab)
        {
            _parentTransform = parentTransform;
            _prefab = prefab;
        }

        /// <summary>
        /// オブジェクトの追加生成時に実行される
        /// </summary>
        protected override TextMeshPro CreateInstance()
        {
            //新しく生成
            var e = GameObject.Instantiate(_prefab);

            //ヒエラルキーが散らからないように一箇所にまとめる
            //e.transform.SetParent(_parentTransform);

            return e;
        }
    }
}
