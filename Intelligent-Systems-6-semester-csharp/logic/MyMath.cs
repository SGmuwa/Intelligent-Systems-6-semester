using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.logic
{
    static class MyMath
    {
        public static double SearchYifX0X1Y0YX(double x0, double x1, double y0, double y1, double x)
        {
            return (y1 - y0) / (x1 - x0) * x;
        }
    }
}
