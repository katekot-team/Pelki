using UnityEngine;

public class PlayerPCController : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;

    private void Update()
    {
        playerManager.move = Input.GetAxis("Horizontal");
        playerManager.jump = Input.GetButtonDown("Jump");
        playerManager.dash = Input.GetButtonDown("Dash");
        playerManager.hit = Input.GetButtonDown("Fire1");
        playerManager.fire = Input.GetButtonDown("Fire2");
    }
}
