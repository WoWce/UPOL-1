using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace _6_palette
{
    /*
     * Class for converting adaptive-palette binary file to bitmap
     * 
     */
    class AdaptivePaletteReader
    {
        List<int[]> palette = new List<int[]>();
        public Bitmap convertDataToBitmap(byte[] imgData)
        {

            Stream stream = new MemoryStream(imgData);
            byte[] buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            int Width = BitConverter.ToInt32(buffer, 0);
            stream.Read(buffer, 0, 4);
            int Height = BitConverter.ToInt32(buffer, 0);
            stream.Read(buffer, 0, 4);
            int PaletteLength = BitConverter.ToInt32(buffer, 0);
            byte[] paletteBuf = new byte[PaletteLength];
            stream.Read(paletteBuf, 0, PaletteLength);
            palette = byteArrayToPalette(paletteBuf);
            Console.WriteLine(PaletteLength);
            Bitmap bmp = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            byte[] buffer2 = new byte[4];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    stream.Read(buffer2, 0, 4);
                    int tableIndex = BitConverter.ToInt32(buffer2, 0);
                    int R = palette[tableIndex][0];
                    int G = palette[tableIndex][1];
                    int B = palette[tableIndex][2];
                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                }
            }
            stream.Close();
            return bmp;
        }

        private List<int[]> byteArrayToPalette(byte[] array)
        {
            List<int[]> list = new List<int[]>();
            Stream stream = new MemoryStream(array);
            byte[] buffer = new byte[4];
            for (int i = 0; i < array.Length/3; i++)
            {
                int[] tmp = new int[3];
                stream.Read(buffer, 0, 4);
                tmp[0] = BitConverter.ToInt32(buffer, 0);
                stream.Read(buffer, 0, 4);
                tmp[1] = BitConverter.ToInt32(buffer, 0);
                stream.Read(buffer, 0, 4);
                tmp[2] = BitConverter.ToInt32(buffer, 0);
                list.Add(tmp);
            }
            return list;
        }
    }
}
