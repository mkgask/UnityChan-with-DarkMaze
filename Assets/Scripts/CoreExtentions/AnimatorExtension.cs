using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Action = UnityEngine.Events.UnityAction;



public static class AnimatorExtension
{

    public static bool IsEnd(this Animator animator, int layer_index = 0)
    {
        //オブジェクトがアクティブではない時は終了と同義とする
        if (!animator.enabled ||
                !animator.gameObject.activeSelf ||
                !animator.gameObject.activeInHierarchy)
            return true;

        return 1 <= animator.GetCurrentAnimatorStateInfo(layer_index).normalizedTime;
    }

    public static void radioBool(this Animator anim, string on) {
        foreach (var param in anim.parameters) {
            if (param.type != AnimatorControllerParameterType.Bool) continue;
            //Debug.Log("AnimatorExtension.radioBool : " + param.name);
            anim.SetBool(param.name, (param.name == on) ? true : false);
        }
    }



    private static Dictionary<Tuple<Animator,int>, IReadOnlyReactiveProperty<bool>> _reactiveProperties = new Dictionary<Tuple<Animator, int>, IReadOnlyReactiveProperty<bool>>();

    public static IReadOnlyReactiveProperty<bool> GetIsEndReactiveProperty(this Animator animator, int layerIndex = 0)
    {
        var key = new Tuple<Animator, int>(animator, layerIndex);

        if (_reactiveProperties.ContainsKey(key))
            return _reactiveProperties[key];

        _reactiveProperties.Add(key,
            animator.ObserveEveryValueChanged(_ =>
                animator.IsEnd(layerIndex)
            ).ToReactiveProperty()
        );

        //解放処理
        animator.OnDestroyAsObservable().Subscribe(_ => _reactiveProperties.Remove(key));

        return _reactiveProperties[key];
    }


    /// <summary>
    /// アニメーション再生　コールバック指定可
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateName"></param>
    /// <param name="layer"></param>
    /// <param name="normalizedTime"></param>
    /// <param name="nextFrameAction">再生開始の次フレーム時コールバック</param>
    /// <param name="endAction">終了時コールバック</param>
    public static void Play(this Animator animator, string stateName, int layer = 0, float normalizedTime = 0f, Action nextFrameAction = null, Action endAction = null)
    {
        animator.Play(stateName, layer, normalizedTime);

        if (endAction != null/* || nextFrameAction != null */)
            Observable.NextFrame().Subscribe(_ =>
            {
                nextFrameAction?.Invoke();

                if (endAction != null)
                    animator.GetIsEndReactiveProperty()
                        .First(isEnd => isEnd)
                        .Subscribe(__ => endAction())
                        .AddTo(animator);

            }).AddTo(animator);
    }

    /// <summary>
    /// アニメーション再生　コールバック指定可
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateNameHash"></param>
    /// <param name="layer"></param>
    /// <param name="normalizedTime"></param>
    /// <param name="nextFrameAction">再生開始の次フレーム時コールバック</param>
    /// <param name="endAction">終了時コールバック</param>
    public static void Play(this Animator animator, int stateNameHash, int layer = 0, float normalizedTime = 0f, Action nextFrameAction = null, Action endAction = null)
    {
        animator.Play(stateNameHash, layer, normalizedTime);

        if (endAction != null || nextFrameAction != null)
            Observable.NextFrame().Subscribe(_ =>
            {
                nextFrameAction?.Invoke();

                if (endAction != null)
                    animator.GetIsEndReactiveProperty()
                        .First(isEnd => isEnd)
                        .Subscribe(__ => endAction())
                        .AddTo(animator);

            }).AddTo(animator);

    }

}