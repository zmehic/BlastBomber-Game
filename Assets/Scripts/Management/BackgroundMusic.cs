using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastBomberV2.Management
{
    public class BackgroundMusic : MonoBehaviour
    {
        private static BackgroundMusic _backgroundMusic;

        private void Start()
        {
            if (_backgroundMusic == null)
            {
                _backgroundMusic = this;
                DontDestroyOnLoad(_backgroundMusic);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
