namespace TheatricalPlayersRefactoringKata;

public interface IPlayTypeCalculator
{
    int CalculateAmount(int baseAmount, int audience);

    int CalculateCredits(int audience);
}
