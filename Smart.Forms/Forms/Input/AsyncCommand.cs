﻿namespace Smart.Forms.Input
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    ///
    /// </summary>
    public sealed class AsyncCommand : ObserveCommandBase<AsyncCommand>
    {
        private readonly Func<Task> execute;

        private readonly Func<bool> canExecute;

        private bool executing;

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        public AsyncCommand(Func<Task> execute)
            : this(execute, () => true)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        public AsyncCommand(Action execute)
            : this(execute, () => true)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public AsyncCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = () =>
            {
                execute();
                return TaskHelper.CompletedTask;
            };
            this.canExecute = canExecute;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute(object parameter)
        {
            return !executing && canExecute();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        public override async void Execute(object parameter)
        {
            executing = true;
            RaiseCanExecuteChanged();

            try
            {
                var task = execute();
                await task;
            }
            finally
            {
                executing = false;
                RaiseCanExecuteChanged();
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class AsyncCommand<T> : ObserveCommandBase<AsyncCommand<T>>
    {
        private static readonly bool IsValueType = typeof(T).GetTypeInfo().IsValueType;

        private readonly Func<T, Task> execute;

        private readonly Func<T, bool> canExecute;

        private bool executing;

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        public AsyncCommand(Func<T, Task> execute)
            : this(execute, x => true)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        public AsyncCommand(Action<T> execute)
            : this(execute, x => true)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public AsyncCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = arg =>
            {
                execute(arg);
                return TaskHelper.CompletedTask;
            };
            this.canExecute = canExecute;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute(object parameter)
        {
            return !executing && canExecute(Cast(parameter));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            Execute(Cast(parameter));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        public async void Execute(T parameter)
        {
            executing = true;
            RaiseCanExecuteChanged();

            try
            {
                var task = execute(parameter);
                await task;
            }
            finally
            {
                executing = false;
                RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private static T Cast(object parameter)
        {
            if ((parameter == null) && IsValueType)
            {
                return default(T);
            }

            return (T)parameter;
        }
    }
}
