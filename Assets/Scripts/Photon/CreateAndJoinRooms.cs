using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

using Photon.Realtime;
using Photon.Pun;
//using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public Main main;
    public TMP_InputField roomCode;
    private bool pressed;
    public PhotonView view;

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

    public void CreateRoom() {
        if(!string.IsNullOrWhiteSpace(roomCode.text) && !pressed) {
            //Debug.Log("Creating");
            pressed = true;
            RoomOptions roomOptions = new RoomOptions() {
                MaxPlayers = (byte)2,
                IsVisible = false
            };
            PhotonNetwork.CreateRoom(roomCode.text, roomOptions, null);
        }
    }

    public void JoinRoom() {
        if(!string.IsNullOrWhiteSpace(roomCode.text) && !pressed) {
            //Debug.Log("Joining");
            pressed = true;
            PhotonNetwork.JoinRoom(roomCode.text);
        }
    }

    public override void OnCreatedRoom() {
        //StartCoroutine(JoiningRoom()); doesn't work?
        //view.RPC("PlayerConnected", RpcTarget.All);
        //Debug.Log("Created");
        pressed = false;
        main.SetGameState(11);
    }

    public override void OnJoinedRoom() {
        //StartCoroutine(JoiningRoom()); doesn't work?
        //view.RPC("PlayerConnected", RpcTarget.All);
        //Debug.Log("Joined");
        pressed = false;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        //StartCoroutine(JoiningRoom()); doesn't work?
        view.RPC("PlayerConnected", RpcTarget.All);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        main.SetGameState(9);
        pressed = false;
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        //Debug.Log("Join Fail");
        pressed = false;
        CreateRoom();
    }

    /*IEnumerator JoiningRoom() {
        transition.SetActive(true);
        transition.GetComponent<AudioSource>().Play();
        transition.GetComponent<Animator>().SetTrigger("Close");

        yield return new WaitForSeconds(0.75f);

        PhotonNetwork.LoadLevel("Versus");
    }*/
}
