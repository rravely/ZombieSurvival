using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 180f;

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private Camera camera;

    private Rigidbody player_R;
    private Animator player_ani;

    private Vector3 playerPos = new Vector3(955f, 580f, 0f);
    private Vector3 direction = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out player_R);
        TryGetComponent(out player_ani);
        TryGetComponent(out playerInput);
        
        /*
         * 만약 컴포넌트가 없을 경우 추가하는 법
        if (!TryGetComponent(out player_R))
        {
            gameObject.AddComponent<Rigidbody>();
            TryGetComponent(out player_R);
        }
        */
    }
    void Update()
    {
        float angle = AngleBetweenTwoPoints(playerPos, Input.mousePosition);
        transform.rotation = Quaternion.Euler(new Vector3(0f, -angle - 45f, 0f));
    }


    private void FixedUpdate()
    {
        Move();
        Rotate();

        player_ani.SetFloat("Move", playerInput.moveValue);
    }

    public void Move()
    {
        Vector3 moveDirection = playerInput.moveValue * transform.forward * moveSpeed * Time.deltaTime;

        player_R.MovePosition(player_R.position + moveDirection);
    }
    public void Rotate()
    {
        float turn = playerInput.rotateValue * rotateSpeed * Time.deltaTime;

        player_R.rotation = player_R.rotation * Quaternion.Euler(0, turn, 0);
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
