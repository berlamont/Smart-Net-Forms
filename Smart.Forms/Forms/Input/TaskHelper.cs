﻿namespace Smart.Forms.Input
{
    using System.Threading.Tasks;

    /// <summary>
    ///
    /// </summary>
    internal static class TaskHelper
    {
        /// <summary>
        ///
        /// </summary>
        public static Task CompletedTask { get; } = Task.FromResult(false);
    }
}
