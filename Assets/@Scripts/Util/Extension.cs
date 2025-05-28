using System.Collections.Generic;
using UnityEngine;

namespace extension
{
    public static class Extension
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
                component = go.AddComponent<T>();
            return component;
        }

        // * GameObject 배열을 주어진 리스트에 복사 (주의 : 기존 데이터는 삭제)
        public static void ToList(this GameObject[] goArr, Dictionary<string, GameObject> list)
        {
            list.Clear();
            foreach (GameObject go in goArr)
            {
                list.Add(go.name, go);
            }
        }

        public static bool NullCheck<T>(params T[] tArr)
        {
            foreach(T t in tArr)
            {
                if (t == null)
                {
                    Debug.Log(typeof(T) + " : Null Reference Exception");
                    return true;
                }
            }
            return false;
        }
    }
}