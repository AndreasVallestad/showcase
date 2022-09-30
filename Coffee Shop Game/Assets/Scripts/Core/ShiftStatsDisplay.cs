using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valley;

namespace Funksoft.Barista
{
    public class ShiftStatsDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _dayText;
        [SerializeField]
        private TextMeshProUGUI _completedText;
        [SerializeField]
        private TextMeshProUGUI _failedText;

        private void Start()
        {
            _dayText.text = MonoBehaviourSingleton<PersistentShiftStats>.Instance.CurrentDayIndex.ToString();
            _completedText.text = MonoBehaviourSingleton<PersistentShiftStats>.Instance.CompletedCustomers.ToString() + " satisfied customers";
            _failedText.text = MonoBehaviourSingleton<PersistentShiftStats>.Instance.FailedCustomers.ToString() + " unhappy customers";
        }


    }
}
