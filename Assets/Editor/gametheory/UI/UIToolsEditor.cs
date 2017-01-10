using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace gametheory.UI
{
	public class UIToolsEditor 
	{
		#region Constants
		const string BASE_PATH = "gametheory/UI";
		const string ATTACH_PATH = BASE_PATH + "/Attach";
		const string CONTROL_PATH = BASE_PATH + "/Controls";
		#endregion

		#region Methods
		[MenuItem(BASE_PATH + "/Setup Canvases", false)]
		public static void CreateCanvases()
		{
			GameObject obj = new GameObject("Canvas",typeof(RectTransform),typeof(Canvas),typeof(CanvasScaler),
				typeof(GraphicRaycaster), typeof(OverriddenViewController));

			if(Selection.activeTransform != null)
				obj.transform.parent = Selection.activeTransform;

			Canvas can = obj.GetComponent<Canvas>();
			can.renderMode = RenderMode.ScreenSpaceOverlay;
			can.worldCamera = Camera.main;

			CanvasScaler scaler = obj.GetComponent<CanvasScaler>();
			scaler.matchWidthOrHeight = 0.5f;
			scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

			GameObject alertObj = new GameObject("AlertCanvas",typeof(RectTransform),typeof(Canvas),typeof(CanvasScaler),
				typeof(GraphicRaycaster));

			alertObj.transform.parent = obj.transform.parent;

			Canvas alertCan = alertObj.GetComponent<Canvas>();
			alertCan.renderMode = RenderMode.ScreenSpaceOverlay;
			alertCan.worldCamera = Camera.main;
			alertCan.sortingOrder = can.sortingOrder + 1;

			CanvasScaler alertScaler = obj.GetComponent<CanvasScaler>();
			scaler.matchWidthOrHeight = scaler.matchWidthOrHeight;
			scaler.uiScaleMode = scaler.uiScaleMode;

			UIAlertController alertCon = alertObj.AddComponent<UIAlertController>();
			alertCon.SeparateCanvas = true;
			alertCon.CanvasRect = (alertCan.transform as RectTransform);
		}

		[MenuItem(BASE_PATH + "/View", false)]
		public static void CreateView()
		{
			GameObject obj = new GameObject("UIView",typeof(RectTransform),typeof(CanvasGroup),typeof(UIView));

			if(Selection.activeTransform != null)
				obj.transform.parent = Selection.activeTransform;

			UIView view = obj.GetComponent<UIView>();
			view.CanvasGroup = obj.GetComponent<CanvasGroup>();
		}

		[MenuItem(BASE_PATH + "/Alert", false)]
		public static void CreateAlert()
		{
			GameObject obj = new GameObject("UIAlert",typeof(RectTransform),typeof(CanvasGroup),typeof(UIAlert));

			if(Selection.activeTransform != null)
				obj.transform.parent = Selection.activeTransform;

			UIAlert view = obj.GetComponent<UIAlert>();
			view.CanvasGroup = obj.GetComponent<CanvasGroup>();

			RectTransform rect = view.transform as RectTransform;
			if(obj.transform.parent != null)
			{
				rect.anchoredPosition = Vector2.zero;
				rect.sizeDelta = (obj.transform.parent as RectTransform).sizeDelta;
				rect.localScale = Vector3.one;
			}

			GameObject imgObj = new GameObject("Image",typeof(Image));

			imgObj.transform.parent = obj.transform;

			ExtendedImage img = imgObj.AddComponent<ExtendedImage>();
			img.Image = imgObj.GetComponent<Image>();

			//resize image to fill space
			RectTransform imgRect = imgObj.transform as RectTransform;
			imgRect.sizeDelta = rect.sizeDelta;
			imgRect.anchoredPosition = Vector2.zero;
			imgRect.localScale = Vector3.one;
		}
		#endregion

		#region Control Methods
		[MenuItem(CONTROL_PATH + "/Image", false)]
		public static void CreateImage()
		{
			GameObject obj = new GameObject("Image",typeof(Image));

			if(Selection.activeTransform != null)
				obj.transform.parent = Selection.activeTransform;

			ExtendedImage img = obj.AddComponent<ExtendedImage>();
			img.Image = obj.GetComponent<Image>();
		}

		[MenuItem(CONTROL_PATH + "/RawImage", false)]
		public static void CreateRawImage()
		{
			GameObject obj = new GameObject("RawImage",typeof(RawImage));

			if(Selection.activeTransform != null)
				obj.transform.parent = Selection.activeTransform;

			ExtendedRawImage img = obj.AddComponent<ExtendedRawImage>();
			img.Image = obj.GetComponent<RawImage>();
		}

		[MenuItem(CONTROL_PATH + "/Text", false)]
		public static void CreateText()
		{
			GameObject obj = new GameObject("Text",typeof(Text));

			if(Selection.activeTransform != null)
				obj.transform.parent = Selection.activeTransform;

			ExtendedText text = obj.AddComponent<ExtendedText>();
			text.Label = obj.GetComponent<Text>();
		}

		[MenuItem(CONTROL_PATH +"/Button/Image", false)]
		public static void CreateImageButton()
		{
			GameObject obj = new GameObject("Button",typeof(Image),typeof(Button));

			if(Selection.activeTransform != null)
				obj.transform.parent = Selection.activeTransform;

			ExtendedButton btn = obj.AddComponent<ExtendedButton>();
			btn.Button = obj.GetComponent<Button>();
		}

		[MenuItem(CONTROL_PATH + "/Button/Text", false)]
		public static void CreateTextButton()
		{
			GameObject obj = new GameObject("Button",typeof(Text),typeof(Button));

			if(Selection.activeTransform != null)
				obj.transform.parent = Selection.activeTransform;

			ExtendedButton btn = obj.AddComponent<ExtendedButton>();
			btn.Button = obj.GetComponent<Button>();
		}
		#endregion

		#region Attach Methods
		[MenuItem(ATTACH_PATH + "/Toggle", false)]
		public static void AttachToggle()
		{
			if(Selection.gameObjects.Length > 0)
			{
				GameObject obj = null;
				ExtendedToggle toggle = null;
				for(int index =0; index < Selection.gameObjects.Length; index++)
				{
					obj = Selection.gameObjects[index];
					toggle = obj.AddComponent<ExtendedToggle>();

					toggle.Toggle = obj.GetComponent<Toggle>();
					toggle.Text = obj.GetComponentInChildren<Text>();
				}
			}
			else
				Debug.LogWarning("No objects selected");
		}
		[MenuItem(ATTACH_PATH + "/Dropdown", false)]
		public static void AttachDropdown()
		{
			if(Selection.gameObjects.Length > 0)
			{
				GameObject obj = null;
				ExtendedDropdown dropdown = null;
				for(int index =0; index < Selection.gameObjects.Length; index++)
				{
					obj = Selection.gameObjects[index];
					dropdown = obj.AddComponent<ExtendedDropdown>();

					dropdown.Dropdown = obj.GetComponent<Dropdown>();
				}
			}
			else
				Debug.LogWarning("No objects selected");
		}
		[MenuItem(ATTACH_PATH + "/Inputfield", false)]
		public static void AttachInputfield()
		{
			if(Selection.gameObjects.Length > 0)
			{
				GameObject obj = null;
				ExtendedInputField input = null;
				for(int index =0; index < Selection.gameObjects.Length; index++)
				{
					obj = Selection.gameObjects[index];
					input = obj.AddComponent<ExtendedInputField>();

					input.InputField = obj.GetComponent<InputField>();
				}
			}
			else
				Debug.LogWarning("No objects selected");
		}

		[MenuItem(ATTACH_PATH + "/Scrollbar", false)]
		public static void AttachScrollbar()
		{
			if(Selection.gameObjects.Length > 0)
			{
				GameObject obj = null;
				ExtendedScrollbar bar = null;
				for(int index =0; index < Selection.gameObjects.Length; index++)
				{
					obj = Selection.gameObjects[index];
					bar = obj.AddComponent<ExtendedScrollbar>();

					bar.Slider = obj.GetComponent<Scrollbar>();
					bar.BackgroundImage = obj.GetComponent<Image>();
				}
			}
			else
				Debug.LogWarning("No objects selected");
		}

		[MenuItem(ATTACH_PATH + "/Slider", false)]
		public static void AttachSlider()
		{
			if(Selection.gameObjects.Length > 0)
			{
				int sub =0;
				GameObject obj = null;
				Image img = null;
				Image[] children = null;
				ExtendedSlider slider = null;
				for(int index =0; index < Selection.gameObjects.Length; index++)
				{
					obj = Selection.gameObjects[index];
					slider = obj.AddComponent<ExtendedSlider>();

					slider.Slider = obj.GetComponent<Slider>();
					slider.BackgroundImage = obj.GetComponent<Image>();

					children = obj.GetComponentsInChildren<Image>();
					for(sub = 0; sub < children.Length; sub++)
					{
						img = children[sub];
						if(img.gameObject.name == "Fill")
							slider.FillImage = img;
						else if(img.gameObject.name == "Background")
							slider.BackgroundImage = img;
					}
				}
			}
			else
				Debug.LogWarning("No objects selected");
		}

		[MenuItem(ATTACH_PATH + "/ScrollRect", false)]
		public static void AttachScrollRect()
		{
			if(Selection.gameObjects.Length > 0)
			{
				GameObject obj = null;
				ExtendedScrollRect rect = null;
				for(int index =0; index < Selection.gameObjects.Length; index++)
				{
					obj = Selection.gameObjects[index];
					rect = obj.AddComponent<ExtendedScrollRect>();

					rect.ScrollRect = obj.GetComponent<ScrollRect>();
				}
			}
			else
				Debug.LogWarning("No objects selected");
		}

		[MenuItem(ATTACH_PATH + "/UIList", false)]
		public static void AttachUIList()
		{
			if(Selection.gameObjects.Length > 0)
			{
				GameObject obj = null;
				UIList rect = null;
				for(int index =0; index < Selection.gameObjects.Length; index++)
				{
					obj = Selection.gameObjects[index];
					rect = obj.AddComponent<UIList>();

					rect.Scroll = obj.GetComponent<ScrollRect>();
				}
			}
			else
				Debug.LogWarning("No objects selected");
		}

		[MenuItem(ATTACH_PATH + "/PagingList", false)]
		public static void AttachPaging()
		{
			if(Selection.gameObjects.Length > 0)
			{
				GameObject obj = null;
				PaginingList rect = null;
				for(int index =0; index < Selection.gameObjects.Length; index++)
				{
					obj = Selection.gameObjects[index];
					rect = obj.AddComponent<PaginingList>();

					rect.Scroll = obj.GetComponent<ScrollRect>();
				}
			}
			else
				Debug.LogWarning("No objects selected");
		}
		#endregion
	}
}