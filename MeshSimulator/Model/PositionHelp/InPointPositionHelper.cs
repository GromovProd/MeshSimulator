using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model.PositionHelp
{
    public class InPointPositionHelper : IPositionHelper
    {
        public List<Coordinate> GetCoordinates(int countOfStations, int connectionRadius, int height, int width)
        {
            var list = new List<Coordinate>();

            for (int i = 0; i < countOfStations; i++)
            {
                var centr = new Coordinate() { X = width / 2, Y = height / 2 };
                list.Add(centr);
            }

            return list;

        }
    }
}
