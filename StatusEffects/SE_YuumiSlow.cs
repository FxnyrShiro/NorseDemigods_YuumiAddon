using UnityEngine;

namespace NorseDemigods.Yuumi.StatusEffects;

/// <summary>
///     The slow applied by Prowling Projectile and Final Chapter.
/// </summary>
public class SE_YuumiSlow : SE_Stats
{
    internal static int? StatusEffectHash;

    public SE_YuumiSlow()
    {
        name = "SE_YuumiSlow";
        m_name = YuumiLocalization.T("$se_yuumi_slow_name");
        m_icon = YuumiIcons.ToSprite(YuumiIcons.Q);
        if (!StatusEffectHash.HasValue)
        {
            StatusEffectHash = NameHash();
        }
    }

    public void Setup(Character character, float slowPercent, float duration)
    {
        m_character = character;
        m_ttl = duration;
        m_speedModifier = -Mathf.Clamp(slowPercent, 0f, 90f) / 100f;
        m_tooltip = Helper.SafeFormat(YuumiLocalization.T("$se_yuumi_slow_tooltip"), slowPercent.ToString("0"));
    }

    public static void ApplyTo(Character target, float slowPercent, float duration)
    {
        if (target == null || duration <= 0f)
        {
            return;
        }

        if (StatusEffectHash.HasValue && target.GetSEMan().HaveStatusEffect(StatusEffectHash.Value))
        {
            target.GetSEMan().RemoveStatusEffect(StatusEffectHash.Value);
        }

        SE_YuumiSlow statusEffect = (SE_YuumiSlow)target.GetSEMan().AddStatusEffect(
            (SE_YuumiSlow)ScriptableObject.CreateInstance(typeof(SE_YuumiSlow)), false, 0, 0f, -1);
        statusEffect?.Setup(target, slowPercent, duration);
    }
}
