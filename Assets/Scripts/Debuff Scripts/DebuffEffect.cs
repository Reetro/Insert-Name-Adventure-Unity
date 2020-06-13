using System.Collections;
using UnityEngine;

public class DebuffEffect : MonoBehaviour
{
    private float ticks = 0;
    private float occurrence = 0;
    private AuraManager auraManger = null;
    private ScriptableDebuff debuff = null;
    private GameObject target = null;

    protected DebuffIcon icon = null;

    private bool firstRun = false;

    public virtual void StartDebuff(float ticks, float occurrence, AuraManager auraManger, ScriptableDebuff debuff, DebuffIcon icon, GameObject target)
    {
        this.ticks = ticks;
        this.occurrence = occurrence;
        this.auraManger = auraManger;
        this.debuff = debuff;
        this.icon = icon;

        firstRun = true;

        StartCoroutine(DebuffTimer());
    }

    public virtual void StartDebuff(float ticks, float occurrence, AuraManager auraManger, ScriptableDebuff debuff, GameObject target)
    {
        this.ticks = ticks;
        this.occurrence = occurrence;
        this.auraManger = auraManger;
        this.debuff = debuff;

        firstRun = true;

        StartCoroutine(DebuffTimer());
    }

    private IEnumerator DebuffTimer()
    {
        if (firstRun)
        {
            ApplyDebuffEffect();

            firstRun = false;
        }

        while (ticks > 0)
        {
            yield return new WaitForSecondsRealtime(occurrence);
            ticks--;

            ApplyDebuffEffect();
        }

        if (ticks <= 0)
        {
            OnDebuffEnd();
        }
    }

    public virtual void ApplyDebuffEffect()
    {
        // To be overridden in child
        Debug.LogWarning("Debuff Effect: " + gameObject.name + "has no debuff effect being applied");
    }

    public virtual void OnDebuffEnd()
    {
        if (icon)
        {
            auraManger.RemoveDebuff(gameObject, this, icon);
        }
        else
        {
            auraManger.RemoveDebuff(gameObject, this);
        }
    }

    public GameObject GetTarget()
    {
        return target;
    }

    public float GetTicks()
    {
        return ticks;
    }

    public float GetOccurrence()
    {
        return occurrence;
    }

    public AuraManager GetAuraManager()
    {
        return auraManger;
    }

    public ScriptableDebuff GetDebuff()
    {
        return debuff;
    }
}
