using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace BlastBomberV2
{
    public class TimerScript : MonoBehaviour
    {
        public float timerValue = 120;
        public TMPro.TMP_Text timerText;

        void Update()
        {
            if (timerValue > 0)
            {
                timerValue -= Time.deltaTime;
            }
            else
            {
                timerValue = 0;
            }

            DisplayTime(timerValue);
        }

        void DisplayTime(float timeToDisplay)
        {
            if (timeToDisplay < 0)
            {
                timeToDisplay = 0;
            }

            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            
            timerText.text=string.Format("{0:00}:{1:00}",minutes,seconds);
        }
    }
}
