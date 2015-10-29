using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model.PositionHelp
{
    public class InLinePositionHelper : IPositionHelper
    {
        public List<Coordinate> GetCoordinates(int countOfStations, int connectionRadius, int height, int width)
        {
            var list = new List<Coordinate>();

            var columns = (height / (connectionRadius * 0.8))-1;
            var k = 0;
            var n = 0;

            while (k < countOfStations)
            {
                for (int i = 0; i < columns; i++)
                {
                    if (k < countOfStations)
                    {
                        k++;
                        var c = new Coordinate() { X = connectionRadius * 0.8 + i * connectionRadius * 0.8, Y = connectionRadius * 0.8 + n * connectionRadius * 0.8 };
                        list.Add(c);
                    }
                }
                n++;
            }
            return list;

        }
    }
}
