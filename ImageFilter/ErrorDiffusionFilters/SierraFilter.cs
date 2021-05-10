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
    public class SierraFilter : ErrorDiffusionAlgorithm
    {
        public SierraFilter(uint R, uint G, uint B) : base(5, 5, 2)
        {
            KR = R;
            KG = G;
            KB = B;
            Weigths[2, 3] = 5;
            Weigths[2, 4] = 3;
            Weigths[3, 0] = 2;
            Weigths[3, 1] = 4;
            Weigths[3, 2] = 5;
            Weigths[3, 3] = 4;
            Weigths[3, 4] = 2;
            Weigths[4, 0] = 0;
            Weigths[4, 1] = 2;
            Weigths[4, 2] = 3;
            Weigths[4, 3] = 2;
            Weigths[4, 4] = 0;
            for (int i = 0; i < Heigth; i++)
                for (int j = 0; j < Width; j++)
                    Weigths[i, j] = Weigths[i, j] / 32;
        }
    }
}
