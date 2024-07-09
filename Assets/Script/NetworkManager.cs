using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public static NetworkRunner runnerInstance;
    public string lobbyName = "defalut";

    public Transform sessionListContentParent;
    public GameObject sessionListEntryPrefab;
    public Dictionary<string, GameObject> sessionListUIDict = new(); 


    private void Awake()
    {
        runnerInstance = gameObject.GetComponent<NetworkRunner>();
        if(runnerInstance == null)
        {
            runnerInstance = gameObject.AddComponent<NetworkRunner>();
        } 
    }

    private void Start()
    {
        runnerInstance.JoinSessionLobby(SessionLobby.Shared, lobbyName);
    }

    public void CreateRandomSession()
    {
        string randomSessionName = $"Room {UnityEngine.Random.Range(1000, 9999)}";
        runnerInstance.StartGame(new StartGameArgs()
        { 
            GameMode = GameMode.Shared,
            SessionName = randomSessionName,
        });
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if(player == runnerInstance.LocalPlayer)
        {
            SceneManager.LoadScene("Main");
        }
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("session list updated");
        DeleteOldSessionFromUI(sessionList);

        CompareLists(sessionList);

        
    }

    private void CompareLists(List<SessionInfo> sessionList)
    {
        foreach(var item in sessionList)
        {
            if(sessionListUIDict.ContainsKey(item.Name))
            {
                UpdateEntryUI(item);
            }
            else
            {
                CreateEntryUI(item);
            }
        }
    }

    private void UpdateEntryUI(SessionInfo session)
    {
        var newEntry = sessionListUIDict[session.Name];
        SessionListEntry entryScript = newEntry.GetComponent<SessionListEntry>();

        entryScript.roomName.text = session.Name;
        entryScript.playerCount.text = session.PlayerCount.ToString() + "/" + session.MaxPlayers.ToString();
        entryScript.joinButton.interactable = session.IsOpen;

        newEntry.gameObject.SetActive(session.IsVisible);
    }

    private void CreateEntryUI(SessionInfo session)
    {
        var newEntry = Instantiate(sessionListEntryPrefab, sessionListContentParent);

        SessionListEntry entryScript = newEntry.GetComponent<SessionListEntry>();

        sessionListUIDict.Add(session.Name, newEntry);
        entryScript.roomName.text = session.Name;
        entryScript.playerCount.text = session.PlayerCount.ToString() + "/" + session.MaxPlayers.ToString();
        entryScript.joinButton.interactable = session.IsOpen;

        newEntry.gameObject.SetActive(session.IsVisible);
    }

    private void DeleteOldSessionFromUI(List<SessionInfo> sessionList)
    {
        bool isContained = false;
        GameObject UIToDelete = null;

        foreach (var entry in sessionListUIDict)
        {
            foreach (SessionInfo session in sessionList)
            {
                if (session.Name == entry.Key)
                {
                    isContained = true;
                    break;
                }
            }

            if(!isContained)
            {
                UIToDelete = entry.Value;
                sessionListUIDict.Remove(entry.Key);
                Destroy(UIToDelete);
            }
        }
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }
}
