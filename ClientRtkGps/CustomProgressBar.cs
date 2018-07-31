using System.Drawing;
using System.Windows.Forms;

namespace ClientRtkGps
{
    class CustomProgressBar : Control
    {
        public CustomProgressBar()
        {
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, false);

            this.ForeColor = Color.Green;
            this.BackColor = Color.White;
            this.BorderColor = Color.DarkGray;

            this.Width = 100;
            this.Height = 23;
        }

        private Color BorderColor;

        public int Minimum { get; set; } = 0;
        public int Maximum { get; set; } = 100;

        private int mValue;
        public int Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var rc = new RectangleF(0, 0, (float)(this.Width * (Value - Minimum) / Maximum), this.Height);
            using (var br = new SolidBrush(this.ForeColor))
            {
                e.Graphics.FillRectangle(br, rc);
            }
            using (var br = new Pen(BorderColor, 2))
            {
                e.Graphics.DrawRectangle(br, DisplayRectangle);
            }
            base.OnPaint(e);
        }
    }
}
