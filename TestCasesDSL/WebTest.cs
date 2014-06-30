using System.Collections.Generic;

namespace TestCasesDSL
{
    public class WebTest
    {
        public string TestCase { get; set; }
        public string Nazev { get; set; }
        public string Url { get; set; }

        public IEnumerable<ControlCommand> ControlCommand { get; set; }


        public IEnumerable<ControlCommand> CheckCommand { get; set; }
    }
}
