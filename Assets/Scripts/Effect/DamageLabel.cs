using System;
using System.Collections;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using TMPro;


namespace Effect
{
    public class DamageLabel : MonoBehaviour
    {
        [SerializeField] private string text = "0";

        [SerializeField] private int time = 3000;

        [SerializeField] private float alpha_scale = 1f;

        [SerializeField] private float pos_scale = 1f;

        [SerializeField] private Vector3 update_position = Vector3.up;

        [SerializeField] private Color[] colors;

        [SerializeField] private Color outline_color;

        private float alpha;

        private CanvasRenderer renderer;

        private RectTransform rect_transform;

        private TextMeshProUGUI tmpugui;

        private int color_index = 0;

        public int id = 0;

        void OnEnable()
        {
            //Debug.Log("DamageLabel " + id.ToString() + " :  OnEnable() : " + text);
        }

        void Start()
        {
            renderer = GetComponent<CanvasRenderer>();
            rect_transform = GetComponent<RectTransform>();
            tmpugui = GetComponent<TextMeshProUGUI>();
            alpha = 1f;

            tmpugui.alpha = alpha;
            tmpugui.color = colors[color_index];
            tmpugui.text = text;
            //renderer.SetColor(colors[color_index]);
            //renderer.SetAlpha(alpha);
            //tmpugui.outlineColor = outline_color;
            //tmpugui.outlineWidth = 2;

            //Debug.Log("DamageLabel " + id.ToString() + " :  Start() : " + colors[color_index].ToString());

            // 指定の分だけ毎フレーム移動
            this.FixedUpdateAsObservable().Subscribe(x => {
                // 透明度
                alpha -= (Time.deltaTime * alpha_scale);
                tmpugui.alpha = alpha;
                //renderer.SetAlpha(alpha);
                // 高さ
                rect_transform.localPosition += (update_position * pos_scale);
            });

            // 指定時間で消滅
            Observable.Timer(TimeSpan.FromMilliseconds(time)).Subscribe(x => Destroy(gameObject));
        }

        public void setColor(int index)
        {
            color_index = index;
            //Debug.Log("DamageLabel " + id.ToString() + " :  setColor() : " + index.ToString());
        }

        public void setDamage(int damage)
        {
            setColor(0);
            text = damage.ToString();
            //Debug.Log("DamageLabel " + id.ToString() + " :  setDamage() : " + text);
        }

        public void setMiss()
        {
            setColor(1);
            text = "Miss";
            //Debug.Log("DamageLabel " + id.ToString() + " :  setDamage() : " + text);
        }

        public void setLvUP()
        {
            setColor(2);
            text = "LV UP";
            //Debug.Log("DamageLabel " + id.ToString() + " :  setDamage() : " + text);
        }

    }
}
