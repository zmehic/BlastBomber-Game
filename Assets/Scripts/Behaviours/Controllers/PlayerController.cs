using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BlastBomberV2.Core;
using BlastBomberV2.Management;
using BlastBomberV2.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

namespace BlastBomberV2.Behaviours.Controllers
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private float _speed;
        
        private HealthBarScript hpscr;
        private ScoreScript scrscrp;
        public int hp = 100;
        public int currentHealth;
   

        private Rigidbody playerRigidBody;

        [SerializeField] private GameObject bomba;
        [SerializeField] private GameObject eksplozija;

        private Animator _animator;
        public float bombFuseTime = 3f;
        public int bombAmount = 1;
        private int bombsRemaining;
        private InputManager inmng;
        private TimerScript timer;

        private int X;
        private int Z;

        public void SetSpeed(float speed)
        {
            _speed += speed;
        }
        public void Setup(InputManager _inputManager, HealthBarScript _healthBar, ScoreScript scr,TimerScript tscr)
        {
            
            inmng = _inputManager;
            hpscr = _healthBar;
            scrscrp = scr;
            timer = tscr;
            bombsRemaining = bombAmount;
            if (OptionsManager.diff == 1)
            {
                _speed = _speed * 1.5f;
                hp = 150;
                currentHealth = hp;
                _healthBar.SetMaxHealth(hp);

            }
            else if (OptionsManager.diff == 0)
            {
                _speed = _speed * 1.8f;
                hp = 200;
                currentHealth = hp;
                _healthBar.SetMaxHealth(hp);
            }
            else
            {
                _speed = _speed * 1.2f;
                _healthBar.SetMaxHealth(hp);
                currentHealth = hp;
            }

            playerRigidBody = this.GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _inputManager.MovementInputReceived += InputManager_OnMovementInputReceived;
            _inputManager.Fire += InputManager_OnFire;
            _inputManager.Exit += InputManager_OnExit;


        }

        private void Update()
        {
            
            StartCoroutine(CheckLife());
        }

        private IEnumerator CheckLife()
        {
            if (currentHealth <= 0)
            {
                _animator.SetBool("isDead",true);
                _animator.SetBool("isWalking", false);
                inmng.enabled = false;
                yield return new WaitForSeconds(3);
                OptionsManager.lvl = 0;
                OptionsManager.diff = 0;
                OptionsManager.character = 0;
                OptionsManager.mode = 0;
                
                SceneManager.LoadScene("MainMenu");
                
            }
            else if (timer.timerValue == 0)
            {
                OptionsManager.lvl = 0;
                OptionsManager.diff = 0;
                OptionsManager.character = 0;
                OptionsManager.mode = 0;
                SceneManager.LoadScene("MainMenu");
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            EnemyController other = collision.gameObject.GetComponent<EnemyController>();
            if (other)
            {
                currentHealth = currentHealth - 50;
                hpscr.SetHealth(currentHealth);
            }
        }

        private void InputManager_OnExit()
        {
            OptionsManager.lvl = 0;
            OptionsManager.diff = 0;
            OptionsManager.character = 0;
            OptionsManager.mode = 0;
            SceneManager.LoadScene("MainMenu");
        }
        private void InputManager_OnMovementInputReceived(float horizontal, float vertical)
        {
            if (horizontal != 0)
            {
                playerRigidBody.AddForce(new Vector3(horizontal, 0, 0) * _speed * Time.fixedDeltaTime,
                    ForceMode.VelocityChange);
                playerRigidBody.MoveRotation(Quaternion.Euler(Vector3.up * 90 * horizontal));
                HandleAnimation();
            }
            else if (vertical != 0)
            {
                playerRigidBody.AddForce(new Vector3(0, 0, vertical) * _speed * Time.fixedDeltaTime,
                    ForceMode.VelocityChange);
                playerRigidBody.MoveRotation(vertical > 0
                    ? Quaternion.Euler(Vector3.zero)
                    : Quaternion.Euler(Vector3.up * 180 * vertical));
                HandleAnimation();

            }
            else if (vertical == 0 && horizontal == 0)
            {
                _animator.SetBool("isWalking", false);
            }
        }

        private void HandleAnimation()
        {
            bool isRunning = _animator.GetBool("isWalking");
            if (isRunning != true)
            {
                _animator.SetBool("isWalking", true);
            }
        }

        private void InputManager_OnFire()
        {
            if (bombsRemaining > 0)
            {
                StartCoroutine(PlaceBomb());
            }

        }

        private IEnumerator PlaceBomb()
        {
            var x = Mathf.RoundToInt(playerRigidBody.position.x);
            var z = Mathf.RoundToInt(playerRigidBody.position.z);
            GameObject bomb = Instantiate(bomba, new Vector3(x, 0, z),
                Quaternion.identity);

            //lvlMng.MakeNodeNotWalkable(new Vector3(x,0,z));
            bombsRemaining--;

            yield return new WaitForSeconds(bombFuseTime);
            Explosion(x, z);
            //lvlMng.MakeNodeWalkable(new Vector3(x,0,z));
            DestroyImmediate(bomb, true);
            bombsRemaining++;
        }

        private void Explosion(int x, int z)
        {
            X = x;
            Z = z;
            GameObject explosion = Instantiate(eksplozija, new Vector3(x, 0, z), Quaternion.identity);
            GameObject explosion2 = Instantiate(eksplozija, new Vector3(x + 1, 0, z), Quaternion.identity);
            GameObject explosion3 = Instantiate(eksplozija, new Vector3(x - 1, 0, z), Quaternion.identity);
            GameObject explosion4 = Instantiate(eksplozija, new Vector3(x, 0, z + 1), Quaternion.identity);
            GameObject explosion5 = Instantiate(eksplozija, new Vector3(x, 0, z - 1), Quaternion.identity);
            PronadjiElement();

        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(new Vector3(X,0,Z+1),0.3f);
        }

        public void SpeedUpThePlayer()
        {
            
        }

        private void PronadjiElement()
        {
            var collidedGameObjects1 =
                Physics.OverlapSphere(new Vector3(X, 0, Z + 1), 0.3f)
                    .Except(new[] { GetComponent<Collider>() })
                    .Select(c => c.gameObject)
                    .ToArray();
         
            var collidedGameObjects2 =
                Physics.OverlapSphere(new Vector3(X, 0.1f, Z - 1), 0.3f)
                    .Except(new[] { GetComponent<Collider>() })
                    .Select(c => c.gameObject)
                    .ToArray();
            var collidedGameObjects3 =
                Physics.OverlapSphere(new Vector3(X + 1, 0.1f, Z), 0.3f)
                    .Except(new[] { GetComponent<Collider>() })
                    .Select(c => c.gameObject)
                    .ToArray();
            var collidedGameObjects4 =
                Physics.OverlapSphere(new Vector3(X - 1, 0.1f, Z), 0.3f)
                    .Except(new[] { GetComponent<Collider>() })
                    .Select(c => c.gameObject)
                    .ToArray();

            if (collidedGameObjects1.Length > 0)
            {
                for (int i = 0; i < collidedGameObjects1.Length; i++)
                {
                    if (collidedGameObjects1[i]?.GetComponent<Transform>().name == "Destructable(Clone)")
                    {
                        //lvlMng.MakeNodeWalkable(new Vector3(collidedGameObjects1[i].transform.position.x,collidedGameObjects1[i].transform.position.y,collidedGameObjects1[i].transform.position.z));
                        Destroy(collidedGameObjects1[i]);
                        FindObjectOfType<PowerUpSpawner>().BlockDestroyed(collidedGameObjects1[i].transform.position);
                    }
                    else if (collidedGameObjects1[i]?.GetComponent<Transform>().name == "0" ||
                             collidedGameObjects1[i]?.GetComponent<Transform>().name == "1" ||
                             collidedGameObjects1[i]?.GetComponent<Transform>().name == "2")
                    {
                        Destroy(collidedGameObjects1[i]);
                        scrscrp.DodajScore(1);
                    }
                    
                    //PlayerCat(Clone)
                    //else if (collidedGameObjects1[i]?.GetComponent<Transform>().name == "PlayerCat(Clone)")
                    //{
                    //    currentHealth = hp - 100;
                    //    this.GetComponent<HealthBarScript>().SetHealth(currentHealth);
                    //}
                }

            }

            if (collidedGameObjects2.Length > 0)
            {
                for (int i = 0; i < collidedGameObjects2.Length; i++)
                {
                    if (collidedGameObjects2[i]?.GetComponent<Transform>().name == "Destructable(Clone)")
                    {
                        //lvlMng.MakeNodeWalkable(new Vector3(collidedGameObjects2[i].transform.position.x,collidedGameObjects2[i].transform.position.y,collidedGameObjects2[i].transform.position.z));

                        Destroy(collidedGameObjects2[i]);
                        FindObjectOfType<PowerUpSpawner>().BlockDestroyed(collidedGameObjects2[i].transform.position);
                    }
                    else if (collidedGameObjects2[i]?.GetComponent<Transform>().name == "0" ||
                             collidedGameObjects2[i]?.GetComponent<Transform>().name == "1" ||
                             collidedGameObjects2[i]?.GetComponent<Transform>().name == "2")
                    {
                        Destroy(collidedGameObjects2[i]);
                        scrscrp.DodajScore(1);
                    }
                    //PlayerCat(Clone)
                    //else if (collidedGameObjects2[i]?.GetComponent<Transform>().name == "PlayerCat(Clone)")
                    //{
                    //    currentHealth = hp - 100;
                    //    this.GetComponent<HealthBarScript>().SetHealth(currentHealth);
                    //}
                }
            }

            if (collidedGameObjects3.Length > 0)
            {
                for (int i = 0; i < collidedGameObjects3.Length; i++)
                {
                    if (collidedGameObjects3[i]?.GetComponent<Transform>().name == "Destructable(Clone)")
                    {
                        //lvlMng.MakeNodeWalkable(new Vector3(collidedGameObjects3[i].transform.position.x,collidedGameObjects3[i].transform.position.y,collidedGameObjects3[i].transform.position.z));

                        Destroy(collidedGameObjects3[i]);
                        FindObjectOfType<PowerUpSpawner>().BlockDestroyed(collidedGameObjects3[i].transform.position);
                    }
                    else if (collidedGameObjects3[i]?.GetComponent<Transform>().name == "0" ||
                             collidedGameObjects3[i]?.GetComponent<Transform>().name == "1" ||
                             collidedGameObjects3[i]?.GetComponent<Transform>().name == "2")
                    {
                        Destroy(collidedGameObjects3[i]);
                        scrscrp.DodajScore(1);
                    }
                    //PlayerCat(Clone)
                    //else if (collidedGameObjects3[i]?.GetComponent<Transform>().name == "PlayerCat(Clone)")
                    //{
                    //    currentHealth = hp - 100;
                    //    this.GetComponent<HealthBarScript>().SetHealth(currentHealth);
                    //}
                }
            }

            if (collidedGameObjects4.Length > 0)
                {
                    for (int i = 0; i < collidedGameObjects4.Length; i++)
                    {
                        if (collidedGameObjects4[i]?.GetComponent<Transform>().name == "Destructable(Clone)")
                        {
                            //lvlMng.MakeNodeWalkable(new Vector3(collidedGameObjects4[i].transform.position.x,collidedGameObjects4[i].transform.position.y,collidedGameObjects4[i].transform.position.z));

                            Destroy(collidedGameObjects4[i]);
                            FindObjectOfType<PowerUpSpawner>().BlockDestroyed(collidedGameObjects4[i].transform.position);
                        }
                        
                        else if (collidedGameObjects4[i]?.GetComponent<Transform>().name == "0" ||
                                 collidedGameObjects4[i]?.GetComponent<Transform>().name == "1" ||
                                 collidedGameObjects4[i]?.GetComponent<Transform>().name == "2")
                        {
                            Destroy(collidedGameObjects4[i]);
                            scrscrp.DodajScore(1);
                        }
                        //PlayerCat(Clone)
                        //else if (collidedGameObjects4[i]?.GetComponent<Transform>().name == "PlayerCat(Clone)")
                        //{
                        //    currentHealth = hp - 100;
                        //    this.GetComponent<HealthBarScript>().SetHealth(currentHealth);
                        //}
                    }
                }

                if ((Mathf.RoundToInt(playerRigidBody.position.x) == X &&
                     Mathf.RoundToInt(playerRigidBody.position.z) == Z + 1)||
                    (Mathf.RoundToInt(playerRigidBody.position.x) == X &&
                    Mathf.RoundToInt(playerRigidBody.position.z) == Z - 1) ||
                    (Mathf.RoundToInt(playerRigidBody.position.x) == X + 1 &&
                     Mathf.RoundToInt(playerRigidBody.position.z) == Z) ||
                    (Mathf.RoundToInt(playerRigidBody.position.x) == X - 1 &&
                     Mathf.RoundToInt(playerRigidBody.position.z) == Z))
                {
                    currentHealth = currentHealth - 100;
                    hpscr.SetHealth(currentHealth);
                }
            }
        }
    }

            
        
