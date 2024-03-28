using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS_Project
{
    internal class LRUMemoryManager : IMemoryManager
    {
        private LinkedList<uint> lruList; // Doubly-linked list to track LRU order
        private Dictionary<uint, LinkedListNode<uint>> pageMap; // Maps virtual page numbers to nodes in the LRU list
        private Dictionary<uint, uint> virtualToPhysicalMap; // Maps virtual page numbers to physical page numbers
        private int physicalPageCount;

        public LRUMemoryManager(int physicalPageCount)
        {
            this.physicalPageCount = physicalPageCount;
            lruList = new LinkedList<uint>();
            pageMap = new Dictionary<uint, LinkedListNode<uint>>();
            virtualToPhysicalMap = new Dictionary<uint, uint>();
        }

        public (bool hit, uint physicalPage) AccessPage(uint virtualPageNumber)
        {
            // If the page is already in memory (hit)
            if (pageMap.TryGetValue(virtualPageNumber, out var node))
            {
                // Move the accessed page to the back of the LRU list (most recently used)
                lruList.Remove(node);
                lruList.AddLast(node);
                return (true, virtualToPhysicalMap[virtualPageNumber]);
            }

            // On miss, we need to add the page to memory
            uint physicalPage;
            if (lruList.Count >= physicalPageCount) // If memory is full
            {
                // Evict the least recently used page from the front of the list
                var leastUsedPage = lruList.First;
                lruList.RemoveFirst();
                physicalPage = virtualToPhysicalMap[leastUsedPage.Value];

                // Update mappings to remove the evicted page
                pageMap.Remove(leastUsedPage.Value);
                virtualToPhysicalMap.Remove(leastUsedPage.Value);
            }
            else
            {
                // Find a free physical page (simple approach: use count of pages in memory)
                physicalPage = (uint)lruList.Count;
            }

            // Add the new page as the most recently used
            var newNode = new LinkedListNode<uint>(virtualPageNumber);
            lruList.AddLast(newNode);
            pageMap[virtualPageNumber] = newNode;
            virtualToPhysicalMap[virtualPageNumber] = physicalPage;

            return (false, physicalPage); // Miss
        }
    }
}
