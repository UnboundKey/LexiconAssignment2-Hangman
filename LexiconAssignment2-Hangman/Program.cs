using System;
using System.Linq;
using System.Text;

namespace LexiconAssignment2_Hangman
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        private static readonly string[] Wordlist = {"same", "annanas",};
        private static string _pickedWord;
        private static StringBuilder _wrongLettersBuilder = new StringBuilder();
        private static char[] _correctChars;
        private static bool IsWon = false;

        static void Main(string[] args)
        {
            _pickedWord = PickWord(Wordlist);
            _correctChars = FillCharArray(_pickedWord);

            Console.WriteLine("I've picked a word, now try to guess it");
            while (!IsWon)
            {
                PrintCurrentLetters();
                var input = Console.ReadLine();
                ModeSelect(input);
                IsWon = CheckWin();
            }
        }

        public static void ModeSelect(string input)
        {
            if (input != null && input.Length > 1)
            {
                WholeWordGuess(input);
            }
            else if (input != null && input.Length == 1)
            {
                SingleCharacterGuess(input, _pickedWord, _correctChars, _wrongLettersBuilder);
            }
        }

        private static bool CheckWin()
        {
            if (new string(_correctChars) == _pickedWord)
            {
                return true;
            }

            return false;
        }

        public static char[] FillCharArray(string pickedWord)
        {
            var outputArray = new char[pickedWord.Length];
            for (int i = 0; i < pickedWord.Length; i++)
            {
                outputArray[i] = '_';
            }

            return outputArray;
        }

        public static void SingleCharacterGuess(string input, string pickedword, char[] correctChars, StringBuilder sb)
        {
            // this is a very hacky way of doing it but it works
            string correctCharsBeforeCheck = new string(correctChars);
            if (CheckRepeatInput(input, correctChars, sb)) return;
            for (int index = 0; index < pickedword.Length; index++)
            {
                char letter = pickedword[index];
                if (input[0] == letter)
                {
                    correctChars[index] = letter;
                }
            }

            // if the string hasn't been updated then you've got a wrong letter. There fore append it
            if (correctCharsBeforeCheck == new string(correctChars))
            {
                sb.Append(input);
            }
        }

        public static bool CheckRepeatInput(string input, char[] correctChars, StringBuilder sb)
        {
            if (correctChars.Contains(input[0]) || sb.ToString().Contains(input[0]))
            {
                Console.WriteLine($"{input} has already been tried, please try another letter");
                return true;
            }

            return false;
        }

        private static void WholeWordGuess(string input)
        {
            if (input == _pickedWord)
            {
                _correctChars = input.ToCharArray();
            }
        }

        private static void PrintCurrentLetters()
        {
            Console.WriteLine(_correctChars);
            Console.WriteLine("Wrong Letters: {0}", _wrongLettersBuilder);
        }

        public static string PickWord(string[] wordStrings)
        {
            Random r = new Random();
            int randSelection = r.Next(0, wordStrings.Length);
            _pickedWord = wordStrings[randSelection];
            return _pickedWord;
        }
    }
}