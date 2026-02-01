namespace RecipeFinderTest.Unit
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {


        }
    }
    public class StringNormalizationTests
    {
        [Fact]
        public void NormalizeSearchQuery_Trims_And_Lowercases()
        {
            var input = "  ChIcKeN  ";
            var result = input.Trim().ToLowerInvariant();

            Assert.Equal("chicken", result);
        }
    }

}