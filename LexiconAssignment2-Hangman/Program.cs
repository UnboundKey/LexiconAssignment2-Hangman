using System;
using System.IO;
using System.Text;

namespace LexiconAssignment2_Hangman
{
    // ReSharper disable once ClassNeverInstantiated.Global
    class Program
    {
        private static string[] _wordlist = new []{"the","same","collection"};
        private static string _pickedWord;
        private StringBuilder _wrongLettersBuilder = new StringBuilder();
        private int _numberOfGuesses = 0;
        private int _maxGuesses = 10;

        static void Main(string[] args)
        {
            _pickedWord = PickWord(_wordlist);
            printWord();
            // Todo Get player input
            // Todo check if letter is in wrongletter array if so warn them politely.

        }

        private static void printWord()
        {
            // TODO Print Underscores (_) for each letter in chosen word
            throw new NotImplementedException();
        }

        private static string PickWord(string[] wordStrings)
        {
            Random r = new Random();
            int randSelection = r.Next(0,wordStrings.Length);
            _pickedWord = wordStrings[randSelection];
            return _pickedWord;
        }
    }
}