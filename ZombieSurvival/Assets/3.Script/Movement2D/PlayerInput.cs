using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private string moveAxisName = "Vertical";
    [SerializeField] private string rotateAxisName = "Horizontal";
    [SerializeField] private string fire = "Fire1";
    [SerializeField] private string reload = "Reload";

    //GetAxis -> return float 자료형
    public float moveValue { get; private set; }
    public float rotateValue { get; private set; }

    //GetButton -> return bool 자료형
    public bool isFire { get; private set; }
    public bool isReload { get; private set; }

    // Update is called once per frame
    void Update()
    {
        //gameover 가 선언되었을 때 움직임을 막는 메소드 만들기

        moveValue = Input.GetAxis(moveAxisName);
        rotateValue = Input.GetAxis(rotateAxisName);
        isFire = Input.GetButton(fire);
        isReload = Input.GetButton(reload);
    }
}
