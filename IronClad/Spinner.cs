namespace Mohr.Jonas.IronClad;

public static class Spinner
{
    private static readonly char[] spinnerChars = ['⠋', '⠙', '⠹', '⠸', '⠼', '⠴', '⠦', '⠧', '⠇', '⠏',];

    public static void SpinToCompletion(string message, Action action)
    {
        var task = Task.Run(action);
        SpinToCompletion(message, task);
        if (task.IsFaulted)
            throw task.Exception;
    }

    public static T SpinToCompletion<T>(string message, Func<T> func)
    {
        var task = Task.Run(func);
        SpinToCompletion(message, task);
        if (task.IsFaulted)
            throw task.Exception;
        return task.Result;
    }

    private static void SpinToCompletion(string message, Task task)
    {
        var i = 0;
        while (!task.IsCompleted)
        {
            Console.Write('\r');
            Console.Write(spinnerChars[i]);
            Console.Write(' ');
            Console.Write(message);
            i++;
            if (i >= spinnerChars.Length)
                i = 0;
            Thread.Sleep(200);
        }
        Console.Write('\r');
    }
}