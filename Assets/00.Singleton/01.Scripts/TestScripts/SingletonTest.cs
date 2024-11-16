using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �̱��� Ŭ���� �����̳� �ڽ��� �̱��� Ŭ������ �Ǵ� �׽�Ʈ�� �ϴ� ��ũ��Ʈ
/// </summary>
public class SingletonTest : MonoBehaviour
{
    /// <summary>
    /// _isThisSingleton�� True�� ��� �ڽ��� ���� �̱��� ������Ʈ�� �ȴ�.
    /// </summary>
    [SerializeField] private bool _isThisSingleton = false;

    private static SingletonTest _instance;
    private static bool _applicationQuitting = false;

    /// <summary>
    /// �̱��� �ν��Ͻ��� �����ϱ� ���� �Ӽ��Դϴ�.
    /// </summary>
    public static SingletonTest Instance
    {
        get
        {
            if (_applicationQuitting)
            {
                Debug.LogWarning("[Singleton] �ν��Ͻ� '" + typeof(SingletonTest) + "'�� �̹� �ı��Ǿ����ϴ�. null�� ��ȯ�մϴ�.");
                return null;
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_isThisSingleton)
        {
            // �̱��� �ߺ� ����
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Debug.LogWarning("[Singleton] �ߺ� �ν��Ͻ��� �߰ߵǾ� �ı��˴ϴ�: " + gameObject.name);
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

