using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageFilter
{
    public class Coordinates
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Kernel
    {
        public string Name { get; set; }
        public int[,] Matrix { get; set; }
        public int Heigth { get; set; }
        public int Width { get; set; }
        public Coordinates Anchor { get; set; }
        public int Offset { get; set; }
        public int Divisor { get; set; }

        public Kernel(int[,] matrix, string name, int heigth = 3, int width = 3, int x = 1, int y = 1)
        {
            Heigth = heigth;
            Width = width;
            Anchor = new Coordinates(x, y);
            Matrix = new int[9, 9];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigth; j++)
                    Matrix[i, j] = matrix[i, j];
            //Matrix = matrix;
            ComputeDivisor();
            Offset = 0;
            Name = name;
        }

        public Kernel(int[,] matrix, string name, Coordinates anchor, int heigth = 3, int width = 3)
        {
            Heigth = heigth;
            Width = width;
            Anchor = anchor;
            Matrix = new int[9, 9];
            /*for (int i = 0; i < width; i++)
                for (int j = 0; j < heigth; j++)
                    Matrix[i, j] = matrix[i, j];*/
           //Matrix = matrix;
            ComputeDivisor();
            Offset = 0;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public Kernel(Kernel original)
        {
            Heigth = original.Heigth;
            Width = original.Width;
            Anchor = original.Anchor;
            Matrix = original.Matrix;
            Offset = original.Offset;
            Name = original.Name;
            ComputeDivisor();
        }

        public void ComputeDivisor()
        {
            int sum = 0;
            for(int i = 0; i < Heigth; i++)
            {
                for(int j = 0; j < Width; j++)
                {
                    sum += Matrix[i, j];
                }
            }
            if (sum == 0) Divisor = 1;
            else Divisor = sum;
        }

        public void SetDefaultAnchor()
        {
            Anchor.X = Width / 2;
            Anchor.Y = Heigth / 2;
        }

        
    }
}
