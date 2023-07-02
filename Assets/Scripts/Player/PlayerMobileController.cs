using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//управление игрой для телефона
public class PlayerMobileController : MonoBehaviour
{
    [SerializeField] DynamicJoystick joystick;
    [SerializeField] List<Button> buttonAbilities = new List<Button>();

    PlayerManager playerManager;
    

    private void Awake()
    {
        
        playerManager = GetComponent<PlayerManager>();
    }

    private void FixedUpdate()
    {
        Move();
        if (GameManager.haveCompanion && GameManager.energy > 0)
        {
            EnebleAbilities(true);
        }
        else
        {
            EnebleAbilities(false);
        }
    }

    void EnebleAbilities(bool b)
    {
        foreach (Button button in buttonAbilities)
        {
            button.gameObject.SetActive(b);
        }
    }

    void Move()
    {
        playerManager.move = joystick.Horizontal;
        
    }

    public void ClickJoystick(bool click)
    {
        playerManager.ClickJoystick(click);
    }


    public void Jump()
    {
        StartCoroutine(ToJump());
           
    }

    IEnumerator ToJump()
    {
        playerManager.jump = true;
        yield return new WaitForEndOfFrame();
        playerManager.jump = false;

    }

    public void Hit()
    {
        StartCoroutine(ToHit());
    }

    IEnumerator ToHit()
    {
        playerManager.hit = true;
        yield return new WaitForEndOfFrame();
        playerManager.hit = false;
    }

    public void Fire()
    {
        StartCoroutine(ToFire());
    }

    IEnumerator ToFire()
    {
        playerManager.fire = true;
        yield return new WaitForEndOfFrame();
        playerManager.fire = false;
    }

    public void Dash()
    {
        StartCoroutine(ToDash());
    }

    IEnumerator ToDash()
    {
        playerManager.dash = true;
        yield return new WaitForEndOfFrame();
        playerManager.dash = false;

    }
}
