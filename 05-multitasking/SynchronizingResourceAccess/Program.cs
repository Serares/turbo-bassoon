using System.Diagnostics;
WriteLine("Please wait for the tasks to complete.");
Stopwatch timer = Stopwatch.StartNew();

Task a = Task.Factory.StartNew(MethodA);
Task b = Task.Factory.StartNew(MethodB);

Task.WaitAll(new Task[] { a, b });

WriteLine();
WriteLine($"Results: {SharedObjects.Message}");
WriteLine($"Counter: {SharedObjects.Counter}");
WriteLine($"{timer.ElapsedMilliseconds:#,##0} elapsed milliseconds");
