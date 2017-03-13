using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
	public class ExtendedGrid : VisualElement
	{
		#region Public Vars
		public bool Fill;
		public int ItemCount;
		public GridLayoutGroup Grid;
		#endregion

		#region Overridden Methods
		protected override void OnInit ()
		{
			base.OnInit ();
			Resize();
		}
		#endregion

		#region Methods
		public void Resize()
		{
			RectTransform rect = Grid.transform as RectTransform;

			int dividor = Mathf.CeilToInt((float)ItemCount / (float)Grid.constraintCount);

			//Debug.Log(dividor);

			if(Fill)
			{
				Grid.cellSize = new Vector2(rect.rect.width / Grid.constraintCount - (Grid.spacing.x * Grid.constraintCount -1), 
					rect.rect.height / dividor - (Grid.spacing.y * Grid.constraintCount -1));
			}
			else
			{
				Grid.cellSize = new Vector2(rect.rect.height / 
				dividor - (Grid.spacing.x * dividor), rect.rect.height / dividor - (Grid.spacing.y * dividor));
			}
		}
		#endregion
	}
}