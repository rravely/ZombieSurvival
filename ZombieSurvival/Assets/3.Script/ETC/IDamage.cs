using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    //매개변수: 피해량, 맞은 위치, 맞은 위치의 각도
    void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal); 
}
