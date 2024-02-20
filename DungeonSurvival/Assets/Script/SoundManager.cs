using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : Singleton<SoundManager>
{
    public float mainVolume = 1.0f;
    public float bgmVolume = 1.0f;
    public float sfxVolume = 1.0f;

    [field: SerializeField] public AudioMixer Mixer { get; set; }
    [field: SerializeField] public AudioSource BgmSource { get; set; }
    [field: SerializeField] public AudioSource SfxReferenceSource { get; set; }
    [SerializeField] private SoundSO commonMonsterSound;
    private Queue<AudioSource> _sfxPool;
    
    

    public override void Awake()
    {
        base.Awake();

        const int PoolLength = 16;

        _sfxPool = new Queue<AudioSource>();

        GameObject pool = new GameObject("SFXPool");
        pool.transform.SetParent(transform);

        for (int i = 0; i < PoolLength; ++i)
        {
            var source = Instantiate(SfxReferenceSource, pool.transform);

            _sfxPool.Enqueue(source);
        }

        commonMonsterSound.InitSoundDictionary();
        
        
        
    }


    // Start is called before the first frame update
    void Start()
    {
        UpdateVolume();
    }

    
    
    public void PlaySfxAt(Vector3 position, AudioClip clip, bool spatialized)
    {
        var source = _sfxPool.Dequeue();

        source.clip = clip;
        source.transform.position = position;

        source.spatialBlend = spatialized ? 1.0f : 0.0f;
        
        source.Play();
        
        _sfxPool.Enqueue(source);
    }
    
    public void PlayCommonMonsterSfxAt(Vector3 position, string clipName, bool spatialized)
    {
        var source = _sfxPool.Dequeue();

        source.clip = commonMonsterSound.soundDictionary[clipName];
        source.transform.position = position;

        source.spatialBlend = spatialized ? 1.0f : 0.0f;
        
        source.Play();
        
        _sfxPool.Enqueue(source);
    }
    
    public void UpdateVolume()
    {
        Mixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(0.0001f, mainVolume)) * 30.0f);
        Mixer.SetFloat("SfxVolume", Mathf.Log10(Mathf.Max(0.0001f, sfxVolume)) * 30.0f);
        Mixer.SetFloat("BgmVolume", Mathf.Log10(Mathf.Max(0.0001f, bgmVolume)) * 30.0f);
    }

}
