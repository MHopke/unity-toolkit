using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace gametheory.UI
{
    public class UIAlertController : MonoBehaviour 
    {
		#region Events
		public static event System.Action alertPresented;
		public static event System.Action alertRemoved;
		#endregion

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

        void OnDestroy()
        {
            UIView view = null;
            while(_alertStack.Count > 0)
            {
                view = _alertStack.Pop();
                if (view != null && !view.gameObject.activeSelf)
                {
                    view.CleanUp();
                }
            }
        }
        #endregion

        #region Methods
        public bool AlertsOpen()
		{
			return _alertStack.Count > 0;
		}
        public void PresentAlert(UIAlert alert)
        {
            if(_alertStack.Count == 0)
            {
                UIViewController.LoseFocus();

				if(SeparateCanvas)
					gameObject.SetActive(true);

				PresentEvent();
            }
            else
                _alertStack.Peek().Hide();
            
			alert.SetToFront();

			_alertStack.Push(alert);

            HookUpClose(alert);
        }
        public void RemoveAlert()
        {
            if(_alertStack.Count > 0)
            {
                _alertStack.Peek().transitionOutEvent -= RemoveAlert;
				string name = _alertStack.Pop().name;
				//Debug.Log("removed " + name +" Now count is: " + _alertStack.Count);
            }

           /* if(_alertStack.Count > 0)
                Debug.Log(_alertStack.Peek().name);*/

            if(_alertStack.Count == 0)
            {
				if(SeparateCanvas)
					gameObject.SetActive(false);
                UIViewController.GainFocus();

				RemoveEvent();
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

			RemoveEvent();
        }

		void PresentEvent()
		{
			if(alertPresented != null)
				alertPresented();
		}

		void RemoveEvent()
		{
			if(alertRemoved != null)
				alertRemoved();
		}
        void HookUpClose(UIAlert alert)
        {
            alert.transitionOutEvent += RemoveAlert;
            alert.Activate();
			//alert.Activated();
        }
        #endregion
    }
}
