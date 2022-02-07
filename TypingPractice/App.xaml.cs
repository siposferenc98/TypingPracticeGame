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
            //return JsonSerializer.Deserialize<List<Game>>(File.ReadAllText("./Games.json"));
            return new List<Game>();
        }
    }
}
