using UnityEngine;

public class NonTargetSkill : Skill
{
    private PlayerController _player;

    protected override void ActivateSkill()
    {
        //transform.position = _player.Center;
        gameObject.SetActive(true);
        StartCoroutine(DeActivateSkill());
    }
}
