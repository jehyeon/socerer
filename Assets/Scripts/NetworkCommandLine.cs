using System.Collections.Generic;
using MLAPI;
using UnityEngine;

public class NetworkCommandLine : MonoBehaviour
{
    private NetworkManager netManager;

    void Start()
    {
        netManager = GetComponentInParent<NetworkManager>();

        // Editor에서 돌리면 true
        if (Application.isEditor) return;

        var args = GetCommandlineArgs();

        // out: 참조를 통해 인수를 전달할 수 있음
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

        // 현재 프로세스에 대한 명령줄 인수가 포함된 문자열 배열 반환
        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; ++i)
        {
            var arg = args[i].ToLower();
            if (arg.StartsWith("-"))
            {
                var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                // value == null is true: value?.StartsWith() = null
                // value == null is false: value?.StartsWith() = value.StartsWith()
                // ?? 연산자 null이 아닌 경우 왼쪽 피연산자 값을 반환
                value = (value?.StartsWith("-") ?? false) ? null : value;

                argDictionary.Add(arg, value);
            }
        }
        return argDictionary;
    }
}