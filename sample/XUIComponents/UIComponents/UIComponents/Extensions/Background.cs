﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace UIComponents.Extensions
{
    public enum BackgroundOptions
    {
        /// <summary>
        /// Center the background image.
        /// </summary>
        Center,

        /// <summary>
        /// Scale the background image, retaining the aspect ratio.
        /// </summary>
        Scale,

        /// <summary>
        /// Stretch the background image to fill the widget's area.
        /// </summary>
        Stretch,

        /// <summary>
        /// Tile the background image at its original size.
        /// </summary>
        Tile
    }

    public class Background : View
    {
        public static readonly BindableProperty ImageProperty = BindableProperty.Create("Image", typeof(FileImageSource), typeof(Background), default(FileImageSource));

        public static readonly BindableProperty OptionProperty = BindableProperty.Create("Option", typeof(BackgroundOptions), typeof(Background), BackgroundOptions.Scale);


        public FileImageSource Image
        {
            get { return (FileImageSource)GetValue(ImageProperty); }
            set {
                SetValue(ImageProperty, value); 
            }
        }

        public BackgroundOptions Option
        {
            get { return (BackgroundOptions)GetValue(OptionProperty); }
            set {
                Console.WriteLine($"Option Set:{value}");
                SetValue(OptionProperty, value); 
            }
        }
    }
}
