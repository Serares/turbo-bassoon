using System.Diagnostics;

OutputThreadInfo();
Stopwatch timer = Stopwatch.StartNew();
// SectionTitle("Running methods sync on one thread");
// MethodA();
// MethodB();
// MethodC();
// WriteLine($"{timer.ElapsedMilliseconds:#,##0} ms elapsed.");
// timer.Restart();
// SectionTitle("Running methods async on multiple thread");
// Task taskA = new(MethodA);
// taskA.Start();
// Task taskB = Task.Factory.StartNew(MethodB);
// Task taskC = Task.Run(MethodC);

// Task[] tasks = { taskA, taskB, taskC };
// Task.WaitAll(tasks);
// WriteLine($"{timer.ElapsedMilliseconds:#,##0} ms elapsed.");
// timer.Restart();

// SectionTitle("Passing the result of one task to another");
// Task<string> taskServiceThenSProc = Task.Factory
// .StartNew(CallWebService) // return Task<decimal>
// .ContinueWith(prevTask => CallStoredProcedure(prevTask.Result));
// WriteLine($"Result: {taskServiceThenSProc.Result}");
// WriteLine($"{timer.ElapsedMilliseconds:#,##0} ms elapsed");

SectionTitle("Nested and child tasks");
Task outerTask = Task.Factory.StartNew(OutherMethod);
outerTask.Wait();
WriteLine("Console app is stopping");