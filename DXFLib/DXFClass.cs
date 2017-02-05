// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLib
{
    public class DXFClass
    {
        [Flags]
        public enum Caps
        {
            NoOperationAllowed = 0,
            Erase = 1,
            Transform = 2,
            ColorChange = 4,
            LayerChange = 8,
            LineTypeChange = 16,
            LineTypeScale = 32,
            VisibilityChange = 64,
            AllOperationExceptCloning = 127,
            Cloning = 128,
            AllOp = 255,
            R13FormatProxy = 32768
        }

        public string DXFRecord { get; set; }
        public string AppName { get; set; }
        public string ClassName { get; set; }
        public Caps? ClassProxyCapabilities { get; set; }
        public bool? WasProxy { get; set; }
        public bool? IsEntity { get; set; }
    }
}
