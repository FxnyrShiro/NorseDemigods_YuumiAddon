using System.Collections.Generic;
using NorseDemigods.Yuumi.StatusEffects;
using UnityEngine;

namespace NorseDemigods.Yuumi.Abilities;

/// <summary>
///     R — Final Chapter: five advancing waves that damage and slow enemies while healing allies.
/// </summary>
public class AbilityFinalChapter : DemigodAbility
{
    private int _wavesLeft;
    private int _waveIndex;
    private float _waveTimer;
    private Vector3 _forward;

    public AbilityFinalChapter(AbilityParametersStruct abilityInfo)
        : base(abilityInfo)
    {
    }

    public AbilityFinalChapter(Player owner, Demigod demigod, AbilityParametersStruct abilityInfo)
        : base(owner, demigod, abilityInfo)
    {
        Icon = YuumiIcons.R;
        Cooldown = YuumiConfig.RCooldown.Value;
        EnergyCost = YuumiConfig.REnergyCost.Value;
    }

    public override string GetTooltip()
    {
        return Helper.SafeFormat(base.GetTooltip(), YuumiConfig.RWaves.Value.ToString("0"),
            GetDamage().ToString("0.#"), YuumiConfig.RSlowAmount.Value.ToString("0"),
            YuumiConfig.RSlowDuration.Value.ToString("0.#"), GetHeal().ToString("0.#"));
    }

    public override string GetStatusTooltip()
    {
        return Helper.SafeFormat(base.GetStatusTooltip(), YuumiConfig.RWaves.Value.ToString("0"));
    }

    public override void Execute()
    {
        _forward = Owner.GetLookDir();
        _forward.y = 0f;
        if (_forward.sqrMagnitude < 0.01f)
        {
            _forward = Owner.transform.forward;
            _forward.y = 0f;
        }

        _forward.Normalize();
        _wavesLeft = Mathf.Max(1, YuumiConfig.RWaves.Value);
        _waveIndex = 0;
        _waveTimer = 0f;

        Helper.TriggerAnim(Owner, "staff_summon");
        base.Execute();
    }

    public override void Update(float dt)
    {
        base.Update(dt);

        if (_wavesLeft <= 0 || Owner == null || Owner.IsDead())
        {
            return;
        }

        _waveTimer -= dt;
        if (_waveTimer > 0f)
        {
            return;
        }

        _waveTimer = Mathf.Max(0.05f, YuumiConfig.RWaveInterval.Value);
        SpawnWave(_waveIndex);
        _waveIndex++;
        _wavesLeft--;
    }

    private void SpawnWave(int index)
    {
        float distance = YuumiConfig.RWaveStartDistance.Value + YuumiConfig.RWaveDistance.Value * index;
        Vector3 center = Owner.transform.position + _forward * distance;

        if (Helper.FindSurface(center + Vector3.up * 2f, out Vector3 surface))
        {
            center = surface + Vector3.up * 0.5f;
        }

        HitData hitData = new HitData
        {
            m_point = center,
            m_dir = _forward,
            m_damage = new HitData.DamageTypes
            {
                m_spirit = GetDamage()
            }
        };
        hitData.SetAttacker(Owner);

        float damage = Helper.DoAreaDamage(hitData, YuumiConfig.RWaveRadius.Value);
        float slow = YuumiConfig.RSlowAmount.Value;
        float slowDuration = YuumiConfig.RSlowDuration.Value;

        List<Character> characters = new List<Character>();
        Character.GetCharactersInRange(center, YuumiConfig.RWaveRadius.Value + 1f, characters);
        foreach (Character character in characters)
        {
			if (Demigod is Demigods.DemigodYuumi yuumiDemigod)
			{
			    yuumiDemigod.OnAbilityHitEnemy(character);
			}

            if (character != null && Helper.AreCharactersEnemies(Owner, character))
            {
                SE_YuumiSlow.ApplyTo(character, slow, slowDuration);
            }
        }

        Helper.DoAreaHealing(Owner, center, YuumiConfig.RWaveRadius.Value + 2f, GetHeal());

        if (damage > 0f && Demigod is Demigods.DemigodYuumi yuumi)
        {
            yuumi.OnAbilityHitEnemy(null);
        }

        if (Resources.FxStarsPurple != null)
        {
            Helper.SpawnGameObject(Resources.FxStarsPurple, center, Quaternion.identity);
        }

        if (Resources.SfxHolyStrike != null)
        {
            Helper.PlaySoundEffect(Resources.SfxHolyStrike, center);
        }
    }

    private float GetDamage()
    {
        float skillLevel = Owner.GetSkillLevel(NorseDemigods.Instance.SkillDemigod);
        return Mathf.Lerp(YuumiConfig.RDamageAtBaseLevel.Value, YuumiConfig.RDamageAtMaxLevel.Value,
            skillLevel / 100f) * Helper.GetDamageModifier(Owner);
    }

    private float GetHeal()
    {
        float skillLevel = Owner.GetSkillLevel(NorseDemigods.Instance.SkillDemigod);
        return Mathf.Lerp(YuumiConfig.RHealAtBaseLevel.Value, YuumiConfig.RHealAtMaxLevel.Value, skillLevel / 100f) *
               Helper.GetHealingModifier(Owner);
    }
}
