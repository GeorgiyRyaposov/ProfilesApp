using Microsoft.Extensions.DependencyInjection;
using ProfilesApp.StateMachine.States;

namespace ProfilesApp.StateMachine;

public interface IAppStateMachine
{
    bool CompletedWork { get; }
    
    void SetInitialState();
    void SetProfileCreationState();
    void SetExitState();
}

public class AppStateMachine : IAppStateMachine
{
    public bool CompletedWork { get; private set; }
    
    private readonly IServiceProvider _serviceProvider;
    
    private IState _currentState;

    public AppStateMachine(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    private void SetState(IState state)
    {
        if (_currentState == state)
        {
            return;
        }
        
        _currentState = state;
        _currentState.Enter();
    }

    public void SetInitialState()
    {
        var state = _serviceProvider.GetService<InitialState>();
        SetState(state);
    }

    public void SetProfileCreationState()
    {
        var state = _serviceProvider.GetService<ProfileCreationState>();
        SetState(state);
    }
    
    public void SetExitState()
    {
        var state = _serviceProvider.GetService<ExitState>();
        SetState(state);
        
        CompletedWork = true;
    }
}