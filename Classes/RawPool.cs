using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackOpsGSCInjector
{
    public class RawPool
    {
        private int XAssetPoolAddr = 0;

        private byte[] XAssetPoolDataBuffer;

        public List<int> EmptyIndices;
        public List<Rawfile> Rawfiles;
        public int MaxIndex = 0;


        private const int XAssetSize = 0xC; //Rawfile struct size. 
        private const int XAssetPoolMax = 0x400;

        private const int FREE_SPACE = 0x2000000;
        private int CURRENT_POSITION = 0;

        public RawPool(int XAssetPoolAddr = 0)
        {
            this.XAssetPoolAddr = XAssetPoolAddr;

            EmptyIndices = new List<int>();
            Rawfiles = new List<Rawfile>();
        }

        /// <summary>
        /// Reads the Rawfile asset pool buffer
        /// </summary>
        public void ReadPoolData()
        {
            this.XAssetPoolDataBuffer = Manager.obj.memory.Extension.ReadBytes(this.XAssetPoolAddr, (XAssetPoolMax * XAssetSize) + 4);
        }

        /// <summary>
        /// Parses the rawfile asset pool and finds the empty entries in the asset pool
        /// </summary>
        public void ReadEmptyIndices()
        {
            if(XAssetPoolDataBuffer == null)
            {
                throw new Exception("XAssetPoolDataBuffer is null");
            }

            EmptyIndices.Clear();

            int first = Helper.ReadIntFromByteArray(XAssetPoolDataBuffer, 0);

            int address = (first - XAssetPoolAddr);

            int unusedAddress = 0;

            for(int i = 0; i < XAssetPoolMax; i++)
            {
                if(first == 0)
                {
                    break;
                }

                unusedAddress = Helper.ReadIntFromByteArray(XAssetPoolDataBuffer, address);

                if(unusedAddress == 0)
                {
                    break;
                }

                address = (unusedAddress - XAssetPoolAddr);

                EmptyIndices.Add(address / XAssetSize);
            }
        }

        /// <summary>
        /// Parse the asset pool to find the existing rawfiles
        /// </summary>
        public void ReadRawfiles()
        {
            Rawfiles.Clear();

            for(int i = 0; i < XAssetPoolMax; i++)
            {
                if(EmptyIndices.IndexOf(i) >= 0)
                {
                    continue;
                }

                Rawfile rawfile = new Rawfile();
                rawfile.index = i;
                rawfile.NamePointer = Helper.ReadIntFromByteArray(XAssetPoolDataBuffer, (i * XAssetSize) + 4);
                rawfile.Length = Helper.ReadIntFromByteArray(XAssetPoolDataBuffer, (i * XAssetSize) + 8);
                rawfile.BufferPointer = Helper.ReadIntFromByteArray(XAssetPoolDataBuffer, (i * XAssetSize) + 12);

                rawfile.Name = Manager.obj.memory.Extension.ReadString(rawfile.NamePointer);

                Rawfiles.Add(rawfile);
            }
        }

        /// <summary>
        /// Loop through each rawfile and if it's marked for overwriting, overwrite it.
        /// </summary>
        /// <returns></returns>
        public bool OverwriteRawfiles()
        {
            Manager.obj.rawpool.Rawfiles.ForEach((rawfile) =>
            {
                if (rawfile.Overwrite)
                {
                    Manager.obj.rawpool.WriteRawfile(rawfile);
                }
            });

            return true;
        }

        /// <summary>
        /// Writes the custom rawfile into our freespace buffer and modify the rawfile's length and buffer pointer
        /// </summary>
        /// <param name="rawfile"></param>
        private void WriteRawfile(Rawfile rawfile)
        {
            int index = rawfile.index;

            int write_addr = FREE_SPACE + CURRENT_POSITION;

            Manager.obj.memory.Extension.WriteBytes(write_addr, rawfile.Buffer);

            CURRENT_POSITION += rawfile.Length;

            rawfile.BufferPointer = write_addr;

            Manager.obj.memory.Extension.WriteInt32(XAssetPoolAddr + (index * XAssetSize) + 12, rawfile.BufferPointer);
            Manager.obj.memory.Extension.WriteInt32(XAssetPoolAddr + (index * XAssetSize) + 8, rawfile.Length);
        }

        ///Wrapper function for parsing the rawfile data
        public void InitData()
        {
            ReadPoolData();
            ReadEmptyIndices();
            ReadRawfiles();
        }

        /// <summary>
        /// Dumps the existing rawfiles in memory to the specified path
        /// </summary>
        /// <param name="dumpLocationPath">Path to dump the rawfiles.</param>
        /// <returns>Total number of rawfiles dumped</returns>
        public int DumpRawfiles(string dumpLocationPath)
        {
            ReadPoolData();
            ReadEmptyIndices();
            ReadRawfiles();

            int count = 0;

            for (int i = 0; i < Rawfiles.Count - 1; i++)
            {
                Rawfile rawfile = Rawfiles[i];

                string name = Manager.obj.memory.Extension.ReadString(rawfile.NamePointer);

                if (!string.IsNullOrEmpty(name))
                {
                    byte[] buffer = Manager.obj.memory.Extension.ReadBytes(rawfile.BufferPointer, rawfile.Length);

                    if (name.Contains("/"))
                    {
                        name = name.Replace("/", @"\");

                        string[] tmp = name.Replace(@"\", "|").Split('|');
                        string dir = dumpLocationPath + @"\" + name.Replace(tmp[tmp.Length - 1], "");

                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                    }

                    string path = name.Replace("//", @"\").Replace("/", @"\");
                    File.WriteAllBytes(dumpLocationPath + @"\" + path, buffer);

                    count++;
                }
            }

            return count;
        }


    }
}
