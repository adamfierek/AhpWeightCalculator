using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

var key = ConsoleKey.Y;

//declare R factors
//source: Saaty T., The Analythic Hierarchy and Analytic Network Processes for the Measurement of Intangible Criteria and for Decision-Making
var R = new Dictionary<int, double>
{
    {1, 0},
    {2, 0},
    {3, 0.52},
    {4, 0.89},
    {5, 1.11},
    {6, 1.25},
    {7, 1.35},
    {8, 1.40},
    {9, 1.45},
    {10, 1.49},
    {11, 1.52},
    {12, 1.54},
    {13, 1.56},
    {14, 1.58},
    {15, 1.59}
};

while(key==ConsoleKey.Y)
{
    //Read input data
    Console.Write("1. row of comparsion matrix: ");
    var row = Console.ReadLine().Split(' ').Select(s => s.ToDouble()).ToArray();
    var m = row.Length;
    var c = new double[m, m];

    for (var i = 0; i < m; i++)
    {
        c[0, i] = row[i];
    }

    for (var i = 1; i < m; i++)
    {
        Console.Write($"{i + 1}. row of comparsion matrix: ");
        row = Console.ReadLine().Split(' ').Select(s => s.ToDouble()).ToArray();
        for (var j = 0; j < m; j++)
        {
            c[i, j] = row[j];
        }

    }

    //computing normalized cu matrix
    var cu = new double[m, m];

    for (int i = 0; i < m; i++)
    {
        for (int j = 0; j < m; j++)
        {
            cu[i, j] = c[i, j] / Enumerable.Range(0, m).Select(x => c[x, j]).Sum();
        }
    }


    //computing weights
    var c2 = new double[m];

    Console.WriteLine("Computed weights:");

    for (var i = 0; i < m; i++)
    {
        c2[i] = Enumerable.Range(0, m).Select(x => cu[i, x]).Sum() / cu.Cast<double>().Sum();
        Console.WriteLine("{0:0.0000}", c2[i]);
    }

    //checking consistence of computed data

    var lambda_max = Enumerable.Range(0, m).Select(x => c2[x] * Enumerable.Range(0, m).Select(y => c[y, x]).Sum()).Sum();

    var ci = (lambda_max - m) / (m - 1);

    var cr = ci / R[m];

    Console.WriteLine("CI factor: {0:0.0000}", ci);
    Console.WriteLine("CR factor: {0:0.0000}", cr);
    if (Math.Abs(cr) >= 0.15)
    {
        Console.WriteLine("The C matrix is inconsistent!");
    }

    Console.Write("Continue with next input data? (Y/N)");
    key = Console.ReadKey().Key;
    Console.WriteLine();
}

static class Converters
{
    /// <summary>
    /// Extension method for string to parse fractions in input
    /// </summary>
    /// <param name="s"></param>
    /// <returns>string contains number or fraction</returns>
    public static double ToDouble(this string s)
    {
        s = s.Replace(',', '.');
        if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
        {
            return result;
        }
        var ingedients = s.Split('/');
        return Convert.ToDouble(ingedients[0], CultureInfo.InvariantCulture) / Convert.ToDouble(ingedients[1], CultureInfo.InvariantCulture);
    }
}




