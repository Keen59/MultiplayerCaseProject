using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    #region Private-Varaibles

    private PhotonView photonView;

    private PlayerUnit unit;

    #endregion
    #region Life-Cycle

    /*--------------------------------*/

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        unit = GetComponent<PlayerUnit>();

        /*   if (SceneManager.GetActiveScene().buildIndex == 1 && PhotonNetwork.IsMasterClient)
           {
               photonView.RPC("SetStartPosition", RpcTarget.AllBuffered);
           }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wood" && photonView.IsMine)
        {
            unit.AddWood();
            photonView.RPC("CollectWood",RpcTarget.MasterClient, other.GetComponent<PhotonView>().ViewID);
        }
    }

    /*--------------------------------*/

    #endregion

    #region RPC

    [PunRPC]
    public async void CollectWood(int ViewID)
    {
        PhotonNetwork.Destroy(PhotonView.Find(ViewID).gameObject);

    }

    #endregion


}
