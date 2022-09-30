using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Funksoft.Barista
{
    public class RegisterDisplay : MonoBehaviour
    {
        private Shift _shift;
        private Canvas _canvas;

        [SerializeField]
        private TextMeshPro _text;

        // Start is called before the first frame update
        void Awake()
        {
            _shift = FindObjectOfType<Shift>();
            _canvas = FindObjectOfType<Canvas>();
        }

        // Update is called once per frame
        void Update()
        {
            _text.text = _shift.ShiftTimer.ToString("F0");
        }
    }
}
