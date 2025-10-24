using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KochFractal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeComboBox();
        }

        private void InitializeComboBox()
        {
            comboBox1.Items.Clear();
            for (int i = 1; i <= 7; i++)
            {
                comboBox1.Items.Add(i);
            }
            comboBox1.SelectedIndex = 0;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("�������� ������� ��������!");
                return;
            }

            int depth = (int)comboBox1.SelectedItem;
            DrawKochFractal(depth);
        }



        private void DrawKochFractal(int depth)
        {
            Stopwatch sw = Stopwatch.StartNew();

            // ������� bitmap ��� ���������
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // ��������� ����� ��� ������ ����
                PointF startPoint = new PointF(50, pictureBox1.Height / 2);
                PointF endPoint = new PointF(pictureBox1.Width - 50, pictureBox1.Height / 2);

                // ������ ������ ����
                DrawKochLine(g, startPoint, endPoint, depth);
            }

            sw.Stop();

            // ���������� ���������
            pictureBox1.Image = bmp;
            label1.Text = $"����� ����������: {sw.Elapsed.TotalMilliseconds:F2} ��";
        }

        private void DrawKochLine(Graphics g, PointF p1, PointF p2, int depth)
        {
            if (depth == 0)
            {
                // ������� ������ - ������ ������ �����
                using (Pen pen = new Pen(Color.Blue, 1))
                {
                    g.DrawLine(pen, p1, p2);
                }
                return;
            }

            // ��������� ����� ��� ������� �������
            PointF p3 = CalculatePoint(p1, p2, 1.0f / 3);
            PointF p5 = CalculatePoint(p1, p2, 2.0f / 3);

            // ��������� ������� ������������
            PointF p4 = CalculateTrianglePoint(p3, p5);

            // ���������� ������ 4 �������
            DrawKochLine(g, p1, p3, depth - 1);
            DrawKochLine(g, p3, p4, depth - 1);
            DrawKochLine(g, p4, p5, depth - 1);
            DrawKochLine(g, p5, p2, depth - 1);
        }

        private PointF CalculatePoint(PointF p1, PointF p2, float ratio)
        {
            return new PointF(
                p1.X + (p2.X - p1.X) * ratio,
                p1.Y + (p2.Y - p1.Y) * ratio
            );
        }

        private PointF CalculateTrianglePoint(PointF p1, PointF p2)
        {
            // ��������� �������� �������
            PointF midPoint = new PointF((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);

            // ��������� ������ �����������
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;

            // ������������ ������ �� 60 �������� � ������������
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            float height = length * (float)Math.Sqrt(3) / 2;

            // ���������������� ������ (���������� �� 90 ��������)
            float perpX = -dy;
            float perpY = dx;

            // ����������� ���������������� ������
            float perpLength = (float)Math.Sqrt(perpX * perpX + perpY * perpY);
            perpX /= perpLength;
            perpY /= perpLength;

            // ��������� ������� ������������
            return new PointF(
                midPoint.X + perpX * height,
                midPoint.Y + perpY * height
            );
        }

        // ������ ������� ��������
        private void EvaluateRecursionDepth()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("������ ������� �������� ��� ������ ����:");
            sb.AppendLine("� ���������� ����������� �������: 4^n");
            sb.AppendLine("� ������� ��������: n");
            sb.AppendLine("� ���������: O(4^n)");
            sb.AppendLine("� ������������� �������: 1-7");

            for (int i = 1; i <= 7; i++)
            {
                long calls = (long)Math.Pow(4, i);
                sb.AppendLine($"������� {i}: ~{calls:N0} �������");
            }

            MessageBox.Show(sb.ToString(), "������ ��������");
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            EvaluateRecursionDepth();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
