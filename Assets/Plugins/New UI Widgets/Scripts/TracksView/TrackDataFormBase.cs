﻿namespace UIWidgets
{
	using System;
	using UnityEngine;

	/// <summary>
	/// Base class for the data form.
	/// </summary>
	/// <typeparam name="TData">Data type.</typeparam>
	/// <typeparam name="TPoint">Point type.</typeparam>
	[HelpURL("https://ilih.name/unity-assets/UIWidgets/docs/widgets/collections/tracksview.html")]
	public abstract class TrackDataFormBase<TData, TPoint> : MonoBehaviourInitiable
		where TData : ITrackData<TPoint>
		where TPoint : IComparable<TPoint>
	{
		/// <summary>
		/// Data.
		/// </summary>
		public TData Data
		{
			get;
			protected set;
		}

		/// <summary>
		/// Create data.
		/// </summary>
		public abstract void Create();

		/// <summary>
		/// Create data with specified StartPoint.
		/// </summary>
		/// <param name="startPoint">SpartPoint.</param>
		public abstract void Create(TPoint startPoint);

		/// <summary>
		/// Edit data.
		/// </summary>
		/// <param name="data">Data.</param>
		public abstract void Edit(TData data);

		/// <summary>
		/// Add listeners.
		/// </summary>
		public abstract void AddListeners();

		/// <summary>
		/// Remove listeners.
		/// </summary>
		public abstract void RemoveListeners();

		/// <inheritdoc/>
		protected override void InitOnce()
		{
			base.InitOnce();

			AddListeners();
		}

		/// <summary>
		/// Process destroy event.
		/// </summary>
		protected virtual void OnDestroy()
		{
			RemoveListeners();
		}
	}
}