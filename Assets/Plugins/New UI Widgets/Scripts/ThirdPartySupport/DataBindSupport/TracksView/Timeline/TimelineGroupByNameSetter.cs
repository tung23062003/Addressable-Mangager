#if UIWIDGETS_DATABIND_SUPPORT
namespace UIWidgets.Timeline.DataBind
{
	using Slash.Unity.DataBind.Foundation.Setters;
	using UnityEngine;

	/// <summary>
	/// Set the GroupByName of a Timeline depending on the System.Boolean data value.
	/// </summary>
	[AddComponentMenu("Data Bind/New UI Widgets/Setters/[DB] Timeline GroupByName Setter")]
	public class TimelineGroupByNameSetter : ComponentSingleSetter<UIWidgets.Timeline.Timeline, System.Boolean>
	{
		/// <inheritdoc />
		protected override void UpdateTargetValue(UIWidgets.Timeline.Timeline target, System.Boolean value)
		{
			target.GroupByName = value;
		}
	}
}
#endif