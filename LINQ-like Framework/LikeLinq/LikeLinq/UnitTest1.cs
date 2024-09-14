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
    }
}
