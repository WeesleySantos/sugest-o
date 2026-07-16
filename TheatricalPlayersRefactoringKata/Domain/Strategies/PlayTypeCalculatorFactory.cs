using System;
using System.Collections.Generic;

namespace TheatricalPlayersRefactoringKata;

public class PlayTypeCalculatorFactory
{
    private readonly IReadOnlyDictionary<string, IPlayTypeCalculator> _calculators;

    public PlayTypeCalculatorFactory()
        : this(new Dictionary<string, IPlayTypeCalculator>
        {
            ["tragedy"] = new TragedyCalculator(),
            ["comedy"] = new ComedyCalculator()
        })
    {
    }

    public PlayTypeCalculatorFactory(
        IReadOnlyDictionary<string, IPlayTypeCalculator> calculators)
    {
        _calculators = calculators;
    }

    public IPlayTypeCalculator Get(string playType)
    {
        if (_calculators.TryGetValue(playType, out var calculator))
        {
            return calculator;
        }

        throw new Exception("unknown type: " + playType);
    }
}
