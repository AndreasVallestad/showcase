using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ProtoRogue{
    public class PlayerBodyDisplay : MonoBehaviour
    {

        [SerializeField]
        private Body playerBody;

        private TextMeshProUGUI text;

        private void Awake()
        {
            TryGetComponent<TextMeshProUGUI>(out text);
        }

        private void Start()
        {
            UpdateText();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
                UpdateText();

        }

        private void UpdateText()
        {
            text.text = "";
            foreach(BodyPart part in playerBody.mainBody.subBodyParts)
            {
                text.text += (part.partName + " - " + part.Health + " : " + part.status.ToString() + "\n");
            }
        }

        
    }
}
