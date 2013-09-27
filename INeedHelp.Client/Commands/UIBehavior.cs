using System;
using System.Windows.Input;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace INeedHelp.Client.Commands
{
    public static class UIBehavior
    {
        public static ICommand GetLoaded(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(LoadedProperty);
        }

        public static void SetLoaded(DependencyObject obj, ICommand value)
        {
            obj.SetValue(LoadedProperty, value);
        }

        public static readonly DependencyProperty LoadedProperty =
            DependencyProperty.RegisterAttached("Loaded", typeof(ICommand), typeof(UIBehavior), 
            new PropertyMetadata(null, new PropertyChangedCallback(ExecuteLoadedCommand)));

        private static void ExecuteLoadedCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var page = d as Page;
            var command = (d as UIElement).GetValue(UIBehavior.LoadedProperty) as ICommand;
            page.Loaded += ((s, ev) => command.Execute(null));
        }
    }
}