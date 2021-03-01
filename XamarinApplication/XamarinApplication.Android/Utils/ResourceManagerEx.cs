using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XamarinApplication.Droid.Utils
{
    internal static class ResourceManagerEx
    {
        internal static int IdFromTitle(string title, Type type)
        {
            string name = Path.GetFileNameWithoutExtension(title);
            int id = GetId(type, name);
            return id;
        }

        private static int GetId(Type type, string propertyName)
        {
            FieldInfo[] props = type.GetFields();
            FieldInfo prop = props.Select(p => p).FirstOrDefault(p => p.Name == propertyName);
            if (prop != null)
                return (int)prop.GetValue(type);
            return 0;
        }
    }
}