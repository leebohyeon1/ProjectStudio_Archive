using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� �̱��� Ŭ������ ����Ͽ� ������ �̱��� ������ ������ �� �ֽ��ϴ�.
/// </summary>
public class Singleton_LiteVer : MonoBehaviour
{
    private static Singleton_LiteVer _instance;
    private static bool _applicationQuitting = false;

    /// <summary>
    /// �̱��� �ν��Ͻ��� �����ϱ� ���� �Ӽ��Դϴ�.
    /// </summary>
    public static Singleton_LiteVer Instance
    {
        get
        {
            if (_applicationQuitting)
            {
                Debug.LogWarning("[Singleton] �ν��Ͻ� '" + typeof(Singleton_LiteVer) + "'�� �̹� �ı��Ǿ����ϴ�. null�� ��ȯ�մϴ�.");
                return null;
            }
            return _instance;
        }
    }

    private void Awake()
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
