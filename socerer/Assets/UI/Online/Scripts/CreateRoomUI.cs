using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CreateRoomUI : MonoBehaviour
{
    [SerializeField]
    private List<Image> temporaryImgs;

    [SerializeField]
    private List<Button> temporarayButtons;

    [SerializeField]
    private List<Button> maxPlayerButtons;

    private CreateGameRoomData roomData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void UpdateTemporaryImgs()
    {

    }

    public void CreateRoom()
    {
        var manager = RoomManager.singleton;
        // ¹æ ¼³Á¤
        manager.StartHost();
    }
}

public class CreateGameRoomData
{
    public int temporaryCount;
    public int maxPlayerCount;
}