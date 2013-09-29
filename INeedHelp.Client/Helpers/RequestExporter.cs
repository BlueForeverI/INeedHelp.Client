using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INeedHelp.Client.Models;
using Windows.Storage;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp;

namespace INeedHelp.Client.Helpers
{
    public class RequestExporter
    {
        public async static Task ExportToHtml(HelpRequestModel request, StorageFile file)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html>\n");
            sb.Append("<html>\n");
            sb.Append("<head>\n<title>" + request.Title + "</title>\n</head>\n");
            sb.Append("<body>\n");
            sb.Append("<h1>" + request.Title + "</h1>\n");
            sb.Append("<img src=\"" + request.PictureUrl + "\" />\n");
            sb.Append("<p>" + request.Text + "</p>\n");
            sb.Append("</body>\n</html>");

            await FileIO.WriteTextAsync(file, sb.ToString());
        }
    }
}
