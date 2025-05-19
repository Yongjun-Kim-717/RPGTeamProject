using System.Collections;
using UnityEngine;

public class SkillTest : MonoBehaviour
{
    bool _isCoolTime = false;
    float _currentTime = 0f;
    const float _coolTime = 4f;
    const float _duration = 3f;
    PlayerController _player;

    void Start()
    {
        // GameManager에서 player 관리하는게
        _player = GameObject.FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        WaitCool();
    }

    void WaitCool()
    {
        if(_isCoolTime)
        {
            _currentTime += Time.deltaTime;
            if(_currentTime>=_coolTime)
            {
                _isCoolTime = false;
                _player.RemoveDictionary(this.gameObject);
            }
        }
    }

    public IEnumerator TurnOnSkill(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
        yield return new WaitForSeconds(_duration);
        TurnOffSkill();
    }
        
    void TurnOffSkill()
    {
        gameObject.SetActive(false);
        _currentTime = 0f;
        _isCoolTime = true;
        _player.AddDictionary(gameObject);
    }
}
