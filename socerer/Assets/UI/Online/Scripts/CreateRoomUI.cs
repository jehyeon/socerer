using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}

public class CreateGameRoomData
{
    public int temporaryCount;
    public int maxPlayerCount;
}