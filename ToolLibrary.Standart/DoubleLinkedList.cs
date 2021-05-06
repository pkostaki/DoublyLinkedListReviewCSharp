using System;
using System.Collections;
using System.Collections.Generic;

namespace ToolLibrary.Standart
{
    /// <summary>
    /// Represents a node in a <see cref="DoubleLinkedList{T}"/>.
    /// </summary>
    /// <typeparam name="T">Specifies the value type.</typeparam>
    public class DoubleLinkedListNode<T>
    {
        internal DoubleLinkedListNode(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Reference to the previous node.
        /// </summary>
        public DoubleLinkedListNode<T> Prev { get; internal set; }

        /// <summary>
        /// Reference to the next node.
        /// </summary>
        public DoubleLinkedListNode<T> Next { get; internal set; }
  
        /// <value>Get the value of node.</value>
        public T Value { get; }
    }

    
    /// <summary>
    /// Represents a doubly linked list.
    /// </summary>
    /// <typeparam name="T"> Specifies the element type of the linked list.</typeparam>
    public class DoubleLinkedList<T> : IEnumerable<DoubleLinkedListNode<T>>
    {
        ///<value>Get the first node.</value>
        public DoubleLinkedListNode<T> FirstNode { get; private set; }

        ///<value>Get the last node.</value>
        public DoubleLinkedListNode<T> LastNode { get; private set; }

        ///<value>Get count of nodes.</value>
        public int Count { get; private set; } = 0;

      
        public DoubleLinkedListNode<T> this[int i]
        {
            get
            {
                if (i < 0 || i >= Count)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return Find(i);
            }
            set {
                if (i < 0 || i >= Count)
                {
                    throw new ArgumentOutOfRangeException();
                }
                var node = Find(i);
                value.Prev = node.Prev;
                node.Prev.Next = value;

                value.Next = node.Next;
                node.Next.Prev = value;
            }
        }

        /// <summary>
        /// Adds a new node containing the specified <paramref name="value"/> at the end of the <see cref="DoubleLinkedList{T}"></see>.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <returns>Added node.</returns>
        public DoubleLinkedListNode<T> Add(T value)
        {
            return Add(new DoubleLinkedListNode<T> (value)); 
        }

        /// <summary>
        /// Adds a <paramref name="node"/> at the end of the <see cref="DoubleLinkedList{T}"></see>.
        /// </summary>
        /// <param name="node">The node to add.</param>
        /// <returns>Added node.</returns>
        public DoubleLinkedListNode<T> Add(DoubleLinkedListNode<T> node)
        {
            return Count == 0 ? AddFirstNode(node) : InsertAfter(LastNode, node);
        }

        /// <summary>
        /// Inserts a new node containing the specified <paramref name="value"/> at the specified <paramref name="index"/> of the <see cref="DoubleLinkedList{T}"></see>.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="value">The object to insert.</param>
        /// <returns>Added node.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown exception when <paramref name="index"/> is out of range.</exception>
        public DoubleLinkedListNode<T> Insert(int index, T value)
        {
            // behaviour simular C# List
            if (Count == 0 && index==0) 
            {
                return AddFirstNode(new DoubleLinkedListNode<T>(value));
            }

            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return InsertBefore(Find(index), new DoubleLinkedListNode<T> (value));
        }

        /// <summary>
        /// Inserts a new node containing the specified <paramref name="value"/> before <paramref name="beforeNode"/> of the <see cref="DoubleLinkedList{T}"></see>.
        /// </summary>
        /// <param name="beforeNode">Node, before which will inserting.</param>
        /// <param name="value">Value to insert.</param>
        /// <returns>Inserted node.</returns>
        public DoubleLinkedListNode<T> Insert(DoubleLinkedListNode<T> beforeNode, T value)
        {
            return InsertBefore(beforeNode, new DoubleLinkedListNode<T>(value));
        }

        /// <summary>
        /// Inserts a <paramref name="node"/> before <paramref name="beforeNode"/> of the <see cref="DoubleLinkedList{T}"></see>.
        /// </summary>
        /// <param name="beforeNode">Node, before which will inserting.</param>
        /// <param name="node">Node to insert.</param>
        /// <returns>Inserted node.</returns>
        public DoubleLinkedListNode<T> Insert(DoubleLinkedListNode<T> beforeNode, DoubleLinkedListNode<T> node)
        {
            return InsertBefore(beforeNode, node);
        }

        private DoubleLinkedListNode<T> AddFirstNode(DoubleLinkedListNode<T> node)
        {
            FirstNode = LastNode = node;
            Count++;
            return node;
        }

        private DoubleLinkedListNode<T> InsertAfter(DoubleLinkedListNode<T> afterThisNode, DoubleLinkedListNode<T> node)
        {
            node.Prev = afterThisNode;
            if (afterThisNode.Next == null)
            {
                node.Next = null;
                LastNode = node;
            }
            else
            {
                node.Next = afterThisNode.Next;
                afterThisNode.Next.Prev = node;
            }

            afterThisNode.Next = node;

            Count++;
            return node;
        }

        private DoubleLinkedListNode<T> InsertBefore(DoubleLinkedListNode<T> beforeThisNode, DoubleLinkedListNode<T> node)
        {
            node.Next = beforeThisNode;
            if (beforeThisNode.Prev == null)
            {
                node.Prev = null;
                FirstNode = node;
            }
            else
            {
                node.Prev = beforeThisNode.Prev;
                beforeThisNode.Prev.Next = node;
            }
            beforeThisNode.Prev = node;

            Count++;
            return node;
        }

        private DoubleLinkedListNode<T> Find(int index)
        {
            // todo can be optimized by selection of direction of search
            var i = 0;
            DoubleLinkedListNode<T> result = FirstNode;
            while (i != index) 
            {
                result = result.Next;
                i++;
            }
            
            return result;
        }

        /// <summary>
        /// Removes node specified by <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index at list.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown exception when <paramref name="index"/> is out of range.</exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            var findedItem = Find(index);
            if (findedItem != null)
            {
                Remove(findedItem);
            }
        }

        /// <summary>
        /// Removes the <paramref name="node"/> from list.
        /// </summary>
        /// <param name="node">Node to remove.</param>
        public void Remove(DoubleLinkedListNode<T> node)
        {
            //todo: needs to check if node belongs to this list before removing
            if (node.Prev == null)
            {
                FirstNode = node.Next;
            }
            else
            {
                node.Prev.Next = node.Next;
            }

            if (node.Next == null)
            {
                LastNode = node.Prev;
            }
            else
            {
                node.Next.Prev = node.Prev;
            }
            Count--;
        }

        /// <summary>
        /// Adds the nodes of the specified <paramref name="source"/> to the end of the <see cref="DoubleLinkedList{T}"/>.
        /// </summary>
        /// <param name="source"><see cref="DoubleLinkedList{T}"/>.</param>
        public void AddRange(DoubleLinkedList<T> source)
        {
            var startNode = LastNode;
            foreach(var node in source)
            {
                startNode = Count == 0 ? AddFirstNode(new DoubleLinkedListNode<T>(node.Value)) : InsertAfter(startNode, new DoubleLinkedListNode<T> (node.Value));
            }
        }

        /// <summary>
        /// Inserts nodes of the specified <paramref name="source"/> into the <see cref="DoubleLinkedList{T}"/> at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index at which note should be inserted.</param>
        /// <param name="source"><see cref="DoubleLinkedList{T}"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown exception when <paramref name="index"/> is out of range.</exception>
        public void InsertRange(int index, DoubleLinkedList<T> source)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            var startNode = Find(index);
            foreach (var node in source.Reverse())
            {
                startNode = InsertBefore(startNode, new DoubleLinkedListNode<T>(node.Value));
            }
        }

        /// <summary>
        /// Removes a range of nodes from the <see cref="DoubleLinkedList{T}"/>.
        /// </summary>
        /// <param name="startIndex">Start index.</param>
        /// <param name="endIndex">End index.</param>
        /// <exception cref="ArgumentOutOfRangeException">Index is less than 0 or <paramref name="startIndex"/> bigger than <paramref name="endIndex"/> </exception>
        public void RemoveRange(int startIndex, int endIndex)
        {
            endIndex = Math.Min(endIndex, Count - 1); 
            if (startIndex < 0 || startIndex > endIndex)
            {
                throw new ArgumentOutOfRangeException();
            }
            var itemToDelete = Find(startIndex);
            var countToDelete = endIndex - startIndex;
            for (var i = 0; i <= countToDelete; i++)
            {
                Remove(itemToDelete);
                itemToDelete = itemToDelete.Next;
            }
        }

        /// <summary>
        /// Exposes the enumerator, which supports a simple iteration over a list reversly.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> that can be used to iterate through the collection.</returns>
        public IEnumerable<DoubleLinkedListNode<T>> Reverse()
        {
            var iterItem = LastNode;
            while (iterItem != null)
            {
                yield return iterItem;
                iterItem = iterItem.Prev;
            }
        }
       
        /// <inheritdoc/>
        public IEnumerator<DoubleLinkedListNode<T>> GetEnumerator()
        {
            var iterItem = FirstNode;
            while (iterItem != null)
            {
                yield return iterItem;
                iterItem = iterItem.Next;
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
