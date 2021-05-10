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
    public class ConvolutionalFilters
    {
        private static int Truncate(int value)
        {
            if (value > 255)
                return 255;
            if (value < 0)
                return 0;
            return value;
        }

        public static Bitmap ImageWithPadding(Bitmap image, Kernel kernel)
        {
            Bitmap tmp = new Bitmap(image.Width + kernel.Width - 1, image.Height + kernel.Heigth - 1);
            for (int x = 0; x < tmp.Width; x++)
            {
                for(int y = 0; y < tmp.Height; y++)
                {
                    tmp.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                }
            }
            for (int x = 0; x < image.Width; x++)
            {
                for(int y = 0; y < image.Height; y++)
                {
                    try
                    {
                        tmp.SetPixel(x + kernel.Anchor.X, y + kernel.Anchor.Y, image.GetPixel(x, y));
                    }
                    catch
                    {
                        MessageBox.Show($"Error occured when creating matrix with padding. Coordinates: x: {x + kernel.Anchor.X} y: {y + kernel.Anchor.Y}");
                    }
                }
            }
            return tmp;
        }

        public static Bitmap Filter(Bitmap image, Kernel kernel)
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height);
            Bitmap padded = ImageWithPadding(image, kernel);

            for(int x = 0; x < newImage.Width; x++)
            {
                for(int y = 0; y < newImage.Height; y++)
                {
                    int newR = 0;
                    int newG = 0;
                    int newB = 0;
                    for (int k = 0; k < kernel.Width; k++)
                    {
                        for(int l = 0; l < kernel.Heigth; l++)
                        {
                            try
                            {
                                newR += (kernel.Matrix[k, l] * padded.GetPixel(x + k, y + l).R);
                                newG += (kernel.Matrix[k, l] * padded.GetPixel(x + k, y + l).G);
                                newB += (kernel.Matrix[k, l] * padded.GetPixel(x + k, y + l).B);
                            }
                            catch
                            {
                                MessageBox.Show($"Error occured when applying convolution filter. Coordinates: x:{x + k} y:{y + l}");
                            }
                        }
                    }
                    newR = (byte)Truncate(kernel.Offset + newR / kernel.Divisor);
                    newG = (byte)Truncate(kernel.Offset + newG / kernel.Divisor);
                    newB = (byte)Truncate(kernel.Offset + newB / kernel.Divisor);
                    try
                    {
                        newImage.SetPixel(x, y, Color.FromArgb(newR, newG, newB));
                    }
                    catch
                    {
                        MessageBox.Show($"Error occured when setting new pixels after filter application. Coordinates: x:{x} y: {y}");
                    }
                }
            }

            return newImage;
        }

        public static Kernel FindKernelByName(List <Kernel> kernels, string name)
        {
            foreach(var kernel in kernels)
            {
                if (kernel.Name == name)
                    return kernel;
            }
            return null;
        }

    }
}
