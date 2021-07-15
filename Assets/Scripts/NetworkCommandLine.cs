using System.Collections.Generic;
using MLAPI;
using UnityEngine;

public class NetworkCommandLine : MonoBehaviour
{
    private NetworkManager netManager;

    void Start()
    {
        netManager = GetComponentInParent<NetworkManager>();

        // Editor���� ������ true
        if (Application.isEditor) return;

        var args = GetCommandlineArgs();

        // out: ������ ���� �μ��� ������ �� ����
        if (args.TryGetValue("-mlapi", out string mlapiValue))
        {
            switch (mlapiValue)
            {
                case "server":
                    netManager.StartServer();
                    break;
                case "host":
                    netManager.StartHost();
                    break;
                case "client":

                    netManager.StartClient();
                    break;
            }
        }
    }

    private Dictionary<string, string> GetCommandlineArgs()
    {
        Dictionary<string, string> argDictionary = new Dictionary<string, string>();

        // ���� ���μ����� ���� ����� �μ��� ���Ե� ���ڿ� �迭 ��ȯ
        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; ++i)
        {
            var arg = args[i].ToLower();
            if (arg.StartsWith("-"))
            {
                var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                // value == null is true: value?.StartsWith() = null
                // value == null is false: value?.StartsWith() = value.StartsWith()
                // ?? ������ null�� �ƴ� ��� ���� �ǿ����� ���� ��ȯ
                value = (value?.StartsWith("-") ?? false) ? null : value;

                argDictionary.Add(arg, value);
            }
        }
        return argDictionary;
    }
}