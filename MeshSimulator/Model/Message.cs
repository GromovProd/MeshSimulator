using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model
{
    public class Message : MeshSimulator.Model.IMessage
    {
        private bool isNoise = true;
        public bool IsNoise
        {
            get { return isNoise; }
            set { isNoise = value; }
        }

        private bool isSpecial = false;

        public bool IsSpecial
        {
            get { return isSpecial; }
            set { isSpecial = value; }
        }


        private int fromId;
        public int FromId
        {
            get { return fromId; }
            set { fromId = value; }
        }

        private int toId;
        public int ToId
        {
            get { return toId; }
            set { toId = value; }
        }

        public Message(bool isNoise, bool isSpecial, int fromId, int toId)
        {
            IsNoise = isNoise;
            IsSpecial = isSpecial;
            FromId = fromId;
            ToId = toId;
        }



    }
}
