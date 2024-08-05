# Flowgorithm's Exercise - Array

## 📚 Table of Contents
- [Unit Test - Convert Roman numbers to Integers and vice versa](#1-unit-test-for-convert-roman-numbers-to-integers-and-vice-versa)
- [Convert Roman numbers to Integers and vice versa](#2-convert-roman-numbers-to-integers-and-vice-versa)


### 1. Unit Test for Convert Roman numbers to Integers and vice versa

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RomanToInteger;

namespace RomanToIntegerTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestRomanToInt()
        {
            Assert.AreEqual(1, Program.RomanToInt("I"));
            Assert.AreEqual(4, Program.RomanToInt("IV"));
            Assert.AreEqual(9, Program.RomanToInt("IX"));
            Assert.AreEqual(58, Program.RomanToInt("LViiiI"));
            Assert.AreEqual(1994, Program.RomanToInt("McmxCIV"));
        }

        [TestMethod]
        public void TestIntToRoman()
        {
            Assert.AreEqual("I", Program.IntToRoman(1));
            Assert.AreEqual("IV", Program.IntToRoman(4));
            Assert.AreEqual("IX", Program.IntToRoman(9));
            Assert.AreEqual("LVIII", Program.IntToRoman(58));
            Assert.AreEqual("MCMXCIV", Program.IntToRoman(1994));
        }

        [TestMethod]
        public void TestIsInt()
        {
            Assert.IsTrue(Program.IsInt("123"));   
            Assert.IsFalse(Program.IsInt("abc"));   
            Assert.IsFalse(Program.IsInt("12.3"));  
            Assert.IsFalse(Program.IsInt("0"));     
            Assert.IsFalse(Program.IsInt("-5"));    
            Assert.IsFalse(Program.IsInt(""));   
            Assert.IsFalse(Program.IsInt(null));   
        }

        [TestMethod]
        public void TestIsRoman()
        {
            Assert.IsTrue(Program.IsRoman("IV"));
            Assert.IsTrue(Program.IsRoman("MCMXCIV"));
            Assert.IsFalse(Program.IsRoman("123"));
            Assert.IsFalse(Program.IsRoman("ABC"));
            Assert.IsFalse(Program.IsRoman("XYZ"));
            Assert.IsTrue(Program.IsRoman("XLII"));
            Assert.IsFalse(Program.IsInt(""));
            Assert.IsFalse(Program.IsInt(null));
        }
    }
}
```
<img alt="Unit test" src="https://github.com/user-attachments/assets/c0f1614d-d624-47c5-9cb4-76747043f8a2">

### 2. Convert Roman numbers to integers and vice versa
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

- **_IsInt(string input)_**: Check if input is integer type or not?.
```csharp
public static bool IsInt(string input){
    return int.TryParse(input, out int number) && number > 0;
}
```

- **_IsRoman(string input)_**: Check if input is Roman type or not?.
```csharp
public static bool IsRoman(string input) {
    return Regex.IsMatch(input.ToUpper(), @"^[IVXLCDM]+$");
}
```

- **_Main Function_**
```csharp
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RomanToInteger
{
    public class Program
    {
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
                while (num >= item.Key){
                    result += item.Value;
                    num -= item.Key;
                }
            }
            return result;
        }
        public static bool IsInt(string input)
        {
            return int.TryParse(input, out int number) && number > 0;
        }

        public static bool IsRoman(string input) {
            return Regex.IsMatch(input.ToUpper(), @"^[IVXLCDM]+$");
        }
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

<img alt="Main" src="https://github.com/user-attachments/assets/8f70f561-2ccd-4ba8-ade8-0959b2161937">
