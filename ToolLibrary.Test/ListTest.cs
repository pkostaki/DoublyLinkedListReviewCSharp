using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using ToolLibrary.Standart;

namespace ToolLibrary.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        [TestCase(new[] { 5 })]
        [TestCase(new[] { 5, 2 })]
        [TestCase(new[] { 5, 4, 3, 2, 1 })]
        public void AddTest(int[] testValues)
        {
            // Testing Add operation includes:
            // 1. test Count property changes
            // 2. test Prev property changes
            // 3. test Next property changes
            // 4. test Value property changes

            var list = new DoubleLinkedList<int>();
            for (var i = 0; i < testValues.Length; i++)
            {
                Assert.IsTrue(list.Count == i, "Wrong nodes count.");
                list.Add(testValues[i]);
                Assert.IsTrue(list.Count == i + 1, "Wrong nodes count.");
            }

            for (var i = 0; i < list.Count; i++)
            {
                var node = list[i];
                var expectedPrev = i == 0 ? null : list[i - 1];
                var expectedNext = i == list.Count - 1 ? null : list[i + 1];
                Assert.IsTrue(node.Prev == expectedPrev, "Wrong prev node.");
                Assert.IsTrue(node.Next == expectedNext, "Wrong next node.");
                Assert.IsTrue(node.Value == testValues[i], "Wrong element value.");

                if (expectedPrev != null)
                {
                    Assert.IsTrue(expectedPrev.Next == node, "Wrong next element.");
                }
                if (expectedNext != null)
                {
                    Assert.IsTrue(expectedNext.Prev == node, "Wrong prev element.");
                }
            }
        }

        [Test]
        [TestCase(new int[0], 0, 9)]
        [TestCase(new[] { 5 }, 0, 9)]
        [TestCase(new[] { 5, 2 }, 0, 9)]
        [TestCase(new[] { 5, 2 }, 1, 9)]
        [TestCase(new[] { 5, 4, 3, 2, 1 }, 0, 9)]
        [TestCase(new[] { 5, 4, 3, 2, 1 }, 2, 9)]
        [TestCase(new[] { 5, 4, 3, 2, 1 }, 3, 9)]
        public void InsertTest(int[] initValues, int insertIndex, int insertValue)
        {
            // Testing Insert operation on includes:
            // 1. test Count property changes
            // 2. test Prev property changes
            // 3. test Next property changes
            // 4. test Value property changes.

            var list = new DoubleLinkedList<int>();
            for (int i = 0; i < initValues.Length; i++)
            {
                list.Add(initValues[i]);
            }

            var expectedPrev = insertIndex == 0 ? null : list[insertIndex - 1];
            var expectedNext = insertIndex == list.Count ? null : list[insertIndex];

            Assert.IsTrue(list.Count == initValues.Length, "Wrong elements count.");
            var insertedNode = list.Insert(insertIndex, insertValue);
            Assert.IsTrue(list.Count == initValues.Length + 1, "Wrong elements count.");

            Assert.IsTrue(insertedNode.Prev == expectedPrev, "Wrong prev element.");
            Assert.IsTrue(insertedNode.Next == expectedNext, "Wrong next element.");
            Assert.IsTrue(insertedNode.Value == insertValue, "Wrong element value.");
            if (expectedPrev != null)
            {
                Assert.IsTrue(expectedPrev.Next == insertedNode, "Wrong next element.");
            }
            if (expectedNext != null)
            {
                Assert.IsTrue(expectedNext.Prev == insertedNode, "Wrong prev element.");
            }
        }

        [Test]
        [TestCase(new[] { 5 })]
        [TestCase(new[] { 5, 2 })]
        [TestCase(new[] { 5, 4, 3, 2, 1 })]
        public void IndexerTest(int[] testValues)
        {
            var list = new DoubleLinkedList<int>();
            var addedNodes = new DoubleLinkedListNode<int>[testValues.Length];
            for (int i = 0; i < testValues.Length; i++)
            {
                addedNodes[i] = list.Add(testValues[i]);
            }

            for (int i = 0; i < testValues.Length; i++)
            {
                Assert.IsTrue(list[i] == addedNodes[i], "Wrong node returning by indexer.");
            }
            
            // Todo check set operation.
        }

        [Test]
        [TestCase(new[] { 5 }, 0, false)]
        [TestCase(new[] { 5 }, 1, true)]
        [TestCase(new int[0], 0, true)]
        [TestCase(new[] { 5, 2 }, 0, false)]
        [TestCase(new[] { 5, 2 }, 1, false)]
        [TestCase(new[] { 5, 4, 3, 2, 1 }, 3, false)]
        public void RemoveAtTest(int[] testValues, int removeIndex, bool expectException)
        {
            // Remove functionality test includes:
            // 1. test changes of nodes count 
            // 2. test occurrence of exception 
            // 3. test consistency of list.
            var wasException = false;
            var list = new DoubleLinkedList<int>();
            for (int i = 0; i < testValues.Length; i++)
            {
                list.Add(testValues[i]);
            }

            var expectedValuesList = testValues.ToList();
            try
            {
                var prevCountValue = list.Count;
                list.RemoveAt(removeIndex);
                Assert.IsTrue(list.Count == prevCountValue - 1, "Wrong elements count value.");

                // Update expected values range and then compare lists.
                expectedValuesList.RemoveAt(removeIndex);
                for (int i = 0; i < list.Count; i++)
                {
                    Assert.IsTrue(expectedValuesList[i] == list[i].Value, "Wrong list consistency.");

                    if (i > 0)
                    {
                        Assert.IsTrue(list[i - 1].Next == list[i], "Wrong next node.");
                        Assert.IsTrue(list[i - 1] == list[i].Prev, "Wrong prev node.");
                    }
                    if (i < list.Count - 1)
                    {
                        Assert.IsTrue(list[i + 1].Prev == list[i], "Wrong prev node.");
                        Assert.IsTrue(list[i + 1] == list[i].Next, "Wrong next node.");
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                wasException = true;
            }

            Assert.IsTrue(wasException == expectException, "Unexpected exception catched.");
            Assert.IsTrue(wasException == expectException, "Unexpected exception catched.");
        }

        [Test]
        [TestCase(new[] { 5 }, 0, false)]
        [TestCase(new[] { 5 }, 1, true)]
        [TestCase(new int[0], 0, true)]
        [TestCase(new[] { 5, 2 }, 1, false)]
        [TestCase(new[] { 5, 4, 3, 2, 1 }, 3, false)]
        public void RemoveTest(int[] testValues, int removeIndex, bool expectException)
        {
            // Remove functionality test includes:
            // 1. test changes of count of elements
            // 2. test occurrence of exception 
            // 3. test list consistency.

            var wasException = false;
            var list = new DoubleLinkedList<int>();
            var addedNodes = new List<DoubleLinkedListNode<int>>();
            for (int i = 0; i < testValues.Length; i++)
            {
                addedNodes.Add(list.Add(testValues[i]));
            }

            try
            {
                var prevCountValue = list.Count;
                list.Remove(addedNodes[removeIndex]);
                Assert.IsTrue(list.Count == prevCountValue - 1, "Wrong elements count value");

                // Update expected values range and then compare lists.
                addedNodes.RemoveAt(removeIndex);
                for (int i = 0; i < list.Count; i++)
                {
                    Assert.IsTrue(addedNodes[i].Value == list[i].Value, "Wrong list consistency.");
                    if (i > 0)
                    {
                        Assert.IsTrue(list[i - 1].Next == list[i], "Wrong next node.");
                        Assert.IsTrue(list[i - 1] == list[i].Prev, "Wrong prev node.");
                    }
                    if (i < list.Count-1)
                    {
                        Assert.IsTrue(list[i + 1].Prev == list[i], "Wrong prev node.");
                        Assert.IsTrue(list[i + 1] == list[i].Next, "Wrong next node.");
                    }
                }


            }
            catch (ArgumentOutOfRangeException)
            {
                wasException = true;
            }

            Assert.IsTrue(wasException == expectException, "Unexpected exception catched.");
            Assert.IsTrue(wasException == expectException, "Unexpected exception catched.");
        }


        [Test]
        [TestCase(new int[0], new[] { 5, 4 }, new[] { 5, 4 })]
        [TestCase(new[] { 5, 4 }, new[] { 3, 2, 1 }, new[] { 5, 4, 3, 2, 1 })]
        public void AddRangeTest(int[] targetListValues, int[] sourceListValues, int[] expectedResult)
        {
            var targetList = new DoubleLinkedList<int>();
            for (int i = 0; i < targetListValues.Length; i++)
            {
                targetList.Add(targetListValues[i]);
            }

            var sourceList = new DoubleLinkedList<int>();
            for (int i = 0; i < sourceListValues.Length; i++)
            {
                sourceList.Add(sourceListValues[i]);
            }

            targetList.AddRange(sourceList);

            for (int i = 0; i < targetList.Count; i++)
            {
                Assert.IsTrue(targetList[i].Value == expectedResult[i], "Unexpected element.");
            }

            // Aware that source list unchanged.
            for (int i = 0; i < sourceList.Count; i++)
            {
                Assert.IsTrue(sourceList[i].Value == sourceListValues[i], "Unexpected element.");
            }
        }

        [Test]
        [TestCase(new[] { 3 }, new[] { 5, 4 }, 0, new[] { 5, 4, 3 })]
        [TestCase(new[] { 5, 4 }, new[] { 3, 2, 1 }, 1, new[] { 5, 3, 2, 1, 4 })]
        [TestCase(new[] { 5, 4 }, new[] { 3, 2, 1 }, 0, new[] { 3, 2, 1, 5, 4 })]
        public void InsertRangeTest(int[] targetListValues, int[] sourceListValues, int insertIndex, int[] expectedResult)
        {
            var targetList = new DoubleLinkedList<int>();
            for (int i = 0; i < targetListValues.Length; i++)
            {
                targetList.Add(targetListValues[i]);
            }

            var sourceList = new DoubleLinkedList<int>();
            for (int i = 0; i < sourceListValues.Length; i++)
            {
                sourceList.Add(sourceListValues[i]);
            }

            targetList.InsertRange(insertIndex, sourceList);

            for (int i = 0; i < targetList.Count; i++)
            {
                Assert.IsTrue(targetList[i].Value == expectedResult[i], "Unexpected element at target collection.");
            }

            // Aware that source list unchanged.
            for (int i = 0; i < sourceList.Count; i++)
            {
                Assert.IsTrue(sourceList[i].Value == sourceListValues[i], "Unexpected element at source collection.");
            }
        }

        [Test]
        [TestCase(new[] { 3 }, 0, 0, new int[0])]
        [TestCase(new[] { 5, 4, 3, 2, 1 }, 0, 2, new[] { 2, 1})]
        [TestCase(new[] { 5, 4, 3, 2, 1 }, 2, 4, new[] { 5, 4})]
        [TestCase(new[] { 5, 4, 3, 2, 1 }, 2,3, new[] { 5, 4, 1 })]
        public void RemoveRangeTest(int[] testValues, int startIndex, int endIndex, int[] expectedResult)
        {
            var list = new DoubleLinkedList<int>();
            for (int i = 0; i < testValues.Length; i++)
            {
                list.Add(testValues[i]);
            }

            list.RemoveRange(startIndex, endIndex);

            for(var i=0; i < list.Count; i++)
            {
                Assert.IsTrue(list[i].Value == expectedResult[i], "Wrong operation result.");
            }
            // Todo test exceptional situations.
        }
    }
}