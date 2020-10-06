﻿// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace MicroElements.Processing.TaskManager
{
    /// <summary>
    /// Represents task executor.
    /// </summary>
    /// <typeparam name="TSessionState">Session state.</typeparam>
    /// <typeparam name="TOperationState">Operation state.</typeparam>
    public interface ITaskExecutor<TSessionState, TOperationState>
    {
        /// <summary>
        /// Executes operation and returns updated operation.
        /// </summary>
        /// <param name="session">Owner session.</param>
        /// <param name="operation">Operation to execute.</param>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Updated operation.</returns>
        Task<IOperation<TOperationState>> ExecuteAsync(
            ISession<TSessionState> session,
            IOperation<TOperationState> operation,
            CancellationToken cancellation = default);
    }
}