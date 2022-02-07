using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using System.Windows;

namespace TypingPractice.ViewModels
{
    public enum DifficultyEnum
    {
        Easy,
        Medium,
        Hard
    }
    internal class MainWindowVM : INotifyPropertyChanged
    {
        //Fields
        #region Fields
        private readonly Words _words = new();
        private string _currentWord = string.Empty;
        private string _currentTextBoxValue = string.Empty;
        private TimeSpan _currentTimer;
        private TimeSpan _currentRoundStartValue;
        private TimeSpan _startingTime;
        private DifficultyEnum _difficulty = DifficultyEnum.Medium;
        private readonly Stopwatch _stopwatch = new();
        private CancellationTokenSource _timerTokenSource = new();
        private Game _game;
        #endregion
        public event EventHandler? FocusTextBox;
        private TimeSpan Time => Difficulty switch
        {
            DifficultyEnum.Easy => TimeSpan.FromSeconds(10),
            DifficultyEnum.Medium => TimeSpan.FromSeconds(7),
            DifficultyEnum.Hard => TimeSpan.FromSeconds(5),
            _ => throw new NotImplementedException()
        };
        private int Lives => Difficulty switch
        {
            DifficultyEnum.Easy => 7,
            DifficultyEnum.Medium => 5,
            DifficultyEnum.Hard => 3,
            _ => throw new NotImplementedException()
        };

        public string CurrentWord 
        {
            get => _currentWord;
            set
            {
                _currentWord = value;
                RaisePropertyChanged();
            } 
        }
        public string CurrentTextBoxValue
        {
            get => _currentTextBoxValue;
            set
            {
                _currentTextBoxValue = value;
                if(!CurrentWord.StartsWith(CurrentTextBoxValue) && _stopwatch.IsRunning)
                {
                    _currentTextBoxValue = string.Empty;
                    Game.Lives--;
                }
                if (CurrentTextBoxValue == CurrentWord && _stopwatch.IsRunning)
                {
                    Game.Characters += CurrentWord.Length;
                    Game.Score += 10;
                    GetANewWordAndRestartTheTimer();
                }
            } 
        }
        public TimeSpan CurrentTimer
        {
            get => _currentTimer;
            set
            {
                _currentTimer = value;
                RaisePropertyChanged();
            }
        }
        public DifficultyEnum Difficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                Game.Lives = Lives;
                SetTimers();
                //RaisePropertyChanged("Game");
            }
        }
        public Game Game 
        { 
            get => _game;
            set
            {
                _game = value;
                RaisePropertyChanged();
            }
        }
        public bool DifficultyPanelIsEnabled => !_stopwatch.IsRunning;

        public ICommand StartGame => new Button(() => StartAGame());
        public ICommand SaveHighScore => new ButtonCE(ShowSaveHighScoreWindow, SaveHighScoreCE);
        public ICommand ShowLeaderboard => new Button(ShowLeaderBoard);
        public ICommand StopGame => new ButtonCE(() => _timerTokenSource.Cancel(),CancelStopperCE);

        public MainWindowVM()
        {
            SetTimers();
            Game = new()
            {
                Lives = Lives
            };
        }

        private async void StartTimerAsync(CancellationToken token)
        {
            _stopwatch.Restart();
            RaisePropertyChanged("DifficultyPanelIsEnabled");
            FocusTextBox?.Invoke(this,EventArgs.Empty);

            while (_stopwatch.IsRunning && Game.Lives > 0)
            {
                if (CurrentTimer <= TimeSpan.Zero || token.IsCancellationRequested)
                    break;

                CurrentTimer = _currentRoundStartValue - _stopwatch.Elapsed;
                await Task.Delay(10,CancellationToken.None);
            }

            _stopwatch.Stop();
            Game._elapsed.Stop();
            SetTimers();
            Application.Current.Dispatcher.Invoke(() =>
            {
                CommandManager.InvalidateRequerySuggested();
            });
            RaisePropertyChanged("DifficultyPanelIsEnabled");
        }

        private void GetANewWordAndRestartTheTimer()
        {
            CurrentWord = _words.GetRandomWord();
            _currentTextBoxValue = string.Empty;
            if (_startingTime / _currentRoundStartValue < 2)
                _currentRoundStartValue = _currentRoundStartValue.Subtract(TimeSpan.FromSeconds(0.5));
            _stopwatch.Restart();
        }
        private void StartAGame()
        {
            _timerTokenSource = new();
            CancellationToken token = _timerTokenSource.Token;
            CurrentWord = _words.GetRandomWord();
            Game = new()
            {
                Lives = Lives,
                Difficulty = Difficulty.ToString()
            };
            Game._elapsed.Start();
            Task.Run(() => StartTimerAsync(token));
        }
        private void SetTimers()
        {
            CurrentTimer = Time;
            _currentRoundStartValue = Time;
            _startingTime = Time;
        }

        private void ShowSaveHighScoreWindow()
        {
            SaveHighScore saveHighScore = new(Game);
            saveHighScore.ShowDialog();
        }
        private bool SaveHighScoreCE()
        {
            return !_stopwatch.IsRunning && Game.Elapsed > TimeSpan.Zero;
        }

        private void ShowLeaderBoard()
        {
            Leaderboard leaderboard = new();
            leaderboard.Show();
        }
        private bool CancelStopperCE()
        {
            return _stopwatch.IsRunning;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string? property = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
