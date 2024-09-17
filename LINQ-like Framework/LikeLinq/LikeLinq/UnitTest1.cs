using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace LikeLinq.Tests
{
    [TestClass]
    public class MyLinqTests
    {
        private static readonly Person[] _people = new[]
        {
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "Bob", Age = 25 },
            new Person { Name = "Charlie", Age = 35 }
        };

        private static readonly int[] _numbers = new[] { 1, 2, 3, 4, 5 };

        [TestMethod]
        public void Where_Should_Filter_Elements()
        {
            var queryable = new QueryableArray<int>(_numbers);
            var result = queryable.Where(x => x > 3);
            CollectionAssert.AreEqual(new[] { 4, 5 }, result.ToList());
        }

        [TestMethod]
        public void Where_Should_Filter_Elements_With_Complex_Condition()
        {
            var queryable = new QueryableArray<Person>(_people);
            var result = queryable.Where(p => p.Age > 25 && p.Name.StartsWith("A"));
            CollectionAssert.AreEqual(new[] { _people[0] }, result.ToList());
        }

        [TestMethod]
        public void Where_Should_Return_Empty_List_When_No_Match()
        {
            var queryable = new QueryableArray<int>(_numbers);
            var result = queryable.Where(x => x > 10);
            CollectionAssert.AreEqual(new List<int>(), result.ToList());
        }

        [TestMethod]
        public void Select_Should_Project_To_New_Type()
        {
            var queryable = new QueryableArray<int>(_numbers);
            var result = queryable.Select(x => x * 2);
            CollectionAssert.AreEqual(new[] { 2, 4, 6, 8, 10 }, result.ToList());
        }

        [TestMethod]
        public void Select_Should_Create_Anonymous_Type()
        {
            var queryable = new QueryableArray<Person>(_people);
            var result = queryable.Select(p => new { p.Name, IsAdult = p.Age >= 18 }).ToList();
            Assert.AreEqual("Alice", result[0].Name);
            Assert.AreEqual(true, result[0].IsAdult);
            Assert.AreEqual("Bob", result[1].Name);
            Assert.AreEqual(true, result[1].IsAdult);
            Assert.AreEqual("Charlie", result[2].Name);
            Assert.AreEqual(true, result[2].IsAdult);
        }

        [TestMethod]
        public void OrderBy_Should_Sort_In_Ascending_Order()
        {
            var queryable = new QueryableArray<int>(_numbers);
            var result = queryable.OrderBy(x => x);
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5 }, result.ToList());
        }

        [TestMethod]
        public void OrderByDescending_Should_Sort_In_Descending_Order()
        {
            var queryable = new QueryableArray<int>(_numbers);
            var result = queryable.OrderByDescending(x => x);
            CollectionAssert.AreEqual(new[] { 5, 4, 3, 2, 1 }, result.ToList());
        }

        [TestMethod]
        public void OrderBy_Should_Sort_People_By_Age()
        {
            var queryable = new QueryableArray<Person>(_people);
            var result = queryable.OrderBy(p => p.Age).ToList();
            Assert.AreEqual("Bob", result[0].Name);
            Assert.AreEqual("Alice", result[1].Name);
            Assert.AreEqual("Charlie", result[2].Name);
        }

        [TestMethod]
        public void OrderByDescending_Should_Sort_People_By_Age_Descending()
        {
            var queryable = new QueryableArray<Person>(_people);
            var result = queryable.OrderByDescending(p => p.Age).ToList();
            Assert.AreEqual("Charlie", result[0].Name);
            Assert.AreEqual("Alice", result[1].Name);
            Assert.AreEqual("Bob", result[2].Name);
        }

        // Additional test methods for edge cases

        [TestMethod]
        public void Where_Should_Handle_Empty_Data()
        {
            var queryable = new QueryableArray<int>(new int[0]);
            var result = queryable.Where(x => x > 0);
            CollectionAssert.AreEqual(new List<int>(), result.ToList());
        }

        [TestMethod]
        public void Select_Should_Handle_Empty_Data()
        {
            var queryable = new QueryableArray<int>(new int[0]);
            var result = queryable.Select(x => x * 2);
            CollectionAssert.AreEqual(new List<int>(), result.ToList());
        }

        [TestMethod]
        public void OrderBy_Should_Handle_Empty_Data()
        {
            var queryable = new QueryableArray<int>(new int[0]);
            var result = queryable.OrderBy(x => x);
            CollectionAssert.AreEqual(new List<int>(), result.ToList());
        }

        [TestMethod]
        public void OrderByDescending_Should_Handle_Empty_Data()
        {
            var queryable = new QueryableArray<int>(new int[0]);
            var result = queryable.OrderByDescending(x => x);
            CollectionAssert.AreEqual(new List<int>(), result.ToList());
        }
    }
}
