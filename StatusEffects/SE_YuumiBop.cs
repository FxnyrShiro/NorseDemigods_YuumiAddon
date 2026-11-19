using UnityEngine;

namespace NorseDemigods.Yuumi.StatusEffects;

/// <summary>
///     "Feline Friendship" — the passive proc: a short armor bonus plus healing.
/// </summary>
public class SE_YuumiBop : SE_Stats
{
    internal static int? StatusEffectHash;

    private GameObject _fx;

    public SE_YuumiBop()
    {
        name = "SE_YuumiBop";
        m_name = YuumiLocalization.T("$se_yuumi_bop_name");
        m_icon = YuumiIcons.ToSprite(YuumiIcons.Passive);
        if (!StatusEffectHash.HasValue)
        {
            StatusEffectHash = NameHash();
        }
    }

    public void Setup(Character character, float duration, float armorBonus)
    {
        m_character = character;
        m_ttl = duration;
        m_addArmor = armorBonus;
        m_tooltip = Helper.SafeFormat(YuumiLocalization.T("$se_yuumi_bop_tooltip"), armorBonus.ToString("0"));

        if (Resources.FxHolyAura != null)
        {
            _fx = Helper.SpawnGameObject(Resources.FxHolyAura, m_character.GetCenterPoint(), Quaternion.identity);
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

    public static void ApplyTo(Character target, float duration, float armorBonus)
    {
        if (target == null)
        {
            return;
        }

        if (StatusEffectHash.HasValue && target.GetSEMan().HaveStatusEffect(StatusEffectHash.Value))
        {
            target.GetSEMan().RemoveStatusEffect(StatusEffectHash.Value);
        }

        SE_YuumiBop statusEffect = (SE_YuumiBop)target.GetSEMan().AddStatusEffect(
            (SE_YuumiBop)ScriptableObject.CreateInstance(typeof(SE_YuumiBop)), false, 0, 0f, -1);
        statusEffect?.Setup(target, duration, armorBonus);
    }
}
