﻿using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System;

namespace CommunityToolkit.Maui.Extensions.Internals
{
    public class MultiValueConverterExtension : IMarkupExtension<IMultiValueConverter>
	{
		public IMultiValueConverter ProvideValue(IServiceProvider serviceProvider)
			=> (IMultiValueConverter)this;

		object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
			=> ((IMarkupExtension<IMultiValueConverter>)this).ProvideValue(serviceProvider);
	}
}