using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCasesDSL.HtmlConvertor
{
    public class HtmlTestCase
    {
        public List<HtmlControlCommand> Commands { get; private set; }
        public string Title { get; set; }
        public string BaseUrl { get; set; }
        public HtmlTestCase(List<HtmlControlCommand> commands, string title, string baseUrl)
        {
            Commands = commands;
            BaseUrl = baseUrl;
            Title = title;
        }
    }
}
