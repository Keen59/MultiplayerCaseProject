using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private Sprite _playerSprite, _goalSprite;
    [SerializeField] private ScriptableGrid _scriptableGrid;
    [SerializeField] private bool _drawConnections;
    [SerializeField] private List<NodeBase> NodeBases;
    [SerializeField] private List<NodeBase> WoodNodeBases;
    public TMP_Text WoodText;

    public Dictionary<Vector2, NodeBase> Tiles { get; private set; }



    async void Awake()
    {

        Instance = this;

        Tiles = _scriptableGrid.SerializeGrid(NodeBases);
        //Tiles = _scriptableGrid.GenerateGrid();

        foreach (var tile in Tiles.Values) tile.CacheNeighbors();

        NodeBase.OnHoverTile += OnTileHover;

        List<NodeBase> WalkableNodes = new List<NodeBase>();

        await InitializePlayers();

        if (PhotonNetwork.IsMasterClient)
            await InitializeWoodPlanks();

    }


    private void OnDestroy() => NodeBase.OnHoverTile -= OnTileHover;

    private void OnTileHover(Transform Player, NodeBase PlayerNode, NodeBase nodeBase)
    {

        for (var i = 0; i < Tiles.Values.Count; i++)
        {
            Tiles.Values.ElementAt(i).RevertTile();
        }

        var path = Pathfinding.FindPath(PlayerNode, nodeBase);

        List<Transform> pathTransforms = new List<Transform>();

        for (int i = 0; i < path.Count; i++)
        {
            pathTransforms.Add(path[i].transform);
            path[i]._fCostText.text = i.ToString();
        }
        Player.GetComponent<PlayerUnit>().InitializePath(pathTransforms);
    }

    public async UniTask<NodeBase> GetWalkableNodeBase()
    {
        List<NodeBase> Walkables = await WalkableNodes();

        NodeBase nodebase = Walkables[Random.Range(0, Walkables.Count)];

        return nodebase;
    }

    public async UniTask<List<NodeBase>> WalkableNodes()
    {
        List<NodeBase> Walkables = new List<NodeBase>();
        for (int i = 0; i < Tiles.Count; i++)
        {
            if (Tiles.ElementAt(i).Value.Walkable)
            {
                Walkables.Add(Tiles.ElementAt(i).Value);
            }
        }

        return Walkables;
    }
    public NodeBase GetTileAtPosition(Vector2 pos) => Tiles.TryGetValue(pos, out var tile) ? tile : null;

    private async UniTask InitializePlayers()
    {
        List<NodeBase> nodes = await WalkableNodes();
        for (int i = 0; i < 3; i++)
        {
            NodeBase node = nodes[Random.Range(0, nodes.Count)];
            nodes.Remove(node);

            GameObject player = PhotonNetwork.Instantiate(PhotonNetwork.LocalPlayer.CustomProperties["Character"].ToString(), new Vector3(node.transform.position.x, 1, node.transform.position.z), Quaternion.identity, 0);

            await player.GetComponent<PlayerUnit>().Initialize(WoodText, node);
        }
    }

    private async UniTask InitializeWoodPlanks()
    {
        List<NodeBase> nodes = await WalkableNodes();
        int PlankNodeCount = nodes.Count / 6;

        for (int i = 0; i < PlankNodeCount; i++)
        {
            NodeBase node = nodes[Random.Range(0, nodes.Count)];
            PhotonNetwork.Instantiate("Wood", node.transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || !_drawConnections) return;
        Gizmos.color = Color.red;
        foreach (var tile in Tiles)
        {
            if (tile.Value.Connection == null) continue;
            Gizmos.DrawLine((Vector3)tile.Key + new Vector3(0, 0, -1), (Vector3)tile.Value.Connection.Coords.Pos + new Vector3(0, 0, -1));
        }
    }
}
