﻿namespace Smart.Forms.Markup
{
    using System;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    ///
    /// </summary>
    [ContentProperty("Source")]
    public class ImageResourceExtension : IMarkupExtension
    {
        /// <summary>
        ///
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return Source == null ? null : ImageSource.FromResource(Source);
        }
    }
}
