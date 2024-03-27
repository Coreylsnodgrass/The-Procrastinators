using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHS_Project
{
    public interface IMemoryManager
    {
        /// <summary>
        /// Processes an access to a given virtual page, determining whether it results in a hit or a miss,
        /// and handles the page replacement if needed.
        /// </summary>
        /// <param name="virtualPageNumber">The virtual page number being accessed.</param>
        /// <returns>
        /// A tuple where:
        /// - The first item (bool) indicates whether the access was a hit (true) or a miss (false).
        /// - The second item (uint) represents the physical page number associated with the accessed virtual page.
        /// </returns>
        (bool hit, uint physicalPage) AccessPage(uint virtualPageNumber);
    }

}
