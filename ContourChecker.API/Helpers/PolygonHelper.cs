using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.Types;

namespace ContourChecker.API.Helpers
{
    public static class PolygonHelper
    {
        /// <summary>
        /// https://stackoverflow.com/questions/4243042/c-sharp-point-in-polygon Dubbs777 answer includes points on edge
        /// </summary>
        /// <param name="point"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static bool IsInPolygon(this VVector point, IEnumerable<VVector> polygon)
        {
            bool result = false;
            var a = polygon.Last();
            foreach (var b in polygon)
            {
                if ((b.x == point.x) && (b.y == point.y))
                    return true;

                if ((b.y == a.y) && (point.y == a.y) && (a.x <= point.x) && (point.x <= b.x))
                    return true;

                if ((b.y < point.y) && (a.y >= point.y) || (a.y < point.y) && (b.y >= point.y))
                {
                    if (b.x + (point.y - b.y) / (a.y - b.y) * (a.x - b.x) <= point.x)
                        result = !result;
                }
                a = b;
            }
            return result;
        }

        public static double ClosestPoint(VVector pt, VVector[] outerSlicePart)
        {
            return outerSlicePart.Min(v => v.DistanceTo(pt));
        }

        public static double DistanceTo(this VVector vv, VVector v2)
        {
            return Math.Sqrt((vv.x - v2.x) * (vv.x - v2.x) + (vv.y - v2.y) * (vv.y - v2.y) +
                             (vv.z - v2.z) * (vv.z - v2.z));
        }
    }
}
