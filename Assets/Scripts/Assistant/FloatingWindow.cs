using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class FloatingWindow : MonoBehaviour
{
    // Windows DLL stuff cause Unity doesn't support transparent windows
#if UNITY_STANDALONE_WIN
    // Dialog - Windows only
    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr winHandle, string text, string caption, uint type);
    
    // Get reference to current window - Windows only
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    // Fit window within margins - Windows only
    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr winHandle, ref MARGINS margins);

    // Change attribute of window, whatever that means
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr winHandle, int nIndex, uint dwNewLong);
    
    // Set position of windows duh
    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr winHandle, IntPtr winHandleInsertAfter, int x, int y,
        int cx, int cy, uint uFlags);

    // New extended window code
    private const int GWL_EXSTYLE = -20;
    // Layered - it's just fast ig?
    private const uint WS_EX_LAYERED = 0x00080000;
    // Transparent - you can click through it yayyy
    private const uint WS_EX_TRANSPARENT = 0x00000020;
    
    // Top window ID
    private static readonly IntPtr WIN_HANDLER_TOP = new IntPtr(-1);
#endif
    
    void Start()
    {
        Application.runInBackground = true;
        
        // Windows only - render window
#if UNITY_STANDALONE_WIN
        //MessageBox(new IntPtr(0), "Test", "Dialog", 0);
        
        // Apparently Unity crashes when you use these API methods in Editor lol
//#if !UNITY_EDITOR
        // Prepare + render the above window
        IntPtr winHandle = GetActiveWindow();
        MARGINS margins = new MARGINS { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(winHandle, ref margins);

        // Make it fast & click-through
        SetWindowLong(winHandle, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);

        SetWindowPos(winHandle, WIN_HANDLER_TOP, 0, 0, 0, 0, 0);
//#endif
#endif
    }

    void Update()
    {
        
    }

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cxTopHeight;
        public int cxBottomHeight;
    }
}
