using System;
using System.Xml;
using System.Linq;
using System.IO;

namespace Xutils.ConsoleApp
{
    /// <summary>
    /// Contains helper methods to write on the console using colors based on special tags in the text contents.
    /// </summary>
    public class ColoredConsole : IColoredConsole
    {
        public TextWriter Out => Console.Out;

        public TextWriter Error => Console.Error;

        public TextReader In => Console.In;

        public bool IsInputRedirected => Console.IsInputRedirected;

        public bool IsOutputRedirected => Console.IsOutputRedirected;

        public bool IsErrorRedirected => Console.IsErrorRedirected;

        public ConsoleColor ForegroundColor { get => Console.ForegroundColor; set => Console.ForegroundColor = value; }
        public ConsoleColor BackgroundColor { get => Console.BackgroundColor; set => Console.BackgroundColor = value; }

        public event ConsoleCancelEventHandler CancelKeyPress
        {
            add => Console.CancelKeyPress += value;
            remove => Console.CancelKeyPress -= value;
        }

        private void SetForeColor(string colorName)
        {
            switch (colorName.ToLowerInvariant())
            {
                case "b":
                case "blue":
                    ForegroundColor = ConsoleColor.Blue;
                    break;

                case "r":
                case "red":
                    ForegroundColor = ConsoleColor.Red;
                    break;

                case "n":
                case "green":
                    ForegroundColor = ConsoleColor.Green;
                    break;

                case "w":
                case "white":
                    ForegroundColor = ConsoleColor.White;
                    break;

                case "y":
                case "yellow":
                    ForegroundColor = ConsoleColor.Yellow;
                    break;

                case "m":
                case "magenta":
                    ForegroundColor = ConsoleColor.Magenta;
                    break;

                case "c":
                case "cyan":
                    ForegroundColor = ConsoleColor.Cyan;
                    break;

                case "g":
                case "gray":
                    ForegroundColor = ConsoleColor.Gray;
                    break;

                case "db":
                case "darkblue":
                    ForegroundColor = ConsoleColor.DarkBlue;
                    break;

                case "dr":
                case "darkred":
                    ForegroundColor = ConsoleColor.DarkRed;
                    break;

                case "dn":
                case "darkgreen":
                    ForegroundColor = ConsoleColor.DarkGreen;
                    break;

                case "dy":
                case "darkyellow":
                    ForegroundColor = ConsoleColor.DarkYellow;
                    break;

                case "dm":
                case "darkmagenta":
                    ForegroundColor = ConsoleColor.DarkMagenta;
                    break;

                case "dc":
                case "darkcyan":
                    ForegroundColor = ConsoleColor.DarkCyan;
                    break;

                case "dg":
                case "darkgrey":
                    ForegroundColor = ConsoleColor.DarkGray;
                    break;

                case "k":
                case "black":
                    ForegroundColor = ConsoleColor.Black;
                    break;
            }
        }

        private void SetBackColor(string colorName)
        {
            switch (colorName.ToLowerInvariant())
            {
                case "b":
                case "blue":
                    BackgroundColor = ConsoleColor.Blue;
                    break;

                case "r":
                case "red":
                    BackgroundColor = ConsoleColor.Red;
                    break;

                case "n":
                case "green":
                    BackgroundColor = ConsoleColor.Green;
                    break;

                case "w":
                case "white":
                    BackgroundColor = ConsoleColor.White;
                    break;

                case "y":
                case "yellow":
                    BackgroundColor = ConsoleColor.Yellow;
                    break;

                case "m":
                case "magenta":
                    BackgroundColor = ConsoleColor.Magenta;
                    break;

                case "c":
                case "cyan":
                    BackgroundColor = ConsoleColor.Cyan;
                    break;

                case "g":
                case "gray":
                    BackgroundColor = ConsoleColor.Gray;
                    break;

                case "db":
                case "darkblue":
                    BackgroundColor = ConsoleColor.DarkBlue;
                    break;

                case "dr":
                case "darkred":
                    BackgroundColor = ConsoleColor.DarkRed;
                    break;

                case "dn":
                case "darkgreen":
                    BackgroundColor = ConsoleColor.DarkGreen;
                    break;

                case "dy":
                case "darkyellow":
                    BackgroundColor = ConsoleColor.DarkYellow;
                    break;

                case "dm":
                case "darkmagenta":
                    BackgroundColor = ConsoleColor.DarkMagenta;
                    break;

                case "dc":
                case "darkcyan":
                    BackgroundColor = ConsoleColor.DarkCyan;
                    break;

                case "dg":
                case "darkgrey":
                    BackgroundColor = ConsoleColor.DarkGray;
                    break;

                case "k":
                case "black":
                    BackgroundColor = ConsoleColor.Black;
                    break;
            }
        }

        /// <summary>
        /// Writes the given <paramref name="text"/> on the Console, using colors provided by tags in the text, followed by a new line.
        /// </summary>
        /// <param name="text">Text to be written on the Console, optionally contaning tags for defining foreground and background colors, and numeric placeholders for replacing with the <paramref name="args"/>.</param>
        /// <param name="args">Additional values which can be replaced in the given <paramref name="text"/> for the equivalent placeholders.</param>
        public void ColoredWriteLine(string text, params string[] args)
        {
            ColoredWrite($"{text}\r\n", args);
            Console.WriteLine();
        }

        /// <summary>
        /// Writes the given <paramref name="text"/> on the Console, using colors provided by tags in the text.
        /// </summary>
        /// <param name="text">Text to be written on the Console, optionally contaning tags for defining foreground and background colors, and numeric placeholders for replacing with the <paramref name="args"/>.</param>
        /// <param name="args">Additional values which can be replaced in the given <paramref name="text"/> for the equivalent placeholders.</param>
        public void ColoredWrite(string text, params string[] args)
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
                    var currentForeColor = ForegroundColor;
                    var currentBackColor = BackgroundColor;
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
                    ColoredWrite(fragment.InnerXml);
                    BackgroundColor = currentBackColor;
                    ForegroundColor = currentForeColor;
                }
                else
                {
                    Console.Write(fragment.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">"));
                }
            }
        }

        public void ResetColor() => Console.ResetColor();

        private static Lazy<ColoredConsole> singletonColoredConsole = new Lazy<ColoredConsole>();

        public static ColoredConsole Singleton { get => singletonColoredConsole.Value; }
    }
}
