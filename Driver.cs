namespace MHS_Project
{
    internal class Driver
    {
        //Testing grounds
        // To run the simulation, read memory references from a file, and output the results of the simulation to the console or a file.
        static void Main(string[] args)
        {
            var config = new MemoryConfig();

            //string filePath = @"..\..\..\misc\real_tr.dat";
            //var rawReferences = MemoryReferenceHandler.ParseMemoryReferencesFromFile(filePath);
            //MemoryReferenceHandler.DisplayMemoryReferenceFromList(rawReferences);


            MemoryConfig.DisplayMemoryConfig(config);
            var listOfRawReferences = MemoryReferenceHandler.ReadMemoryReferencesFromInput();

            //After being prompted for memory traces, dont forget to type 'end' like the prompt references
            MemoryReferenceHandler.DisplayMemoryReferenceFromList(listOfRawReferences);
            Console.ReadLine();
        }
    }
}
