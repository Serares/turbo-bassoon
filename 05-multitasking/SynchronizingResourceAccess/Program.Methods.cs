partial class Program
{
    private static void MethodA()
    {
        // track if a lock was taken
        bool lockTaken = false;
        try
        {
            Monitor.Enter(SharedObjects.Conch, ref lockTaken);
            if (lockTaken)
            {
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(Random.Shared.Next(2000));
                    SharedObjects.Message += "A"; // concatenate string to shared resource
                    Interlocked.Increment(ref SharedObjects.Counter); // interlocked is not needed because the Conch is already protecting all shared resources
                    // but in case we don't use the Conch, we need to use Interlocked to protect the Counter
                    Write("."); // show activity
                }
            }
        }
        finally
        {
            if (lockTaken)
            {
                Monitor.Exit(SharedObjects.Conch);
            }
        }
    }

    private static void MethodB()
    {
        // track if a lock was taken
        // Only call Monitor.Exit if a lock was taken
        // if you try to exit without entering, you get an exception
        bool lockTaken = false;
        try
        {
            lockTaken = Monitor.TryEnter(SharedObjects.Conch, TimeSpan.FromSeconds(15));
            if (lockTaken)
            {
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(Random.Shared.Next(2000));
                    Interlocked.Increment(ref SharedObjects.Counter);
                    SharedObjects.Message += "B";
                }
            }
            else
            {
                WriteLine("Method B timed out when entering a monitor on conch.");
            }
        }
        finally
        {
            if (lockTaken)
            {
                Monitor.Exit(SharedObjects.Conch);
            }
        }
    }
}