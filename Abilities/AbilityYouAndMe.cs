using System.Collections.Generic;
using UnityEngine;

namespace NorseDemigods.Yuumi.Abilities;

/// <summary>
///     W — You and Me!: leap to an ally and bond with them, buffing both of you.
/// </summary>
public class AbilityYouAndMe : DemigodAbility
{
    public AbilityYouAndMe(AbilityParametersStruct abilityInfo)
        : base(abilityInfo)
    {
    }

    public AbilityYouAndMe(Player owner, Demigod demigod, AbilityParametersStruct abilityInfo)
        : base(owner, demigod, abilityInfo)
    {
        Icon = YuumiIcons.W;
        Cooldown = YuumiConfig.WCooldown.Value;
        EnergyCost = YuumiConfig.WEnergyCost.Value;
        Duration = YuumiConfig.WDuration.Value;
        IsBuff = true;
        CanBeStopped = true;
    }

    public override string GetTooltip()
    {
        return Helper.SafeFormat(base.GetTooltip(), YuumiConfig.WDuration.Value.ToString("0"),
            YuumiConfig.WAllyDamageBonus.Value.ToString("0"), YuumiConfig.WAllySpeedBonus.Value.ToString("0"),
            YuumiConfig.WAllyHealPerSec.Value.ToString("0.#"));
    }

    public override string GetStatusTooltip()
    {
        return base.GetStatusTooltip();
    }

    public override bool CanExecute(bool showMessage = true)
    {
        if (FindAlly() == null)
        {
            if (showMessage)
            {
                Owner.Message(MessageHud.MessageType.TopLeft, YuumiLocalization.T("$yuumi_no_ally_message"));
            }

            return false;
        }

        return base.CanExecute(showMessage);
    }

    public override void Execute()
    {
        Character ally = FindAlly();
        if (ally == null)
        {
            return;
        }

        if (Vector3.Distance(Owner.transform.position, ally.transform.position) > 3f)
        {
            Vector3 offset = Owner.transform.position - ally.transform.position;
            offset.y = 0f;
            if (offset.sqrMagnitude < 0.01f)
            {
                offset = -ally.transform.forward;
            }

            Vector3 position = ally.transform.position + offset.normalized * 1.5f;
            position.y = ally.transform.position.y;
            Owner.TeleportTo(position, Owner.transform.rotation, false);
        }

        if (Demigod is Demigods.DemigodYuumi yuumi)
        {
            yuumi.AttachTo(ally);
        }

        if (Resources.FxStarsPurple != null)
        {
            Helper.SpawnGameObject(Resources.FxStarsPurple, ally.GetCenterPoint(), Quaternion.identity);
        }

        if (Resources.SfxHealing != null)
        {
            Helper.PlaySoundEffect(Resources.SfxHealing, ally.GetCenterPoint());
        }

        base.Execute();
    }

    public override void OnBuffTick()
    {
        if (!(Demigod is Demigods.DemigodYuumi yuumi) || yuumi.AttachedAlly == null)
        {
            Stop();
        }

        base.OnBuffTick();
    }

    public override void Stop(bool keyUp = false)
    {
        if (IsActive && Demigod is Demigods.DemigodYuumi yuumi)
        {
            yuumi.Detach(false);
        }

        base.Stop(keyUp);
    }

    private Character FindAlly()
    {
        float range = YuumiConfig.WRange.Value;

        if (Helper.FindCrosshairTarget(Owner, range, out var hitInfo) && hitInfo.Collider != null)
        {
            GameObject hitObject = Projectile.FindHitObject(hitInfo.Collider);
            if (hitObject != null && hitObject.TryGetComponent(out Character crosshairCharacter) &&
                IsValidAlly(crosshairCharacter))
            {
                return crosshairCharacter;
            }
        }

        Character closest = null;
        float closestDistance = float.MaxValue;

        List<Character> characters = new List<Character>();
        Character.GetCharactersInRange(Owner.GetCenterPoint(), range, characters);
        foreach (Character character in characters)
        {
            if (!IsValidAlly(character))
            {
                continue;
            }

            float distance = Vector3.Distance(Owner.transform.position, character.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = character;
            }
        }

        return closest;
    }

    private bool IsValidAlly(Character character)
    {
        if (character == null || character == Owner || character.IsDead())
        {
            return false;
        }

        if (Helper.AreCharactersEnemies(Owner, character))
        {
            return false;
        }

        if (character is Player)
        {
            return true;
        }

        return character.TryGetComponent(out Tameable tameable) && tameable.IsTamed();
    }
}
