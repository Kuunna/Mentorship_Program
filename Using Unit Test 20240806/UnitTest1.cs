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
