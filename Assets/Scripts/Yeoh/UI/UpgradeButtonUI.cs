using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum UpgradeType
{
    AOEDamage,
    AOERange,
    AOECastTime,
    AOECooldown,

    LaserDamage,
    LaserRange,
    LaserCastTime,
    LaserCooldown,

    HealSpeed,
    HealCastTime,
    HealCooldown,
    
}

public class UpgradeButtonUI : MonoBehaviour
{
    public UpgradeType upgradeType;

    public TextMeshProUGUI labelTMP;
    public TextMeshProUGUI nextMagTMP;
    public TextMeshProUGUI levelTMP;
    public TextMeshProUGUI costTMP;
    public GameObject[] levelBars;

    void Update()
    {
        UpdateTexts();
        UpdateLevelBars();
    }

    void UpdateTexts()
    {
        switch(upgradeType)
        {
            case UpgradeType.AOEDamage:
            {
                labelTMP.text = "Damage:";
                nextMagTMP.text = $"{UpgradeManager.Current.GetAoeNextDmg()}";
                levelTMP.text = $"{UpgradeManager.Current.GetAoeDmgLvl()}";
                costTMP.text = $"{UpgradeManager.Current.GetAoeDmgNextCostChi()}";
            } break;
            case UpgradeType.AOERange:
            {
                labelTMP.text = "Radius:";
                nextMagTMP.text = $"{UpgradeManager.Current.GetAoeNextRange()}";
                levelTMP.text = $"{UpgradeManager.Current.GetAoeRangeLvl()}";
                costTMP.text = $"{UpgradeManager.Current.GetAoeRangeNextCostChi()}";
            } break;
            case UpgradeType.AOECastTime:
            {
                labelTMP.text = "Cast Time:";
                nextMagTMP.text = $"{UpgradeManager.Current.GetAoeNextCastTime()}";
                levelTMP.text = $"{UpgradeManager.Current.GetAoeCastTimeLvl()}";
                costTMP.text = $"{UpgradeManager.Current.GetAoeCastTimeNextCostChi()}";
            } break;
            case UpgradeType.AOECooldown:
            {
                labelTMP.text = "Cooldown:";
                nextMagTMP.text = $"{UpgradeManager.Current.GetAoeNextCooldown()}";
                levelTMP.text = $"{UpgradeManager.Current.GetAoeCooldownLvl()}";
                costTMP.text = $"{UpgradeManager.Current.GetAoeCooldownNextCostChi()}";
            } break;

            case UpgradeType.LaserDamage:
            {
                labelTMP.text = "Damage:";
                nextMagTMP.text = $"{UpgradeManager.Current.GetLaserNextDmgTotal()}";
                levelTMP.text = $"{UpgradeManager.Current.GetLaserDmgLvl()}";
                costTMP.text = $"{UpgradeManager.Current.GetLaserDmgNextCostChi()}";
            } break;
            case UpgradeType.LaserRange:
            {
                labelTMP.text = "Range:";
                nextMagTMP.text = $"{UpgradeManager.Current.GetLaserNextRange()}";
                levelTMP.text = $"{UpgradeManager.Current.GetLaserRangeLvl()}";
                costTMP.text = $"{UpgradeManager.Current.GetLaserRangeNextCostChi()}";
            } break;
            case UpgradeType.LaserCastTime:
            {
                labelTMP.text = "Cast Time:";
                nextMagTMP.text = $"{UpgradeManager.Current.GetLaserNextCastTime()}";
                levelTMP.text = $"{UpgradeManager.Current.GetLaserCastTimeLvl()}";
                costTMP.text = $"{UpgradeManager.Current.GetLaserCastTimeNextCostChi()}";
            } break;
            case UpgradeType.LaserCooldown:
            {
                labelTMP.text = "Cooldown:";
                nextMagTMP.text = $"{UpgradeManager.Current.GetLaserNextCooldown()}";
                levelTMP.text = $"{UpgradeManager.Current.GetLaserCooldownLvl()}";
                costTMP.text = $"{UpgradeManager.Current.GetLaserCooldownNextCostChi()}";
            } break;

            case UpgradeType.HealSpeed:
            {
                labelTMP.text = "Amount:";
                nextMagTMP.text = $"{UpgradeManager.Current.GetHealNextAmountTotal()}";
                levelTMP.text = $"{UpgradeManager.Current.GetHealSpeedLvl()}";
                costTMP.text = $"{UpgradeManager.Current.GetHealSpeedNextCostChi()}";
            } break;
            case UpgradeType.HealCastTime:
            {
                labelTMP.text = "Cast Time:";
                nextMagTMP.text = $"{UpgradeManager.Current.GetHealNextCastTime()}";
                levelTMP.text = $"{UpgradeManager.Current.GetHealCastTimeLvl()}";
                costTMP.text = $"{UpgradeManager.Current.GetHealCastTimeNextCostChi()}";
            } break;
            case UpgradeType.HealCooldown:
            {
                labelTMP.text = "Cooldown:";
                nextMagTMP.text = $"{UpgradeManager.Current.GetHealNextCooldown()}";
                levelTMP.text = $"{UpgradeManager.Current.GetHealCooldownLvl()}";
                costTMP.text = $"{UpgradeManager.Current.GetHealCooldownNextCostChi()}";
            } break;
        }
    }

    int currentLvl=-1;

    void UpdateLevelBars()
    {
        int lvl=-1;

        switch(upgradeType)
        {
            case UpgradeType.AOEDamage:
            {
                lvl = int.Parse(UpgradeManager.Current.GetAoeDmgLvl());
            } break;
            case UpgradeType.AOERange:
            {
                lvl = int.Parse(UpgradeManager.Current.GetAoeRangeLvl());
            } break;
            case UpgradeType.AOECastTime:
            {
                lvl = int.Parse(UpgradeManager.Current.GetAoeCastTimeLvl());
            } break;
            case UpgradeType.AOECooldown:
            {
                lvl = int.Parse(UpgradeManager.Current.GetAoeCooldownLvl());
            } break;

            case UpgradeType.LaserDamage:
            {
                lvl = int.Parse(UpgradeManager.Current.GetLaserDmgLvl());
            } break;
            case UpgradeType.LaserRange:
            {
                lvl = int.Parse(UpgradeManager.Current.GetLaserRangeLvl());
            } break;
            case UpgradeType.LaserCastTime:
            {
                lvl = int.Parse(UpgradeManager.Current.GetLaserCastTimeLvl());
            } break;
            case UpgradeType.LaserCooldown:
            {
                lvl = int.Parse(UpgradeManager.Current.GetLaserCooldownLvl());
            } break;

            case UpgradeType.HealSpeed:
            {
                lvl = int.Parse(UpgradeManager.Current.GetHealSpeedLvl());
            } break;
            case UpgradeType.HealCastTime:
            {
                lvl = int.Parse(UpgradeManager.Current.GetHealCastTimeLvl());
            } break;
            case UpgradeType.HealCooldown:
            {
                lvl = int.Parse(UpgradeManager.Current.GetHealCooldownLvl());
            } break;
        }

        if(currentLvl != lvl)
        {
            currentLvl = lvl;
            ActivateBars();
        }
    }

    void ActivateBars()
    {
        for(int i=0; i<levelBars.Length; i++)
        {
            if(i<currentLvl)
            {
                levelBars[i].SetActive(true);
            }
            else
            {
                levelBars[i].SetActive(false);
            }
        }
    }

    public void OnUpgradeButton()
    {
        switch(upgradeType)
        {
            case UpgradeType.AOEDamage:
            {
                UpgradeManager.Current.UpgradeLvl(ref UpgradeManager.Current.aoeDmgLvl, UpgradeManager.Current.GetAoeDmgNextCost());
            } break;
            case UpgradeType.AOERange:
            {
                UpgradeManager.Current.UpgradeLvl(ref UpgradeManager.Current.aoeRangeLvl, UpgradeManager.Current.GetAoeRangeNextCost());
            } break;
            case UpgradeType.AOECastTime:
            {
                UpgradeManager.Current.UpgradeLvl(ref UpgradeManager.Current.aoeCastTimeLvl, UpgradeManager.Current.GetAoeCastTimeNextCost());
            } break;
            case UpgradeType.AOECooldown:
            {
                UpgradeManager.Current.UpgradeLvl(ref UpgradeManager.Current.aoeCooldownLvl, UpgradeManager.Current.GetAoeCooldownNextCost());
            } break;

            case UpgradeType.LaserDamage:
            {
                UpgradeManager.Current.UpgradeLvl(ref UpgradeManager.Current.laserDmgLvl, UpgradeManager.Current.GetLaserDmgNextCost());
            } break;
            case UpgradeType.LaserRange:
            {
                UpgradeManager.Current.UpgradeLvl(ref UpgradeManager.Current.laserRangeLvl, UpgradeManager.Current.GetLaserRangeNextCost());
            } break;
            case UpgradeType.LaserCastTime:
            {
                UpgradeManager.Current.UpgradeLvl(ref UpgradeManager.Current.laserCastTimeLvl, UpgradeManager.Current.GetLaserCastTimeNextCost());
            } break;
            case UpgradeType.LaserCooldown:
            {
                UpgradeManager.Current.UpgradeLvl(ref UpgradeManager.Current.laserCooldownLvl, UpgradeManager.Current.GetLaserCooldownNextCost());
            } break;

            case UpgradeType.HealSpeed:
            {
                UpgradeManager.Current.UpgradeLvl(ref UpgradeManager.Current.healSpeedLvl, UpgradeManager.Current.GetHealSpeedNextCost());
            } break;
            case UpgradeType.HealCastTime:
            {
                UpgradeManager.Current.UpgradeLvl(ref UpgradeManager.Current.healCastTimeLvl, UpgradeManager.Current.GetHealCastTimeNextCost());
            } break;
            case UpgradeType.HealCooldown:
            {
                UpgradeManager.Current.UpgradeLvl(ref UpgradeManager.Current.healCooldownLvl, UpgradeManager.Current.GetHealCooldownNextCost());
            } break;
        }
    }
}
