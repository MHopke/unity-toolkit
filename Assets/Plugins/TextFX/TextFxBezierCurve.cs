using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Boomlagoon.JSON;

[System.Serializable]
public class BezierCurvePoint
{
	public Vector3 m_anchor_point;
	public Vector3 m_handle_point;
	
	public Vector3 HandlePoint(bool inverse)
	{
		if(inverse)
			return m_anchor_point + (m_anchor_point - m_handle_point);
		else
			return m_handle_point;
	}
}

[System.Serializable]
public class TextFxBezierCurve
{
	const int m_gizmo_line_subdivides = 25;
	const int NUM_CURVE_SAMPLE_SUBSECTIONS = 50;
	
	public List<BezierCurvePoint> m_anchor_points;
#if UNITY_EDITOR	
	bool m_editor_visible = false;
	public bool EditorVisible { get { return m_editor_visible; } set { m_editor_visible = value; } }
#endif
	Vector3[] m_temp_anchor_points;
	Vector3 point_dir_vec;
	Vector3 rot;
	
	public TextFxBezierCurve()
	{
		
	}
	
	public TextFxBezierCurve(TextFxBezierCurve curve)
	{
		m_anchor_points = new List<BezierCurvePoint>();
		if(curve.m_anchor_points != null)
			m_anchor_points.InsertRange(0, curve.m_anchor_points);
	}
	
	public void AddNewAnchor()
	{
		if(m_anchor_points == null || m_anchor_points.Count == 0)
		{
			m_anchor_points = new List<BezierCurvePoint>();
			
			m_anchor_points.Add(new BezierCurvePoint() { m_anchor_point = new Vector3(-5, 0, 0), m_handle_point = new Vector3(-2.5f, 4f, 0)} );
			m_anchor_points.Add(new BezierCurvePoint() { m_anchor_point = new Vector3(5, 0, 0), m_handle_point = new Vector3(2.5f, 4f, 0)} );
		}
		else
		{
			BezierCurvePoint last_point = m_anchor_points[m_anchor_points.Count - 1];
			m_anchor_points.Add(new BezierCurvePoint() { m_anchor_point = last_point.m_anchor_point + new Vector3(5, 0, 0), m_handle_point = last_point.m_handle_point + new Vector3(5, 0, 0)} );
		}
	}
	
	public Vector3 GetCurvePoint(float progress, int num_anchors = 4, int curve_idx = -1)
	{
		if(m_anchor_points == null || m_anchor_points.Count < 2)
			return Vector3.zero;
		
		if(m_temp_anchor_points == null || m_temp_anchor_points.Length < num_anchors)
			m_temp_anchor_points = new Vector3[num_anchors];
		
		if(curve_idx < 0)
		{
			// Work out curve idx from progress
			curve_idx = Mathf.FloorToInt(progress);
			
			progress %= 1;
		}
		
		if(curve_idx >= m_anchor_points.Count - 1)
		{
			curve_idx = m_anchor_points.Count - 2;
			progress = 1;
		}
		
		
		for(int idx=1; idx < num_anchors; idx++)
		{
			if(num_anchors == 4)
			{
				if(idx == 1)
					m_temp_anchor_points[idx-1] = m_anchor_points[curve_idx].m_anchor_point + ( m_anchor_points[curve_idx].HandlePoint(curve_idx > 0) -  m_anchor_points[curve_idx].m_anchor_point) * progress;
				else if(idx == 2)
					m_temp_anchor_points[idx-1] = m_anchor_points[curve_idx].HandlePoint(curve_idx > 0) + ( m_anchor_points[curve_idx+1].m_handle_point -  m_anchor_points[curve_idx].HandlePoint(curve_idx > 0)) * progress;
				else if(idx == 3)
					m_temp_anchor_points[idx-1] = m_anchor_points[curve_idx+1].m_handle_point + ( m_anchor_points[curve_idx+1].m_anchor_point -  m_anchor_points[curve_idx+1].m_handle_point) * progress;
			}
			else
				m_temp_anchor_points[idx-1] = m_temp_anchor_points[idx-1] + (m_temp_anchor_points[idx] - m_temp_anchor_points[idx-1]) * progress;
		}
		
		if(num_anchors == 2)
			return m_temp_anchor_points[0];
		else
			return GetCurvePoint(progress, num_anchors-1, curve_idx);
	}
	
	public Vector3 GetCurvePointRotation(float progress, int curve_idx = -1)
	{
		if(m_anchor_points == null || m_anchor_points.Count < 2)
			return Vector3.zero;
		
		if(curve_idx < 0)
		{
			// Work out curve idx from progress
			curve_idx = Mathf.FloorToInt(progress);
			
			progress %= 1;
		}
		
		if(curve_idx >= m_anchor_points.Count - 1)
		{
			curve_idx = m_anchor_points.Count - 2;
			progress = 1;
		}
		
		point_dir_vec = GetCurvePoint(Mathf.Clamp(progress + 0.01f, 0, 1), curve_idx : curve_idx) - GetCurvePoint(Mathf.Clamp(progress - 0.01f, 0, 1), curve_idx : curve_idx);
		
		rot = (Quaternion.AngleAxis(-90, (point_dir_vec)) * Quaternion.LookRotation(Vector3.Cross(point_dir_vec, Vector3.forward), Vector3.forward)).eulerAngles;
		
		// Clamp all axis rotations to be within [-180, 180] range for more sensible looking rotation transitions
		rot.x -= rot.x < 180 ? 0 : 360;
		rot.y -= rot.y < 180 ? 0 : 360;
		rot.z -= rot.z < 180 ? 0 : 360;
		
		return rot;
	}
	
	public float[] GetLetterProgressions(ref LetterSetup[] letters, int letter_anchor)
	{
		float[] letter_progressions = new float[letters.Length];
		
		if(m_anchor_points == null || m_anchor_points.Count < 2)
		{
			for(int idx=0; idx < letters.Length; idx++)
			{
				letter_progressions[idx] = 0;
			}
			return letter_progressions;
		}
		
		float progress_inc = 1f / NUM_CURVE_SAMPLE_SUBSECTIONS;
		float progress;
		Vector3 new_point = new Vector3();
		Vector3 last_point = new Vector3();
		int letter_idx = 0, line_number=0;
		float base_letters_offset = 0, letters_offset = 0;
		LetterSetup letter;
		float last_line_length = 0, line_length = 0;
		
		
		letter = letters[letter_idx];
		base_letters_offset = letters[0].m_base_offset.x;
		letters_offset = letter.m_base_offset.x - base_letters_offset + GetLetterAnchorOffset(letter, letter_anchor);

		for(int curve_idx=0; curve_idx < m_anchor_points.Count - 1; curve_idx++)
		{
			for(int idx=0; idx <= NUM_CURVE_SAMPLE_SUBSECTIONS; idx++)
			{
				progress = idx * progress_inc;
				
				new_point = GetCurvePoint(progress, curve_idx : curve_idx);
				
				if(idx > 0)
				{
					line_length += (new_point - last_point).magnitude;
					
					while(letter_idx < letters.Length && line_length > letters_offset)
					{
						// calculate relative progress between the last two points which would represent the next letters offset distance
						progress = curve_idx + ((idx-1) * progress_inc) + (((letters_offset - last_line_length) / (line_length - last_line_length)) * progress_inc);
						
						letter_progressions[letter_idx] = progress;
						
						letter_idx++;
						
						if(letter_idx < letters.Length)
						{
							letter = letters[letter_idx];
							
							if(letter.m_progression_variables.m_line_value > line_number)
							{
								line_number = letter.m_progression_variables.m_line_value;
								
								idx=-1;
								line_length = 0;
							}
							
							letters_offset = letter.m_base_offset.x - base_letters_offset + GetLetterAnchorOffset(letter, letter_anchor);
						}
					}
					
					if(letter_idx == letters.Length)
						break;
				}
				
				last_point = new_point;
				last_line_length = line_length;
			}
		}
		
		// Handle any letters which didn't have room to fit on the line
		// Currently forces them to all position at the end of the line
		for(int idx=letter_idx; idx < letters.Length; idx++)
		{
			letter_progressions[idx] = m_anchor_points.Count - 1;
		}
		
		return letter_progressions;
	}
	
	float GetLetterAnchorOffset(LetterSetup letter, int letter_anchor)
	{
		return (letter_anchor == (int) TextfxTextAnchor.LowerCenter || letter_anchor == (int) TextfxTextAnchor.MiddleCenter || letter_anchor == (int) TextfxTextAnchor.UpperCenter || letter_anchor == (int) TextfxTextAnchor.BaselineCenter ? (letter.m_offset_width + (letter.m_width - letter.m_offset_width))/2
				: ((letter_anchor == (int) TextfxTextAnchor.LowerRight || letter_anchor == (int) TextfxTextAnchor.MiddleRight || letter_anchor == (int) TextfxTextAnchor.UpperRight || letter_anchor == (int) TextfxTextAnchor.BaselineRight) ? (letter.m_offset_width + (letter.m_width - letter.m_offset_width)) : 0));
	}
	
	
	public JSONValue ExportData()
	{
		JSONObject json_data = new JSONObject();
		
		JSONArray anchors_data = new JSONArray();
		JSONObject anchor_point_data;
		
		foreach(BezierCurvePoint anchor_point in m_anchor_points)
		{
			anchor_point_data = new JSONObject();
			anchor_point_data["m_anchor_point"] = anchor_point.m_anchor_point.ExportData();
			anchor_point_data["m_handle_point"] = anchor_point.m_handle_point.ExportData();
			
			anchors_data.Add(anchor_point_data);
		}
		
		json_data["ANCHORS_DATA"] = anchors_data;
		
		return new JSONValue(json_data);
	}
	
	public void ImportData(JSONObject json_data)
	{
		m_anchor_points = new List<BezierCurvePoint>();
		
		BezierCurvePoint curve_point;
		JSONObject anchor_json;
		
		foreach(JSONValue anchor_data in json_data["ANCHORS_DATA"].Array)
		{
			anchor_json = anchor_data.Obj;
			curve_point = new BezierCurvePoint();
			curve_point.m_anchor_point = anchor_json["m_anchor_point"].Obj.JSONtoVector3();
			curve_point.m_handle_point = anchor_json["m_handle_point"].Obj.JSONtoVector3();
			m_anchor_points.Add(curve_point);
		}
	}
	
#if UNITY_EDITOR
	public void OnSceneGUI(Vector3 position_offset, Vector3 scale)
	{
		if(m_anchor_points == null || m_anchor_points.Count < 2)
			return;
		
		bool changed = false;
		
		DrawCurvePoint(m_anchor_points[0], position_offset, scale, true, ref changed);
		
		for(int p_idx=1; p_idx < m_anchor_points.Count; p_idx++)
		{
			DrawCurvePoint(m_anchor_points[p_idx], position_offset, scale, p_idx == m_anchor_points.Count - 1, ref changed);
			
			Handles.DrawBezier(
								Vector3.Scale(m_anchor_points[p_idx-1].m_anchor_point, scale) + position_offset,
								Vector3.Scale(m_anchor_points[p_idx].m_anchor_point, scale) + position_offset,
								(p_idx == 1 ? Vector3.Scale(m_anchor_points[p_idx-1].m_handle_point, scale) : Vector3.Scale(m_anchor_points[p_idx-1].m_anchor_point + (m_anchor_points[p_idx-1].m_anchor_point - m_anchor_points[p_idx-1].m_handle_point), scale)) + position_offset,
								Vector3.Scale(m_anchor_points[p_idx].m_handle_point, scale) + position_offset,
								Color.red, null, 2);
		}
	}
	
	
	void DrawCurvePoint(BezierCurvePoint curve_point, Vector3 position_offset, Vector3 scale, bool start_end_point, ref bool changed)
	{
		Vector3 handle_offset =  curve_point.m_handle_point - curve_point.m_anchor_point;
		curve_point.m_anchor_point = Vector3.Scale(Handles.PositionHandle(Vector3.Scale(curve_point.m_anchor_point,scale) + position_offset, Quaternion.identity) - position_offset, new Vector3(1/scale.x,1/scale.y,1/scale.z));
		
		if(!changed && GUI.changed)
		{
			changed = true;
			curve_point.m_handle_point = curve_point.m_anchor_point + handle_offset;
		}
		
		curve_point.m_handle_point = Vector3.Scale(Handles.PositionHandle(Vector3.Scale(curve_point.m_handle_point,scale) + position_offset, Quaternion.identity) - position_offset, new Vector3(1/scale.x,1/scale.y,1/scale.z));
		
		Handles.color = Color.white;
		Handles.DrawLine(!start_end_point ? Vector3.Scale(curve_point.m_anchor_point, scale) + Vector3.Scale((curve_point.m_anchor_point - curve_point.m_handle_point), scale) + position_offset : Vector3.Scale(curve_point.m_anchor_point, scale) + position_offset, 
						Vector3.Scale(curve_point.m_handle_point, scale) + position_offset);
	}
#endif	
}