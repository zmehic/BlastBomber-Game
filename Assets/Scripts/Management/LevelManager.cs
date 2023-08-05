using System;
using System.Collections;
using System.Collections.Generic;
using BlastBomberV2.Core;
using BlastBomberV2.Core.IO;
using UnityEngine;
using Random = UnityEngine.Random;



namespace BlastBomberV2.Management
{
    public class LevelManager : MonoBehaviour
    {
        //[SerializeField] private float spacing = 1;
        
        [SerializeField]
        private List<TextAsset> levelImage;

        private int lvl = OptionsManager.lvl;
        private Dictionary<Color,LevelBlock> levelBlockDictionary { get; set; }
        
        private Dictionary<int,Dictionary<Color,LevelBlock>> listOfDictionaries { get; set; }

        

        private Vector2Int size;
        private Level<ILevelBlock> level;
        private List<Vector3> enemyInstantiationPositions;

        public event Action<Level<ILevelBlock>> LevelLoaded;
        public event Action<List<Vector3>> EnemyInstantiationPositionsCollected;

        private void Start()
        {
            listOfDictionaries = new Dictionary<int, Dictionary<Color, LevelBlock>>()
            {
                {0,new Dictionary<Color, LevelBlock>() {{
                        new Color(0, 0, 0), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Destructable }
                    },
                    {
                        new Color(0, 0, 1), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Tree }
                    },
                    {
                        new Color(0, 1, 0), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Bush }
                    },
                    {
                        new Color(0, 1, 1), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Berries }
                    },
                    {
                        new Color(1, 0, 0), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Water }
                    },
                    {
                        new Color(1, 0, 1), new LevelBlock() { IsWalkable = true, BlockType = LevelBlockType.Enemy }
                    },
                    {
                        new Color(1, 1, 0),
                        new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.LevelBoundary1 }
                    },
                    {
                        new Color(1,1,1), new LevelBlock(){IsWalkable = true,BlockType = LevelBlockType.PlayerInstantiationPosition}  
                    },              
                    {
                        Color.clear,
                        new LevelBlock() { IsWalkable = true, BlockType = LevelBlockType.Empty }
                    },
                    
                    {
                        new Color(0.760f, 0.6f, 1),
                        new LevelBlock() { IsWalkable = true, BlockType = LevelBlockType.LevelBoundary1 }
                    }
                }},
                {
                    1,new Dictionary<Color, LevelBlock>(){{
                    new Color(0, 0, 0), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Destructable }
                },
                {
                    new Color(0, 0, 1), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.DeerSkull }
                },
                {
                    new Color(0, 1, 0), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Rock }
                },
                {
                    new Color(0, 1, 1), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Cactus }
                },
                {
                    new Color(1, 0, 0), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Vase }
                },
                {
                    new Color(1, 0, 1), new LevelBlock() { IsWalkable = true, BlockType = LevelBlockType.Enemy }
                },
                {
                    new Color(1, 1, 0),
                    new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.LevelBoundary2 }
                },
                {
                    new Color(1,1,1), new LevelBlock(){IsWalkable = true,BlockType = LevelBlockType.PlayerInstantiationPosition}  
                },
                {
                    Color.clear,
                    new LevelBlock() { IsWalkable = true, BlockType = LevelBlockType.Empty }
                },
                {
                    new Color(0.760f, 0.6f, 1),
                    new LevelBlock() { IsWalkable = true, BlockType = LevelBlockType.LevelBoundary1 }
                }
                }},
                {2,new Dictionary<Color, LevelBlock>() {{
                        new Color(0, 0, 0), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Destructable }
                    },
                    {
                        new Color(0, 0, 1), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Skull }
                    },
                    {
                        new Color(0, 1, 0), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Chest}
                    },
                    {
                        new Color(0, 1, 1), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.StoneMonument }
                    },
                    {
                        new Color(1, 0, 0), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Barrel }
                    },
                    {
                        new Color(1, 0, 1), new LevelBlock() { IsWalkable = true, BlockType = LevelBlockType.Enemy }
                    },
                    {
                        new Color(1, 1, 0),
                        new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.LevelBoundary3 }
                    },
                    {
                        new Color(1,1,1), new LevelBlock(){IsWalkable = true,BlockType = LevelBlockType.PlayerInstantiationPosition}  
                    },
                    {
                        Color.clear,
                        new LevelBlock() { IsWalkable = true, BlockType = LevelBlockType.Empty }
                    }}},
                {
                    3,new Dictionary<Color, LevelBlock>(){{
                    new Color(0, 0, 0), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Destructable }
                },
                {
                    new Color(0, 0, 1), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.WinterTree }
                },
                {
                    new Color(0, 1, 0), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.WinterBush }
                },
                {
                    new Color(0, 1, 1), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.WinterBerries }
                },
                {
                    new Color(1, 0, 0), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.WinterBarrel }
                },
                {
                    new Color(1, 0, 1), new LevelBlock() { IsWalkable = true, BlockType = LevelBlockType.Enemy }
                },
                {
                    new Color(1, 1, 0),
                    new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.LevelBoundary4 }
                },
                {
                    new Color(1,1,1), new LevelBlock(){IsWalkable = true,BlockType = LevelBlockType.PlayerInstantiationPosition}  
                },
                {
                    Color.clear,
                    new LevelBlock() { IsWalkable = true, BlockType = LevelBlockType.Empty }
                }
                }}
            };
            
            
            levelBlockDictionary = new Dictionary<Color, LevelBlock>()
            {
                {
                    new Color(0, 0, 0), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Destructable }
                },
                {
                    new Color(0, 0, 1), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Tree }
                },
                {
                    new Color(0, 1, 0), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Bush }
                },
                {
                    new Color(0, 1, 1), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Berries }
                },
                {
                    new Color(1, 0, 0), new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.Water }
                },
                {
                    new Color(1, 0, 1), new LevelBlock() { IsWalkable = true, BlockType = LevelBlockType.Enemy }
                },
                {
                    new Color(1, 1, 0),
                    new LevelBlock() { IsWalkable = false, BlockType = LevelBlockType.LevelBoundary1 }
                },
                {
                  new Color(1,1,1), new LevelBlock(){IsWalkable = true,BlockType = LevelBlockType.PlayerInstantiationPosition}  
                },
                {
                    Color.clear,
                    new LevelBlock() { IsWalkable = true, BlockType = LevelBlockType.Empty }
                }

            };

            
        }

        public void LoadLevel()
        {
            enemyInstantiationPositions = new List<Vector3>();
            Color[,] loadedLevelRaw=null;
            if (OptionsManager.lvl == 0)
            {
                
                 loadedLevelRaw = LevelLoader.LoadLevel(levelImage[0]);
            }
            else if (OptionsManager.lvl==1)
            {
                loadedLevelRaw = LevelLoader.LoadLevel(levelImage[1]);
            }
            else if (OptionsManager.lvl == 2)
            {
                loadedLevelRaw = LevelLoader.LoadLevel(levelImage[2]);
            }
            else if (OptionsManager.lvl == 3)
            {
                loadedLevelRaw = LevelLoader.LoadLevel(levelImage[3]);
            }

            level = new Level<ILevelBlock>(loadedLevelRaw.GetLength(0), loadedLevelRaw.GetLength(1));
            for (int i = 0; i < level.Width; i++)
            {
                for (int j = 0; j < level.Height; j++)
                {
                    if (listOfDictionaries[lvl].TryGetValue(loadedLevelRaw[i, j], out var block))
                    {
                        level.AddNode(i, j, block);
                        if(block.BlockType==LevelBlockType.Enemy)
                            enemyInstantiationPositions.Add(NodeToWorldPosition(new Vector2Int(i,j)));
                    }
                }
            }

            for (int i = 1; i < level.Width-1; i++)
            {
                for (int j = 1; j < level.Height-1; j++)
                {
                    level.DetectNeighbors(level.Grid[i,j]);
                }
            }
            
            LevelLoaded?.Invoke(level);
            EnemyInstantiationPositionsCollected?.Invoke(enemyInstantiationPositions);
            
        }
        
        public Node<ILevelBlock> GetRandomWalkableNode()
        {
            var randomNode = new Node<ILevelBlock>(-1, -1);
            randomNode.Block = new LevelBlock(){ IsWalkable = false };

            while (!randomNode.Block.IsWalkable)
            {
                randomNode = level.Grid[Random.Range(0, level.Grid.GetLength(0)),
                    Random.Range(0, level.Grid.GetLength(1))];
            }

            return randomNode;
        }
        
        public Vector2Int WorldPositionToNodeIndex(Vector3 position) =>
            level.WorldToNodePosition(position);
        
        public Node<ILevelBlock> WorldPositionToNode(Vector3 position)
        {
            var nodePosition = level.WorldToNodePosition(position);
            return level.Grid[nodePosition.x, nodePosition.y];
        }
        public void MakeNodeNotWalkable(Vector3 position)
        {
            var nodePosition = level.WorldToNodePosition(position);
            level.Grid[nodePosition.x, nodePosition.y].Block.IsWalkable = false;
        }
        public void MakeNodeWalkable(Vector3 position)
        {
            var nodePosition = level.WorldToNodePosition(position);
            level.Grid[nodePosition.x, nodePosition.y].Block.IsWalkable = true;
        }
        
        public Vector3 NodeToWorldPosition(Vector2Int nodePosition) =>
            level.NodeToWorldPosition(nodePosition.x, nodePosition.y);
        
        public Vector3 NodeToWorldPosition(Node<ILevelBlock> nodePosition) =>
            level.NodeToWorldPosition(nodePosition.X, nodePosition.Y);
    }
}
