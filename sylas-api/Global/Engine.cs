using System;
using System.Globalization;

namespace sylas_api.Global;

public class Engine
{
    public static int GetCurrentLevelXpPercent(int level, int currentXp)
    {
        return currentXp * 100 / GetNextLevelTotalXp(level);
    }

    public static int GetNextLevelTotalXp(int currentLevel)
    {
        return 50 * (currentLevel + 1) + 100;
    }

    public static (int, int) AddXP(int currentLevel, int currentXp, int xpToAdd)
    {
        int nextLevelXp = GetNextLevelTotalXp(currentLevel);
        currentXp += xpToAdd;
        if (currentXp >= nextLevelXp)
        {
            currentLevel++;
            currentXp -= nextLevelXp;
        }
        return (currentLevel, currentXp);
    }

    public static (DateTime, DateTime) GetFirstAndLastDayOfWeek(int year, int week)
    {
        // Trouve le lundi de la semaine "semaine" dans l'année "annee" selon l'ISO 8601
        DateTime jan1 = DateTime.SpecifyKind(new(year, 1, 1), DateTimeKind.Utc);
        int shiftedDay = DayOfWeek.Thursday - jan1.DayOfWeek;
        DateTime firstYearDay = jan1.AddDays(shiftedDay);
        var calendar = CultureInfo.CurrentCulture.Calendar;
        int firstYearWeek = calendar.GetWeekOfYear(
            firstYearDay, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        DateTime firstDay = firstYearDay.AddDays((week - firstYearWeek) * 7).AddDays(-3);
        DateTime lastDay = firstDay.AddDays(6);

        return (firstDay, lastDay);
    }

    public static int GetCurrentWeekNumber()
    {
        DateTime today = DateTime.Today;

        // Obtenir le numéro de semaine selon la norme ISO 8601 (Europe)
        var cultureInfo = CultureInfo.CurrentCulture;
        int weekNumber = cultureInfo.Calendar.GetWeekOfYear(
            today,
            CalendarWeekRule.FirstFourDayWeek,
            DayOfWeek.Monday);

        return weekNumber;
    }
}
