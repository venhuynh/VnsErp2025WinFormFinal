using System;

namespace Dal.Test
{
    /// <summary>
    /// Simple entry point for Phase 1 testing
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Run the Phase 1 test program
            Phase1TestProgram.Main(args).GetAwaiter().GetResult();
        }
    }
}
