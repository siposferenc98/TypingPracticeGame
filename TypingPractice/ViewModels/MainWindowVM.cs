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
        private TimeSpan _currentRoundStartValue = TimeSpan.FromSeconds(5);
        private readonly Stopwatch _stopwatch = new();
        private readonly CancellationTokenSource _timerTokenSource = new();
        

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
                if(CheckIfWordsMatch() && _stopwatch.IsRunning)
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
        public DifficultyEnum Difficulty { get; set; } = DifficultyEnum.Medium;
        public ICommand StartGame => new Button(() => StartAGame());
        public ICommand CancelStopper => new Button(() => _timerTokenSource.Cancel() );

        public MainWindowVM()
        {

        }

        private async void StartTimerAsync(CancellationToken token)
        {
            _stopwatch.Start();
            while (_stopwatch.IsRunning)
            {
                CurrentTimer = _currentRoundStartValue - _stopwatch.Elapsed;
                if (CurrentTimer <= TimeSpan.Zero || token.IsCancellationRequested)
                    _stopwatch.Stop();
                await Task.Delay(10,CancellationToken.None);
            }
        }

        private void GetANewWordAndRestartTheTimer()
        {
            CurrentWord = _words.GetRandomWord();
            _currentTextBoxValue = string.Empty;
            _stopwatch.Restart();
            if (_currentRoundStartValue > TimeSpan.FromSeconds(2))
                _currentRoundStartValue = _currentRoundStartValue.Subtract(TimeSpan.FromSeconds(1));
        }

        private bool CheckIfWordsMatch()
        {
            if(CurrentTextBoxValue == CurrentWord)
                return true;
            return false;
        }
        private void StartAGame()
        {
            CancellationToken token = _timerTokenSource.Token;
            CurrentWord = _words.GetRandomWord();
            Task.Run(() => StartTimerAsync(token));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string? property = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
