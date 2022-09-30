using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using pEventBus;

namespace Funksoft.Barista
{
    public class Customer : MonoBehaviour, IClickable, IEventReceiver<Shift.ShiftStateEvent>
    {
        [SerializeField]
        private bool _debugLogsEnabled = false;

        [SerializeField]
        private DatabaseSO _database;

        [SerializeField]
        public CustomerData CustomerData;

        public Order Order;
        
        public float TimeRemaining;

        public int FailedServingsCount = 0;

        private bool _pauseTimer;

        private SpriteRenderer _spriteRenderer;

        public struct Leave : IEvent
        {
            public Customer customer;
            public bool satisfied;
        }

        public struct ServeDrinkInput : IEvent
        {
            public Customer customer;
        }

        public struct Selected : IEvent
        {
            public Customer customer;
        }

        private void Awake()
        {
            TryGetComponent<SpriteRenderer>(out _spriteRenderer);
            //Create order in Awake instead of Start, so the Customer UI can be activated at the same time as the customer is instantiated, without order being missing or outdated.
            CreateRandomOrder();
        }


        private void Start()
        {
            _spriteRenderer.sprite = CustomerData.Sprite;
            TimeRemaining = CustomerData.PatienceTimer;
            if (_debugLogsEnabled)
            {
                TestUI.Log("Customer Type: " + CustomerData.name + ". Patience Time: " + CustomerData.PatienceTimer);
            }
            
        }

        private void OnEnable()
        {
            EventBus.Register(this);
        }
        private void OnDisable()
        {
            EventBus.UnRegister(this);
        }

        public void OnEvent(Shift.ShiftStateEvent e)
        {
            switch(e.type)
            {
                case Shift.ShiftEventType.Paused:
                    _pauseTimer = true;
                    break;
                case Shift.ShiftEventType.Unpaused:
                    _pauseTimer = false;
                    break;
                case Shift.ShiftEventType.ShiftEnded:
                    LeaveUnsatisfied();
                    break;
            }
        }

        private void Update()
        {
            
            //Customer Patience Countdown. Leave when timer has run out.
            if (!_pauseTimer)
                TimeRemaining -= Time.deltaTime;

            if (TimeRemaining < Mathf.Epsilon)
            {
                LeaveUnsatisfied();
            }
        }

        public void OnActivation()
        {
            //EventBus<ServeDrinkInput>.Raise(new ServeDrinkInput{ customer = this});
            EventBus<Selected>.Raise(new Selected{ customer = this});
        
        }
        //Provide name and sprites for this object and its clickable component states.
        public string GetDisplayName()
        {
            return "Serve drink to " + CustomerData.name;
        }
        public Sprite GetHoverSprite()
        {
            return CustomerData.HoverSprite;
        }
        public Sprite GetClickedSprite()
        {
            return CustomerData.ClickedSprite;
        }

        public void AttemptServe()
        {
            EventBus<ServeDrinkInput>.Raise(new ServeDrinkInput{ customer = this});
        }

        public void ServedWrongOrder()
        {
            //Play bad groan sound
            if (FailedServingsCount < CustomerData.MaxServeTries)
            {
                //Wrong order, but wont leave yet
                FailedServingsCount++;
                return;
            }
            LeaveUnsatisfied();
        }

        public void LeaveSatisfied()
        {
            EventBus<Leave>.Raise
            (new Leave{ customer = this, satisfied = true });
        }

        private void LeaveUnsatisfied()
        {
            EventBus<Leave>.Raise
            (new Leave{ customer = this, satisfied = false });
        }

        private void CreateRandomOrder()
        {
            //Get random DrinkRecipe from database.
            DrinkRecipeData recipe = _database.DrinkRecipes.HashSet.ElementAt(UnityEngine.Random.Range(0, _database.DrinkRecipes.HashSet.Count));
            HashSet<SideIngredientData> sideIngredients = new HashSet<SideIngredientData>();

            //50-50 chance of each side ingredient getting added to drink.
            foreach(SideIngredientData si in _database.SideIngredients.HashSet)
            {
                if (UnityEngine.Random.Range(0,2) > 0)
                    sideIngredients.Add(si);
            }
            //Create order
            Order = new Order(recipe, sideIngredients, 5f);
                
        }
    }
}
