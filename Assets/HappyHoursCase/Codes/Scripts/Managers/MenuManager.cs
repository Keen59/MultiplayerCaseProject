using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    #region Public-Varaibles

    public static MenuManager instance;

    public GameObject MenuCanvas;
    public GameObject NetworkCanvas;

    [SerializeField] private GameObject SearchMatchButton;

    #endregion

    #region Private-Varaibles

    #endregion

    #region LifeCycle

    private void Awake()
    {
        instance = this;
    }

    /*------------------------------------------------*/

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    #endregion

    #region Client


    #endregion

    #region Server


    #endregion
}
