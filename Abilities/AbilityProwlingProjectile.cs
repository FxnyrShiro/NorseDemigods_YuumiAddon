using System.Collections.Generic;
using NorseDemigods.Controllers;
using NorseDemigods.Yuumi.StatusEffects;
using UnityEngine;

namespace NorseDemigods.Yuumi.Abilities;

/// <summary>
///     Q — Prowling Projectile: a homing bolt that damages and slows enemies.
/// </summary>
public class AbilityProwlingProjectile : DemigodAbility
{
    public AbilityProwlingProjectile(AbilityParametersStruct abilityInfo)
        : base(abilityInfo)
    {
    }

    public AbilityProwlingProjectile(Player owner, Demigod demigod, AbilityParametersStruct abilityInfo)
        : base(owner, demigod, abilityInfo)
    {
        Icon = YuumiIcons.Q;
        Cooldown = YuumiConfig.QCooldown.Value;
        EnergyCost = YuumiConfig.QEnergyCost.Value;
    }

    public override string GetTooltip()
    {
        return Helper.SafeFormat(base.GetTooltip(), GetDamage().ToString("0.#"),
            YuumiConfig.QSlowAmount.Value.ToString("0"), YuumiConfig.QSlowDuration.Value.ToString("0.#"));
    }

    public override string GetStatusTooltip()
    {
        return Helper.SafeFormat(base.GetStatusTooltip(), GetDamage().ToString("0.#"));
    }

    public override bool CanExecute(bool showMessage = true)
    {
        if (Helper.IsPlayerBusy(Owner))
        {
            return false;
        }

        return base.CanExecute(showMessage);
    }

    public override void Execute()
    {
        Vector3 direction = Owner.GetLookDir();
        Vector3 spawnPosition = Owner.GetCenterPoint() + direction * 1.2f;
        Vector3 aimPoint = spawnPosition + direction * YuumiConfig.QProjectileRange.Value;
        Character target = null;

        if (Helper.FindCrosshairTarget(Owner, YuumiConfig.QProjectileRange.Value, out var hitInfo) &&
            hitInfo.Collider != null)
        {
            aimPoint = hitInfo.Target;
            GameObject hitObject = Projectile.FindHitObject(hitInfo.Collider);
            if (hitObject != null && hitObject.TryGetComponent(out Character hitCharacter) &&
                Helper.AreCharactersEnemies(Owner, hitCharacter))
            {
                target = hitCharacter;
            }
        }

        Vector3 flightDirection = aimPoint - spawnPosition;
        if (flightDirection.sqrMagnitude < 0.01f)
        {
            flightDirection = Owner.transform.forward;
        }

        flightDirection.Normalize();

        GameObject projectileObject = Helper.SpawnGameObject(Resources.FxArcaneBolt,
            spawnPosition, Quaternion.LookRotation(flightDirection));
        if (projectileObject == null)
        {
            return;
        }

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        if (projectile == null)
        {
            Object.Destroy(projectileObject);
            return;
        }

        HitData hitData = new HitData
        {
            m_damage = new HitData.DamageTypes
            {
                m_spirit = GetDamage()
            }
        };
        hitData.SetAttacker(Owner);

        projectile.Setup(Owner, Vector3.zero, 10f, hitData, null, null);
        projectile.m_aoe = YuumiConfig.QAoeRadius.Value;
        projectile.m_ttl = 8f;
        projectile.m_gravity = 0f;
        projectile.m_hitOwner = false;
        projectile.m_hitFriendly = false;

        projectileObject.AddComponent<Torpedo>().Setup(Owner, flightDirection, YuumiConfig.QSeekRadius.Value,
            4f, YuumiConfig.QProjectileSpeed.Value, 0.05f, false, target);

		projectileObject.AddComponent<ProwlingProjectileTracker>().Ability = this;
        if (Resources.SfxLaser != null)
        {
            Helper.PlaySoundEffect(Resources.SfxLaser, Owner.transform.position);
        }
        Helper.TriggerAnim(Owner, "staff_summon");
        base.Execute();
    }

    /// <summary>
    ///     Called by the Projectile.OnHit patch when this projectile hits an enemy.
    /// </summary>
    public void OnProjectileHitEnemy(Character victim)
    {
        float slow = YuumiConfig.QSlowAmount.Value;
        float duration = YuumiConfig.QSlowDuration.Value;

        List<Character> characters = new List<Character>();
        Character.GetCharactersInRange(victim.GetCenterPoint(), YuumiConfig.QAoeRadius.Value + 1f, characters);
        foreach (Character character in characters)
        {
            if (character != null && Helper.AreCharactersEnemies(Owner, character))
            {
                SE_YuumiSlow.ApplyTo(character, slow, duration);
            }
        }

        if (Demigod is Demigods.DemigodYuumi yuumi)
        {
            yuumi.OnAbilityHitEnemy(victim);
        }
    }

    private float GetDamage(float skillLevel)
    {
        return (YuumiConfig.QBaseDamage.Value + YuumiConfig.QDamagePerLevel.Value * skillLevel) *
               Helper.GetDamageModifier(Owner);
    }

    private float GetDamage()
    {
        return GetDamage(Owner.GetSkillLevel(NorseDemigods.Instance.SkillDemigod));
    }
}

public class ProwlingProjectileTracker : MonoBehaviour
{
    public AbilityProwlingProjectile Ability;
}
