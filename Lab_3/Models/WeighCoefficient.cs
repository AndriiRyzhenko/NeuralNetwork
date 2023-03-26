namespace Lab_3.Models;

public class WeighCoefficient
{
    public double W1_10 { get; set; }
    public double W1_11 { get; set; }
    public double W1_12 { get; set; }

    public double W1_20 { get; set; }
    public double W1_21 { get; set; }
    public double W1_22 { get; set; }

    public double W2_10 { get; set; }
    public double W2_11 { get; set; }
    public double W2_12 { get; set; }

    public List<string> ToCsv(int era)
    {
        return new List<string> {
            era.ToString(),
            W1_10.ToString(),
            W1_11.ToString(),
            W1_12.ToString(),
            W1_20.ToString(),
            W1_21.ToString(),
            W1_22.ToString(),
            W2_10.ToString(),
            W2_11.ToString(),
            W2_12.ToString()
        };
    }
}

