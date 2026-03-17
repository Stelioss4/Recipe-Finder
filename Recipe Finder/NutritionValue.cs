using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Finder
{
    public class NutritionValue
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private int recipeId;

        public int RecipeId
        {
            get { return recipeId; }
            set { recipeId = value; }
        }

        private double? calories;

        public double? Calories
        {
            get { return calories; }
            set { calories = value; }
        }

        private double? protein;

        public double? Protein
        {
            get { return protein; }
            set { protein = value; }
        }

        private double? fat;

        public double? Fat
        {
            get { return fat; }
            set { fat = value; }
        }

        private double? carbohydrates;

        public double? Carbohydrates
        {
            get { return carbohydrates; }
            set { carbohydrates = value; }
        }

        private string rawText;

        public string RawText
        {
            get { return rawText; }
            set { rawText = value; }
        }

        private Recipe recipe;

        public virtual Recipe Recipe
        {
            get { return recipe; }
            set { recipe = value; }
        }
    }
}
