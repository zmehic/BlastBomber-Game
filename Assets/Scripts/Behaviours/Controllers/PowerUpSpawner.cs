using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastBomberV2
{
    public class PowerUpSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] powerUpPrefabs;

        public void BlockDestroyed(Vector3 pos)
        {
            if (Random.Range(0,100) < 25)
            {
                Instantiate(powerUpPrefabs[0], pos, Quaternion.identity);
            }
            else if(Random.Range(0,100)>25 && Random.Range(0,100)<50)
            {
                Instantiate(powerUpPrefabs[1], pos, Quaternion.identity);
            }
        }
    }
}
