﻿using Polynomial;
using System.Text;

#region variable defines
string polynomial1, polynomial2, finalPolynomial;
List<Monomial> monomials1 = new();
List<Monomial> monomials2 = new();
List<Monomial> finalMonomials = new();
double degree, coefficient;
char? arg;
#endregion

void GetPolynomials()
{
    Console.Clear();
    Console.CursorVisible = true;
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("  1st Polynomial: ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    polynomial1 = Console.ReadLine() + " "; // Getting the first polynomial from user
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("  2nd Polynomial: ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    polynomial2 = Console.ReadLine() + " "; // Getting the second polynomial from user
    Console.CursorVisible = false;

    polynomial1 = polynomial1.Replace(" ", ""); // Making polynomial1 ready for calculations
    polynomial1 = polynomial1.Replace("+", " +");
    polynomial1 = polynomial1.Replace("-", " -");
    polynomial1 = polynomial1.Trim();
    polynomial1 += " ";

    polynomial2 = polynomial2.Replace(" ", ""); // Making polynomial2 ready for calculations
    polynomial2 = polynomial2.Replace("+", " +");
    polynomial2 = polynomial2.Replace("-", " -");
    polynomial2 = polynomial2.Trim();
    polynomial2 += " ";
}

bool? BreakIntoMonomials()
{
    int hatIndex = 0, spaceIndex = -1, argIndex = -1;
    arg = null;
    foreach (char c in polynomial1) // Set the name of variable
        if (c != '^' && (c > '9' || c < 0) && c != ' ' && c != '.')
        {
            arg = c;
            break;
        }
    if (arg == null)
        foreach (char c in polynomial2) // Set the name of variable
            if (c != '^' && (c > '9' || c < 0) && c != ' ' && c != '.')
            {
                arg = c;
                break;
            }
    arg ??= 'x';
    while (true) // Fix one-degree isues in polynomial1
    {
        argIndex = polynomial1.IndexOf(Convert.ToChar(arg), argIndex + 1);
        if (argIndex == -1) break;
        if (polynomial1[argIndex + 1] != '^')
            polynomial1 = polynomial1.Insert(argIndex + 1, "^1");
    }
    while (true) // Fix one-degree isues polynomial2
    {
        argIndex = polynomial2.IndexOf(Convert.ToChar(arg), argIndex + 1);
        if (argIndex == -1) break;
        if (polynomial2[argIndex + 1] != '^')
            polynomial2 = polynomial2.Insert(argIndex + 1, "^1");
    }
    while (true) // " 2x^3 "    // Breaking polynomial1 into monomials and storing them in a list
    {
        if (polynomial1 == " ") break; // Do nothing if the polynomial is empty
        if (spaceIndex == polynomial1.Length - 1) break; // All monomials are stored
        hatIndex = polynomial1.IndexOf("^", hatIndex + 1); // = 3
        if (hatIndex == -1 || polynomial1.Substring(spaceIndex + 1, hatIndex - spaceIndex).IndexOf(" ") != -1) // The monomial is zero-degree
        {
            coefficient = Convert.ToDouble(polynomial1.Substring(spaceIndex + 1, polynomial1.IndexOf(" ", spaceIndex + 1) - spaceIndex - 1)); // " +36 "
            degree = 0;
            monomials1.Add(new Monomial(degree, coefficient));
            spaceIndex = polynomial1.IndexOf(" ", spaceIndex + 1);
            if (hatIndex != -1) hatIndex = polynomial1.LastIndexOf("^", hatIndex - 1);
            continue;
        }
        if (arg != polynomial1[hatIndex - 1]) return false; // Check the variables to be the same
        coefficient = polynomial1.Substring(spaceIndex + 1, hatIndex - spaceIndex - 2) switch // = 2
        {
            "" => 1,
            "+" => 1,
            "-" => -1,
            _ => Convert.ToDouble(polynomial1.Substring(spaceIndex + 1, hatIndex - spaceIndex - 2)),
        };
        spaceIndex = polynomial1.IndexOf(" ", spaceIndex + 1); // = 5
        degree = Convert.ToDouble(polynomial1.Substring(hatIndex + 1, spaceIndex - hatIndex - 1)); // = 3
        monomials1.Add(new Monomial(degree, coefficient));
    }
    hatIndex = 0;
    spaceIndex = -1;
    while (true) // " 52x^10 "    // Breaking polynomial2 into monomials and storing them in a list
    {
        if (polynomial2 == " ") break; // Do nothing if the polynomial is empty
        if (spaceIndex == polynomial2.Length - 1) break; // All monomials are stored
        hatIndex = polynomial2.IndexOf("^", hatIndex + 1); // = 4
        if (hatIndex == -1 || polynomial2.Substring(spaceIndex + 1, hatIndex - spaceIndex).IndexOf(" ") != -1) // The monomial is zero-degree
        {
            coefficient = Convert.ToDouble(polynomial2.Substring(spaceIndex + 1, polynomial2.IndexOf(" ", spaceIndex + 1) - spaceIndex - 1)); // " +4 "
            degree = 0;
            monomials2.Add(new Monomial(degree, coefficient));
            spaceIndex = polynomial2.IndexOf(" ", spaceIndex + 1);
            if (hatIndex != -1) hatIndex = polynomial2.LastIndexOf("^", hatIndex - 1);
            continue;
        }

        if (arg != polynomial2[hatIndex - 1]) return null; // Check the variables to be the same
        coefficient = polynomial2.Substring(spaceIndex + 1, hatIndex - spaceIndex - 2) switch // = 52
        {
            "" => 1,
            "+" => 1,
            "-" => -1,
            _ => Convert.ToDouble(polynomial2.Substring(spaceIndex + 1, hatIndex - spaceIndex - 2)),
        };
        spaceIndex = polynomial2.IndexOf(" ", hatIndex + 1); // = 7
        degree = Convert.ToDouble(polynomial2.Substring(hatIndex + 1, spaceIndex - hatIndex - 1)); // = 10
        monomials2.Add(new Monomial(degree, coefficient));
    }
    return true;
}

void SortPolynomials()
{
    if (monomials1.Count > 1) QuickSortPoly(monomials1, 0, monomials1.Count - 1);
    if (monomials2.Count > 1) QuickSortPoly(monomials2, 0, monomials2.Count - 1);
    for (int i = 0; i < monomials1.Count - 1; i++)
    {
        if (monomials1[i].Degree == monomials1[i + 1].Degree)
        {
            monomials1[i].Coefficient += monomials1[i + 1].Coefficient;
            monomials1.Remove(monomials1[i + 1]);
            i--;
        }
    }
    for (int i = 0; i < monomials2.Count - 1; i++)
    {
        if (monomials2[i].Degree == monomials2[i + 1].Degree)
        {
            monomials2[i].Coefficient += monomials2[i + 1].Coefficient;
            monomials2.Remove(monomials2[i + 1]);
            i--;
        }
    }
}

void QuickSortPoly(List<Monomial> list, int leftIndex, int rightIndex)
{
    var i = leftIndex;
    var j = rightIndex;
    var pivot = list[leftIndex].Degree;
    while (i <= j)
    {
        while (list[i].Degree > pivot)
            i++;
        while (list[j].Degree < pivot)
            j--;

        if (i <= j)
        {
            (list[j], list[i]) = (list[i], list[j]);
            i++;
            j--;
        }
    }

    if (leftIndex < j)
        QuickSortPoly(list, leftIndex, j);
    if (i < rightIndex)
        QuickSortPoly(list, i, rightIndex);
}

void Add()
{
    Monomial a, b;
    finalMonomials.Clear();
    while (true)
    {
        if (monomials1.Count == 0 && monomials2.Count == 0) // All monomials are written
            break;
        else if (monomials1.Count == 0) // Only 2nd polynomial is remaining
        {
            b = monomials2.First();
            monomials2.Remove(monomials2.First());
            degree = b.Degree;
            coefficient = b.Coefficient;
        }
        else if (monomials2.Count == 0) // Only 1st polynomial is remaining
        {
            a = monomials1.First();
            monomials1.Remove(monomials1.First());
            degree = a.Degree;
            coefficient = a.Coefficient;
        }
        else // Both polynomials still have some monomials
        {
            a = monomials1.First();
            b = monomials2.First();
            if (a.Degree > b.Degree) // cx^5 , cx^3
            {
                monomials1.Remove(monomials1.First());
                degree = a.Degree;
                coefficient = a.Coefficient;
            }
            else if (a.Degree < b.Degree) // cx^3 , cx^5
            {
                monomials2.Remove(monomials2.First());
                degree = b.Degree;
                coefficient = b.Coefficient;
            }
            else // cx^3 , cx^3
            {
                monomials1.Remove(monomials1.First());
                monomials2.Remove(monomials2.First());
                degree = a.Degree;
                coefficient = a.Coefficient + b.Coefficient;
            }
        }
        degree = Math.Round(degree, 5, MidpointRounding.ToEven);
        coefficient = Math.Round(coefficient, 5, MidpointRounding.ToEven);
        finalMonomials.Add(new Monomial(degree, coefficient));
    }
}

void WriteFinalPolynomial()
{
    bool isPretty = true;
    Console.ForegroundColor = ConsoleColor.Blue;
    finalPolynomial = $"  --> f({arg}) = ";
    foreach (var monomial in finalMonomials)
    {
        degree = monomial.Degree;
        coefficient = monomial.Coefficient;
        if (coefficient == 0) // Write nothing for zero
            continue;
        else if (coefficient > 0 && finalPolynomial.Length != 13) // Check the sign
            finalPolynomial += "+ ";
        else if (coefficient < 0)
            finalPolynomial += "- ";
        coefficient = (coefficient > 0) ? coefficient : -coefficient;
        if (coefficient == 1)
        {
            if (degree == 0)
                finalPolynomial += $"1 ";
            else if (degree == 1)
                finalPolynomial += $"{arg} ";
            else
            {
                if (degree > 3)
                {
                    isPretty = false;
                    finalPolynomial += $"{arg}^{degree} ";
                }
                else if (isPretty)
                    finalPolynomial += degree switch
                    {
                        2 => $"{arg}\xB2 ",
                        3 => $"{arg}\xB3 ",
                        _ => $"{arg}^{degree} "
                    };
                else
                    finalPolynomial += $"{arg}^{degree} ";
            }
        } // Writing one-coefficient monomial
        else
        {
            if (degree == 0)
                finalPolynomial += $"{coefficient} ";
            else if (degree == 1)
                finalPolynomial += $"{coefficient}{arg} ";
            else
            {
                if (degree > 3)
                {
                    isPretty = false;
                    finalPolynomial += $"{coefficient}{arg}^{degree} ";
                }
                else if (isPretty)
                    finalPolynomial += degree switch
                    {
                        2 => $"{coefficient}{arg}\xB2 ",
                        3 => $"{coefficient}{arg}\xB3 ",
                        4 => $"{coefficient}{arg}^{degree} ",
                        5 => $"{coefficient}{arg}^{degree} ",
                        6 => $"{coefficient}{arg}^{degree} ",
                        7 => $"{coefficient}{arg}^{degree} ",
                        8 => $"{coefficient}{arg}^{degree} ",
                        9 => $"{coefficient}{arg}^{degree} ",
                        _ => $"{coefficient}{arg}^{degree} "
                    };
                else
                    finalPolynomial += $"{coefficient}{arg}^{degree} ";
            }
        }// Writing not-one-coefficient monomial
    }
    if (polynomial1 != " " && finalPolynomial.Length == 13) finalPolynomial += "0";
    Console.WriteLine(finalPolynomial);
}

void ClaculateByValue()
{
    Console.ForegroundColor = ConsoleColor.Red;
    double argValue, answer = 0;
    Console.CursorVisible = true;
    Console.Write($"  {arg} = ");
    while (true) // Get value of x until its valid
        try
        {
            argValue = Convert.ToDouble(Console.ReadLine());
            break;
        }
        catch
        {
            Console.SetCursorPosition(5, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(6, Console.CursorTop - 1);
        }
    Console.CursorVisible = false;
    Console.SetCursorPosition(argValue.ToString().Length + 6, Console.CursorTop - 1);
    Console.Write($" --> f({argValue}) = ");
    if (finalPolynomial.Length == 13) return;
    foreach (var monomial in finalMonomials) // Calculate the sum of all monomials based on the given vlaue of x
        answer += monomial.Coefficient * Math.Pow(argValue, monomial.Degree);
    Console.Write(answer); // Write the value of f(x)
}

Enums.WhatToDo WhatToDo()
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("\n\n  R: restart   |   ESC: exit");
    while (true)
    {
        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.R:
                Console.Clear();
                return Enums.WhatToDo.restart;
            case ConsoleKey.Escape:
                return Enums.WhatToDo.exit;
            default:
                break;
        }
    }
}

void CancelHandler(object? sender = null, ConsoleCancelEventArgs? args = null)
{
    Console.ResetColor();
    Console.Write("\n");
}

while (true)
{
    Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelHandler);
    Console.OutputEncoding = Encoding.Unicode;
    Console.Title = "Polynomial";
    GetPolynomials();
    try // Check for any possible invalid inputs
    {
        bool? state = BreakIntoMonomials();
        if (state == false)
            Console.Write("  Invalid input! Please enter single-variable polynomial!");
        else if (state == null)
            Console.Write("  Variables are not the same!");
        else
        {
            SortPolynomials();
            Add();
            WriteFinalPolynomial();
            ClaculateByValue();
        }
    }
    catch
    {
        Console.Write("  Invalid inputs! Please enter valid polynomials!");
    }
    if (WhatToDo() == Enums.WhatToDo.exit) // Continue receiving inputs if 'restart', and end the program if 'exit'
    {
        CancelHandler();
        return;
    }
}