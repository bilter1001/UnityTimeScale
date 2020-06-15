using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

namespace PlayControllerScript
{
    public class PlayController
    {
        private static PlayController _instance;

        public static PlayController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlayController();
                }

                return _instance;
            }
        }


        Dictionary<VideoPlayer, float> dicVideoPlayer = new Dictionary<VideoPlayer, float>();
        Dictionary<AudioSource, float> dicAudioSource = new Dictionary<AudioSource, float>();
                

        public void SetVideoAudioSpeed(float speed)
        {
            dicVideoPlayer.Clear();
            dicAudioSource.Clear();
            VideoPlayer[] arrVideoPlayer = GameObject.FindObjectsOfType<VideoPlayer>();
            foreach (var itemVideo in arrVideoPlayer)
            {
                dicVideoPlayer[itemVideo] = itemVideo.playbackSpeed;
                itemVideo.playbackSpeed *= speed;
            }

            AudioSource[] arrAudioSource = GameObject.FindObjectsOfType<AudioSource>();
            foreach (var itemAudio in arrAudioSource)
            {
                dicAudioSource[itemAudio] = itemAudio.pitch;
                itemAudio.pitch *= speed;
            }
        }

        public void ReSetVideoAudioSpeed()
        {
            VideoPlayer[] arrVideoPlayer = GameObject.FindObjectsOfType<VideoPlayer>();
            foreach (var itemVideo in arrVideoPlayer)
            {

                float oriSpeed = 1;
                if (dicVideoPlayer.TryGetValue(itemVideo, out oriSpeed))
                {
                    itemVideo.playbackSpeed = oriSpeed;
                }
                else
                {
                    itemVideo.playbackSpeed = 1;
                }

            }

            AudioSource[] arrAudioSource = GameObject.FindObjectsOfType<AudioSource>();
            foreach (var itemAudio in arrAudioSource)
            {
                float oriPitch = 1;
                if (dicAudioSource.TryGetValue(itemAudio, out oriPitch))
                {
                    itemAudio.pitch = oriPitch;
                }
                else
                {
                    itemAudio.pitch = 1;
                }
            }
        }

        public void ChangeTimelineSpeed(float speed)
        {
            //获取所有场景中的PlayableDirector
            List<PlayableDirector> directors = new List<PlayableDirector>();
            //检索所有的PlayableDirector
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                var roots = scene.GetRootGameObjects();
                if (roots == null)
                {
                    continue;
                }
                foreach (var root in roots)
                {
                    var components = root.GetComponentsInChildren<PlayableDirector>();
                    if (components != null)
                    {
                        directors.AddRange(components);
                    }
                }
            }
            SetDirectorsSpeed(directors, speed);
        }

        void SetDirectorsSpeed(List<PlayableDirector> directors, float speed)
        {
            //检索所有的轨道
            if (directors == null || directors.Count <= 0)
            {
                return;
            }

            foreach (var director in directors)
            {
                if (director == null || director.playableAsset == null)
                {
                    continue;
                }

                var playableBindings = director.playableAsset.outputs.ToList();
                if (!playableBindings.Any())
                {
                    continue;
                }

                ////遍历track，找到音频轨道
                //foreach (var playableBinding in playableBindings)
                //{
                //    if (playableBinding.sourceObject is AudioTrack)
                //    {
                //        AudioTrack audioTrack = playableBinding.sourceObject as AudioTrack;
                //        //遍历clip
                //        foreach (var clip in audioTrack.GetClips())
                //        {
                //            if (clip.asset is AudioPlayableAsset)
                //            {
                //                clip.timeScale = speed; //设置该TimelineClip的SpeedMultiplier
                //                Debug.LogError("clip.timeScale: " + clip.timeScale);
                //            }
                //        }
                //    }
                //}

                PlayableGraph? playableGraph = director.playableGraph;
                if (playableGraph != null && ((PlayableGraph)playableGraph).IsValid())
                {
                    int rootCount = director.playableGraph.GetRootPlayableCount();
                    for (int i = 0; i < rootCount; i++)
                    {
                        director.playableGraph.GetRootPlayable(i).SetSpeed(speed);
                    }
                }
            }
        }
    }
}
