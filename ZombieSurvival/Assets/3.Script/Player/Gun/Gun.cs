using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    /*
        ���� ����
            ������
            źâ�� ����� ��
            �߻� �غ�
        �߻�� ��ġ
        �Ѿ� linerenderer
        �� �߻�  audio
        �� ��Ÿ�
        
        ���� �� data
        �޼� ������ ������
        
        effect -> particle system

        method
            Fire
            Reload
            EffectPlay
     */

    //���� ����
    public enum State
    {
        Ready, //�߻��غ� �Ϸ�
        Empty, //źâ�� ��
        Reloading //������
    }

    public State state { get; private set; }

    //�߻�� ��ġ
    public Transform Fire_transform;
    //�Ѿ� lineRenderer
    private LineRenderer BulletLineRenderer;
    //�� �߻� audio
    private AudioSource audio;
    //�� ��Ÿ�
    private float distance = 50f;
    //���� �� ������
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
        BulletLineRenderer.enabled = false; //������Ʈ�� Ȱ��ȭ ���¸� �����ϴ� �Լ�
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
        //�÷��̾��� �� ���°� �غ�����̰�, ������ �߻�ð��� ���� �ð����� ���� �� �߻��ϰ� 
        if (state.Equals(State.Ready) && Time.time >= lastFireTime + data.TimebetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }
        
    }

    public void Shot()
    {
        //�Ѿ��� ���� ��ü�� �����ϰų� ���� ��ü�� ������ ������ ���� �޼��� -> raycast 
        RaycastHit hit;
        //raycast�� ���� Ÿ���� ���� �ϱ� ���� ����
        Vector3 hitPosition = Vector3.zero; //
        
        if (Physics.Raycast(Fire_transform.position, Fire_transform.forward, out hit, distance))
        {
            //�Ѿ��� �¾��� ���
            //�츮�� ���� �������̽��� ������ �ͼ� ���� ������Ʈ���� �������� ������.
            IDamage target = hit.collider.GetComponent<IDamage>();

            if (target != null)
            {
                target.OnDamage(data.damage, hit.point, hit.normal);
            }
            hitPosition = hit.point;

            //damage�� �Դ� �÷��̾�� ����
        }
        else
        {
            //ray�� �ٸ� ��ü�� �浹���� �ʾҴٸ� or ź���� �ִ� ���� �Ÿ����� ���ư��� �� ��ġ�� �浹 ��ġ�� ���
            hitPosition = Fire_transform.position + Fire_transform.forward * distance;
        }

        //���� �� ����Ʈ �÷���
        StartCoroutine(ShotEffect(hitPosition));
        magAmmo--;
        if (magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    //���η����� �����ð� ���� ��Ÿ���� �ߴٰ� �ٽ� �������� �ϱ� ���ؼ� �ڷ�ƾ ���
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        //�ѿ� ���õ� ��ƼŬ �ý��� �÷���
        shotEffect.Play();
        shellEffect.Play();

        audio.PlayOneShot(data.ShotClip);

        //���� ������ ����
        BulletLineRenderer.SetPosition(0, Fire_transform.position);
        BulletLineRenderer.SetPosition(1, hitPosition);

        BulletLineRenderer.enabled = true;
        yield return new WaitForSeconds(0.03f);
        BulletLineRenderer.enabled = false;
    }

    //Reload
    //���� reload ������ �������� Ȯ���� �޼��� 

    public bool Reload()
    {
        //���� �������� �ʿ����� ������ return�� �޼���
        //�̹� ������ ���̰ų�, �Ѿ��� ���ų�, źâ�� �̹� �Ѿ��� ���� �� ���(30���� ���)
        if (state.Equals(State.Reloading) || ammioRemain <= 0 || magAmmo >= data.magCapacity)
        {
            return false;
        }

        //�� ������
        StartCoroutine("Reload_co");
        return true;
    }

    private IEnumerator Reload_co()
    {
        //�������� �������� �ϴ� �ڷ�ƾ
        state = State.Reloading;
        audio.PlayOneShot(data.ReloadClip);

        //������ �ϴ� �ð� ����
        yield return new WaitForSeconds(data.ReloadTime);

        //������ �� ����ϰų� ����Ǿ� �� �͵�
        //��ü źâ���� ���� ź�� ���
        int ammofill = data.magCapacity - magAmmo;
        //źâ�� ä������ ź���� ���� ź�ຸ�� ���ٸ� ä������ ź����� ���� ź����� ���� ���δ�.
        if (ammioRemain < ammofill)
        {
            ammofill = ammioRemain;
        }
        //źâ ä���
        magAmmo += ammofill;
        ammioRemain -= ammofill;
        state = State.Ready;
    }
}
