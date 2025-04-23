namespace ProfilesApp.StateMachine.States;

public class ExitState : IState
{
    public void Enter()
    {
        Console.WriteLine("Нажмите любую кнопку для выхода..");
        Console.ReadKey();
    }
}