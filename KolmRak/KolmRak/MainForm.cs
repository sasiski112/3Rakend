using System;
using System.Drawing;
using System.Windows.Forms;

namespace KolmRakendust
{
    public class MainForm : Form
    {
        private Button btnPictureViewer;
        private Button btnMathQuiz;
        private Button btnMatchingGame;
        private Label lblTitle;

        public MainForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Kolm WinForms Rakendust";
            this.Size = new Size(450, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            lblTitle = new Label
            {
                Text = "Vali käivitatav rakendus:",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(30, 20),
                AutoSize = true
            };

            btnPictureViewer = CreateButton("1. Pildi vaatamise programm", new Point(50, 70), (s, e) => new PictureViewerForm().Show());
            btnMathQuiz = CreateButton("2. Matemaatiline mäng", new Point(50, 130), (s, e) => new MathQuizForm().Show());
            btnMatchingGame = CreateButton("3. Sarnaste piltide mäng", new Point(50, 190), (s, e) => new MatchingGameForm().Show());

            this.Controls.Add(lblTitle);
            this.Controls.Add(btnPictureViewer);
            this.Controls.Add(btnMathQuiz);
            this.Controls.Add(btnMatchingGame);
        }

        private Button CreateButton(string text, Point location, EventHandler onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Location = location,
                Size = new Size(330, 45),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                UseVisualStyleBackColor = true
            };
            btn.Click += onClick;
            return btn;
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}