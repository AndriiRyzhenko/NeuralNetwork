namespace Lab_3.Models;

public class Result
{
    public double TrainEpsilonSquareSum { get; set; }
    public double TestEpsilonSquareSum { get; set; }

    public List<string> ToCsv(int era)
    {
        double trainEpsilonSquareSum = TrainEpsilonSquareSum / 2;
        double trainEAverage = trainEpsilonSquareSum / 1000;
        double testEpsilonSquareSum = TestEpsilonSquareSum / 2;
        double testEpsilonAverage = testEpsilonSquareSum / 4;

        return new List<string> { era.ToString(), trainEpsilonSquareSum.ToString(), trainEAverage.ToString(),
                testEpsilonSquareSum.ToString(), testEpsilonAverage.ToString() };
    }
}

