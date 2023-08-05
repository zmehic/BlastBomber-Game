using System;
using System.Collections;
using System.Collections.Generic;
using BlastBomberV2.Behaviours.Controllers;
using BlastBomberV2.Management;
using UnityEngine;
using Random = UnityEngine.Random;


namespace BlastBomberV2
{
    [RequireComponent(typeof(Collider))]
    public class PowerUp : MonoBehaviour
    {
        enum PowerUps{Speed, Teleportation};
        [SerializeField]
        private GameObject Player;

        private LevelManager lvlMng;

        [SerializeField] private PowerUps PowerUpType;

        [SerializeField] private float speedIncreaseAmount = 50;
        [SerializeField] private float powerUpDuration = 5;
        [SerializeField] private GameObject artToDisable = null;

        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            lvlMng = FindObjectOfType<LevelManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            
            if (other.gameObject.tag == "Player")
            {
                if (PowerUpType == PowerUps.Speed)
                {
                    PlayerController1 pc1 = other.gameObject.GetComponent<PlayerController1>();
                    PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                    if (playerController != null || pc1!=null)
                    {
                        if(OptionsManager.character==0)
                        StartCoroutine(SpeedUp(playerController));
                        else
                        {
                            StartCoroutine(SpeedUp(pc1));
                        }
                    }
                }
                else if (PowerUpType==PowerUps.Teleportation)
                {
                    StartCoroutine(TeleportPowerUp(other));
                }
            }
        }

        public IEnumerator TeleportPowerUp(Collider other)
        {
      
            _collider.enabled = false;
            artToDisable.SetActive(false);
            var obj = lvlMng.GetRandomWalkableNode();
            other.gameObject.transform.position = new Vector3(obj.X, 0, obj.Y);
            yield return new WaitForSeconds(powerUpDuration);
            Destroy(gameObject);
        }
        public void ActivateSpeedPowerUp(PlayerController playerController)
        {
            playerController.SetSpeed(speedIncreaseAmount);
        }
        public void ActivateSpeedPowerUp(PlayerController1 playerController)
        {
            playerController.SetSpeed(speedIncreaseAmount);
        }

        public void DeactivateSpeedPowerUp(PlayerController playerController)
        {
            playerController.SetSpeed(-speedIncreaseAmount);

        }
        public void DeactivateSpeedPowerUp(PlayerController1 playerController)
        {
            playerController.SetSpeed(-speedIncreaseAmount);

        }
        public IEnumerator SpeedUp(PlayerController playerController)
        {
            _collider.enabled = false;
            artToDisable.SetActive(false);
            ActivateSpeedPowerUp(playerController);
            yield return new WaitForSeconds(powerUpDuration);
            DeactivateSpeedPowerUp(playerController);
            Destroy((gameObject));
        }
        public IEnumerator SpeedUp(PlayerController1 playerController)
        {
            _collider.enabled = false;
            artToDisable.SetActive(false);
            ActivateSpeedPowerUp(playerController);
            yield return new WaitForSeconds(powerUpDuration);
            DeactivateSpeedPowerUp(playerController);
            Destroy((gameObject));
        }
    }
}
