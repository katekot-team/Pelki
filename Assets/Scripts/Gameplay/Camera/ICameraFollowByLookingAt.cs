using UnityEngine;

namespace Pelki.Gameplay.Camera
{
    public interface ICameraFollowByLookingAt
    {
        Transform FollowRoot { get; }
        bool IsLookingRight { get; }
    }
}