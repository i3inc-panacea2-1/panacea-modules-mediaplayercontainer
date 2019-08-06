using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Panacea.Modules.MediaPlayerContainer
{
    class MouseCaptureForm : Form
    {
        public MouseCaptureForm()
        {
            //SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            TopMost = true;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            //BackColor = Color.Red;
            TransparencyKey = Color.Red;
            var pb = new PictureBox()
            {
                BackColor = Color.Red,
                Dock = DockStyle.Fill,
                Margin = new Padding(20,20,20,20)
            };
            pb.MouseDown += pictureBox1_MouseDown;
            Controls.Add(pb);
            this.Click += MouseCaptureForm_Click;
        }


        private void MouseCaptureForm_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            OnClick2?.Invoke(this, null);


        }

        public event EventHandler OnClick2;

        #region Form Dragging API Support
        //The SendMessage function sends a message to a window or windows.

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]

        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        //ReleaseCapture releases a mouse capture

        [DllImportAttribute("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]

        public static extern bool ReleaseCapture();

        #endregion
    }
}
