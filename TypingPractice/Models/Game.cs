using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TypingPractice.Models
{
    public class Game : INotifyPropertyChanged
    {
        private int _lives;
        private string _name = string.Empty;
        private int _score;
        private int _characters;
        private Stopwatch _elapsed = new();

        public string? Difficulty { get; set; }
        public int Lives
        {
            get => _lives;
            set 
            {
                _lives = value;
                RaisePropertyChanged();
            }
        }
        public string Name { get => _name; set => _name = value; }
        public int Score 
        {
            get => _score; 
            set
            {
                _score = value;
                RaisePropertyChanged();
            } 
        }
        public double Wpm => double.IsNaN(Math.Round((Characters / 5) / Elapsed.Elapsed.TotalMinutes)) ? 0 : Math.Round((Characters / 5) / Elapsed.Elapsed.TotalMinutes);
        public int Characters 
        {
            get => _characters;
            set
            {
                _characters = value;
                RaisePropertyChanged("Wpm");
            }
        }
        public Stopwatch Elapsed { get => _elapsed;}

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string? property = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
