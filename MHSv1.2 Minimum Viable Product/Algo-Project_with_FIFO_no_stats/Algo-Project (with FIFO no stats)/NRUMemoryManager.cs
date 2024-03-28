namespace MHS_Project
{
    /// <summary>
    /// Internal class for managing Memory with a Not Recently Used (NRU) eviction policy.
    /// </summary>
    internal class NRUMemoryManager : IMemoryManager
    {
        /// <summary>
        /// A Dictionary to keep track of the order virtual pages were loaded.
        /// </summary>
        private Dictionary<uint, int> nruQueue;

        /// <summary>
        /// A Dictionary to map virtual page numbers to physical page numbers.
        /// </summary>
        private Dictionary<uint, uint> virtualToPhysicalMap;

        /// <summary>
        /// A Dictionary to maintain a reverse mapping for easy lookup.
        /// </summary>
        private Dictionary<uint, uint> physicalToVirtualMap;

        /// <summary>
        /// Physical page count to determine the number of available physical pages.
        /// </summary>
        private int physicalPageCount;

        /// <summary>
        /// Initializes an instance of the NRU class.
        /// </summary>
        /// <param name="physicalPageCount">Number of available physical pages.</param>
        public NRUMemoryManager(int physicalPageCount)
        {
            this.physicalPageCount = physicalPageCount;
            nruQueue = new Dictionary<uint, int>();
            virtualToPhysicalMap = new Dictionary<uint, uint>();
            physicalToVirtualMap = new Dictionary<uint, uint>();
        }

        /// <summary>
        /// Accesses a page in the memory.
        /// </summary>
        /// <param name="virtualPageNumber">The number of the virtual page to access.</param>
        /// <returns>A tuple with a boolean indicating whether the access was a hit or a miss, and the physical page number if the access was a hit.</returns>
        public (bool hit, uint physicalPage) AccessPage(uint virtualPageNumber)
        {
            // Check for a hit
            if (virtualToPhysicalMap.TryGetValue(virtualPageNumber, out var physicalPage))
            {
                UpdateState(virtualPageNumber);
                return (true, physicalPage);
            }

            // Miss: If memory is full, we need to evict the least recently used page
            if (nruQueue.Count >= physicalPageCount)
            {
                var leastRecentlyUsedVirtualPage = FindLeastRecentlyUsedPage();
                var leastRecentlyUsedPhysicalPage = virtualToPhysicalMap[leastRecentlyUsedVirtualPage];

                // Remove the mapping
                nruQueue.Remove(leastRecentlyUsedVirtualPage);
                virtualToPhysicalMap.Remove(leastRecentlyUsedVirtualPage);
                physicalToVirtualMap.Remove(leastRecentlyUsedPhysicalPage);

                // Reuse the physical page for the new virtual page
                virtualToPhysicalMap[virtualPageNumber] = leastRecentlyUsedPhysicalPage;
                physicalToVirtualMap[leastRecentlyUsedPhysicalPage] = virtualPageNumber;

                nruQueue[virtualPageNumber] = 0;

                return (false, leastRecentlyUsedPhysicalPage);
            }
            else
            {
                // Find the next available physical page number
                uint newPhysicalPage = 0;
                while (physicalToVirtualMap.ContainsKey(newPhysicalPage))
                {
                    newPhysicalPage++;
                }

                // Update mappings
                virtualToPhysicalMap[virtualPageNumber] = newPhysicalPage;
                physicalToVirtualMap[newPhysicalPage] = virtualPageNumber;
                nruQueue.Add(virtualPageNumber, 0);

                return (false, newPhysicalPage);  // Miss
            }
        }

        private uint FindLeastRecentlyUsedPage()
        {
            uint victimVirtualPage = 0;
            int minValue = int.MaxValue;

            foreach (var page in nruQueue)
            {
                if (page.Value < minValue)
                {
                    minValue = page.Value;
                    victimVirtualPage = page.Key;
                }
            }

            return victimVirtualPage;
        }

        private void UpdateState(uint virtualPageNumber)
        {
            // If the virtual page number exists in nruQueue, increment the value
            if (nruQueue.ContainsKey(virtualPageNumber))
            {
                nruQueue[virtualPageNumber]++;
            }
            else
            {
                // If not present, initialize the state and add it to the queue
                nruQueue[virtualPageNumber] = 1;
            }
        }
    }
}
