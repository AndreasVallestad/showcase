using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;
using TMPro;
using UnityEngine.UI;

namespace Funksoft.Barista
{
    public class HoverText : MonoBehaviour, IEventReceiver<ClickableObject.HoverEvent>
    {

        private float _panelAlpha;

        [SerializeField]
        private TextMeshProUGUI _text;
        [SerializeField]
        private Image _panelImage;

        private void Start()
        {
            _text.text = "";
            _panelAlpha = _panelImage.color.a;
            _panelImage.color = new Color(_panelImage.color.r, _panelImage.color.g, _panelImage.color.b, 0f);
        }

        private void OnEnable()
        {
            EventBus.Register(this);
        }
        private void OnDisable()
        {
            EventBus.UnRegister(this);
        }

        //Receive event from clickableObject with name for display on mouse hover.
        public void OnEvent(ClickableObject.HoverEvent e)
        {
                _text.text = e.objectDisplayName;
                //Show background panel color, if there is hovertext
                if (e.objectDisplayName != "")
                {
                    _panelImage.color = new Color(_panelImage.color.r, _panelImage.color.g, _panelImage.color.b, _panelAlpha);
                    return;
                }
                //Clear background panel cover, if no hovertext is being displayed.
                _panelImage.color = new Color(_panelImage.color.r, _panelImage.color.g, _panelImage.color.b, 0f);
        }
    }
}
