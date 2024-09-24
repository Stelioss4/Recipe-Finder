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

        private User _profile;

        public User Profile
        {
            get { return _profile; }
            set { _profile = value; }
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
    }
}
