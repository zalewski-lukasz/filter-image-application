using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageFilter.ErrorDiffusionFilters
{
    public abstract class ErrorDiffusionAlgorithm
    {
        public Bitmap Apply(Bitmap Original)
        {
            Bitmap tmp = GetImageWithPadding(Original);
            Bitmap newImage = new Bitmap(Original.Width, Original.Height);

            for (int i = 0; i < Original.Height; i++)
                {
                for (int j = Width / 2; j < Original.Width + Width / 2; j++)
                {
                    Color originalPixel = tmp.GetPixel(j, i);
                    Color K = GetNewPixel(originalPixel);
                    tmp.SetPixel(j, i, K);
                    Color errorPixel = GetErrorPixel(originalPixel, K);

                    int y = 0, f_y = 0, f_x = 0, x = 0;
                    try
                    {
                        for ( x = j + 1, f_x = Width / 2 + 1, f_y = Heigth / 2; x < j + Width / 2 - 1; x++, f_x++)
                        {
                            Color rightPixel = tmp.GetPixel(x, i);
                            tmp.SetPixel(x, i, GetPixelAfterErrorCorrection(rightPixel, errorPixel, Weigths[f_x, f_y]));
                        }
                    } 
                    catch
                    {
                        MessageBox.Show("Error when calculating pixels to the right!");
                        return null;
                    }

                    try
                    {
                        for (y = i + 1, f_y = Heigth / 2 + 1; f_y < Heigth; y++, f_y++)
                        {
                            for ( f_x = 0; f_x < Width; f_x++)
                            {
                                Color bottom = bottom = tmp.GetPixel(j + f_x - Width / 2, y);
                                try
                                {
                                    tmp.SetPixel(j + f_x - Width / 2, y, GetPixelAfterErrorCorrection(bottom, errorPixel, Weigths[f_y, f_x]));
                                }
                                catch
                                {
                                    MessageBox.Show($"Error when trying to set pixel!");
                                    return null;
                                }  
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show($"Error when calculating pixels down from the original!");
                        return null;
                    }
                }
            }

            for(int x = 0; x < newImage.Width; x++)
            {
                for(int y = 0; y < newImage.Height; y++)
                {
                    newImage.SetPixel(x, y, tmp.GetPixel(x + Width /2, y));
                }
            }

            return newImage;
        }

        private Bitmap GetImageWithPadding(Bitmap Original)
        {
            Bitmap tmp = new Bitmap(Original.Width + Width - 1, Original.Height + Heigth / 2);

            try
            {
                for (int x = 0; x < tmp.Width; x++)
                {
                    for (int y = 0; y < tmp.Height; y++)
                    {
                        tmp.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    }
                }

                for (int x = 0; x < Original.Width; x++)
                {
                    for (int y = 0; y < Original.Height; y++)
                    {
                        tmp.SetPixel(x + Width /2, y, Original.GetPixel(x, y));
                    }
                }
            }
            catch
            {
                MessageBox.Show("Can't create an image with padding!");
            }

            return tmp;
        }

        protected ErrorDiffusionAlgorithm(int height, int width, uint k)
        {
            Width = width;
            Heigth = height;
            KR = k;
            KG = k;
            KB = k;
            Weigths = new double[Heigth, Width];
            for (int i = 0; i < Heigth; i++)
                for (int j = 0; j < Width; j++)
                    Weigths[i, j] = 0;
        }

        protected double[,] Weigths;
        protected int Width { get; set; }
        protected int Heigth { get; set; }
        public uint KR { get; set; }
        public uint KG { get; set; }
        public uint KB { get; set; }

        protected int GetCorrectColor(int val, uint k)
        {
            if(k == 2)
            {
                if (val > 127) return 255;
                else return 0;
            }
            int step = (int)(255 / (k - 1));
            for (int i = 0; i < k - 1; i++)
            {
                if (Math.Abs(val - i * step) <= step / 2)
                    return i * step;
                if (Math.Abs(val - (i + 1) * step) <= step / 2)
                    return (i + 1) * step;
            } 
            return 255;
        }

        protected Color GetNewPixel(Color OldPixel)
        {
            int R = GetCorrectColor(OldPixel.R, KR);
            int G = GetCorrectColor(OldPixel.G, KG);
            int B = GetCorrectColor(OldPixel.B, KB);
            return Color.FromArgb(R, G, B);   
        }

        protected Color GetErrorPixel(Color OldPixel, Color NewPixel)
        {
            int R = OldPixel.R - NewPixel.R;
            int G = OldPixel.G - NewPixel.G;
            int B = OldPixel.B - NewPixel.B;

            if (R > 255) R = 255; else if (R < 0) R = 0;
            if (G > 255) G = 255; else if (G < 0) G = 0;
            if (B > 255) B = 255; else if (B < 0) B = 0;

            return Color.FromArgb(R, G, B);
        }

        protected Color GetPixelAfterErrorCorrection(Color OriginalPixel, Color ErrorPixel, double Filter)
        {
            int R = OriginalPixel.R;
            int G = OriginalPixel.G;
            int B = OriginalPixel.B;

            R += (int)(ErrorPixel.R * Filter);
            G += (int)(ErrorPixel.G * Filter);
            B += (int)(ErrorPixel.B * Filter);

            if (R > 255) R = 255; else if (R < 0) R = 0;
            if (G > 255) G = 255; else if (G < 0) G = 0;
            if (B > 255) B = 255; else if (B < 0) B = 0;

            return Color.FromArgb(R, G, B);
        }
    }
}
