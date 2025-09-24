using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		private string _failedNode;

		public string FailedNode
		{
			get { return _failedNode; }
			set { _failedNode = value; }
		}

		private bool _IsSuccess;

		public bool IsSuccess
		{
			get { return _IsSuccess; }
			set { _IsSuccess = value; }
		}

	}
}
