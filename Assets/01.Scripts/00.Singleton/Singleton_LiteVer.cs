using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이 싱글톤 클래스를 사용하여 간단한 싱글톤 패턴을 구현할 수 있습니다.
/// </summary>
public class Singleton_LiteVer : MonoBehaviour
{
    private static Singleton_LiteVer _instance;
    private static bool _applicationQuitting = false;

    /// <summary>
    /// 싱글톤 인스턴스에 접근하기 위한 속성입니다.
    /// </summary>
    public static Singleton_LiteVer Instance
    {
        get
        {
            if (_applicationQuitting)
            {
                Debug.LogWarning("[Singleton] 인스턴스 '" + typeof(Singleton_LiteVer) + "'는 이미 파괴되었습니다. null을 반환합니다.");
                return null;
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // 싱글톤 중복 방지
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Debug.LogWarning("[Singleton] 중복 인스턴스가 발견되어 파괴됩니다: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        _applicationQuitting = true;
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _applicationQuitting = true;
            _instance = null;
        }
    }
}
