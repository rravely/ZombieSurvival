using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamage
{
    /*
     * initHealth -> �ʱ� ü��
     * curHealth -> ���� ü��
     * �׾����� ��Ҵ��� -> ������ �޴´�(IDamage)
     * �׾��� �� �̺�Ʈ �߻��� ����
     */

    public float startHealth = 100f;
    //�� ��ũ��Ʈ�� ���� ��ü�̹Ƿ� ���� ��ü���� ������ �� �ֵ��� ���� ������ protected ���
    public float health { get; protected set; }
    public bool isDead { get; protected set; }

    public event Action onDeath;

    protected virtual void OnEnable()
    {
        //���� �ʱ�ȭ
        isDead = false;
        health = startHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnDamage(float damage, Vector3 hitposition, Vector3 hitNormal)
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            //�״� �޼���
            Die();
        }
    }

    public virtual void Die()
    {
        if (onDeath != null) //�̺�Ʈ null check
        {
            onDeath(); //�̺�Ʈ ȣ��
        }

        isDead = true;
    }

    public virtual void RestoreHealth(float newHealth) 
    {
        //ü���� ������Ű�� �̺�Ʈ �߻��� ȣ���� �޼��� ����
        if (isDead)
        {
            return;
        }
        health += newHealth;
    }
}
