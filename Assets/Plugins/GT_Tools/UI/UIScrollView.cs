using UnityEngine;
using System.Collections;

namespace gametheory.UI
{
    /// <summary>
    /// Representation of a scroll view.
    /// </summary>
    public class UIScrollView : UIView {

    	#region Enumerations
    	public enum ScrollType { HORIZONTAL = 0, VERTICAL, BOTH }
    	#endregion

    	#region Constants
    	const float BASE_SCROLL_VELOCITY = 1.0f;
    	const float BASE_SCROLL_VELOCITY_DECAY = 40.0f;
    	const float BASE_MIN_SCROLL_MOVEMENT_FOR_VELOCITY = 5.0f;
    	#endregion

    	#region Public Variables
    	public GUITexture _background;

    	public ScrollType _type;

    	public Rect _scrollAreaRect; //The original view position
    	#endregion

    	#region Private Vars
    	bool _eventsAdded;
    	bool _scrollActivated;

    	float _scrollVelocity;
    	float _velocity;
    	float _scrollVelocityDecay;
    	float _lastDelta;
    	#endregion

    	#region Unity Methods
    	void Start()
    	{
    		_velocity = 0.0f;
    	}
    	#endregion

    	#region Overriden Method
    	protected override void Initialize()
    	{
    		if(_background)
    			_background.Init();

    		_scrollAreaRect.Scale(UIScreen.AspectRatio);

    		if(_type == ScrollType.VERTICAL)
    		{
    			_scrollVelocity = BASE_SCROLL_VELOCITY * UIScreen.AspectRatio.y;
    			_scrollVelocityDecay = BASE_SCROLL_VELOCITY_DECAY * UIScreen.AspectRatio.y;
    		}
    		else if (_type == ScrollType.HORIZONTAL)
    		{
    			_scrollVelocity = BASE_SCROLL_VELOCITY * UIScreen.AspectRatio.x;
    			_scrollVelocityDecay = BASE_SCROLL_VELOCITY_DECAY * UIScreen.AspectRatio.x;
    		}
    		_velocity = 0.0f;

    		base.Initialize();
    	}
    		
    	protected override void Activation()
    	{
    		if(_background)
    			_background.Activate();

    		CheckHorizontalBorders();
    		CheckVerticalBorders();

    		AddEvents();

    		_scrollActivated = true;

    		base.Activation();
    	}

    	protected override void Deactivation()
    	{
    		if(_background)
    			_background.Deactivate();

    		RemoveEvents();

    		_scrollActivated = false;

    		base.Deactivation();
    	}

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (_scrollActivated) 
            {
                if (_velocity != 0.0f) 
                {
                    if(_type == ScrollType.VERTICAL)
                    {
                        _scrollAreaRect.y -= _velocity;
                        CheckVerticalBorders();
                    }
                    else if (_type == ScrollType.HORIZONTAL)
                    {
                        _scrollAreaRect.x -= _velocity;
                        CheckHorizontalBorders();
                    }

                    if (_velocity > 0.0f) {
                        _velocity -= Time.deltaTime * _scrollVelocityDecay;
                        if (_velocity < 0.0f)
                            _velocity = 0.0f;
                    }
                    else {
                        _velocity += Time.deltaTime * _scrollVelocityDecay;
                        if (_velocity > 0.0f)
                            _velocity = 0.0f;
                    }
                }
            }
        }

    	protected override void DrawContent()
    	{
    		if(_background)
    			_background.Draw();

    		GUI.BeginGroup(_scrollAreaRect);
    		base.DrawContent();
    		GUI.EndGroup();
    	}

    	protected override void InPlace()
    	{
    		for(int i = 0; i < _elements.Count; i++)
    		{
    			if(_elements[i] != null)
    				_elements[i].SetToPosition();
    		}

    		base.InPlace();
    	}


    	public override void LostFocus()
    	{
    		InputHandler.RemoveInputMoving(TouchMoving);

    		base.LostFocus();
    	}
    	public override void GainedFocus()
    	{
    		InputHandler.AddInputMoving(TouchMoving);

    		base.GainedFocus();
    	}

    	public override void Exit()
    	{
    		base.Exit();

    		if(_background)
    			_background.Exit();
    	}
    	#endregion

    	#region Methods
    	void CheckHorizontalBorders()
    	{
    		if(_scrollAreaRect.x <= -_scrollAreaRect.width)
    			_scrollAreaRect.x = -_scrollAreaRect.width;
    		else if(_scrollAreaRect.x >= 0.0f)
    			_scrollAreaRect.x = 0.0f;
    	}

    	void CheckVerticalBorders()
    	{
    		if(_scrollAreaRect.y <= -(_scrollAreaRect.height - _viewRect.height))
    			_scrollAreaRect.y = -(_scrollAreaRect.height - _viewRect.height);
    		else if(_scrollAreaRect.y >= 0.0f)
    			_scrollAreaRect.y = 0.0f;
    	}

    	void AddEvents()
    	{
    		if (_eventsAdded)
    			return;

    		InputHandler.AddInputStart(TouchStart);
    		InputHandler.AddInputMoving(TouchMoving);
    		InputHandler.AddInputEnd(TouchEnd);
    		_eventsAdded = true;
    	}

    	void RemoveEvents()
    	{
    		if (!_eventsAdded)
    			return;

    		InputHandler.RemoveInputStart(TouchStart);
    		InputHandler.RemoveInputMoving(TouchMoving);
    		InputHandler.RemoveInputEnd(TouchEnd);
    		_eventsAdded = false;
    	}
    	#endregion

    	#region Touch Events
    	void TouchStart(Vector2 position, int id)
    	{
    		if (_scrollActivated)
    			_velocity = 0.0f;
    	}

    	void TouchMoving(Vector2 pos, Vector2 delta, int id)
    	{
    		if (_scrollActivated) {
    			if (_movementState == MovementState.IN_PLACE) {
    				//Adjust movement for each type
    				if (_type == ScrollType.HORIZONTAL) {
    					_scrollAreaRect.x -= delta.x;

    					_lastDelta = delta.x;

    					CheckHorizontalBorders();
    				}
    				else if (_type == ScrollType.VERTICAL) {
    					_scrollAreaRect.y -= delta.y;

    					_lastDelta = delta.y;

    					CheckVerticalBorders();

    				}
    				else {
    					_scrollAreaRect.x += delta.x;
    					_scrollAreaRect.y += delta.y;

    					CheckHorizontalBorders();
    					CheckVerticalBorders();
    				}
    			}
    		}
    	}

    	void TouchEnd(Vector2 position, int id)
    	{
    		if(_scrollActivated)
    		{
    			_velocity = _lastDelta * _scrollVelocity;
    		}
    	}
    	#endregion
    }
}