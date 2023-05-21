using CsvHelper.Configuration.Attributes;

namespace Lab_3.Models;

public class Point
{
    public double X1 { get; set; }
    public double X2 { get; set; }
    public double D { get; set; }

    public Point(double X1, double X2, double D)
    {
        this.X1 = X1;
        this.X2 = X2;
        this.D = D;
    }
    [Ignore]
    public double Y { get; set; }
    [Ignore]
    public double Epsilon { get; set; }
}
