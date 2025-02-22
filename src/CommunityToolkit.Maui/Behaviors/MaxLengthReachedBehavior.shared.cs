﻿#nullable enable
using System;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Behaviors.Internals;
using CommunityToolkit.Maui.Helpers;
using Microsoft.Maui.Controls;

namespace CommunityToolkit.Maui.Behaviors
{
    /// <summary>
    /// The <see cref="MaxLengthReachedBehavior"/> is a behavior that allows the user to trigger an action when a user has reached the maximum length allowed on an <see cref="InputView"/>. It can either trigger a <see cref="ICommand"/> or an event depending on the user's preferred scenario.
    /// </summary>
    public class MaxLengthReachedBehavior : BaseBehavior<InputView>
	{
		/// <summary>
		/// Backing BindableProperty for the <see cref="Command"/> property.
		/// </summary>
		public static readonly BindableProperty CommandProperty
			= BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(MaxLengthReachedBehavior));

		/// <summary>
		/// Command that is triggered when the value configured in <see cref="InputView.MaxLength" /> is reached. Both the <see cref="MaxLengthReached"/> event and this command are triggered. This is a bindable property.
		/// </summary>
		public ICommand? Command
		{
			get => (ICommand?)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="ShouldDismissKeyboardAutomatically"/> property.
		/// </summary>
		public static readonly BindableProperty ShouldDismissKeyboardAutomaticallyProperty
			= BindableProperty.Create(nameof(ShouldDismissKeyboardAutomatically), typeof(bool), typeof(MaxLengthReachedBehavior), false);

		/// <summary>
		/// Indicates whether or not the keyboard should be dismissed automatically after the maximum length is reached. This is a bindable property.
		/// </summary>
		public bool ShouldDismissKeyboardAutomatically
		{
			get => (bool)GetValue(ShouldDismissKeyboardAutomaticallyProperty);
			set => SetValue(ShouldDismissKeyboardAutomaticallyProperty, value);
		}

		readonly WeakEventManager<MaxLengthReachedEventArgs> maxLengthReachedEventManager = new WeakEventManager<MaxLengthReachedEventArgs>();

		/// <summary>
		/// Event that is triggered when the value configured in <see cref="InputView.MaxLength" /> is reached. Both the <see cref="Command"/> and this event are triggered. This is a bindable property.
		/// </summary>
		public event EventHandler<MaxLengthReachedEventArgs> MaxLengthReached
		{
			add => maxLengthReachedEventManager.AddEventHandler(value);
			remove => maxLengthReachedEventManager.RemoveEventHandler(value);
		}

		protected override void OnViewPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			base.OnViewPropertyChanged(sender, e);
			if (e.PropertyName == InputView.TextProperty.PropertyName)
				OnTextPropertyChanged();
		}

		void OnTextPropertyChanged()
		{
			if (View?.Text == null || View.Text.Length < View.MaxLength)
				return;

			if (ShouldDismissKeyboardAutomatically)
				View.Unfocus();

			var newTextValue = View.Text.Substring(0, View.MaxLength);

			maxLengthReachedEventManager.RaiseEvent(this, new MaxLengthReachedEventArgs(newTextValue), nameof(MaxLengthReached));

			if (Command?.CanExecute(newTextValue) ?? false)
				Command.Execute(newTextValue);
		}
	}
}