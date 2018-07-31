using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRtkGps
{
    interface ISerialReceiver
    {
        int BytesToRead
        {
            get;
        }

        int Read(byte[] buffer, int offset, int count);
    }
}
