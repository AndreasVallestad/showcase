using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;

namespace Funksoft.Barista
{
    public class Drink : MonoBehaviour, IEventReceiver<MainDispenser.Used>, IEventReceiver<SideDispenser.Used>, IEventReceiver<Customer.Leave>
    {
        [SerializeField]
        private bool _debugLogsEnabled = false;

        [SerializeField]
        private DisplayCupContents _displayCupContents;

        [SerializeField]
        public DrinkMixture DrinkMixture = new DrinkMixture(); //Holds the actual ingredients and amounts of liquid in the drink.

        private void Awake()
        {
            _displayCupContents.DrinkMixture = DrinkMixture;
        }

        //Subscribe to events
        private void OnEnable()
        {
            EventBus.Register(this);
        }
        private void OnDisable()
        {
            EventBus.UnRegister(this);
        }

        //Receive events from pEventBus
        public void OnEvent(MainDispenser.Used e)
        {
            AddMainIngredient(e.ingredient, e.amount);
            _displayCupContents.UpdateDisplay();
        }
        public void OnEvent(SideDispenser.Used e)
        {
            AddSideIngredient(e.ingredient);
            _displayCupContents.UpdateSideIngredientDisplay();
        }
        public void OnEvent(Customer.Leave e)
        {
            if (e.satisfied)
                Clear();
        }

        public void AddMainIngredient(MainIngredientData ingredient, float amount)
        {
            //Add non existant ingredient keys before attempting to change any values. Prevents crash from attempting to change value of yet to be added key.
            if (!DrinkMixture.MainIngredients.ContainsKey(ingredient))
                DrinkMixture.MainIngredients.Add(ingredient, 0f);

            //If cup does not overflow if amount is added, allow it
            if (DrinkMixture.GetTotalLiquid + amount < DrinkMixture.MaxTotalLiquid)
            {
                DrinkMixture.MainIngredients[ingredient] += amount;
            }
            else //If it would overflow, fill to the brim instead.
            {
                DrinkMixture.MainIngredients[ingredient] += (DrinkMixture.MaxTotalLiquid - DrinkMixture.GetTotalLiquid);
            }
        }

        public void AddSideIngredient(SideIngredientData ingredient)
        {
            DrinkMixture.SideIngredients.HashSet.Add(ingredient);
        }

        public void Clear()
        {
            DrinkMixture = new DrinkMixture();
            _displayCupContents.DrinkMixture = DrinkMixture;
            _displayCupContents.ResetIngredientDisplay();
            if (_debugLogsEnabled)
                TestUI.Log("Drink contents emptied.");
        }

        
    }
}

