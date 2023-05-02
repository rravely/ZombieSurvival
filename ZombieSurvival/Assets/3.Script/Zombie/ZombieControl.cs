using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieControl : LivingEntity
{
    //������� ���̾�
    [Header("Target Layer")]
    public LayerMask targetLayer;
    private LivingEntity targetLiving;

    //��θ� ����� AI Agent �ʿ� 
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
    [SerializeField] private float TimeBetAttack = 0.5f; //���� �ӵ�
    private float lastAttackTimeBet;

    private bool isTarget
    {
        get
        {
            //������Ƽ �޼��� �����
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
        //GetComponentInChildren: �ڽ� ��ü �߿� ���� ���ο� �����ϴ� ������Ʈ ��ȯ
        //GetComponentsInchildren: ���� ��ü�� ������Ʈ�� ��� ��ȯ. �̶� ��ȯ ���´� �迭
    }

    public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        //�÷��̾� �Ѿ˿� �¾��� �� ����
        //�Ѿ˿� ������ hit Effect�� �Ѿ��� ���ƿ� ������ �ٶ󺸵���
        if (!isDead)
        {
            hitEffect.transform.position = hitPosition;
            //�ش� ȸ������ �ٶ󺸴� ȸ�� ���·� ��ȯ
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
        navAgent.isStopped = true; //target �׸� ã�ƴٴϱ�
        navAgent.enabled = false;
    }

    private void OnTriggerStay(Collider collider)
    {
        //��� ���� �� ���������� ȣ��
        if (!isDead && Time.time >= lastAttackTimeBet + TimeBetAttack)
        {
            if (collider.TryGetComponent(out LivingEntity en))
            {
                if (targetLiving.Equals(en)) //���񳢸� �浹�ϴ� ���� �ƴ� �÷��̾�� �浹
                {
                    lastAttackTimeBet = Time.time;
                    //ClosestPoint -> �ǰ� ��ġ�� �ǰ� ������ �ٻ簪���� ���
                    Vector3 hitPoint = collider.ClosestPoint(transform.position);

                    Vector3 hitNormal = transform.position - collider.transform.position;
                    //Player damage �޼��� ȣ��
                    en.OnDamage(damage, hitPoint, hitNormal);
                }
            }
        }
    }

    public void SetUp(ZombieData data)
    {
        startHealth = data.health;
        damage = data.damage;

        //nav agent�� speed���� ���� ������ �ͼ� �� ������ �°� ����
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

    //������ ����� ��ġ�� �ֱ������� ã�Ƽ� ���� �۾�
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
            //yield return new WaitForSeconds(0.25f); �� ����
            yield return null;
        }
    }
}
