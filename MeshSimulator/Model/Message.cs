using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model
{
    public class Message
    {
        private bool isNoise = true;

        public bool IsNoise
        {
            get { return isNoise; }
            set { isNoise = value; }
        }
        public Message(bool isNoise)
        {
            IsNoise = isNoise;
        }



    }
}
