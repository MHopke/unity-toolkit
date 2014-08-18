using UnityEngine;
using System.Collections;

namespace gametheory.UI
{

    /// <summary>
    /// This class is a collection of UI elements. Each UIView
    /// has a position and size. Each GUI element's position & size
    /// are relative to the UIView's position & size.
    /// </summary>
    public class GUIView : UIView
    {
        #region Enums
        protected enum MovementState { INITIAL = 0, IN_PLACE, EXITING, EXITED }
        #endregion

        #region Constants
        const float CLOSE_ENOUGH = 30.0f;
        const float SPEED_MOD = 100.0f;
        #endregion

        #region Public Variables
        //used to determine if useGUILayout should be used
        //views that need to utilize GUI.depth should have this turned on
        public bool _useGUILayout;

        //lower numbers are drawn on top of higher numbers
        public int _depth;

        public Rect _viewRect;
        #endregion

        #region Protected Variables
        protected bool _transitioning;

        protected float _movementRate;

        protected MovementState _movementState;

        protected Vector2 _currentPosition;
        protected Vector2 _startPosition;

        protected Transition _currentTransition;
        #endregion

        #region Unity Methods
        protected void Update()
        {
            if (_movementState == MovementState.INITIAL)
            {
                _movementRate = 1.0f / (_currentPosition - _currentTransition._targetPosition).magnitude;

                SetPosition(Vector2.Lerp(_currentPosition, _currentTransition._targetPosition, _movementRate * _currentTransition._speed * SPEED_MOD * Time.deltaTime));

                if (Mathf.Abs((_currentPosition - _currentTransition._targetPosition).magnitude) <= CLOSE_ENOUGH)
                {
                    SetPosition(_currentTransition._targetPosition);

                    _movementState = MovementState.IN_PLACE;

                    TransitionInEvent();
                }
            }
            else if (_movementState == MovementState.EXITING)
            {
                _movementRate = 1.0f / (_currentPosition - _currentTransition._targetPosition).magnitude;

                SetPosition(Vector2.Lerp(_currentPosition, _currentTransition._targetPosition, _movementRate * _currentTransition._speed * SPEED_MOD * Time.deltaTime));

                if (Mathf.Abs((_currentPosition - _currentTransition._targetPosition).magnitude) <= CLOSE_ENOUGH)
                {
                    SetPosition(_currentTransition._targetPosition);

                    _movementState = MovementState.EXITED;

                    TransitionOutEvent();

                    Deactivate();
                }
            }

            OnUpdate();
        }

        protected virtual void OnUpdate() { }

        void OnGUI()
        {
            GUI.depth = _depth;

            if (UINavigationController.Skin)
                GUI.skin = UINavigationController.Skin;

            GUI.BeginGroup(_viewRect);
            DrawContent();
            GUI.EndGroup();
        }

        /// <summary>
        /// Draws the content with in the view.
        /// </summary>
        protected virtual void DrawContent()
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                if (_elements[i] && _elements[i].Active)
                {
                    (_elements[i] as GUIBase).Draw();
                }
            }
        }

        #endregion

        #region Activation, Deactivation Methods
        protected override void Initialize()
        {
            base.Initialize();

            if (!_useGUILayout)
                useGUILayout = false;

            UIScreen.AdjustRect(ref _viewRect, _screenSetting);

            _startPosition.x = _viewRect.x;
            _startPosition.y = _viewRect.y;

            _currentPosition = _startPosition;

            if (_elements != null)
            {
                for (int i = 0; i < _elements.Count; i++)
                {
                    if (_elements[i])
                        _elements[i].Init();
                }
            }
        }

        public void Activate(Transition transition)
        {
            if (enabled) return;

            Activation();

            _currentTransition = transition;
            _currentTransition._targetPosition.Scale(UIScreen.AspectRatio);
        }

        /// <summary>
        /// Overloadable method which handles the actual activation of UI elements
        /// </summary>
        protected override void Activation()
        {
            base.Activation();

            _currentTransition = new Transition(_currentPosition, 1f);

            SetPosition(_startPosition);

            _movementState = MovementState.INITIAL;

#if LOG
    		Debug.Log(name + " activated.");
#endif
        }
        #endregion

        #region Position Methods
        /// <summary>
        /// Reposition the UI elements according to the newPosition.
        /// </summary>
        /// <param name="newPosition"></param>
        /// <param name="animate">Determines if the UIView animates.</param>
        public void Reposition(Vector2 newPosition, bool animate = false)
        {
            Vector2 scale = new Vector2(newPosition.x / _viewRect.x, newPosition.y / _viewRect.y);

            if (_elements != null)
            {
                for (int i = 0; i < _elements.Count; i++)
                {
                    if (_elements[i])
                        (_elements[i] as GUIBase).Reposition(scale);
                }
            }
        }

        public bool IsUIInPlace()
        {
            if (_elements != null)
            {
                for (int i = 0; i < _elements.Count; i++)
                {
                    if (_elements[i] && !_elements[i].InPlace)
                        return false;
                }
            }

            return true;
        }
        protected virtual void InPlace()
        {
            enabled = false;

            _movementState = MovementState.IN_PLACE;

            TransitionInEvent();
        }

        void SetPosition(Vector2 position)
        {
            _currentPosition.x = position.x;
            _currentPosition.y = position.y;

            _viewRect.x = position.x;
            _viewRect.y = position.y;
        }
        #endregion

        #region Exit Methods
        public override void Exit()
        {
            _currentTransition = new Transition(_currentPosition, 1f);

            base.Exit();

            _movementState = MovementState.EXITING;
        }

        public void FlagForExit(Transition transition)
        {
            _currentTransition = transition;

            Exit();
        }

        bool HasUIExited()
        {
            if (_elements != null)
            {
                for (int i = 0; i < _elements.Count; i++)
                {
                    if (_elements[i] && !_elements[i].HasExited)
#if LOG
    					{
    						Debug.Log(_elements[i].name);
    						return false;
    					}
#else
                        return false;
#endif
                }
            }

            return true;
        }
        #endregion
    }
}
