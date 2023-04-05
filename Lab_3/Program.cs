using CsvHelper;
using Lab_3.Models;
using System.Globalization;
using System.Text;

namespace Lab_3;
class Program
{
    private const double K = 1;
    private const double Eta = 0.7;
    private const int Eras = 5000;
    private const int Points = 1000;


    private static WeighCoefficient _weighCoefficient;
    private static List<Point> _trainSet = new();
    private static List<Point> _testSet = new();

    static void Main(string[] args)
    {
        //GenerateData();

        using (var fileReader = new StreamReader(Path.Combine("../../..", "Data", "Data.csv")))
        using (var csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture))
        {
            _trainSet = csvReader.GetRecords<Point>().ToList();
        }

        _testSet.Add(new Point(0, 0, 0));
        _testSet.Add(new Point(0, 1, 1));
        _testSet.Add(new Point(1, 0, 1));
        _testSet.Add(new Point(1, 1, 0));

        //_weighCoefficient = new WeighCoefficient
        //{
        //    W1_10 = 0.1,
        //    W1_11 = -0.3,
        //    W1_12 = 0.4,
        //    W1_20 = -0.7,
        //    W1_21 = -0.1,
        //    W1_22 = 0.01,
        //    W2_10 = 0.4,
        //    W2_11 = -0.2,
        //    W2_12 = 0.1
        //};

        _weighCoefficient = new WeighCoefficient
        {
            W1_10 = 0.5,
            W1_11 = -0.2,
            W1_12 = 0.4,
            W1_20 = -0.2,
            W1_21 = -0.01,
            W1_22 = 0.01,
            W2_10 = 0.8,
            W2_11 = -0.1,
            W2_12 = 0.9
        };


        using StreamWriter epsilonFileWriter = new StreamWriter(Path.Combine("../../../Data", "epsilons.csv"), true);
        using CsvWriter epsilonCsvWriter = new CsvWriter(epsilonFileWriter, CultureInfo.InvariantCulture);

        using StreamWriter weightsFileWriter = new StreamWriter(Path.Combine("../../../Data", "weights.csv"), true);
        using CsvWriter weightsCsvWriter = new CsvWriter(weightsFileWriter, CultureInfo.InvariantCulture);

        Result result;

        for (int i = 1; i <= Eras; i++)
        {
            Console.WriteLine($"Era: {i}");
            result = new Result();

            foreach (Point point in _trainSet)
            {
                double epsilon = CalculatePoint(point, true);
                result.TrainEpsilonSquareSum += Math.Pow(epsilon, 2);
            }

            foreach (Point point in _testSet)
            {
                double epsilon = CalculatePoint(point, false);
                result.TestEpsilonSquareSum += Math.Pow(epsilon, 2);
            }

            epsilonCsvWriter.WriteField(result.ToCsv(i));
            epsilonCsvWriter.NextRecord();

            weightsCsvWriter.WriteField(_weighCoefficient.ToCsv(i));
            weightsCsvWriter.NextRecord();
        }

    }

    public static void GenerateData()
    {
        Random random = new Random();
        var pointList = new List<Point>();

        for (int i = 0; i < Points; i++)
        {
            double x1 = random.NextDouble();
            double x2 = random.NextDouble();

            int d = x2 < x1 + 0.5 && x2 > x1 - 0.5 ? 0 : 1;

            pointList.Add(new Point(x1, x2, d));
        }

        using (var writer = new StreamWriter(Path.Combine("../../../Data", "Data.csv"), false, Encoding.UTF8))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(pointList);
        }
    }

    private static double CalculatePoint(Point point, bool recalculateWeights)
    {
        double u11 = _weighCoefficient.W1_10 * 1 + _weighCoefficient.W1_11 * point.X1 + _weighCoefficient.W1_12 * point.X2;
        double u12 = _weighCoefficient.W1_20 * 1 + _weighCoefficient.W1_21 * point.X1 + _weighCoefficient.W1_22 * point.X2;
        double y11 = CalculateF(K, u11);
        double y12 = CalculateF(K, u12);
        double u21 = _weighCoefficient.W2_10 * 1 + _weighCoefficient.W2_11 * y11 + _weighCoefficient.W2_12 * y12;

        point.Y = CalculateF(K, u21);
        point.Epsilon = point.D - point.Y;

        if (recalculateWeights)
        {
            _weighCoefficient.W2_10 += Eta * point.Epsilon * CalculateFDash(K, u21);
            _weighCoefficient.W2_11 += Eta * point.Epsilon * CalculateFDash(K, u21) * y11;
            _weighCoefficient.W2_12 += Eta * point.Epsilon * CalculateFDash(K, u21) * y12;

            _weighCoefficient.W1_10 += Eta * point.Epsilon * CalculateFDash(K, u21) * _weighCoefficient.W2_11 * CalculateFDash(K, u11);
            _weighCoefficient.W1_11 += Eta * point.Epsilon * CalculateFDash(K, u21) * _weighCoefficient.W2_11 * CalculateFDash(K, u11) * point.X1;
            _weighCoefficient.W1_12 += Eta * point.Epsilon * CalculateFDash(K, u21) * _weighCoefficient.W2_11 * CalculateFDash(K, u11) * point.X2;

            _weighCoefficient.W1_20 += Eta * point.Epsilon * CalculateFDash(K, u21) * _weighCoefficient.W2_12 * CalculateFDash(K, u12);
            _weighCoefficient.W1_21 += Eta * point.Epsilon * CalculateFDash(K, u21) * _weighCoefficient.W2_12 * CalculateFDash(K, u12) * point.X1;
            _weighCoefficient.W1_22 += Eta * point.Epsilon * CalculateFDash(K, u21) * _weighCoefficient.W2_12 * CalculateFDash(K, u12) * point.X2;
        }

        return point.Epsilon;
    }

    public static double CalculateF(double k, double u)
    {
        return 1 / (1 + Math.Exp(-1 * k * u));
    }

    public static double CalculateFDash(double k, double u)
    {
        double F = CalculateF(k, u);
        return F * k * Math.Abs(1 - F);
    }
}
