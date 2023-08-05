
using System.Collections.Generic;
using BlastBomberV2.ScriptableObjects;
using UnityEngine;
using Random=UnityEngine.Random;


namespace BlastBomberV2.Management
{
    public class EnemiesManager : MonoBehaviour
    {
        [Tooltip("Spawn an object after x seconds")]
        [SerializeField]
        private float spawningInterval;

        [SerializeField]
        private float waitBeforeEnemySpawning;

        [SerializeField]
        private int maxNumberOfEnemies;

        private List<Vector3> instantiationPositions;
        
        [SerializeField]
        private List<EnemyData> enemyData;

        private List<GameObject> instantiatedEnemies;

        //Super crazy hack
        public void EnemyKilled(int index)
        {
            instantiatedEnemies.RemoveAt(index);
            for (int i = 0; i < instantiatedEnemies.Count; i++)
            {
                instantiatedEnemies[i].name = i.ToString();
            }
        }

        public void Initialize(List<Vector3> instantiationPos)
        {
            instantiationPositions = instantiationPos;
            instantiatedEnemies = new List<GameObject>();
            
            this.InvokeRepeating("SpawnEnemies", waitBeforeEnemySpawning, spawningInterval);
        }
        
        private void SpawnEnemies()
        {
            if (instantiatedEnemies.Count < maxNumberOfEnemies)
            {
                if (OptionsManager.diff == 0)
                {
                    enemyData[0].speed = 3;
                    maxNumberOfEnemies = 3;
                }
                else if (OptionsManager.diff == 1)
                {
                    enemyData[0].speed = 5;
                    maxNumberOfEnemies = 4;
                }
                else
                {
                    enemyData[0].speed = 7;
                    maxNumberOfEnemies = 5;
                }
                
                var enemy = Instantiate(enemyData[Random.Range(0, enemyData.Count)].enemyPrefab,
                    instantiationPositions[Random.Range(0, instantiationPositions.Count)], Quaternion.identity);
                instantiatedEnemies.Add(enemy);
                enemy.name = (instantiatedEnemies.Count - 1).ToString();
                
            }
        }
    }
}
