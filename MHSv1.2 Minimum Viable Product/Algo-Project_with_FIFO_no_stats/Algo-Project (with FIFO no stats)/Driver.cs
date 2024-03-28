using System.Security.Cryptography.X509Certificates;

namespace MHS_Project
{
    internal class Driver
    {
        static void Main(string[] args)
        {
            string policy;

            // Run simulation in a loop
            do
            {
                Console.WriteLine("\nChoose a memory management policy:");
                Console.WriteLine("1. FIFO");
                Console.WriteLine("2. NRU");
                Console.WriteLine("3. LRU");
                Console.WriteLine("4. Greedy");
                Console.WriteLine("Enter 'quit' to exit.");

                policy = Console.ReadLine();

                switch (policy)
                {
                    case "1":
                        RunFifoSimulation();
                        break;
                    case "2":
                        RunNruSimulation();
                        break;
                    case "3":
                        RunLRUSimulation();
                        break;
                    case "4":
                        RunGreedyOfflineSimulation();
                        break;
                    case "quit":
                        break;
                    default:
                        Console.WriteLine("Invalid selection, please try again.");
                        break;
                }

            } while (policy != "quit");

        }
        public static void RunFifoSimulation()
        {
            var config = new MemoryConfig();
            MemoryConfig.DisplayMemoryConfig(config);

            var memoryManager = new FIFOMemoryManager(config.NumberOfPhysicalPages);
            var listOfRawReferences = MemoryReferenceHandler.ReadMemoryReferencesFromInput();

            Console.WriteLine("Address    Virt. Page #  Offset  Result  Phys. Page #");

            int totalNumberOfReferences = 0;
            int totalNumberOfHits = 0;
            int totalReads = 0;
            int totalWrites = 0;

            foreach (var reference in listOfRawReferences)
            {
                totalNumberOfReferences++;

                if (reference.AccessType == 'R') totalReads++;
                else if (reference.AccessType == 'W') totalWrites++;

                var (hit, physicalPage) = memoryManager.AccessPage(reference.VirtualPageNumber);
                Console.WriteLine($"{reference.Address:X8}    {reference.VirtualPageNumber:X}           {reference.Offset:X}      {(hit ? "Hit " : "Miss")}    {physicalPage}");

                if (hit) { totalNumberOfHits++; }
            }


            // ... simulation loop with AccessPage calls

            // After the loop, calculate and print summary statistics
            double hitRatio = (double)totalNumberOfHits / totalNumberOfReferences;
            double readRatio = (double)totalReads / totalNumberOfReferences;
            double writeRatio = (double)totalWrites / totalNumberOfReferences;


            Console.WriteLine($"\nSummary Statistics:");
            Console.WriteLine($"    Total number of references:    {totalNumberOfReferences}");
            Console.WriteLine($"    Total number of reads:         {totalReads}");
            Console.WriteLine($"    Total number of writes:        {totalWrites}");
            Console.WriteLine($"    Read Ratio:                    {readRatio:F4}");
            Console.WriteLine($"    Write Ratio:                   {writeRatio:F4}");
            Console.WriteLine($"    Total number of hits:          {totalNumberOfHits}");
            Console.WriteLine($"    Total number of misses:        {(totalNumberOfReferences - totalNumberOfHits)}");
            Console.WriteLine($"    Hit Ratio:                     {hitRatio:F4}");
            Console.WriteLine($"    Miss Ratio:                    {1 - hitRatio:F4}");


            Console.ReadLine();
            Console.Clear();
        }

        public static void RunNruSimulation()
        {
            var config = new MemoryConfig();
            MemoryConfig.DisplayMemoryConfig(config);

            var memoryManager = new NRUMemoryManager(config.NumberOfPhysicalPages);
            var listOfRawReferences = MemoryReferenceHandler.ReadMemoryReferencesFromInput();

            Console.WriteLine("Address    Virt. Page #  Offset  Result  Phys. Page #");

            int totalNumberOfReferences = 0;
            int totalNumberOfHits = 0;
            int totalReads = 0;
            int totalWrites = 0;

            foreach (var reference in listOfRawReferences)
            {
                totalNumberOfReferences++;

                if (reference.AccessType == 'R') totalReads++;
                else if (reference.AccessType == 'W') totalWrites++;

                var (hit, physicalPage) = memoryManager.AccessPage(reference.VirtualPageNumber);
                Console.WriteLine($"{reference.Address:X8}    {reference.VirtualPageNumber:X}           {reference.Offset:X}      {(hit ? "Hit " : "Miss")}    {physicalPage}");

                if (hit) { totalNumberOfHits++; }
            }


            // ... simulation loop with AccessPage calls

            // After the loop, calculate and print summary statistics
            double hitRatio = (double)totalNumberOfHits / totalNumberOfReferences;
            double readRatio = (double)totalReads / totalNumberOfReferences;
            double writeRatio = (double)totalWrites / totalNumberOfReferences;

            Console.WriteLine($"\nSummary Statistics:");
            Console.WriteLine($"    Total number of reference:    {totalNumberOfReferences}");
            Console.WriteLine($"    Total number of reads:        {totalReads}");
            Console.WriteLine($"    Total number of writes:       {totalWrites}");
            Console.WriteLine($"    Read Ratio:                   {readRatio:F4}");
            Console.WriteLine($"    Write Ratio:                  {writeRatio:F4}");
            Console.WriteLine($"    Total number of hits:         {totalNumberOfHits}");
            Console.WriteLine($"    Total number of misses:       {(totalNumberOfReferences - totalNumberOfHits)}");
            Console.WriteLine($"    Hit Ratio:                    {hitRatio:F4}");
            Console.WriteLine($"    Miss Ratio:                   {1 - hitRatio:F4}");
            // Miss ratio
            // Runtime stat

            Console.ReadLine();
            Console.Clear();
        }

        public static void RunLRUSimulation()
        {
            var config = new MemoryConfig();
            MemoryConfig.DisplayMemoryConfig(config);

            var memoryManager = new LRUMemoryManager(config.NumberOfPhysicalPages);
            var listOfRawReferences = MemoryReferenceHandler.ReadMemoryReferencesFromInput();

            Console.WriteLine("Address    Virt. Page #  Offset  Result  Phys. Page #");

            int totalNumberOfReferences = 0;
            int totalNumberOfHits = 0;
            int totalReads = 0;
            int totalWrites = 0;

            foreach (var reference in listOfRawReferences)
            {
                totalNumberOfReferences++;

                if (reference.AccessType == 'R') totalReads++;
                else if (reference.AccessType == 'W') totalWrites++;

                var (hit, physicalPage) = memoryManager.AccessPage(reference.VirtualPageNumber);
                Console.WriteLine($"{reference.Address:X8}    {reference.VirtualPageNumber:X}           {reference.Offset:X}      {(hit ? "Hit " : "Miss")}    {physicalPage}");

                if (hit) { totalNumberOfHits++; }
            }


            // ... simulation loop with AccessPage calls

            // After the loop, calculate and print summary statistics
            double hitRatio = (double)totalNumberOfHits / totalNumberOfReferences;
            double readRatio = (double)totalReads / totalNumberOfReferences;
            double writeRatio = (double)totalWrites / totalNumberOfReferences;

            Console.WriteLine($"\nSummary Statistics:");
            Console.WriteLine($"    Total number of references:    {totalNumberOfReferences}");
            Console.WriteLine($"    Total number of reads:         {totalReads}");
            Console.WriteLine($"    Total number of writes:        {totalWrites}");
            Console.WriteLine($"    Read Ratio:                    {readRatio:F4}");
            Console.WriteLine($"    Write Ratio:                   {writeRatio:F4}");
            Console.WriteLine($"    Total number of hits:          {totalNumberOfHits}");
            Console.WriteLine($"    Total number of misses:        {(totalNumberOfReferences - totalNumberOfHits)}");
            Console.WriteLine($"    Hit Ratio:                     {hitRatio:F4}");
            Console.WriteLine($"    Miss Ratio:                    {1 - hitRatio:F4}");


            Console.ReadLine();
            Console.Clear();
        }

        public static void RunGreedyOfflineSimulation()
        {
            var config = new MemoryConfig();
            MemoryConfig.DisplayMemoryConfig(config);

            var listOfRawReferences = MemoryReferenceHandler.ReadMemoryReferencesFromInput();
            var memoryManager = new GreedyOfflineMemoryManager(config.NumberOfPhysicalPages, listOfRawReferences);
            

            Console.WriteLine("Address    Virt. Page #  Offset  Result  Phys. Page #");

            int totalNumberOfReferences = 0;
            int totalNumberOfHits = 0;
            int totalReads = 0;
            int totalWrites = 0;

            foreach (var reference in listOfRawReferences)
            {
                totalNumberOfReferences++;

                if (reference.AccessType == 'R') totalReads++;
                else if (reference.AccessType == 'W') totalWrites++;

                var (hit, physicalPage) = memoryManager.AccessPage(reference.VirtualPageNumber);
                Console.WriteLine($"{reference.Address:X8}    {reference.VirtualPageNumber:X}           {reference.Offset:X}      {(hit ? "Hit " : "Miss")}    {physicalPage}");

                if (hit) { totalNumberOfHits++; }
            }


            // ... simulation loop with AccessPage calls

            // After the loop, calculate and print summary statistics
            double hitRatio = (double)totalNumberOfHits / totalNumberOfReferences;
            double readRatio = (double)totalReads / totalNumberOfReferences;
            double writeRatio = (double)totalWrites / totalNumberOfReferences;

            Console.WriteLine($"\nSummary Statistics:");
            Console.WriteLine($"    Total number of references:    {totalNumberOfReferences}");
            Console.WriteLine($"    Total number of reads:         {totalReads}");
            Console.WriteLine($"    Total number of writes:        {totalWrites}");
            Console.WriteLine($"    Read Ratio:                    {readRatio:F4}");
            Console.WriteLine($"    Write Ratio:                   {writeRatio:F4}");
            Console.WriteLine($"    Total number of hits:          {totalNumberOfHits}");
            Console.WriteLine($"    Total number of misses:        {(totalNumberOfReferences - totalNumberOfHits)}");
            Console.WriteLine($"    Hit Ratio:                     {hitRatio:F4}");
            Console.WriteLine($"    Miss Ratio:                    {1 - hitRatio:F4}");


            Console.ReadLine();
            Console.Clear();
        }
    }

     
}
