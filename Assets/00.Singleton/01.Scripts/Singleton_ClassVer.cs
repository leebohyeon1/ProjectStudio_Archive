using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이 싱글톤 클래스를 상속하여 싱글톤을 만들 수 있습니다.
/// 사용 시 클래스 이름을 변경하여 사용하는 것을 추천합니다 (예: Singleton_ClassVer -> Singleton).
/// 사용 방법: public class MyClassName : Singleton<MyClassName> {}
/// </summary>
public class Singleton_ClassVer<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static bool _applicationQuitting = false;
    private static object _lock = new object();

    /// <summary>
    /// 이 속성을 통해 싱글톤 인스턴스에 접근합니다.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_applicationQuitting)
            {
                Debug.LogWarning("[Singleton] 인스턴스 '" + typeof(T) + "'는 이미 파괴되었습니다. null을 반환합니다.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    // 기존 인스턴스 검색
                    _instance = FindObjectOfType<T>();

                    // 존재하지 않을 경우 새 인스턴스 생성
                    if (_instance == null)
                    {
                        // 싱글톤을 부착할 새로운 GameObject 생성
                        var singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        // 인스턴스를 씬 전환 시에도 유지
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake() 
    {
        // 싱글톤 중복 방지
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Debug.LogWarning("[Singleton] 중복 인스턴스가 발견되어 파괴됩니다: " + gameObject.name);
                Destroy(gameObject);
            }
        }
    }

    protected virtual void OnEnable() { }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void FixedUpdate() { }

    protected virtual void LateUpdate() { }

    protected virtual void OnDisable() { }

    private void OnApplicationQuit()
    {
        _applicationQuitting = true;
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _applicationQuitting = true;
        }
    }
}
