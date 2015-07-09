using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseHookSample
{
    public partial class Form1 : Form
    {
        private KeyboardHook mHook;

        private static Dictionary<int, string> EventNames = new Dictionary<int, string>
        {
            { KeyboardHook.WM_KEYDOWN,    "キー押下"},
            { KeyboardHook.WM_KEYUP,      "キー開放"},
            { KeyboardHook.WM_SYSKEYDOWN, "システムキー押下"},
            { KeyboardHook.WM_SYSKEYUP,   "システムキー開放"},
        };

        public Form1()
        {
            InitializeComponent();

            this.mHook = new KeyboardHook();
            this.mHook.KeyHookEvent += KeyHookEvent;
        }

        void KeyHookEvent(object sender, KeyHookEventArgs e)
        {
            // イベントが来なかったら解除
            if( !EventNames.ContainsKey(e.Code))
            {
                return;
            }

            this.dataGridView1.Rows.Add(EventNames[e.Code], e.Key.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.mHook.Active = !this.mHook.Active;
            this.button1.Text = this.mHook.Active ? "フック停止" : "フック開始";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
        }
    }
}
