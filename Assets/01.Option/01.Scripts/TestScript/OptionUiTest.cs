using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUiTest : MonoBehaviour
{
    [SerializeField] private ScreenOption _screenOption;
    [SerializeField] private SoundOption _soundOption;
    [SerializeField] private AudioManager _audioManager;

    [Space(20f)]
    [SerializeField] private Button _saveBtn;
    [SerializeField] private Button _resetBtn;
    [SerializeField] private Button[] _sfxBtns;

    void Start()
    {
        _saveBtn.onClick.AddListener(() => { Save(); });
        _resetBtn.onClick.AddListener(() => { Reset(); });

        _sfxBtns[0].onClick.AddListener(() => { PlaySfx(1); });
        _sfxBtns[1].onClick.AddListener(() => { PlaySfx(2); });

        _audioManager.PlayBGM("BGM1");
    }

    void Update()
    {
        
    }

    private void PlaySfx(int num) 
    {
        _audioManager.PlaySFX("SFX" + num);
    }


    private void Save()
    {
        _screenOption.SaveOption();
        _soundOption.SaveOption();
    }

    private void Reset()
    {
        _screenOption.ResetOption();
        _soundOption.ResetOption();
    }

}
