using Colyseus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyServerManager : ColyseusManager<MyServerManager>
{
    protected override void Start()
    {
        base.Start();
        MyServerManager.Instance.InitializeClient();
        Debug.Log($"Init client {client.ToString()}");
    }
}
