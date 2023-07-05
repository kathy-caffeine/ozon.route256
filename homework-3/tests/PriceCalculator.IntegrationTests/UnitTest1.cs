using Xunit.Abstractions;

namespace PriceCalculator.IntegrationTests
{
    public class UnitTest1
    {

        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTest1(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Test1()
        {
            var app = new AppFixture();

            var httpclient = app.CreateClient();
            var responce = await httpclient.GetAsync("/swagger/index.html");

            responce.EnsureSuccessStatusCode();
            _testOutputHelper.WriteLine(await responce.Content.ReadAsStringAsync());
        }
    }
}