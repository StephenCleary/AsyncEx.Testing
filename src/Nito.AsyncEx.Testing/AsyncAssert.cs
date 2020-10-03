using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Nito.AsyncEx.Testing
{
    /// <summary>
    /// Provides static methods useful for testing asynchronous methods and tasks.
    /// </summary>
    public static class AsyncAssert
    {
        /// <summary>
        /// Ensures that a synchronous delegate throws an exception of an expected type.
        /// </summary>
        /// <typeparam name="TException">The type of exception to expect.</typeparam>
        /// <param name="action">The synchronous delegate to test.</param>
        /// <param name="allowDerivedTypes">Whether derived types should be accepted.</param>
        public static TException Throws<TException>(Action action, bool allowDerivedTypes = true)
            where TException : Exception
        {
            _ = action ?? throw new ArgumentNullException(nameof(action));
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (allowDerivedTypes && !(ex is TException))
                    throw new Exception("Delegate threw exception of type " + ex.GetType().Name + ", but " + typeof(TException).Name + " or a derived type was expected.", ex);
                if (!allowDerivedTypes && ex.GetType() != typeof(TException))
                    throw new Exception("Delegate threw exception of type " + ex.GetType().Name + ", but " + typeof(TException).Name + " was expected.", ex);
                return (TException)ex;
            }
            throw new Exception("Delegate did not throw expected exception " + typeof(TException).Name + ".");
        }

        /// <summary>
        /// Ensures that a synchronous delegate throws an exception.
        /// </summary>
        /// <param name="action">The synchronous delegate to test.</param>
        public static Exception Throws(Action action)
        {
            return Throws<Exception>(action, true);
        }

        /// <summary>
        /// Ensures that an asynchronous delegate throws an exception of an expected type.
        /// </summary>
        /// <typeparam name="TException">The type of exception to expect.</typeparam>
        /// <param name="action">The asynchronous delegate to test.</param>
        /// <param name="allowDerivedTypes">Whether derived types should be accepted.</param>
        public static async Task<TException> ThrowsAsync<TException>(Func<Task> action, bool allowDerivedTypes = true)
            where TException : Exception
        {
            _ = action ?? throw new ArgumentNullException(nameof(action));
            try
            {
                await action().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (allowDerivedTypes && !(ex is TException))
                    throw new Exception("Delegate threw exception of type " + ex.GetType().Name + ", but " + typeof(TException).Name + " or a derived type was expected.", ex);
                if (!allowDerivedTypes && ex.GetType() != typeof(TException))
                    throw new Exception("Delegate threw exception of type " + ex.GetType().Name + ", but " + typeof(TException).Name + " was expected.", ex);
                return (TException)ex;
            }
            throw new Exception("Delegate did not throw expected exception " + typeof(TException).Name + ".");
        }

        /// <summary>
        /// Ensures that an asynchronous delegate throws an exception.
        /// </summary>
        /// <param name="action">The asynchronous delegate to test.</param>
        public static Task<Exception> ThrowsAsync(Func<Task> action)
        {
            return ThrowsAsync<Exception>(action, true);
        }

        /// <summary>
        /// Ensures that a task throws an exception of an expected type.
        /// </summary>
        /// <typeparam name="TException">The type of exception to expect.</typeparam>
        /// <param name="task">The task to observe.</param>
        /// <param name="allowDerivedTypes">Whether derived types should be accepted.</param>
        public static Task<TException> ThrowsAsync<TException>(Task task, bool allowDerivedTypes = true)
            where TException : Exception
        {
            return ThrowsAsync<TException>(() => task, allowDerivedTypes);
        }

        /// <summary>
        /// Ensures that a task throws an exception.
        /// </summary>
        /// <param name="task">The task to observe.</param>
        public static Task<Exception> ThrowsAsync(Task task)
        {
            return ThrowsAsync<Exception>(task, true);
        }

        /// <summary>
        /// Ensures that a synchronous delegate is cancelled.
        /// </summary>
        /// <param name="action">The synchronous delegate to test.</param>
        public static OperationCanceledException Cancels(Action action)
        {
            return Throws<OperationCanceledException>(action, true);
        }


        /// <summary>
        /// Ensures that an asynchronous delegate is cancelled.
        /// </summary>
        /// <param name="action">The asynchronous delegate to test.</param>
        public static Task<OperationCanceledException> CancelsAsync(Func<Task> action)
        {
            return ThrowsAsync<OperationCanceledException>(action, true);
        }

        /// <summary>
        /// Ensures that a task is cancelled.
        /// </summary>
        /// <param name="task">The task to observe.</param>
        public static Task<OperationCanceledException> CancelsAsync(Task task)
        {
            return ThrowsAsync<OperationCanceledException>(task, true);
        }

        /// <summary>
        /// Attempts to ensure that a task never completes. If the task takes a long time to complete, this method may not detect that it (incorrectly) completes.
        /// </summary>
        /// <param name="task">The task to observe.</param>
        /// <param name="timeout">The amount of time to (asynchronously) wait for the task to complete.</param>
        public static async Task NeverCompletesAsync(Task task, int timeout = 500)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));

            // Wait for the task to complete, or the timeout to fire.
            var completedTask = await Task.WhenAny(task, Task.Delay(timeout)).ConfigureAwait(false);
            if (completedTask == task)
                throw new Exception("Task completed unexpectedly.");

            // If the task didn't complete, attach a continuation that will raise an exception on a random thread pool thread if it ever does complete.
            try
            {
                throw new Exception("Task completed unexpectedly.");
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                var info = ExceptionDispatchInfo.Capture(ex);
                var __ = task.ContinueWith(_ => info.Throw(), TaskScheduler.Default);
            }
        }
    }
}
