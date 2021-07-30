using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public override void OnStartServer()
    {
        Debug.Log("Server Started");
    }

    public override void OnStopServer()
    {
        Debug.Log("Sever Stopped");
    }

    public override void OnClientConnect(NetworkConnection connection)
    {
        Debug.Log("Connected to Server");
    }

    public override void OnClientDisconnect(NetworkConnection connection)
    {
        Debug.Log("Disconnected to Server");
    }
}
