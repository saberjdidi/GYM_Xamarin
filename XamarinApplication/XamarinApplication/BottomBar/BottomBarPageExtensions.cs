using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinApplication.BottomBar
{
    public static class BottomBarPageExtensions
    {
        public static readonly BindableProperty TabColorProperty = BindableProperty.CreateAttached(
                "TabColor", typeof(Color), typeof(BottomBarPageExtensions), Color.Transparent
            );

        public static void SetTabColor(BindableObject bindable, Color color)
        {
            bindable.SetValue(TabColorProperty, color);
        }

        public static Color GetTabColor(BindableObject bindable) =>
            (Color)bindable.GetValue(TabColorProperty);
    }
}
