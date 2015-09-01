using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace gametheory.UI
{
    public class UIAlertController : MonoBehaviour 
    {
        #region Public Vars
        public static UIAlertController Instance = null;
        #endregion

        #region Private Vars
        Stack<UIView> _alertStack;
        #endregion

        #region Unity Methods
        void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                _alertStack = new Stack<UIView>();
            }
            else
                Destroy(gameObject);
        }
        #endregion

        #region Methods
        public void PresentAlert(UIView alert)
        {
            if(_alertStack.Count == 0)
            {
                UIViewController.LoseFocus();
            }
            else
                _alertStack.Peek().Hide();
            
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
        void HookUpClose(UIView alert)
        {
            alert.transitionOutEvent += RemoveAlert;
            alert.Activate();
        }
        #endregion
    }
}
