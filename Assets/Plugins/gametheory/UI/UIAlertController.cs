using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace gametheory.UI
{
    public class UIAlertController : MonoBehaviour 
    {
        #region Public Vars
        //public Image _alpha;
        //public CanvasGroup _alphaGroup;

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
                //_alpha.enabled = true;
                //_alphaGroup.blocksRaycasts = true;
                UIViewController.LoseFocus();
            }
            else
                _alertStack.Peek().Hide();

            if(!alert._skipStack)
                _alertStack.Push(alert);
        }
        public void RemoveAlert()
        {
            if(_alertStack.Count > 0)
                _alertStack.Pop();

            if(_alertStack.Count == 0)
            {
                //_alpha.enabled = false;
                //_alphaGroup.blocksRaycasts = false;
                UIViewController.GainFocus();
            }
            else
                _alertStack.Peek().UnHide();
        }
        #endregion
    }
}
