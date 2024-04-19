using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Finder
{
    public class Review
    {
        private string _reviewText;

        public string ReviewText
        {
            get { return _reviewText; }
            set { _reviewText = value; }
        }

        private DateTime _timeStam;

        public DateTime TimeStam
        {
            get { return _timeStam; }
            set { _timeStam = value; }
        }

        private UsersProfile _profile;

        public UsersProfile Profile
        {
            get { return _profile; }
            set { _profile = value; }
        }
    }
}
