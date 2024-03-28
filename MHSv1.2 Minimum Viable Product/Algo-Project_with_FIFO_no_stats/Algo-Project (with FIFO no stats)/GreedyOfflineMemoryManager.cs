using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS_Project
{
    internal class GreedyOfflineMemoryManager : IMemoryManager
    {
        private int physicalPageCount;
        private HashSet<uint> pagesInMemory;
        private List<uint> futureReferences; // List of virtual page numbers derived from MemoryReference objects
        private int currentReferenceIndex = 0; // Tracks the current position in the list of references

        public GreedyOfflineMemoryManager(int physicalPageCount, List<MemoryReference> listOfRawReferences)
        {
            this.physicalPageCount = physicalPageCount;
            this.pagesInMemory = new HashSet<uint>();
            // Convert and store the list of future virtual page numbers from MemoryReference objects
            this.futureReferences = listOfRawReferences
                .Select(reference => reference.VirtualPageNumber)
                .ToList();
        }

        public (bool hit, uint physicalPage) AccessPage(uint virtualPageNumber)
        {
            // Increment the reference index each time a page is accessed
            currentReferenceIndex++;

            // Check for a page hit
            if (pagesInMemory.Contains(virtualPageNumber))
            {
                return (true, virtualPageNumber); // Simplified assumption: virtualPageNumber is used for physicalPageNumber
            }

            // On a miss, decide if we need to evict a page
            if (pagesInMemory.Count >= physicalPageCount)
            {
                // Determine the optimal page to evict based on future references
                uint pageToEvict = DeterminePageToEvict();
                pagesInMemory.Remove(pageToEvict);
            }

            // Add the new page
            pagesInMemory.Add(virtualPageNumber);
            return (false, virtualPageNumber);
        }

        private uint DeterminePageToEvict()
        {
            int maxIndex = futureReferences.Count; // Maximum possible index (none of the pages will be accessed later than this)
            uint pageToEvict = 0;
            int longestAccessIndex = -1;

            foreach (var page in pagesInMemory)
            {
                int nextAccessIndex = FindNextUseIndex(page);
                if (nextAccessIndex == -1)
                {
                    return page; // Immediately evict a page that won't be accessed again
                }
                if (nextAccessIndex > longestAccessIndex)
                {
                    longestAccessIndex = nextAccessIndex;
                    pageToEvict = page;
                }
            }

            return pageToEvict;
        }

        private int FindNextUseIndex(uint page)
        {
            for (int i = currentReferenceIndex; i < futureReferences.Count; i++)
            {
                if (futureReferences[i] == page)
                {
                    return i;
                }
            }
            return -1; // Page will not be used again
        }
    }
}
