using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public Main main;
    public CreateAndJoinRooms roomManager;

    public void Connect() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Cancel() {
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() {
        //SceneManager.LoadScene("MultiplayerMenu");
        main.SetGameState(9);
    }
}
