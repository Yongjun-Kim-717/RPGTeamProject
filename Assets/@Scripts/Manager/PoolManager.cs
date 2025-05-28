using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    // 오브젝트 풀링 리스트 (List의 탐색 시간을 낮춰보자)
    Dictionary <string, List<GameObject>> _poolList;
    // 오브젝트 풀링 관리 변수
    Dictionary<string, GameObject> _parentObjectList;

    protected override void Initialize()
    {
        base.Initialize();
        _poolList = new Dictionary<string, List<GameObject>>();
        _parentObjectList = new Dictionary<string, GameObject>();
    }

    public GameObject GetObjectFromPool<T>(Vector3 spawnPos, string name) where T : MonoBehaviour
    {
        // 오브젝트 풀 리스트에 해당 name의 오브젝트 리스트가 존재하는지 검사
        if (_poolList.ContainsKey(name))
        {
            // 해당 name의 풀 리스트 모든 오브젝트 검사
            for(int i = 0; i<_poolList[name].Count; i++)
            {
                // 풀 리스트의 오브젝트 활성화 여부 검사 및 비활성화 객체 활성화 및 좌표 초기화
                if(!_poolList[name][i].activeSelf)
                {
                    _poolList[name][i].SetActive(false);
                    _poolList[name][i].transform.position = spawnPos;

                    return _poolList[name][i];
                }
            }
            // 존재하지 않으므로 오브젝트 매니저의 Spawn 메서드로 동적 생성 및 풀 리스트 등록
            GameObject obj = ObjectManager.Instance.GetObject<T>(spawnPos, name);
            obj.transform.SetParent(_parentObjectList[name].transform, false);
            _poolList[name].Add(obj);
            return obj;
        }
        // 오브젝트 풀 리스트에 해당 name의 오브젝트 리스트가 존재하지 않을 시
        else
        {
            // 풀링 관리 딕셔너리에 해당 타입 오브젝트가 존재하지 않을 시
            if(!_parentObjectList.ContainsKey(name))
            {
                GameObject go = new GameObject(name + "Pool");
                _parentObjectList.Add(name, go);
            }
            // 동적으로 오브젝트 생성 후 풀링리스트 동적 생성 및 추가
            var obj = ObjectManager.Instance.GetObject<T>(spawnPos, name);
            obj.transform.SetParent(_parentObjectList[name + "Pool"].transform, false);
            List<GameObject> newList = new List<GameObject>();
            newList.Add(obj);
            _poolList.Add(name, newList);
            return obj;
        }
    }
}
