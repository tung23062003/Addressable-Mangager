﻿namespace UIWidgets
{
	using UnityEngine;
	using UnityEngine.EventSystems;

	/// <summary>
	/// InputField listener.
	/// </summary>
	[HelpURL("https://ilih.name/unity-assets/UIWidgets/docs/components/listeners/inputfield.html")]
	public class InputFieldListener : SelectListener, IUpdateSelectedHandler
	{
		/// <summary>
		/// Move event.
		/// </summary>
		[SerializeField]
		public MoveEvent OnMoveEvent = new MoveEvent();

		/// <summary>
		/// Submit event.
		/// </summary>
		[SerializeField]
		public SubmitEvent OnSubmitEvent = new SubmitEvent();

		/// <summary>
		/// OnUpdateSelected event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnUpdateSelected(BaseEventData eventData)
		{
			if (CompatibilityInput.LeftArrowDown)
			{
				var axisEvent = new AxisEventData(EventSystem.current)
				{
					moveDir = MoveDirection.Left,
				};
				OnMoveEvent.Invoke(axisEvent);
				return;
			}

			if (CompatibilityInput.RightArrowDown)
			{
				var axisEvent = new AxisEventData(EventSystem.current)
				{
					moveDir = MoveDirection.Right,
				};
				OnMoveEvent.Invoke(axisEvent);
				return;
			}

			if (CompatibilityInput.UpArrowDown)
			{
				var axisEvent = new AxisEventData(EventSystem.current)
				{
					moveDir = MoveDirection.Up,
				};
				OnMoveEvent.Invoke(axisEvent);
				return;
			}

			if (CompatibilityInput.DownArrowDown)
			{
				var axisEvent = new AxisEventData(EventSystem.current)
				{
					moveDir = MoveDirection.Down,
				};
				OnMoveEvent.Invoke(axisEvent);
				return;
			}

			if (CompatibilityInput.TabDown || CompatibilityInput.EnterDown)
			{
				var isEnter = CompatibilityInput.EnterPressed;
				var isShift = CompatibilityInput.ShiftPressed;
				if (!(isEnter && isShift))
				{
					OnSubmitEvent.Invoke(eventData, isEnter);
				}

				return;
			}
		}
	}
}