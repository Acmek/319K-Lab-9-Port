using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;

public class UART : MonoBehaviour
{
    public PhotonView view;
    public Main main;
    public Queue<char> FIFO = new Queue<char>();
    public ConnectToServer networkConnection;
    public CreateAndJoinRooms roomManager;

    public char UART2_InChar() {
        if (FIFO.Count == 0) {
            return (char)0xFF;
        }
        return FIFO.Dequeue();
    }

    public void UART1_Output(int output) {
        view.RPC("UART1_OutputRPC", RpcTarget.Others, output);
    }

    [PunRPC]
    private void UART1_OutputRPC(int output) {
        FIFO.Enqueue((char)((output & 0xFF000000) >> 24));
        FIFO.Enqueue((char)((output & 0x00FF0000) >> 16));
        FIFO.Enqueue((char)((output & 0x0000FF00) >> 8));
        FIFO.Enqueue((char)((output & 0x000000FF)));
    }

    [PunRPC]
    private void PlayerConnected() {
        main.SetGameState(2);
    }
}
