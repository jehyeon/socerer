using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Spawning;
using MLAPI.Transports.UNET;
using System;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField] private GameObject _connectionButtonPanel;
    [SerializeField] private string ipAddress = "127.0.0.1";

    UNetTransport transport;

    // 서버에서 동작
    public void Host()
    {
        _connectionButtonPanel.SetActive(false);
        // ConnectionApprovalCallback is not called by the Client host
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost(Vector3.zero, Quaternion.identity);
    }

    // 서버에서 동작
    private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
    {
        // check the incoming data
        bool approve = System.Text.Encoding.ASCII.GetString(connectionData) == "Password1234";
        // null: 기본 Player 프리팹
        callback(true, null, approve, Vector3.zero, Quaternion.identity);
    }

    public void Join()
    {
        // NetworkManager - U Net Transport - Connect Address 
        transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        transport.ConnectAddress = ipAddress;

        _connectionButtonPanel.SetActive(false);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("Password1234");
        NetworkManager.Singleton.StartClient();
    }

    public void IPAddressChanged(string newAddress)
    {
        // IP Input Field에 입력 시 초기화가 됨
        this.ipAddress = newAddress;
    }
}
