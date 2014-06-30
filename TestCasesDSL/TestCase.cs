using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCasesDSL
{
    public class TestCase
    {
        public string Nazev { get; set; }
        public IEnumerable<WebTest> WebTests { get; set; }
      

    }
}
