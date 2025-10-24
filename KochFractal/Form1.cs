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
                MessageBox.Show("Выберите глубину рекурсии!");
                return;
            }

            int depth = (int)comboBox1.SelectedItem;
            DrawKochFractal(depth);
        }



        private void DrawKochFractal(int depth)
        {
            Stopwatch sw = Stopwatch.StartNew();

            // Создаем bitmap для рисования
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Начальные точки для кривой Коха
                PointF startPoint = new PointF(50, pictureBox1.Height / 2);
                PointF endPoint = new PointF(pictureBox1.Width - 50, pictureBox1.Height / 2);

                // Рисуем кривую Коха
                DrawKochLine(g, startPoint, endPoint, depth);
            }

            sw.Stop();

            // Отображаем результат
            pictureBox1.Image = bmp;
            label1.Text = $"Время построения: {sw.Elapsed.TotalMilliseconds:F2} мс";
        }

        private void DrawKochLine(Graphics g, PointF p1, PointF p2, int depth)
        {
            if (depth == 0)
            {
                // Базовый случай - рисуем прямую линию
                using (Pen pen = new Pen(Color.Blue, 1))
                {
                    g.DrawLine(pen, p1, p2);
                }
                return;
            }

            // Вычисляем точки для деления отрезка
            PointF p3 = CalculatePoint(p1, p2, 1.0f / 3);
            PointF p5 = CalculatePoint(p1, p2, 2.0f / 3);

            // Вычисляем вершину треугольника
            PointF p4 = CalculateTrianglePoint(p3, p5);

            // Рекурсивно рисуем 4 отрезка
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
            // Вычисляем середину отрезка
            PointF midPoint = new PointF((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);

            // Вычисляем вектор направления
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;

            // Поворачиваем вектор на 60 градусов и масштабируем
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            float height = length * (float)Math.Sqrt(3) / 2;

            // Перпендикулярный вектор (повернутый на 90 градусов)
            float perpX = -dy;
            float perpY = dx;

            // Нормализуем перпендикулярный вектор
            float perpLength = (float)Math.Sqrt(perpX * perpX + perpY * perpY);
            perpX /= perpLength;
            perpY /= perpLength;

            // Вычисляем вершину треугольника
            return new PointF(
                midPoint.X + perpX * height,
                midPoint.Y + perpY * height
            );
        }

        // Оценка глубины рекурсии
        private void EvaluateRecursionDepth()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Оценка глубины рекурсии для кривой Коха:");
            sb.AppendLine("• Количество рекурсивных вызовов: 4^n");
            sb.AppendLine("• Глубина рекурсии: n");
            sb.AppendLine("• Сложность: O(4^n)");
            sb.AppendLine("• Рекомендуемая глубина: 1-7");

            for (int i = 1; i <= 7; i++)
            {
                long calls = (long)Math.Pow(4, i);
                sb.AppendLine($"Глубина {i}: ~{calls:N0} вызовов");
            }

            MessageBox.Show(sb.ToString(), "Оценка рекурсии");
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
