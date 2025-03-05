partial class Program
{
    private static void MethodA()
    {
        TaskTitle("Starting Method A...");
        OutputThreadInfo();
        Thread.Sleep(3000);
        TaskTitle("Finished Method A.");
    }

    private static void MethodB()
    {
        TaskTitle("Starting Method B...");
        OutputThreadInfo();
        Thread.Sleep(2000);
        TaskTitle("Finished Method B.");
    }

    private static void MethodC()
    {
        TaskTitle("Starting Method C...");
        OutputThreadInfo();
        Thread.Sleep(1000);
        TaskTitle("Finished Method C.");
    }

    private static decimal CallWebService()
    {
        TaskTitle("Start call to web service...");
        OutputThreadInfo();
        Thread.Sleep(Random.Shared.Next(2000, 4000));
        TaskTitle("Finished call to web service.");
        return 29.99M;
    }

    private static string CallStoredProcedure(decimal amount)
    {
        TaskTitle("Start call to web service...");
        OutputThreadInfo();
        Thread.Sleep(Random.Shared.Next(2000, 4000));
        TaskTitle("Finished call to web service.");
        return $"12 products cost more than ${amount:C}";
    }

    private static void OutherMethod()
    {
        TaskTitle("Outher method starting...");
        Task innerTask = Task.Factory.StartNew(InnerMethod, 
        TaskCreationOptions.AttachedToParent // this is how to link a parent to a child task
        );
        TaskTitle("Outher Method finished");
    }

    private static void InnerMethod()
    {
        TaskTitle("Inner method starting...");
        Thread.Sleep(2000);
        TaskTitle("Inner method finished.");
    }
}