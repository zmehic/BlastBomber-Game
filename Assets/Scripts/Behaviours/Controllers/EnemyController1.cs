using System;
using System.Collections;
using System.Collections.Generic;
using BlastBomberV2.Core;
using BlastBomberV2.Management;
using BlastBomberV2.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlastBomberV2.Behaviours.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyController1 : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;

        private LevelManager levelManager;
        private List<Node<ILevelBlock>> path;
        private Node<ILevelBlock> randomWalkableNode;
        private Vector3 currentTarget;
        private Rigidbody currentRigidBody;
        private int wayPointIndex = 0;
        private PathFinder<ILevelBlock> _pathFinder;
        private Color debuggerLineColor;
        private Vector3 previousPositioin;
        private Animator _animator;
        private EnemiesManager enemiesManager;

        private void Start()
        {
            _animator = this.GetComponent<Animator>();
            enemiesManager = FindObjectOfType<EnemiesManager>();
            debuggerLineColor = Random.ColorHSV();
            currentRigidBody = this.GetComponent<Rigidbody>();
            levelManager = FindObjectOfType<LevelManager>();
            _pathFinder = new PathFinder<ILevelBlock>();
            randomWalkableNode = levelManager.GetRandomWalkableNode();
            path = new List<Node<ILevelBlock>>(
                _pathFinder.FindPath(levelManager.WorldPositionToNode(this.transform.position), randomWalkableNode));

            currentTarget = levelManager.NodeToWorldPosition(path[wayPointIndex]);

            StartCoroutine(CheckForPosition());
        }

        private void OnCollisionEnter(Collision collision)
        {
            PlayerController other = collision.gameObject.GetComponent<PlayerController>();
            if (other)
            {
                _animator.SetBool("isWalking",false);
                
            }
            
        }

        private void OnCollisionExit(Collision other)
        {
            _animator.SetBool("isWalking",true);
            
        }

        private void Update()
        {
            this.transform.LookAt(currentTarget, Vector3.up);
        }

        private IEnumerator CheckForPosition()
        {
            while (true)
            {
                yield return new WaitForSeconds(2);
                var currentPosition = this.transform.position;

                if ((previousPositioin - currentPosition).magnitude <= 0.3f)
               {
                   path = new List<Node<ILevelBlock>>(
                       _pathFinder.FindPath(levelManager.WorldPositionToNode(previousPositioin),
                           levelManager.GetRandomWalkableNode()));
                   wayPointIndex = 0;
                   currentTarget = levelManager.NodeToWorldPosition(path[wayPointIndex]);

                   Debug.Log("Enemy remained idle");
               }

                previousPositioin = currentPosition;
            }
        }

        private void FixedUpdate()
        {
            if (path.Count == 0)
                return;

            NavigatePath();
        }

       private void NavigatePath()
       {
           while (Vector3.Distance(this.transform.position, currentTarget) >= 0.2f)
           {
               currentRigidBody.MovePosition(Vector3.MoveTowards(this.transform.position,currentTarget,enemyData.speed*Time.fixedDeltaTime));
               return;
           }

           wayPointIndex++;
           if (wayPointIndex == path.Count)
           {
               path = new List<Node<ILevelBlock>>(
                   _pathFinder.FindPath(levelManager.WorldPositionToNode(this.transform.position),
                       levelManager.GetRandomWalkableNode()));
               wayPointIndex = 0;
           }

           currentTarget = levelManager.NodeToWorldPosition(path[wayPointIndex]);
       }

        private void OnDrawGizmos()
        {
            if (path != null)
            {
                Gizmos.color = debuggerLineColor;
                for (int i = 0; i < path.Count - 1; i++)
                {
                    var startNode = new Vector3(path[i].X, 0.5f, path[i].Y);
                    var endNode = new Vector3(path[i + 1].X, 0.5f, path[i + 1].Y);
                    Gizmos.DrawLine(startNode, endNode);
                }
            }
        }

        private void OnDestroy()
        {
            #if UNITY_EDITOR
            if (!Application.isPlaying) return;
            #endif
            enemiesManager.EnemyKilled(int.Parse(this.name));
            Debug.Log("Got Destroyed");
        }
    }
}