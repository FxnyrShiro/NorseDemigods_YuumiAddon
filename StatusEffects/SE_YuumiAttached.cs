using UnityEngine;

namespace NorseDemigods.Yuumi.StatusEffects;

/// <summary>
///     "You and Me!" — the bond between Yuumi and her ally. Applied both to the ally
///     (offensive buff) and to Yuumi herself (armor and regeneration).
/// </summary>
public class SE_YuumiAttached : SE_Stats
{
    internal static int? StatusEffectHash;

    private float _healPerSecond;
    private float _healTimer;
    private bool _isSelf;
    private GameObject _fx;

    public SE_YuumiAttached()
    {
        name = "SE_YuumiAttached";
        m_name = YuumiLocalization.T("$se_yuumi_attached_name");
        m_icon = YuumiIcons.ToSprite(YuumiIcons.W);
        if (!StatusEffectHash.HasValue)
        {
            StatusEffectHash = NameHash();
        }
    }

    public void Setup(Character character, float duration, bool isSelf, float damageBonus, float speedBonus,
        float healPerSecond, float armorBonus)
    {
        m_character = character;
        m_ttl = duration;
        _isSelf = isSelf;
        _healPerSecond = healPerSecond;
        _healTimer = 1f;

        m_damageModifier = 1f + damageBonus / 100f;
        m_speedModifier = speedBonus / 100f;
        m_addArmor = armorBonus;

        if (isSelf)
        {
            m_name = YuumiLocalization.T("$se_yuumi_attached_self_name");
            m_tooltip = Helper.SafeFormat(YuumiLocalization.T("$se_yuumi_attached_self_tooltip"),
                armorBonus.ToString("0"), healPerSecond.ToString("0.#"));
        }
        else
        {
            m_name = YuumiLocalization.T("$se_yuumi_attached_name");
            m_tooltip = Helper.SafeFormat(YuumiLocalization.T("$se_yuumi_attached_tooltip"),
                damageBonus.ToString("0.#"), speedBonus.ToString("0"), healPerSecond.ToString("0.#"));
        }

        CreateEffects();
    }

    public override void UpdateStatusEffect(float dt)
    {
        if (m_character == null)
        {
            return;
        }

        if (_healPerSecond > 0f && m_character.GetHealth() < m_character.GetMaxHealth())
        {
            _healTimer -= dt;
            if (_healTimer <= 0f)
            {
                m_character.Heal(_healPerSecond);
                _healTimer += 1f;
            }
        }

        if (_fx != null)
        {
            _fx.transform.rotation = Quaternion.identity;
        }

        base.UpdateStatusEffect(dt);
    }

    public override void Stop()
    {
        DestroyEffects();
        base.Stop();
    }

    public static void ApplyToAlly(Character target, float duration, float damageBonus, float speedBonus,
        float healPerSecond)
    {
        if (target == null)
        {
            return;
        }

        ZNetView nview = target.GetComponent<ZNetView>();
        if (nview != null && !nview.IsOwner())
        {
            nview.InvokeRPC(YuumiRpc.ApplyAttach, target.GetZDOID(), duration, damageBonus, speedBonus,
                healPerSecond);
            return;
        }

        Add(target, duration, false, damageBonus, speedBonus, healPerSecond, 0f);
    }

    public static void ApplyToSelf(Character target, float duration, float armorBonus, float healPerSecond)
    {
        if (target == null)
        {
            return;
        }

        Add(target, duration, true, 0f, 0f, healPerSecond, armorBonus);
    }

    private static void Add(Character target, float duration, bool isSelf, float damageBonus, float speedBonus,
        float healPerSecond, float armorBonus)
    {
        SE_YuumiAttached statusEffect = (SE_YuumiAttached)target.GetSEMan().AddStatusEffect(
            (SE_YuumiAttached)ScriptableObject.CreateInstance(typeof(SE_YuumiAttached)), false, 0, 0f, -1);
        statusEffect?.Setup(target, duration, isSelf, damageBonus, speedBonus, healPerSecond, armorBonus);
    }

    private void CreateEffects()
    {
        if (m_character == null)
        {
            return;
        }

        GameObject prefab = _isSelf ? Resources.FxStarsPurple : Resources.FxWindBuff;
        if (prefab == null)
        {
            return;
        }

        _fx = Helper.SpawnGameObject(prefab, m_character.GetCenterPoint(), Quaternion.identity);
        if (_fx != null)
        {
            _fx.transform.SetParent(m_character.transform);
        }
    }

    private void DestroyEffects()
    {
        if (_fx != null)
        {
            _fx.transform.SetParent(null);
            Helper.DestroyFx(_fx);
            _fx = null;
        }
    }
}
