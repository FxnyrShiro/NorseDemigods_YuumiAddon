using NorseDemigods.Yuumi.StatusEffects;
using UnityEngine;

namespace NorseDemigods.Yuumi.Demigods;

/// <summary>
///     Yuumi, the Magical Cat. She is a support demigod: her passive "Feline Friendship"
///     periodically empowers her next ability hit, and her abilities heal and buff allies.
/// </summary>
public class DemigodYuumi : Demigod
{
    private float _bopTimer;

    internal Character AttachedAlly { get; private set; }

    internal bool IsBopReady => _bopTimer <= 0f;

	public DemigodYuumi()
	{
	    DemigodStatusEffectHash = "SE_DemigodYuumi".GetStableHashCode();
	}

    public override void Update()
    {
        base.Update();

        if (Owner == null)
        {
            return;
        }

        if (_bopTimer > 0f)
        {
            _bopTimer -= Time.deltaTime;
            if (_bopTimer <= 0f)
            {
                _bopTimer = 0f;
                OnBopReady();
            }
        }

        if (AttachedAlly != null)
        {
            bool allyGone = AttachedAlly.IsDead();
            bool tooFar = Vector3.Distance(Owner.transform.position, AttachedAlly.transform.position) >
                          YuumiConfig.WBreakRange.Value;
            if (allyGone || tooFar)
            {
                Detach(true);
            }
        }
    }

    public override void OnPlayerSpawned()
    {
        base.OnPlayerSpawned();
        AttachedAlly = null;
        _bopTimer = Mathf.Max(1f, YuumiConfig.PBopInterval.Value * 0.5f);
    }

    public override void OnPlayerDeath()
    {
        Detach(false);
        base.OnPlayerDeath();
    }

    public override void OnVictimDamage(Character victim, HitData hit)
    {
        base.OnVictimDamage(victim, hit);
        if (victim != null && victim != Owner && Helper.AreCharactersEnemies(Owner, victim))
        {
            OnAbilityHitEnemy(victim);
        }
    }

    public override void OnDestroy()
    {
        AttachedAlly = null;
        base.OnDestroy();
    }

    internal void AttachTo(Character ally)
    {
        if (ally == null || ally == Owner)
        {
            return;
        }

        AttachedAlly = ally;

        SE_YuumiAttached.ApplyToSelf(Owner, YuumiConfig.WDuration.Value, YuumiConfig.WSelfArmor.Value,
            YuumiConfig.WSelfHealPerSec.Value);
        SE_YuumiAttached.ApplyToAlly(ally, YuumiConfig.WDuration.Value, YuumiConfig.WAllyDamageBonus.Value,
            YuumiConfig.WAllySpeedBonus.Value, YuumiConfig.WAllyHealPerSec.Value);

        if (Player.m_localPlayer == Owner)
        {
            Owner.Message(MessageHud.MessageType.TopLeft,
                Helper.SafeFormat(YuumiLocalization.T("$yuumi_attach_message"), ally.GetHoverName()));
        }
    }

    internal void Detach(bool notify)
    {
        if (AttachedAlly != null)
        {
            AttachedAlly = null;
        }

        if (SE_YuumiAttached.StatusEffectHash.HasValue && Owner != null)
        {
            Owner.GetSEMan().RemoveStatusEffect(SE_YuumiAttached.StatusEffectHash.Value);
        }

        if (notify && Player.m_localPlayer == Owner)
        {
            Owner.Message(MessageHud.MessageType.TopLeft, YuumiLocalization.T("$yuumi_attach_broken_message"));
        }
    }

    internal void OnAbilityHitEnemy(Character victim)
    {
        if (!IsBopReady)
        {
            return;
        }

        TriggerBop(victim != null ? victim.GetCenterPoint() : Owner.GetCenterPoint());
    }

    private void TriggerBop(Vector3 center)
    {
        float skill = Owner.GetSkillLevel(NorseDemigods.Instance.SkillDemigod);
        float heal = Mathf.Lerp(YuumiConfig.PBopHealAtBaseLevel.Value, YuumiConfig.PBopHealAtMaxLevel.Value,
            skill / 100f) * Helper.GetHealingModifier(Owner);

        Helper.DoAreaHealing(Owner, center, YuumiConfig.PBopRadius.Value, heal);
        SE_YuumiBop.ApplyTo(Owner, YuumiConfig.PBopArmorDuration.Value, YuumiConfig.PBopArmor.Value);

        if (AttachedAlly != null)
        {
            SE_YuumiBop.ApplyTo(AttachedAlly, YuumiConfig.PBopArmorDuration.Value, YuumiConfig.PBopArmor.Value);
        }

        _bopTimer = Mathf.Max(1f, YuumiConfig.PBopInterval.Value);

        if (Player.m_localPlayer == Owner)
        {
            Owner.Message(MessageHud.MessageType.TopLeft, YuumiLocalization.T("$yuumi_bop_triggered_message"));
        }

        if (Resources.FxHolyExplosion != null)
        {
            Helper.SpawnGameObject(Resources.FxHolyExplosion, center, Quaternion.identity);
        }

        if (Resources.SfxHealing != null)
        {
            Helper.PlaySoundEffect(Resources.SfxHealing, Owner.transform.position);
        }
        if (DemigodStatusEffect != null)
        {
            DemigodStatusEffect.AddEnergy(1);
        }
    }

    private void OnBopReady()
    {
        if (Player.m_localPlayer == Owner)
        {
            Owner.Message(MessageHud.MessageType.TopLeft, YuumiLocalization.T("$yuumi_bop_ready_message"));
        }

        if (Resources.FxStarsPurple != null)
        {
            Helper.SpawnGameObject(Resources.FxStarsPurple, Owner.GetCenterPoint(), Quaternion.identity);
        }
    }
}
