using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Automata_Application
{
    public partial class Form1 : Form
    {

        private Dictionary<int, Dictionary<char, int>> transitionTable;
        private int currentState = 0;



      

        public Form1()
        {
            InitializeComponent();
            this.Paint += Form1_Paint;

        }

        private void Form1_Load(object sender, EventArgs e)
        {


            transitionTable = new Dictionary<int, Dictionary<char, int>>
            {
                { 0, new Dictionary<char, int> { { 'a', 1 }, { 'b', 0 } } },  
                { 1, new Dictionary<char, int> { { 'a', 1 }, { 'b', 2 } } },  
                { 2, new Dictionary<char, int> { { 'a', 1 }, { 'b', 3 } } },
                { 3, new Dictionary<char, int> { { 'a', 3 }, { 'b', 3} } },   
               
            };

         

            Button circularButton = button1;
            Button circularButton2 = button2;
            Button circularButton3 = button3;
            Button circularButton4 = button4;



            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;

            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, circularButton.Width, circularButton.Height);
            circularButton.Region = new Region(path);

            System.Drawing.Drawing2D.GraphicsPath path2 = new System.Drawing.Drawing2D.GraphicsPath();
            path2.AddEllipse(0, 0, circularButton2.Width, circularButton2.Height);
            circularButton2.Region = new Region(path2);

            System.Drawing.Drawing2D.GraphicsPath path3 = new System.Drawing.Drawing2D.GraphicsPath();
            path3.AddEllipse(0, 0, circularButton3.Width, circularButton3.Height);
            circularButton3.Region = new Region(path3);


            System.Drawing.Drawing2D.GraphicsPath path4 = new System.Drawing.Drawing2D.GraphicsPath();
            path4.AddEllipse(0, 0, circularButton4.Width, circularButton4.Height);
            circularButton4.Region = new Region(path4);



        }


        private async Task switchStateAnimation(int state)
        {
            

            switch (state)
            {
                case 0:
                    button1.BackColor = Color.LightBlue; // Q0
                    break;
                case 1:
                    button2.BackColor = Color.LightBlue; // Q1
                    break;
                case 2:
                    button4.BackColor = Color.LightBlue; // Q2
                    break;
                case 3:
                    button3.BackColor = Color.LightBlue; // Q3
                    break;
                   
            }

           
            await Task.Delay(1500);

          
        }

        public void resetBtnColor()
        {
            button1.BackColor = Color.LightCoral;
            button2.BackColor = Color.LightCoral;
            button4.BackColor = Color.LightCoral;
            button3.BackColor = Color.Lime;
           
        }

        private async void checker(string regex)
        {
            int state = 0; // Starting at initial state Q0

            // Process each character in the input string using the transition table

            foreach (char ch in regex)
            {

               
                if (transitionTable[state].ContainsKey(ch))
                {
                    

                    state = transitionTable[state][ch];
                 
                    
                    await switchStateAnimation(state); 
                    resetBtnColor();
                    Console.WriteLine($"Input: {ch}");
                    label10.Text = "State: " + state;
                    Console.WriteLine($"State: Q{state}");
                }
                
                if (state == 3) 
                {
                    break;
                }
            }

          
            if (state == 3) 
            {
                MessageBox.Show("The string is accepted.");
            }
            else
            {
                MessageBox.Show("The string is not accepted.");
            }
        }




        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black, 2); // Define the color and thickness of the line

            // Get the center positions of each button
            Point button1Center = new Point(button1.Left + button1.Width / 2, button1.Top + button1.Height / 2);
            Point button2Center = new Point(button2.Left + button2.Width / 2, button2.Top + button2.Height / 2);
            Point button3Center = new Point(button3.Left + button3.Width / 2, button3.Top + button3.Height / 2);
            Point button4Center = new Point(button4.Left + button4.Width / 2, button4.Top + button4.Height / 2);

            // Draw straight lines between buttons for transitions
            g.DrawLine(pen, button1Center, button2Center); // Transition from button1 to button2
            DrawArrow(g, pen, button1Center, button2Center); // Add arrowhead

            g.DrawLine(pen, button2Center, button3Center); // Transition from button2 to button3
            DrawArrow(g, pen, button2Center, button3Center); // Add arrowhead

            // Draw loops around Q1, Q2, and Q3
            Rectangle loopRectQ1 = new Rectangle(button1.Left, button1.Top - 30, 40, 40);
            Rectangle loopRectQ2 = new Rectangle(button2.Left, button2.Top - 30, 40, 40);
            Rectangle loopRectQ3 = new Rectangle(button3.Left, button3.Top - 30, 40, 40);

            g.DrawArc(pen, loopRectQ1, 0, 360);
            DrawLoopArrow(g, pen, loopRectQ1, 270); // Arrowhead for loop around Q1

            g.DrawArc(pen, loopRectQ2, 0, 360);
            DrawLoopArrow(g, pen, loopRectQ2, 270); // Arrowhead for loop around Q2

            g.DrawArc(pen, loopRectQ3, 0, 360);
            DrawLoopArrow(g, pen, loopRectQ3, 270); // Arrowhead for loop around Q3

            // Draw a curved line (Bezier curve) from Q2 (button2) to Q1 (button1)
            Point controlPoint1 = new Point(button4Center.X - 50, button2Center.Y - 50);
            Point controlPoint2 = new Point(button2Center.X + 50, button2Center.Y - 50);
            g.DrawBezier(pen, button4Center, controlPoint1, controlPoint2, button2Center);
            DrawArrow(g, pen, controlPoint2, button2Center); // Arrowhead for curved line

            pen.Dispose();
        }

        private void DrawArrow(Graphics g, Pen pen, Point start, Point end)
        {
            const int arrowSize = 7;
            float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            PointF arrowPoint1 = new PointF(
                end.X - arrowSize * (float)Math.Cos(angle - Math.PI / 6),
                end.Y - arrowSize * (float)Math.Sin(angle - Math.PI / 6));
            PointF arrowPoint2 = new PointF(
                end.X - arrowSize * (float)Math.Cos(angle + Math.PI / 6),
                end.Y - arrowSize * (float)Math.Sin(angle + Math.PI / 6));
            g.DrawLine(pen, end, arrowPoint1);
            g.DrawLine(pen, end, arrowPoint2);
        }

        private void DrawLoopArrow(Graphics g, Pen pen, Rectangle loopRect, int angle)
        {
            const int arrowSize = 5;
            float arrowAngle = angle * (float)Math.PI / 500;
            PointF arrowPoint = new PointF(
                loopRect.Right - 3,
                loopRect.Top + 25
            ); 

            PointF arrowPoint1 = new PointF(
                arrowPoint.X - arrowSize * (float)Math.Cos(arrowAngle - Math.PI / 6),
                arrowPoint.Y - arrowSize * (float)Math.Sin(arrowAngle - Math.PI / 6)
            );
            PointF arrowPoint2 = new PointF(
                arrowPoint.X - arrowSize * (float)Math.Cos(arrowAngle + Math.PI / 6),
                arrowPoint.Y - arrowSize * (float)Math.Sin(arrowAngle + Math.PI / 6)
            );

            g.DrawLine(pen, arrowPoint, arrowPoint1);
            g.DrawLine(pen, arrowPoint, arrowPoint2);
        }





        private void btnValidate_Click(object sender, EventArgs e)
        {
            string str = input.Text;
            checker(str);
        }

        private void label1_Click(object sender, EventArgs e)
        {
          

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
