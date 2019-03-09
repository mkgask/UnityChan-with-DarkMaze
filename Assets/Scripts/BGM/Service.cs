using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using RankService = GameStageRank.GameStageRankService;
using Rank = GameStageRank.Rank;
using UnityAction = UnityEngine.Events.UnityAction;

namespace sgffu.Bgm
{

    public class Service
    {
        private static Rank now_rank;

        private static AudioSource[] audio_sources;

        private static int bgm_main_id = 0;    // 現在再生中のBGM ID

        private static int bgm_sub_id = 1;    // サブ再生用のBGM ID

        private static float fade_duration;

        private static float bgm_scale;


        public static void init(AudioSource[] audioSources, float fadeDuration, float bgmScale)
        {
            audio_sources = audioSources;
            fade_duration = fadeDuration;
            bgm_scale = bgmScale;
        }


        public static void bgmCheck() {
            var active_scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            switch(active_scene) {
                case "Maze": roomBgmCheck(); break;
                default: titleBgm(); break;
            }
        }

        public static void titleBgm()
        {
            var entity = Factory.getTitleBgm();
            bgm_set(entity.clip, bgm_sub_id);
            bgm_start(bgm_sub_id, 0f);
            bgm_cross_fade();
        }

        public static void roomBgmCheck()
        {
            // ゲームステージランクサービスからランクもらう
            var rank = RankService.bgm();
            //Debug.Log("BGM Rank:" + rank);
            // ランクが変わってなければ終了
            if (now_rank == rank) return;

            // ランクにあわせたEntityを取得
            var entity = Factory.get(rank);
            //Debug.Log("BGM Load: " + entity.clip.name);

            // サブの曲を入れ替え
            bgm_set(entity.clip, bgm_sub_id);

            // 入れ替え後のサブの曲を開始
            bgm_start(bgm_sub_id, 0f);

            // BGMクロスフェード開始（メインを小さく、サブを大きく）（メインとサブを入れ替え）
            bgm_cross_fade();
        }



        public static void bgm_id_toggle()
        {
            if (bgm_main_id == 0) {
                bgm_main_id = 1;
                bgm_sub_id = 0;
            } else {
                bgm_main_id = 0;
                bgm_sub_id = 1;
            }
        }

        public static void bgm_set(AudioClip clip, int bgm_id)
        {
            audio_sources[bgm_id].clip = clip;
        }

        public static void bgm_start(int bgm_id, float start_volume)
        {
            if (audio_sources[bgm_id].clip != null) {
                audio_sources[bgm_id].volume = start_volume;
                audio_sources[bgm_id].Play();
            }
        }

        public static void bgm_stop(int bgm_id)
        {
            if (audio_sources[bgm_id].clip != null) audio_sources[bgm_id].Stop();
        }

        public static void bgm_cross_fade()
        {
            FadeVolume(fade_duration, () => {
                //Debug.Log("BGM Fade END");
                bgm_stop(bgm_main_id);
                bgm_id_toggle();
            });
        }



        private static IDisposable FadeVolume(float duration, UnityAction onComplete = null)
        {
            List<AudioSource> ls = new List<AudioSource>();

            var ob = Observable.FromCoroutine<float>(o => TransitionVolume(o, duration))
                .Subscribe(v => {
                    //Debug.Log("v: " + v);
                    //if (audio_sources[bgm_main_id] != null) audio_sources[bgm_main_id].volume = v;
                    //if (audio_sources[bgm_sub_id] != null) audio_sources[bgm_sub_id].volume = 1f - v;
                    audio_sources[bgm_main_id].volume = v * bgm_scale;
                    audio_sources[bgm_sub_id].volume = (1f - v) * bgm_scale;
                }, () => {
                    if (onComplete != null) onComplete.Invoke();
                });
            return ob;
        }

        private static IEnumerator TransitionVolume(IObserver<float> observer, float duration)
        {
            var timer = duration;

            while(0 < timer)
            {
                timer -= Time.deltaTime;
                var v = Mathf.Clamp01(timer / duration);
                observer.OnNext(v);
                yield return null;
            }

            observer.OnCompleted();
        }

    }

}