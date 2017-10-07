using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM_to_COM
{
    class ByteStuffing
    {
        private const byte Flag = 0x7E;
        private const byte Esc = 0x7D;
        private const byte FlagCh = 0x5E;

        public static byte[] Decode(byte[] data)
        {
            if (!SendValidation(data)) return null;

            int dataLength = data.Length;
            int count = 0;
            for (int i = 0; i < dataLength; i++)
                if (data[i] == Esc)
                {
                    count++;
                    if (data[i + 1] == Esc)
                        i++;
                }

            int newDataLength = dataLength - 4 - count;
            byte[] newData = new byte[newDataLength];
            int j = 0;
            for (int i = 3; i < dataLength - 1; i++)
            {
                if (data[i] == Esc)
                {
                    if (data[++i] == FlagCh)
                        newData[j++] = Flag;
                    else
                        newData[j++] = Esc;
                }
                else
                {
                    newData[j++] = data[i];
                }
            }
            return newData;
        }

        public static byte[] Encode(byte[] data)
        {
            int newDataLength = GetNewDataLength(data);
            byte[] newData = new byte[newDataLength];
            InitializeBytes(ref newData);

            int j = 3;
            for (int i = 0; i < data.Length; i++)
            {
                switch (data[i])
                {
                    case Flag:
                        newData[j++] = Esc;
                        newData[j++] = FlagCh;
                        break;
                    case Esc:
                        newData[j++] = Esc;
                        newData[j++] = Esc;
                        break;
                    default:
                        newData[j++] = data[i];
                        break;
                }
            }
            return newData;
        }

        private static bool SendValidation(byte[] data)
        {
            return data[1] == 0xFF && data[2] == 0x00;
        }

        private static void InitializeBytes(ref byte[] data)
        {
            data[0] = Flag;
            data[1] = 0xFF;
            data[2] = 0x00;
            data[data.Length - 1] = Flag;
        }

        private static int GetNewDataLength(byte[] data)
        {
            int count = 0;
            for (int i = 0; i < data.Length; i++)
                if ((data[i] == Flag) || (data[i] == Esc))
                    count++;
            return data.Length + count + 4;
        }

    }
}
