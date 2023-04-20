namespace Troocoo.Api.Helpers
{
    public static class Utility
    {
        public static bool IsNumberLessPositiveOrGreaterThanZero(decimal number)
        {
            return number > 0;
        }

        public static int GetFractionalPart(decimal number)
        {
            return (int)((number - Math.Truncate(number)) * 100);
        }

        public static int GetDecimalPart(decimal number)
        {
            return (int)Math.Truncate(number);
        }
    }
}
