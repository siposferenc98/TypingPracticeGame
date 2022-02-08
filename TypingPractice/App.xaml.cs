using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
using System.IO;

namespace TypingPractice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static List<Game> Games = new();

        public App()
        {
            Games = ReadFromJson();
        }

        private List<Game> ReadFromJson()
        {
            try
            {
                return JsonSerializer.Deserialize<List<Game>>(File.ReadAllText("./Games.json"))!;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Couldn't load the leaderboard from JSON, error: {ex.Message}");
                return new();
            }
                    
        }
    }
}
