using McMaster.Extensions.CommandLineUtils;

namespace Xutils.ConsoleApp
{
    public interface IColoredConsole : IConsole
    {
        /// <summary>
        /// Writes the given <paramref name="text"/> on the Console, using colors provided by tags in the text, followed by a new line.
        /// </summary>
        /// <param name="text">Text to be written on the Console, optionally contaning tags for defining foreground and background colors, and numeric placeholders for replacing with the <paramref name="args"/>.</param>
        /// <param name="args">Additional values which can be replaced in the given <paramref name="text"/> for the equivalent placeholders.</param>
        void ColoredWriteLine(string text, params string[] args);

        /// <summary>
        /// Writes the given <paramref name="text"/> on the Console, using colors provided by tags in the text.
        /// </summary>
        /// <param name="text">Text to be written on the Console, optionally contaning tags for defining foreground and background colors, and numeric placeholders for replacing with the <paramref name="args"/>.</param>
        /// <param name="args">Additional values which can be replaced in the given <paramref name="text"/> for the equivalent placeholders.</param>
        void ColoredWrite(string text, params string[] args);
    }
}
