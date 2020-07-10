using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zlib;

namespace BlackOpsGSCInjector
{
    public class Project
    {
        public string folder { get; set; }

        public bool ProjectLoaded;

        public Project()
        {

        }

        public Project(string folder)
        {
            this.folder = folder;
        }

        private int indexOfArray(string name)
        {
            if(Manager.obj.rawpool == null)
            {
                return -1;
            }

            for (int i = 0; i < Manager.obj.rawpool.Rawfiles.Count; i++)
            {
                if ( Manager.obj.memory.Extension.ReadString(Manager.obj.rawpool.Rawfiles[i].NamePointer) == name)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Loads the project folder and compresses GSC and CSC files.
        /// </summary>
        /// <returns>void</returns>
        public void Load()
        {
            //If rawpool is null throw exception if not, call RawPool.InitData to read the Rawfile asset pool data.
            if(Manager.obj.rawpool == null)
            {
                throw new Exception("Rawpool is null");
            }
            else
            {
                Manager.obj.rawpool.InitData();
            }

            string path = folder;

            //Parse the project folder and for gamescript and clientscript files compress the files using default zlib.
            foreach (string str in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
            {
                string fileName = str.Substring(path.Length + 1).Replace('\\', '/');

                int len = (int)new FileInfo(str).Length + 1;

                byte[] buffer = new byte[len];

                Buffer.BlockCopy(File.ReadAllBytes(str), 0, buffer, 0, (int)len - 1);

                if (fileName.EndsWith(".gsc") || fileName.EndsWith(".csc"))
                {
                    int DecompressedLength = len;

                    byte[] comp = ZlibStream.CompressBuffer(buffer);
                    int CompressedLength = comp.Length;
                    byte[] script_header = new byte[CompressedLength + 8];

                    Buffer.BlockCopy(BitConverter.GetBytes(DecompressedLength), 0, script_header, 0, 4);
                    Buffer.BlockCopy(BitConverter.GetBytes(CompressedLength), 0, script_header, 4, 4);
                    Buffer.BlockCopy(comp, 0, script_header, 8, (int)CompressedLength);

                    buffer = script_header;
                    len = CompressedLength + 8;
                }

                //Find if the rawfile exists in the asset pool and if so, overwrite it.
                int file_index = indexOfArray(fileName);
                if (file_index >= 0)
                {
                    Manager.obj.rawpool.Rawfiles[file_index].Overwrite = true;
                    Manager.obj.rawpool.Rawfiles[file_index].Buffer = buffer;
                    Manager.obj.rawpool.Rawfiles[file_index].Length = len;
                }
            }

            ProjectLoaded = true;
        }
    }
}
