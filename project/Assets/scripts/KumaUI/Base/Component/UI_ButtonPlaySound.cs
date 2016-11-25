// #define GAME_MEDIA


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if GAME_MEDIA
using Game.Media;
#endif

/// <summary>
/// sound component
/// </summary>
[AddComponentMenu("UI/Button PlaySound")]
public class UI_ButtonPlaySound : UI_Event
{
    [SerializeField]
    public AudioClip audioClip;
    [SerializeField]
    [Range(0f, 1f)] public float volume = 1f;
    [SerializeField]
    [Range(0f, 2f)] public float pitch = 1f;

    public override void OnPointerClick(PointerEventData eventData)
    {
        // base.OnPointerClick(eventData);
        // OnClick();
        if(mAnyMove) return;
        if(sDisableEvent2 && !mIgnoreDisable) return;
        if(sDisableEvent && !mIgnoreDisable) return;
        sIsEvent = true;
        if(onClick != null)
        {
            // sIsEvent = true;
            onClick(eventData , this);
            OnClick();
        }
    }

    void OnClick ()
    {
        if(audioClip != null)
        {
            if (audioClipNew == null)
            {
                PlaySound(audioClip, volume, pitch);
            }
            else
            {
                PlaySound(audioClipNew, volume, pitch);
            }
            // PlaySound(audioClip , (float)GameSetting.instance.musicVolume / 100f , pitch);
        }
    }


    private  AudioClip audioClipNew = null;
    public void RefreshSetAudioClip(AudioClip clip)
    {
        audioClipNew = clip;
    }

    static AudioListener mListener;
    static public AudioSource PlaySound (AudioClip clip, float volume, float pitch)
    {
        // volume *= soundVolume;

        // if (clip != null && volume > 0.01f && GameSetting.instance.enableSfx)
        // {
        //     if (mListener == null || !(mListener.enabled && mListener.gameObject.activeSelf))
        //     {
        //         mListener = GameObject.FindObjectOfType(typeof(AudioListener)) as AudioListener;

        //         if (mListener == null)
        //         {
        //             Camera cam = Camera.main;
        //             if (cam == null) cam = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
        //             if (cam != null) mListener = cam.gameObject.AddComponent<AudioListener>();
        //         }
        //     }

        //     if (mListener != null && mListener.enabled && mListener.gameObject.activeSelf)
        //     {
        //         AudioSource source = mListener.GetComponent<AudioSource>();
        //         if (source == null) source = mListener.gameObject.AddComponent<AudioSource>();
        //         source.pitch = pitch;
        //         source.volume = volume;
        //         source.PlayOneShot(clip, volume);
        //         return source;
        //     }
        // }
        //if(clip == null || GlobalObject.soundPlayer == null)
        //{
        //    return null;
        //}
        //GlobalObject.soundPlayer.Play(clip);
        return null;
    }
}
