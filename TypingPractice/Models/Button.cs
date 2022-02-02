using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TypingPractice.Models
{
    internal class Button : ICommand
    {
        Action Action { get; set; }
        public event EventHandler? CanExecuteChanged;
        public Button(Action action)
        {
            Action = action;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            Action.Invoke();
        }
    }
}
