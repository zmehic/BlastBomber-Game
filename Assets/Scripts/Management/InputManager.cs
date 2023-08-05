using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastBomberV2.Management
{
    public class InputManager : MonoBehaviour
    {
        public event Action<float, float> MovementInputReceived;
        public event Action Fire;
        public event Action Exit;

        private void FixedUpdate()
        {
            MovementInputReceived?.Invoke(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Exit?.Invoke();
            }
        }
    }
}
