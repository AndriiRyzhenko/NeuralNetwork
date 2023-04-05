using Numpy;
using Numpy.Models;

namespace Lab_4;

class Program
{
    static void Main(string[] args)
    {
        var x = np.array(new double[,] { { -1, 1, 1 }, { 1, 1, -1 } });

        var w = Hopfild(x);

        Console.WriteLine("Matrix of weighting factors:");
        Console.WriteLine(w);

        var distorted = np.array(new double[,] { { 1 }, { 1 }, { 1 } });

        var result = np.zeros(new Shape(w.shape[0], 1));
        for (int i = 0; i < w.shape[0]; i++)
        {
            var y = np.dot(w[i], distorted);

            result[i, 0] = GetValue(y);
        }

        Print(result, 1);

        var iteration = 2;

        for (; iteration <= 100; iteration++)
        {
            var new_distorted = np.copy(result);

            var newResult = np.zeros(new Shape(w.shape[0], 1));

            for (int i = 0; i < w.shape[0]; i++)
            {
                var y = np.dot(w[i], new_distorted);
                newResult[i, 0] = GetValue(y);
            }

            Print(newResult, iteration);

            if (np.array_equal(result, newResult))
            {
                Console.WriteLine("Final result:");
                Console.WriteLine(result);
                break;
            }

            result = np.copy(newResult);
        }

        if (iteration >= 100)
        {
            Console.WriteLine("Failed to stabilize network");
        }
    }

    static NDarray Hopfild(NDarray X)
    {
        int N = X.shape[0];
        int n = X.shape[1];

        NDarray newNDarray = np.zeros(new Shape(n, n));

        for (int i = 0; i < N; i++)
        {
            NDarray x = X[i];
            newNDarray += np.outer(x, x);
        }

        var result = 1.0 / n * (newNDarray - np.identity(n) * N);

        return result;
    }

    static NDarray GetValue(NDarray y)
    {
        if (y.GetData<double>()[0] < 0)
        {
            return new NDarray(new int[] { -1 });
        }
        else
        {
            return new NDarray(new int[] { 1 });
        }
    }

    //static bool Equal(NDarray left, NDarray right)
    //{
    //    var leftArr = left.GetData<double>();
    //    var rightArr = right.GetData<double>();
    //    return left[0] == right[0] && left[1] == right[1] && left[2] == right[2];
    //}

    static void Print(NDarray array, int era)
    {
        var arr = array.GetData<double>();
        Console.WriteLine($"Iteration: {era,5} Matrix: [{arr[0],2} {arr[1],2} {arr[2],2}]");
    }
}