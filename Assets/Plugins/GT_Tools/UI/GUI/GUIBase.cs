using UnityEngine;
using System.Collections;

namespace gametheory.UI
{
    public class GUIBase : UIBase 
    {
        #region Constants
        protected const float CLOSE_ENOUGH = 3.0f; //Used when checking transition positions
        protected const float SMOOTH_FACTOR = 25.0f; //Applied to transition movement
        #endregion

        #region Public Variables
        //Rectangle to draw the element. (in pixels)
        public Rect _drawRect;

        //The customStyle to apply to the base component of any UI element.
        //If an element uses multiple styles you will need to add more.
        public CustomStyle _primaryStyle;
        #endregion

        #region Protected Variables
        protected float _movementRate;

        protected Vector2 _startPosition;
        protected Vector2 _currentPosition;
        protected Vector2 _targetPosition;
        #endregion

        #region Overriden Methods
        protected override void OnInit()
        {
            UIScreen.AdjustRect(ref _drawRect, _screenSetting);

            _startPosition = new Vector2(_drawRect.x,_drawRect.y);

            _currentPosition = _startPosition;

            _primaryStyle.style.contentOffset.Scale(UIScreen.AspectRatio);
        }

        protected override void OnActivate(MovementState moveState)
        {
            base.OnActivate(moveState);

            if(moveState == MovementState.INITIAL)
                SetStartPosition();
            else if(moveState == MovementState.IN_PLACE)
                SetToPosition();
        }
        #endregion

        #region Draw Methods
        public virtual void Draw(){}
        #endregion

        #region Position Methods
        /// <summary>
        /// Sets the position. Designed to be overriden such as in UISprite.
        /// </summary>
        /// <param name="position">Position.</param>
        protected virtual void SetPosition(Vector2 position)
        {
            _currentPosition = position;
            _drawRect.x = position.x;
            _drawRect.y = position.y;
        }
        public void SetStartPosition()
        {
            SetPosition(_startPosition);
        }
        public void Reposition(Vector2 scale, bool animate=false)
        {
            if (animate)
            {
                _targetPosition = new Vector2(_currentPosition.x * scale.x, _currentPosition.y * scale.y);

                _movementState = MovementState.MOVING;

                enabled = true;
            }
            else
            {
                SetPosition(new Vector2(_currentPosition.x * scale.x, _currentPosition.y * scale.y));
            }
        }
        public void SetToPosition()
        {
            SetPosition(new Vector2(_drawRect.x,_drawRect.y));
        }
        public void SetX(float x)
        {
            _currentPosition.x = x;
            SetPosition(_currentPosition);
        }
        public void SetY(float y)
        {
            _currentPosition.y = y;
            SetPosition(_currentPosition);
        }
        public void SetStartX(float x)
        {
            _startPosition.x = x;
        }

        public void SetStartY(float y)
        {
            _startPosition.y = y;
        }
        #endregion

        #region Size Methods
        public void Resize(Vector2 scale, bool animate = false)
        {
            if (animate)
            {

            }
            else
            {
                transform.Scale(scale.x,scale.y,1);
            }
        }
        #endregion

        #region Type Methods
        /// <summary>
        /// Determines the base type.
        /// </summary>
        /// <returns>The base type.</returns>
        public override System.Type GetBaseType()
        {
            return typeof(GUIBase);
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Gets the size. UIBase doesn't actually have a size, but inherited might.
        /// </summary>
        protected virtual void GetSize(){}

        /// <summary>
        /// Gets the bounding area of the element (in pixels).
        /// </summary>
        /// <returns>The bounds.</returns>
        public virtual Rect GetBounds(){return _drawRect;}

        public Vector2 CurrentPosition
        {
            get { return _currentPosition; }
            set { SetPosition(value); }
        }
        public Vector2 StartPosition
        {
            get { return _startPosition; }
        }
        #endregion
    }
}