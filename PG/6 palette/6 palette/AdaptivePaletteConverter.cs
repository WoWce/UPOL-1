using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace _6_palette
{
    class AdaptivePaletteConverter
    {
        private List<int[]> palette { get; set; }
        private Bitmap bmp;

        public AdaptivePaletteConverter(Bitmap bmp)
        {
            this.bmp = bmp;
            extractColors();
        }
        /*
         * Extracts colors from image and creates 1 bucket 
         */
        private void extractColors()
        {
            BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
               ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            List<int[]> bucket = new List<int[]>(); //bucket
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {

                    Color PixelColor = Color.FromArgb(System.Runtime.InteropServices.Marshal.ReadInt32(data.Scan0, (data.Stride * i) + (4 * j)));
                    
                    int[] bucketArr = new int[3];
                    bucketArr[0] = PixelColor.R;
                    bucketArr[1] = PixelColor.G;
                    bucketArr[2] = PixelColor.B;
                    bucket.Add(bucketArr);
                }
            }
            bmp.UnlockBits(data);
            palette = fillTable(bucket, 1);
        }

        //uses median-cut algorithm
        private List<int[]> fillTable(List<int[]> bucket, int bucketsNum)
        {
            if (bucketsNum < 256)
            {
                
                List<int[]> firstBucket = new List<int[]>();
                List<int[]> secondBucket = new List<int[]>();
                int RMax = bucket[0][0];
                int RMin = bucket[0][0];
                int GMax = bucket[0][1];
                int GMin = bucket[0][1];
                int BMax = bucket[0][2];
                int BMin = bucket[0][2];
                //find widest range of RGB
                for (int i = 0; i < bucket.Count; i++)
                {
                   
                    if(RMax < bucket[i][0])
                    {
                        RMax = bucket[i][0];
                    }else if(RMin > bucket[i][0])
                    {
                        RMin = bucket[i][0];
                    }
                    if (GMax < bucket[i][1])
                    {
                        GMax = bucket[i][1];
                    }
                    else if (GMin > bucket[i][1])
                    {
                        GMin = bucket[i][1];
                    }
                    if (BMax < bucket[i][2])
                    {
                        BMax = bucket[i][2];
                    }
                    else if (BMin > bucket[i][2])
                    {
                        BMin = bucket[i][2];
                    }
                }
                int RRange = RMax - RMin;
                int GRange = GMax - GMin;
                int BRange = BMax - BMin;
                //code below could be reduced... oops
                if (RRange > GRange && RRange > BRange || RRange == 255)
                {
                    List<int[]> orderedBucket = bucket.OrderBy(o => o[0]).ToList();
                    List<int[]> bucketTable1 = orderedBucket.Take(orderedBucket.Count / 2).ToList();
                    List<int[]> bucketTable2 = orderedBucket.Skip(orderedBucket.Count - orderedBucket.Count / 2).Take(orderedBucket.Count).ToList();
                    firstBucket = fillTable(bucketTable1, bucketsNum * 2);
                    secondBucket = fillTable(bucketTable2, bucketsNum * 2);
                } else if(GRange > BRange)
                {
                    List<int[]> orderedBucket = bucket.OrderBy(o => o[1]).ToList();
                    List<int[]> bucketTable1 = orderedBucket.Take(orderedBucket.Count / 2).ToList();
                    List<int[]> bucketTable2 = orderedBucket.Skip(orderedBucket.Count - orderedBucket.Count / 2).Take(orderedBucket.Count).ToList();
                    firstBucket = fillTable(bucketTable1, bucketsNum * 2);
                    secondBucket = fillTable(bucketTable2, bucketsNum * 2);
                }
                else if(BRange > GRange)
                {
                    List<int[]> orderedBucket = bucket.OrderBy(o => o[2]).ToList();
                    List<int[]> bucketTable1 = orderedBucket.Take(orderedBucket.Count / 2).ToList();
                    List<int[]> bucketTable2 = orderedBucket.Skip(orderedBucket.Count - orderedBucket.Count / 2).Take(orderedBucket.Count).ToList();
                    firstBucket = fillTable(bucketTable1, bucketsNum * 2);
                    secondBucket = fillTable(bucketTable2, bucketsNum * 2);
                }
                /*
                 * sometimes happens that half of buckets have all equal ranges and here starts problem of loosing colors
                 * I solved it using random
                 */
                else
                {
                    Random rnd = new Random();
                    int colorNumber = rnd.Next(0, 2);
                    List<int[]> orderedBucket = bucket.OrderBy(o => o[colorNumber]).ToList();
                    List<int[]> bucketTable1 = orderedBucket.Take(orderedBucket.Count / 2).ToList();
                    List<int[]> bucketTable2 = orderedBucket.Skip(orderedBucket.Count - orderedBucket.Count / 2).Take(orderedBucket.Count).ToList();
                    firstBucket = fillTable(bucketTable1, bucketsNum * 2);
                    secondBucket = fillTable(bucketTable2, bucketsNum * 2);
                }
                
                firstBucket.AddRange(secondBucket); //merges buckets
                
                return firstBucket;
            }
            else
            {
                //when there is already 256 buckets and if  every bucket contains more colors it finds median
                int[] median = new int[3];
                median[0] = median[1] = median[2] = 0;
                foreach(var a in bucket)
                {
                    median[0] += a[0];
                    median[1] += a[1];
                    median[2] += a[2];
                }
                median[0] /= bucket.Count;
                median[1] /= bucket.Count;
                median[2] /= bucket.Count;
                List<int[]> final = new List<int[]>();
                final.Add(median);
                return final;
            } 
        }
        /*
         * Parces image due to created palette 
         */
        private byte[] parceImageToByteArray()
        {
            BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            List<byte> imageData = new List<byte>();
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color PixelColor = Color.FromArgb(System.Runtime.InteropServices.Marshal.ReadInt32(data.Scan0, (data.Stride * i) + (4 * j)));
                    int R = PixelColor.R;
                    int G = PixelColor.G;
                    int B = PixelColor.B;
                    int bestIndex = 0;
                    int currentDst = 300;
                    for (int k = 0; k < palette.Count; k++)
                    {
                        int clrDist = Math.Abs(R - palette[k][0]) + Math.Abs(G - palette[k][1]) + Math.Abs(B - palette[k][2]);
                        if (clrDist < currentDst) { currentDst = clrDist; bestIndex = k; }
                    }
                    imageData.AddRange(BitConverter.GetBytes(bestIndex));
                }
            }
            bmp.UnlockBits(data);
            return imageData.ToArray();
        }
        /*
         * Creates byte array from image data and palette
         */
        public byte[] GetImageData()
        {
            List<byte> image = new List<byte>();
            image.AddRange(BitConverter.GetBytes(bmp.Width));
            image.AddRange(BitConverter.GetBytes(bmp.Height));
            Console.WriteLine(palette.Count);
            image.AddRange(BitConverter.GetBytes(paletteToByteArray(palette).Length));
            image.AddRange(paletteToByteArray(palette));
            image.AddRange(parceImageToByteArray());
            
            return image.ToArray();
        }
        //Converts palette to byte array
        private byte[] paletteToByteArray(List<int[]> list)
        {
            List<byte> array = new List<byte>();
            foreach(int[] v in list)
            {
                array.AddRange(BitConverter.GetBytes(v[0]));
                array.AddRange(BitConverter.GetBytes(v[1]));
                array.AddRange(BitConverter.GetBytes(v[2]));
            }
            Console.WriteLine(array.Count);
            return array.ToArray();
        }


    }
}
