using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using pEventBus;
using TMPro;
using System;

namespace Funksoft.Barista
{
    public class CustomerCounter : MonoBehaviour, 
                                   IEventReceiver<Shift.SpawnCustomer>, IEventReceiver<Customer.Leave>, 
                                   IEventReceiver<MoveCamera.CamMoveFinished>, IEventReceiver<MoveCamera.CamMoveStarted>, 
                                   IEventReceiver<Customer.Selected>
    {
        [SerializeField]
        private bool _debugLogsEnabled = false;

        [SerializeField]
        private DatabaseSO _database;

        [SerializeField]
        private GameObject _customerPrefab;
        [SerializeField]
        private CustomerUI[] _customerUIs;

        [SerializeField, Header("Properties")]
        private int _maxCustomerCount = 5;

        [SerializeField]
        private List<Transform> _customerPositions;

        [SerializeField]
        private float _customerUIHeight = 3f;

        public Customer[] Customers = new Customer[3];

        private void OnEnable()
        {
            EventBus.Register(this);
        }
        private void OnDisable()
        {
            EventBus.UnRegister(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                CreateNewCustomer(_database.CustomerTypes.HashSet.ElementAt(UnityEngine.Random.Range(0, _database.CustomerTypes.HashSet.Count)));
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                foreach(Customer c in Customers)
                {
                    Debug.Log(c);
                }
            }
        }

        #region Receive pEventBus events
        //Receive order to spawn new customer from Day/shift manager
        public void OnEvent(Shift.SpawnCustomer e)
        {
            CreateNewCustomer(e.customerType);
        }
        //Receive event from spawned customer when they leave
        public void OnEvent(Customer.Leave e)
        {
            CustomerLeft(e.customer, e.satisfied);
        }
        //Receive event from customer when they are selected, and display order display
        public void OnEvent(Customer.Selected e)
        {
            foreach(CustomerUI cui in _customerUIs)
                cui.CloseOrderPanel();
            
            var index = Array.IndexOf<Customer>(Customers, e.customer);
            _customerUIs[index].OpenOrderPanel();
        }

        //Change Scale of CustomerUI object to make it invisible, when starting a move from left to right screen
        public void OnEvent(MoveCamera.CamMoveStarted e)
        {
            if (e.screenMovingTo == MoveCamera.Screen.RightScreen)
            {
                foreach(CustomerUI CUI in _customerUIs)
                {
                    CUI.gameObject.SetActive(false);
                    CUI.CloseOrderPanel();
                }
            }
        }
        //Change Scale of CustomerUI object to make it visible, when fully moved from right to left screen
        public void OnEvent(MoveCamera.CamMoveFinished e )
        {
            if (e.screenMovedTo == MoveCamera.Screen.LeftScreen)
            {
                foreach(CustomerUI CUI in _customerUIs)
                {
                    if (CUI.Customer == null)
                        continue;
                    CUI.gameObject.SetActive(true);
                    
                    
                    //Set position of UI to be above customer
                    Vector3 customerPos = CUI.Customer.transform.position;
                    CUI.transform.position = Camera.main.WorldToScreenPoint(new Vector3(customerPos.x, customerPos.y + _customerUIHeight, customerPos.z));
                }
            }
        }
        #endregion


        //Remove customers from queue and handle changes needed when customers 
        private void CustomerLeft(Customer customer, bool wasSatisfied)
        {
            if (Customers.Contains(customer))
            {
                //Remove active CustomerUI of customer when they leave.
                int index = Array.IndexOf(Customers, customer);
                _customerUIs[index].CloseOrderPanel();
                _customerUIs[index].gameObject.SetActive(false);

                //Remove customer from array
                Customers[index] = null;
                Destroy(customer.gameObject);
                
                //Handle stats and results based of failed or completed customer
                if (wasSatisfied)
                {
                    PersistentShiftStats.Instance.CompletedCustomers += 1;
                    if (_debugLogsEnabled)
                        TestUI.Log("Customer " + customer.CustomerData.name + " left satisfied with their order.");
                    return;
                }
                if (_debugLogsEnabled)
                    TestUI.Log("Customer " + customer.CustomerData.name + " stomped off unhappy without their order.");
                PersistentShiftStats.Instance.FailedCustomers += 1;
                
            }  
        }


        private void CreateNewCustomer(CustomerData customerData)
        {
            for(var i = 0; i < Customers.Length; i++)
            {
                if (Customers[i] != null)
                    continue;
                
                //Spawn customer, Add to customer List, and put it on the right world position in the queue
                var inst = Instantiate(_customerPrefab, Vector3.zero, Quaternion.identity);
                Customer customer;
                inst.TryGetComponent<Customer>(out customer);
                Customers[i] = customer;
                Vector3 spawnPos = _customerPositions[Array.IndexOf(Customers,customer)].position;
                inst.transform.position = spawnPos;

                //Give the created customer instance its data.
                customer.CustomerData = customerData;

                #region Create CustomerUI
                //Set info, and activate customerUI
                var thisCustomerUI = _customerUIs[Array.IndexOf(Customers, customer)];
                thisCustomerUI.gameObject.SetActive(true);
                thisCustomerUI.transform.localScale = new Vector3(1,1,1);
                thisCustomerUI.Customer = customer;
                thisCustomerUI.transform.position = Camera.main.WorldToScreenPoint(new Vector3(spawnPos.x, spawnPos.y + _customerUIHeight, spawnPos.z));
                #endregion
                break;
            }
            
            
        }
    }
}
