public interface IInputCommand
{
    void Execute();
}

public interface IInputCommand<T>
{
    void Execute(T value);
}

public class SimpleCommand : IInputCommand
{
    private readonly System.Action _action;

    public SimpleCommand(System.Action action)
    {
        _action = action;
    }

    public void Execute()
    {
        _action?.Invoke();
    }
}

public class ValueCommand<T> : IInputCommand<T>
{
    private readonly System.Action<T> _action;

    public ValueCommand(System.Action<T> action)
    {
        _action = action;
    }

    public void Execute(T value)
    {
        _action?.Invoke(value);
    }
}