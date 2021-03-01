using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace XamarinApplication.Droid.Utils
{
    public class PageController : IPageController
    {
        private readonly global::BottomBar.Droid.ReflectedProxy<Page> _proxy;

        public static IPageController Create(Page page) => new PageController(page);

        private PageController(Page page)
        {
            _proxy = new global::BottomBar.Droid.ReflectedProxy<Page>(page);
        }

        public Rectangle ContainerArea
        {
            get => _proxy.GetPropertyValue<Rectangle>();

            set => _proxy.SetPropertyValue(value);
        }

        public bool IgnoresContainerArea
        {
            get => _proxy.GetPropertyValue<bool>();

            set => _proxy.SetPropertyValue(value);
        }

        public ObservableCollection<Element> InternalChildren
        {
            get => _proxy.GetPropertyValue<ObservableCollection<Element>>();

            set => _proxy.SetPropertyValue(value);
        }

        public void SendAppearing()
        {
            _proxy.Call();
        }

        public void SendDisappearing()
        {
            _proxy.Call();
        }
    }
}