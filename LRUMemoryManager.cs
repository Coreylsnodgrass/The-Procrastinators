using System;
using System.Collections.Generic;
using System.Linq;

namespace MHS_Project
{
    internal class MemoryManager
    {
        private DataCache _dataCache;
        private PageTable _pageTable;
        private TLB _tlb;

        // LRU Cache Fields
        private readonly int _cacheCapacity = 100; // Example capacity, adjust as needed
        private Dictionary<uint, LinkedListNode<(uint virtualAddress, byte[] data)>> _lruCacheMap;
        private LinkedList<(uint virtualAddress, byte[] data)> _lruList;

        public MemoryManager(DataCache dataCache, PageTable pageTable, TLB tlb, int cacheCapacity)
        {
            _dataCache = dataCache;
            _pageTable = pageTable;
            _tlb = tlb;
            _cacheCapacity = cacheCapacity;

            _lruCacheMap = new Dictionary<uint, LinkedListNode<(uint, byte[])>>();
            _lruList = new LinkedList<(uint, byte[])>();
        }

        public byte[] GetData(uint virtualAddress)
        {
            // First, attempt to retrieve data from the LRU cache
            if (_lruCacheMap.TryGetValue(virtualAddress, out var node))
            {
                // Move accessed node to MRU position
                _lruList.Remove(node);
                _lruList.AddLast(node);
                return node.Value.data;
            }

            // If not in LRU cache, fetch from main data cache (or memory, depending on your design)
            uint physicalAddress = TranslateAddress(virtualAddress);
            byte[] data = _dataCache.GetData(physicalAddress); // This assumes GetData method returns data directly

            // Add the fetched data to the LRU cache
            PutDataInLRUCache(virtualAddress, data);

            return data;
        }

        public void WriteData(uint virtualAddress, byte[] data)
        {
            uint physicalAddress = TranslateAddress(virtualAddress);
            _dataCache.WriteData(physicalAddress, data);

            // Update the LRU cache with the new data
            PutDataInLRUCache(virtualAddress, data);
        }

        private void PutDataInLRUCache(uint virtualAddress, byte[] data)
        {
            if (_lruCacheMap.ContainsKey(virtualAddress))
            {
                // Update existing entry and move to MRU position
                _lruList.Remove(_lruCacheMap[virtualAddress]);
            }
            else if (_lruCacheMap.Count >= _cacheCapacity)
            {
                // Evict LRU entry
                var lru = _lruList.First;
                _lruList.RemoveFirst();
                _lruCacheMap.Remove(lru.Value.virtualAddress);
            }

            // Add new or updated entry to cache
            var entry = (virtualAddress, data);
            _lruCacheMap[virtualAddress] = _lruList.AddLast(entry);
        }

        private uint TranslateAddress(uint virtualAddress)
        {
            // Implement address translation logic here
            // This could involve checking the TLB and falling back to the page table
            return 0; // Placeholder
        }

        // Additional methods and logic for TLB, page table updates, and cache clearing
    }
}
