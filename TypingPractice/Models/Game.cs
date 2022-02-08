using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TypingPractice.Models
{
    public class Game : INotifyPropertyChanged
    {
        private int _lives;
        private string _name = string.Empty;
        private int _score;
        private double _wpm;
        private int _characters;
        private TimeSpan _elapsed;
        public Stopwatch _stopwatch = new();

        public string Name { get => _name; set => _name = value; }
        public string? Difficulty { get; set; }
        public int Score 
        {
            get => _score; 
            set
            {
                _score = value;
                RaisePropertyChanged();
            } 
        }
        public int Lives
        {
            get => _lives;
            set 
            {
                _lives = value;
                RaisePropertyChanged();
            }
        }
        public int Characters 
        {
            get => _characters;
            set
            {
                _characters = value;
                Wpm = double.IsNaN(Math.Round((_characters / 5) / _stopwatch.Elapsed.TotalMinutes)) ? 0 : Math.Round((_characters / 5) / _stopwatch.Elapsed.TotalMinutes);
            }
        }
        public double Wpm
        {
            get => _wpm;
            set
            {
                _wpm = value;
                RaisePropertyChanged();
            }

        }
        public TimeSpan Elapsed
        {
            get
            {
                if (_elapsed == TimeSpan.Zero)
                    return _stopwatch.Elapsed;
                else
                    return _elapsed;
            } 
            set
            {
                _elapsed = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string? property = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public override string ToString()
        {
            return $"Name: {Name}, Score: {Score}";
        }
    }
}
