using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    //�Ű�����: ���ط�, ���� ��ġ, ���� ��ġ�� ����
    void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal); 
}
