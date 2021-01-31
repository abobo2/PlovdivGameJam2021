using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthWidget :MonoBehaviour
    {
        public Health Target;

        public List<Image> HealthImages;

        public Sprite Health;

        public Sprite NoHealth;

        public void Update()
        {
            for (int i = 0; i < HealthImages.Count; i++)
            {
                HealthImages[i].sprite = Target.Value > i ? Health : NoHealth;
            }
        }
    }
}