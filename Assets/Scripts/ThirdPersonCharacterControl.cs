using UnityEngine;

public class ThirdPersonCharacterControl : MonoBehaviour
{
    [SerializeField] private float _speed = 5;

    private void Update ()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");
        var run = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;

        Debug.Log(run);
        var playerMovement = new Vector3(hor, 0f, ver) * (_speed * run * Time.deltaTime);
        transform.Translate(playerMovement, Space.Self);
    }
}