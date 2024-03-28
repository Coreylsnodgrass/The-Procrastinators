using System;
using System.Collections.Generic;
using System.IO; 

namespace MHS_Project
{
    internal class MemoryReferenceHandler
    {
        public const uint PageSize = 256;

        
        public static List<MemoryReference> ReadMemoryReferencesFromInput()
        {
            List<string> rawReferences = new List<string>();
            Console.WriteLine("Enter file path for memory references ('end' to finish):");

            while (true)
            {
                string input = Console.ReadLine();
                if (input.ToLower() == "end")
                {
                    break;
                }
                rawReferences.Add(input);
            }

            return ParseMemoryReferencesFromFile(rawReferences[0]);
            //return ParseMemoryReferencesFromList(rawReferences);
        }

        public static List<MemoryReference> ParseMemoryReferencesFromList(List<string> rawReferences)
        {
            List<MemoryReference> references = new List<MemoryReference>();
            foreach (var line in rawReferences)
            {
                var parts = line.Split(':');
                if (parts.Length != 2) continue; // Skip invalid lines

                char accessType = parts[0][0];
                if (accessType != 'R' && accessType != 'W') continue; // Skip if not 'R' or 'W'

                uint address = Convert.ToUInt32(parts[1], 16);
                uint virtualPageNumber = address / PageSize;
                uint offset = address % PageSize;

                references.Add(new MemoryReference
                {
                    AccessType = accessType,
                    Address = address,
                    VirtualPageNumber = virtualPageNumber,
                    Offset = offset
                });
            }
            return references;
        }

        //Parses memory references directly from file -- NOT REQUIRED**** Use STD IN
        public static List<MemoryReference> ParseMemoryReferencesFromFile(string filePath)
        {
            List<MemoryReference> references = new List<MemoryReference>();

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return references; // Return an empty list or handle the error as appropriate
            }

            try
            {
                // Open the file and read line by line
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(':');
                        if (parts.Length != 2) continue; // Skip invalid lines

                        char accessType = parts[0][0];
                        if (accessType != 'R' && accessType != 'W') continue; // Skip if not 'R' or 'W'

                        uint address = Convert.ToUInt32(parts[1], 16);
                        uint virtualPageNumber = address / MemoryReference.PageSize;
                        uint offset = address % MemoryReference.PageSize;

                        references.Add(new MemoryReference
                        {
                            AccessType = accessType,
                            Address = address,
                            VirtualPageNumber = virtualPageNumber,
                            Offset = offset
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle potential exceptions (e.g., file reading errors)
                Console.WriteLine($"Error reading file: {ex.Message}");
            }

            return references;
        }

        public static void DisplayMemoryReferenceFromList(List<MemoryReference> memRef)
        {
            foreach (MemoryReference reference in memRef)
            {
                Console.WriteLine($"{reference.AccessType}  Address: {reference.Address:X}, VP: {reference.VirtualPageNumber}, Offset: {reference.Offset}");
            }
        }
    }
}
