using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const int SEChannelCount = 8;

    private static SoundManager singletonInstance;

    public static SoundManager SingletonInstance
    {
        get
        {
            if (singletonInstance == null)
            {
                var obj = new GameObject("SoundManager");
                DontDestroyOnLoad(obj);
                singletonInstance = obj.AddComponent<SoundManager>();
            }

            return singletonInstance;
        }
    }


    private AudioSource bgmSource;
    private List<AudioSource> seSources = new List<AudioSource>();

    private AudioClip[] seClips;
    private AudioClip[] bgmClips;
    
    private Dictionary<string, int> seIndexes = new Dictionary<string, int>();
    private Dictionary<string, int> bgmIndexes = new Dictionary<string, int>();

    private int seSourceIdx = 0;

    private void Awake()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        for (int i = 0; i < SEChannelCount; i++)
        {
            var seSource = gameObject.AddComponent<AudioSource>();
            seSource.playOnAwake = false;
            seSources.Add(seSource);
        }

        seClips = Resources.LoadAll<AudioClip>("AudioClip/SE");
        bgmClips = Resources.LoadAll<AudioClip>("AudioClip/BGM");

        for (int i = 0; i < seClips.Length; i++)
        {
            seIndexes[seClips[i].name] = i;
        }

        for (int i = 0; i < bgmClips.Length; i++)
        {
            bgmIndexes[bgmClips[i].name] = i;
        }
    }

    public void PlayBGM(string clipName, bool useLoop, float volume)
    {
        bgmSource.clip = bgmClips[bgmIndexes[clipName]];
        bgmSource.loop = useLoop;
        bgmSource.volume = volume;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySE(string clipName, bool useLoop, float volume)
    {
        var seSource = seSources[seSourceIdx];
        seSource.clip = seClips[seIndexes[clipName]];
        seSource.loop = useLoop;
        seSource.volume = volume;
        seSource.Play();
        seSourceIdx = (seSourceIdx + 1) % seSources.Count;
    }
}
