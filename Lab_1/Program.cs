using CsvHelper;
using Lab_1.Models;
using System.Globalization;
using System.Text;

class Program
{
    private static Point mainPoint = new Point(-3, 3, 1, 5, 0.1f, 8, 8);
    private static float Increment = 9;

    static void Main()
    {
        List<Point> points = new List<Point>();

        for (float i = 1.2f; i < 4.1f; i += 0.5f)
        {
            for (float j = -2.4f; j < 2.4; j += 0.5f)
            {
                Point e = new Point(mainPoint);
                e.Set_X(j);
                e.Set_Y(i);
                points.Add(e);
            }
        }

        List<Result> results = new List<Result>();

        for (int i = 0; i < 1000; i++)
        {
            points.ForEach(point => point.F = (float)Math.Abs(point.Get_X()) - 2 * point.Get_Y());

            Result result = GetResults(points);
            results.Add(result);

            List<Point> newGenerationPoints = new List<Point>();
            List<int> indexesToCross = Roulette(points, points.Sum(p => p.F + Increment), Increment);
            var iterator = indexesToCross.GetEnumerator();

            while (iterator.MoveNext())
            {
                int i1 = iterator.Current;
                if (!iterator.MoveNext())
                {
                    break;
                }
                int i2 = iterator.Current;

                CrossTwoPoints(newGenerationPoints, mainPoint, points[i1], points[i2]);
            }

            newGenerationPoints.ForEach(MutatePoint);
            points = newGenerationPoints;
        }

        AppendCsv(results);
    }

    private static List<int> Roulette(List<Point> entities, float sumF, float increment)
    {
        List<int> winnerIndexes = new List<int>();
        for (int i = 0; i < entities.Count; i++)
        {
            float rouletteResult = (float)(0f + new Random().NextDouble() * (100f - 0f));
            float currentPointer = 0f;
            for (int j = 0; j < entities.Count; j++)
            {
                currentPointer += ((entities[j].F + increment) / sumF) * 100;
                if (rouletteResult <= currentPointer)
                {
                    winnerIndexes.Add(j);
                    break;
                }
            }
        }
        return winnerIndexes;
    }

    private static Result GetResults(List<Point> points)
    {
        Point bestPoints = points.OrderByDescending(e => e.F).FirstOrDefault();

        return new Result
        {
            F_Avg = (points.Sum(p => p.F) / points.Count).ToString("0.000"),
            F_Max = bestPoints.F.ToString("0.000"),
            X_Bytes = bestPoints.X_Bytes,
            Y_Bytes = bestPoints.Y_Bytes,
            X = bestPoints.Get_X().ToString("0.0"),
            Y = bestPoints.Get_Y().ToString("0.0")
        };
    }

    private static void AppendCsv(List<Result> results)
    {
        using (var writer = new StreamWriter(Path.Combine("../../../Data", "Data.csv"), false, Encoding.UTF8))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(results);
        }
    }

    private static void CrossTwoPoints(List<Point> newGenerationList, Point point, Point e1, Point e2)
    {
        Point newE1 = new Point(point);
        Point newE2 = new Point(point);
        (string, string) newXBites = CrossBiteStrings(e1.X_Bytes, e2.X_Bytes);
        newE1.Set_X_Bytes(newXBites.Item1, e1.X_Bytes);
        newE2.Set_X_Bytes(newXBites.Item2, e2.X_Bytes);
        (string, string) newYBites = CrossBiteStrings(e1.Y_Bytes, e2.Y_Bytes);
        newE1.Set_Y_Bytes(newYBites.Item1, e1.Y_Bytes);
        newE2.Set_Y_Bytes(newYBites.Item2, e2.Y_Bytes);

        newGenerationList.Add(newE1);
        newGenerationList.Add(newE2);
    }

    private static void MutatePoint(Point Point)
    {
        if (GetRandomFloat(0f, 100f) < 1f)
        {
            char[] xCharArray = Point.X_Bytes.ToCharArray();
            int indexX = GetRandomInt(0, xCharArray.Length - 1);
            xCharArray[indexX] = xCharArray[indexX] == '0' ? '1' : '0';
            Point.Set_X_Bytes(new string(xCharArray), Point.X_Bytes);
        }
        if (GetRandomFloat(0f, 100f) < 1f)
        {
            char[] yCharArray = Point.Y_Bytes.ToCharArray();
            int indexY = GetRandomInt(0, yCharArray.Length - 1);
            yCharArray[indexY] = yCharArray[indexY] == '0' ? '1' : '0';
            Point.Set_Y_Bytes(new string(yCharArray), Point.Y_Bytes);
        }
    }

    private static (string, string) CrossBiteStrings(string s1, string s2)
    {
        int index = GetRandomInt(1, s1.Length - 1);
        string r1 = s1.Substring(0, index) + s2.Substring(index);
        string r2 = s2.Substring(0, index) + s1.Substring(index);
        return (r1, r2);
    }

    private static int GetRandomInt(int min, int max)
    {
        return new Random().Next(min, max + 1);
    }

    private static float GetRandomFloat(float min, float max)
    {
        return (float)(min + new Random().NextDouble() * (max - min));
    }
}
