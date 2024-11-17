using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 화면 설정을 관리하는 클래스
/// 해상도, 프레임 레이트, 화면 모드 등을 설정할 수 있는 기능 포함
/// 저장의 경우 현재는 플레이어프리팹을 사용하고 있음
/// </summary>
public class ScreenOption : MonoBehaviour
{
    // 해상도 드롭다운
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    private List<Resolution> _uniqueResolutions; // 고유한 해상도 리스트

    // 프레임 레이트 드롭다운
    [SerializeField] private TMP_Dropdown _frameRateDropdown;
    private readonly List<int> _frameRates = new List<int> { 30, 45, 60, 75, 120, 144, 240 }; // 설정 가능한 프레임 레이트 리스트

    // 화면 모드 드롭다운
    [SerializeField] private TMP_Dropdown _screenModeDropdown;
    public enum ScreenMode
    {
        _fullScreenWindow = 0, // 전체 화면 모드
        _window = 1            // 창 모드
    }

    // 사용자 설정값의 인덱스 저장 변수
    private int _resolutionIndex;
    private int _frameRateIndex;
    private int _screenModeIndex;

    // 저장 및 리셋 버튼
    [SerializeField] private Button _saveBtn;
    [SerializeField] private Button _resetBtn;

    //==========================================================

    void Start()
    {
        // 각 드롭다운 초기화
        ClearResolution();
        ClearFrame();
        ClearScreenMode();

        // 초기 설정값 적용
        SetInitialResolution();
        SaveOption();

        // 버튼 클릭 이벤트 추가
        _saveBtn.onClick.AddListener(() => { SaveBtn(); });
        _resetBtn.onClick.AddListener(() => { ResetBtn(); });
    }
    //==========================================================

    #region 해상도 설정

    // 해상도 드롭다운 초기화
    void ClearResolution()
    {
        Resolution[] allResolutions = Screen.resolutions; // 사용 가능한 모든 해상도 가져오기
        _uniqueResolutions = allResolutions.Distinct(new ResolutionComparer()).ToList(); // 중복 제거 후 리스트로 변환
        _resolutionDropdown.ClearOptions(); // 기존 옵션 제거

        List<string> resolutionOptions = new List<string>(); // 드롭다운에 추가할 해상도 옵션 리스트
        int currentResolutionIndex = 0; // 현재 해상도의 인덱스 저장

        for (int i = 0; i < _uniqueResolutions.Count; i++)
        {
            string option = _uniqueResolutions[i].width + " x " + _uniqueResolutions[i].height;
            resolutionOptions.Add(option);

            if (_uniqueResolutions[i].width == Screen.currentResolution.width &&
                _uniqueResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i; // 현재 해상도를 인덱스로 저장
            }
        }

        _resolutionDropdown.AddOptions(resolutionOptions); // 해상도 옵션 추가
        _resolutionDropdown.value = PlayerPrefs.GetInt("resolution", currentResolutionIndex); // 저장된 해상도 값 로드
        _resolutionDropdown.RefreshShownValue(); // 드롭다운 갱신

        // 드롭다운 값 변경 시 이벤트 추가
        _resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    // 해상도 설정
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _uniqueResolutions[resolutionIndex];
        this._resolutionIndex = resolutionIndex;
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen); // 해상도 적용
    }

    // 초기 해상도 설정
    private void SetInitialResolution()
    {
        int savedResolutionIndex = PlayerPrefs.GetInt("resolution", -1);

        if (savedResolutionIndex == -1)
        {
            // 저장된 값이 없을 경우 현재 모니터 해상도로 설정
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
            // 저장된 값이 있을 경우 해당 값으로 설정
            SetResolution(savedResolutionIndex);
        }
    }

    #endregion

    #region 프레임 레이트 설정

    // 프레임 레이트 드롭다운 초기화
    void ClearFrame()
    {
        _frameRateDropdown.ClearOptions();
        List<string> frameRateOptions = _frameRates.ConvertAll(rate => rate + " FPS");
        _frameRateDropdown.AddOptions(frameRateOptions);

        int savedFrameRateIndex = PlayerPrefs.GetInt("frameRate", -1);
        if (savedFrameRateIndex == -1)
        {
            savedFrameRateIndex = _frameRates.IndexOf(60); // 기본값: 60 FPS
            _frameRateIndex = savedFrameRateIndex;
        }

        _frameRateDropdown.value = savedFrameRateIndex;
        _frameRateDropdown.RefreshShownValue();

        // 드롭다운 값 변경 시 이벤트 추가
        _frameRateDropdown.onValueChanged.AddListener(SetFrameRate);

        SetFrameRate(savedFrameRateIndex);
    }

    // 프레임 레이트 설정
    public void SetFrameRate(int frameRateIndex)
    {
        int frameRate = _frameRates[frameRateIndex];
        this._frameRateIndex = frameRateIndex;
        Application.targetFrameRate = frameRate; // 프레임 레이트 적용
    }

    #endregion

    #region 화면 모드 설정

    // 화면 모드 드롭다운 초기화
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

    // 화면 모드 변경
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

    #region 버튼

    // 저장 버튼 클릭 시 호출
    public void SaveBtn()
    {
        SaveOption();
    }

    // 리셋 버튼 클릭 시 호출
    public void ResetBtn()
    {
        ResetOption();
    }

    #endregion

    //==========================================================

    // 옵션 저장
    void SaveOption()
    {
        PlayerPrefs.SetInt("resolution", _resolutionIndex);
        PlayerPrefs.SetInt("frameRate", _frameRateIndex);
        PlayerPrefs.SetInt("screenMode", _screenModeIndex);
        PlayerPrefs.Save();
    }

    // 옵션 리셋
    void ResetOption()
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

// 해상도 비교 클래스
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
