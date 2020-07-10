using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryLib;

namespace BlackOpsGSCInjector
{
    public class Manager
    {
        private static readonly Lazy<Manager> mgr = new Lazy<Manager>(() => new Manager());

        public static Manager obj { get { return mgr.Value; } }

        public Memory memory { get; set; }

        private string _process = string.Empty;
        public string process
        {
            get
            {
                return _process;
            }

            set
            {
                if (memory != null)
                {
                    memory.ProcessName = value;
                }

                _process = value;
            }
        }

        public RawPool rawpool;

        public Project project { get; set; }

        private Manager()
        {
            memory = new Memory(string.Empty);
        }
    }
}
