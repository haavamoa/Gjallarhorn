using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Gjallarhorn.Client.UWP.Helpers
{
    public static class PropertyChangedExtensions
    {
        public static void Raise(this PropertyChangedEventHandler propertyChanged, [CallerMemberName] string propertyName = "", object sender = null)
        {
            propertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }

        public static bool RaiseWhenSet<S>(this PropertyChangedEventHandler propertyChanged, ref S backingStore, S value, object sender = null, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<S>.Default.Equals(backingStore, value))
                return false;
            backingStore = value;
            propertyChanged?.Raise(propertyName, sender);
            return true;
        }
    }
}