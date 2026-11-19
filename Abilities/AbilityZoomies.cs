using NorseDemigods.Yuumi.StatusEffects;
using UnityEngine;

namespace NorseDemigods.Yuumi.Abilities;

/// <summary>
///     E — Zoomies: a burst of healing, movement speed and damage.
/// </summary>
public class AbilityZoomies : DemigodAbility
{
    public AbilityZoomies(AbilityParametersStruct abilityInfo)
        : base(abilityInfo)
    {
    }

    public AbilityZoomies(Player owner, Demigod demigod, AbilityParametersStruct abilityInfo)
        : base(owner, demigod, abilityInfo)
    {
        Icon = YuumiIcons.E;
        Cooldown = YuumiConfig.ECooldown.Value;
        EnergyCost = YuumiConfig.EEnergyCost.Value;
    }

    public override string GetTooltip()
    {
        return Helper.SafeFormat(base.GetTooltip(), GetHeal().ToString("0.#"),
            YuumiConfig.ESpeedBonus.Value.ToString("0"), YuumiConfig.EDamageBonus.Value.ToString("0"),
            YuumiConfig.EDuration.Value.ToString("0"));
    }

    public override string GetStatusTooltip()
    {
        return base.GetStatusTooltip();
    }

    public override void Execute()
    {
        float heal = GetHeal();
        float duration = YuumiConfig.EDuration.Value;
        float speedBonus = YuumiConfig.ESpeedBonus.Value;
        float damageBonus = YuumiConfig.EDamageBonus.Value;

        SE_YuumiZoomies.ApplyTo(Owner, duration, heal, speedBonus, damageBonus);

        if (Demigod is Demigods.DemigodYuumi yuumi && yuumi.AttachedAlly != null)
        {
            SE_YuumiZoomies.ApplyTo(yuumi.AttachedAlly, duration, heal, speedBonus, damageBonus);
        }

        if (Resources.FxWindHeal != null)
        {
            Helper.SpawnGameObject(Resources.FxWindHeal, Owner.GetCenterPoint(), Quaternion.identity);
        }

        if (Resources.SfxHealing != null)
        {
            Helper.PlaySoundEffect(Resources.SfxHealing, Owner.GetCenterPoint());
        }

        Helper.TriggerAnim(Owner, "gpower");
        base.Execute();
    }

    private float GetHeal()
    {
        float skillLevel = Owner.GetSkillLevel(NorseDemigods.Instance.SkillDemigod);
        return Mathf.Lerp(YuumiConfig.EHealAtBaseLevel.Value, YuumiConfig.EHealAtMaxLevel.Value, skillLevel / 100f) *
               Helper.GetHealingModifier(Owner);
    }
}
