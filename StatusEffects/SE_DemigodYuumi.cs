using NorseDemigods.Yuumi;
using UnityEngine;

namespace NorseDemigods.StatusEffects;

public class SE_DemigodYuumi : SE_Demigod
{
    public SE_DemigodYuumi()
    {
        name = "SE_DemigodYuumi";
        m_name = "Yuumi";
        m_ttl = 0f;
        m_icon = YuumiIcons.ChampionSprite;
    }
}
