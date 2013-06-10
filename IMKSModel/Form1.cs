﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace IMKSModel
{
    public partial class Form1 : Form
    {



            Excel.Application excelapp;
            Excel.Workbook excelappworkbook;
            Excel.Workbooks excelappworkbooks;
            Excel.Sheets excelsheets;
            Excel.Worksheet excelworksheet;
            Excel.Range excelcells_a1;
            Excel.Range excelcells;







        public struct Source
        {
            public int id;
            public double x, y;
            public double radius;
        }

        public int sourcesCount = 8;
        public Source[] sources = new Source[8];




        public struct Receiver
        {
            public int id;
            public double x, y;
            public double radius;
            public double innerRadius;
        }

        public int receiversCount = 8;
        public Receiver[] receivers = new Receiver[8];



        int logicalCount;
            double busRadius = 1.5;         //  Радиус шины


        public struct Channel
        {
            public int id;
            public double x, y;
            public double radius;
            public bool isOk;
        }

        public int channelsCount = 0;
        public Channel[] channels = new Channel[5000];
        public int channelsToBreakCount = 0;
        public Channel[] channelsToBreak = new Channel[5000];





        public Form1()
        {
            InitializeComponent();



            excelapp = new Excel.Application();
            excelapp.Visible = true;
            excelapp.SheetsInNewWorkbook = 3;
            excelapp.Workbooks.Add(Type.Missing);
            excelapp.DisplayAlerts = true;
            excelappworkbooks = excelapp.Workbooks;
            excelappworkbook = excelappworkbooks[1];
            excelsheets = excelappworkbook.Worksheets;
            excelworksheet = (Excel.Worksheet)excelsheets.get_Item(1);

            excelcells_a1 = excelworksheet.get_Range("A1", Type.Missing);

            excelcells = excelcells_a1.get_Offset(0, 0);




            ///////////////////////////
            //////  Источники   ///////  
            ///////////////////////////


            double sourceRadius = 0.2;

            sources[0].id = 0;
            sources[0].x = 0.27;
            sources[0].y = 2.00;
            sources[0].radius = sourceRadius;

            sources[1].id = 1;
            sources[1].x = 1.10;
            sources[1].y = 2.81;
            sources[1].radius = sourceRadius;

            sources[2].id = 2;
            sources[2].x = 2.22;
            sources[2].y = 2.64;
            sources[2].radius = sourceRadius;

            sources[3].id = 3;
            sources[3].x = 2.80;
            sources[3].y = 1.61;
            sources[3].radius = sourceRadius;

            sources[4].id = 4;
            sources[4].x = 2.39;
            sources[4].y = 0.51;
            sources[4].radius = sourceRadius;

            sources[5].id = 5;
            sources[5].x = 1.30;
            sources[5].y = 0.13;
            sources[5].radius = sourceRadius;

            sources[6].id = 6;
            sources[6].x = 0.36;
            sources[6].y = 0.81;
            sources[6].radius = sourceRadius;

            sources[7].id = 7;
            sources[7].x = 1.49;
            sources[7].y = 1.50;
            sources[7].radius = sourceRadius;




            ///////////////////////////
            //////  Приёмники   ///////  
            ///////////////////////////


            double receiverRadius = 0.23;
            double receiverInnerRadius = 0.13;

            receivers[0].id = 0;
            receivers[0].x = 0.23;
            receivers[0].y = 1.97;
            receivers[0].radius = receiverRadius;
            receivers[0].innerRadius = receiverInnerRadius;

            receivers[1].id = 1;
            receivers[1].x = 1.08;
            receivers[1].y = 2.76;
            receivers[1].radius = receiverRadius;
            receivers[1].innerRadius = receiverInnerRadius;

            receivers[2].id = 2;
            receivers[2].x = 2.23;
            receivers[2].y = 2.62;
            receivers[2].radius = receiverRadius;
            receivers[2].innerRadius = receiverInnerRadius;

            receivers[3].id = 3;
            receivers[3].x = 2.82;
            receivers[3].y = 1.63;
            receivers[3].radius = receiverRadius;
            receivers[3].innerRadius = receiverInnerRadius;

            receivers[4].id = 4;
            receivers[4].x = 2.44;
            receivers[4].y = 0.54;
            receivers[4].radius = receiverRadius;
            receivers[4].innerRadius = receiverInnerRadius;

            receivers[5].id = 5;
            receivers[5].x = 1.31;
            receivers[5].y = 0.2;
            receivers[5].radius = receiverRadius;
            receivers[5].innerRadius = receiverInnerRadius;

            receivers[6].id = 6;
            receivers[6].x = 0.35;
            receivers[6].y = 0.81;
            receivers[6].radius = receiverRadius;
            receivers[6].innerRadius = receiverInnerRadius;

            receivers[7].id = 7;
            receivers[7].x = 1.49;
            receivers[7].y = 1.50;
            receivers[7].radius = receiverRadius;
            receivers[7].innerRadius = receiverInnerRadius;




            ////////////////////////
            //////  Каналы   ///////  
            ////////////////////////

            double x, y, d;
            double dx = 0.0429;             //  Шаг по x для гексагональной разметки
            double dy = 0.0357;             //  Шаг по y для гексагональной разметки
            double x0 = 1.5;                //  Центр шины - x
            double y0 = 1.5;                //  Центр шины - y
            double channelRaduis = 0.021;    //  Радиус одного канала


            int y_step = 0;
            y = 0;
            while (y < busRadius * 2)
            {

                //  Для гексагональной разметки координаты по x на каждом уровне смещаем по-разному
                if (y_step % 2 == 0)
                {
                    x = 0;
                }
                else
                {
                    x = dx / 2;
                }


                while (x < busRadius * 2)
                {
                    d = Math.Sqrt((x0 - x) * (x0 - x) + (y0 - y) * (y0 - y));
                    if (d < busRadius)
                    {
                        //  Канал в шине (точка в круге)

                        channels[channelsCount].x = x;
                        channels[channelsCount].y = y;
                        channels[channelsCount].radius = channelRaduis;
                        channels[channelsCount].isOk = true;

                        channelsCount++;
                    }


                    x = x + dx;
                }

                y = y + dy;
                y_step++;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void Commutation(double breakPercent, double dz, int ExcelCol)
        {

            double d, intens, d_intens, d_zatyx, receiverIntens;
            int channelsInSourceCount, channelsInReceiver;

            int drawK = 1000;

            logicalCount = 0;

            label2.Text = "";
            label3.Text = "";

            int ExcelRow = 1;

            for (int i = 0; i < sourcesCount; i++)
            {
                channelsInSourceCount = 0;
                channelsInReceiver = 0;
                intens = 0;
                receiverIntens = 0;
                

                if (i == 0)
                {
                    System.Drawing.Pen myPen;
                    myPen = new System.Drawing.Pen(System.Drawing.Color.Blue, 5);

                    System.Drawing.Graphics formGraphics = this.CreateGraphics();
                    Rectangle rect = new Rectangle(Convert.ToInt32((sources[i].x - sources[i].radius) * drawK),
                                                    Convert.ToInt32((sources[i].y - sources[i].radius) * drawK - 1750),
                                                    Convert.ToInt32(sources[i].radius * 2 * drawK),
                                                    Convert.ToInt32(sources[i].radius * 2 * drawK));
                    formGraphics.DrawEllipse(myPen, rect);
                    myPen.Dispose();
                    formGraphics.Dispose();
                }





                for (int j = 0; j < channelsCount; j++)
                {
                    if (channels[j].isOk)
                    {

                        d = Math.Sqrt((sources[i].x - channels[j].x) * (sources[i].x - channels[j].x) + (sources[i].y - channels[j].y) * (sources[i].y - channels[j].y));



                        if (d < sources[i].radius + dz * Math.Tan(Math.PI/8))
                        {
                            channelsInSourceCount++;
//                            intens = intens - 421 * d + 104;

                            d_intens = 0;
/*
                            if (d < 0.05)
                            {
                                d_intens = 100;
                            }
                            else if ((d >= 0.05) && (d < 0.1))
                            {
                                d_intens = 100;
                            }
                            else if ((d >= 0.1) && (d < 0.15))
                            {
                                d_intens = 100;
                            }
                            else if ((d >= 0.15) && (d < 0.2))
                            {
                                d_intens = 100;
                            }
                            else if ((d >= 0.2) && (d < 0.25))
                            {
                                d_intens = 100;
                            }
                            else if ((d >= 0.25) && (d < 0.3))
                            {
                                d_intens = 100;
                            }
                            else if ((d >= 0.3) && (d < 0.35))
                            {
                                d_intens = 100;
                            }
                            else if ((d >= 0.35) && (d < 0.40))
                            {
                                d_intens = 100;
                            }
                            else if (d >= 0.40)
                            {
                                d_intens = 0;
                            }

 */

                            if (d <= 0.5)
                            {
                                d_intens = Math.Sqrt(-200 * (d - 0.5));
                            }
                            else
                            {
                                d_intens = 0;
                            }
  
/*
                            d_zatyx = 1;

                            if (dz < 0.05)
                            {
                                d_zatyx = 1;
                            }
                            else if ((dz >= 0.05) && (dz < 0.1))
                            {
                                d_zatyx = 0.9;
                            }
                            else if ((dz >= 0.1) && (dz < 0.15))
                            {
                                d_zatyx = 0.8;
                            }
                            else if ((dz >= 0.15) && (dz < 0.2))
                            {
                                d_zatyx = 0.7;
                            }
                            else if ((dz >= 0.2) && (dz < 0.25))
                            {
                                d_zatyx = 0.6;
                            }
                            else if ((dz >= 0.25) && (dz < 0.3))
                            {
                                d_zatyx = 0.5;
                            }
                            else if (dz >= 0.3)
                            {
                                d_zatyx = 0.4;
                            }
*/

                            if (dz <= 0.6)
                            {
                                d_zatyx = Math.Sqrt(-1.66 * (dz - 0.6));
                            }
                            else
                            {
                                d_zatyx = 0;
                            }

                            intens = intens + d_intens * d_zatyx;

//                            intens = intens + (18.42 * d * d - 8.60 * d + 1.08) * 100;

                            if (i == 0)
                            {
                                System.Drawing.Pen myPen;
                                myPen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(255, 0, 0));

                                System.Drawing.Color color = new System.Drawing.Color();

                                if (d_intens != 0)
                                {
                                    color = System.Drawing.Color.FromArgb(Convert.ToInt16(2.55 * d_intens), 0, 0);
                                }
                                else
                                {
                                    color = System.Drawing.Color.FromArgb(0, 255, 0);
                                }

                                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(color);


                                System.Drawing.Graphics formGraphics = this.CreateGraphics();
                                Rectangle rect = new Rectangle( Convert.ToInt32((channels[j].x - channels[j].radius) * drawK),
                                                                Convert.ToInt32((channels[j].y - channels[j].radius) * drawK - 1750),
                                                                Convert.ToInt32(channels[j].radius * 2 * drawK),
                                                                Convert.ToInt32(channels[j].radius * 2 * drawK));
                                formGraphics.FillEllipse(myBrush, rect);
                                myPen.Dispose();
                                formGraphics.Dispose();
                            }


/*
                            //  Проверяем попадание в приёмник
                            for (int k = 0; k < receiversCount; k++)
                            {
                                d = Math.Sqrt((receivers[k].x - channels[j].x) * (receivers[k].x - channels[j].x) + (receivers[k].y - channels[j].y) * (receivers[k].y - channels[j].y));

                                if ((d < receivers[k].radius) && (d > receivers[k].innerRadius))
                                {
                                    channelsInReceiver++;

                                    receiverIntens = receiverIntens + (18.42 * d * d - 8.60 * d + 1.08) * 100;
                                }


                            }
*/

                            
                        }
                    }

                    else
                    {

                        d = Math.Sqrt((sources[i].x - channels[j].x) * (sources[i].x - channels[j].x) + (sources[i].y - channels[j].y) * (sources[i].y - channels[j].y));

                        if (d < sources[i].radius)
                        {


                            if (i == 0)
                            {
                                System.Drawing.Pen myPen;
                                myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
                                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.BlueViolet);
                                System.Drawing.Graphics formGraphics = this.CreateGraphics();
                                Rectangle rect = new Rectangle(Convert.ToInt32((channels[j].x - channels[j].radius) * drawK),
                                                                Convert.ToInt32((channels[j].y - channels[j].radius) * drawK - 1750),
                                                                Convert.ToInt32(channels[j].radius * 2 * drawK),
                                                                Convert.ToInt32(channels[j].radius * 2 * drawK));
                                formGraphics.FillEllipse(myBrush, rect);
                                myPen.Dispose();
                                formGraphics.Dispose();
                            }
                        }
                    }


                }

                if (channelsInSourceCount > 0)
                {
                    logicalCount++;

//                    label2.Text = label2.Text + channelsInSourceCount.ToString() + "/" + Math.Round(intens).ToString() + "; ";
//                    label3.Text = label3.Text + channelsInReceiver.ToString() + "/" + Math.Round(receiverIntens).ToString() + "; ";


                    excelcells = excelcells_a1.get_Offset(ExcelRow, ExcelCol);
                    excelcells.Value2 = Math.Round(intens).ToString();


                    ExcelRow++;
                }



            }


        }


        private void button1_Click(object sender, EventArgs e)
        {
            Commutation(0, 0, 0);
        }


        private bool isPointInSector(double x, double y, double r, double phi)
        {
            if (y >= 0)
            {

                if (((x * x + y * y) <= r * r) && (phi >= Math.Atan2(y, x)))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {

                if (((x * x + y * y) <= r * r) && (phi >= (2*Math.PI + Math.Atan2(y, x))))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            Random rand = new Random();

            int test = 1;

            double breakPercent = 0.1;
            int channelToBreak;
            int breakCount = Convert.ToInt32(Math.Round(channelsCount * breakPercent));
            double commonBreakPercent;



            if (test == 1)
            {
                commonBreakPercent = 0;
                breakPercent = 0.01;
                int k = 0;

                //  Убираем каналы до тех пор, пока процент выбывших не превысит 90
                while (commonBreakPercent < 90)
                {
                    //  Делаем коммутацию, считаем уровень засветки и выводим в Ексель
                    Commutation(commonBreakPercent, 0, k);

                    //  Считаем сколько каналов нужно убрать на следующем шаге
                    breakCount = Convert.ToInt32(Math.Round(channelsCount * breakPercent));

                    //  Убираем необходимое количество каналов
                    for (int i = 0; i < breakCount; i++)
                    {
                        channelToBreak = rand.Next(channelsCount);
                        while (channels[channelToBreak].isOk == false)
                        {
                            channelToBreak = rand.Next(channelsCount);
                        }

                        channels[channelToBreak].isOk = false;

                    }


                    //  Считаем сколько каналов в итоге убрали
                    breakCount = 0;
                    for (int i = 0; i < channelsCount; i++)
                    {
                        if (channels[i].isOk == false)
                        {
                            breakCount++;
                        }

                    }
                    commonBreakPercent = Math.Round(Convert.ToDouble(breakCount) / channelsCount * 100);


                    k++;


                }
            }



            if (test == 2)
            {
                double phi = Math.PI;


                //  Отбираем в массив channelsBreak все каналы из channels, входящие в заданный сектор phi
                //  Это те каналы, которые могут быть подвержены рандомному повреждению
                channelsToBreakCount = 0;
                for (int i = 0; i < channelsCount; i++)
                {
                    if (isPointInSector(channels[i].x - busRadius, channels[i].y - busRadius, busRadius, phi))
                    {
                        channelsToBreak[channelsToBreakCount] = channels[i];
                        channelsToBreak[channelsToBreakCount].id = i;
                        channelsToBreak[channelsToBreakCount].isOk = true;
                        channelsToBreakCount++;
                    }
                }



                if (channelsToBreakCount < breakCount)
                {
                    //  Если кол-во каналов, которые могут быть подвержены рандомному повреждению меньше, чем кол-во каналов,
                    //  которые юзер определил процентом от общего кол-ва каналов, то выходит, что нужно повредить все каналы из возможных

                    for (int i = 0; i < channelsToBreakCount; i++)
                    {
                        channels[channelsToBreak[i].id].isOk = false;
                    }
                }
                else
                {

                    for (int i = 0; i < breakCount; i++)
                    {
                        channelToBreak = rand.Next(channelsToBreakCount);
                        while (channelsToBreak[channelToBreak].isOk == false)
                        {
                            channelToBreak = rand.Next(channelsToBreakCount);
                        }

                        channels[channelToBreak].isOk = false;
                        channels[channelsToBreak[channelToBreak].id].isOk = false;

                    }

                }
            }




            if (test == 3)
            {

                int k = 0;
                for (int i = 0; i < 100; i++)
                {
                    //  Делаем коммутацию, считаем уровень засветки и выводим в Ексель
                    Commutation(0, 0, k);
                    k++;
                    for (int j = 0; j < 8; j++)
                    {

                        sources[j].x = sources[j].x + 0.005;
//                        sources[j].y = 0.51;
                    }


                }

            }




            if (test == 4)
            {

                int k = 0;
                double dz = 0;
                for (int i = 0; i < 1000; i++)
                {
                    //  Делаем коммутацию, считаем уровень засветки и выводим в Ексель
                    Commutation(0, dz, k);
                    k++;
                    dz = dz + 0.001;


                }

            }




            breakCount = 0;
            for (int i = 0; i < channelsCount; i++)
            {
                if (channels[i].isOk == false)
                {
                    breakCount++;
                }

            }


            label1.Text = "Выбыло " + breakCount.ToString() + " из " + channelsCount.ToString() + " (" + Convert.ToInt32(Math.Round( Convert.ToDouble(breakCount)/channelsCount * 100)).ToString() + ")";
        }
    }
}