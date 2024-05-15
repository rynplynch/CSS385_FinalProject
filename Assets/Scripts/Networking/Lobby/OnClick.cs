using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClick : MonoBehaviour
{

    public TestLobby testLobby;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //testLobby.CreateLobby();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            testLobby.ListLobbies();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            //testLobby.QuickJoinLobby();
        }
    }
}
