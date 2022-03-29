using System;
using System.IO;
using System.Text;
using Xunit;
using LexiconAssignment2_Hangman;
namespace HangmanTests
{
    public class HangmanTests
    {

        [Fact]
        public void CheckMultipleLetters()
        {
            string word = "annanas";
            char[] correctArray = Program.FillCharArray(word);
            StringBuilder sb = new StringBuilder();
            Program.SingleCharacterGuess("n", word, correctArray,sb);
            Assert.Equal("_nn_n__", new string(correctArray));
            Assert.Empty(sb.ToString());
        }

        [Fact]
        public void CheckRepeatInput_true()
        {
            string word = "annanas";
            char[] correctArray = Program.FillCharArray(word);
            StringBuilder sb = new StringBuilder();
            Program.SingleCharacterGuess("n", word, correctArray,sb);
            Program.SingleCharacterGuess("n", word, correctArray,sb);
            Assert.True(Program.DetectRepeatInput("n",correctArray,sb));
            Assert.Empty(sb.ToString());
        }
        
        [Fact]
        public void CheckRepeatInput_false()
        {
            string word = "annanas";
            char[] correctArray = Program.FillCharArray(word);
            StringBuilder sb = new StringBuilder();
            Program.SingleCharacterGuess("n", word, correctArray,sb);
            Assert.False(Program.DetectRepeatInput("a",correctArray,sb));
            Assert.Empty(sb.ToString());
        }
        
        [Fact]
        public void PickWord()
        {
            string[] words = {"test"};
            var pickWord = Program.PickWord(words);
            Assert.Equal(words[0],pickWord);
        }

        [Fact]
        public void LoadWordList_fileDoesNotExist()
        {
            string[] output = { };
            Program.LoadWordlistFromFile("file.txt", output);
            Assert.Throws<FileNotFoundException>(() => Program.LoadWordlistFromFile("file.txt", output));
        }
    }
}