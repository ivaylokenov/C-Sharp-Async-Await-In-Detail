namespace StateMachine
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    public class Compiled
    {
        private sealed class InputContext
        {
            public int input;

            internal void ConsoleWriteLine()
            {
                Console.WriteLine(input);
            }
        }

        private sealed class ReturnContext
        {
            public static readonly ReturnContext Context = new ReturnContext();

            public static Func<bool> FunctionCall;

            internal bool ReturnTrue()
            {
                return true;
            }
        }

        private sealed class TaskStateMachine : IAsyncStateMachine
        {
            // Initial value = -1
            public int state;

            // Task builder
            public AsyncTaskMethodBuilder taskBuilder;

            public string[] args;

            // Context of the first awaited task
            private InputContext inputContext;

            // Result of the second awaited task
            private bool result;

            // Execution context
            private TaskAwaiter FirstAwaiter;

            // Execution context
            private TaskAwaiter<bool> SecondAwaiter;

            // This is the section where actual logic of original method exist
            // Contain the logic to execute the code till await statement 
            // Also configure stuff of the wake up call when async method complete it's execution
            public void MoveNext()
            {
                // Set state to local variable for performance
                int num = state;
                try
                {
                    // Variables to save awaiters for the new tasks
                    TaskAwaiter firstAwaiter;
                    TaskAwaiter<bool> secondAwaiter;

                    // Initially -1
                    if (num != 0)
                    {
                        if (num == 1)
                        {
                            // We get the awaiter from the execution context again
                            secondAwaiter = SecondAwaiter;

                            // Set to null to release memory allocation
                            SecondAwaiter = default;

                            // Restart the state as we are about to finish
                            num = (state = -1);

                            // Don't use goto unless you know what you are doing! :)
                            goto IL_0114;
                        }

                        // Execute first task with its context
                        inputContext = new InputContext();
                        inputContext.input = 5;
                        firstAwaiter = Task.Run(inputContext.ConsoleWriteLine).GetAwaiter();

                        // This block is for optimization in case the task is already finished (Task.FromResult)
                        // Most probably - this block will be executed
                        if (!firstAwaiter.IsCompleted)
                        {
                            num = (state = 0);
                            // Save the awaiter for the next state.
                            FirstAwaiter = firstAwaiter;
                            TaskStateMachine stateMachine = this;

                            // This call to AwaitUnsafeOnCompleted is where most of the magic happens
                            // In this step we register the StateMachine as continuation of the task by calling AwaitUnsafeOnCompleted 
                            // But how it is done? 
                            // builder.AwaitUnsafeOnCompleted do multiple things in background
                            // 1. TaskMethodBuilder captures Execution context 
                            // 2. Create an MoveNextAction using Execution context 
                            // 3. This MoveNextAction will call the MoveNext of state machine and provide execution context
                            // 4. Set MoveNextAction as callback to awaiter on complete Using awaiter.UnsafeOnCompleted(action)
                            taskBuilder.AwaitUnsafeOnCompleted(ref firstAwaiter, ref stateMachine);
                            return;
                        }
                    }
                    else
                    {
                        // Second time - we get the awaiter from the execution context
                        firstAwaiter = FirstAwaiter;

                        // Set to null to release memory allocation
                        FirstAwaiter = default;
                        num = (state = -1);
                    }

                    firstAwaiter.GetResult();

                    // Execute second task with its context
                    secondAwaiter = Task
                        .Run(ReturnContext.FunctionCall ?? (ReturnContext.FunctionCall = ReturnContext.Context.ReturnTrue))
                        .GetAwaiter();

                    // Again - for optimization
                    if (!secondAwaiter.IsCompleted)
                    {
                        num = (state = 1);
                        // Save the awaiter for the next state.
                        SecondAwaiter = secondAwaiter;
                        TaskStateMachine stateMachine = this;
                        // Register the state machine to continue with the next state
                        taskBuilder.AwaitUnsafeOnCompleted(ref secondAwaiter, ref stateMachine);
                        return;
                    }

                    IL_0114:
                    // Finish by getting the awaiter result and execute the logic
                    result = secondAwaiter.GetResult();
                    Console.WriteLine(result);
                }
                catch (Exception exception)
                {
                    // Exception handling mechanism
                    state = -2;
                    taskBuilder.SetException(exception);
                    return;
                }

                // Set the state to final state we are done
                state = -2;

                // Set the result on the task builder
                taskBuilder.SetResult();
            }

            public void SetStateMachine(IAsyncStateMachine stateMachine)
            {
            }
        }

        public static Task MainCompiled(string[] args)
        {
            var taskMethodBuilder = AsyncTaskMethodBuilder.Create();

            TaskStateMachine stateMachine = new TaskStateMachine();
            stateMachine.args = args;
            stateMachine.taskBuilder = taskMethodBuilder;
            stateMachine.state = -1;

            taskMethodBuilder.Start(ref stateMachine);

            return stateMachine.taskBuilder.Task;
        }
    }
}