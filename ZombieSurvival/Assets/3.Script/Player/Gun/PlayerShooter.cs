using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    /*�� ������Ʈ �տ� ���߱�
     * PlayerInput ������Ʈ �ʿ�
     * �޸ӳ��̵� ���� -> �ִϸ������� Avatar
    */

    public Gun gun;

    //to fit gun in player
    [Header("Gun fit")]
    public Transform gunpivot;
    public Transform leftHandMount;
    public Transform rightHandMount;

    [Header("Component")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Animator playerAni;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //input ���� �̺�Ʈ
        if (playerInput.isFire)
        {
            gun.Fire();
        }
        else if (playerInput.isReload)
        {
            if (gun.Reload())
            {
                playerAni.SetTrigger("Reload");
            }
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //���� �������� ������ �Ȳ�ġ�� �̵�
        gunpivot.position = playerAni.GetIKHintPosition(AvatarIKHint.RightElbow);

        //IK�� ����Ͽ� �޼��� ��ġ�� ȸ���� �� �ܼ� �����̿� ����
        playerAni.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        playerAni.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        playerAni.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAni.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        //IK�� ����Ͽ� �޼��� ��ġ�� ȸ���� �� �ܼ� �����̿� ����
        playerAni.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAni.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAni.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAni.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}
