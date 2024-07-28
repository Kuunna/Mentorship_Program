# Flowgorithm's Exercise - Array

## 📚 Table of Contents
- [Question 1 - Find the element that appears the most in the array](#1-find-the-element-that-appears-the-most-in-the-array)
- [Question 2 - Convert Roman numbers to Decimals and vice versa](#2-convert-roman-numbers-to-decimals-and-vice-versa)

### 1. Find the element that appears the most in the array
Given an integer n, create an array of n elements, each randomly ranging from 0 to 9. Find the element that appears the most in the array

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    static void Main(string[] args)
    {
        Console.Write("Enter the number of array elements (n): ");
        int n = int.Parse(Console.ReadLine());
        int[] arr = new int[n];

        Console.Write("Your array is: ");
        Random random = new Random();
        for (int i = 0; i < n; i++) {
            arr[i] = random.Next(1, 10);
            Console.Write(arr[i] + " ");
        }
        Console.WriteLine();

        Dictionary<int, int> frequency = new Dictionary<int, int>();
        foreach (int num in arr)
        {
            if (frequency.ContainsKey(num))
                frequency[num]++;
            else
                frequency.Add(num, 1);
        }

        // Find the maximum frequency
        int maxCount = frequency.Values.Max();
        // Filter elements with the maximum frequency
        var mostFrequent = frequency.Where(x => x.Value == maxCount).Select(x => x.Key);

        Console.Write("The most frequent elements are: ");
        foreach (int num in mostFrequent)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine("with " + maxCount + " occurrences.");
        Console.ReadKey();
    }
}
```

<img alt="Question 1 - Main" src="https://github.com/user-attachments/assets/18b90129-1664-4693-8f3c-a4b3d91996d9">


### 2. Convert Roman numbers to Decimals and vice versa
Given an decimals number (int n) or roman number (string n). Convert Roman numbers to Decimals and vice versa

- **_RomanToInt(string s)_**: Convert Roman to Decimals numbers.
```csharp
public static int RomanToInt(string s)
{
    var romanNumerals = new Dictionary<char, int>
    {
        {'I', 1},   {'V', 5},
        {'X', 10},  {'L', 50},
        {'C', 100}, {'D', 500},
        {'M', 1000}
    };

    var result = 0;
    var prevValue = 0;

    for (var i = s.Length - 1; i >= 0; i--)
    {
        var currentValue = romanNumerals[s[i]];
        if (currentValue < prevValue)
            result -= currentValue;
        else
            result += currentValue;
        prevValue = currentValue;
    }

    return result;
}
```

- **_IntToRoman(int num)_**: Convert Decimals to Roman numbers.
```csharp
public static string IntToRoman(int num)
{
    var romanNumerals = new Dictionary<int, string>
    {
        {1000, "M"}, {900, "CM"}, {500, "D"}, {400, "CD"},
        {100,  "C"}, {90,  "XC"}, {50,  "L"}, {40,  "XL"},
        {10,   "X"}, {9,   "IX"}, {5,   "V"}, {4,   "IV"},
        {1,    "I"}
    };
    var result = "";
    foreach (var item in romanNumerals)
    {
        while (num >= item.Key)
        {
            result += item.Value;
            num -= item.Key;
        }
    }
    return result;
}
```

- **_Main Function_**
```csharp
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RomanToInteger
{
    internal class Program
    {
        public static void Main()
        {
            string input;
            do {
                Console.Write("Enter the number you want to convert (or 'done' to exit): ");
                input = Console.ReadLine();

                if (input.ToLower() != "done") {
                    if (IsInt(input)) {
                        Console.Write("Decimal number converted to Roman is: ");
                        Console.WriteLine(IntToRoman(int.Parse(input)));
                    }
                    else if (IsRoman(input)) {
                        Console.Write("Roman number converted to Decimal is: ");
                        Console.WriteLine(RomanToInt(input.ToUpper()));
                    }
                    else {
                        Console.WriteLine("Invalid input! Please enter a valid Roman or Decimal number!");
                    }
                }
            } while (input.ToLower() != "done");
            Console.WriteLine("Goodbye!");
            Console.ReadKey();
        }
    }
}
```

<img alt="Question 2 - Main" src="https://github.com/user-attachments/assets/8f70f561-2ccd-4ba8-ade8-0959b2161937">
