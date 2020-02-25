using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Gjallarhorn.Clients.WPF.Extenstions
{
    public static class UIElementExtensions
    {
        public static Task<bool> FadeTo(this UIElement element, double to, double length = 250)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            var animation = new DoubleAnimation()
            {
                From = element.Opacity,
                To = to,
                FillBehavior = FillBehavior.HoldEnd,
                BeginTime = TimeSpan.FromSeconds(0),
                Duration = new Duration(TimeSpan.FromMilliseconds(length))
            };
            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
            storyboard.Completed += (o, e) => taskCompletionSource.SetResult(true);
            storyboard.Begin();
            return taskCompletionSource.Task;
        }
    }
}