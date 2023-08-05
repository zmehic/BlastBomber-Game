using System;
using System.Collections;
using System.Collections.Generic;
using BlastBomberV2.Core;
using BlastBomberV2.Core.IO;
using BlastBomberV2.Management;

using UnityEngine;
using Random = UnityEngine.Random;


namespace BlastBomberV2.Behaviours.Controllers
{
    public class LevelGenerator : MonoBehaviour
    {
        
        
        [SerializeField] private float spacing = 1;

        [SerializeField] private GameObject boundaryBlockPrefab;

        [SerializeField] private List<GameObject> levelBlocks;
        [SerializeField] private List<GameObject> Ground;
        int lvl = OptionsManager.lvl;
        
  
        private Dictionary<LevelBlockType,GameObject> levelBlockDictionary { get; set; }

        private void Awake()
        {
            
            //GenerateLevelBoundaries();
            //GenerateLevel();
            levelBlockDictionary = new Dictionary<LevelBlockType, GameObject>()
            {
                { LevelBlockType.LevelBoundary1, levelBlocks[0] },
                { LevelBlockType.Destructable, levelBlocks[3] },
                { LevelBlockType.Berries, levelBlocks[1] },
                { LevelBlockType.Bush, levelBlocks[2] },
                { LevelBlockType.Ladder, levelBlocks[4] },
                { LevelBlockType.Tree, levelBlocks[5] },
                { LevelBlockType.Water, levelBlocks[6] },
                { LevelBlockType.PlayerInstantiationPosition, levelBlocks[7] },
                { LevelBlockType.Cactus ,levelBlocks[8]},
                { LevelBlockType.DeerSkull ,levelBlocks[9]},
                { LevelBlockType.Jar,levelBlocks[10]},
                { LevelBlockType.Rock,levelBlocks[11]},
                { LevelBlockType.Vase,levelBlocks[12]},
                { LevelBlockType.LevelBoundary2 ,levelBlocks[13]},
                { LevelBlockType.Barrel ,levelBlocks[14]},
                { LevelBlockType.Chest ,levelBlocks[15]},
                {LevelBlockType.Fire,levelBlocks[16]},
                { LevelBlockType.StoneMonument,levelBlocks[17]},
                {LevelBlockType.Skull,levelBlocks[18]},
                {LevelBlockType.LevelBoundary3,levelBlocks[19]},
                { LevelBlockType.WinterBarrel ,levelBlocks[20]},
                { LevelBlockType.WinterBerries ,levelBlocks[21]},
                { LevelBlockType.WinterBush ,levelBlocks[22]},
                { LevelBlockType.WinterFire ,levelBlocks[23]},
                { LevelBlockType.WinterTree ,levelBlocks[24]},
                { LevelBlockType.LevelBoundary4 ,levelBlocks[25]}


            };
            

            var levelManager = this.GetComponent<LevelManager>();
            levelManager.LevelLoaded += LevelManager_OnLevelLoaded;
            //var output = LevelLoader.LoadLevel(levelImage);
            //GenerateLevelFromColorArray(output);
        }

        private void LevelManager_OnLevelLoaded(Level<ILevelBlock> level)
        {
            
            var levelBlocksParent = new GameObject("LevelBlocksParent");
            for (int i = 0; i < level.Width; i++)
            {
                for (int j = 0; j < level.Height; j++)
                {
                    var blockType = ((LevelBlock)level.Grid[i, j].Block).BlockType;

                    if(levelBlockDictionary.TryGetValue(blockType,out var block))
                    {
                        if (blockType == LevelBlockType.PlayerInstantiationPosition)
                        {
                            var player = Instantiate(block, new Vector3(i * spacing, -0.5f, j * spacing),
                                Quaternion.identity);
                            if (OptionsManager.character == 0)
                            {
                                player.GetComponent<PlayerController>().Setup(FindObjectOfType<InputManager>(),FindObjectOfType<HealthBarScript>(),FindObjectOfType<ScoreScript>(),FindObjectOfType<TimerScript>());
                                
                            }
                            else if(OptionsManager.character==1)
                            {
                                
                                player.GetComponent<PlayerController1>().Setup(FindObjectOfType<InputManager>(),FindObjectOfType<HealthBarScript>(),FindObjectOfType<ScoreScript>(),FindObjectOfType<TimerScript>());
                            }

                        }
                        else if(blockType==LevelBlockType.Water)
                        {
                            Instantiate(block, new Vector3(i * spacing, -0.5f, j * spacing), Quaternion.AngleAxis(90,new Vector3(1,0,0)));
                        }
                        else if (blockType == LevelBlockType.Bush)
                        {
                            Instantiate(block,new Vector3(i*spacing,-0.5f,j*spacing),Quaternion.identity);
                        }
                        else if (blockType == LevelBlockType.Berries)
                        {
                            Instantiate(block, new Vector3(i * spacing, -0.5f, j * spacing), Quaternion.identity);
                        }
                        else if (blockType == LevelBlockType.Enemy)
                        {
                            Instantiate(block, new Vector3(i * spacing, -1, j * spacing),Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(block, new Vector3(i * spacing, 0, j * spacing), Quaternion.identity,
                                levelBlocksParent.transform);
                            
                        }
                        
                    }

                    if (lvl == 0)
                    {
                        Instantiate(Ground[lvl], new Vector3(i * spacing, -1, j * spacing), Quaternion.identity);
                        
                    }
                    else if (lvl == 1)
                    {
                        Instantiate(Ground[lvl], new Vector3(i * spacing, -1, j * spacing), Quaternion.identity);
                    }
                    else if (lvl == 2)
                    {
                        Instantiate(Ground[lvl], new Vector3(i * spacing, -1, j * spacing), Quaternion.identity);

                    }
                    
                    else if (lvl == 3)
                    {
                        Instantiate(Ground[lvl], new Vector3(i * spacing, -1, j * spacing), Quaternion.identity);

                    }
                    
                }
            }
        }

       //private void GenerateLevelFromColorArray(Color[,] colorMap)
       //{
       //    var blockParent = new GameObject("LevelBlock");
       //    for (int i = 0; i < (size.x-2)*(size.y-2); i++)
       //    {
       //        int x = i % (size.y-2);
       //        int y = i / (size.y-2);

       //        
       //        if(levelBlockDictionary.TryGetValue(colorMap[x,y],out var block))
       //        {Instantiate(block,
       //         new Vector3(x * spacing - ((size.x-2) / 2), 0.5f, y * spacing - ((size.y-2) / 2)), Quaternion.identity,blockParent.transform);}
       //        //if (Random.value>0.5f)
       //        //{
       //        //    Instantiate(levelBlock,
       //        //        new Vector3(x * spacing - ((size.x-2) / 2), 0.5f, y * spacing - ((size.y-2) / 2)), Quaternion.identity,blockParent.transform);
       //        //}
       //    }
       //}
        
       // private void GenerateLevel()
       // {
       //     var blockParent = new GameObject("LevelBlock");
       //     for (int i = 0; i < (size.x-2)*(size.y-2); i++)
       //     {
       //         int x = i % (size.y-2);
       //         int y = i / (size.y-2);
//
       //         if (Random.value>0.5f)
       //         {
       //             Instantiate(levelBlock,
       //                 new Vector3(x * spacing - ((size.x-2) / 2), 0.5f, y * spacing - ((size.y-2) / 2)), Quaternion.identity,blockParent.transform);
       //         }
       //     }
       // }

       // private void GenerateLevelBoundaries()
       // {
       //     var boundaryParent = new GameObject("LevelBoundaryParent");
       //     for (int i = 0; i < size.x*size.y; i++)
       //     {
       //         int x = i % size.y;
       //         int y = i / size.y;
//
       //         if (x == 0 || x == size.y - 1 || y == 0 || y == size.y - 1)
       //         {
       //             Instantiate(boundaryBlockPrefab,
       //                 new Vector3(x * spacing - (size.x / 2), 0.5f, y * spacing - (size.y / 2)), Quaternion.identity,boundaryParent.transform);
       //         }
       //     }
       // }
    }
}
