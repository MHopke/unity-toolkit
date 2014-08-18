using UnityEngine;

namespace gametheory.UI
{
    public class UIFadeTransition : MonoBehaviour 
    {
    	#region Events
    	public static event System.Action transitionInFinished;
    	public static event System.Action transitionOutFinished;
    	#endregion

    	#region Enumerations
    	enum FadeState { NONE = 0, IN, OUT }
    	#endregion

    	#region Public Variables
    	public float _fadeOutTime;
    	public float _fadeInTime;
    	#endregion

    	#region Private Variables
    	Color _originalColor;
    	Color _color;

    	Texture2D _texture;

    	Rect _rect;

    	FadeState _fadeState;
    	#endregion

    	#region Unity Methods
    	void Start()
    	{
    		_originalColor = new Color(0.925f, 0.941f, 0.945f,0.0f);
    		_texture = new Texture2D(1, 1);
    		_texture.SetPixel(0, 0, _originalColor);
    		_texture.Apply();

    		//_rect = AutoSized.CreateRect(0, 0, 640, 960);

    		_fadeState = FadeState.NONE;

    		enabled = false;
    	}

    	// Update is called once per frame
    	void Update () 
    	{
    		if(_fadeState == FadeState.IN)
    		{
    			_color = _texture.GetPixel(0, 0);
    			_color.a += Time.deltaTime * (1.0f / _fadeInTime);
    			_texture.SetPixel(0, 0, _color);
    			_texture.Apply();

    			if(_texture.GetPixel(0,0).a >= 0.98f)
    			{
    				_color.a = 1.0f;
    				_texture.SetPixel(0,0, _color);
    				_texture.Apply();

    				_fadeState = FadeState.OUT;

    				if(transitionInFinished != null)
    					transitionInFinished();
    			}
    		} else if(_fadeState == FadeState.OUT)
    		{
    			_color = _texture.GetPixel(0, 0);
    			_color.a -= Time.deltaTime * (1.0f / _fadeOutTime);
    			_texture.SetPixel(0, 0, _color);
    			_texture.Apply();

    			if(_texture.GetPixel(0,0).a <= 0.0f)
    			{
    				//Debug.Log("faded in");

    				_color.a = 0.0f;
    				_texture.SetPixel(0,0, _color);
    				_texture.Apply();

    				_fadeState = FadeState.NONE;

    				enabled = false;

    				if(transitionOutFinished != null)
    					transitionOutFinished();
    			}
    		}
    	}

    	void OnGUI()
    	{
    		GUI.depth = -3;
    		GUI.DrawTexture(_rect, _texture);
    	}
    	#endregion

    	#region Other Methods
    	public void Transition()
    	{
    		if(enabled || _fadeState != FadeState.NONE)
    			return;

    		_texture.SetPixel(0, 0, _originalColor);
    		_texture.Apply();

    		_fadeState = FadeState.IN;
    		enabled = true;
    	}
    	#endregion
    }
}