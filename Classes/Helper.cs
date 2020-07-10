﻿using MemoryLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackOpsGSCInjector
{
    public static class Helper
    {
        /// <summary>
        /// Read string from string pointer
        /// </summary>
        /// <param name="pointerToStringAddr">Pointer to the string</param>
        /// <returns>String stored at pointer</returns>
        public static string ReadStringPointer(int pointerToStringAddr)
        {
            if (string.IsNullOrEmpty(Manager.obj.process))
            {
                throw new Exception("ProcessName not set");
            }

            int stringAddr = Manager.obj.memory.Extension.ReadInt(pointerToStringAddr);

            return Manager.obj.memory.Extension.ReadString(stringAddr);
        }


        /// <summary>
        /// Returns an int from a specific position in a byte array
        /// </summary>
        /// <param name="arr">Array to return int from </param>
        /// <param name="position">Position in the array to start reading</param>
        /// <returns>int stored at the specified position</returns>
        public static int ReadIntFromByteArray(byte[] arr, int position)
        {
            byte[] buffer = new byte[4];

            Buffer.BlockCopy(arr, position, buffer, 0, 4);

            return BitConverter.ToInt32(buffer, 0);
        }
    }
}
