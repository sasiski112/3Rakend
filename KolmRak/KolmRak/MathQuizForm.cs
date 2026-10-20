using System;
using System.Drawing;
using System.Windows.Forms;

namespace KolmRakendust
{
    public class MathQuizForm : Form
    {
        private Label lblTime, lblScore, lblQuestionProgress, lblPlusLeft, lblPlusRight, lblPlusSign, lblEquals;
        private NumericUpDown numSum;
        private Button btnStart, btnSubmit;
        private Timer timer;

        private int timeLeft;
        private int score;
        private int currentQuestion;
        private const int totalQuestions = 5;

        private Random random = new Random();
        private int addend1, addend2;

        public MathQuizForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Matemaatiline mäng (Punktisüsteemiga)";
            this.Size = new Size(450, 320);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;


            lblScore = new Label { Text = "Skoor: 0", Font = new Font("Segoe UI", 11, FontStyle.Bold), Location = new Point(30, 20), AutoSize = true };
            lblQuestionProgress = new Label { Text = "Küsimus: 0/5", Font = new Font("Segoe UI", 11), Location = new Point(170, 20), AutoSize = true };
            lblTime = new Label { Text = "Aeg: -- s", Font = new Font("Segoe UI", 11, FontStyle.Bold), Location = new Point(320, 20), AutoSize = true, ForeColor = Color.DarkRed };


            lblPlusLeft = new Label { Text = "?", Font = new Font("Segoe UI", 18), Location = new Point(50, 85), Size = new Size(50, 35), TextAlign = ContentAlignment.MiddleCenter };
            lblPlusSign = new Label { Text = "+", Font = new Font("Segoe UI", 18), Location = new Point(110, 85), Size = new Size(30, 35), TextAlign = ContentAlignment.MiddleCenter };
            lblPlusRight = new Label { Text = "?", Font = new Font("Segoe UI", 18), Location = new Point(150, 85), Size = new Size(50, 35), TextAlign = ContentAlignment.MiddleCenter };
            lblEquals = new Label { Text = "=", Font = new Font("Segoe UI", 18), Location = new Point(210, 85), Size = new Size(30, 35), TextAlign = ContentAlignment.MiddleCenter };


            numSum = new NumericUpDown { Font = new Font("Segoe UI", 18), Location = new Point(250, 83), Width = 100, Enabled = false };
            numSum.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) SubmitAnswer(); };


            btnSubmit = new Button { Text = "Vasta", Font = new Font("Segoe UI", 10), Location = new Point(250, 130), Size = new Size(100, 35), Enabled = false };
            btnSubmit.Click += (s, e) => SubmitAnswer();

            btnStart = new Button { Text = "Alusta uut mängu", Font = new Font("Segoe UI", 11, FontStyle.Bold), Location = new Point(120, 200), Size = new Size(200, 45) };
            btnStart.Click += BtnStart_Click;

            timer = new Timer { Interval = 1000 };
            timer.Tick += Timer_Tick;

            this.Controls.AddRange(new Control[] { lblScore, lblQuestionProgress, lblTime, lblPlusLeft, lblPlusSign, lblPlusRight, lblEquals, numSum, btnSubmit, btnStart });
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            score = 0;
            currentQuestion = 0;
            lblScore.Text = "Skoor: 0";
            btnStart.Enabled = false;
            numSum.Enabled = true;
            btnSubmit.Enabled = true;

            NextQuestion();
        }

        private void NextQuestion()
        {
            currentQuestion++;

            if (currentQuestion > totalQuestions)
            {
                EndGame();
                return;
            }

            lblQuestionProgress.Text = $"Küsimus: {currentQuestion}/{totalQuestions}";

            addend1 = random.Next(1, 50);
            addend2 = random.Next(1, 50);
            lblPlusLeft.Text = addend1.ToString();
            lblPlusRight.Text = addend2.ToString();
            numSum.Value = 0;
            numSum.Focus();

            timeLeft = 10;
            lblTime.Text = $"Aeg: {timeLeft} s";
            timer.Start();
        }

        private void SubmitAnswer()
        {
            timer.Stop();

            if (addend1 + addend2 == numSum.Value)
            {
                int pointsEarned = 100 + (timeLeft * 10);
                score += pointsEarned;
                lblScore.Text = $"Skoor: {score}";
                MessageBox.Show($"Õige vastus! Said +{pointsEarned} punkti!", "Tulemus", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Vale vastus! Õige oli {addend1 + addend2}.", "Tulemus", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NextQuestion();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                lblTime.Text = $"Aeg: {timeLeft} s";
                timeLeft--;
            }
            else
            {
                timer.Stop();
                lblTime.Text = "Aeg läbi!";
                MessageBox.Show($"Aeg sai otsa! Õige vastus oli {addend1 + addend2}.", "Aeg läbi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                NextQuestion();
            }
        }

        private void EndGame()
        {
            timer.Stop();
            numSum.Enabled = false;
            btnSubmit.Enabled = false;
            btnStart.Enabled = true;

            MessageBox.Show($"Mäng on läbi!\nSinu lõppskoor: {score} punkti!", "Mäng läbi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}