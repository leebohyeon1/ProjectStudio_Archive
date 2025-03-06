using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AudioManager�� �۵���Ű�� Ŭ����
/// </summary>
public class SoundOption : MonoBehaviour
{
    [SerializeField] private AudioManager _audioManager;

    [Space(20f)]
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Button _bgmButton;
    private bool _isBgmMute = false;

    [Space(10f)]
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Button _sfxButton;
    private bool _isSfxMute = false;


    void Start()
    {
        _bgmButton.onClick.AddListener(() => { MuteBgm(); });
        _sfxButton.onClick.AddListener(() => { MuteSfx(); });

        _bgmSlider.onValueChanged.AddListener(delegate { BgmVolumeChange(_bgmSlider.value); });
        _sfxSlider.onValueChanged.AddListener(delegate { SfxVolumeChange(_sfxSlider.value); });

        ResetOption();
    }

    //========================================

    private void BgmVolumeChange(float volume)
    {
        _audioManager.SetBgmVolume(volume);
    }

    private void MuteBgm()
    {
        _isBgmMute = !_isBgmMute;
        _audioManager.MuteBGM(_isBgmMute);
    }

    //========================================

    private void SfxVolumeChange(float volume)
    {
        _audioManager.SetSfxVolume(volume);
    }

    private void MuteSfx()
    {
        _isSfxMute = !_isSfxMute;
        _audioManager.MuteSFX(_isSfxMute);
    }

    public void SaveOption()
    {
        _audioManager.SaveAudioSettings();
    }

    public void ResetOption()
    {
        _audioManager.LoadAudioSettings();

        _bgmSlider.value = PlayerPrefs.GetFloat("BgmVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");

        _isBgmMute = (PlayerPrefs.GetInt("BgmMuted") == 1);
        _isSfxMute = (PlayerPrefs.GetInt("SfxMuted") == 1);
    }
}
