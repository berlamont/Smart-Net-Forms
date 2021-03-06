﻿namespace Smart.Forms.Validation
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Smart.ComponentModel;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValidationValue<T> : NotificationValue<T>, IValidatable, IValidity
    {
        private bool hasError;

        /// <summary>
        ///
        /// </summary>
        public List<IValidationRule<T>> Validations { get; } = new List<IValidationRule<T>>();

        /// <summary>
        ///
        /// </summary>
        public ObservableCollection<string> Errors { get; } = new ObservableCollection<string>();

        /// <summary>
        ///
        /// </summary>
        public bool HasError
        {
            get => hasError;
            set
            {
                Errors.Clear();
                SetProperty(ref hasError, value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public ValidationValue()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public ValidationValue(T value)
            : base(value)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            Errors.Clear();

            foreach (var message in Validations.Where(x => !x.Validate(Value)).Select(x => x.ErrorMessage))
            {
                Errors.Add(message);
            }

            HasError = Errors.Count > 0;

            return !HasError;
        }
    }
}
