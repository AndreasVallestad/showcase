using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Valley;

namespace Funksoft.Barista
{
    public class DaySelectPanel : MonoBehaviour
    {
        [SerializeField]
        private DatabaseSO _database;
        [SerializeField]
        private GameObject _dayButtonPrefab;

        [SerializeField, Header("Button Layout Variables")]
        private int _daysInRow;
        [SerializeField]
        private int _dayButtonSize = 75;
        [SerializeField]
        private int _borderPadding = 50;
        [SerializeField]
        private int _buttonSpacing = 25;
        [SerializeField]
        private int _yRowPadding = 200;

        private List<GameObject> _buttonInstances = new List<GameObject>();

        //Initialization object in Awake due to being modified in OnEnable (Awake -> OnEnable -> Start)
        private void Awake()
        {
            #region Create Day Buttons
            int index = 0;
            foreach(DayData d in _database.Days.HashSet)
            {
                //Create buttons and set the button position in a proper layout, with rows and columns, follwing layout parameters.
                var x = _borderPadding + (index % _daysInRow) * (_dayButtonSize + _buttonSpacing);
                var y = _yRowPadding + (Mathf.CeilToInt(index / _daysInRow) * (_borderPadding + _dayButtonSize));
                var pos = new Vector3(transform.position.x + x, transform.position.y - y, transform.position.z);
                var inst = Instantiate(_dayButtonPrefab, pos, Quaternion.identity, this.transform);
                _buttonInstances.Add(inst);

                //Set button text to match day index (+1 so visual display is not 0 indexed)
                var text = inst.GetComponentInChildren<TextMeshProUGUI>(false);
                text.text = (index+1).ToString();
                //Make button OnClick event call LoadDay function, with the parameter from the button text (-1 adjust for data structures being 0 indexed, but visuals being 1-indexed)
                inst.GetComponent<Button>().onClick.AddListener(() => LoadDay(Int32.Parse(text.text)-1));
                index++;
            }
            #endregion
        }

        public void ToggleWindow()
        {
            if (this.gameObject.activeSelf)
                this.gameObject.SetActive(false);
            else
                this.gameObject.SetActive(true);
        }



        private void LoadDay(int index)
        {
            MonoBehaviourSingleton<SceneLoader>.Instance.LoadShiftScene(index);
        }

    }
}
