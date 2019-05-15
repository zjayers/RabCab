﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RabCab.Settings;

namespace RabCab.Entities.Controls
{
    public sealed class ReadOnlyTextBox : TextBox
    {
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);

        public ReadOnlyTextBox()
        {
            this.ReadOnly = true;
            this.GotFocus += TextBoxGotFocus;
            //this.Paint += TextBorder_Paint;
            this.BorderStyle = BorderStyle.None;
            this.Cursor = Cursors.Arrow; // mouse cursor like in other controls
        }

        private void TextBoxGotFocus(object sender, EventArgs args)
        {
            HideCaret(this.Handle);
        }

        private void TextBorder_Paint(object sender, PaintEventArgs e)
        {
            var borderColor = Colors.GetCadBorderColor();
            var borderStyle = ButtonBorderStyle.Solid;
            var borderWidth = 1;

            ControlPaint.DrawBorder(
                e.Graphics,
                this.ClientRectangle,
                borderColor,
                borderWidth,
                borderStyle,
                borderColor,
                borderWidth,
                borderStyle,
                borderColor,
                borderWidth,
                borderStyle,
                borderColor,
                borderWidth,
                borderStyle);

        }

    }
}