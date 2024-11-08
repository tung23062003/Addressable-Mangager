﻿// <auto-generated/>
// Auto-generated added to suppress names errors.

namespace UIWidgets
{
	using UIWidgets.Attributes;
	using UnityEngine;

	/// <summary>
	/// Spinner.
	/// </summary>
	public class Spinner : SpinnerBase<int>
	{
		/// <summary>
		/// onValueChange event.
		/// </summary>
		[DataBindEvent(nameof(Value))]
		public OnChangeEventInt onValueChangeInt = new OnChangeEventInt();

		/// <summary>
		/// onEndEdit event.
		/// </summary>
		public SubmitEventInt onEndEditInt = new SubmitEventInt();

		/// <summary>
		/// Initializes a new instance of the <see cref="Spinner"/> class.
		/// </summary>
		public Spinner()
		{
			ValueMax = 100;
			ValueStep = 1;
		}

		/// <summary>
		/// Increase value on step.
		/// </summary>
		public override void Plus()
		{
			if ((Value <= 0) || (int.MaxValue - Value) >= Step)
			{
				Value += Step;
			}
			else
			{
				Value = int.MaxValue;
			}
		}

		/// <summary>
		/// Decrease value on step.
		/// </summary>
		public override void Minus()
		{
			if ((Value >= 0) || (Mathf.Abs(int.MinValue - Value) >= Step))
			{
				Value -= Step;
			}
			else
			{
				Value = int.MinValue;
			}
		}

		/// <inheritdoc/>
		public override void SetValue(int newValue, bool raiseEvent)
		{
			if (SpinnerValue == InBounds(newValue))
			{
				SetTextValue();

				return;
			}

			SpinnerValue = InBounds(newValue);

			SetTextValue();

			if (raiseEvent)
			{
				onValueChangeInt.Invoke(SpinnerValue);
			}
		}

		/// <summary>
		/// Called when value changed.
		/// </summary>
		/// <param name="value">Value.</param>
		protected override void ValueChange(string value)
		{
			if (Validation == SpinnerValidation.OnEndInput)
			{
				return;
			}

			if (value.Length == 0)
			{
				value = "0";
			}

			ParseValue(value);
		}

		/// <summary>
		/// Parse value.
		/// </summary>
		/// <param name="value">Value.</param>
		protected void ParseValue(string value)
		{
			int new_value;
			if (!int.TryParse(value, out new_value))
			{
				new_value = (value.Trim()[0] == '-') ? int.MinValue : int.MaxValue;
			}

			SetValue(new_value);
		}

		/// <summary>
		/// Called when end edit.
		/// </summary>
		/// <param name="value">Value.</param>
		protected override void ValueEndEdit(string value)
		{
			if (value.Length == 0)
			{
				value = "0";
			}

			ParseValue(value);
			onEndEditInt.Invoke(SpinnerValue);
		}

		/// <summary>
		/// Validate when key down for Validation=OnEndInput.
		/// </summary>
		/// <returns>The char.</returns>
		/// <param name="validateText">Validate text.</param>
		/// <param name="charIndex">Char index.</param>
		/// <param name="addedChar">Added char.</param>
		protected override char ValidateShort(string validateText, int charIndex, char addedChar)
		{
			var empty_text = validateText.Length <= 0;
			var is_positive = empty_text || validateText[0] != '-';

			var selection_start = InputFieldAdapter.SelectionStart;
			var selection_end = InputFieldAdapter.SelectionEnd;

			var selection_at_start = selection_start == 0 && selection_start != selection_end;

			if (selection_at_start)
			{
				charIndex = selection_start;
			}

			var not_first = charIndex != 0;

			if (not_first || empty_text || is_positive || selection_at_start)
			{
				if (addedChar >= '0' && addedChar <= '9')
				{
					return addedChar;
				}

				if (addedChar == '-' && charIndex == 0 && Min < 0)
				{
					return addedChar;
				}
			}

			return '\0';
		}

		/// <summary>
		/// Validates when key down for Validation=OnKeyDown.
		/// </summary>
		/// <returns>The char.</returns>
		/// <param name="validateText">Validate text.</param>
		/// <param name="charIndex">Char index.</param>
		/// <param name="addedChar">Added char.</param>
		protected override char ValidateFull(string validateText, int charIndex, char addedChar)
		{
			var number = validateText.Insert(charIndex, addedChar.ToString());

			if ((Min > 0) && (charIndex == 0) && (addedChar == '-'))
			{
				return (char)0;
			}

			var min_parse_length = ((number.Length > 0) && (number[0] == '-')) ? 2 : 1;
			if (number.Length >= min_parse_length)
			{
				int new_value;
				if (!int.TryParse(number, out new_value))
				{
					return (char)0;
				}

				if (new_value != InBounds(new_value))
				{
					return (char)0;
				}
			}

			return addedChar;
		}

		/// <summary>
		/// Clamps a value between a minimum and maximum value.
		/// </summary>
		/// <returns>The bounds.</returns>
		/// <param name="value">Value.</param>
		protected override int InBounds(int value)
		{
			return ValueLimits ? Mathf.Clamp(value, ValueMin, ValueMax) : value;
		}
	}
}