using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace LexiconAssignment2_Hangman
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
    public class Program
    {
        private static readonly string[] Wordlist = {"same", "antennas"};
        private static string _pickedWord;
        private static StringBuilder _wrongLettersBuilder = new StringBuilder();
        private static char[] _correctChars;
        private int maxAttempts;
        private int currentAtempt;

        static void Main()
        {
            _pickedWord = PickWord(Wordlist);
            _correctChars = FillCharArray(_pickedWord);

            Console.WriteLine("I've picked a word, now try to guess it");
            while (true)
            {
                PrintCurrentLetters();
                var input = Console.ReadLine();
                ModeSelect(input);
                //TODO Set up Win Screen
                //Todo Limit amount of attempts
                //TODO set up a lose condition
            }
        }

        private static void ModeSelect(string input)
        {
            if (input != null && input.Length > 1)
            {
                WholeWordGuess(input, _pickedWord,_correctChars);
            }
            else if (input != null && input.Length == 1)
            {
                SingleCharacterGuess(input, _pickedWord, _correctChars, _wrongLettersBuilder);
            }
        }

        public static string PickWord(string[] wordStrings)
        {
            Random r = new Random();
            int randSelection = r.Next(0, wordStrings.Length);
            _pickedWord = wordStrings[randSelection];
            return _pickedWord;
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

        public static bool CheckWin()
        {
            if (new string(_correctChars) == _pickedWord)
            {
                return true;
            }

            return false;
        }
        
        public static void SingleCharacterGuess(string input, string pickedword, char[] correctChars, StringBuilder sb)
        {
            // this is a very hacky way of doing it but it works
            string correctCharsBeforeCheck = new string(correctChars);

            if (DetectRepeatInput(input, correctChars, sb)) return;

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

        public static void WholeWordGuess(string input, string pickedWord, char[] correctChars)
        {
            if (input == pickedWord)
            {
                correctChars = input.ToCharArray();
            }
        }

        private static void PrintCurrentLetters()
        {
            Console.WriteLine(_correctChars);
            Console.WriteLine($"Wrong Letters {_wrongLettersBuilder}");
        }

        public static bool DetectRepeatInput(string input, char[] correctChars, StringBuilder sb)
        {
            if (correctChars.Contains(input[0]) || sb.ToString().Contains(input[0]))
            {
                Console.WriteLine($"{input} has already been tried, please try another letter");
                return true;
            }

            return false;
        }
    }
}