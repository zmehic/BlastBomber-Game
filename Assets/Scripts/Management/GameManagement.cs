using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastBomberV2.Management
{
    public class GameManagement : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;

        private EnemiesManager enemiesManager;
        private List<Vector3> enemyInstatiationPositions;

        private void Start()
        {
            enemiesManager = this.GetComponent<EnemiesManager>();
            levelManager.EnemyInstantiationPositionsCollected += LevelManager_OnEnemyInstantiationPositionCollected;
            levelManager.LoadLevel();
        }

        private void LevelManager_OnEnemyInstantiationPositionCollected(List<Vector3> enemyInstantiationPositions)
        {
            this.enemyInstatiationPositions = enemyInstantiationPositions;
            enemiesManager.Initialize(this.enemyInstatiationPositions);
        }
    }
}
