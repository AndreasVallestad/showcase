using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;

namespace Funksoft.Barista
{
    //Class Responsible for on screen testing/debugging GUI and Inputs. Messiest class in the whole project. Naming it Test-Something will make you do that.
    public class TestUI : Valley.MonoBehaviourSingleton<TestUI>
    {
        [SerializeField, Header("Panel and Text")]
        private Vector2 _cupPanelPos;
        [SerializeField]
        private Vector2 _midPanelPos;
        [SerializeField]
        private Vector2 _customerPanelPos;
        [SerializeField]
        private Vector2 _logWindowPos;
        [SerializeField]
        private float _panelWidth = 800f;
        [SerializeField]
        private float _panelHeight = 600f;
        [SerializeField]
        private float _textLabelWidth = 200;
        [SerializeField]
        private float _textLabelHeight = 50;
        [SerializeField]
        private float _logLabelWidth = 300;
        [SerializeField]
        private float _logLabelHeight = 50;
        [SerializeField]
        private float _textPaddingX = 50;
        [SerializeField]
        private float _textPaddingY = 50;
        [SerializeField]
        private float _spaceBetweenText = 50;

        [SerializeField, Header("Buttons")]
        private float _buttonWidth = 200f; //Width of standard size button, in screen space units (pixels).
        [SerializeField]
        private float _buttonHeight = 75f; //Height of standard size button, in screen space units (pixels).
        [SerializeField]
        private float _buttonPaddingX = 50f; //Outermost padding on horizontal sides, in screen space units. (pixels).
        [SerializeField]
        private float _buttonPaddingY = 50f; //Outermost padding on vertical sides, in screen space units. (pixels).
        [SerializeField]
        private float _spaceBetweenButtons = 50f; //Spac between buttons in the same row, in screen space units. (pixels).


        #region References
        [SerializeField, Header("References")]
        private Drink _drink;

        [SerializeField]
        private CustomerCounter _customerCounter;

        [SerializeField]
        private List<MainDispenser> _mainDispensers;

        [SerializeField]
        private List<SideDispenser> _sideDispensers;

        private Camera _mainCamera;

        #endregion
        
        private string _currentLogText;
        private Customer _currentCustomer;
        private GUIStyle _style;

        private Vector3 _topLeft;
        private Vector3 _topRight;
        private Vector3 _botLeft;
        private Vector3 _botRight;


        public struct ServeInputTriggered : IEvent
        {
            public Drink drink;
            public Customer customer;
        }


        protected override void Awake()
        {   
            base.Awake();

            _mainCamera = Camera.main;

            _topLeft = new Vector3(0f, 0f, 0f);
            _topRight = new Vector3(Screen.width, 0f);
            _botLeft = new Vector3(0f, Screen.height, 0f);
            _botRight = new Vector3(Screen.width,Screen.height, 0f);
   
        }

        public static void Log(string text)
        {
            Instance._currentLogText = text;
        }
        public static void ClearLog()
        {
            Instance._currentLogText = null;
        }

        private void OnGUI()
        {
            _style = new GUIStyle(GUI.skin.button);

            if (_currentLogText != null)
                DrawLogGUI(_logWindowPos, _logLabelWidth, _logLabelHeight, _currentLogText);

            DrawInputButtonsGUI();
            DrawCupPanelGUI(_cupPanelPos);

            _style.alignment = TextAnchor.MiddleCenter;
            _style.fontSize = 30;
            /*
            Rect cTabsRect;
            //Create tab buttons for each customer & store selected customer
            for(var i = 0; i < _customerCounter.Customers.Count; i++)
            {
                cTabsRect = new Rect(_topRight.x - _buttonPaddingX - _buttonWidth/2 - i * (_buttonWidth/2 + _spaceBetweenButtons),
                                     _topRight.y + _buttonPaddingY
                                     , _buttonWidth/2, _buttonHeight);

                if (GUI.Button(cTabsRect, _customerCounter.Customers[i].TimeRemaining.ToString("F2"), _style))
                {
                    _currentCustomer = _customerCounter.Customers[i];
                }
            }
            //if a customer is selected, create customer panel with its data
            if (_currentCustomer)
                DrawCustomerPanel(_customerPanelPos, _currentCustomer);
            */
        }

        private void DrawLogGUI(Vector2 pos, float width, float height, string text)
        {
            _style.alignment = TextAnchor.UpperLeft;
            _style.fontSize = 28;
            var rect = new Rect(pos.x + _mainCamera.WorldToScreenPoint(pos).x - width/2, 
                                pos.y + _mainCamera.WorldToScreenPoint(pos).y - height/2,
                                width, height);
            GUI.Label(rect, text, _style);
        }

        private void DrawInputButtonsGUI()
        {
            //Set properties and values for buttons and text.
            _style.alignment = TextAnchor.MiddleCenter;
            _style.fontSize = 30;
            Rect rect;

            rect = new Rect(_botLeft.x, _botLeft.y - 50, 50, 50);
            //Quit game button
            if (GUI.Button(rect, "X", _style))
                Application.Quit();
            /*
            //MainIngredient dispenser buttons
            for(var i = 0; i < _mainDispensers.Count; i++)
            {
                MainDispenser dispenser = _mainDispensers[i];

                //Set position and size of buttons.
                rect = new Rect(_topLeft.x + _buttonPaddingX + i * (_buttonWidth + _spaceBetweenButtons), 
                                    _topLeft.y + _buttonPaddingY,
                                    _buttonWidth, _buttonHeight);

                //Create button and detect button input.
                if (GUI.RepeatButton(rect, dispenser.Ingredient.Name, _style))
                    dispenser.Use();
            }

            //SideIngredient dispenser buttons
            for(var i = 0; i < _sideDispensers.Count; i++)
            {
                SideDispenser dispenser = _sideDispensers[i];

                //Set position and size of buttons.
                rect = new Rect(_topLeft.x + _buttonPaddingX + i * (_buttonWidth + _spaceBetweenButtons), 
                                    _topLeft.y + _buttonPaddingY + _buttonHeight + _spaceBetweenButtons,
                                    _buttonWidth, _buttonHeight);

                //Create button and detect button input.
                if (GUI.Button(rect, dispenser.Ingredient.Name, _style))
                    dispenser.Use();
            }

            #region Clear Drink Button
            //Set position and size of button
            rect = new Rect(_botLeft.x + _buttonPaddingX, _botLeft.y - _buttonPaddingY - _buttonHeight , _buttonWidth, _buttonHeight);
            //Create button and detect button input
            if (GUI.Button(rect, "Clear Drink", _style))
            {
                _drink.Clear();
            }
                
            #endregion
            
            #region Serve Drink Button
            //Set position and size of button
            rect = new Rect(_botRight.x - _buttonWidth -_buttonPaddingX, _botRight.y - _buttonPaddingY - _buttonHeight, _buttonWidth, _buttonHeight);
            //Create button and detect button input.
            if (GUI.Button(rect, "Serve", _style))
            {
                var inputEvent = new ServeInputTriggered{drink = _drink, customer = _currentCustomer};
                EventBus<ServeInputTriggered>.Raise(inputEvent);
            }
                
            #endregion
            */
        }

        private void DrawMidPanelGUI(DrinkRecipeData recipe, Vector2 pos)
        {
            Rect textRect;

            //Create Panel and Title text
            var panelRect = new Rect(_topLeft.x + _mainCamera.WorldToScreenPoint(pos).x - _panelWidth/2, 
                                     _topLeft.y + _mainCamera.WorldToScreenPoint(pos).y - _panelHeight/2, 
                                     _panelWidth, _panelHeight);
            
            if (recipe != null)
            {
                //Drink recipe title text
                _style.alignment = TextAnchor.UpperCenter;
                _style.fontSize = 30;
                GUI.Box(panelRect, "Assembled Drink: " + recipe.Name, _style);

                //Style for text labels
                _style.alignment = TextAnchor.MiddleCenter;
                _style.fontSize = 25;
                
                //Ingredients text labels
                for(var i = 0; i < recipe.Ingredients.Count; i++)
                {
                    //Set area for text label
                    textRect = new Rect(panelRect.x + _textPaddingX, 
                                        panelRect.y + _textPaddingY + i * (_spaceBetweenText + _textLabelHeight), 
                                        _textLabelWidth, _textLabelHeight);
                    //Create text label
                    GUI.Label(textRect, recipe.Ingredients[i].Name, _style);
                }

            }
            //Log window
            textRect = new Rect(panelRect.x, panelRect.y + _panelHeight - _textLabelHeight*3, _panelWidth, _textLabelHeight*3);
            GUI.Label(textRect, "Test string", _style);



        }

        private void DrawCupPanelGUI(Vector2 pos)
        {/*
            //Create Panel and Title text
            var panelRect = new Rect(_topLeft.x + _mainCamera.WorldToScreenPoint(pos).x - _panelWidth/2, 
                                     _topLeft.y + _mainCamera.WorldToScreenPoint(pos).y - _panelHeight/2, 
                                     _panelWidth, _panelHeight);
            _style.alignment = TextAnchor.UpperCenter;
            GUI.Box(panelRect, "Current Drink Mixture Contents", _style);

            //Set style for text contained in panel
            _style.alignment = TextAnchor.MiddleCenter;
            _style.fontSize = 25;

            int index = 0;
            Rect textRect;
            //Display values for each main ingredient
            foreach(KeyValuePair<MainIngredientData, float> pair in _drink.DrinkMixture.MainIngredients)
            {
                //Set area for text label
                textRect = new Rect(panelRect.x + _textPaddingX, 
                                    panelRect.y + _textPaddingY + index * (_spaceBetweenText + _textLabelHeight), 
                                    _textLabelWidth, _textLabelHeight);
                //Create text label
                GUI.Label(textRect, pair.Key.Name + " : " + pair.Value.ToString("F2") + "ml", _style);
                index++;
            }

            index = 0;
            foreach(SideIngredientData si in _drink.DrinkMixture.SideIngredients.HashSet)
            {
                //Set area for text label
                textRect = new Rect(panelRect.x + _panelWidth - _textPaddingX - _textLabelWidth, 
                                    panelRect.y + _textPaddingY + index * (_spaceBetweenText + _textLabelHeight), 
                                    _textLabelWidth, _textLabelHeight);
                //Create text label
                GUI.Label(textRect, si.Name, _style);
                index++;
            }
            
            
            //Total liquid text
            textRect = new Rect(panelRect.x, panelRect.y + _panelHeight - _textLabelHeight, _panelWidth, _textLabelHeight);
            GUI.Label(textRect, "Total Liquid: " + _drink.DrinkMixture.GetTotalLiquid.ToString("F2") + "ml / " + _drink.DrinkMixture.MaxTotalLiquid + "ml", _style);
            */
        }

        private void DrawCustomerPanel(Vector2 pos, Customer customer)
        {
            //Create Panel and Title text
            var panelRect = new Rect(_topLeft.x + _mainCamera.WorldToScreenPoint(pos).x - _panelWidth/2, 
                                     _topLeft.y + _mainCamera.WorldToScreenPoint(pos).y - _panelHeight/2, 
                                     _panelWidth, _panelHeight);
            //Set style for panel and title text
            _style.alignment = TextAnchor.UpperCenter;
            _style.fontSize = 30;
            //Create panel and title text
            GUI.Box(panelRect, "Customer: " + customer.CustomerData.name + " | Time Left: " + customer.TimeRemaining.ToString("F2"), _style);

            Rect textRect;
            textRect = new Rect(panelRect.x + _panelWidth / 2 - _textLabelWidth/2, panelRect.y + _textPaddingY, _textLabelWidth, _textLabelHeight);

            _style.alignment = TextAnchor.MiddleCenter;
            GUI.Label(textRect, customer.Order.Drink.Name, _style);
            _style.fontSize = 25;

            //Add text for each main ingredient in customer order
            for(var i = 0; i < customer.Order.Drink.Ingredients.Count; i++)
            {
                //Set area for text label
                textRect = new Rect(panelRect.x + _textPaddingX, 
                                    panelRect.y + _textPaddingY + _textLabelHeight + _spaceBetweenText + i * (_spaceBetweenText + _textLabelHeight), 
                                    _textLabelWidth, _textLabelHeight);
                //Create text label
                GUI.Label(textRect, customer.Order.Drink.Ingredients[i].Name, _style);
            }

            int index = 0;
            foreach(SideIngredientData si in customer.Order.SideIngredients)
            {
                //Set area for text label
                textRect = new Rect(panelRect.x + _panelWidth - _textPaddingX - _textLabelWidth, 
                                    panelRect.y + _textPaddingY + _textLabelHeight + _spaceBetweenText + index * (_spaceBetweenText + _textLabelHeight), 
                                    _textLabelWidth, _textLabelHeight);
                //Create text label
                GUI.Label(textRect, si.Name, _style);
                index++;
            }
        }

        
    }
}
