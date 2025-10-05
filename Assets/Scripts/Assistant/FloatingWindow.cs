using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class FloatingWindow : MonoBehaviour
{
    // Stuff that only works on Windows cause of native API
    // Also Unity crashes when running this code in editor
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
    // Self-explanatory name
    [DllImport("User32.dll")]
    private static extern IntPtr GetActiveWindow();
    
    // Change attributes of window
    [DllImport("User32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
    
    // Set window position in "hierarchy"
    [DllImport("User32.dll")]
    private static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    
    // Extends window based on given margins
    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
    
    // Get information about window
    [DllImport("User32.dll")]
    private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

    
    // Color stuff - when pixel is transparent, it should be clickable through
    private bool isClickThrough = false;
    private bool isOnOpaquePixel = true;
    // Threshold to determine alpha values
    private float opaqueThreshold = 0.1f;
    // 1x1 texture to get the color of it later
    private Texture2D colorPickerTexture = null;

    private Camera currentCamera;

    // Window handle - a way to refer each window
    private IntPtr windowHandle;

    private void Awake()
    {
        windowHandle = GetActiveWindow();

        if (Camera.main)
        {
            Camera.main.clearFlags = CameraClearFlags.Depth;
            Camera.main.backgroundColor = new Color(0, 0, 0, 0);
        }
        
        { // SetWindowLong
            const int GWL_STYLE = -16;
            const uint WS_POPUP = 0x80000000;

            SetWindowLong(windowHandle, GWL_STYLE, WS_POPUP);
        }

        { // Set extended window style
            //SetClickThrough(true);
            SetClickThrough(false);
            isClickThrough = false;
        }

        { // SetWindowPos
            IntPtr HWND_TOPMOST = new IntPtr(-1);
            const uint SWP_NOSIZE = 0x0001;
            const uint SWP_NOMOVE = 0x0002;
            const uint SWP_NOACTIVE = 0x0010;
            const uint SWP_SHOWWINDOW = 0x0040;

            SetWindowPos(windowHandle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVE | SWP_SHOWWINDOW);
        }

        { // DwmExtendFrameIntoClientArea
            MARGINS margins = new MARGINS()
            {
                cxLeftWidth = -1
            };

            DwmExtendFrameIntoClientArea(windowHandle, ref margins);
        }
    }

    private void SetClickThrough(bool through)
    {
        // Some windows API stuff:
        const int GWL_EXSTYLE = -20;  // Extended Window
        const uint WS_EX_LAYERED = 0x080000;  // Layered = fast
        const uint WS_EX_TRANSPARENT = 0x00000020;  // Transparent - everyone knows what that means

        uint currentStyle = GetWindowLong(windowHandle, GWL_EXSTYLE);

        if (through)
        {
            SetWindowLong(windowHandle, GWL_EXSTYLE, currentStyle | WS_EX_LAYERED | WS_EX_TRANSPARENT);
        }
        else
        {
            // No idea what needed to be change here, but glad it works
            SetWindowLong(windowHandle, GWL_EXSTYLE, (currentStyle | WS_EX_LAYERED) & ~WS_EX_TRANSPARENT);
        }
        // Debug.LogError($"Window style set to: {(through ? "click-through" : "normal")}");
    }



    void Start()
    {
        
        if (!currentCamera)
        {
            // Grab that default camera
            currentCamera = Camera.main;

            // If there ain't one - look for some other
            if (!currentCamera)
            {
                currentCamera = FindObjectOfType<Camera>();
            }
        }
        
        colorPickerTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);

        // Coroutine to check colors under the cursor
        StartCoroutine(PickColorCoroutine());
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     isClickThrough = !isClickThrough;
        //     SetClickThrough(isClickThrough);
        //     Debug.LogError($"Manually toggled click-through to: {isClickThrough}");
        // }
        
        // Switch to clickable through or nah
        UpdateClickThrough();
    }
    
    void UpdateClickThrough()
    {
        if (isClickThrough)
        {
            // Stop clicks when there is an object
            if (isOnOpaquePixel)
            {
                SetClickThrough(false);
                isClickThrough = false;
            }
        }
        else
        {
            // If pixel isn't colored = it's transparent, so allow clicking through it
            if (!isOnOpaquePixel)
            {
                SetClickThrough(true);
                isClickThrough = true;
            }
        }
    }

    // Every coroutine run check if pixel is transparent or not
    private IEnumerator PickColorCoroutine()
    {
        while (Application.isPlaying)
        {
            yield return new WaitForEndOfFrame();
            ObservePixelUnderCursor(currentCamera);
        }
        yield return null;
    }

    
    // Check if pixel under is transparent or nah
    void ObservePixelUnderCursor(Camera cam)
    {
        if (!cam)
        {
            Debug.LogError("No Camera you dumbass");
            return;
        }
        
        Vector2 mousePos = Input.mousePosition;
        Rect camRect = cam.pixelRect;

        // Is mouse in drawing range? If so - guess if it's on an object or just void
        if (camRect.Contains(mousePos))
        {
            try
            {
                // Finally get the color
                colorPickerTexture.ReadPixels(new Rect(mousePos, Vector2.one), 0, 0);
                Color color = colorPickerTexture.GetPixel(0, 0);

                // Determine if that's opaque or nah
                //Debug.LogError($"Mouse at {mousePos}, Pixel RGBA: {color.r}, {color.g}, {color.b}, {color.a}");                // アルファ値がしきい値以上ならば、不透過とする
                isOnOpaquePixel = (color.a >= opaqueThreshold);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("SHIT" + ex.Message);
                isOnOpaquePixel = false;
            }
        }
        else
        {
            isOnOpaquePixel = false;
        }
    }

    // Get these margins
    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }
#endif  // UNITY_STANDALONE_WIN && !UNITY_EDITOR
}