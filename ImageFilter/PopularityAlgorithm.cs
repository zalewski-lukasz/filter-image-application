using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageFilter
{
    public class ColorCount
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int Count { set; get; }

        public ColorCount(int r, int g, int b, int count)
        {
            R = r;
            G = g;
            B = b;
            Count = count;
        }
    }

    public class PopularityAlgorithm
    {
        public int K { get; set; }

        public int[ , ,] ColorSpace;
        
        List<ColorCount> MostPopularColors;

        public PopularityAlgorithm(int k = 8)
        {
            K = k;
            ColorSpace = new int[256, 256, 256];
            NullifySpace();
            MostPopularColors = new List<ColorCount>();
        }

        public Bitmap Apply(Bitmap Original)
        {
            Bitmap newImage = new Bitmap(Original);

            try
            {
                for (int x = 0; x < Original.Width; x++)
                {
                    for (int y = 0; y < Original.Height; y++)
                    {
                        AddColor(Original.GetPixel(x, y));
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error when getting colors to color space!");
            }

            try
            {
                PrepareList();
            }
            catch
            {
                MessageBox.Show("Error when managing list of most popular colors!");
            }

            for (int x = 0; x < Original.Width; x++)
            {
                for (int y = 0; y < Original.Height; y++)
                {
                    Color pixel = newImage.GetPixel(x, y);
                    double distance = ColorDifference(pixel, Color.FromArgb(MostPopularColors[0].R, MostPopularColors[0].G, MostPopularColors[0].B));
                    int index = 0;
                    try
                    {
                        for (int i = 0; i < MostPopularColors.Count; i++)
                        {
                            if (distance > ColorDifference(pixel, Color.FromArgb(MostPopularColors[i].R, MostPopularColors[i].G, MostPopularColors[i].B)))
                            {
                                index = i;
                                distance = ColorDifference(pixel, Color.FromArgb(MostPopularColors[i].R, MostPopularColors[i].G, MostPopularColors[i].B));
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error when trying to calculate distance between colors!");
                    }
                    
                    try
                    {
                        newImage.SetPixel(x, y, Color.FromArgb(MostPopularColors[index].R, MostPopularColors[index].G, MostPopularColors[index].B));
                    }
                    catch
                    {
                        MessageBox.Show("Error when trying to put in new pixels into the filtered image!");
                    }
                }
            }

            return newImage;
        }

        private void NullifySpace()
        {
            for(int i = 0; i < 256; i++)
            {
                for(int j = 0; j < 256; j++)
                {
                    for(int k = 0; k < 256; k++)
                    {
                        ColorSpace[i, j, k] = 0;
                    }
                }
            }
        }

        public void AddColor(Color clr)
        {
            ColorSpace[clr.R, clr.G, clr.B]++;
        }

        public void PrepareList()
        {
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    for (int k = 0; k < 256; k++)
                    {
                        if(ColorSpace[i, j, k] >0)
                        {
                            ColorCount tmp = new ColorCount(i, j, k, ColorSpace[i, j, k]);
                            MostPopularColors.Add(tmp);
                        }
                    }
                }
            }

            int len = MostPopularColors.Count;
            for(int i = 0; i < len; i++)
            {
                for(int j = 0; j < len - i - 1; j++)
                {
                    if (MostPopularColors[i].Count > MostPopularColors[i + 1].Count)
                    {
                        ColorCount clr = new ColorCount(MostPopularColors[i].R, MostPopularColors[i].G, MostPopularColors[i].B, MostPopularColors[i].Count);
                        MostPopularColors[i] = MostPopularColors[i + 1];
                        MostPopularColors[i + 1] = clr;
                    }
                }
            }

            for (int i = 0; i < len - K; i++)
                MostPopularColors.RemoveAt(0);
        }

        double ColorDifference(Color left, Color right)
        {
            return Math.Sqrt(Math.Pow(left.R - right.R, 2) + Math.Pow(left.G - right.G, 2) + Math.Pow(left.B - right.B, 2));
        }
    }
}
