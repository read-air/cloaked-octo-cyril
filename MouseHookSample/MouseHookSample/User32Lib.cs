using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MouseHookSample
{
    /// <summary>
    /// Windowsフック種別
    /// </summary>
    public static class WindowsHookType
    {
        /// <summary>
        /// キーボードフック
        /// </summary>
        public const int WH_KEYBOARD = 2;
    }

    /// <summary>
    /// User32.lib用ラッパークラス
    /// </summary>
    internal static class User32Lib
    {
        #region 型定義
        /// <summary>
        /// フック用プロシージャ
        /// </summary>
        /// <param name="nCode">メッセージコード</param>
        /// <param name="wp">WPARAM値</param>
        /// <param name="lp">LPARAM値</param>
        /// <returns>フックプロシージャの戻り値、戻さない場合はIntPtr.Zero</returns>
        public delegate IntPtr HookProc(int nCode, IntPtr wp, IntPtr lp);
        #endregion

        #region インポートメソッド
        /// <summary>
        /// Windowsフック作成
        /// </summary>
        /// <param name="idHook">フックID</param>
        /// <param name="lpfn">プロシージャ</param>
        /// <param name="hInstance">インスタンス</param>
        /// <param name="dwThreadId">スレッドID</param>
        /// <returns>作成されたフックハンドル</returns>
        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, SetLastError=true)]
        public extern static IntPtr SetWindowsHookEx(Int32 idHook, HookProc lpfn, IntPtr hInstance, UInt32 dwThreadId);

        /// <summary>
        /// Windowsフック開放
        /// </summary>
        /// <param name="hhk">開放されたフックハンドル</param>
        /// <returns></returns>
        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool UnhookWindowsHookEx(IntPtr hhk);

        /// <summary>
        /// フック情報譲渡
        /// </summary>
        /// <param name="hhk">フックハンドラ</param>
        /// <param name="nCode">メッセージコード</param>
        /// <param name="wp">WPARAM値</param>
        /// <param name="lp">LPARAM値</param>
        /// <returns>フックプロシージャの戻り値</returns>
        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public extern static IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wp, IntPtr lp);
        #endregion
    }

    /// <summary>
    /// Kernel32.lib用ラッパークラス
    /// </summary>
    internal static class Kernel32Lib
    {
        /// <summary>
        /// 最終発生エラー取得
        /// </summary>
        /// <returns>エラーコード</returns>
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        public extern static UInt32 GetLastError();
    }
}
