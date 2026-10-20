using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KolmRakendust
{
    public class MatchingGameForm : Form
    {
        private TableLayoutPanel tableLayout;
        private Label firstClicked = null, secondClicked = null;
        private Random random = new Random();
        private Timer timer = new Timer();

        private List<string> icons = new List<string>
        {
            "N", "N", "m", "m", "b", "b", "v", "v",
            "w", "w", "z", "z", "5", "5", "6", "6"
        };

        public MatchingGameForm()
        {
            InitializeComponents();
            AssignIconsToSquares();
        }

        private void InitializeComponents()
        {
            this.Text = "Sarnaste piltide leidmise mäng";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterParent;

            tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.CornflowerBlue,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset,
                RowCount = 4,
                ColumnCount = 4
            };

            for (int i = 0; i < 4; i++)
            {
                tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            }

            for (int i = 0; i < 16; i++)
            {
                Label lbl = new Label
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.CornflowerBlue,
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Webdings", 48, FontStyle.Bold),
                    ForeColor = Color.CornflowerBlue
                };
                lbl.Click += label_Click;
                tableLayout.Controls.Add(lbl);
            }

            timer.Interval = 750;
            timer.Tick += Timer_Tick;

            this.Controls.Add(tableLayout);
        }

        private void AssignIconsToSquares()
        {
            foreach (Control control in tableLayout.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        private void label_Click(object sender, EventArgs e)
        {
            if (timer.Enabled) return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                if (clickedLabel.ForeColor == Color.Black) return;

                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }

                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                CheckForWinner();

                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            firstClicked.ForeColor = Color.CornflowerBlue;
            secondClicked.ForeColor = Color.CornflowerBlue;
            firstClicked = null;
            secondClicked = null;
        }

        private void CheckForWinner()
        {
            foreach (Control control in tableLayout.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null && iconLabel.ForeColor == iconLabel.BackColor)
                    return;
            }
            MessageBox.Show("Leidsid kõik paarid üles!", "Palju õnne!");
            Close();
        }
    }
}