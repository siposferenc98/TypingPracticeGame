using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace TypingPractice.ViewModels
{
    internal class LeaderBoardVM
    {
        public ObservableCollection<Game> EasyGames { get; set; }
        public ObservableCollection<Game> MediumGames { get; set; }
        public ObservableCollection<Game> HardGames { get; set; }

        public LeaderBoardVM()
        {
            EasyGames = new(App.Games.Where(x => x.Difficulty == "Easy"));
            MediumGames = new(App.Games.Where(x => x.Difficulty == "Medium"));
            HardGames = new(App.Games.Where(x => x.Difficulty == "Hard"));
        }

    }
}
