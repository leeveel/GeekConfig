using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Base
{
    public class Debuger
    {
        public static void Err(string txt)
        {
            Debug.LogError(txt);
        }
    }
}
