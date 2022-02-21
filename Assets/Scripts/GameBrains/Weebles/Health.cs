using System;
using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace GameBrains.Weebles
{
    public class Health : ExtendedMonoBehaviour
    {
        [SerializeField] int maxHealth = 100;
        int currentHealth;

        public event Action<float> OnHealthPercentChanged = delegate { };

        public override void OnEnable()
        {
            base.OnEnable();
            currentHealth = maxHealth;
        }

        public void ModifyHealth(int amount)
        {
            currentHealth += amount;
            var currentHealthPercent = currentHealth / (float)maxHealth;
            OnHealthPercentChanged(currentHealthPercent);
        }

        public override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.Space)) { ModifyHealth(-10); }
        }
    }
}