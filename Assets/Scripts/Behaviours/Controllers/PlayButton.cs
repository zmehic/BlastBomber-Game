using System;
using System.Collections;
using System.Collections.Generic;
using BlastBomberV2.Management;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BlastBomberV2
{
    public class PlayButton : MonoBehaviour
    {

        [SerializeField]
        public Image oldImage;
        [SerializeField]
        public Sprite newSpriteGrass;
        [SerializeField]
        public Sprite newSpriteDesert;
        [SerializeField]
        public Sprite newSpriteIce;
        [SerializeField]
        public Sprite newSpriteDungeon;

        [SerializeField] public Sprite OldSprite;
        [SerializeField] public Sprite SoundOn;
        [SerializeField] public Sprite SoundOff;


        

        public void ChangeLvl(int lvl)
        {
            OptionsManager.lvl = lvl;
            if(OptionsManager.lvl==0)
                oldImage.sprite = newSpriteGrass;
            else if (OptionsManager.lvl == 1)
                oldImage.sprite = newSpriteDesert;
            else if (OptionsManager.lvl == 2)
                oldImage.sprite = newSpriteDungeon;
            else
            {
                oldImage.sprite = newSpriteIce;
            }

        }
        public void ChangeDiff(int diff)
        {
            OptionsManager.diff = diff;
            
        }

        public void ChangeCharacter(int character)
        {
            OptionsManager.character = character;
        }

        public void ChangeMode(int mode)
        {
            OptionsManager.mode = mode;
        }

        public void ChangeSound()
        {
            if (OptionsManager.sound == true)
            {
                OptionsManager.sound = false;
                AudioListener.pause = true;
                

            }
            else
            {
                OptionsManager.sound = true;
                AudioListener.pause = false;
                OldSprite = SoundOff;

            }
        }

        public void PlayGame()
        {
            if (OptionsManager.character == 0)
            {
                SceneManager.LoadScene("Playground");
                
            }
            else
            {
                SceneManager.LoadScene("Playground 1");
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
