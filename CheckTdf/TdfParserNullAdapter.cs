namespace CheckTdf
{
    using System;
    using System.Collections.Generic;

    using TAUtil.Tdf;

    public class TdfParserNullAdapter : ITdfNodeAdapter
    {
        private readonly string filename;

        private TdfParser parser;

        private Stack<HashSet<string>> blockNames;

        private Stack<HashSet<string>> propertyNames;

        public TdfParserNullAdapter(string filename)
        {
            this.filename = filename;
        }

        public bool WarningsFound { get; private set; }

        public void Initialize(TdfParser parser)
        {
            this.parser = parser;
            this.blockNames = new Stack<HashSet<string>>();
            this.propertyNames = new Stack<HashSet<string>>();

            this.PushStacks();
        }

        public void BeginBlock(string name)
        {
            var success = this.blockNames.Peek().Add(name);
            if (!success)
            {
                this.Warning(
                    "Block '{0}' already defined",
                    name,
                    this.parser.CurrentLine,
                    this.parser.CurrentColumn);
            }

            this.PushStacks();
        }

        public void AddProperty(string name, string value)
        {
            name = name.Trim();

            var success = this.propertyNames.Peek().Add(name);
            if (!success)
            {
                this.Warning(
                    "Property '{0}' already defined",
                    name,
                    this.parser.CurrentLine,
                    this.parser.CurrentColumn);
            }

            if (value.Contains("="))
            {
                this.Warning(
                    "Property '{0}' contains '=' (is the previous property missing a semicolon?)",
                    name,
                    this.parser.CurrentLine,
                    this.parser.CurrentColumn);
            }
        }

        public void EndBlock()
        {
            this.blockNames.Pop();
            this.propertyNames.Pop();
        }

        private void Warning(string warn, params object[] objs)
        {
            var msg = string.Format(warn, objs);
            Console.WriteLine(
               "{0}:{2}: Warning: {1}",
                this.filename,
                msg,
                this.parser.CurrentLine);

            this.WarningsFound = true;
        }

        private void PushStacks()
        {
            this.blockNames.Push(new HashSet<string>(StringComparer.Ordinal));
            this.propertyNames.Push(new HashSet<string>(StringComparer.OrdinalIgnoreCase));
        }
    }
}
