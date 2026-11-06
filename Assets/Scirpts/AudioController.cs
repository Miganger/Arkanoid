using Ball;
using Enemies;
using Enemies.Boss;
using Enums.Audio;
using Paddle;
using PowerUps;
using Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Fade Settings")]
    [SerializeField, Min(0f)] private float fadeDuration = 1f;

    [Header("Audio sources")]
    [SerializeField] private List<AudioSource> SFX;
    [SerializeField] private List<AudioSource> music;

    private Coroutine fadeCoroutine;

    private void OnEnable()
    {
        SaveSystem.OnSave += OnSaveDataChange;
        SaveSystem.OnLoad += OnSaveDataChange;

        Missle.Hit += OnExplosion;
        BossMissle.Hit += OnExplosion;
        PaddleFireController.LunchBall += OnBounce;
        PaddleFireController.LaserFire += OnLaserFire;
        BallBounce.Bounce += OnBounce;
        PowerUpObject.PickedUp += OnPowerUpPickedUp;
        EnemyController.Death += OnExplosion;
    }
    private void OnDisable()
    {
        SaveSystem.OnSave -= OnSaveDataChange;
        SaveSystem.OnLoad -= OnSaveDataChange;

        Missle.Hit -= OnExplosion;
        BossMissle.Hit -= OnExplosion;
        PaddleFireController.LunchBall -= OnBounce;
        PaddleFireController.LaserFire -= OnLaserFire;
        BallBounce.Bounce -= OnBounce;
        PowerUpObject.PickedUp -= OnPowerUpPickedUp;
        EnemyController.Death -= OnExplosion;
    }

    private void OnSaveDataChange()
    {
        SetVolume("SFXVolume", SaveSystem.Data.SFXVolume);
        SetVolume("MusicVolume", SaveSystem.Data.MusicVolume);
    }

    private void SetVolume(string parameter, float value)
    {
        float volumeDb = value <= 0f
            ? -80f
            : Mathf.Log10(value) * 20f;

        audioMixer.SetFloat(parameter, volumeDb);
    }

    private void OnPowerUpPickedUp()
    {
        PlaySoundEffect(ESoundType.PowerUp);
    }
    private void OnLaserFire()
    {
        PlaySoundEffect(ESoundType.Laser);
    }
    private void OnBounce()
    {
        PlaySoundEffect(ESoundType.Bounce);
    }
    private void OnExplosion()
    {
        PlaySoundEffect(ESoundType.Explosion);
    }

    public void PlaySoundEffect(ESoundType soundType)
    {
        SFX[(int)soundType].Play();
    }
    public void PlayMusic(EMusicType musicType)
    {
        StartFade(music[(int)musicType], 1f);
    }
    public void StopMusic(EMusicType musicType)
    {
        StartFade(music[(int)musicType], 0f);
    }

    private void StartFade(AudioSource source, float targetVolume)
    {
        if(fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(Fade(source, targetVolume));
    }
    private IEnumerator Fade(AudioSource source, float targetVolume)
    {
        var startVolume = source.volume;

        if(startVolume == 0f)
            source.Play();

        float elapsedTime = 0f;

        while(elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float t = fadeDuration <= 0f ? 1f : Mathf.Clamp01(elapsedTime / fadeDuration);
            t = Mathf.SmoothStep(0f, 1f, t);

            source.volume = Mathf.Lerp(
                startVolume, targetVolume, t);

            yield return null;
        }

        source.volume = targetVolume;

        if (targetVolume <= 0f)
            source.Stop();

        fadeCoroutine = null;
    }
}


