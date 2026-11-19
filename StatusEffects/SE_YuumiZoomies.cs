using UnityEngine;

namespace NorseDemigods.Yuumi.StatusEffects;

/// <summary>
///     "Zoomies" — a short burst of speed, damage and healing.
/// </summary>
public class SE_YuumiZoomies : SE_Stats
{
    internal static int? StatusEffectHash;

    private GameObject _fx;

    public SE_YuumiZoomies()
    {
        name = "SE_YuumiZoomies";
        m_name = YuumiLocalization.T("$se_yuumi_zoomies_name");
        m_icon = YuumiIcons.ToSprite(YuumiIcons.E);
        if (!StatusEffectHash.HasValue)
        {
            StatusEffectHash = NameHash();
        }
    }

    public void Setup(Character character, float duration, float heal, float speedBonus, float damageBonus)
    {
        m_character = character;
        m_ttl = duration;
        m_speedModifier = speedBonus / 100f;
        m_damageModifier = 1f + damageBonus / 100f;
        m_tooltip = Helper.SafeFormat(YuumiLocalization.T("$se_yuumi_zoomies_tooltip"),
            speedBonus.ToString("0"), damageBonus.ToString("0"));

        if (heal > 0f && character != null && character.GetHealth() < character.GetMaxHealth())
        {
            character.Heal(heal);
        }

        if (Resources.FxWindHeal != null)
        {
            _fx = Helper.SpawnGameObject(Resources.FxWindHeal, m_character.GetCenterPoint(), Quaternion.identity);
            if (_fx != null)
            {
                _fx.transform.SetParent(m_character.transform);
            }
        }
    }

    public override void Stop()
    {
        if (_fx != null)
        {
            _fx.transform.SetParent(null);
            Helper.DestroyFx(_fx);
            _fx = null;
        }

        base.Stop();
    }

    public static void ApplyTo(Character target, float duration, float heal, float speedBonus, float damageBonus)
    {
        if (target == null)
        {
            return;
        }

        ZNetView nview = target.GetComponent<ZNetView>();
        if (nview != null && !nview.IsOwner())
        {
            nview.InvokeRPC(YuumiRpc.ApplyZoomies, target.GetZDOID(), duration, heal, speedBonus, damageBonus);
            return;
        }

        Add(target, duration, heal, speedBonus, damageBonus);
    }

    internal static void Add(Character target, float duration, float heal, float speedBonus, float damageBonus)
    {
        if (StatusEffectHash.HasValue && target.GetSEMan().HaveStatusEffect(StatusEffectHash.Value))
        {
            target.GetSEMan().RemoveStatusEffect(StatusEffectHash.Value);
        }

        SE_YuumiZoomies statusEffect = (SE_YuumiZoomies)target.GetSEMan().AddStatusEffect(
            (SE_YuumiZoomies)ScriptableObject.CreateInstance(typeof(SE_YuumiZoomies)), false, 0, 0f, -1);
        statusEffect?.Setup(target, duration, heal, speedBonus, damageBonus);
    }
}
