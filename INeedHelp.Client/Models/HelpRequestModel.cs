using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INeedHelp.Client.Models
{
    public class HelpRequestModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string PictureUrl { get; set; }
        public bool Solved { get; set; }
        public CoordinatesModel Coordinates { get; set; }

        public UserModel User { get; set; }
        public IEnumerable<UserModel> Helpers { get; set; }
        public IEnumerable<CommentModel> Comments { get; set; }
    }
}
