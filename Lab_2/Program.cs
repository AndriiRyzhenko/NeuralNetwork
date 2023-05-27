using CsvHelper;
using Lab_2.Models;
using System.Globalization;
using System.Text;

class Program
{
    const double Pi = 3.14159265358979323846;

    public static int[,] baseRules = new int[,]
    {
        {1, 1, 6},
        {1, 2, 6},
        {1, 3, 6},
        {1, 4, 6},
        {1, 5, 6},
        {1, 6, 6},
        {2, 1, 6},
        {2, 2, 6},
        {2, 3, 6},
        {2, 4, 5},
        {2, 5, 5},
        {2, 6, 5},
        {3, 1, 6},
        {3, 2, 6},
        {3, 3, 6},
        {3, 4, 5},
        {3, 5, 5},
        {3, 6, 5},
        {4, 1, 2},
        {4, 2, 2},
        {4, 3, 2},
        {4, 4, 1},
        {4, 5, 1},
        {4, 6, 1},
        {5, 1, 2},
        {5, 2, 2},
        {5, 3, 2},
        {5, 4, 1},
        {5, 5, 1},
        {5, 6, 1},
        {6, 1, 1},
        {6, 2, 1},
        {6, 3, 1},
        {6, 4, 1},
        {6, 5, 1},
        {6, 6, 1}
    };

    static void Main()
    {
        double dt = 0.01;
        double dmu = 0.1;

        double t = 0;
        double tMax = 50;

        double phi = 25 * Pi / 180;
        double omega = 2 * Pi / 180;
        double phiMin = -Pi;
        double phiMax = Pi;
        double omegaMin = -Pi / 9;
        double omegaMax = Pi / 9;
        double muMin = -Pi / 36;
        double muMax = Pi / 36;

        List<Point> results = new();

        while (t < tMax - 0.5 * dt)
        {
            double phiCalc = 200 * (phi - phiMin) / (phiMax - phiMin) - 100;
            double omegaCalc = 200 * (omega - omegaMin) / (omegaMax - omegaMin) - 100;

            double muCalc = -100;
            double s1 = 0;
            double s2 = 0;

            while (muCalc < 100 - 0.5 * dmu)
            {
                double xiDash = 0;

                for (int i = 0; i < baseRules.GetLength(0); i++)
                {
                    double alfa = CalculateMembership(baseRules[i, 0], phiCalc);
                    double beta = CalculateMembership(baseRules[i, 1], omegaCalc);
                    double gama = Math.Min(alfa, beta);
                    double delta = CalculateMembership(baseRules[i, 2], muCalc);
                    double xi = Math.Min(gama, delta);
                    if (xiDash < xi)
                    {
                        xiDash = xi;
                    }
                }

                s1 = s1 + muCalc * xiDash * dmu;
                s2 = s2 + xiDash * dmu;
                muCalc = muCalc + dmu;
            }

            double mu_dash = s1 / s2;
            double mu = (mu_dash + 100) * (muMax - muMin) / 200 + muMin;

            phi = phi + omega * dt;
            omega = omega + mu * dt;

            results.Add(new Point() { T = t, Omega = omega, Phi = phi, Mu = mu });

            t = t + dt;
            Console.WriteLine(t);
        }

        AppendCsv(results);

    }

    private static void AppendCsv(List<Point> results)
    {
        using (var writer = new StreamWriter(Path.Combine("../../../Data", "Data.csv"), false, Encoding.UTF8))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(results);
        }
    }

    public static double CalculateMembership(int term, double x)
    {
        return term switch
        {
            1 => CalculateMembership(x, -1000, -200, -100, -50),
            2 => CalculateMembership(x, -100, -50, -50, -10),
            3 => CalculateMembership(x, -50, -10, 0, 0),
            4 => CalculateMembership(x, 0, 0, 10, 50),
            5 => CalculateMembership(x, 10, 50, 50, 100),
            6 => CalculateMembership(x, 50, 100, 200, 1000),
            _ => 0
        };
    }

    public static double CalculateMembership(double x, double a, double b, double c, double d)
    {
        if (x <= a)
        {
            return 0;
        }
        else if (x > a && x <= b)
        {
            return (x - a) / (b - a);
        }
        else if (x > b && x <= c)
        {
            return 1;
        }
        else if (x > c && x <= d)
        {
            return (d - x) / (d - c);
        }
        else
        {
            return 0;
        }
    }
}