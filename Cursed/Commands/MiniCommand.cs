using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Cursed.Commands
{
    public class MiniCommand : ICommand
    {
        Action action;
        public event EventHandler CanExecuteChanged;

        public MiniCommand(Action action)
        {
            this.action = action;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action();
        }
    }
}
