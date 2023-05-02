using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    /*
        총의 상태
            재장전
            탄창이 비었을 때
            발사 준비
        발사될 위치
        총알 linerenderer
        총 발사  audio
        총 사거리
        
        현재 총 data
        왼손 오른손 포지션
        
        effect -> particle system

        method
            Fire
            Reload
            EffectPlay
     */

    //총의 상태
    public enum State
    {
        Ready, //발사준비 완료
        Empty, //탄창이 빔
        Reloading //재장전
    }

    public State state { get; private set; }

    //발사될 위치
    public Transform Fire_transform;
    //총알 lineRenderer
    private LineRenderer BulletLineRenderer;
    //총 발사 audio
    private AudioSource audio;
    //총 사거리
    private float distance = 50f;
    //현재 총 데이터
    public GunData data;
    //Effect
    public ParticleSystem shotEffect;
    public ParticleSystem shellEffect;

    private float lastFireTime;

    public int ammioRemain = 100;
    public int magAmmo;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
        BulletLineRenderer = GetComponent<LineRenderer>();

        //BulletLineRenderer setting
        BulletLineRenderer.positionCount = 2;
        BulletLineRenderer.enabled = false; //컴포넌트의 활성화 상태를 변경하는 함수
    }

    void OnEnable()
    {
        ammioRemain = data.StartAmmoRemaion;
        magAmmo = data.magCapacity;

        state = State.Ready;
        lastFireTime = 0;
    }

    public void Fire()
    {
        //플레이어의 총 상태가 준비상태이고, 마지막 발사시간이 현재 시간보다 작을 때 발사하게 
        if (state.Equals(State.Ready) && Time.time >= lastFireTime + data.TimebetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }
        
    }

    public void Shot()
    {
        //총알이 닿은 물체를 저장하거나 닿은 물체의 정보를 가지고 오는 메서드 -> raycast 
        RaycastHit hit;
        //raycast를 맞은 타겟이 무언가 하기 위한 변수
        Vector3 hitPosition = Vector3.zero; //
        
        if (Physics.Raycast(Fire_transform.position, Fire_transform.forward, out hit, distance))
        {
            //총알을 맞았을 경우
            //우리가 만든 인터페이스를 가지고 와서 맞은 오브젝트에게 데미지를 입힌다.
            IDamage target = hit.collider.GetComponent<IDamage>();

            if (target != null)
            {
                target.OnDamage(data.damage, hit.point, hit.normal);
            }
            hitPosition = hit.point;

            //damage를 입는 플레이어와 좀비
        }
        else
        {
            //ray가 다른 물체와 충돌하지 않았다면 or 탄알이 최대 사정 거리까지 날아갔을 때 위치를 충돌 위치로 사용
            hitPosition = Fire_transform.position + Fire_transform.forward * distance;
        }

        //총을 쏜 이펙트 플레이
        StartCoroutine(ShotEffect(hitPosition));
        magAmmo--;
        if (magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    //라인렌더러 일정시간 동안 나타나게 했다가 다시 없어지게 하기 위해서 코루틴 사용
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        //총에 관련된 파티클 시스템 플레이
        shotEffect.Play();
        shellEffect.Play();

        audio.PlayOneShot(data.ShotClip);

        //라인 랜더러 설정
        BulletLineRenderer.SetPosition(0, Fire_transform.position);
        BulletLineRenderer.SetPosition(1, hitPosition);

        BulletLineRenderer.enabled = true;
        yield return new WaitForSeconds(0.03f);
        BulletLineRenderer.enabled = false;
    }

    //Reload
    //현재 reload 가능한 상태인지 확인할 메서드 

    public bool Reload()
    {
        //현재 재장전이 필요한지 안한지 return할 메서드
        //이미 재장전 중이거나, 총알이 없거나, 탄창에 이미 총알이 가득 찬 경우(30발인 경우)
        if (state.Equals(State.Reloading) || ammioRemain <= 0 || magAmmo >= data.magCapacity)
        {
            return false;
        }

        //총 재장전
        StartCoroutine("Reload_co");
        return true;
    }

    private IEnumerator Reload_co()
    {
        //직접적인 재장전을 하는 코루틴
        state = State.Reloading;
        audio.PlayOneShot(data.ReloadClip);

        //재장전 하는 시간 지연
        yield return new WaitForSeconds(data.ReloadTime);

        //재장전 후 계산하거나 실행되야 할 것들
        //전체 탄창에서 남은 탄약 계산
        int ammofill = data.magCapacity - magAmmo;
        //탄창에 채워야할 탄약이 남은 탄약보다 많다면 채워야할 탄약수를 남은 탄약수에 맞춰 줄인다.
        if (ammioRemain < ammofill)
        {
            ammofill = ammioRemain;
        }
        //탄창 채우기
        magAmmo += ammofill;
        ammioRemain -= ammofill;
        state = State.Ready;
    }
}
