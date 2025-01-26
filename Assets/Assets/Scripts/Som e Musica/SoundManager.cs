
using System;
using UnityEngine;
using UnityEngine.Audio;
using System;
using TMPro;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public Sound[] musicas, efeitos;
    public AudioSource musicaSource, efeitoSource;

    public Slider volumeMusicaSlider;
    public Slider volumeSFXSlider;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        volumeMusicaSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("musica", 0.5f));
        volumeSFXSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("efeito", 0.5f));
    }
    void Start()
    {
        PlayMusica("tema");
    }
    public void PlayMusica(string nome)
    {
        Sound s = Array.Find(musicas, x => x.nome == nome);

        if (s != null)
        {
            musicaSource.clip = s.clip;
            musicaSource.Play();
        }
    }
    public void PlayEfeito(string nome)
    {
        Sound s = Array.Find(efeitos, x => x.nome == nome);

        if (s != null)
        {
            efeitoSource.PlayOneShot(s.clip);
        }
    }
    public void VolumeMusica(float volume)
    {
        musicaSource.volume = volume;
        PlayerPrefs.SetFloat("musica", volume);
    }
    public void VolumeEfeito(float volume)
    {
        efeitoSource.volume = volume;
        PlayerPrefs.SetFloat("efeito", volume);
    }


}


