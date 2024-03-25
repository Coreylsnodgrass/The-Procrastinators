using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS_Project
{
    // To manage the simulation of the virtual memory system. This could include methods to read memory references from a file, as well as methods to simulate the memory accesses and update the TLB, page table, and data cache based on the results of the memory accesses.
    internal class MemorySimulator
    {

        private TLB tlb;
        private PageTable pageTable;
        private DataCache dataCache;

        public MemorySimulator(TLB tlb, PageTable pageTable, DataCache dataCache)
        {
            this.tlb = tlb;
            this.pageTable = pageTable;
            this.dataCache = dataCache;
        }

        public void SimulateMemoryAccesses(string filePath)
        {
            // Read memory references from the file
            var memoryReferences = ReadMemoryReferencesFromFile(filePath);

            // Simulate the memory accesses
            foreach (var reference in memoryReferences)
            {
                var virtualAddress = reference.VirtualAddress;
                var accessType = reference.AccessType;

                // Translate the virtual address to a physical address
                var physicalAddress = TranslateVirtualAddress(virtualAddress);

                // Access the memory location
                if (accessType == AccessType.READ)
                {
                    Console.WriteLine("Reading from address {0:X8} (Physical: {1:X8})", virtualAddress, physicalAddress);
                }
                else
                {
                    Console.WriteLine("Writing to address {0:X8} (Physical: {1:X8})", virtualAddress, physicalAddress);
                }

                // Update the TLB, page table, and data cache based on the results of the memory access
                UpdateMemoryStructures(virtualAddress, physicalAddress);
            }

            // Display statistics
            DisplayStatistics();
        }

        private List<MemoryReference> ReadMemoryReferencesFromFile(string filePath)
        {
            var memoryReferences = new List<MemoryReference>();
            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        var accessType = parts[0].ToUpper() == "R" ? AccessType.READ : AccessType.WRITE;
                        var virtualAddress = Convert.ToUInt32(parts[1], 16);
                        memoryReferences.Add(new MemoryReference(accessType, virtualAddress));
                    }
                }
            }
            return memoryReferences;
        }

        private uint TranslateVirtualAddress(uint virtualAddress)
        {
            // Perform TLB lookup
            var tlbEntry = tlb.LookupEntry((int)(virtualAddress >> 12));
            if (tlbEntry != null)
            {
                // If TLB hit, return the physical address
                return (uint)(tlbEntry.Value << 12) | (virtualAddress & 0xFFF);
            }

            // Perform page table lookup
            var pageTableEntry = pageTable.LookupEntry((int)(virtualAddress >> 12));
            if (pageTableEntry == null)
            {
                // If page table miss, generate a page fault
                throw new InvalidOperationException("Page table miss");
            }

            // Return the physical address
            return (uint)pageTableEntry.PhysicalAddress << 12 | (virtualAddress & 0xFFF);
        }

        private void UpdateMemoryStructures(uint virtualAddress, uint physicalAddress)
        {
            // Update the TLB
            tlb.AddEntry((int)(virtualAddress >> 12), physicalAddress.ToString("X8"));

            // Update the page table
            // (No need to update the page table for this simplified example)

            // Update the data cache
            // (No need to update the data cache for this simplified example)
        }

        private void DisplayStatistics()
        {
            /*
            // Display statistics
            // (No need to display statistics for this simplified example)
            */
        }
    }

        internal enum AccessType
        {
            READ,
            WRITE
        }

        internal class MemoryReference
        {
            public AccessType
        }
}




