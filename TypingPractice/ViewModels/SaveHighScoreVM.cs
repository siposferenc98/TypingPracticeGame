using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypingPractice.ViewModels
{
    internal class SaveHighScoreVM
    {
        public Game Game { get; set; }
        public SaveHighScoreVM(Game game)
        {
            Game = game;
        }
    }
}
