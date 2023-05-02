using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Gundata", fileName = "Gun Data")]
public class GunData : ScriptableObject
{
    /*
     ���ݷ�
     �����
     ������ �ð�
     źâ�뷮
     ó�� �־��� ��ü �Ѿ˷�
     �ѼҸ�
     ������ �Ҹ�
     */

    public float damage = 25f; //���ݷ�

    public float TimebetFire = 0.12f; //�����
    public float ReloadTime = 1.8f; //������ �ð�

    public int magCapacity = 30; //źâ�뷮
    public int StartAmmoRemaion = 100; //ó�� �־��� ��ü �Ѿ˷�

    public AudioClip ShotClip;
    public AudioClip ReloadClip;
}
