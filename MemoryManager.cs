using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS_Project
{
    //To manage the virtual memory system, including TLB, page table, and data cache.
    internal class MemoryManager
    {
            private DataCache _dataCache;
            private PageTable _pageTable;
            private TLB _tlb;

            public MemoryManager(DataCache dataCache, PageTable pageTable, TLB tlb)
            {
                _dataCache = dataCache;
                _pageTable = pageTable;
                _tlb = tlb;
            }

            public uint TranslateAddress(uint virtualAddress)
            {
                // First try to use the TLB
                if (_tlb.Contains(virtualAddress))
                {
                    return _tlb[virtualAddress];
                }

                // If not found in the TLB, use the page table
                uint physicalAddress = _pageTable.TranslateAddress(virtualAddress);

                // Add the translation to the TLB
                _tlb.Add(virtualAddress, physicalAddress);

                return physicalAddress;
            }

            public byte[] GetData(uint virtualAddress)
            {
                uint physicalAddress = TranslateAddress(virtualAddress);

                return _dataCache.GetData(physicalAddress);
            }

            public void WriteData(uint virtualAddress, byte[] data)
            {
                uint physicalAddress = TranslateAddress(virtualAddress);

                _dataCache.WriteData(physicalAddress, data);
            }

            public void LoadPageTable(List<PageTableEntry> pageTableEntries)
            {
                _pageTable.Load(pageTableEntries);
            }

            public void ClearTLB()
            {
                _tlb.Clear();
            }

            public void LoadDataCache(List<CacheEntry> cacheEntries)
            {
                _dataCache.Load(cacheEntries);
            }

            public void ClearDataCache()
            {
                _dataCache.Clear();
            }
     }

        interface ICache
        {
            uint TranslateAddress(uint virtualAddress);
            byte[] GetData(uint physicalAddress);
            void WriteData(uint physicalAddress, byte[] data);
            void Clear();
        }

        interface ITranslator
        {
            bool Contains(uint virtualAddress);
            void Add(uint virtualAddress, uint physicalAddress);
            uint Lookup(uint virtualAddress);
            void Clear();
        }

        interface IMemory
        {
            void LoadPageTable(List<PageTableEntry> pageTableEntries);
            void LoadDataCache(List<CacheEntry> cacheEntries);
        }

        interface IEntry
        {
            uint Address { get; set; }
        }

        internal class PageTableEntry : IEntry
        {
            public uint Address { get; set; }
        }

        internal class CacheEntry : IEntry
        {
            public uint Address { get; set; }
            public byte[] Data { get; set; }
        }

        internal class TLB : ITranslator
        {
            // ...
        }

        internal class DataCache : ICache
        {
            // ...
        }

        internal class PageTable : IMemory
        {
            // ...
        }
}
