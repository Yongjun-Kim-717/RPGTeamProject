using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextManager : Singleton<DamageTextManager>, IEventSubscriber, IDeactivateObject
{
    private Dictionary<string, GameObject> _damageTextList;
    [SerializeField] private Canvas _canvas;

    protected override void Initialize()
    {
        _damageTextList = new Dictionary<string, GameObject>();
        _canvas = GameObject.Find("UI_DamageText").GetComponent<Canvas>();
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        DamageTextEvent.OnDamageTaken += ActivateDamageText;
    }
    #endregion
    #region IDeactivateObject
    public void Deactivate()
    {
        foreach(GameObject go in _damageTextList.Values)
        {
            go.SetActive(false);
        }
    }
    #endregion

    void ActivateDamageText(Vector3 pos, float damage, bool isCritical)
    {
        GameObject damageText = PoolManager.Instance.GetObjectFromPool<DamageTextController>
            (pos, isCritical ? "CriticalDamageText" : "NormalDamageText");
        damageText.GetComponent<DamageTextController>()._originPos = pos;
        TMP_Text textMeshPro = damageText.GetComponent<TMP_Text>();
        textMeshPro.text = damage.ToString();
        damageText.transform.SetParent(_canvas.transform, true);
    }
}
