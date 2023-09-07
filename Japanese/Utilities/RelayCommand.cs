using System;
using System.Windows.Input;

namespace Japanese.Utilities
{
#pragma warning disable CS8604, CS8625

    /// <summary>
    /// A reusable implementation of ICommand for handling actions in MVVM applications.
    /// </summary>
    class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute">The action to execute when the command is invoked.</param>
        /// <param name="canExecute">A function that determines whether the command can execute (optional).</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Determines whether the command can execute.
        /// </summary>
        /// <param name="parameter">The parameter to pass to the command.</param>
        /// <returns>True if the command can execute, otherwise false.</returns>
        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        /// <summary>
        /// Executes the command with the given parameter.
        /// </summary>
        /// <param name="parameter">The parameter to pass to the command.</param>
        public void Execute(object? parameter)
        {
            execute(parameter);
        }
    }

#pragma warning restore CS8604, CS8625

}
