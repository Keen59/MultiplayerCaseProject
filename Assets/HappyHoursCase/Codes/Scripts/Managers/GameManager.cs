using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    #region Public-Varaibles

    public static GameManager instance;

    [HideInInspector]public PlayerUnit CurrentPlayerUnit;


    #endregion

    #region Life-Cycle

    /*--------------------------------*/

    private void Awake()
    {
        instance = this;
    }

    /*--------------------------------*/

    #endregion
    #region Client
    
    public void SelectCurrentPlayer(PlayerUnit player)
    {

        if (CurrentPlayerUnit != null)
            CurrentPlayerUnit.SelectCharacter();

        CurrentPlayerUnit = player;
        CurrentPlayerUnit.SelectCharacter();
    }

    #endregion

}
