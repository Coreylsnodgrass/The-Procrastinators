using System;
using System.Collections.Generic;
using System.Linq;

namespace MHS_Project
{
    internal class LRUMemoryManager
    {
        private LinkedList<uint> lruList; // LinkedList to keep track of page usage order for LRU
        private Dictionary<uint, LinkedListNode<uint>> pageNodeMap; // Maps virtual page numbers to nodes in the LinkedList
        private Dictionary<uint, uint> virtualToPhysicalMap; // Maps virtual page numbers to physical page numbers
        private Dictionary<uint, uint> physicalToVirtualMap; // Reverse mapping for easy lookup
        private int physicalPageCount;

        public LRUMemoryManager(int physicalPageCount)
        {
            this.physicalPageCount = physicalPageCount;
            lruList = new LinkedList<uint>();
            pageNodeMap = new Dictionary<uint, LinkedListNode<uint>>();
            virtualToPhysicalMap = new Dictionary<uint, uint>();
            physicalToVirtualMap = new Dictionary<uint, uint>();
        }

        public (bool hit, uint physicalPage) AccessPage(uint virtualPageNumber)
        {
            // Check for a hit
            if (virtualToPhysicalMap.TryGetValue(virtualPageNumber, out var physicalPage))
            {
                // Move accessed page to the end of the LRU list (most recently used)
                var node = pageNodeMap[virtualPageNumber];
                lruList.Remove(node);
                lruList.AddLast(node);

                return (true, physicalPage);
            }

            // Miss: Need to load the page into memory
            // If memory is full, evict the least recently used page
            if (lruList.Count >= physicalPageCount)
            {
                var leastUsedVirtualPage = lruList.First.Value;
                lruList.RemoveFirst();
                pageNodeMap.Remove(leastUsedVirtualPage);

                var oldestPhysicalPage = virtualToPhysicalMap[leastUsedVirtualPage];
                virtualToPhysicalMap.Remove(leastUsedVirtualPage);
                physicalToVirtualMap.Remove(oldestPhysicalPage);

                // Reuse the physical page for the new virtual page
                virtualToPhysicalMap[virtualPageNumber] = oldestPhysicalPage;
                physicalToVirtualMap[oldestPhysicalPage] = virtualPageNumber;
            }
            else
            {
                // If there's still room, find the next available physical page number
                uint newPhysicalPage = 0;
                while (physicalToVirtualMap.ContainsKey(newPhysicalPage))
                {
                    newPhysicalPage++;
                }

                virtualToPhysicalMap[virtualPageNumber] = newPhysicalPage;
                physicalToVirtualMap[newPhysicalPage] = virtualPageNumber;
            }

            // Add the new or updated page to the LRU list (as the most recently used page)
            var newNode = lruList.AddLast(virtualPageNumber);
            pageNodeMap[virtualPageNumber] = newNode;

            return (false, virtualToPhysicalMap[virtualPageNumber]);  // Return the physical page number (miss)
        }
    }
}
