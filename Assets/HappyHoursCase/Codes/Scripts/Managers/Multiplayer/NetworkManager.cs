using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Public-Varaibles

    public static NetworkManager instance;

    #endregion

    #region LifeCycle

    private void Awake()
    {
        instance = this;
    }

    /*------------------------------------------------*/

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();
    }

    #endregion

    #region Client



    /*------------------------------------------------*/

    public async void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public async void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    #endregion

    #region Server
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("Connected master server");
        JoinLobby();
    }

    
    public override void OnJoinedLobby()
    {
        MenuManager.instance.MenuCanvas.SetActive(true);
        MenuManager.instance.NetworkCanvas.SetActive(false);

        Debug.Log("Joined Lobby");
    }

    /*------------------------------------------------*/

    public override void OnJoinRandomFailed(short returnCode, string message)
    {

    }

    #endregion
}
