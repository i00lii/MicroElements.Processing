﻿// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using MicroElements.Metadata;
using NodaTime;

namespace MicroElements.Processing.TaskManager
{
    /// <summary>
    /// Represents single untyped operation (task).
    /// </summary>
    public interface IOperation : IMetadataProvider
    {
        /// <summary>
        /// Gets operation id.
        /// </summary>
        OperationId Id { get; }

        /// <summary>
        /// Gets operation status.
        /// </summary>
        OperationStatus Status { get; }

        /// <summary>
        /// Gets date and time of start.
        /// </summary>
        LocalDateTime? StartedAt { get; }

        /// <summary>
        /// Gets date and time of finish.
        /// </summary>
        LocalDateTime? FinishedAt { get; }

        /// <summary>
        /// Gets exception occured on task execution.
        /// </summary>
        Exception? Exception { get; }
    }

    /// <summary>
    /// Represents single operation with state.
    /// </summary>
    /// <typeparam name="TOperationState">Operation state.</typeparam>
    public interface IOperation<TOperationState> : IOperation
    {
        /// <summary>
        /// Gets operation state.
        /// </summary>
        TOperationState State { get; }
    }
}
