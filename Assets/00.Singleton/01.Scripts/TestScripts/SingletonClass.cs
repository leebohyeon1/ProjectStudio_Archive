using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonClass : Singleton_ClassVer<SingletonClass>
{
    protected override void Start()
    {
        base.Start();
       
    }

    public void Call()
    {
        Debug.Log("�̱��� Ŭ���� ����");
    }
}
