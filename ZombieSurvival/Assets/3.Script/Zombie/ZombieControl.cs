using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieControl : LivingEntity
{
    //추적대상 레이어
    [Header("Target Layer")]
    public LayerMask targetLayer;
    private LivingEntity targetLiving;

    //경로를 계산한 AI Agent 필요 
    private NavMeshAgent navAgent;

    //AudioClip
    [Header("Effect")]
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private ParticleSystem hitEffect;

    private Animator enemyAni;
    private AudioSource enemyAudio;
    private Renderer enemyRenderer;

    [SerializeField] private float damage = 20f;
    [SerializeField] private float TimeBetAttack = 0.5f; //공격 속도
    private float lastAttackTimeBet;

    private bool isTarget
    {
        get
        {
            //프로퍼티 메서드 만들기
            if (targetLiving != null && !targetLiving.isDead)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    void Awake()
    {
        TryGetComponent(out navAgent);
        TryGetComponent(out enemyAni);
        TryGetComponent(out enemyAudio);
        enemyRenderer = GetComponentInChildren<Renderer>(); 
        //GetComponentInChildren: 자식 객체 중에 가장 선두에 존재하는 컴포넌트 반환
        //GetComponentsInchildren: 하위 객체의 컴포넌트를 모두 반환. 이때 반환 형태는 배열
    }

    public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        //플레이어 총알에 맞았을 때 실행
        //총알에 맞으면 hit Effect가 총알이 날아온 방향을 바라보도록
        if (!isDead)
        {
            hitEffect.transform.position = hitPosition;
            //해당 회전값을 바라보는 회전 상태로 변환
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();
            enemyAudio.PlayOneShot(hitClip);
        }
        base.OnDamage(damage, hitPosition, hitNormal);
    }

    public override void Die()
    {
        base.Die();

        Collider[] colliders = GetComponents<Collider>();
        foreach(Collider c in colliders)
        {
            c.enabled = false;
        }
        navAgent.isStopped = true; //target 그만 찾아다니기
        navAgent.enabled = false;
    }

    private void OnTriggerStay(Collider collider)
    {
        //닿고 있을 때 지속적으로 호출
        if (!isDead && Time.time >= lastAttackTimeBet + TimeBetAttack)
        {
            if (collider.TryGetComponent(out LivingEntity en))
            {
                if (targetLiving.Equals(en)) //좀비끼리 충돌하는 것이 아닌 플레이어와 충돌
                {
                    lastAttackTimeBet = Time.time;
                    //ClosestPoint -> 피격 위치와 피격 방향을 근사값으로 계산
                    Vector3 hitPoint = collider.ClosestPoint(transform.position);

                    Vector3 hitNormal = transform.position - collider.transform.position;
                    //Player damage 메서드 호출
                    en.OnDamage(damage, hitPoint, hitNormal);
                }
            }
        }
    }

    public void SetUp(ZombieData data)
    {
        startHealth = data.health;
        damage = data.damage;

        //nav agent의 speed관련 변수 가지고 와서 그 변수에 맞게 설정
        navAgent.speed = data.speed;
        enemyRenderer.material.color = data.skinColor;
    }

    void Start()
    {
        StartCoroutine(UpdateTargetPosition());
    }

    void Update()
    {
        enemyAni.SetBool("HasTarget", isTarget);
    }

    //추적할 대상이 위치를 주기적으로 찾아서 갱신 작업
    private IEnumerator UpdateTargetPosition()
    {
        while(!isDead)
        {
            if (isTarget)
            {
                navAgent.isStopped = false;
                navAgent.SetDestination(targetLiving.transform.position);
            }
            else
            {
                navAgent.isStopped = true;
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, targetLayer);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].TryGetComponent(out LivingEntity en))
                    {
                        if (!en.isDead)
                        {
                            targetLiving = en;
                            break;
                        }
                    }
                }
            }
            //yield return new WaitForSeconds(0.25f); 도 가능
            yield return null;
        }
    }
}
