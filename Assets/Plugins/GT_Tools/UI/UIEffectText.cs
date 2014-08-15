using UnityEngine;
using System.Collections;

namespace gametheory.UI
{
    public class UIEffectText : UIBase 
    {
    	#region Public Methods
    	public EffectManager _effect;
    	public float _zDepth;
    	#endregion

    	#region Overridden Methods
    	protected override void OnInit()
    	{
    		base.OnInit();

    		if(!_effect)
    			_effect = GetComponent<EffectManager>();
    	}
    	protected override void OnActivate(MovementState moveState)
    	{
    		base.OnActivate(moveState);

    		_effect.SetText(_effect.Text, true);
    	}
    	protected override void OnDeactivate()
    	{
    		base.OnDeactivate();
    	}
    	#endregion

    	#region Methods
    	public void PlayEffect()
    	{
    		if(_effect)
    			_effect.PlayAnimation();
    	}
    	#endregion

    	#region Accessors
    	public string Text
    	{
    		get { return _effect.Text; }
    		set { _effect.SetText(value,true); }
    	}
    	#endregion
    }
}
