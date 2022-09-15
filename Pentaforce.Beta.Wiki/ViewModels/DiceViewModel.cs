using System.Collections;

namespace Pentaforce.Beta.Wiki.ViewModels
{
    public class DiceViewModel : IEnumerable<DiceType>
    {
        private IEnumerable<DiceType> Dice { get; init; } = default;

        public double Value { get; init; } = default;

        public double ExplodeValue { get; init; } = default;

        public double MaxValue { get; init; } = default;

        public double ExplodeMaxValue { get; init; } = default;

        public DiceViewModel(params DiceType[] dice)
        {
            Dice = dice;
            Value = Dice.Select(x => x.GetValue()).Sum();
            ExplodeValue = Math.Round(Dice.Select((x, indx) =>
            {
                var avg = x.GetValue();
                var sides = x.GetSides();
                var explodeChance = 1d / (double)sides;
                return explodeChance * avg + avg;
            }).Sum(), 2);
            MaxValue = Dice.Select(x => ((int)x.GetValue()) * 2).Sum();
            ExplodeMaxValue = MaxValue * 2;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not DiceViewModel otherDice) return base.Equals(obj);

            //if (Dice.Count() != otherDice.Count()) return false;

            //foreach (var die in Dice.Select((x, indx) => (x, indx)))
            //{
            //    if (otherDice.ElementAt(die.indx) != die.x) return false;
            //}

            if (ExplodeValue != otherDice.ExplodeValue) return false;

            return true;
        }

        public override string ToString()
        {
            return string.Join(" ", Dice);
        }

        public IEnumerator<DiceType> GetEnumerator()
        {
            return Dice.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static IEnumerable<DiceViewModel> GetDice()
        {
            var result = new List<DiceViewModel>();
            var dice = Enum.GetValues<DiceType>();

            foreach (var item in GetDiceList(7))
            {
                var viewModel = new DiceViewModel(item.OrderBy(x => (int)x).ToArray());
                if (result.Any(x => x.Equals(viewModel))) continue;
                result.Add(viewModel);
            }

            return result;
        }

        private static ICollection<ICollection<DiceType>> GetDiceList(int expectedCount, ICollection<DiceType> dice = null)
        {
            if (dice == null) dice = new List<DiceType>();

            var result = new List<ICollection<DiceType>>();

            foreach (var die in Enum.GetValues<DiceType>())
            {
                if (dice.Contains(die)) continue;

                var list = dice.ToList();
                list.Add(die);

                result.Add(list);

                if (expectedCount == list.Count) continue;

                result.AddRange(GetDiceList(expectedCount, list));
            }

            return result;
        }
    }
}
