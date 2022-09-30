using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProtoRogue
{
    public class BodyPart
    {
        public string partName;
        //Sub-parts this bodypart consists of (parental hierarchy)
        public List<BodyPart> subBodyParts = new List<BodyPart>();

        private float health;
        public float Health
        {
            get{ return health;}
            set
            {
                health = Mathf.Max(0f, value);
                UpdateStatus();
            }
        }
        private float maxHealth;

        //How much coverage this bodypart (and its subren) has of its parent bodypart (ex: right arm is 0.18 (18%) of the body, hand is 0.12 (12%) of the arm)
        private float coverage;
        public float Coverage
        {
            get{ return coverage;}
            //Limit value to be set only within (and including) 0 & 1.
            private set{coverage = Mathf.Clamp(value, 0f, 1f);}
        }
        //Get the coverage of this bodypart that does not include its sub parts coverage. Ex: the coverage of the arm without including the coverage of the hand.
        private float RemainingCoverage
        {
            get
            {
                float result = 1f;
                //Reduce by the coverage of each sub part from the parent to get the "this part only" coverage value
                foreach(BodyPart part in subBodyParts)
                {
                    result -= part.Coverage;
                }
                return result;
            }
        }

        //
        public enum Status{Healthy, Injured, Disabled, Destroyed}
        public Status status = Status.Healthy;


        public BodyPart(string partName, float coverage = 1f, float health = 100f)
        {
            this.partName = partName;
            this.Coverage = coverage;
            this.Health = health;
            maxHealth = health;
        }

        //Update the current status of the bodypart based on its current health
        private void UpdateStatus()
        {
            if (Health >= maxHealth)
                status = Status.Healthy;
            else if (Health <= 0f)
                status = Status.Destroyed;
            else if (Health <= maxHealth / 2)
                status = Status.Disabled;
            else
                status = Status.Injured;

        }
    }
}