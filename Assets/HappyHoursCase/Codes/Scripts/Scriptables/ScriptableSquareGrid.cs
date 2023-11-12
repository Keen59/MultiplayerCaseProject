using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "New Scriptable Square Grid")]
    public class ScriptableSquareGrid : ScriptableGrid
    {
        [SerializeField, Range(3, 50)] private int _gridWidth = 16;
        [SerializeField, Range(3, 50)] private int _gridHeight = 9;

        public override Dictionary<Vector2, NodeBase> GenerateGrid()
        {
            var tiles = new Dictionary<Vector2, NodeBase>();
            var grid = new GameObject
            {
                name = "Grid"
            };
            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    var tile = Instantiate(nodeBasePrefab,new Vector3(x,0,y),Quaternion.Euler(90,0,0));

                    tile.transform.SetParent(grid.transform);
                    tile.Init(DecideIfObstacle(), new SquareCoords { Pos = new Vector3(x, y) });
                    tiles.Add(new Vector2(x, y), tile);
                }
            }

            return tiles;
        }

        public override Dictionary<Vector2, NodeBase> SerializeGrid(List<NodeBase> Nodebases)
        {
            var tiles = new Dictionary<Vector2, NodeBase>();

            for (int x = 0; x < Nodebases.Count; x++)
            {

                var tile = Nodebases[x];
                Vector2 Pos =new Vector2(tile.transform.position.x, tile.transform.position.z);

                tile.Init(tile.Walkable, new SquareCoords { Pos = Pos });

                tiles.Add(new Vector2((int)tile.transform.position.x, (int)tile.transform.position.z), tile);


            }
            return tiles;

        }
    }
