using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS_Project
{
    //To manage the Translating Lookaside Buffer (TLB) entries for the virtual memory system.
    //  This could include methods to add and remove TLB entries, as well as methods to look up an address in the TLB.
    internal class TLB
    {
            // A dictionary to store the entries
            private readonly Dictionary<int, string> _tlb = new Dictionary<int, string>();

            // Method to add an entry
            public void AddEntry(int key, string value)
            {
                // If the key already exists, replace the value
                if (_tlb.ContainsKey(key))
                {
                    _tlb[key] = value;
                }
                else
                {
                    // Otherwise, add the new entry
                    _tlb.Add(key, value);
                }
            }

            // Method to remove an entry by key
            public void RemoveEntry(int key)
            {
                if (_tlb.ContainsKey(key))
                {
                    _tlb.Remove(key);
                }
            }

            // Method to look up an entry by key
            public string LookupEntry(int key)
            {
                if (_tlb.ContainsKey(key))
                {
                    return _tlb[key];
                }

                return null;
            }

            // Method to display the contents of the TLB
            public void DisplayTLB()
            {
                Console.WriteLine("TLB:");

                foreach (var entry in _tlb)
                {
                    Console.WriteLine($"Key: {entry.Key}, Value: {entry.Value}");
                }
            }
    }
}
