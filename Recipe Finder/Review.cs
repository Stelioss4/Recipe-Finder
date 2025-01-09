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

        private DateTime _timeStamp;

        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }

        private User _profile;

        public virtual User Profile
        {
            get { return _profile; }
            set { _profile = value; }
        }

        // private Recipe _recipe;

        //public virtual Recipe Recipe
        //{
        //    get { return _recipe; }
        //    set { _recipe = value; }
        //}


        //private int _recipeId;

        //public int RecipeId
        //{
        //    get { return _recipeId; }
        //    set { _recipeId = value; }
        //}


        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
    }
}
