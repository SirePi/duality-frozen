// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SnowyPeak.Duality.Plugin.Frozen.HTML.Dom;
using SnowyPeak.Duality.Plugin.Frozen.HTML.Css;

namespace SnowyPeak.Duality.Plugin.Frozen.HTML
{
    /// <summary>
    /// Defines a Duality core plugin.
    /// </summary>
    public class HtmlParser : Parser
    {
        private Node _htmlTree { get; set; }

        public Node ParseHtml(string html)
        {
            Parse(html);
            return _htmlTree;
        }

        private Node ParseNode()
        {
            Node result;

            if(NextChar == '<')
            {
                result = ParseElement();
            }
            else
            {
                result = ParseText();
            }

            return result;
        }

        private Text ParseText()
        {
            return new Text()
            {
                Value = ConsumeWhile(new Func<char, bool>(delegate(char c)
                    {
                        return c != '<';
                    }))
            };
        }

        private Element ParseElement()
        {
            Assert(new Func<bool>(delegate() { return ConsumeChar() == '<'; }));

            string tagName = ParseName();
            IEnumerable<KeyValuePair<string, string>> attributes = ParseAttributes();

            Assert(new Func<bool>(delegate() { return ConsumeChar() == '>'; }));

            IEnumerable<Node> children = ParseNodes();

            Assert(new Func<bool>(delegate() { return ConsumeChar() == '<'; }));
            Assert(new Func<bool>(delegate() { return ConsumeChar() == '/'; }));
            Assert(new Func<bool>(delegate() { return ParseName().Equals(tagName); }));
            Assert(new Func<bool>(delegate() { return ConsumeChar() == '>'; }));

            Element result = new Element() { TagName = tagName };
            result.Attributes.AddRange(attributes);
            result.Children.AddRange(children);

            return result;
        }

        private KeyValuePair<string, string> ParseAttribute()
        {
            string name = ParseName();

            Assert(new Func<bool>(delegate() { return ConsumeChar() == '='; }));

            string value = ParseAttributeValue();

            return new KeyValuePair<string, string>(name, value);
        }

        private string ParseAttributeValue()
        {
            char openQuote = ConsumeChar();

            Assert(new Func<bool>(delegate() { return openQuote == '"' || openQuote == '\''; }));

            string value = ConsumeWhile(new Func<char, bool>(delegate(char c)
                {
                    return c != openQuote;
                }));

            Assert(new Func<bool>(delegate() { return ConsumeChar() == openQuote; }));

            return value;
        }

        private IEnumerable<KeyValuePair<string, string>> ParseAttributes()
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            while(true)
            {
                ConsumeWhitespace();
                if(NextChar == '>')
                {
                    break;
                }

                result.Add(ParseAttribute());

            }

            return result;
        }

        private IEnumerable<Node> ParseNodes()
        {
            List<Node> result = new List<Node>();

            while (true)
            {
                ConsumeWhitespace();
                if (EOF || StartsWith("</"))
                {
                    break;
                }
                result.Add(ParseNode());
            }

            return result;
        }

        protected override void Parse(string input)
        {
            _input = input.Trim();
            _pos = 0;

            List<Node> tree = new List<Node>(ParseNodes());
            _htmlTree = null;

            if (tree.Count == 1)
            {
                _htmlTree = tree[0];
            }
            else
            {
                _htmlTree = new Element() { TagName = "HTML" };
                _htmlTree.Children.AddRange(tree);
            }
        }
    }
}