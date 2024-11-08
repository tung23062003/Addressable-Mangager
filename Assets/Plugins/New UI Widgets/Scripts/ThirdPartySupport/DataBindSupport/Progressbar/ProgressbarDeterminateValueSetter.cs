#if UIWIDGETS_DATABIND_SUPPORT
namespace UIWidgets.DataBind
{
	using Slash.Unity.DataBind.Foundation.Setters;
	using UnityEngine;

	/// <summary>
	/// Set the Value of a ProgressbarDeterminate depending on the System.Int32 data value.
	/// </summary>
	[AddComponentMenu("Data Bind/New UI Widgets/Setters/[DB] ProgressbarDeterminate Value Setter")]
	public class ProgressbarDeterminateValueSetter : ComponentSingleSetter<UIWidgets.ProgressbarDeterminate, System.Int32>
	{
		/// <inheritdoc />
		protected override void UpdateTargetValue(UIWidgets.ProgressbarDeterminate target, System.Int32 value)
		{
			target.Value = value;
		}
	}
}
#endif