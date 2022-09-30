using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Funksoft.Barista
{
    public class CustomerUI : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private GameObject _orderPanel;
        [SerializeField]
        public GameObject HeadUI;
        [SerializeField]
        private TextMeshProUGUI _headTimerText;

        [SerializeField]
        private Image _orderImage;
        [SerializeField]
        private TextMeshProUGUI _orderTitleText;
        [SerializeField]
        private TextMeshProUGUI _orderSideIngredientText;
        [SerializeField]
        private TextMeshProUGUI _orderTimerText;

        private Customer _customer;
        public Customer Customer
        {
            get {return _customer;}
            set
            {
                _customer = value;
                DisplayOrderData();
            }
        }
        public Vector3 WorldPos{ get; set;}


        private void Start()
        {
            _orderPanel.SetActive(false);
            DisplayOrderData();   
        }


        private void LateUpdate()
        {
            _headTimerText.text = Customer.TimeRemaining.ToString("F0");
            _orderTimerText.text = _headTimerText.text;
        }

        /*
        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            //_showExtendedInfo = true;
            _orderPanel.SetActive(true);
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            //_showExtendedInfo = false;
            _orderPanel.SetActive(false);
        }
        */

        public void OpenOrderPanel()
        {
            _orderPanel.SetActive(true);
            HeadUI.SetActive(false);
        }
        public void CloseOrderPanel()
        {
            _orderPanel.SetActive(false);
            HeadUI.SetActive(true);
        }

        public void ActivateServe()
        {
            _customer.AttemptServe();
        }

        //Give display components the relevant data
        private void DisplayOrderData()
        {

            _orderTitleText.text = Customer.Order.Drink.Name;
            _orderImage.sprite = Customer.Order.Drink.DrinkSprite;
            //Name each side ingredient on a new line. Clear first to wipe inspector written scene view test content.
            _orderSideIngredientText.text = "";
            foreach(SideIngredientData si in Customer.Order.SideIngredients)
            {
                _orderSideIngredientText.text += si.Name + "\n";
            }
        }
    }
}
