using BlastBomberV2.Management;
using UnityEngine;
using UnityEngine.UI;

namespace BlastBomberV2.Behaviours.Controllers
{
    public class HealthBarScript: MonoBehaviour
    {
        [SerializeField]
        public Slider slider;

        public Gradient gradient;
        public Image Fill;
       
        public void SetHealth(int health)
        {
            
            slider.value = health;
            Fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        public void SetMaxHealth(int health)
        {
            slider.maxValue = health;
            slider.value = health;
            Fill.color = gradient.Evaluate(1f);
        }

    }
}
