namespace UIWidgets
{
	using System.Collections.Generic;
	using UIWidgets.Attributes;
	using UIWidgets.Pool;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.Serialization;

	/// <summary>
	/// Base class for the SnapGrid.
	/// </summary>
	#if UNITY_2018_4_OR_NEWER
	[ExecuteAlways]
	#else
	[ExecuteInEditMode]
	#endif
	[DataBindSupport]
	public abstract partial class SnapGridBase : UIBehaviourConditional
	{
		/// <summary>
		/// Lines at X axis.
		/// </summary>
		protected List<LineX> LinesX = new List<LineX>();

		/// <summary>
		/// Lines at Y axis.
		/// </summary>
		protected List<LineY> LinesY = new List<LineY>();

		[SerializeField]
		[FormerlySerializedAs("snapBorderInside")]
		Border snapBorderInner;

		/// <summary>
		/// Allow snapping to the inner side of the border.
		/// </summary>
		[DataBindField]
		public Border SnapBorderInner
		{
			get => snapBorderInner;

			set
			{
				if (snapBorderInner != value)
				{
					snapBorderInner = value;
					UpdateLines();
				}
			}
		}

		[SerializeField]
		[FormerlySerializedAs("snapBorderOutside")]
		Border snapBorderOuter;

		/// <summary>
		/// Allow snapping to the outer side of the border.
		/// </summary>
		[DataBindField]
		public Border SnapBorderOuter
		{
			get => snapBorderOuter;

			set
			{
				if (snapBorderOuter != value)
				{
					snapBorderOuter = value;
					UpdateLines();
				}
			}
		}

		/// <summary>
		/// Event on lines changed.
		/// </summary>
		[SerializeField]
		public UnityEvent OnLinesChanged = new UnityEvent();

		/// <summary>
		/// Snap lines at X axis.
		/// </summary>
		protected List<LineX> SnapLinesX = new List<LineX>();

		/// <summary>
		/// Snap lines at Y axis.
		/// </summary>
		protected List<LineY> SnapLinesY = new List<LineY>();

		RectTransform rectTransform;

		/// <summary>
		/// RectTransform.
		/// </summary>
		public RectTransform RectTransform
		{
			get
			{
				if (rectTransform == null)
				{
					rectTransform = transform as RectTransform;
				}

				return rectTransform;
			}
		}

		/// <inheritdoc/>
		protected override void InitOnce()
		{
			base.InitOnce();

			UpdateLines();
		}

		/// <summary>
		/// Process dimensions changed event.
		/// </summary>
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();

			UpdateLines();
		}

		#if UNITY_EDITOR

		/// <summary>
		/// Process validate event.
		/// </summary>
		protected override void OnValidate()
		{
			base.OnValidate();

			UpdateLines();
		}

		#endif

		/// <summary>
		/// Update lines.
		/// </summary>
		protected abstract void UpdateLines();

		/// <summary>
		/// Get lines at X axis.
		/// </summary>
		/// <param name="includeBorders">Include borders if snap borders enabled.</param>
		/// <param name="output">Result.</param>
		public virtual void GetLinesX(bool includeBorders, List<float> output)
		{
			if (includeBorders && (SnapBorderInner.Left || SnapBorderOuter.Left))
			{
				output.Add(0f);
			}

			foreach (var line in LinesX)
			{
				output.Add(line.X);
			}

			if (includeBorders && (SnapBorderInner.Right || SnapBorderOuter.Right))
			{
				output.Add(RectTransform.rect.width);
			}
		}

		/// <summary>
		/// Get lines at Y axis.
		/// </summary>
		/// <param name="includeBorders">Include borders if snap borders enabled.</param>
		/// <param name="output">Result.</param>
		public void GetLinesY(bool includeBorders, List<float> output)
		{
			if (includeBorders && (SnapBorderInner.Bottom || SnapBorderOuter.Bottom))
			{
				output.Add(0f);
			}

			foreach (var line in LinesY)
			{
				output.Add(line.Y);
			}

			if (includeBorders && (SnapBorderInner.Top || SnapBorderOuter.Top))
			{
				output.Add(RectTransform.rect.height);
			}
		}

		/// <summary>
		/// Get lines at X axis.
		/// </summary>
		/// <param name="output">Result.</param>
		public virtual void GetLinesX(List<LineX> output)
		{
			if (SnapBorderInner.Left || SnapBorderOuter.Left)
			{
				output.Add(new LineX(0f, SnapBorderInner.Left, SnapBorderOuter.Left));
			}

			output.AddRange(LinesX);

			if (SnapBorderInner.Right || SnapBorderOuter.Right)
			{
				output.Add(new LineX(RectTransform.rect.width, SnapBorderOuter.Right, SnapBorderInner.Right));
			}
		}

		/// <summary>
		/// Get grid position.
		/// </summary>
		/// <returns>Position.</returns>
		protected virtual Vector2 GridPosition()
		{
			var size = RectTransform.rect.size;
			var pivot = RectTransform.pivot;
			var position = RectTransform.position;
			var scale = RectTransform.lossyScale;

			position.x = (position.x / scale.x) - (size.x * pivot.x);
			position.y = (position.y / scale.y) - (size.y * pivot.y);

			return position;
		}

		/// <summary>
		/// Get lines at Y axis.
		/// </summary>
		/// <param name="output">Result.</param>
		public virtual void GetLinesY(List<LineY> output)
		{
			if (SnapBorderInner.Bottom || SnapBorderOuter.Bottom)
			{
				output.Add(new LineY(0f, SnapBorderInner.Bottom, SnapBorderOuter.Bottom));
			}

			output.AddRange(LinesY);

			if (SnapBorderInner.Top || SnapBorderOuter.Top)
			{
				output.Add(new LineY(RectTransform.rect.height, SnapBorderOuter.Top, SnapBorderInner.Top));
			}
		}

		/// <summary>
		/// Snap point.
		/// </summary>
		/// <param name="point">Point.</param>
		/// <param name="snapDistance">Maximum distance to snap.</param>
		/// <returns>Distance to the nearest line or lines.</returns>
		public virtual Result Snap(Point point, Vector2 snapDistance)
		{
			var distance = new Distance(float.MaxValue);

			Snap(point, ref distance);

			return distance.Snap(snapDistance);
		}

		/// <summary>
		/// Snap point.
		/// </summary>
		/// <param name="point">Point.</param>
		/// <param name="distance">Distance.</param>
		public virtual void Snap(Point point, ref Distance distance)
		{
			var current_position = GridPosition();

			foreach (var line in SnapLinesX)
			{
				var x = current_position.x + line.X - point.X;

				if (line.SnapLeft && point.Left)
				{
					distance.Left(x);
				}

				if (line.SnapRight && point.Right)
				{
					distance.Right(x);
				}
			}

			foreach (var line in SnapLinesY)
			{
				var y = current_position.y + line.Y - point.Y;

				if (line.SnapTop && point.Top)
				{
					distance.Top(y);
				}

				if (line.SnapBottom && point.Bottom)
				{
					distance.Bottom(y);
				}
			}
		}

		/// <summary>
		/// Snap points.
		/// </summary>
		/// <param name="points">Points.</param>
		/// <param name="snapDistance">Maximum distance to snap.</param>
		/// <returns>Distance to the nearest line or lines.</returns>
		public virtual Result Snap(List<Point> points, Vector2 snapDistance)
		{
			var distance = new Distance(float.MaxValue);
			Snap(points, ref distance);

			return distance.Snap(snapDistance);
		}

		/// <summary>
		/// Snap points.
		/// </summary>
		/// <param name="points">Points.</param>
		/// <param name="distance">Distance.</param>
		public virtual void Snap(List<Point> points, ref Distance distance)
		{
			SnapLinesX.Clear();
			GetLinesX(SnapLinesX);

			SnapLinesY.Clear();
			GetLinesY(SnapLinesY);

			var current_position = GridPosition();

			foreach (var point in points)
			{
				foreach (var line in SnapLinesX)
				{
					var x = current_position.x + line.X - point.X;

					if (line.SnapLeft && point.Left)
					{
						distance.Left(x);
					}

					if (line.SnapRight && point.Right)
					{
						distance.Right(x);
					}
				}

				foreach (var line in SnapLinesY)
				{
					var y = current_position.y + line.Y - point.Y;
					if (line.SnapTop && point.Top)
					{
						distance.Top(y);
					}

					if (line.SnapBottom && point.Bottom)
					{
						distance.Bottom(y);
					}
				}
			}
		}

		/// <summary>
		/// Snap target.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="snapDistance">Maximum distance to snap.</param>
		/// <returns>Distance to the nearest line or lines.</returns>
		public virtual Result Snap(RectTransform target, Vector2 snapDistance)
		{
			using var _ = ListPool<Point>.Get(out var temp);

			temp.Add(new Point(target, this, true, true));
			temp.Add(new Point(target, this, true, false));
			temp.Add(new Point(target, this, false, true));
			temp.Add(new Point(target, this, false, false));

			return Snap(temp, snapDistance);
		}

		/// <summary>
		/// Snap target.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="distance">Distance.</param>
		public virtual void Snap(RectTransform target, ref Distance distance)
		{
			using var _ = ListPool<Point>.Get(out var temp);

			temp.Add(new Point(target, this, true, true));
			temp.Add(new Point(target, this, true, false));
			temp.Add(new Point(target, this, false, true));
			temp.Add(new Point(target, this, false, false));

			Snap(temp, ref distance);
		}

		/// <summary>
		/// Snap target.
		/// </summary>
		/// <param name="snapGrids">Snap grids.</param>
		/// <param name="snapDistance">Snap distance.</param>
		/// <param name="target">Target.</param>
		/// <returns>Distance to the nearest line or lines.</returns>
		public static Result Snap(List<SnapGridBase> snapGrids, Vector2 snapDistance, RectTransform target)
		{
			if (snapGrids == null)
			{
				return default;
			}

			if (snapGrids.Count == 0)
			{
				return default;
			}

			var distance = new Distance(float.MaxValue);

			foreach (var snap_grid in snapGrids)
			{
				if (snap_grid == null)
				{
					continue;
				}

				snap_grid.Snap(target, ref distance);
			}

			return distance.Snap(snapDistance);
		}
	}
}