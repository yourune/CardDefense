namespace BG_Games.Card_Game_Core.Tools
{
    public static class StringExtensions
    {
        public static string RemoveWhitespaces(this string str)
        {
            return str.Replace(" ", string.Empty);
        }
    }
}