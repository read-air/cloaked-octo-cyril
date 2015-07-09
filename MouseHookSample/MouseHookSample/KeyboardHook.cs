using System;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MouseHookSample
{
    /// <summary>
    /// マウスフッククラス
    /// </summary>
    class KeyboardHook : IDisposable
    {
        #region メンバ
        /// <summary>
        /// フック用ハンドル
        /// </summary>
        private IntPtr mHookHandle = IntPtr.Zero;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KeyboardHook()
        {
        }
        #endregion

        #region DllImport
        [DllImport("MyDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int add(int a, int b);
        #endregion

        #region プロパティ
        /// <summary>
        /// フックのアクティブ状態を取得、設定する
        /// </summary>
        public bool Active
        {
            get
            {
                return this.mHookHandle != IntPtr.Zero;
            }

            set
            {
                if (value)
                {
                    this.Start();
                }
                else
                {
                    this.Stop();
                }
            }
        }
        #endregion

        #region 外部メソッド
        /// <summary>
        /// キーボードフック開始
        /// </summary>
        public void Start()
        {
            // 動作中ならこれ以上動作させない
            if (this.Active)
            {
                return;
            }

            // フックハンドル取得
            this.mHookHandle = 
                User32Lib.SetWindowsHookEx(WindowsHookType.WH_KEYBOARD, KeyboardHookProc, GetInstance(), 0);

            // フック失敗用エラー表示
            if (this.mHookHandle == IntPtr.Zero)
            {
                Console.WriteLine("Last Error : 0x" + Kernel32Lib.GetLastError().ToString("x8"));
            }

            return;
        }

        /// <summary>
        /// キーボードフック終了
        /// </summary>
        public void Stop()
        {
            // 停止中ならそのまま終了
            if (!this.Active)
            {
                return;
            }

            // フックハンドル開放
            User32Lib.UnhookWindowsHookEx(this.mHookHandle);
            this.mHookHandle = IntPtr.Zero;

            return;
        }

        /// <summary>
        /// リソース開放
        /// </summary>
        public void Dispose()
        {
            // 開放前に必ず終了する
            this.Stop();
            return;
        }
        #endregion

        #region 内部メソッド
        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <returns>取得したインスタンス</returns>
        private static IntPtr GetInstance()
        {
            IntPtr hInstance = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]);
            return hInstance;
        }

        /// <summary>
        /// キーボードプロシージャ
        /// </summary>
        /// <param name="code">コード</param>
        /// <param name="wParam">WPARAM値</param>
        /// <param name="lParam">LPARAM値</param>
        /// <returns>戻り値</returns>
        private IntPtr KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0)
                Console.WriteLine((Keys)wParam.ToInt32());

            return User32Lib.CallNextHookEx(this.mHookHandle, code, wParam, lParam);
        }
        #endregion
    }
}
