using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TypingPractice.Models
{
    internal class ButtonCE : ICommand
    {
        Action Action { get; set; }
        Func<bool> canExecute { get; set; }
        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public ButtonCE(Action action, Func<bool> ce)
        {
            Action = action;
            canExecute = ce;
        }
        public bool CanExecute(object? parameter)
        {
            return canExecute.Invoke();
        }

        public void Execute(object? parameter)
        {
            Action.Invoke();
        }
    }
}
