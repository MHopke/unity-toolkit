using UnityEngine;

[AddComponentMenu("Rendering/SetRenderQueue")]

/// <summary>
/// Sets the RenderQueue of an object's materials on Awake. This will instance
/// the materials, so the script won't interfere with other renderers that
/// reference the same materials.
/// </summary>
public class SetRenderQueue : MonoBehaviour 
{
	
	[SerializeField]
	protected int[] m_queues = new int[]{3000};
	
	protected void Awake() {
		Material[] materials = renderer.materials;
		for (int i = 0; i < materials.Length && i < m_queues.Length; ++i) {
			materials[i].renderQueue = m_queues[i];
		}
	}
}