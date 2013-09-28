using INeedHelp.Client.Helpers;
using INeedHelp.Client.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ParseStarterProject.Services
{
    public class NavigationService
    {
        private static Type GetViewType(ViewType view)
        {
            switch (view)
            {
                case ViewType.Login:
                    return typeof(LoginView);
                case ViewType.Home:
                    return typeof(HomeView);
                case ViewType.Register:
                    return typeof(RegisterView);
                case ViewType.AddRequest:
                    return typeof(AddRequestView);
                case ViewType.RequestDetails:
                    return typeof(RequestDetailsView);
            }

            return null;
        }

        public static void Navigate(ViewType sourcePageType)
        {
            var pageType = GetViewType(sourcePageType);

            if (pageType != null)
            {

                ((Frame)Window.Current.Content).Navigate(pageType);
            }
        }

        public static void Navigate(ViewType sourcePageType, object parameter)
        {
            var pageType = GetViewType(sourcePageType);

            if (pageType != null)
            {
                ((Frame)Window.Current.Content).Navigate(pageType, parameter);
            }
        }

        public static void GoBack()
        {
            ((Frame)Window.Current.Content).GoBack();
        }
    }
}
