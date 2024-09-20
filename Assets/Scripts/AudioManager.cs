using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource bgmSource;
    private List<AudioSource> sfxSources;
    public float sfxVol { get; private set; }
    public float bgmVol { get; private set; }

    public AudioClip BgmTitle;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Create AudioSources for BGM
            GameObject bgmObject = new GameObject("BGM Source");
            bgmObject.transform.parent = transform;
            bgmSource = bgmObject.AddComponent<AudioSource>();
            bgmSource.loop = true;

            sfxSources = new List<AudioSource>();

            bgmVol = PlayerPrefs.GetFloat("BGM Volume", 1);
            sfxVol = PlayerPrefs.GetFloat("SFX Volume", 1);

            bgmSource.volume = bgmVol;
            PlayBgm(BgmTitle);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBgm(AudioClip clip)
    {
        if (clip == null) return;
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void BgmVol(float vol)
    {
        bgmSource.volume = vol;
        PlayerPrefs.SetFloat("BGM Volume", vol);
        PlayerPrefs.Save();
    }
    public void PlaySfx(AudioClip clip, GameObject obj = null, bool loop = false, float pitch = 1f, float volume = 1f)
    {
        if (clip == null) return;

        GameObject sfxObj = new GameObject("SFX Source"); ;
        sfxObj.transform.parent = obj == null ? transform : obj.transform;
        sfxObj.transform.localPosition = Vector3.zero;

        AudioSource sfxSource = sfxObj.AddComponent<AudioSource>();
        if (obj != null)
        {
            sfxSource.rolloffMode = AudioRolloffMode.Linear;
            sfxSource.maxDistance = 50;
            sfxSource.spatialBlend = 1f;
        }
        sfxSource.loop = loop;
        sfxSource.clip = clip;
        sfxSource.pitch = pitch;
        sfxSource.volume = sfxVol;
        sfxSource.Play();
        sfxSources.Add(sfxSource);

        StartCoroutine(ProcessOnDelayRemoveSfx(sfxSource));
    }

    private IEnumerator ProcessOnDelayRemoveSfx(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length + .5f);
        sfxSources.Remove(source);
        Destroy(source.gameObject);
    }

    public void SfxVol(float vol)
    {
        sfxVol = vol;
        foreach (var audioSource in sfxSources)
        {
            audioSource.volume = vol;
        }
        PlayerPrefs.SetFloat("SFX Volume", vol);
        PlayerPrefs.Save();
    }

    public void StopSfx(AudioClip clip)
    {
        for (int i = sfxSources.Count - 1; i >= 0; i--)
        {
            if (sfxSources[i].clip == clip)
            {
                StartCoroutine(FadeOutAndStop(sfxSources[i]));
                sfxSources.RemoveAt(i);
            }
        }
    }

    private IEnumerator FadeOutAndStop(AudioSource source, float duration = 1.0f)
    {
        float startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        source.Stop();
        Destroy(source.gameObject);
    }
}
