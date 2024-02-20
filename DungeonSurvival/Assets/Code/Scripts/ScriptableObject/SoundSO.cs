using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "ScriptableObject/CreateSoundSO",fileName = "SoundSO")]
public class SoundSO : ScriptableObject
{
    public List<AudioClip> commonSound;

    public Dictionary<string, AudioClip> soundDictionary;

    public void InitSoundDictionary()
    {
        soundDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in commonSound)
        {
            soundDictionary.Add(clip.name, clip);
        }
    }

}
