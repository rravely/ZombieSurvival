using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    /*건 오브젝트 손에 맞추기
     * PlayerInput 컴포넌트 필요
     * 휴머노이드 관리 -> 애니메이터의 Avatar
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
        //input 관련 이벤트
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
        //총의 기준점을 오른쪽 팔꿈치로 이동
        gunpivot.position = playerAni.GetIKHintPosition(AvatarIKHint.RightElbow);

        //IK를 사용하여 왼손의 위치와 회전을 총 외손 손잡이에 맞춤
        playerAni.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        playerAni.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        playerAni.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAni.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        //IK를 사용하여 왼손의 위치와 회전을 총 외손 손잡이에 맞춤
        playerAni.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAni.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAni.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAni.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}
