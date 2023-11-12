using Cysharp.Threading.Tasks;
using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerUnit : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    #region Public-Varaibles

    public NodeBase CurrentNodeBase;

    public bool IsSelected;

    public GameObject Outline;

    public List<Transform> CurrentPath;

    public int CurrentPathIndex;

    public TMP_Text WoodText;

    #endregion

    #region Private-Varaibles

    private PhotonView photonView;

    [SerializeField] private int WoodCount;
    #endregion

    #region Life-Cycle

    /*--------------------------------*/

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    /*--------------------------------*/

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClickTile();
        }

    }

    /*--------------------------------*/

    private void OnMouseDown()
    {
        GameManager.instance.SelectCurrentPlayer(this);
    }

    /*--------------------------------*/

    #endregion

    #region Photon

    /*--------------------------------*/

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = gameObject;
        DontDestroyOnLoad(this.gameObject);
    }

    /*--------------------------------*/

    #endregion

    #region Client

    /*--------------------------------*/

    private void OnClickTile()
    {
        if (IsSelected)
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                if (hitInfo.transform.tag == "Node" && photonView.IsMine)
                {
                    NodeBase tile = hitInfo.transform.gameObject.GetComponent<NodeBase>();
                    tile.OnTileClick(this.GetComponent<Transform>(), CurrentNodeBase);
                }
            }
        }
    }

    /*--------------------------------*/

    public async UniTask Initialize(TMP_Text woodText,NodeBase currentNodeBase)
    {
        if (photonView.IsMine)
        {
            WoodText = woodText;
            photonView.RPC("SetCurrentNodeBase", RpcTarget.AllBuffered, currentNodeBase.GetComponent<PhotonView>().ViewID);
        }
    }

    /*--------------------------------*/

    public void AddWood()
    {
        WoodCount++;
        WoodText.text="Wood Count :" + WoodCount.ToString();

    }

    /*--------------------------------*/

    public void InitializePath(List<Transform> path)
    {
        CurrentPath = null;
        CurrentPathIndex = path.Count - 1;
        CurrentPath = path;
    }

    /*--------------------------------*/

    public void SelectCharacter()
    {
        if (photonView.IsMine)
        {
            if (!IsSelected)
            {
                IsSelected = true;
            }
            else
            {
                IsSelected = false;
            }

            Outline.SetActive(IsSelected);
        }
    }

    /*--------------------------------*/

    #endregion

    #region RPC

    [PunRPC]
    public async void SetCurrentNodeBase(int NodeViewID)
    {
        if (CurrentNodeBase != null)
            CurrentNodeBase.Walkable = true;

        NodeBase node = PhotonView.Find(NodeViewID).GetComponent<NodeBase>();
        CurrentNodeBase = node;
        CurrentNodeBase.SetWalkable(false);
    }

    #endregion

}

