﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class WMG_Ring_Graph : WMG_Graph_Manager {

	[SerializeField] private List<Color> _bandColors;
	public WMG_List<Color> bandColors = new WMG_List<Color>();
	[SerializeField] private List<float> _values;
	public WMG_List<float> values = new WMG_List<float>();
	[SerializeField] private List<string> _labels;
	public WMG_List<string> labels = new WMG_List<string>();
	[SerializeField] private List<string> _ringIDs;
	public WMG_List<string> ringIDs = new WMG_List<string>();

	// public properties
	public bool bandMode { get {return _bandMode;} 
		set {
			if (_bandMode != value) {
				_bandMode = value;
				textureC.Changed();
			}
		}
	}
	public float innerRadiusPercentage { get {return _innerRadiusPercentage;} 
		set {
			if (_innerRadiusPercentage != value) {
				_innerRadiusPercentage = value;
				textureC.Changed();
			}
		}
	}
	public float degrees { get {return _degrees;} 
		set {
			if (_degrees != value) {
				_degrees = value;
				degreesC.Changed();
			}
		}
	}
	public float minValue { get {return _minValue;} 
		set {
			if (_minValue != value) {
				_minValue = value;
				degreesC.Changed();
			}
		}
	}
	public float maxValue { get {return _maxValue;} 
		set {
			if (_maxValue != value) {
				_maxValue = value;
				degreesC.Changed();
			}
		}
	}
	public Color bandColor { get {return _bandColor;} 
		set {
			if (_bandColor != value) {
				_bandColor = value;
				bandColorC.Changed();
			}
		}
	}
	public bool autoUpdateBandAlpha { get {return _autoUpdateBandAlpha;} 
		set {
			if (_autoUpdateBandAlpha != value) {
				_autoUpdateBandAlpha = value;
				bandColorC.Changed();
			}
		}
	}
	public Color ringColor { get {return _ringColor;} 
		set {
			if (_ringColor != value) {
				_ringColor = value;
				ringColorC.Changed();
			}
		}
	}
	public float ringWidth { get {return _ringWidth;} 
		set {
			if (_ringWidth != value) {
				_ringWidth = value;
				textureC.Changed();
			}
		}
	}
	public float ringPointWidthFactor { get {return _ringPointWidthFactor;} 
		set {
			if (_ringPointWidthFactor != value) {
				_ringPointWidthFactor = value;
				textureC.Changed();
			}
		}
	}
	public float bandPadding { get {return _bandPadding;} 
		set {
			if (_bandPadding != value) {
				_bandPadding = value;
				textureC.Changed();
			}
		}
	}
	public float labelLinePadding { get {return _labelLinePadding;} 
		set {
			if (_labelLinePadding != value) {
				_labelLinePadding = value;
				radiusC.Changed();
			}
		}
	}
	public Vector2 leftRightPadding { get {return _leftRightPadding;} 
		set {
			if (_leftRightPadding != value) {
				_leftRightPadding = value;
				radiusC.Changed();
			}
		}
	}
	public Vector2 topBotPadding { get {return _topBotPadding;} 
		set {
			if (_topBotPadding != value) {
				_topBotPadding = value;
				radiusC.Changed();
			}
		}
	}
	public bool antiAliasing { get {return _antiAliasing;} 
		set {
			if (_antiAliasing != value) {
				_antiAliasing = value;
				textureC.Changed();
			}
		}
	}
	public float antiAliasingStrength { get {return _antiAliasingStrength;} 
		set {
			if (_antiAliasingStrength != value) {
				_antiAliasingStrength = value;
				textureC.Changed();
			}
		}
	}

	// Public variables without change tracking
	public bool animateData;
	public float animDuration;
	public Ease animEaseType;
	public Object ringPrefab;
	public GameObject extraRing;
	public GameObject background;
	public GameObject zeroLine;
	public GameObject zeroLineText;
	public GameObject ringsParent;
	public GameObject ringLabelsParent;
	public GameObject contentParent;
	public WMG_Data_Source valuesDataSource;
	public WMG_Data_Source labelsDataSource;
	public WMG_Data_Source ringIDsDataSource;

	// Private backing variables
	[SerializeField] private bool _bandMode;
	[SerializeField] private float _innerRadiusPercentage;
	[SerializeField] private float _degrees;
	[SerializeField] private float _minValue;
	[SerializeField] private float _maxValue;
	[SerializeField] private Color _bandColor;
	[SerializeField] private bool _autoUpdateBandAlpha;
	[SerializeField] private Color _ringColor;
	[SerializeField] private float _ringWidth;
	[SerializeField] private float _ringPointWidthFactor;
	[SerializeField] private float _bandPadding;
	[SerializeField] private float _labelLinePadding;
	[SerializeField] private Vector2 _leftRightPadding;
	[SerializeField] private Vector2 _topBotPadding;
	[SerializeField] private bool _antiAliasing;
	[SerializeField] private float _antiAliasingStrength;

	public float outerRadius { 
		get {
			return Mathf.Min ((getSpriteWidth (this.gameObject) - leftRightPadding.x - leftRightPadding.y) / 2,
			                  (getSpriteHeight (this.gameObject) - topBotPadding.x - topBotPadding.y) / 2); 
		}
	}

	// Original property values for use with dynamic resizing
	private float origGraphWidth;
	public float RingWidthFactor { get { return ((1 - innerRadiusPercentage) * outerRadius) / origGraphWidth; } }

	// cache
	private float containerWidthCached;
	private float containerHeightCached;

	// public getter
	public List<WMG_Ring> rings { get; private set; }

	// private
	private Sprite extraRingSprite;
	private Color[] extraRingColors;
	private int ringTexSize;

	private List<WMG_Change_Obj> changeObjs = new List<WMG_Change_Obj>();
	private WMG_Change_Obj numberRingsC = new WMG_Change_Obj();
	private WMG_Change_Obj bandColorC = new WMG_Change_Obj();
	private WMG_Change_Obj ringColorC = new WMG_Change_Obj();
	private WMG_Change_Obj labelsC = new WMG_Change_Obj();
	private WMG_Change_Obj degreesC = new WMG_Change_Obj();
	private WMG_Change_Obj radiusC = new WMG_Change_Obj();
	private WMG_Change_Obj textureC = new WMG_Change_Obj();

	private bool hasInit;

	void Start() {
		Init ();
		PauseCallbacks();
		AllChanged();
	}

	public void Init() {
		if (hasInit) return;
		hasInit = true;
		
		changeObjs.Add(numberRingsC);
		changeObjs.Add(textureC);
		changeObjs.Add(degreesC);
		changeObjs.Add(ringColorC);
		changeObjs.Add(bandColorC);
		changeObjs.Add(radiusC);
		changeObjs.Add(labelsC);
		
		extraRingSprite = WMG_Util.createSprite(getTexture(extraRing));
		ringTexSize = extraRingSprite.texture.width;
		extraRingColors = new Color[ringTexSize * ringTexSize];
		setTexture(extraRing, extraRingSprite);
		rings = new List<WMG_Ring>();
		origGraphWidth = ((1 - innerRadiusPercentage) * outerRadius);
		
		bandColors.SetList (_bandColors);
		bandColors.Changed += bandColorsChanged;
		
		values.SetList (_values);
		values.Changed += valuesChanged;
		
		labels.SetList (_labels);
		labels.Changed += labelsChanged;
		
		ringIDs.SetList (_ringIDs);
		ringIDs.Changed += ringIDsChanged;
		
		numberRingsC.OnChange += NumberRingsChanged;
		bandColorC.OnChange += BandColorChanged;
		ringColorC.OnChange += RingColorChanged;
		labelsC.OnChange += LabelsChanged;
		degreesC.OnChange += DegreesChanged;
		radiusC.OnChange += RadiusChanged;
		textureC.OnChange += TextureChanged;
		
		PauseCallbacks();
	}

	void Update() {
		updateFromDataSource();
		updateFromResize();
		
		Refresh();
	}

	public void Refresh() {
		ResumeCallbacks();
		PauseCallbacks();
	}
	
	void PauseCallbacks() {
		for (int i = 0; i < changeObjs.Count; i++) {
			changeObjs[i].changesPaused = true;
			changeObjs[i].changePaused = false;
		}
	}
	
	void ResumeCallbacks() {
		for (int i = 0; i < changeObjs.Count; i++) {
			changeObjs[i].changesPaused = false;
			if (changeObjs[i].changePaused) changeObjs[i].Changed();
		}
	}

	void updateFromResize() {
		bool resizeChanged = false;
		updateCacheAndFlag<float>(ref containerWidthCached, getSpriteWidth(this.gameObject), ref resizeChanged);
		updateCacheAndFlag<float>(ref containerHeightCached, getSpriteHeight(this.gameObject), ref resizeChanged);
		if (resizeChanged) {
			radiusC.Changed();
		}
	}

	void updateFromDataSource() {
		if (valuesDataSource != null) {
			values.SetList(valuesDataSource.getData<float>());
		}
		if (labelsDataSource != null) {
			labels.SetList(labelsDataSource.getData<string>());
		}
		if (ringIDsDataSource != null) {
			ringIDs.SetList(ringIDsDataSource.getData<string>());
		}
	}

	void NumberRingsChanged() {
		updateNumberRings();
	}

	void TextureChanged() {
		updateRingsAndBands();
	}

	void DegreesChanged() {
		updateDegrees();
	}

	void RingColorChanged() {
		updateRingColors();
	}

	void BandColorChanged() {
		updateBandColors();
	}

	void RadiusChanged() {
		updateOuterRadius();
	}

	void LabelsChanged() {
		updateLabelsText();
	}

	void AllChanged() {
		numberRingsC.Changed();
		textureC.Changed();
		degreesC.Changed();
		ringColorC.Changed();
		bandColorC.Changed();
		radiusC.Changed();
		labelsC.Changed();
	}

	public void bandColorsChanged(bool editorChange, bool countChanged, bool oneValChanged, int index) {
		WMG_Util.listChanged (editorChange, ref bandColors, ref _bandColors, oneValChanged, index);
		bandColorC.Changed();
	}

	public void valuesChanged(bool editorChange, bool countChanged, bool oneValChanged, int index) {
		WMG_Util.listChanged (editorChange, ref values, ref _values, oneValChanged, index);
		if (countChanged) {
			AllChanged ();
		} else {
			degreesC.Changed();
		}
	}

	public void labelsChanged(bool editorChange, bool countChanged, bool oneValChanged, int index) {
		WMG_Util.listChanged (editorChange, ref labels, ref _labels, oneValChanged, index);
		labelsC.Changed();
	}

	public void ringIDsChanged(bool editorChange, bool countChanged, bool oneValChanged, int index) {
		WMG_Util.listChanged (editorChange, ref ringIDs, ref _ringIDs, oneValChanged, index);
	}

	void updateNumberRings() {
		// Create rings based on values data
		for (int i = 0; i < values.Count; i++) {
			if (labels.Count <= i) labels.AddNoCb("Ring " + (i + 1), ref _labels);
			if (bandColors.Count <= i) bandColors.AddNoCb(bandColor, ref _bandColors);
			if (rings.Count <= i) {
				GameObject obj = GameObject.Instantiate(ringPrefab) as GameObject;
				changeSpriteParent(obj, ringsParent);
				WMG_Ring ring = obj.GetComponent<WMG_Ring>();
				ring.initialize(this);
				rings.Add(ring);
			}
		}
		for (int i = rings.Count - 1; i >= 0; i--) {
			if (rings[i] != null && i >= values.Count) {
				Destroy(rings[i].label);
				Destroy(rings[i].gameObject);
				rings.RemoveAt(i);
			}
		}
	}

	void updateOuterRadius() {
		int newSize = Mathf.RoundToInt(outerRadius*2);
		// extra ring
		changeSpriteSize(extraRing, newSize, newSize);
		// rings and bands
		for (int i = 0; i < rings.Count; i++) {
			changeSpriteSize(rings[i].ring, newSize, newSize);
			changeSpriteSize(rings[i].band, newSize, newSize);
			changeSpriteHeight(rings[i].label, Mathf.RoundToInt(outerRadius + labelLinePadding));
		}
		// zero line
		changeSpriteHeight(zeroLine, Mathf.RoundToInt(outerRadius + labelLinePadding));
		// label line points
		for (int i = 0; i < rings.Count; i++) {
			rings [i].updateRingPoint (i);
		}
		// Update position of content based on padding
		Vector3 contentOffset = new Vector3((leftRightPadding.y - leftRightPadding.x)/2f, (topBotPadding.y - topBotPadding.x)/2f);
		changeSpritePositionTo(contentParent, contentOffset);
	}

	void updateLabelsText() {
		for (int i = 0; i < rings.Count; i++) {
			changeLabelText(rings[i].labelText, labels[i]);
			forceUpdateUI();
			changeSpriteHeight(rings[i].textLine, Mathf.RoundToInt(getSpriteWidth(rings[i].labelBackground) + 10));
		}
	}

	void updateRingsAndBands() {
		// extra ring
		if (bandMode) {
			SetActive(extraRing, true);
			float ringRadius = getRingRadius(rings.Count);
			WMG_Util.updateBandColors(ref extraRingColors, outerRadius*2, ringRadius - ringWidth, ringRadius, antiAliasing, antiAliasingStrength);
			extraRingSprite.texture.SetPixels(extraRingColors);
			extraRingSprite.texture.Apply();
		}
		else {
			SetActive(extraRing, false);
		}
		// rings and bands
		for (int i = 0; i < rings.Count; i++) {
			rings[i].updateRing(i);
		}
	}

	public float getRingRadius(int index) {
		int numRingsToDivide = rings.Count - 1;
		if (bandMode) numRingsToDivide++;
		if (numRingsToDivide == 0) return outerRadius; // Only happens in non-band mode with only 1 ring
		float ringInterval = (1-innerRadiusPercentage) * outerRadius / numRingsToDivide;
		return innerRadiusPercentage * outerRadius + index * ringInterval;
	}

	void updateDegrees() {
		Vector3 baseRotation = new Vector3 (0, 0, -degrees/2);
		float newFill = (360 - degrees) / 360f;
		// extra ring
		changeRadialSpriteRotation(extraRing, baseRotation);
		changeSpriteFill(extraRing, newFill);
		// rings and bands
		for (int i = 0; i < rings.Count; i++) {
			WMG_Ring ring = rings[i];
			// rings
			changeRadialSpriteRotation(rings[i].ring, baseRotation);
			changeSpriteFill(rings[i].ring, newFill);
			// bands
			float valPercent = values[i] / (maxValue - minValue);
			changeRadialSpriteRotation(rings[i].band, baseRotation);
			changeSpriteFill(rings[i].band, 0);
			if (animateData) {
				WMG_Anim.animFill(rings[i].band, animDuration, animEaseType, newFill * valPercent);
			}
			else {
				changeSpriteFill(rings[i].band, newFill * valPercent);
			}
			// labels
			Vector3 labelRotation = new Vector3(0, 0, -valPercent * (360 - degrees));
			if (animateData) {
				if (DOTween.IsTweening(rings[i].label.transform)) { // if already animating, then don't animate relative to current rotation
					labelRotationUpdated(ring, 0);
					float degOffset = 90;
					if (ring.label.transform.localEulerAngles.z < 180) {
						degOffset *= -1;
					}
					WMG_Anim.animRotation(rings[i].label, animDuration, animEaseType, labelRotation + new Vector3(0,0,360) + baseRotation, false);
					WMG_Anim.animRotationCallbackU(rings[i].textLine, animDuration, animEaseType, -labelRotation - baseRotation + new Vector3(0,0, degOffset), false, ()=> labelRotationUpdated(ring, degOffset));
				}
				else {
					rings[i].label.transform.localEulerAngles = baseRotation;
					rings[i].textLine.transform.localEulerAngles = -baseRotation + new Vector3(0,0,90);
					WMG_Anim.animRotation(rings[i].label, animDuration, animEaseType, labelRotation, true);
					WMG_Anim.animRotationCallbackU(rings[i].textLine, animDuration, animEaseType, -labelRotation, true, ()=> labelRotationUpdated(ring, 0));
				}
			}
			else {
				rings[i].label.transform.localEulerAngles = labelRotation + baseRotation;
				rings[i].textLine.transform.localEulerAngles = -labelRotation -baseRotation + new Vector3(0,0,90);
				labelRotationUpdated(ring, 0);
			}
		}
		// zero line
		zeroLine.transform.localEulerAngles = baseRotation;
		zeroLineText.transform.localEulerAngles = -baseRotation;
	}

	void labelRotationUpdated(WMG_Ring ring, float degOffset) {
		if (ring.label.transform.localEulerAngles.z < 180) {
			if (degOffset == 0 || degOffset == 90) {
				ring.textLine.transform.localEulerAngles = new Vector3 (ring.textLine.transform.localEulerAngles.x, 
				                                                        ring.textLine.transform.localEulerAngles.y, 
				                                                        ring.textLine.transform.localEulerAngles.z - 180);
			}
			ring.labelBackground.transform.localEulerAngles = new Vector3 (0, 0, 90);
			changeSpritePivot (ring.labelBackground, WMGpivotTypes.BottomRight);
		} 
		else {
			if (degOffset == -90) {
				ring.textLine.transform.localEulerAngles = new Vector3 (ring.textLine.transform.localEulerAngles.x, 
				                                                        ring.textLine.transform.localEulerAngles.y, 
				                                                        ring.textLine.transform.localEulerAngles.z + 180);
			}
			ring.labelBackground.transform.localEulerAngles = new Vector3 (0, 0, -90);
			changeSpritePivot (ring.labelBackground, WMGpivotTypes.BottomLeft);
		}
	}

	public List<int> getRingsSortedByValue() {
		List<float> newVals = new List<float>(values);
		newVals.Sort();
		List<int> ringIndices = new List<int>();
		for (int i = 0; i < newVals.Count; i++) {
			for (int j = 0; j < values.Count; j++) {
				if (Mathf.Approximately(values[j], newVals[i])) {
					ringIndices.Add(j);
					break;
				}
			}
		}
		return ringIndices;
	}

	void updateRingColors() {
		changeSpriteColor(extraRing, ringColor);
		for (int i = 0; i < rings.Count; i++) {
			changeSpriteColor(rings[i].ring, ringColor);
		}
	}

	void updateBandColors() {
		for (int i = 0; i < rings.Count; i++) {
			if (autoUpdateBandAlpha) {
				bandColors.SetValNoCb(i, new Color(bandColors[i].r, bandColors[i].g, bandColors[i].b, (i + 1f) / rings.Count), ref _bandColors);
			}
			changeSpriteColor(rings[i].band, bandColors[i]);
		}
	}

	public WMG_Ring getRing(string id) {
		for (int i = 0; i < ringIDs.Count; i++) {
			if (id == ringIDs[i]) return rings[i];
		}
		Debug.LogError("No ring found with id: " + id);
		return null;
	}

	public void HighlightRing(string id) {
		for (int i = 0; i < rings.Count; i++) {
			changeSpriteColor(rings[i].band, new Color(bandColor.r, bandColor.g, bandColor.b, 0));
		}
		changeSpriteColor(getRing(id).band, new Color(bandColor.r, bandColor.g, bandColor.b, 1));
	}

	public void RemoveHighlights() {
		bandColorC.Changed ();
	}

}
