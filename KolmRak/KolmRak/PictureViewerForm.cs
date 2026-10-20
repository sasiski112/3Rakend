using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace KolmRakendust
{
    public class PictureViewerForm : Form
    {
        private PictureBox pictureBox;
        private TableLayoutPanel tableLayout;
        private FlowLayoutPanel flowLayout;
        private Button btnShowImage, btnSaveImage, btnClearImage, btnClose;
        private CheckBox chkStretch;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;

        private bool isDrawing = false;
        private Point previousPoint = Point.Empty;
        private Bitmap drawingBitmap = null;

        public PictureViewerForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Pildi vaatamise programm (Joonistamisega)";
            this.Size = new Size(750, 550);
            this.StartPosition = FormStartPosition.CenterParent;

            tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 2
            };
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 85F));
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 90F));
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));

            pictureBox = new PictureBox
            {
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Normal
            };

            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;

            tableLayout.Controls.Add(pictureBox, 0, 0);
            tableLayout.SetColumnSpan(pictureBox, 2);

            chkStretch = new CheckBox
            {
                Text = "Venita pilti (Stretch)",
                Dock = DockStyle.Fill
            };
            chkStretch.CheckedChanged += (s, e) =>
            {
                pictureBox.SizeMode = chkStretch.Checked ? PictureBoxSizeMode.StretchImage : PictureBoxSizeMode.Normal;
            };
            tableLayout.Controls.Add(chkStretch, 0, 1);

            flowLayout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft
            };

            btnShowImage = new Button { Text = "Ava pilt", AutoSize = true };
            btnSaveImage = new Button { Text = "Salvesta", AutoSize = true };
            btnClearImage = new Button { Text = "Tühjenda", AutoSize = true };
            btnClose = new Button { Text = "Sulge", AutoSize = true };

            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            openFileDialog = new OpenFileDialog
            {
                Filter = "Pildifailid|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                InitialDirectory = Directory.Exists(downloadsPath) ? downloadsPath : Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG pilt|*.png|JPEG pilt|*.jpg|Bitmap pilt|*.bmp",
                InitialDirectory = Directory.Exists(downloadsPath) ? downloadsPath : Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            btnShowImage.Click += (s, e) =>
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Image original = Image.FromFile(openFileDialog.FileName);
                    drawingBitmap = new Bitmap(original);
                    original.Dispose();

                    pictureBox.Image = drawingBitmap;
                }
            };

            btnSaveImage.Click += (s, e) =>
            {
                if (drawingBitmap == null)
                {
                    MessageBox.Show("Ühtegi pilti pole avatud ega muudetud!", "Viga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png;
                    string ext = Path.GetExtension(saveFileDialog.FileName).ToLower();

                    if (ext == ".jpg" || ext == ".jpeg")
                        format = System.Drawing.Imaging.ImageFormat.Jpeg;
                    else if (ext == ".bmp")
                        format = System.Drawing.Imaging.ImageFormat.Bmp;

                    drawingBitmap.Save(saveFileDialog.FileName, format);
                    MessageBox.Show("Pilt edukalt salvestatud!", "Salvestamine", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            btnClearImage.Click += (s, e) =>
            {
                pictureBox.Image = null;
                drawingBitmap = null;
            };

            btnClose.Click += (s, e) => this.Close();

            flowLayout.Controls.Add(btnClose);
            flowLayout.Controls.Add(btnSaveImage);
            flowLayout.Controls.Add(btnClearImage);
            flowLayout.Controls.Add(btnShowImage);

            tableLayout.Controls.Add(flowLayout, 1, 1);
            this.Controls.Add(tableLayout);
        }

        private Point GetImageCoordinates(Point mouseLocation)
        {
            if (drawingBitmap == null || pictureBox.Image == null) return mouseLocation;

            if (pictureBox.SizeMode == PictureBoxSizeMode.StretchImage)
            {
                float xRatio = (float)drawingBitmap.Width / pictureBox.ClientSize.Width;
                float yRatio = (float)drawingBitmap.Height / pictureBox.ClientSize.Height;
                return new Point((int)(mouseLocation.X * xRatio), (int)(mouseLocation.Y * yRatio));
            }

            return mouseLocation;
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (drawingBitmap == null) return;
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = true;
                previousPoint = GetImageCoordinates(e.Location);
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && drawingBitmap != null)
            {
                Point currentPoint = GetImageCoordinates(e.Location);

                using (Graphics g = Graphics.FromImage(drawingBitmap))
                {
                    using (Pen pen = new Pen(Color.Red, 4))
                    {
                        g.DrawLine(pen, previousPoint, currentPoint);
                    }
                }
                pictureBox.Invalidate();
                previousPoint = currentPoint;
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = false;
            }
        }
    }
}