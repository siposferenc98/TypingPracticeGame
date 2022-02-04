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
        private readonly Words _words = new();
        private string _currentWord = string.Empty;
        private string _currentTextBoxValue = string.Empty;
        private TimeSpan _currentTimer;
        private TimeSpan _currentRoundStartValue;
        private TimeSpan _startingTime;
        private DifficultyEnum _difficulty = DifficultyEnum.Medium;
        private readonly Stopwatch _stopwatch = new();
        private CancellationTokenSource _timerTokenSource = new();

        public event EventHandler? FocusTextBox;
        private TimeSpan Time => Difficulty switch
        {
            DifficultyEnum.Easy => TimeSpan.FromSeconds(10),
            DifficultyEnum.Medium => TimeSpan.FromSeconds(7),
            DifficultyEnum.Hard => TimeSpan.FromSeconds(5),
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
                }
                if (CurrentTextBoxValue == CurrentWord && _stopwatch.IsRunning)
                {
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
                SetTimers();
            }
        }
        public bool DifficultyPanelIsEnabled => !_stopwatch.IsRunning;
        public bool StopButtonIsEnabled => _stopwatch.IsRunning;

        public ICommand StartGame => new Button(() => StartAGame());
        public ICommand CancelStopper => new Button(() => _timerTokenSource.Cancel() );

        public MainWindowVM()
        {
            SetTimers();
        }

        private async void StartTimerAsync(CancellationToken token)
        {
            _stopwatch.Restart();
            await RaisePropertiesChanged();
            FocusTextBox?.Invoke(this,EventArgs.Empty);

            while (_stopwatch.IsRunning)
            {
                CurrentTimer = _currentRoundStartValue - _stopwatch.Elapsed;
                if (CurrentTimer <= TimeSpan.Zero || token.IsCancellationRequested)
                    break;
                await Task.Delay(10,CancellationToken.None);
            }

            _stopwatch.Stop();
            SetTimers();
            await RaisePropertiesChanged();
        }

        private Task RaisePropertiesChanged()
        {
            RaisePropertyChanged("DifficultyPanelIsEnabled");
            RaisePropertyChanged("StopButtonIsEnabled");
            return Task.CompletedTask;
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
            Task.Run(() => StartTimerAsync(token));
        }
        private void SetTimers()
        {
            CurrentTimer = Time;
            _currentRoundStartValue = Time;
            _startingTime = Time;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string? property = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
