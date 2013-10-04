using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace INeedHelp.Client.Helpers
{
    public class NotificationsManager
    {
        private static BadgeUpdater badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();

        public static void ShowToastNotification(string text)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);

            var element = template.GetElementsByTagName("text")[0];
            element.AppendChild(template.CreateTextNode(text));

            var toast = new ToastNotification(template);
            notifier.Show(toast);  
        }

        public static void ShowTileNotification(string text)
        {
            var wideTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideImageAndText01);
            var imageXml = wideTileXml.GetElementsByTagName("image");
            imageXml.Item(0).Attributes.GetNamedItem("src").InnerText = "ms-appx:///Images/WideTile.png";

            var textXml = wideTileXml.GetElementsByTagName("text");
            textXml.Item(0).InnerText = text;

            var tileNotification = new TileNotification(wideTileXml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        public static void ShowBadgeNotification(int number)
        {
            var badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeGlyph);
            var badgeElement = (XmlElement)badgeXml.SelectSingleNode("/badge");
            badgeElement.SetAttribute("value", number.ToString());

            var badge = new BadgeNotification(badgeXml);
            badgeUpdater.Update(badge);
        }

        public static void ClearBadgeNotifications()
        {
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
        }
    }
}
