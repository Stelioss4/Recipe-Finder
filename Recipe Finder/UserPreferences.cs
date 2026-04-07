namespace Recipe_Finder
{
    public class UserPreferences
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _userId;
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public virtual User User { get; set; }

        private int? _maxCaloriesPerRecipe;
        public int? MaxCaloriesPerRecipe
        {
            get { return _maxCaloriesPerRecipe; }
            set { _maxCaloriesPerRecipe = value; }
        }

        private int? _maxPreparationTimeInMinutes;
        public int? MaxPreparationTimeInMinutes
        {
            get { return _maxPreparationTimeInMinutes; }
            set { _maxPreparationTimeInMinutes = value; }
        }

        private int? _preferredFavoriteRecipesPerWeek;
        public int? PreferredFavoriteRecipesPerWeek
        {
            get { return _preferredFavoriteRecipesPerWeek; }
            set { _preferredFavoriteRecipesPerWeek = value; }
        }

        private int? _weeklyPlanDays;
        public int? WeeklyPlanDays
        {
            get { return _weeklyPlanDays; }
            set { _weeklyPlanDays = value; }
        }
    }
}