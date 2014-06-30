using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestCasesDSL.HtmlConvertor
{
    public class Convertor
    {
        private string _baseUrl;
        public HtmlTestCase Load(string content)
        {
            int indexStart = content.IndexOf("<table");
            int indexEnd = content.IndexOf("</table");

            string title = GetTitle(content);
            _baseUrl = GetBaseUrl(content);

            

            string table = content.Substring(indexStart, indexEnd - indexStart + 8);
            XmlDocument document = new XmlDocument();
            document.LoadXml(table);
            XmlNodeList list = document.SelectNodes("//tr");

            List<HtmlControlCommand> retList = new List<HtmlControlCommand>();
            bool isFirst = true;
            foreach (XmlNode node in list)
            {
                if (isFirst) //todo nějaký popis
                {
                    isFirst = false;
                    continue;
                }

                HtmlControlCommand controlCommand = ParseTD(node);
                retList.Add(controlCommand);
            }
            return new HtmlTestCase(retList, title, _baseUrl);
        }

        private string GetBaseUrl(string content)
        {
            //<link rel="selenium.base" href="https://cedr-fm.mfcr.cz/cedrnf-test/" />

            int indexStart = content.IndexOf("<link rel=\"selenium.base\" href=\"");
            int indexEnd = content.IndexOf("\"", indexStart+35);

            return content.Substring(indexStart+32, indexEnd - indexStart-32);
        }

        private string GetTitle(string content)
        {
            int indexStart = content.IndexOf("<title>");
            int indexEnd = content.IndexOf("</title>");

            return content.Substring(indexStart+7, indexEnd - indexStart-7);
        }

        private HtmlControlCommand ParseTD(XmlNode node)
        {
            XmlNode node1 = node.ChildNodes[0];
            XmlNode node2 = node.ChildNodes[1];
            XmlNode node3 = node.ChildNodes[2];
            HtmlControlCommand command = new HtmlControlCommand(node1.InnerText, node2.InnerText);

            if (command.Action == "open")
                command.WebObject = _baseUrl + command.WebObject;

            if (string.IsNullOrEmpty(node3.InnerText) == false)
                command.Argument = node3.InnerText;

            return command;
        }
    }
}