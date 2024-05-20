using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelayControl : MonoBehaviour
{
    public TestRelay relay;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            relay.CreateRelay();
        }
    }

}
