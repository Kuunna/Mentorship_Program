﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LikeLinq
{
    [TestClass]
    public class LinqServiceTests
    {
        [TestMethod]
        public void Where_ShouldFilterNumbersBasedOnPredicate()
        {
            var numbers = new List<int> { 1, 2, 3, 4, 5 };
            var predicate = new Func<int, bool>(n => n > 2);
            var result = LinqService<int>.From(numbers).Where(predicate).ToArray();

            CollectionAssert.AreEqual(new[] { 3, 4, 5 }, result);
        }

        [TestMethod]
        public void Where_ShouldFilterStudentsBasedOnPredicate()
        {
            var students = new List<Student>
            {
                new Student(1, "John", 16, "A"),
                new Student(2, "Alice", 17, "B"),
                new Student(3, "Bob", 16, "A"),
            };
            var predicate = new Func<Student, bool>(student => student.Grade == "A");
            var result = LinqService<Student>.From(students).Where(predicate).ToArray();

            CollectionAssert.AreEqual(new[] { students[0], students[2] }, result);
        }

        [TestMethod]
        public void Select_ShouldMapNumbersBasedOnSelectorFunction()
        {
            var numbers = new List<int> { 1, 2, 3, 4, 5 };
            var selector = new Func<int, int>(n => n * 2);

            var result = LinqService<int>.From(numbers).Select(selector).ToArray();

            CollectionAssert.AreEqual(new[] { 2, 4, 6, 8, 10 }, result);
        }

        [TestMethod]
        public void Select_ShouldMapStudentsBasedOnSelectorFunction()
        {
            var students = new List<Student>
            {
                new Student(1, "John", 16, "A"),
                new Student(2, "Alice", 17, "B"),
                new Student(3, "Bob", 16, "A"),
            };
            var selector = new Func<Student, string>(student => student.Name);

            var result = LinqService<Student>.From(students).Select(selector).ToArray();

            CollectionAssert.AreEqual(new[] { "John", "Alice", "Bob" }, result);
        }

        [TestMethod]
        public void OrderBy_ShouldOrderNumbersInAscendingOrder()
        {
            var numbers = new List<int> { 5, 3, 1, 4, 2 };
            var keySelector = new Func<int, int>(n => n);

            var result = LinqService<int>.From(numbers).OrderBy(keySelector).ToArray();

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5 }, result);
        }

        [TestMethod]
        public void OrderBy_ShouldOrderStudentsByAgeInAscendingOrder()
        {
            var students = new List<Student>
            {
                new Student(1, "John", 16, "A"),
                new Student(2, "Alice", 17, "B"),
                new Student(3, "Bob", 15, "A"),
            };
            var keySelector = new Func<Student, int>(student => student.Age);

            var result = LinqService<Student>.From(students).OrderBy(keySelector).ToArray();

            CollectionAssert.AreEqual(
                new[] { 15, 16, 17 },
                result.Select(student => student.Age).ToArray()
            );

        }


        [TestMethod]
        public void OrderByDescending_ShouldOrderNumbersInDescendingOrder()
        {
            var numbers = new List<int> { 5, 3, 1, 4, 2 };
            var keySelector = new Func<int, int>(n => n);

            var result = LinqService<int>.From(numbers).OrderByDescending(keySelector).ToArray();

            CollectionAssert.AreEqual(new[] { 5, 4, 3, 2, 1 }, result);
        }

        [TestMethod]
        public void OrderByDescending_ShouldOrderStudentsByAgeInDescendingOrder()
        {
            var students = new List<Student>
            {
                new Student(1, "John", 16, "A"),
                new Student(2, "Alice", 17, "B"),
                new Student(3, "Bob", 15, "A"),
            };
            var keySelector = new Func<Student, int>(student => student.Age);

            var result = LinqService<Student>.From(students).OrderByDescending(keySelector).ToArray();

            CollectionAssert.AreEqual(
                new[] { 17, 16, 15 }, 
                result.Select(s => s.Age).ToArray()
                );
        }

        [TestMethod]
        public void Count_ShouldReturnNumberOfElementsInSequence()
        {
            var numbers = new List<int> { 1, 2, 3 };

            var result = LinqService<int>.From(numbers).Count();

            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void First_ShouldReturnFirstElement()
        {
            var numbers = new List<int> { 1, 2, 3 };

            var result = LinqService<int>.From(numbers).First();

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void First_ShouldThrowExceptionIfSequenceIsEmpty()
        {
            var numbers = new List<int>();

            LinqService<int>.From(numbers).First();
        }

        [TestMethod]
        public void FirstOrDefault_ShouldReturnFirstElement()
        {
            var numbers = new List<int> { 1, 2, 3 };

            var result = LinqService<int>.From(numbers).FirstOrDefault(0);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void FirstOrDefault_ShouldReturnDefaultValueIfSequenceIsEmpty()
        {
            var numbers = new List<int>();

            var result = LinqService<int>.From(numbers).FirstOrDefault(0);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Any_ShouldReturnTrueIfAnyElementSatisfiesPredicate()
        {
            var numbers = new List<int> { 1, 2, 3 };
            var predicate = new Func<int, bool>(n => n > 2);

            var result = LinqService<int>.From(numbers).Any(predicate);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Any_ShouldReturnFalseIfNoElementSatisfiesPredicate()
        {
            var numbers = new List<int> { 1, 2, 3 };
            var predicate = new Func<int, bool>(n => n > 3);

            var result = LinqService<int>.From(numbers).Any(predicate);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void All_ShouldReturnTrueIfAllElementsSatisfyPredicate()
        {
            var numbers = new List<int> { 1, 2, 3 };
            var predicate = new Func<int, bool>(n => n > 0);

            var result = LinqService<int>.From(numbers).All(predicate);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void All_ShouldReturnFalseIfAnyElementDoesNotSatisfyPredicate()
        {
            var numbers = new List<int> { 1, 2, 3 };
            var predicate = new Func<int, bool>(n => n > 1);

            var result = LinqService<int>.From(numbers).All(predicate);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Distinct_ShouldReturnOnlyDistinctElements()
        {
            var numbers = new List<int> { 1, 2, 2, 3, 3, 3 };

            var result = LinqService<int>.From(numbers).Distinct().ToArray();

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, result);
        }

        [TestMethod]
        public void GroupBy_ShouldGroupNumbersByValue()
        {
            var numbers = new List<int> { 1, 2, 2, 3, 1, 4 };
            var keySelector = new Func<int, int>(n => n);

            var result = LinqService<int>.From(numbers).GroupBy(keySelector);
            var expected = new Dictionary<int, List<int>>
            {
                { 1, new List<int> { 1, 1 } },
                { 2, new List<int> { 2, 2 } },
                { 3, new List<int> { 3 } },
                { 4, new List<int> { 4 } }
            };

            foreach (var key in expected.Keys)
            {
                CollectionAssert.AreEqual(expected[key], result[key]);
            }
        }

        [TestMethod]
        public void GroupBy_ShouldGroupStudentsByGrade()
        {
            var students = new List<Student>
            {
                new Student(1, "John", 16, "A"),
                new Student(2, "Alice", 17, "B"),
                new Student(3, "Bob", 15, "A"),
                new Student(4, "Eve", 16, "B"),
            };
            var keySelector = new Func<Student, string>(student => student.Grade);

            var result = LinqService<Student>.From(students).GroupBy(keySelector);

            var expected = new Dictionary<string, List<Student>>
            {
                { "A", new List<Student> { students[0], students[2] } },
                { "B", new List<Student> { students[1], students[3] } }
            };

            foreach (var key in expected.Keys)
            {
                CollectionAssert.AreEqual(expected[key], result[key]);
            }
        }

        [TestMethod]
        public void GroupByToKeyValueList_ShouldGroupNumbersByModule()
        {
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
            var keySelector = new Func<int, int>(n => n % 2);

            var result = LinqService<int>.From(numbers).GroupByToKeyValueList(keySelector);

            var expected = new List<KeyValuePair<int, List<int>>>
            {
                new KeyValuePair<int, List<int>>(1, new List<int> { 1, 3, 5 }),
                new KeyValuePair<int, List<int>>(0, new List<int> { 2, 4, 6 }),
            };

            Assert.AreEqual(expected.Count, result.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Key, result[i].Key);
                CollectionAssert.AreEqual(expected[i].Value, result[i].Value);
            }
        }


        [TestMethod]
        public void GroupByToKeyValueList_ShouldGroupStudentsByGrade()
        {
            var students = new List<Student>
            {
                new Student(1, "John", 16, "A"),
                new Student(2, "Alice", 17, "B"),
                new Student(3, "Bob", 16, "A"),
                new Student(4, "Eve", 17, "B"),
            };

            var keySelector = new Func<Student, string>(student => student.Grade);

            var expected = new List<KeyValuePair<string, List<Student>>>
            {
                new KeyValuePair<string, List<Student>>("A", new List<Student> { students[0], students[2] }),
                new KeyValuePair<string, List<Student>>("B", new List<Student> { students[1], students[3] }),
            };

            var result = LinqService<Student>.From(students).GroupByToKeyValueList(keySelector);

            Assert.AreEqual(expected.Count, result.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Key, result[i].Key);
                CollectionAssert.AreEqual(expected[i].Value, result[i].Value);
            }
        }


        [TestMethod]
        public void Average_ShouldCalculateAverageForCustomSelector()
        {
            var students = new List<Student>
            {
                new Student(1, "John", 16, "A"),
                new Student(2, "Alice", 17, "B"),
                new Student(3, "Bob", 15, "A")
            };
            var selector = new Func<Student, double>(student => student.Age);

            var result = LinqService<Student>.From(students).Average(selector);
            Assert.AreEqual(16.0, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Average_ShouldThrowExceptionForEmptyList()
        {
            var emptyList = new List<int>();
            var selector = new Func<int, double>(n => n);

            LinqService<int>.From(emptyList).Average(selector);
        }

        [TestMethod]
        public void Max_ShouldReturnStudentWithHighestAge()
        {
            var students = new List<Student>
            {
                new Student(1, "John", 16, "A"),
                new Student(2, "Alice", 17, "B"),
                new Student(3, "Bob", 15, "A")
            };
            var selector = new Func<Student, double>(student => student.Age);

            var result = LinqService<Student>.From(students).Max(selector);

            Assert.AreEqual(students[1], result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Max_ShouldThrowException_WhenNoElements()
        {
            var students = new List<Student>();
            var selector = new Func<Student, double>(student => student.Age);

            LinqService<Student>.From(students).Max(selector);
        }

        [TestMethod]
        public void Min_ShouldReturnStudentWithLowestAge()
        {
            var students = new List<Student>
        {
            new Student(1, "John", 16, "A"),
            new Student(2, "Alice", 17, "B"),
            new Student(3, "Bob", 15, "A")
        };
            var selector = new Func<Student, double>(student => student.Age);

            var result = LinqService<Student>.From(students).Min(selector);

            Assert.AreEqual(students[2], result); // Bob có tuổi thấp nhất (15)
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Min_ShouldThrowException_WhenNoElements()
        {
            var students = new List<Student>();
            var selector = new Func<Student, double>(student => student.Age);

            LinqService<Student>.From(students).Min(selector);
        }

        [TestMethod]
        public void Sum_ShouldReturnSumOfStudentAges()
        {
            var students = new List<Student>
            {
                new Student(1, "John", 16, "A"),
                new Student(2, "Alice", 17, "B"),
                new Student(3, "Bob", 15, "A")
            };
            var selector = new Func<Student, double>(student => student.Age);

            var result = LinqService<Student>.From(students).Sum(selector);

            Assert.AreEqual(48, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Sum_ShouldThrowExceptionForEmptyList()
        {
            var students = new List<Student>();
            var selector = new Func<Student, double>(student => student.Age);

            LinqService<Student>.From(students).Sum(selector);
        }

        [TestMethod]
        public void Join_ShouldCombineStudentsAndScores()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student(1, "John", 16, "A"),
                new Student(2, "Alice", 17, "B"),
            };

            var scores = new List<Score>
            {
                new Score(1, 90),
                new Score(2, 85),
            };

            var outerKeySelector = new Func<Student, int>(student => student.Id);
            var innerKeySelector = new Func<Score, int>(score => score.StudentId);
            var resultSelector = new Func<Student, Score, object>((student, score) => new
            {
                StudentName = student.Name,
                ScoreValue = score.ScoreValue
            });

            // Act
            var result = LinqService<Student>.From(students)
                .Join(scores, outerKeySelector, innerKeySelector, resultSelector)
                .ToArray();

            // Assert
            var expected = new[]
            {
                new { StudentName = "John", ScoreValue = 90 },
                new { StudentName = "Alice", ScoreValue = 85 },
            };

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].StudentName, ((dynamic)result[i]).StudentName);
                Assert.AreEqual(expected[i].ScoreValue, ((dynamic)result[i]).ScoreValue);
            }
        }
    }
}
