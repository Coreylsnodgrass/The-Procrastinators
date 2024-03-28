// Import necessary namespaces for collections, LINQ queries, text manipulation, and asynchronous operations
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Declare a namespace to organize code into a hierarchical structure
namespace MHS_Project
{
    // Define the LRUMemoryManager class which implements the IMemoryManager interface
    internal class LRUMemoryManager : IMemoryManager
    {
        // A doubly-linked list to keep track of the order of page usage, with the most recently used at the end
        private LinkedList<uint> lruList;
        // A mapping from virtual page numbers to their corresponding nodes in the lruList for quick access
        private Dictionary<uint, LinkedListNode<uint>> pageMap;
        // A mapping from virtual page numbers to physical page numbers to simulate a physical memory allocation
        private Dictionary<uint, uint> virtualToPhysicalMap;
        // The total number of physical pages available in the system
        private int physicalPageCount;

        // Constructor to initialize the LRUMemoryManager with a specified number of physical pages
        public LRUMemoryManager(int physicalPageCount)
        {
            this.physicalPageCount = physicalPageCount; // Set the total available physical pages
            lruList = new LinkedList<uint>(); // Initialize the list to track the LRU order
            pageMap = new Dictionary<uint, LinkedListNode<uint>>(); // Initialize the dictionary for virtual page to node mapping
            virtualToPhysicalMap = new Dictionary<uint, uint>(); // Initialize the dictionary for virtual to physical page mapping
        }

        // Method to access a page given its virtual page number
        public (bool hit, uint physicalPage) AccessPage(uint virtualPageNumber)
        {
            // Check if the virtual page is already in memory by looking it up in the pageMap
            if (pageMap.TryGetValue(virtualPageNumber, out var node))
            {
                // If found, it's a hit: the page is already in memory
                // Move the accessed page to the back of the LRU list to mark it as most recently used
                lruList.Remove(node);
                lruList.AddLast(node);
                // Return that it's a hit, and provide the physical page number associated with this virtual page
                return (true, virtualToPhysicalMap[virtualPageNumber]);
            }

            // If the page is not in memory (a miss), we need to add it
            uint physicalPage;
            if (lruList.Count >= physicalPageCount) // Check if memory is full
            {
                // Evict the least recently used page, which is at the front of the LRU list
                var leastUsedPage = lruList.First;
                lruList.RemoveFirst(); // Remove the LRU page from the list
                physicalPage = virtualToPhysicalMap[leastUsedPage.Value]; // Find the physical page number for eviction

                // Update the mappings to remove references to the evicted page
                pageMap.Remove(leastUsedPage.Value);
                virtualToPhysicalMap.Remove(leastUsedPage.Value);
            }
            else
            {
                // If there's still room in memory, allocate a new physical page
                // For simplicity, the new page's physical number is the current count of pages in memory
                physicalPage = (uint)lruList.Count;
            }

            // Add the new or recently accessed page to the end of the LRU list as the most recently used
            var newNode = new LinkedListNode<uint>(virtualPageNumber);
            lruList.AddLast(newNode);
            // Update the mappings for the new or accessed page
            pageMap[virtualPageNumber] = newNode;
            virtualToPhysicalMap[virtualPageNumber] = physicalPage;

            // Return that it's a miss, along with the physical page number where the virtual page has been placed
            return (false, physicalPage); // Indicate a miss
        }
    }
}
