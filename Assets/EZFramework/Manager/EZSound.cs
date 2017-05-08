/*
 * Author:      熊哲
 * CreateTime:  11/23/2016 4:22:45 PM
 * Description:
 * 
*/
using DG.Tweening;
using UnityEngine;

namespace EZFramework
{
    [RequireComponent(typeof(AudioSource))]
    public class EZSound : TEZManager<EZSound>
    {
        public AudioSource audioSource { get { return GetComponent<AudioSource>(); } }
        private AudioListener m_AudioListener;
        private AudioListener audioListener
        {
            get
            {
                if (m_AudioListener == null)
                {
                    m_AudioListener = FindObjectOfType<AudioListener>();
                }
                return m_AudioListener;
            }
            set
            {
                m_AudioListener = value;
            }
        }

        public float SpatialBlend { get { return audioSource.spatialBlend; } set { audioSource.spatialBlend = value; } }
        public bool BgmActive
        {
            get
            {
                return PlayerPrefs.GetInt("BgmActive", 1) == 1;
            }
            set
            {
                PlayerPrefs.SetInt("BgmActive", value ? 1 : 0);
                audioSource.mute = !value;
            }
        }
        public float BgmVolume
        {
            get
            {
                return PlayerPrefs.GetFloat("BgmVolume", 1);
            }
            set
            {
                audioSource.volume = value;
                PlayerPrefs.SetFloat("BgmVolume", value);
            }
        }
        public bool EfxActive
        {
            get { return PlayerPrefs.GetInt("EfxActive", 1) == 1; }
            set { PlayerPrefs.SetInt("EfxActive", value ? 1 : 0); }
        }
        public float EfxVolume
        {
            get { return PlayerPrefs.GetFloat("EfxVolume", 1); }
            set { PlayerPrefs.SetFloat("EfxVolume", value); }
        }

        public override void Init()
        {
            base.Init();
            audioSource.mute = !BgmActive;
            audioSource.volume = BgmVolume;
        }
        public override void Exit()
        {
            base.Exit();
            PlayerPrefs.Save();
        }

        public void SetAudioListener(GameObject go)
        {
            Destroy(audioListener);
            audioListener = go.AddComponent<AudioListener>();
        }
        
        public void PlayBgm(AudioClip audio, bool loop = true, float fadeOutTime = 0.2f)
        {
            audioSource.DOFade(0, fadeOutTime).OnComplete(delegate
            {
                audioSource.volume = BgmVolume;
                audioSource.clip = audio;
                audioSource.loop = loop;
                audioSource.Play();
            });
        }
        public void StopBgm(float fadeOutTime = 0.2f)
        {
            audioSource.DOFade(0, fadeOutTime).OnComplete(delegate
            {
                audioSource.Stop();
                audioSource.volume = BgmVolume;
            });
        }

        public void PlayEfx(AudioClip audio)
        {
            if (EfxActive)
            {
                AudioSource.PlayClipAtPoint(audio, audioListener.transform.position, EfxVolume);
            }
        }
        public void PlayEfx(AudioClip audio, Vector3 position)
        {
            if (EfxActive)
            {
                AudioSource.PlayClipAtPoint(audio, position, EfxVolume);
            }
        }
        
        public AudioSource AddAudioSource(GameObject go)
        {
            AudioSource audioSource = go.AddComponent<AudioSource>();
            audioSource.spatialBlend = SpatialBlend;
            return audioSource;
        }
        public AudioSource GetAudioSource(GameObject go)
        {
            AudioSource audioSource = go.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = AddAudioSource(go);
            }
            return audioSource;
        }
        public void RemoveAudioSource(GameObject go)
        {
            Destroy(go.GetComponent<AudioSource>());
        }
        public void Play(GameObject go, AudioClip audio)
        {
            if (EfxActive)
            {
                AudioSource audioSource = GetAudioSource(go);
                audioSource.clip = audio;
                audioSource.volume = EfxVolume;
                audioSource.Play();
            }
        }
    }
}