
namespace LoadTestTool
{
    public partial class Scenario
    {
        public Dictionary<string, string> Context { get; set; }
        public Scenario()
        {
            Context = new Dictionary<string, string>();
        }

        public async Task SampleScenario()
        {
            await Task.Delay(1000);
        }
    }
}
