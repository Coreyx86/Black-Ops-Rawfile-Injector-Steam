using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackOpsGSCInjector
{
	//Shouts out to the research provided by this wiki https://wiki.orbismodding.com/index.php/Rawfile_Asset
	public class Rawfile
    {
		/*
		 *	struct Rawfile_t {
		 *		const char * name;
		 *		int length;
		 *		const char * buffer;
		 *	};
		 */ 
		public int NamePointer = 0;

		public int Length = 0;

		public int BufferPointer = 0;


		public string Name = "";
		public byte[] Buffer;

		public bool Overwrite = false;

		public int index;

		private int location = 0;

		
		public Rawfile(int rawfilePtr = 0)
		{
			this.location = rawfilePtr;
		}
	}
}
