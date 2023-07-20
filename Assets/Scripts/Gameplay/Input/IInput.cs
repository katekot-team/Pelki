namespace Pelki.Gameplay.Input
{
    public interface IInput
    {
        float Horizontal { get; }
        bool IsJump { get; }
    }
}