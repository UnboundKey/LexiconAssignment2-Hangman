﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace LexiconAssignment2_Hangman
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
    public class Program
    {
        private static string[] _wordlist = {"word","sponge","human","death","borrow","cheat","supercalifragilisticexpialidocious"};
        private static string _pickedWord;
        private static StringBuilder _wrongLettersBuilder = new StringBuilder();
        private static char[] _correctChars;
        private static readonly int MaxAttempts = 10;
        private static int _currentAttempt;

        static void Main()
        {
            // _wordlist = LoadWordlistFromFile("wordlist.txt", _wordlist);
            _wordlist =  new string[]{"word"};
            _pickedWord = PickWord(_wordlist).ToUpper();
            _correctChars = FillCharArray(_pickedWord);

            Console.WriteLine("I've picked a word, now try to guess it");
            while ((_currentAttempt < MaxAttempts) ^ CheckWin())
            {
                PrintCurrentStatus(_correctChars,_wrongLettersBuilder,_currentAttempt,MaxAttempts);
                var input = Console.ReadLine()?.ToUpper();
                ModeSelect(input);
            }

            if (CheckWin())
            {
                Console.WriteLine($"Congrats, the word was \"{_pickedWord}\", you won");
                Console.WriteLine("press any key to exit");
                Console.ReadKey();
                
            } else if (_currentAttempt >= MaxAttempts)
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
                string fileContents = File.ReadAllText(fullPath);
                bool hasNewLines = fileContents.Contains("\n");
                if (fileContents.Contains(','))
                {
                    wordlist = fileContents.Split(',');
                }
                else if (hasNewLines)
                {
                    wordlist = fileContents.Split('\n');
                }

                // wordlist = File.ReadAllLines(fullPath)[0].Split(',');
                Console.WriteLine($"Loaded {wordlist.Length} words Successfully");
                return wordlist;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory not found");
                return wordlist;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not Found, Falling back on internal wordlist");
                return wordlist;
            }


        }

        private static void ModeSelect(string input)
        {
            if (input != null && input.Length > 1)
            {
                WholeWordGuess(input, _pickedWord, ref _correctChars);
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

        private static bool CheckWin()
        {
            if (new string(_correctChars) == _pickedWord)
            {
                return true;
            }

            return false;
        }
        
        public static void SingleCharacterGuess(string input, string pickedWord, char[] correctCharsArray, StringBuilder sb)
        {
            // this is a very hacky way of doing it but it works
            string correctCharsBeforeCheck = new string(correctCharsArray);

            if (DetectRepeatInput(input, correctCharsArray, sb)) return;

            for (int index = 0; index < pickedWord.Length; index++)
            {
                char letter = pickedWord[index];
                if (input[0] == letter)
                {
                    correctCharsArray[index] = letter;
                }
            }

            // if the string hasn't been updated then you've got a wrong letter. There fore append it
            if (correctCharsBeforeCheck == new string(correctCharsArray))
            {
                sb.Append(input);
                _currentAttempt++;
            }
        }

        public static void WholeWordGuess(string input, string pickedWord, ref char[] charArray)
        {
            if (input == pickedWord)
            {
                charArray = input.ToCharArray();
                return;
            }

            _currentAttempt++;
        }

        private static void PrintCurrentStatus(char[] charArray, StringBuilder sb, int int1, int int2)
        {
            Console.WriteLine(charArray);
            Console.WriteLine($"Wrong Letters: {sb}");
            Console.WriteLine($"Attempts {int1} : {int2}");
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