using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace BlastBomberV2
{
    public class ScoreScript : MonoBehaviour
    {
        public int scoreValue = 0;
        public TMP_Text valueScore;

        public void DodajScore(int brojPoena)
        {
            scoreValue += brojPoena;
            if(scoreValue>9)
            {
                valueScore.text = scoreValue.ToString();
            }
            else if (scoreValue <= 9)
            {
                valueScore.text = "0" + scoreValue.ToString();
            }
        }
        

    }
}
