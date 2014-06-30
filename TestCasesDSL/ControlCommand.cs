namespace TestCasesDSL
{
    public class ControlCommand
    {
        public string DomObject { get; set; }
        public string Argument { get; set; }
        public ControlCommand(string domObject, string argument)
        {
            Argument = argument;
            DomObject = domObject;
        }
    }


    public class CheckControlCommand : ControlCommand
    {
        public CheckControlCommand(string domObject, string argument)
            : base(domObject, argument)
        { }
    }


    public class HtmlControlCommand
    {
        public string Action { get; set; }
        public string WebObject { get; set; }
        public string Argument { get; set; }
        public HtmlControlCommand(string action, string selector)
        {
            Action = action;
            WebObject = selector;
        }

        public string GeneratedTitle
        {
            get
            {
                string argument;
                if (string.IsNullOrEmpty(Argument) == false)
                    argument = ":" + Argument;
                else
                    argument = string.Empty;

                return Action + ":" + WebObject + argument;
            }
        }
    }
}
