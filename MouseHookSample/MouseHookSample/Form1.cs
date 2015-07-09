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

        public Form1()
        {
            InitializeComponent();

            this.mHook = new KeyboardHook();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.mHook.Active = !this.mHook.Active;
        }
    }
}
