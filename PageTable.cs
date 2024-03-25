using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS_Project
{
    //To manage the page table entries for the virtual memory system. This could include methods to add and remove pages,
    //  as well as methods to look up a page in the table.
    internal class PageTable
    {
        // Define the data structure to store the page table entries
        Dictionary<int, PageTableEntry> pageTable;

        // Define a constructor to initialize the page table
        public PageTable()
        {
            pageTable = new Dictionary<int, PageTableEntry>();
        }

        // Implement methods for inserting, updating, and looking up page table entries

        public PageTableEntry GetPageTableEntry(int virtualPageNumber)
        {
            if (pageTable.ContainsKey(virtualPageNumber))
            {
                return pageTable[virtualPageNumber];
            }

            return null;
        }

        public void InsertPageTableEntry(int virtualPageNumber, PageTableEntry pageTableEntry)
        {
            pageTable[virtualPageNumber] = pageTableEntry;
        }

        public void UpdatePageTableEntry(int virtualPageNumber, PageTableEntry pageTableEntry)
        {
            if (pageTable.ContainsKey(virtualPageNumber))
            {
                pageTable[virtualPageNumber] = pageTableEntry;
            }
        }

        // Implement methods for page fault handling

        public void HandlePageFault(int virtualPageNumber)
        {
            // Implement page fault handling
            // Implement a page replacement policy
        }
    }

    // Define a class for PageTableEntry
    public class PageTableEntry
    {
        public int FrameNumber { get; set; }
        public bool Valid { get; set; }
        public bool Dirty { get; set; }
        public List<byte> ProtectionBits { get; set; }
        public bool ReferenceBit { get; set; }

        // Implement constructors, getters, and setters
    }
}
