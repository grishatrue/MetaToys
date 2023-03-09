using System.Collections;

public interface IState
{
    string AnimationName { get; }

    void OnEnter();
    void OnExit();
    void OnUpdate();
    void OnAuto();
}
