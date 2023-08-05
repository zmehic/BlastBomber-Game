
using UnityEngine;

namespace BlastBomberV2.ScriptableObjects
{
    [CreateAssetMenu(menuName = "BattleCity/Enemy/EnemyData", fileName = "EnemyData")]
    public class EnemyData:ScriptableObject
    {
        public float speed;
        public float attackRange;
        public GameObject enemyPrefab;
    }
}
