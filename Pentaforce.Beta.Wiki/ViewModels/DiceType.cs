namespace Pentaforce.Beta.Wiki.ViewModels
{
    public enum DiceType
    {
        d4 = 25,
        d6 = 35,
        d8 = 45,
        d10 = 55,
        d100 = 56,
        d12 = 65,
        d20 = 105
    }

    public static class DiceTypeExtentions
    { 
        public static double GetValue(this DiceType die)
        {
            var value = die == DiceType.d100 ? (int)die - 1 : (int)die;
            return value / 10d;
        }

        public static int GetSides(this DiceType die)
        {
            return die == DiceType.d100 ? 10 : int.Parse(die.ToString().Replace("d", string.Empty));
        }
    }
}
