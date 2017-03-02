using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class InuResources
{
    public static readonly string ASSET_PATH_PREFIX = "Assets/InuEditor/";

    public static readonly string PREFAB_SUFFIX = ".prefab";
    public static readonly string SFX_SUFFIX = ".mp3";
    public static readonly string SFX2_SUFFIX = ".wav";

    public static readonly string PATH_EFFECT_PREFAB = "InuAssets/effects/";
    public static readonly string PATH_SFX = "InuAssets/sfx/";


    public static List<AudioClip> s_lSfxs = new List<AudioClip>();
    public static void Clear()
    {
        s_lSfxs.Clear();
        InuSFXManager.instance.Clear();
    }

    public static GameObject GetEffectInstance(string _modelName, Vector3 _pos, Quaternion _rotation)
    {
        GameObject result = null;

        GameObject meshPrefab = Resources.Load(ASSET_PATH_PREFIX + PATH_EFFECT_PREFAB + _modelName + PREFAB_SUFFIX, typeof(GameObject)) as GameObject; 
        if (meshPrefab != null)
            result = Object.Instantiate(meshPrefab, _pos, _rotation) as GameObject;
        return result;
    }

    public static AudioClip GetSfx(string _name)
    {
        int sfxIndex = -1;
        for (int i = 0; i < s_lSfxs.Count; i++)
        {
            if (s_lSfxs[i].name.Equals(_name))
            {
                sfxIndex = i;
                break;
            }
        }
        AudioClip clip = null;
        if (sfxIndex >= 0)
        {
            clip = s_lSfxs[sfxIndex];
        }
        else
        {
            clip = Resources.Load(ASSET_PATH_PREFIX + PATH_SFX + _name + SFX_SUFFIX, typeof(AudioClip)) as AudioClip;
            if (clip == null)
            {
                // Attempt with WAV
                clip = Resources.Load(ASSET_PATH_PREFIX + PATH_SFX + _name + SFX2_SUFFIX, typeof(AudioClip)) as AudioClip;
            }

            if (clip)
            {
                s_lSfxs.Add(clip);
            }
            else
            {
                Debug.LogError("null fx!!_name:" + _name);
            }
        }
        return clip;
    }
}

