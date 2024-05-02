namespace Recipe_Finder
{
    public class UIMethods
    {
        public static void RatingAndReview()
        {
            Console.WriteLine("What did you think of the recipe?");
            Console.Write("Rating from 1 to 5: ");

            Rating rating = new Rating();
            double ratingValue;

            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out ratingValue))
                {
                    if (ratingValue >= 1 && ratingValue <= 5)
                    {
                        rating.Value = ratingValue;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                        Console.WriteLine("Please enter a rating between 1 and 5.");
                    }
                }
            }

            Console.WriteLine("Please write us your opinion : ");

            Review review = new Review();

            review.ReviewText = Console.ReadLine();
        }
    }
}
