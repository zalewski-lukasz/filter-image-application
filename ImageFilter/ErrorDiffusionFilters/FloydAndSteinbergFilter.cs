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
    public class FloydAndSteinbergFilter : ErrorDiffusionAlgorithm
    {
        public FloydAndSteinbergFilter(uint R, uint G, uint B) : base(3, 3, 2)
        {
            KR = R;
            KG = G;
            KB = B;
            Weigths[1, 2] = 7;
            Weigths[2, 0] = 3;
            Weigths[2, 1] = 5;
            Weigths[2, 2] = 1;
            for (int i = 0; i < Heigth; i++)
                for (int j = 0; j < Width; j++)
                    Weigths[i, j] = Weigths[i, j] / 16;
        }
   
    }
}
