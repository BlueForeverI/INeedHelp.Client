using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace INeedHelp.Client.Helpers
{
    public class NotificationsManager
    {
        public static void ShowNotification(string text)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);

            var element = template.GetElementsByTagName("text")[0];
            element.AppendChild(template.CreateTextNode(text));

            var toast = new ToastNotification(template);
            notifier.Show(toast);  
        }
    }
}
