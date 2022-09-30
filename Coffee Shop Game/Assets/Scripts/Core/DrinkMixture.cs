using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley;

namespace Funksoft.Barista
{

    [System.Serializable]
    public class DrinkMixture
    {
        //Max amount of total liquid in cup, measured in milliliters. Amount of all ingredients added must be <= this.
        [field: SerializeField]
        public float MaxTotalLiquid{ get; private set;} = 250f;

        public float GetTotalLiquid
        {
            get
            {
                //No entries means no liquid, avoids crash when empty
                if (MainIngredients.Count <= 0)
                    return 0f;

                float runningTotal = 0f;
                //Get total summed values of all ingredient entries
                foreach(KeyValuePair<MainIngredientData, float> pair in MainIngredients)
                    runningTotal += pair.Value;
                return runningTotal;
            }
        }
        
        private Dictionary<MainIngredientData, float> _mainIngredients = new Dictionary<MainIngredientData, float>();
        public Dictionary<MainIngredientData, float> MainIngredients
        {
            get{ return _mainIngredients;}
            private set
            {
                _mainIngredients = value;
                //ClearEmptyIngredients();
            }
        }

        [field: SerializeField]
        public SerializableHashSet<SideIngredientData> SideIngredients{ get; private set; } = new SerializableHashSet<SideIngredientData>();

        
        //Remove ingredient entries with no liquid from dictionary. Safety
        private void ClearEmptyIngredients()
        {
            foreach(KeyValuePair<MainIngredientData, float> pair in _mainIngredients)
            {
                if (pair.Value <= Mathf.Epsilon)
                    _mainIngredients.Remove(pair.Key);
            }
        }

        

        

    }
}
