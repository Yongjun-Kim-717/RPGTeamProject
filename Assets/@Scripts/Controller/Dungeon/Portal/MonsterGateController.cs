using System;
using System.Collections;
using UnityEngine;

public class MonsterGateController : MonoBehaviour
{
    public Action<Vector3> OnGateOpen;

    private void OnEnable()
    {
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        StartCoroutine(SetGate());
    }

    public void SetRotation(Vector3 dir)
    {
        transform.rotation = Quaternion.LookRotation(dir - transform.position, Vector3.down);
    }

    IEnumerator SetGate()
    {
        Vector3 targetScale = new Vector3(3, 3, 3);
        while(true)
        {
            yield return null;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 1f);
            if(Vector3.Distance(transform.localScale, targetScale) <= 1f)
                break;
        }
        OnGateOpen?.Invoke(transform.position);
        targetScale = new Vector3(0.01f, 0.01f, 0.01f);
        while (true)
        {
            yield return null;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 1f);
            if (Vector3.Distance(transform.localScale, targetScale) <= 1f)
                break;
        }
        gameObject.SetActive(false);
    }
}
