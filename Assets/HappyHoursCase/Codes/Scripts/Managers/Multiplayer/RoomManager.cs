using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;

public class RoomManager : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    #region Public-Varaibles

    public static RoomManager instance;

    #endregion

    #region Private-Varaibles

    public List<string> Characters;

    #endregion

    #region LifeCycle

    private void Awake()
    {
        instance = this;
    }

    /*------------------------------------------------*/

    private void Start()
    {

    }

    #endregion

    #region Server
    public async void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom("", roomOptions, TypedLobby.Default);

    }

    /*------------------------------------------------*/

    public override void OnJoinedRoom()
    {
        MenuManager.instance.MenuCanvas.SetActive(false);
        Dictionary<int, Player> CurrentPlayers = PhotonNetwork.CurrentRoom.Players;

        for (int i = 0; i < CurrentPlayers.Keys.Count; i++)
        {
            if (CurrentPlayers.ElementAt(i).Value.CustomProperties.ContainsKey("Character") && CurrentPlayers.ElementAt(i).Value != PhotonNetwork.LocalPlayer)
            {
                Characters.Remove(CurrentPlayers.ElementAt(i).Value.CustomProperties["Character"].ToString());
            }
        }

        //GameObject player = PhotonNetwork.Instantiate(Characters[Characters.Count - 1], new Vector3(CurrentPlayers.Count, 0, 0), Quaternion.identity, 0);

        Hashtable playerStats = PhotonNetwork.LocalPlayer.CustomProperties;
        playerStats["Character"] = Characters[Characters.Count - 1];
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerStats);

    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created the room");
        Hashtable playerStats = new Hashtable
        {
            { "Character", Characters[Characters.Count-1].ToString() }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerStats);

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Someone joined " + newPlayer.CustomProperties["Character"]);

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2 && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
    }

    private void WaitForLevelLoad()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }



    #endregion
    #region RPCs


    #endregion
}
