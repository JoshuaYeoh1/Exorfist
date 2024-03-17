using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int chi;
    int maxLvl=2;
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("AOE")]
    public int aoeDmgLvl=0;
    public int[] aoeDmgLvlCost = {0, 10, 20};
    public float[] aoeDmgLvlMag = {50, 75, 100};

    public int aoeRangeLvl=0;
    public int[] aoeRangeLvlCost = {0, 10, 20};
    public float[] aoeRangeLvlMag = {3, 4, 5};

    public int aoeCastTimeLvl=0;
    public int[] aoeCastTimeLvlCost = {0, 10, 20};
    public float[] aoeCastTimeLvlMag = {2, 1, .5f};

    public int aoeCooldownLvl=0;
    public int[] aoeCooldownLvlCost = {0, 10, 20};
    public float[] aoeCooldownLvlMag = {30, 20, 10};
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Laser")]
    public int laserDmgLvl=0;
    public int[] laserDmgLvlCost = {0, 10, 20};
    public float[] laserDmgLvlMag = {8, 12, 16};

    public int laserRangeLvl=0;
    public int[] laserRangeLvlCost = {0, 10, 20};
    public float[] laserRangeLvlMag = {5, 7.5f, 10};

    public int laserCastTimeLvl=0;
    public int[] laserCastTimeLvlCost = {0, 10, 20};
    public float[] laserCastTimeLvlMag = {2, 1, .5f};

    public int laserCooldownLvl=0;
    public int[] laserCooldownLvlCost = {0, 10, 20};
    public float[] laserCooldownLvlMag = {30, 20, 10};

    public float laserDuration=5;
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Heal")]
    public int healSpeedLvl=0;
    public int[] healSpeedLvlCost = {0, 10, 20};
    public float[] healSpeedLvlMag = {1.5f, 2.5f, 3.5f};

    public int healCastTimeLvl=0;
    public int[] healCastTimeLvlCost = {0, 10, 20};
    public float[] healCastTimeLvlMag = {1.5f, 1, .5f};

    public int healCooldownLvl=0;
    public int[] healCooldownLvlCost = {0, 10, 20};
    public float[] healCooldownLvlMag = {45, 30, 15};

    public float healDuration=3;
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void ResetLevels()
    {
        aoeDmgLvl=0;
        aoeRangeLvl=0;
        aoeCastTimeLvl=0;
        aoeCooldownLvl=0;

        laserDmgLvl=0;
        laserRangeLvl=0;
        laserCastTimeLvl=0;
        laserCooldownLvl=0;

        healSpeedLvl=0;
        healCastTimeLvl=0;
        healCooldownLvl=0;
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public string GetAoeDmgLvl()
    {
        return $"{aoeDmgLvl+1}";
    }
    public float GetAoeDmg()
    {
        return aoeDmgLvlMag[aoeDmgLvl];
    }
    public string GetAoeNextDmg()
    {
        if(aoeDmgLvl<maxLvl)
        {
            return $"{aoeDmgLvlMag[aoeDmgLvl]} -> {aoeDmgLvlMag[aoeDmgLvl+1]}";
        }
        return $"{aoeDmgLvlMag[aoeDmgLvl]}";
    }
    public string GetAoeDmgNextCost()
    {
        if(aoeDmgLvl<maxLvl)
        {
            return $"{aoeDmgLvlCost[aoeDmgLvl+1]} Chi";
        }
        return "MAX";
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public string GetAoeRangeLvl()
    {
        return $"{aoeRangeLvl+1}";
    }
    public float GetAoeRange()
    {
        return aoeRangeLvlMag[aoeRangeLvl];
    }
    public string GetAoeNextRange()
    {
        if(aoeRangeLvl<maxLvl)
        {
            return $"{aoeRangeLvlMag[aoeRangeLvl]} -> {aoeRangeLvlMag[aoeRangeLvl+1]}m";
        }
        return $"{aoeRangeLvlMag[aoeRangeLvl]}m";
    }
    public string GetAoeRangeNextCost()
    {
        if(aoeRangeLvl<maxLvl)
        {
            return $"{aoeRangeLvlCost[aoeRangeLvl+1]} Chi";
        }
        return "MAX";
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public string GetAoeCastTimeLvl()
    {
        return $"{aoeCastTimeLvl+1}";
    }
    public float GetAoeCastTime()
    {
        return aoeCastTimeLvlMag[aoeCastTimeLvl];
    }
    public string GetAoeNextCastTime()
    {
        if(aoeCastTimeLvl<maxLvl)
        {
            return $"{aoeCastTimeLvlMag[aoeCastTimeLvl]} -> {aoeCastTimeLvlMag[aoeCastTimeLvl+1]}s";
        }
        return $"{aoeCastTimeLvlMag[aoeCastTimeLvl]}s";
    }
    public string GetAoeCastTimeNextCost()
    {
        if(aoeCastTimeLvl<maxLvl)
        {
            return $"{aoeCastTimeLvlCost[aoeCastTimeLvl+1]} Chi";
        }
        return "MAX";
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public string GetAoeCooldownLvl()
    {
        return $"{aoeCooldownLvl+1}";
    }
    public float GetAoeCooldown()
    {
        return aoeCooldownLvlMag[aoeCooldownLvl];
    }
    public string GetAoeNextCooldown()
    {
        if(aoeCooldownLvl<maxLvl)
        {
            return $"{aoeCooldownLvlMag[aoeCooldownLvl]} -> {aoeCooldownLvlMag[aoeCooldownLvl+1]}s";
        }
        return $"{aoeCooldownLvlMag[aoeCooldownLvl]}s";
    }
    public string GetAoeCooldownNextCost()
    {
        if(aoeCooldownLvl<maxLvl)
        {
            return $"{aoeCooldownLvlCost[aoeCooldownLvl+1]} Chi";
        }
        return "MAX";
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////



    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public string GetLaserDmgLvl()
    {
        return $"{laserDmgLvl+1}";
    }
    public float GetLaserDmg()
    {
        return laserDmgLvlMag[laserDmgLvl];
    }
    public string GetLaserNextDmg()
    {
        if(laserDmgLvl<maxLvl)
        {
            return $"{laserDmgLvlMag[laserDmgLvl]*25} -> {laserDmgLvlMag[laserDmgLvl+1]*25}";
        }
        return $"{laserDmgLvlMag[laserDmgLvl]*25}";
    }
    public string GetLaserDmgNextCost()
    {
        if(laserDmgLvl<maxLvl)
        {
            return $"{laserDmgLvlCost[laserDmgLvl+1]} Chi";
        }
        return "MAX";
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public string GetLaserRangeLvl()
    {
        return $"{laserRangeLvl+1}";
    }
    public float GetLaserRange()
    {
        return laserRangeLvlMag[laserRangeLvl];
    }
    public string GetLaserNextRange()
    {
        if(laserRangeLvl<maxLvl)
        {
            return $"{laserRangeLvlMag[laserRangeLvl]} -> {laserRangeLvlMag[laserRangeLvl+1]}m";
        }
        return $"{laserRangeLvlMag[laserRangeLvl]}m";
    }
    public string GetLaserRangeNextCost()
    {
        if(laserRangeLvl<maxLvl)
        {
            return $"{laserRangeLvlCost[laserRangeLvl+1]} Chi";
        }
        return "MAX";
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public string GetLaserCastTimeLvl()
    {
        return $"{laserCastTimeLvl+1}";
    }
    public float GetLaserCastTime()
    {
        return laserCastTimeLvlMag[laserCastTimeLvl];
    }
    public string GetLaserNextCastTime()
    {
        if(laserCastTimeLvl<maxLvl)
        {
            return $"{laserCastTimeLvlMag[laserCastTimeLvl]} -> {laserCastTimeLvlMag[laserCastTimeLvl+1]}s";
        }
        return $"{laserCastTimeLvlMag[laserCastTimeLvl]}s";
    }
    public string GetLaserCastTimeNextCost()
    {
        if(laserCastTimeLvl<maxLvl)
        {
            return $"{laserCastTimeLvlCost[laserCastTimeLvl+1]} Chi";
        }
        return "MAX";
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public string GetLaserCooldownLvl()
    {
        return $"{laserCooldownLvl+1}";
    }
    public float GetLaserCooldown()
    {
        return laserCooldownLvlMag[laserCooldownLvl];
    }
    public string GetLaserNextCooldown()
    {
        if(laserCooldownLvl<maxLvl)
        {
            return $"{laserCooldownLvlMag[laserCooldownLvl]} -> {laserCooldownLvlMag[laserCooldownLvl+1]}s";
        }
        return $"{laserCooldownLvlMag[laserCooldownLvl]}s";
    }
    public string GetLaserCooldownNextCost()
    {
        if(laserCooldownLvl<maxLvl)
        {
            return $"{laserCooldownLvlCost[laserCooldownLvl+1]} Chi";
        }
        return "MAX";
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////



    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public string GetHealSpeedLvl()
    {
        return $"{healSpeedLvl+1}";
    }
    public float GetHealSpeed()
    {
        return healSpeedLvlMag[healSpeedLvl];
    }
    public string GetHealNextDmg()
    {
        if(healSpeedLvl<maxLvl)
        {
            return $"{healSpeedLvlMag[healSpeedLvl]*10*3} -> {healSpeedLvlMag[healSpeedLvl+1]*10*3}HP";
        }
        return $"{healSpeedLvlMag[healSpeedLvl]*10*3}HP";
    }
    public string GetHealSpeedNextCost()
    {
        if(healSpeedLvl<maxLvl)
        {
            return $"{healSpeedLvlCost[healSpeedLvl+1]} Chi";
        }
        return "MAX";
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public string GetHealCastTimeLvl()
    {
        return $"{healCastTimeLvl+1}";
    }
    public float GetHealCastTime()
    {
        return healCastTimeLvlMag[healCastTimeLvl];
    }
    public string GetHealNextCastTime()
    {
        if(healCastTimeLvl<maxLvl)
        {
            return $"{healCastTimeLvlMag[healCastTimeLvl]} -> {healCastTimeLvlMag[healCastTimeLvl+1]}s";
        }
        return $"{healCastTimeLvlMag[healCastTimeLvl]}s";
    }
    public string GetHealCastTimeNextCost()
    {
        if(healCastTimeLvl<maxLvl)
        {
            return $"{healCastTimeLvlCost[healCastTimeLvl+1]} Chi";
        }
        return "MAX";
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public string GetHealCooldownLvl()
    {
        return $"{healCooldownLvl+1}";
    }
    public float GetHealCooldown()
    {
        return healCooldownLvlMag[healCooldownLvl];
    }
    public string GetHealNextCooldown()
    {
        if(healCooldownLvl<maxLvl)
        {
            return $"{healCooldownLvlMag[healCooldownLvl]} -> {healCooldownLvlMag[healCooldownLvl+1]}s";
        }
        return $"{healCooldownLvlMag[healCooldownLvl]}s";
    }
    public string GetHealCooldownNextCost()
    {
        if(healCooldownLvl<maxLvl)
        {
            return $"{healCooldownLvlCost[healCooldownLvl+1]} Chi";
        }
        return "MAX";
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    
}
