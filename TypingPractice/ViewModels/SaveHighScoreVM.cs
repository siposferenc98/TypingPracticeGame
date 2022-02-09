using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Windows;

namespace TypingPractice.ViewModels
{
    internal class SaveHighScoreVM
    {
        public Game Game { get; set; }

        public ICommand SaveButton => new ButtonCEObj(SaveJson,SaveButtonCE);
        public SaveHighScoreVM(Game game)
        {
            Game = game;
        }

        private void SaveJson(object? windowParam)
        {
            if (App.Games.Contains(Game))
            {
                Game existinggame = App.Games.First(x=>x == Game);
                existinggame = Game;
            }
            else
            {
                App.Games.Add(Game);
            }

            JsonSerializerOptions options = new()
            {
                WriteIndented = true,
            };
            File.WriteAllText("./Games.json", JsonSerializer.Serialize(App.Games,options));
            Window window = (Window)windowParam!;
            window.Close();
        }

        private bool SaveButtonCE()
        {
            return Game.Name?.Length > 0;
        }
    }
}
