using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 싱글톤 클래스 생성이나 자신이 싱글톤 클래스가 되는 테스트를 하는 스크립트
/// </summary>
public class SingletonTest : MonoBehaviour
{
    /// <summary>
    /// _isThisSingleton이 True일 경우 자신이 직접 싱글턴 오브젝트가 된다.
    /// </summary>
    [SerializeField] private bool _isThisSingleton = false;

    private static SingletonTest _instance;
    private static bool _applicationQuitting = false;

    /// <summary>
    /// 싱글톤 인스턴스에 접근하기 위한 속성입니다.
    /// </summary>
    public static SingletonTest Instance
    {
        get
        {
            if (_applicationQuitting)
            {
                Debug.LogWarning("[Singleton] 인스턴스 '" + typeof(SingletonTest) + "'는 이미 파괴되었습니다. null을 반환합니다.");
                return null;
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_isThisSingleton)
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
        else
        {
            SingletonClass.Instance.Call();
        }
    }

    private void Start()
    {

    }

    private void OnApplicationQuit()
    {
        if (_isThisSingleton)
        {
            _applicationQuitting = true;
        }
    }

    private void OnDestroy()
    {
        if (_isThisSingleton)
        {
            if (_instance == this)
            {
                _applicationQuitting = true;
                _instance = null;
            }
        }
    }
}

