using System.Collections.Concurrent;
using System.Globalization;

namespace PalindromeNumberFiltering
{
    /// <summary>
    /// A static class containing methods for filtering palindrome numbers from a collection of integers.
    /// </summary>
    public static class Selector
    {
        /// <summary>
        /// Retrieves a collection of palindrome numbers from the given list of integers using sequential filtering.
        /// </summary>
        /// <param name="numbers">The list of integers to filter.</param>
        /// <returns>A collection of palindrome numbers.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the input list 'numbers' is null.</exception>
        public static IList<int> GetPalindromeInSequence(IList<int>? numbers)
        {
            ArgumentNullException.ThrowIfNull(numbers);

            var palindromes = new List<int>();

            foreach (int number in numbers)
            {
                if (IsPalindrome(number))
                {
                    palindromes.Add(number);
                }
            }

            return palindromes;
        }

        /// <summary>
        /// Retrieves a collection of palindrome numbers from the given list of integers using parallel filtering.
        /// </summary>
        /// <param name="numbers">The list of integers to filter.</param>
        /// <returns>A collection of palindrome numbers.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the input list 'numbers' is null.</exception>
        public static IList<int> GetPalindromeInParallel(IList<int> numbers)
        {
            ArgumentNullException.ThrowIfNull(numbers);

            var palindromes = new ConcurrentBag<int>();

            _ = Parallel.ForEach(numbers, number =>
            {
                if (IsPalindrome(number))
                {
                    palindromes.Add(number);
                }
            });

            return [.. palindromes];
        }

        /// <summary>
        /// Checks whether the given integer is a palindrome number.
        /// </summary>
        /// <param name="number">The integer to check.</param>
        /// <returns>True if the number is a palindrome, otherwise false.</returns>
        private static bool IsPalindrome(int number)
        {
            if (number < 0)
            {
                return false;
            }

            int length = GetLength(number);
            for (int i = 0; i <= length - i; i++)
            {
                if (GetDigitInDecimalPlace(number, i) != GetDigitInDecimalPlace(number, length - (i + 1)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Recursively checks whether a positive number is a palindrome.
        /// </summary>
        /// <param name="number">The positive number to check.</param>
        /// <param name="divider">The divider used in the recursive check.</param>
        /// <returns>True if the positive number is a palindrome, otherwise false.</returns>
        private static bool IsPositiveNumberPalindrome(int number, int divider)
        {
            if (number <= 0)
            {
                return false;
            }

            int leftDigit = number / divider;
            int rightDigit = number % 10;

            if (leftDigit != rightDigit)
            {
                return false;
            }

            number = (number % divider) / 10;

            divider /= 100;

            return number == 0 || IsPositiveNumberPalindrome(number, divider);
        }

        /// <summary>
        /// Gets the number of digits in the given integer.
        /// </summary>
        /// <param name="number">The integer to count digits for.</param>
        /// <returns>The number of digits in the integer.</returns>
        private static byte GetLength(int number)
        {
            if (number == 0)
            {
                return 1;
            }

            number = Math.Abs(number);
            byte length = 0;

            while (number > 0)
            {
                number /= 10;
                length++;
            }

            return length;
        }

        private static int GetDigitInDecimalPlace(int number, int decimalPlace)
        {
            string numberStr = number.ToString(CultureInfo.InvariantCulture);

            return numberStr[decimalPlace];
        }
    }
}
