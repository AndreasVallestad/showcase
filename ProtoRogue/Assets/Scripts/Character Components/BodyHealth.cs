using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProtoRogue{
    [RequireComponent(typeof(Body))]
    public class BodyHealth : MonoBehaviour, IAttackable, IDamagable
    {
        //The body this health component is for
        private Body body;

        private void Awake()
        {
            TryGetComponent<Body>(out body);
        }

        //Called by attacker when attacked.
        public void OnAttacked(Attack attackingInst)
        {
            //Todo: Replace dying check. Temporary solution, will be replaced. 
            //If the essential bodyparts are destroyed, destroy the character. Prevents freezing during testing when destroying all limbs.
            foreach (BodyPart p in body.mainBody.subBodyParts)
            {
                if (p.partName == "Head" || p.partName == "Torso")
                {
                    if (p.Health < Mathf.Epsilon)
                    {
                        Debug.Log(gameObject.name + " was destroyed by " + attackingInst.gameObject.name);
                        Destroy(gameObject);
                    }
                        
                }
            }

            BodyPart attackedPart;
            //Selected a random SubPart on the body that is above 0 health, and deal the attackers damage to it. (omit Do While loop to let destroyed limbs be targeted)
            do
                attackedPart = body.GetRandomSubPart(body.mainBody);
            while (attackedPart.Health <= 0f);

            attackedPart.Health -= attackingInst.damage;

            Debug.Log("<size=14><b>" + attackingInst.name + "</b> attacked the <b>" + attackedPart.partName + "</b> of <b>" 
            + name + "</b> and dealt <b>" + attackingInst.damage.ToString() + "</b> damage. The " + attackedPart.partName 
            + " of " + name + " has " + attackedPart.Health + " health remaining </size>");
        }

        //Currently only here to fulfill IDamagable requirements, may change or remove later
        public void TakeDamage(float damage)
        {
        }
    }
}