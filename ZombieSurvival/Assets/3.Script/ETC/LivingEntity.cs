using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamage
{
    /*
     * initHealth -> 초기 체력
     * curHealth -> 현재 체력
     * 죽었는지 살았는지 -> 데미지 받는다(IDamage)
     * 죽었을 때 이벤트 발생할 무언가
     */

    public float startHealth = 100f;
    //이 스크립트는 상위 객체이므로 하위 객체에서 접근할 수 있도록 접근 제한자 protected 사용
    public float health { get; protected set; }
    public bool isDead { get; protected set; }

    public event Action onDeath;

    protected virtual void OnEnable()
    {
        //변수 초기화
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
            //죽는 메서드
            Die();
        }
    }

    public virtual void Die()
    {
        if (onDeath != null) //이벤트 null check
        {
            onDeath(); //이벤트 호출
        }

        isDead = true;
    }

    public virtual void RestoreHealth(float newHealth) 
    {
        //체력을 증가시키는 이벤트 발생시 호출할 메서드 제작
        if (isDead)
        {
            return;
        }
        health += newHealth;
    }
}
