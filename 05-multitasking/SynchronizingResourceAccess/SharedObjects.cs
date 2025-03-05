public static class SharedObjects
{
    public static string? Message;
    public static object Conch = new(); // a shared object to lock on ❗it's a refference type that's why locking works
    public static int Counter;
}