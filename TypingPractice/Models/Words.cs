using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TypingPractice.Properties;

namespace TypingPractice.Models
{
    internal class Words
    {
        private List<string> WordsList = Resources.ResourceManager.GetString("Words")!.Split("\n").ToList();

        public Words()
        {

        }

        public string GetRandomWord()
        {
            Random rnd = new();
            return WordsList[rnd.Next(0, WordsList.Count)];
        }
    }
}
