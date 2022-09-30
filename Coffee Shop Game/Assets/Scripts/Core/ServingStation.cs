using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;

namespace Funksoft.Barista
{
    public class ServingStation : MonoBehaviour, IEventReceiver<Customer.ServeDrinkInput>
    {
        [SerializeField]
        private bool _debugLogsEnabled = false;

        [SerializeField]
        private Drink _drink;

        private DrinkAssembler _drinkAssembler;

        private void Awake()
        {
            TryGetComponent<DrinkAssembler>(out _drinkAssembler);
        }

        private void OnEnable()
        {
            EventBus.Register(this);
        }
        private void OnDisable()
        {
            EventBus.UnRegister(this);
        }

        //Triggered when serve input is called (aka. when a customer is clicked)
        public void OnEvent(Customer.ServeDrinkInput e)
        {
            if (TryServeDrink(_drink, e.customer))
            {
                if (_debugLogsEnabled)
                    TestUI.Log("Customer " + e.customer + "'s order was filled and left satisfied.");
                e.customer.LeaveSatisfied();
                return;
            }
            //Cancel serve attempt if there are no main ingredients in the drinkmixture. Customer will not lose patience or count it as a mistake.
            if (_drink.DrinkMixture.MainIngredients.Count == 0)
            {
                if (_debugLogsEnabled)
                    TestUI.Log("You cannot serve the customer an empty drink.");
                return;
            }

            e.customer.ServedWrongOrder();
        }

        /*
        //Attempt to serve the drink when the input is received
        public void OnEvent(TestUI.ServeInputTriggered e)
        {
            if (TryServeDrink(e.drink, e.customer))
            {
                if (_debugLogsEnabled)
                    TestUI.Log("Customer " + e.customer + "'s order was filled and left satisfied.");
                e.customer.LeaveSatisfied();
                return;
            }
            e.customer.ServedWrongOrder();
        }
        */

        public bool TryServeDrink(Drink drink, Customer customer)
        {
            //Make sure customer still exists
            if (customer == null)
                return false;

            //Attempt to assemble drink.
            var assembledDrinkRecipe = _drinkAssembler.AssembleDrink(drink.DrinkMixture);

            //If the converted and assembled drink does not match any recipe
            if (assembledDrinkRecipe == null)
            {
                if (_debugLogsEnabled)
                    TestUI.Log("Mistake: Drink does not match any existing recipe");
                return false;
            }

            //The converted and asembled drink result is not the recipe the customer ordered
            if (assembledDrinkRecipe != customer.Order.Drink)
            {
                if (_debugLogsEnabled)
                    TestUI.Log("Mistake: Wrong drink served. " + customer.CustomerData.name + " ordered: " + 
                              customer.Order.Drink.Name + ". \n You served: " + assembledDrinkRecipe.Name + ".");
                return false;
            }
                
            //Check if drink has all side ingredients ordered.
            foreach(SideIngredientData si in customer.Order.SideIngredients)
            {
                if (!drink.DrinkMixture.SideIngredients.HashSet.Contains(si))
                {
                    if (_debugLogsEnabled)
                        TestUI.Log("Mistake: Drink is missing " + si.Name);
                    return false;
                }
            }
            //Check that drink contains no side ingredients that were not ordered.
            foreach(SideIngredientData si in drink.DrinkMixture.SideIngredients.HashSet)
            {
                if (!customer.Order.SideIngredients.Contains(si))
                {
                    if (_debugLogsEnabled)
                        TestUI.Log("Mistake: Drink was not supposed to have " + si.Name);
                    return false;
                }
            }

            return true;
        }
    }
}
