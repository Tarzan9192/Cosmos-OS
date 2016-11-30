using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosKernel1
{
    public class Math
    {
        //public static int Add(String a, String b, String c)
        //{
        //    String token1 = a;
        //    String token2 = b;
        //    String variableName = c;
        //    int a = 0;
        //    int b = 0;

        //    if (vs.Contains(token1))
        //    {
        //        a = vs.getVariable(token1).getValue();
        //    }
        //    else
        //    {
        //        //try to parse an int
        //        try
        //        {
        //            a = Int32.Parse(token1);
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine("Not a number");
        //            break;
        //        }
        //    }

        //    if (vs.Contains(token2))
        //    {
        //        b = vs.getVariable(token2).getValue();
        //    }
        //    else
        //    {
        //        //try to parse int
        //        try
        //        {
        //            b = Int32.Parse(token2);
        //        }
        //        catch (Exception ea)
        //        {
        //            Console.WriteLine("Not a number");
        //            break;
        //        }
        //    }

        //    int sum = a + b;
        //    Variable newVar = new Variable(variableName, sum);
        //    vs.Add(newVar);
        //}

        public static double Pow(int n, int p)
        {
            double result = 0;

            //power of 0
            if (p == 0)
            {
                result = n;
            }

            //negative power
            if (p < 0)
            {
                while (p < -1)
                {
                    n *= n;
                    p += 1;
                }
                result = 1 / n;
            }
            while (p > 1)
            {
                n *= n;
                p -= 1;
            }

            return result;
        }

        public static double parseDouble(String s)
        {
            String[] input = s.Split('.');
            //Console.WriteLine(input.Length);
            //Console.WriteLine(input[0] + "." + input[1]);
            if (input.Length == 2)
            {
                try
                {
                    Console.WriteLine("in first try");
                    int whole = int.Parse(input[0]);
                    Console.WriteLine(whole);
                    int fraction = int.Parse(input[1]);
                    double mantissa = (double)fraction / (double)(System.Math.Pow(10, input[1].Length));

                    return (double)whole + mantissa;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("First Block");
                    return -1;
                }
            }
            else
            {
                char[] array = s.ToCharArray();
                if (array[0] == '.')
                {
                    try
                    {
                        double fraction = Int32.Parse(input[0]);
                        return fraction / System.Math.Pow(10, input[0].Length);
                    }
                    catch (Exception ea)
                    {
                        Console.WriteLine(ea.Message);
                        return -1;
                    }
                }
                else
                {
                    try
                    {
                        return (double)Int32.Parse(input[0]);
                    }
                    catch (Exception ez)
                    {
                        Console.WriteLine(ez.Message);
                        return -1;
                    }
                }
            }
        }
    }                
}
