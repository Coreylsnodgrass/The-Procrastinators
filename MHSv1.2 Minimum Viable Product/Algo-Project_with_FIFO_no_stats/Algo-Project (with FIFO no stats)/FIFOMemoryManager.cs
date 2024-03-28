

namespace MHS_Project
{
    internal class FIFOMemoryManager : IMemoryManager
    {
        private Queue<uint> fifoQueue; // Queue to keep track of the order virtual pages were loaded
        private Dictionary<uint, uint> virtualToPhysicalMap; // Maps virtual page numbers to physical page numbers
        private Dictionary<uint, uint> physicalToVirtualMap; // Reverse mapping for easy lookup
        private int physicalPageCount;

        public FIFOMemoryManager(int physicalPageCount)
        {
            this.physicalPageCount = physicalPageCount;
            fifoQueue = new Queue<uint>();
            virtualToPhysicalMap = new Dictionary<uint, uint>();
            physicalToVirtualMap = new Dictionary<uint, uint>();
        }

        public (bool hit, uint physicalPage) AccessPage(uint virtualPageNumber)
        {
            // Check for a hit
            if (virtualToPhysicalMap.TryGetValue(virtualPageNumber, out var physicalPage))
            {
                return (true, physicalPage);
            }

            // Miss: If memory is full, we need to evict the oldest page
            if (fifoQueue.Count >= physicalPageCount)
            {
                var oldestVirtualPage = fifoQueue.Dequeue();
                var oldestPhysicalPage = virtualToPhysicalMap[oldestVirtualPage];

                // Remove the oldest mapping
                virtualToPhysicalMap.Remove(oldestVirtualPage);
                physicalToVirtualMap.Remove(oldestPhysicalPage);

                // Reuse the physical page for the new virtual page
                virtualToPhysicalMap[virtualPageNumber] = oldestPhysicalPage;
                physicalToVirtualMap[oldestPhysicalPage] = virtualPageNumber;

                fifoQueue.Enqueue(virtualPageNumber);
                return (false, oldestPhysicalPage);
            }
            else
            {
                // If there's still room, find the next available physical page number
                uint newPhysicalPage = 0;
                while (physicalToVirtualMap.ContainsKey(newPhysicalPage))
                {
                    newPhysicalPage++;
                }

                // Update mappings
                virtualToPhysicalMap[virtualPageNumber] = newPhysicalPage;
                physicalToVirtualMap[newPhysicalPage] = virtualPageNumber;
                fifoQueue.Enqueue(virtualPageNumber);

                return (false, newPhysicalPage);  // Miss
            }
        }
    }
}
