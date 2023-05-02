using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Gundata", fileName = "Gun Data")]
public class GunData : ScriptableObject
{
    /*
     공격력
     연사력
     재장전 시간
     탄창용량
     처음 주어질 전체 총알량
     총소리
     재장전 소리
     */

    public float damage = 25f; //공격력

    public float TimebetFire = 0.12f; //연사력
    public float ReloadTime = 1.8f; //재장전 시간

    public int magCapacity = 30; //탄창용량
    public int StartAmmoRemaion = 100; //처음 주어질 전체 총알량

    public AudioClip ShotClip;
    public AudioClip ReloadClip;
}
