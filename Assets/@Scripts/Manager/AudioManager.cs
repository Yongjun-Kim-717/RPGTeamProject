using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    AudioMixer _masterMixer;
    AudioSource _backgroundSound;
    AudioSource _warningSound;
    AudioSource _clickSound;

    public AudioMixer MasterMixer
    {
        get { return _masterMixer; }
    }

    protected override void Initialize()
    {
        base.Initialize();
        _masterMixer = ObjectManager.Instance.MasterMixer;

        _backgroundSound = gameObject.AddComponent<AudioSource>();
        _backgroundSound.clip = ObjectManager.Instance.BackgroundSoundClip;
        _backgroundSound.outputAudioMixerGroup = _masterMixer.FindMatchingGroups(Define.BGM)[0];
        _backgroundSound.volume = 0.05f;
        _backgroundSound.loop = true;
        _backgroundSound.Play();
        _warningSound = gameObject.AddComponent<AudioSource>();
        _warningSound.playOnAwake = false;
        _warningSound.clip = ObjectManager.Instance.WarningSoundClip;
        _warningSound.outputAudioMixerGroup = _masterMixer.FindMatchingGroups(Define.VFX)[0];
        _warningSound.volume = 0.2f;
        _clickSound = gameObject.AddComponent<AudioSource>();
        _clickSound.playOnAwake = false;
        _clickSound.clip = ObjectManager.Instance.ClickSoundClip;
        _clickSound.outputAudioMixerGroup = _masterMixer.FindMatchingGroups(Define.Click)[0];
        _clickSound.volume = 0.5f;
    }

    public void SetBGMVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        MasterMixer.SetFloat(Define.BGM, Mathf.Log10(volume) * 20f);
    }

    public void SetVFXVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        MasterMixer.SetFloat(Define.VFX, Mathf.Log10(volume) * 20f);
    }

    public void SetClickVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        MasterMixer.SetFloat(Define.Click, Mathf.Log10(volume) * 20f);
    }

    public void PlayBackgroundSound(bool flag)
    {
        if (flag)
        {
            _backgroundSound.Play();
        }
        else
        {
            _backgroundSound.Stop();
        }
    }

    public void PlayWarningSound(bool flag)
    {
        if (flag)
        {
            _warningSound.Play();
        }
        else
        {
            _warningSound.Stop();
        }
    }

    public void PlayClickSound()
    {
        _clickSound.Play();
    }
}
