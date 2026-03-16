namespace Recipe_Finder
{
    public class ScrapeCheckResult
    {
        private string _recipeURL;

        public string RecipeUrl
        {
            get { return _recipeURL; }
            set { _recipeURL = value; }
        }

        private string _recipeName;

        public string RecipeName
        {
            get { return _recipeName; }
            set { _recipeName = value; }
        }

        private string _checkedNode;

        public string CheckedNode
        {
            get { return _checkedNode; }
            set { _checkedNode = value; }
        }

        private bool _isSuccess;

        public bool IsSuccess
        {
            get { return _isSuccess; }
            set { _isSuccess = value; }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private string _extractedValue;

        public string ExtractedValue
        {
            get { return _extractedValue; }
            set { _extractedValue = value; }
        }
    }
}