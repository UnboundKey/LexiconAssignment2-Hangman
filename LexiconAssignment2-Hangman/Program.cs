using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.IO;

namespace LexiconAssignment2_Hangman
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
    public class Program
    {
        private static string[] _wordlist = {"word","sponge"};
        private static string _pickedWord;
        private static StringBuilder _wrongLettersBuilder = new StringBuilder();
        private static char[] _correctChars;
        private static readonly int maxAtempts = 10;
        private static int _currentAttempt;

        static void Main()
        {
            _wordlist = LoadWordlistFromFile("wordlist.txt", _wordlist);
            _pickedWord = PickWord(_wordlist).ToUpper();
            _correctChars = FillCharArray(_pickedWord);

            Console.WriteLine("I've picked a word, now try to guess it");
            while ((_currentAttempt < maxAtempts) ^ CheckWin())
            {
                PrintCurrentLetters();
                var input = Console.ReadLine()?.ToUpper();
                ModeSelect(input);
            }

            if (CheckWin())
            {
                Console.WriteLine($"Congrats, the word was \"{_pickedWord}\", you won");
                Console.WriteLine("press any key to exit");
                Console.ReadKey();
                
            } else if (_currentAttempt >= maxAtempts)
            {
                Console.WriteLine($"Sorry, you lost the game ;). the word was: {_pickedWord}");
                Console.WriteLine("press any key to exit");
                Console.ReadKey();
            }
        }

        public static string[] LoadWordlistFromFile(string filenamePath, string[] wordlist)
        {
            string fullPath = $"{Environment.CurrentDirectory}/{filenamePath}";
            try
            {
                Console.WriteLine($"Loading wordlist from {fullPath}");
                wordlist = File.ReadAllLines(fullPath)[0].Split(',');
                Console.WriteLine($"Load Successful");
                return wordlist;
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine($"Folder containing directory not found");
                return wordlist;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"File not Found, Falling back on internal wordlist");
                return wordlist;
            }


        }

        private static void ModeSelect(string input)
        {
            if (input != null && input.Length > 1)
            {
                WholeWordGuess(input, _pickedWord);
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
        
        public static void SingleCharacterGuess(string input, string pickedWord, char[] correctChars, StringBuilder sb)
        {
            // this is a very hacky way of doing it but it works
            string correctCharsBeforeCheck = new string(correctChars);

            if (DetectRepeatInput(input, correctChars, sb)) return;

            for (int index = 0; index < pickedWord.Length; index++)
            {
                char letter = pickedWord[index];
                if (input[0] == letter)
                {
                    correctChars[index] = letter;
                }
            }

            // if the string hasn't been updated then you've got a wrong letter. There fore append it
            if (correctCharsBeforeCheck == new string(correctChars))
            {
                sb.Append(input);
                _currentAttempt++;
            }
        }

        public static void WholeWordGuess(string input, string pickedWord)
        {
            if (input == pickedWord)
            {
                _correctChars = input.ToCharArray();
            }
        }

        private static void PrintCurrentLetters()
        {
            Console.WriteLine(_correctChars);
            Console.WriteLine($"Wrong Letters: {_wrongLettersBuilder}");
            Console.WriteLine($"Attempts {_currentAttempt} : {maxAtempts}");
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