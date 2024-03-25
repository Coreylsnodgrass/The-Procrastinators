using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS_Project
{
    //To manage the data cache entries for the virtual memory system. This could include methods to add and remove cache entries, as well as methods to look up an address in the cache.
    internal class DataCache
    {
            private readonly int _size;
            private readonly int _associativity;
            private readonly int _blockSize;
            private readonly int _numSets;

            private readonly Dictionary<int, LinkedList<CacheBlock>> _sets;

            public DataCache(int size, int associativity, int blockSize)
            {
                _size = size;
                _associativity = associativity;
                _blockSize = blockSize;

                _numSets = _size / (_associativity * _blockSize);

                _sets = new Dictionary<int, LinkedList<CacheBlock>>();

                for (int i = 0; i < _numSets; i++)
                {
                    _sets[i] = new LinkedList<CacheBlock>();
                }
            }

            public bool ContainsAddress(uint address)
            {
                int index = GetIndex(address);
                LinkedListNode<CacheBlock> currentNode = _sets[index].First;

                while (currentNode != null)
                {
                    if (currentNode.Value.Address == address)
                    {
                        return true;
                    }

                    currentNode = currentNode.Next;
                }

                return false;
            }

            public void AddAddress(uint address, byte[] data)
            {
                int index = GetIndex(address);

                CacheBlock newBlock = new CacheBlock(address, data);

                LinkedList<CacheBlock> currentSet = _sets[index];

                LinkedListNode<CacheBlock> lastNode = null;

                foreach (LinkedListNode<CacheBlock> currentNode in currentSet)
                {
                    if (currentNode.Value.Address == address)
                    {
                        currentNode.Value = newBlock;
                        return;
                    }

                    lastNode = currentNode;
                }

                if (lastNode == null)
                {
                    throw new InvalidOperationException("Invalid last node.");
                }

                currentSet.AddAfter(lastNode, newBlock);

                if (currentSet.Count > _associativity)
                {
                    _sets[index].RemoveFirst();
                }
            }

            private int GetIndex(uint address)
            {
                return (int)(address >> _blockSize) % _numSets;
            }

            private class CacheBlock
            {
                public CacheBlock(uint address, byte[] data)
                {
                    Address = address;
                    Data = data;
                }

                public uint Address { get; set; }
                public byte[] Data { get; set; }
            }
        
    }

}
