﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace LikeLinq
{   
    public class LinqService<T>
    {
        private List<T> data;

        public LinqService(List<T> data)
        {
            this.data = data;
        }

        public static LinqService<T> From(List<T> data)
        {
            return new LinqService<T>(data);
        }

        public List<T> ToArray()
        {
            return new List<T>(data);
        }

        public LinqService<T> Where(Func<T, bool> predicate)
        {
            List<T> filteredData = new List<T>();
            foreach (var item in data)
            {
                if (predicate(item))
                {
                    filteredData.Add(item);
                }
            }
            this.data = filteredData;
            return this;
        }

        public LinqService<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            List<TResult> result = new List<TResult>();
            foreach (var item in data)
            {
                result.Add(selector(item));
            }
            return new LinqService<TResult>(result);
        }

        public LinqService<T> OrderBy<TKey>(Func<T, TKey> keySelector)
        {
            data.Sort((a, b) =>
            {
                var keyA = keySelector(a);
                var keyB = keySelector(b);
                return Comparer<TKey>.Default.Compare(keyA, keyB);
            });
            return this;
        }

        public LinqService<T> OrderByDescending<TKey>(Func<T, TKey> keySelector)
        {
            data.Sort((a, b) =>
            {
                var keyA = keySelector(a);
                var keyB = keySelector(b);
                return Comparer<TKey>.Default.Compare(keyB, keyA);
            });
            return this;
        }
        public int Count() { 
            return data.Count; 
        }

        public T First()
        {
            if (data.Count == 0)
            {
                throw new InvalidOperationException("No elements");
            }
            return data[0]; 
        }

        public T FirstOrDefault(T defaultValue)
        {
            return data.Count > 0 ? data[0] : defaultValue;
        }


        public bool Any(Func<T, bool> predicate)
        {
            foreach (var item in data)
            {
                if (predicate(item))
                {
                    return true;
                }
            }
            return false;
        }

        public bool All(Func<T, bool> predicate)
        {
            foreach (var item in data)
            {
                if (!predicate(item))
                {
                    return false;
                }
            }
            return true;
        }

        public LinqService<T> Distinct()
        {
            var distinctData = new HashSet<T>(data);
            this.data = new List<T>(distinctData);
            return this;
        }

        public Dictionary<TKey, List<T>> GroupBy<TKey>(Func<T, TKey> keySelector)
        {
            var groupedData = new Dictionary<TKey, List<T>>();

            foreach (var item in data)
            {
                var key = keySelector(item);
                if (!groupedData.ContainsKey(key))
                {
                    groupedData[key] = new List<T>(); 
                }
                groupedData[key].Add(item);
            }

            return groupedData;
        }


        public List<KeyValuePair<TKey, List<T>>> GroupByToKeyValueList<TKey>(Func<T, TKey> keySelector)
        {
            var groupedData = new Dictionary<TKey, List<T>>();

            foreach (var item in data)
            {
                var key = keySelector(item);
                if (!groupedData.ContainsKey(key))
                {
                    groupedData[key] = new List<T>();
                }
                groupedData[key].Add(item); 
            }

            return groupedData.ToList();
        }


        public double Average(Func<T, double> selector)
        {
            if (data.Count == 0)
            {
                throw new InvalidOperationException("No elements");
            }

            var sum = 0.0;
            foreach (var item in data)
            {
                sum += selector(item);
            }
            return sum / data.Count;
        }

        public T Max(Func<T, double> selector)
        {
            if (data.Count == 0)
            {
                throw new InvalidOperationException("No elements");
            }

            var maxValue = selector(data[0]);  
            var maxItem = data[0];

            foreach (var item in data)
            {
                var currentValue = selector(item);
                if (currentValue > maxValue)
                {
                    maxValue = currentValue;
                    maxItem = item;
                }
            }
            return maxItem;
        }

        public T Min(Func<T, double> selector)
        {
            if (data.Count == 0)
            {
                throw new InvalidOperationException("No elements");
            }

            var minValue = selector(data[0]); 
            var minItem = data[0];

            foreach (var item in data)
            {
                var currentValue = selector(item);
                if (currentValue < minValue)
                {
                    minValue = currentValue;
                    minItem = item;
                }
            }
            return minItem;
        }


        public double Sum(Func<T, double> selector)
        {
            if (data.Count == 0)
            {
                throw new InvalidOperationException("No elements");
            }

            double sum = 0;

            foreach (var item in data)
            {
                sum += selector(item);
            }
            return sum;
        }

        public LinqService<TResult> Join<TInner, TKey, TResult>(
            IEnumerable<TInner> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector)
        {
            var result = new List<TResult>();

            foreach (var outerItem in data)
            {
                var outerKey = outerKeySelector(outerItem);

                foreach (var innerItem in inner)
                {
                    var innerKey = innerKeySelector(innerItem);

                    if (EqualityComparer<TKey>.Default.Equals(outerKey, innerKey))
                    {
                        result.Add(resultSelector(outerItem, innerItem));
                    }
                }
            }

            return new LinqService<TResult>(result);
        }


    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Grade { get; set; }

        public Student(int id, string name, int age, string grade)
        {
            Id = id;
            Name = name;
            Age = age;
            Grade = grade;
        }
    }
    public class Score
    {
        public int StudentId { get; }
        public double ScoreValue { get; }

        public Score(int studentId, double scoreValue)
        {
            StudentId = studentId;
            ScoreValue = scoreValue;
        }
    }

}
