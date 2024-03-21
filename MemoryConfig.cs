
namespace MHS_Project
{
    internal class MemoryConfig
    {
        // Basic memory hierarchy configurations
        public int NumberOfSets { get; set; }
        public int SetSize { get; set; }
        public int PageSize { get; set; }
        public int NumberOfVirtualPages { get; set; }
        public int NumberOfPhysicalPages { get; set; }
        public bool VirtualAddresses { get; set; }

        // Extended configurations for caches and TLB
        public int DataCacheSets { get; set; }
        public int DataCacheSetSize { get; set; }
        public int DataCacheLineSize { get; set; }
        public bool TLBEnabled { get; set; }
        public int TLBSets { get; set; }
        public int TLBSetSize { get; set; }

        //Default Contructor with hardcoded assignment
        public MemoryConfig()
        {
            NumberOfSets = 2;
            SetSize = 1;
            PageSize = 256;
            NumberOfVirtualPages = 64;
            NumberOfPhysicalPages = 4;
            VirtualAddresses = true;
            TLBEnabled = true;
            TLBSets = 2;
            TLBSetSize = 4;
            DataCacheSets = 4;
            DataCacheSetSize = 1;
            DataCacheLineSize = 16;
        }


        public static void DisplayMemoryConfig(MemoryConfig config)
        {
            Console.WriteLine("Memory Hierarchy Configuration:");
            Console.WriteLine($"Data TLB configuration (Enabled: {(config.TLBEnabled ? "Yes" : "No")})");
            if (config.TLBEnabled)
            {
                Console.WriteLine($"- Number of TLB sets: {config.TLBSets}");
                Console.WriteLine($"- TLB set size: {config.TLBSetSize}");
            }
            Console.WriteLine($"Page Table Configuration:");
            Console.WriteLine($"- Number of virtual pages: {config.NumberOfVirtualPages}");
            Console.WriteLine($"- Number of physical pages: {config.NumberOfPhysicalPages}");
            Console.WriteLine($"- Page size: {config.PageSize} bytes");
            Console.WriteLine($"- Virtual addresses: {(config.VirtualAddresses ? "Enabled" : "Disabled")}");
            Console.WriteLine($"Data Cache Configuration:");
            Console.WriteLine($"- Number of sets: {config.DataCacheSets}");
            Console.WriteLine($"- Set size: {config.DataCacheSetSize}");
            Console.WriteLine($"- Line size: {config.DataCacheLineSize} bytes\n\n");
            // More memory config assigments?
        }

    }
}
