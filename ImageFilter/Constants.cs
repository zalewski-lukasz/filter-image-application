using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageFilter
{
    public class Constants
    {
        public static int BrightnessCoefficient = 30;

        public static int ContrastCoefficient = 100;

        public static double GammaCoefficient = 0.25;

        public static int[,] BlurKernel = new int[3, 3]
        {
            {1, 1, 1},
            {1, 1, 1},
            {1, 1, 1}
        };
        public static int BlurWidth = 3;
        public static int BlurHeigth = 3;

        public static int[,] GaussianBlurKernel = new int[3, 3]
        {
            {0, 1, 0},
            {1, 4, 1},
            {0, 1, 0}
        };
        public static int GaussianBlurWidth = 3;
        public static int GaussianBlurHeigth = 3;

        public static int[,] SharpenKernel = new int[3, 3]
        {
            {-1, -1, -1},
            {-1, 9, -1},
            {-1, -1, -1}
        };
        public static int SharpenHeigth = 3;
        public static int SharpenWidth= 3;

        public static int[,] EdgeDetectionKernel = new int[3, 3]
        {
            {0, -1, 0},
            {0, 1, 0},
            {0, 0, 0}
        };
        public static int EdgeDetectionHeigth = 3;
        public static int EdgeDetectionWidth = 3;

        public static int[,] EmbossKernel = new int[3, 3]
        {
            {-1, 0, 1},
            {-1, 1, 1},
            {-1, 0, 1}
        };
        public static int EmbossWeigth = 3;
        public static int EmbossHeigth = 3;

        public static int[,] Identity = new int[3, 3]
        {
            {0, 0, 0 },
            {0, 1, 0 },
            {0, 0, 0 }
        };
    }
}
