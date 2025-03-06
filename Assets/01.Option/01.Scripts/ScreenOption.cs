using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ȭ�� ������ �����ϴ� Ŭ����
/// �ػ�, ������ ����Ʈ, ȭ�� ��� ���� ������ �� �ִ� ��� ����
/// ������ ��� ����� �÷��̾��������� ����ϰ� ����
/// </summary>
public class ScreenOption : MonoBehaviour
{
    // �ػ� ��Ӵٿ�
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    private List<Resolution> _uniqueResolutions; // ������ �ػ� ����Ʈ

    // ������ ����Ʈ ��Ӵٿ�
    [SerializeField] private TMP_Dropdown _frameRateDropdown;
    private readonly List<int> _frameRates = new List<int> { 30, 45, 60, 75, 120, 144, 240 }; // ���� ������ ������ ����Ʈ ����Ʈ

    // ȭ�� ��� ��Ӵٿ�
    [SerializeField] private TMP_Dropdown _screenModeDropdown;
    public enum ScreenMode
    {
        _fullScreenWindow = 0, // ��ü ȭ�� ���
        _window = 1            // â ���
    }

    // ����� �������� �ε��� ���� ����
    private int _resolutionIndex;
    private int _frameRateIndex;
    private int _screenModeIndex;

    //==========================================================

    void Start()
    {
        // �� ��Ӵٿ� �ʱ�ȭ
        ClearResolution();
        ClearFrame();
        ClearScreenMode();

        // �ʱ� ������ ����
        SetInitialResolution();
        SaveOption();
    }
    //==========================================================

    #region �ػ� ����

    // �ػ� ��Ӵٿ� �ʱ�ȭ
    void ClearResolution()
    {
        Resolution[] allResolutions = Screen.resolutions; // ��� ������ ��� �ػ� ��������
        _uniqueResolutions = allResolutions.Distinct(new ResolutionComparer()).ToList(); // �ߺ� ���� �� ����Ʈ�� ��ȯ
        _resolutionDropdown.ClearOptions(); // ���� �ɼ� ����

        List<string> resolutionOptions = new List<string>(); // ��Ӵٿ �߰��� �ػ� �ɼ� ����Ʈ
        int currentResolutionIndex = 0; // ���� �ػ��� �ε��� ����

        for (int i = 0; i < _uniqueResolutions.Count; i++)
        {
            string option = _uniqueResolutions[i].width + " x " + _uniqueResolutions[i].height;
            resolutionOptions.Add(option);

            if (_uniqueResolutions[i].width == Screen.currentResolution.width &&
                _uniqueResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i; // ���� �ػ󵵸� �ε����� ����
            }
        }

        _resolutionDropdown.AddOptions(resolutionOptions); // �ػ� �ɼ� �߰�
        _resolutionDropdown.value = PlayerPrefs.GetInt("resolution", currentResolutionIndex); // ����� �ػ� �� �ε�
        _resolutionDropdown.RefreshShownValue(); // ��Ӵٿ� ����

        // ��Ӵٿ� �� ���� �� �̺�Ʈ �߰�
        _resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    // �ػ� ����
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _uniqueResolutions[resolutionIndex];
        this._resolutionIndex = resolutionIndex;
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen); // �ػ� ����
    }

    // �ʱ� �ػ� ����
    private void SetInitialResolution()
    {
        int savedResolutionIndex = PlayerPrefs.GetInt("resolution", -1);

        if (savedResolutionIndex == -1)
        {
            // ����� ���� ���� ��� ���� ����� �ػ󵵷� ����
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);

            for (int i = 0; i < _uniqueResolutions.Count; i++)
            {
                if (_uniqueResolutions[i].width == currentResolution.width && _uniqueResolutions[i].height == currentResolution.height)
                {
                    this._resolutionIndex = i;
                    break;
                }
            }
        }
        else
        {
            // ����� ���� ���� ��� �ش� ������ ����
            SetResolution(savedResolutionIndex);
        }
    }

    #endregion

    #region ������ ����Ʈ ����

    // ������ ����Ʈ ��Ӵٿ� �ʱ�ȭ
    void ClearFrame()
    {
        _frameRateDropdown.ClearOptions();
        List<string> frameRateOptions = _frameRates.ConvertAll(rate => rate + " FPS");
        _frameRateDropdown.AddOptions(frameRateOptions);

        int savedFrameRateIndex = PlayerPrefs.GetInt("frameRate", -1);
        if (savedFrameRateIndex == -1)
        {
            savedFrameRateIndex = _frameRates.IndexOf(60); // �⺻��: 60 FPS
            _frameRateIndex = savedFrameRateIndex;
        }

        _frameRateDropdown.value = savedFrameRateIndex;
        _frameRateDropdown.RefreshShownValue();

        // ��Ӵٿ� �� ���� �� �̺�Ʈ �߰�
        _frameRateDropdown.onValueChanged.AddListener(SetFrameRate);

        SetFrameRate(savedFrameRateIndex);
    }

    // ������ ����Ʈ ����
    public void SetFrameRate(int frameRateIndex)
    {
        int frameRate = _frameRates[frameRateIndex];
        this._frameRateIndex = frameRateIndex;
        Application.targetFrameRate = frameRate; // ������ ����Ʈ ����
    }

    #endregion

    #region ȭ�� ��� ����

    // ȭ�� ��� ��Ӵٿ� �ʱ�ȭ
    void ClearScreenMode()
    {
        List<string> options = new List<string> { "FullScreen", "WindowScreen" };

        _screenModeDropdown.ClearOptions();
        _screenModeDropdown.AddOptions(options);
        _screenModeDropdown.onValueChanged.AddListener(index => ChangeFullScreenMode((ScreenMode)index));

        int screenModeIndex = PlayerPrefs.GetInt("screenMode", -1);
        if (screenModeIndex == -1)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
        }
        else
        {
            switch (screenModeIndex)
            {
                case 0:
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
                    break;
                case 1:
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.Windowed);
                    break;
            }
        }
    }

    // ȭ�� ��� ����
    private void ChangeFullScreenMode(ScreenMode mode)
    {
        switch (mode)
        {
            case ScreenMode._fullScreenWindow:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                _screenModeIndex = (int)ScreenMode._fullScreenWindow;
                break;
            case ScreenMode._window:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                _screenModeIndex = (int)ScreenMode._window;
                break;
        }
    }

    #endregion

    //==========================================================

    // �ɼ� ����
    public void SaveOption()
    {
        PlayerPrefs.SetInt("resolution", _resolutionIndex);
        PlayerPrefs.SetInt("frameRate", _frameRateIndex);
        PlayerPrefs.SetInt("screenMode", _screenModeIndex);
        PlayerPrefs.Save();
    }

    // �ɼ� ����
    public void ResetOption()
    {
        _resolutionIndex = PlayerPrefs.GetInt("resolution");
        _frameRateIndex = PlayerPrefs.GetInt("frameRate");
        _screenModeIndex = PlayerPrefs.GetInt("screenMode");

        SetResolution(_resolutionIndex);
        _resolutionDropdown.value = _resolutionIndex;

        SetFrameRate(_frameRateIndex);
        _frameRateDropdown.value = _frameRateIndex;

        ChangeFullScreenMode((ScreenMode)_screenModeIndex);
        _screenModeDropdown.value = _screenModeIndex;
    }
}

//==========================================================

// �ػ� �� Ŭ����
public class ResolutionComparer : IEqualityComparer<Resolution>
{
    public bool Equals(Resolution x, Resolution y)
    {
        return x.width == y.width && x.height == y.height;
    }

    public int GetHashCode(Resolution obj)
    {
        return obj.width.GetHashCode() ^ obj.height.GetHashCode();
    }
}
