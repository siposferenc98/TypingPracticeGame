using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;

namespace TypingPractice.ViewModels
{
    internal class SaveHighScoreVM
    {
        public Game Game { get; set; }

        public ICommand SaveButton => new ButtonCE(SaveJson,SaveButtonCE);
        public SaveHighScoreVM(Game game)
        {
            Game = game;
        }

        private void SaveJson()
        {
            App.Games.Add(Game);
            
            File.WriteAllText("./Games.json", JsonSerializer.Serialize(App.Games));
        }

        private bool SaveButtonCE()
        {
            return Game.Name?.Length > 0;
        }
    }
}
