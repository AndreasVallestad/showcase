using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;

namespace Funksoft.Barista
{
    public class SwitchScreenButton : MonoBehaviour, IEventReceiver<MoveCamera.CamMoveStarted>, IEventReceiver<MoveCamera.CamMoveFinished>
    {
        //Which screen this switch screen button is on.
        [SerializeField]
        private MoveCamera.Screen _buttonScreen;

        private RectTransform rectTransform;

        private void Awake()
        {
            TryGetComponent<RectTransform>(out rectTransform);
        }

        private void OnEnable()
        {
            EventBus.Register(this);
        }
        private void OnDisable()
        {
            EventBus.UnRegister(this);
        }

        public void OnEvent(MoveCamera.CamMoveStarted e)
        {
            rectTransform.localScale = new Vector3(0, 0, 0);
        }

        public void OnEvent(MoveCamera.CamMoveFinished e)
        {
            if (e.screenMovedTo == _buttonScreen)
            {
                rectTransform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
