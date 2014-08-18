using UnityEngine;

namespace gametheory.UI
{
    /// <summary>
    /// Representation of a graphical UI element. Uses Unity 4.3 Sprite System.
    /// </summary>
    public class UISprite : UIBase
    {
    	#region Public Variables
    	public bool _hasRoot;

    	public float _zDepth = 3;

    	public SpriteRenderer _spriteRenderer;

    	public Animator _spriteAnimator;
    	#endregion

    	#region Activation, Deactivation, Init Methods
    	protected override void OnInit()
    	{
    		if(!_spriteRenderer)
    			_spriteRenderer = GetComponent<SpriteRenderer>();

    		if(!_spriteAnimator)
    			_spriteAnimator = GetComponent<Animator>();

    		_spriteRenderer.enabled = false;

    		base.OnInit();
    	}
    	protected override void OnActivate(MovementState moveState)
    	{
    		base.OnActivate(moveState);

    		_spriteRenderer.enabled = true;
    	}
    	protected override void OnDeactivate()
    	{
    		base.OnDeactivate();

    		_spriteRenderer.enabled = false;
    	}
    	#endregion

    	#region Type Methods
    	public override System.Type GetBaseType()
    	{
    		return typeof(UISprite);
    	}
    	#endregion

    	#region Color Methods
    	protected Color GetColor()
    	{
    		return _spriteRenderer.color;
    	}
    	protected void SetColor(Color color)
    	{
    		_spriteRenderer.color = color;
    	}
    	#endregion

    	#region Animation Methods
    	public void SetTrigger(string triggerName)
    	{
    		if(_spriteAnimator && _spriteAnimator.runtimeAnimatorController)
    		{
    			//Debug.Log(triggerName);
    			_spriteAnimator.SetTrigger(triggerName);
    			//enabled = true;
    		} else
    			_spriteRenderer.enabled = true;
    	}
    	public void SetBool(string name, bool value)
    	{
    		if(_spriteAnimator && _spriteAnimator.runtimeAnimatorController)
    		{
    			_spriteAnimator.SetBool(name,value);
    			//enabled = true;
    		}
    	}
    	#endregion

    	#region Accessors
    	public Sprite CurrentSprite
    	{
    		get { return _spriteRenderer.sprite; }
    		set { _spriteRenderer.sprite = value; }
    	}
    	public SpriteRenderer Renderer
    	{
    		get { return _spriteRenderer; }
    		set { _spriteRenderer = value; }
    	}
    	public Animator SpriteAnimator
    	{
    		get { return _spriteAnimator; }
    		set { _spriteAnimator = value; }
    	}
    	#endregion
    }
}