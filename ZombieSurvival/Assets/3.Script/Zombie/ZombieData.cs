using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ZombieData", fileName = "ZombieData")]
public class ZombieData : ScriptableObject
{
    /*
     * ü��
     * �̵��ӵ�
     * ���ݷ�
     * �Ǻλ�
     */

    public float health = 100f;
    public float damage = 20f;
    public float speed = 2f;

    public Color skinColor = Color.white;
}
