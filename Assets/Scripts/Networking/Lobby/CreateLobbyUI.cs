using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreateLobbyUI : MonoBehaviour
{
    public TMP_InputField lobbyName;

    public Slider lobbySize;

    public void CreateLobby()
    {
        TestLobby.Instance.CreateLobby(lobbyName.text, (int)lobbySize.value, true);
    }
}
