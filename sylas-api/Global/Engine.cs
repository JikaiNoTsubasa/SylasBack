using System;

namespace sylas_api.Global;

public class Engine
{
    public static int GetCurrentLevelXpPercent(int level, int currentXp){
        return currentXp*100/GetNextLevelTotalXp(level);
    }

    public static int GetNextLevelTotalXp(int currentLevel){
        return 50*(currentLevel+1) + 100;
    }
}
