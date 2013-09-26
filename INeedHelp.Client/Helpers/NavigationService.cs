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
        private Type GetViewType(ViewType view)
        {
            switch (view)
            {
                case ViewType.Login:
                    return typeof(LoginView);
                case ViewType.Home:
                    return typeof(HomeView);
            }

            return null;
        }

        public void Navigate(ViewType sourcePageType)
        {
            var pageType = this.GetViewType(sourcePageType);

            if (pageType != null)
            {

                ((Frame)Window.Current.Content).Navigate(pageType);
            }
        }

        public void Navigate(ViewType sourcePageType, object parameter)
        {
            var pageType = this.GetViewType(sourcePageType);

            if (pageType != null)
            {
                ((Frame)Window.Current.Content).Navigate(pageType, parameter);
            }
        }

        public void GoBack()
        {
            ((Frame)Window.Current.Content).GoBack();
        }
    }
}
