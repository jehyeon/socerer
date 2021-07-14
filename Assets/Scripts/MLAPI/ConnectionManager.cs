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

    // �������� ����
    public void Host()
    {
        _connectionButtonPanel.SetActive(false);
        // ConnectionApprovalCallback is not called by the Client host
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost(Vector3.zero, Quaternion.identity);
    }

    // �������� ����
    private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
    {
        // check the incoming data
        bool approve = System.Text.Encoding.ASCII.GetString(connectionData) == "Password1234";
        // null: �⺻ Player ������
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
        // IP Input Field�� �Է� �� �ʱ�ȭ�� ��
        this.ipAddress = newAddress;
    }
}
