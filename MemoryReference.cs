
namespace MHS_Project
{
    internal class MemoryReference
    {
        //Fields determined with intial parse
        public char AccessType { get; set; }
        public uint Address { get; set; }
        public uint VirtualPageNumber { get; set; }
        public uint Offset { get; set; }

        public static uint PageSize = 256;

        //Fields TBD by simulation
        public string TLBResult { get; set; }
        public string PTResult { get; set; }
        public uint PhysicalPageNumber { get; set; }
        public string DCResult { get; set; }
        public string L2Result { get; set; }
    }
}
