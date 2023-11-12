using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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
    }

    /*--------------------------------*/

    private void Update()
    {
        HandleMovement();
    }

    /*--------------------------------*/

    #endregion

    #region Movement

    /*--------------------------------*/

    private void HandleMovement()
    {
        if (unit.CurrentPath.Count>0)
        {

            transform.position = Vector3.Lerp(transform.position, Vector3.up + unit.CurrentPath[unit.CurrentPathIndex].transform.position, Time.deltaTime * 5);
            if (unit.CurrentPathIndex > 0 && Vector3.Distance(transform.position, unit.CurrentPath[unit.CurrentPathIndex].transform.position) < 1.25f)
            {
                CheckPathIndex();
            }
        }
    }

    /*--------------------------------*/

    private async void CheckPathIndex()
    {
        unit.CurrentPathIndex--;
        if (photonView.IsMine)
        {
            await unit.Initialize(unit.WoodText, unit.CurrentPath[unit.CurrentPathIndex].GetComponent<NodeBase>());
        }
    }

    /*--------------------------------*/

    #endregion


}
