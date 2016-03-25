using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace gametheory.UI
{
    public class UIAlertController : MonoBehaviour 
    {
        #region Public Vars
		[Tooltip("Whether or not alerts are in a separate Canvas. " +
			"Used to determine if the gameobject/canvas can be turned off/on.")]
		public bool SeparateCanvas;
		public RectTransform CanvasRect;
        public static UIAlertController Instance = null;
        #endregion

        #region Private Vars
		Stack<UIAlert> _alertStack;
        #endregion

        #region Unity Methods
        void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                _alertStack = new Stack<UIAlert>();

				if(SeparateCanvas)
					gameObject.SetActive(false);
            }
            else
                Destroy(gameObject);
        }
        #endregion

        #region Methods
        public void PresentAlert(UIAlert alert)
        {
            if(_alertStack.Count == 0)
            {
                UIViewController.LoseFocus();

				if(SeparateCanvas)
					gameObject.SetActive(true);
            }
            else
                _alertStack.Peek().Hide();
            
			alert.SetToFront();

            HookUpClose(alert);

            _alertStack.Push(alert);
        }
        public void RemoveAlert()
        {
            if(_alertStack.Count > 0)
            {
                _alertStack.Peek().transitionOutEvent -= RemoveAlert;
                _alertStack.Pop();
            }

            //Debug.Log("removed alert. Now count is: " + _alertStack.Count);

            if(_alertStack.Count == 0)
            {
				if(SeparateCanvas)
					gameObject.SetActive(false);
                UIViewController.GainFocus();
            }
            else
                _alertStack.Peek().Show();
        }
        public void ClearStack()
        {
            while(_alertStack.Count > 0)
            {
                _alertStack.Peek().transitionOutEvent -= RemoveAlert;
                _alertStack.Peek().Deactivate();
                _alertStack.Pop();
            }

            UIViewController.GainFocus();
        }
        void HookUpClose(UIAlert alert)
        {
            alert.transitionOutEvent += RemoveAlert;
            alert.Activate();
        }
        #endregion
    }
}
