using UnityEngine;
using System.Runtime.InteropServices;

public class NumLockManager : MonoBehaviour
{
    // Private
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    private static extern short GetKeyState(int keyCode);

    [DllImport("user32.dll")]
    private static extern int GetKeyboardState(byte[] lpKeyState);

    [DllImport("user32.dll", EntryPoint = "keybd_event")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

    private bool originalNumLockState;
    private const byte VK_NUMLOCK = 0x90;
    private const uint KEYEVENTF_EXTENDEDKEY = 1;
    private const int KEYEVENTF_KEYUP = 0x2;
    private const int KEYEVENTF_KEYDOWN = 0x0;

    void Awake()
    {
        originalNumLockState = GetNumLock(); // Save Original NumLock State

        SetNumLock(true); // Set NumLock On 
    }

    void OnApplicationQuit()
    {
        SetNumLock(originalNumLockState); // Set NumLock to Users Original State
    }

    // Get NumLock State
    public bool GetNumLock()
    {
        return (((ushort)GetKeyState(0x90)) & 0xffff) != 0;
    }

    // Set NumLock State
    public void SetNumLock(bool bState)
    {
        if (GetNumLock() != bState)
        {
            keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYDOWN, 0);
            keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
    }
}