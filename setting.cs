// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Windows.Forms;

namespace ToolsGenGkode
{
    /// <summary>
    /// Параметры программы
    /// </summary>
    static class Setting
    {
        public static float zoomSize = 1;
        public static bool move3D = false;
        public static bool keyShift;
        public static MouseButtons mouseButton = MouseButtons.None;

        public static int PosX = 0, PosY = 0, PosZ = -300;
        public static int PosAngleX = 0, PosAngleY = 0, PosAngleZ = 0;

        public static bool GenaMode = false;


    }


    
}
