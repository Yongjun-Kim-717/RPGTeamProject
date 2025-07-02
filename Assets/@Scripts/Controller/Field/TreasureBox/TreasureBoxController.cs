
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreasureBoxController : MonoBehaviour
{
    Define.TreasureRewardType[]  _treasureRewardArray;
    Animator _animator;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _treasureRewardArray = (Define.TreasureRewardType[])Enum.GetValues(typeof(Define.TreasureRewardType));
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Define.PlayerTag))
        {
            _animator.SetTrigger(Define.Contact);
            OpenTreasureBox();
        }
    }

    // * 보물상자를 열었을 때 실행되는 함수
    void OpenTreasureBox()
    {
        //랜덤 보상 개수
        int rewardCount = UnityEngine.Random.Range(0, 6);

        List<Define.TreasureRewardType> getRewardList = new();
        for (int i = 0; i < rewardCount;)
        {
            int index = UnityEngine.Random.Range(0, _treasureRewardArray.Length);
            if (getRewardList.Contains(_treasureRewardArray[index]))
                continue;

            getRewardList.Add(_treasureRewardArray[index]);
            i++;
        }

        UpdatePlayer(getRewardList);

        getRewardList = null;
    }

    //PlayerData와 PlayerInventoryData 업그레이드
    void UpdatePlayer(List<Define.TreasureRewardType> list)
    {
        
    }
}
