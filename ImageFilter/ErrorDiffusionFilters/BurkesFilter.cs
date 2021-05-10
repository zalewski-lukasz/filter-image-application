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
    public class BurkesFilter : ErrorDiffusionAlgorithm
    {
        public BurkesFilter(uint R, uint G, uint B) : base(3, 5, 2)
        {
            KR = R;
            KG = G;
            KB = B;
            Weigths[1, 3] = 8;
            Weigths[1, 4] = 8;
            Weigths[2, 0] = 2;
            Weigths[2, 1] = 4;
            Weigths[2, 2] = 8;
            Weigths[2, 3] = 4;
            Weigths[2, 4] = 2;
            for (int i = 0; i < Heigth; i++)
                for (int j = 0; j < Width; j++)
                    Weigths[i, j] = Weigths[i, j] / 32;
        }
    }
}
