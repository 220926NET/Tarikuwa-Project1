using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Expense.UI
{
    public class Validators
    {
        private static int numberAttempt;
        private static readonly int MAX_ATTEMPTS = 5;

        private static void ErrorMessage(int attempts)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"You have {MAX_ATTEMPTS - attempts} number of attempts left.");
            Console.ResetColor();
        }

        public static string GetInput(string prompt)
        {
            Console.Write($"Please {prompt}: ");
            return Console.ReadLine();
        }

        public static void DisplayMessage(bool check, string message)
        {
            if (!check)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static T Convert<T>(string prompt)
        {
            var userInput = GetInput(prompt);

            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                return (T)converter.ConvertFromString(userInput);
            }
            catch
            {
                DisplayMessage(false, "Invalid entry. Please try again.");
            }

            throw new ArgumentException("Wrong input format");
        }

        public static string HidePassword(string prompt)
        {
            var hide = "#";
            var password = "";
            Console.Write($"Please {prompt}: ");

            ConsoleKeyInfo inputKeyInfo;

            do
            {
                inputKeyInfo = Console.ReadKey(true);

                if (inputKeyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    var pos = Console.CursorLeft;
                    // move the cursor to the left by one character
                    Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    // replace it with space
                    Console.Write(" ");
                    // move the cursor to the left by one character again
                    Console.SetCursorPosition(pos - 1, Console.CursorTop);
                }
                else if (inputKeyInfo.Key != ConsoleKey.Backspace && inputKeyInfo.Key != ConsoleKey.Enter)
                {
                    password += inputKeyInfo.KeyChar;
                    Console.Write(hide);
                }
            } while (inputKeyInfo.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        public static bool Verifypassword(string p, string pp)
        {
            if (p != pp || string.IsNullOrEmpty(p) || string.IsNullOrEmpty(pp))
            {
                DisplayMessage(false, "Password don't match");
                return false;
            }

            return true;
        }

        public static bool ValidEmail(string r)
        {
            var motif = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            var check = false;
            if (r != null) check = Regex.IsMatch(r, motif);
            if (!check)
                DisplayMessage(false, "Please enter correct email.");
            return check;
        }

        public static bool IsPhone(string number)
        {
            var motif = @"^\(?([0-9]{3})\)?[-]?([0-9]{3})[-]?([0-9]{4})$";
            if (number != null)
                return Regex.IsMatch(number, motif);
            DisplayMessage(false, "phone is invalid\nThe correct format is xxx-xxx-xxxx.");
            return false;
        }

        public static void PressEnterToContinue()
        {
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}