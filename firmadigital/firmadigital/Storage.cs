using System;
using System.Collections.Generic;
using System.Text;

namespace firmadigital
{
    public interface Storage
    {
        string SaveImage(byte[] bytedata);
    }
}
