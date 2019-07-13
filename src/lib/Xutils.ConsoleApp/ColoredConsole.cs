using System;
using System.Xml;
using System.Linq;

namespace Xutils.ConsoleApp
{
    /// <summary>
    /// Contains helper methods to write on the console using colors based on special tags in the text contents.
    /// </summary>
    public static class ColoredConsole
    {
        private static void SetForeColor(string colorName)
        {
            switch (colorName.ToLowerInvariant())
            {
                case "b":
                case "blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;

                case "r":
                case "red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case "n":
                case "green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case "w":
                case "white":
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case "y":
                case "yellow":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case "m":
                case "magenta":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;

                case "c":
                case "cyan":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case "g":
                case "gray":
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;

                case "db":
                case "darkblue":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;

                case "dr":
                case "darkred":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;

                case "dn":
                case "darkgreen":
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;

                case "dy":
                case "darkyellow":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;

                case "dm":
                case "darkmagenta":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;

                case "dc":
                case "darkcyan":
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;

                case "dg":
                case "darkgrey":
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;

                case "k":
                case "black":
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
            }
        }

        private static void SetBackColor(string colorName)
        {
            switch (colorName.ToLowerInvariant())
            {
                case "b":
                case "blue":
                    Console.BackgroundColor = ConsoleColor.Blue;
                    break;

                case "r":
                case "red":
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;

                case "n":
                case "green":
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;

                case "w":
                case "white":
                    Console.BackgroundColor = ConsoleColor.White;
                    break;

                case "y":
                case "yellow":
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    break;

                case "m":
                case "magenta":
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    break;

                case "c":
                case "cyan":
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    break;

                case "g":
                case "gray":
                    Console.BackgroundColor = ConsoleColor.Gray;
                    break;

                case "db":
                case "darkblue":
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;

                case "dr":
                case "darkred":
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;

                case "dn":
                case "darkgreen":
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;

                case "dy":
                case "darkyellow":
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;

                case "dm":
                case "darkmagenta":
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    break;

                case "dc":
                case "darkcyan":
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    break;

                case "dg":
                case "darkgrey":
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    break;

                case "k":
                case "black":
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
            }
        }

        /// <summary>
        /// Writes the given <paramref name="text"/> on the Console, using colors provided by tags in the text, followed by a new line.
        /// </summary>
        /// <param name="text">Text to be written on the Console, optionally contaning tags for defining foreground and background colors, and numeric placeholders for replacing with the <paramref name="args"/>.</param>
        /// <param name="args">Additional values which can be replaced in the given <paramref name="text"/> for the equivalent placeholders.</param>
        public static void WriteLine(string text, params string[] args)
        {
            Write($"{text}\r\n", args);
            Console.WriteLine();
        }

        /// <summary>
        /// Writes the given <paramref name="text"/> on the Console, using colors provided by tags in the text.
        /// </summary>
        /// <param name="text">Text to be written on the Console, optionally contaning tags for defining foreground and background colors, and numeric placeholders for replacing with the <paramref name="args"/>.</param>
        /// <param name="args">Additional values which can be replaced in the given <paramref name="text"/> for the equivalent placeholders.</param>
        public static void Write(string text, params string[] args)
        {
            for (int i = 0, l = args.Length; i < l; i++)
                text = text.Replace($"{{{i}}}", args[i]);

            string xml = "<ROOT>" + text + "</ROOT>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            var xmlFragments = from XmlNode node in doc.FirstChild.ChildNodes
                               where node.NodeType == XmlNodeType.Element ||
                                     node.NodeType == XmlNodeType.Text
                               select node;
            foreach (var fragment in xmlFragments)
            {
                if (fragment.NodeType == XmlNodeType.Element)
                {
                    var currentForeColor = Console.ForegroundColor;
                    var currentBackColor = Console.BackgroundColor;
                    var name = fragment.Name.ToLowerInvariant();
                    switch (name)
                    {
                        case "fg":
                        case "fore":
                        case "foreground":
                            SetForeColor(fragment.Attributes["color"]?.Value ?? fragment.Attributes["c"]?.Value ?? currentForeColor.ToString());
                            break;

                        case "bg":
                        case "back":
                        case "background":
                            SetBackColor(fragment.Attributes["color"]?.Value ?? fragment.Attributes["c"]?.Value ?? currentBackColor.ToString());
                            break;

                        default:
                            SetForeColor(fragment.Name);
                            break;
                    }
                    Write(fragment.InnerXml);
                    Console.BackgroundColor = currentBackColor;
                    Console.ForegroundColor = currentForeColor;
                }
                else
                {
                    Console.Write(fragment.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">"));
                }
            }
        }
    }
}
