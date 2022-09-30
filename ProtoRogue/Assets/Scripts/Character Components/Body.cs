using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ProtoRogue
{
    public class Body : MonoBehaviour
    {
        public BodyPart mainBody = new BodyPart("Body");

        [SerializeField]
        private bool _debugLogs = false;

        private void Awake()
        {
            //Manual addition of bodyparts and sub-bodyparts lower in the boidy hierarchy
            mainBody.subBodyParts.Add(new BodyPart("Torso", 0.325f, 200));
            mainBody.subBodyParts.Add(new BodyPart("Head", 0.075f));
            mainBody.subBodyParts.Add(new BodyPart("Right Arm", 0.125f));
            mainBody.subBodyParts.Add(new BodyPart("Left Arm", 0.125f));
            mainBody.subBodyParts.Add(new BodyPart("Right Leg", 0.175f));
            mainBody.subBodyParts.Add(new BodyPart("Left Leg", 0.175f));
            //Order sub bodyparts list by their overage values. Low to high.
            mainBody.subBodyParts = mainBody.subBodyParts.OrderBy(o=>o.Coverage).ToList();
            if (_debugLogs)
                Debug.Log("Bodypart selected: " + GetRandomSubPart(mainBody).partName);
        }
        
        public BodyPart GetRandomSubPart(BodyPart part, bool coverageWeighted = true)
        {
            //Return a random body part, chance based on each parts coverage value. 
            if (coverageWeighted)
            {
                //Roll a random value to check which bodyparts range it falls within.
                float roll = Random.Range(0f, 0.99f);
                //Keep a running total of the checked coverage values to compare to.
                float runningTotal = 0f;

                if (_debugLogs)
                    Debug.Log("Roll: " + roll.ToString("F3"));
                
                //Check each bodypart if the roll is within their coverage value, 
                foreach(BodyPart subPart in part.subBodyParts)
                {
                    //adds coverage and checks against running total to
                    runningTotal += subPart.Coverage;
                    if (_debugLogs)
                        Debug.Log("Current Part: " + subPart.partName + ", Coverage: " + subPart.Coverage + ". Running Total: " + runningTotal);
                    if (roll < runningTotal)
                    {
                        if (_debugLogs)
                        {
                            Debug.Log("Roll (" + roll.ToString("F3") + ") is within running coverage total of " + runningTotal + " for " + subPart.partName);
                            Debug.Log("Checking for sub-parts on " + subPart.partName);
                        }
                        //Rerun this same function on the selected part to check if a sub-part is chosen.
                        return GetRandomSubPart(subPart);
                    }
                }
                if (_debugLogs)
                    Debug.Log("No further sub bodypart within roll range. Main part selected: " + part.partName);
                //If a further sub part is not selected, or does not exist, return this part.
                return part;
            }
            return null;
        }
        
    }
}