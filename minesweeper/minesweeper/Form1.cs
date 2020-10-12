using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace minesweeper
{
    public class Field : Button
    {
        private Button button1;

        public int X { get; set; }
        public int Y { get; set; }

        public bool hasBeenVisited = false;

        public bool Bomb { get; set; }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.ResumeLayout(false);

        }
    }


    public partial class Form1 : Form
    {
        bool hasBeenClicked = false;
        Field[,] FieldButtons = new Field[20, 20];

        public Form1()
        {
            InitializeComponent();
            this.flowLayoutPanel1.Width = 400;
            this.flowLayoutPanel1.Height = 400;

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    FieldButtons[i, j] = new Field()
                    {
                        Width = 20,
                        Height = 20,
                        Margin = Padding.Empty,
                        X = i,
                        Y = j,
                    };
                    FieldButtons[i, j].Click += HandleButtonPress;
                    this.flowLayoutPanel1.Controls.Add(FieldButtons[i, j]);
                }
            }

        }

        public void HandleButtonPress(object sender, EventArgs e)
        {
            Random random = new Random();
            var field = (Field)sender;
            if(isInTheBoard(field.X, field.Y) == false || FieldButtons[field.X, field.Y].hasBeenVisited)
            {
                return;
            }
            FieldButtons[field.X, field.Y].hasBeenVisited = true;
            if (hasBeenClicked == false)
            {
                for (int i = 0; i < 50; i++)
                {
                    var randomX = random.Next(0, 20);
                    var randomY = random.Next(0, 20);

                    if (randomX == field.X && randomY == field.Y)
                    {
                        i--;
                        continue;
                    }
                    if (FieldButtons[randomX, randomY].Bomb == true)
                    {
                        i--;
                        continue;
                    }

                    FieldButtons[randomX, randomY].Bomb = true;
                }

                hasBeenClicked = true;
            }
            if (FieldButtons[field.X, field.Y].Bomb)
            {
                foreach (var item in FieldButtons)
                {
                    if (item.Bomb)
                    {
                        item.Text = "B";
                    }
                }
                MessageBox.Show("Boomm");
            }
            else
            {
                int counter = 0;

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            continue;
                        }
                        else
                        {
                            if (isInTheBoard(field.X + i, field.Y + j) == false)
                            {
                                continue;
                            }
                            else
                            {
                                if (FieldButtons[field.X + i, field.Y + j].Bomb)
                                {
                                    counter++;
                                }
                            }
                        }
                    }
                }

                if (counter == 0)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if(isInTheBoard(field.X + i, field.Y + j) == false)
                            {
                                continue;
                            }

                            HandleButtonPress(FieldButtons[field.X + i, field.Y + j], e);
                        }
                    }
                }

                FieldButtons[field.X, field.Y].Text = counter.ToString();
            }

        }

        private bool isInTheBoard(int x, int y)
            => x >= 0 && x < 20 && y >= 0 && y < 20;

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
