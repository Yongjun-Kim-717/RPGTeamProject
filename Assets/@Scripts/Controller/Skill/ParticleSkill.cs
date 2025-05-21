using UnityEngine;

public class ParticleSkill : MonoBehaviour
{
    /// <summary>
    /// test�� - �Ƹ��� ���� ����
    /// </summary>
    /// <param name="other"></param>
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag(Define.MonsterTag))
        {
            Debug.Log($"Particle Collision with {other.name}");
        }
    }

    private void OnParticleTrigger()
    {
        Debug.Log("�浹");
    }
}
