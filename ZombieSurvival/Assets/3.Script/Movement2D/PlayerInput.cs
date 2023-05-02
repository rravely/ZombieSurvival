using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private string moveAxisName = "Vertical";
    [SerializeField] private string rotateAxisName = "Horizontal";
    [SerializeField] private string fire = "Fire1";
    [SerializeField] private string reload = "Reload";

    //GetAxis -> return float �ڷ���
    public float moveValue { get; private set; }
    public float rotateValue { get; private set; }

    //GetButton -> return bool �ڷ���
    public bool isFire { get; private set; }
    public bool isReload { get; private set; }

    // Update is called once per frame
    void Update()
    {
        //gameover �� ����Ǿ��� �� �������� ���� �޼ҵ� �����

        moveValue = Input.GetAxis(moveAxisName);
        rotateValue = Input.GetAxis(rotateAxisName);
        isFire = Input.GetButton(fire);
        isReload = Input.GetButton(reload);
    }
}
