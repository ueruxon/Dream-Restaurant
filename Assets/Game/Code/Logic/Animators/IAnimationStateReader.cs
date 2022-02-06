namespace Game.Code.Logic.Animators
{
    public interface IAnimationStateReader
    {
        void EnteredState(int stateHash);
        void ExitedState(int stateHash);
    }
}