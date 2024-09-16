using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;// Chỉ dùng để so sánh kết quả với LINQ

namespace LikeLinq
{
    [TestClass]
    public class MyLinqTests
    {
        [TestMethod]
        public void Where_Should_Filter_Elements()
        {
            var numbers = new[] { 1, 2, 3, 4, 5 };
            var queryable = numbers.AsQueryable(); // Tạo một IQueryable từ danh sách

            var result = queryable.Where(x => x > 3);

            CollectionAssert.AreEqual(new[] { 4, 5 }, result.ToList());
        }
        [TestMethod]
        public void Where_Should_Filter_Elements_With_Complex_Condition()
        {
            var people = new[]
            {
                new Person { Name = "Alice", Age = 30 },
                new Person { Name = "Bob", Age = 25 },
                new Person { Name = "Charlie", Age = 30 }
            };

            var result = people.Where(p => p.Age > 25 && p.Name.StartsWith("A"));

            CollectionAssert.AreEqual(new[] { people[0] }, result.ToList());
        }

        [TestMethod]
        public void Where_Should_Return_Empty_List_When_No_Match()
        {
            var numbers = new[] { 1, 2, 3 };

            var result = numbers.Where(x => x > 10);

            CollectionAssert.AreEqual(new List<int>(), result.ToList());
        }

        [TestMethod]
        public void Select_Should_Project_To_New_Type()
        {
            var numbers = new[] { 1, 2, 3 };

            var result = numbers.Select(x => x * 2);

            CollectionAssert.AreEqual(new[] { 2, 4, 6 }, result.ToList());
        }

        [TestMethod]
        public void Select_Should_Create_Anonymous_Type()
        {
            var people = new[]
            {
                new Person { Name = "Alice", Age = 30 },
                new Person { Name = "Bob", Age = 25 }
            };

            var result = people.Select(p => new { p.Name, IsAdult = p.Age >= 18 });
        }
    }
}
