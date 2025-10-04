using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;

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

    [DllImport("user32.dll")]
    static extern int SetLayeredWindowAttributes(IntPtr winHandle, uint crKey, byte bAlpha, uint dwFlags);
    

    //private const uint LWA_COLORKEY = 0x00000001;

    // New extended window code
    private const int GWL_EXSTYLE = -20;
    // Layered - it's just fast ig?
    private const uint WS_EX_LAYERED = 0x00080000;
    // Transparent - you can click through it yayyy
    private const uint WS_EX_TRANSPARENT = 0x00000020;
    
    // Top window ID
    private static readonly IntPtr WIN_HANDLER_TOP = new IntPtr(-1);
    
    // Prepare + render the above window
    private IntPtr WinHandle;
#endif
    
    void Start()
    {
        // Windows only - render window
#if UNITY_STANDALONE_WIN
        //MessageBox(new IntPtr(0), "Test", "Dialog", 0);
        
        // Apparently Unity crashes when you use these API methods in Editor lol
//#if !UNITY_EDITOR
        WinHandle = GetActiveWindow();
        
        MARGINS margins = new MARGINS { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(WinHandle, ref margins);
        
        // Make it fast & click-through
        SetWindowLong(WinHandle, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
        //SetWindowLong(WinHandle, GWL_EXSTYLE, WS_EX_LAYERED);
        //SetLayeredWindowAttributes(WinHandle, 0, 0, LWA_COLORKEY);
        
        SetWindowPos(WinHandle, WIN_HANDLER_TOP, 0, 0, 0, 0, 0);
//#endif
#endif
        
        Application.runInBackground = true;
    }

    void Update()
    {
        //bool isOk = Physics2D.OverlapPoint(GetMouseWorldPos()) == null;
        bool isOverUi = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) != null;
        MakeTransparentClick(!isOverUi);
        // MakeTransparentClick(Physics2D.OverlapPoint(CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition()) == null);
    }

    private bool IsPointerOverUi()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        
        PointerEventData pointerEvent = new PointerEventData(EventSystem.current);
        pointerEvent.position = Input.mousePosition;

        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEvent, hits);

        return hits.Count > 0;
    }

    private static Vector3 GetMouseWorldPos()
    {
        Camera cam = Camera.main;
        Vector3 worldPos;
        if (cam != null)
        {
            worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0f;
        }
        else
        {
            worldPos = Vector3.zero;
        }

        return worldPos;
    }

    private void MakeTransparentClick(bool transparent)
    {
        if (transparent)
        {
            SetWindowLong(WinHandle, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
        }
        else
        {
            SetWindowLong(WinHandle, GWL_EXSTYLE, WS_EX_LAYERED);
        }
    }

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cxTopHeight;
        public int cxBottomHeight;
    }
}
