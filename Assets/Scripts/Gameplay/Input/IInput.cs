namespace Pelki.Gameplay.Input
{
    public interface IInput
    {
        float Horizontal { get; }
        float RawHorizontal { get; }
        bool IsJump { get; }
        bool IsRangedAttacking { get; }
    }
}