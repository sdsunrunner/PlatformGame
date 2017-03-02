using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*rule one:
explosions always play completely

rule 2: 
there can only be 3 sounds of the same weapon type playing while the first one is still playing.

rule 3:
update the newest unique weapon sounds

rule 4:
there can only be a maximum of 9-12 wepapon sounds playing altogether.

rule 5:
if the same weapon sound is 0.1 sec or less than the previous sound, do not play

*/

public class InuSFXManager
{
    static InuSFXManager sInstance;

    public static InuSFXManager instance
    {
        get
        {
            if (sInstance == null)
            {
                sInstance = new InuSFXManager();
            }
            return sInstance;
        }
    }

    const int MAX_SFX_NUMBER = 12;
    const int MAX_SAME_SFX_NUMBER = 3;
    const float MIN_SAME_SFX_INTERVAL = 0.2f;

    public List<AudioSource> mAudioes = new List<AudioSource>();
    List<string> mClipNames = new List<string>();
    Dictionary<string, float> mAudioPlayTime = new Dictionary<string, float>();
    Dictionary<string, int> mSameSfxNumber = new Dictionary<string, int>();

    public void Clear()
    {
        mAudioes.Clear();
        mClipNames.Clear();
        mAudioPlayTime.Clear();
        mSameSfxNumber.Clear();
    }
    public void Play(AudioSource _audio, string _clipName)
    {
        // check same sfx limitation
        if (!CheckSameLimit(_clipName))
            return;

        if (!mAudioes.Contains(_audio))
        {
            if (mSameSfxNumber.ContainsKey(_clipName))
            {
                mSameSfxNumber[_clipName]++;
                mAudioPlayTime[_clipName] = Time.time;
            }
            else
            {
                mSameSfxNumber.Add(_clipName, 1);
                mAudioPlayTime.Add(_clipName, Time.time);
            }

            _audio.Stop();

            mAudioes.Add(_audio);
            mClipNames.Add(_clipName);

            // check total sfx limitation
            CheckTotalLimit();
        }
        else
        {
            if (mSameSfxNumber.ContainsKey(_clipName))
            {
                mSameSfxNumber[_clipName]++;
                mAudioPlayTime[_clipName] = Time.time;
            }
            else
            {
                mSameSfxNumber.Add(_clipName, 1);
                mAudioPlayTime.Add(_clipName, Time.time);
            }

            _audio.Stop();

            int index = mAudioes.IndexOf(_audio);
            mAudioes.RemoveAt(index);
            mAudioes.Add(_audio);

            string prev_clipname = mClipNames[index];
            if (mSameSfxNumber.ContainsKey(prev_clipname))
            {
                mSameSfxNumber[prev_clipname]--;
            }
            mClipNames.RemoveAt(index);
            mClipNames.Add(_clipName);
        }

        // play sfx
        AudioClip clip =InuResources.GetSfx(_clipName);
        if (clip)
        {
            _audio.clip = clip;
            _audio.volume = 1;            
            _audio.Play();
        }
    }

    bool CheckSameLimit(string _clipName)
    {
        if (mSameSfxNumber.ContainsKey(_clipName))
        {
            // rule 5: if the same weapon sound is 0.1 sec or less than the previous sound, do not play
            if (Time.time - mAudioPlayTime[_clipName] <= MIN_SAME_SFX_INTERVAL)
                return false;

            // remove stopped audio
            if (mSameSfxNumber[_clipName] >= MAX_SAME_SFX_NUMBER)
            {
                for (int i = mClipNames.Count - 1; i >= 0; i--)
                {
                    if (mClipNames[i] == _clipName)
                    {
                        if (mAudioes[i])
                        {
                            if (!mAudioes[i].isPlaying)
                            {
                                // MUST STOP
                                mAudioes[i].Stop();
                                mSameSfxNumber[_clipName]--;
                                mAudioes.RemoveAt(i);
                                mClipNames.RemoveAt(i);
                            }
                        }
                        else
                        {
                            mSameSfxNumber[_clipName]--;
                            mAudioes.RemoveAt(i);
                            mClipNames.RemoveAt(i);
                        }
                    }
                }
            }

            if (mSameSfxNumber[_clipName] >= MAX_SAME_SFX_NUMBER)
            {
                // rule one: explosions always play completely
                if (_clipName.Contains("Explosion"))
                {
                    return false;
                }
                // rule 3:	update the newest unique weapon sounds
                else
                {
                    for (int i = 0; i < mClipNames.Count; i++)
                    {
                        if (mClipNames[i] == _clipName)
                        {
                            if (mAudioes[i])
                            {
                                // MUST STOP
                                mAudioes[i].Stop();
                                mSameSfxNumber[_clipName]--;
                                mAudioes.RemoveAt(i);
                                mClipNames.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }
        }
        return true;
    }

    void CheckTotalLimit()
    {
        if (mAudioes.Count > MAX_SFX_NUMBER)
        {
            for (int i = mAudioes.Count - 2; i >= 0; i--)
            {
                if (mAudioes[i])
                {
                    if (!mAudioes[i].isPlaying)
                    {
                        string clip = mClipNames[i];
                        // MUST STOP
                        mAudioes[i].Stop();
                        mAudioes.RemoveAt(i);
                        mClipNames.RemoveAt(i);
                        if (mSameSfxNumber.ContainsKey(clip))
                        {
                            mSameSfxNumber[clip]--;
                        }
                    }
                }
                else
                {
                    string clip = mClipNames[i];
                    mAudioes.RemoveAt(i);
                    mClipNames.RemoveAt(i);
                    if (mSameSfxNumber.ContainsKey(clip))
                    {
                        mSameSfxNumber[clip]--;
                    }
                }
            }
        }

        if (mAudioes.Count > MAX_SFX_NUMBER)
        {
            string clip = mClipNames[0];
            if (mAudioes[0])
            {
                // MUST STOP
                mAudioes[0].Stop();
            }
            mAudioes.RemoveAt(0);
            mClipNames.RemoveAt(0);

            if (mSameSfxNumber.ContainsKey(clip))
            {
                mSameSfxNumber[clip]--;
            }
        }
    }

    public void Destroy(AudioSource _audio)
    {
        if (mAudioes.Contains(_audio))
        {
            int index = mAudioes.IndexOf(_audio);
            string clip = mClipNames[index];
            if (mAudioes[index])
            {
                // MUST STOP
                mAudioes[index].Stop();
            }
            mAudioes.RemoveAt(index);
            mClipNames.RemoveAt(index);

            if (mSameSfxNumber.ContainsKey(clip))
            {
                mSameSfxNumber[clip]--;
            }
        }
    }
}

