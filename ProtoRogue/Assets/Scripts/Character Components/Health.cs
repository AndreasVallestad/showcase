using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProtoRogue{
    public class Health : MonoBehaviour, IAttackable, IDamagable
    {
        //Holds the normal, default max health. Doubles as the starting health.
        [SerializeField]
        private float maxHealth = default;
        
        //Holds current health
        [SerializeField]
        private float currentHealth = default;
        public float CurrentHealth{ 
            get{return currentHealth;}
            //Cap currentHealth at maxHealth
            private set
            {
                //Setvalue, cap value, and call death function when out of health
                currentHealth = value;
                currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
                if (currentHealth <= 0f){Die();}
            }
        }

        private void Start()
        {
            //Set initial health at full health
            CurrentHealth = maxHealth;
        }

        //Called by attacker when attacked.
        public void OnAttacked(Attack attackingInst)
        {
            Debug.Log("<size=14><b>" + attackingInst.name + "</b> attacked <b>" +  name + "</b> and dealt <b>" + attackingInst.damage.ToString() + "</b> damage.</size>");
            //If this gameObject has a script that handles taking damage (implements IDamagable), tell it to take damage from the attack taken.
            TakeDamage(attackingInst.damage);
        }

        public void TakeDamage(float damage)
        {
            //reduce health by damage
            CurrentHealth -= damage;
        }

        private void Die()
        {
            Debug.Log("<size=14><b>" + name + "</b> died.</size>");
            Destroy(gameObject);
        }
    }
}
