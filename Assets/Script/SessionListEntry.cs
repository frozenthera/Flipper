using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SessionListEntry : MonoBehaviour
{
    public TextMeshProUGUI roomName, playerCount;
    public Button joinButton;

    public void JoinRoom()
    {
        NetworkManager.runnerInstance.StartGame(new Fusion.StartGameArgs()
        { 
            SessionName = roomName.text,
        });
    }
}
