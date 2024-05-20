using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JoinRelayUI : MonoBehaviour
{
    public TMP_InputField input;

    public TestRelay testRelay;


    public void JoinRelay()
    {
        testRelay.JoinRelay(input.text);
    }
}
