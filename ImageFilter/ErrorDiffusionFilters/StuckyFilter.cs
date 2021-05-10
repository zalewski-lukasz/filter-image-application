using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageFilter.ErrorDiffusionFilters
{
    public class StuckyFilter : ErrorDiffusionAlgorithm
    {
        public StuckyFilter(uint R, uint G, uint B) : base(5, 5, 2)
        {
            KR = R;
            KG = G;
            KB = B;
            Weigths[2, 3] = 8;
            Weigths[2, 4] = 8;
            Weigths[3, 0] = 2;
            Weigths[3, 1] = 4;
            Weigths[3, 2] = 8;
            Weigths[3, 3] = 4;
            Weigths[3, 4] = 2;
            Weigths[4, 0] = 1;
            Weigths[4, 1] = 2;
            Weigths[4, 2] = 4;
            Weigths[4, 3] = 2;
            Weigths[4, 4] = 1;
            for (int i = 0; i < Heigth; i++)
                for (int j = 0; j < Width; j++)
                    Weigths[i, j] = Weigths[i, j] / 42;
        }
    }
}
