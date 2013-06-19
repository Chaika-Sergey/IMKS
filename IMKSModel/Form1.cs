using System;
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
            public double intens;
        }

        public int receiversCount;
        public Receiver[] receivers = new Receiver[5000];



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


            double x, y, d, dx, dy, x0, y0;
            int y_step;



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
            /*
  
                        //  Вариант один к одному для ВОСЬМИ приёмников 

                        double receiverRadius = 0.23;
                        double receiverInnerRadius = 0.13;
                        receiversCount = 8;

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
            */

            //  Полтное гексагональное покрытие приёмниками

            x0 = 1.5;                //  Центр шины - x
            y0 = 1.5;                //  Центр шины - y
            /*
            dx = 0.46;             //  Шаг по x для гексагональной разметки
            dy = 0.42;             //  Шаг по y для гексагональной разметки
            double receiverRadius = 0.23;
            double receiverInnerRadius = 0.0;
            */
            dx = 0.46;             //  Шаг по x для гексагональной разметки
            dy = 0.42;             //  Шаг по y для гексагональной разметки
            double receiverRadius = 0.20;
            double receiverInnerRadius = 0.10;


            y_step = 0;
            y = 0;
            receiversCount = 0;
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

                        receivers[receiversCount].x = x;
                        receivers[receiversCount].y = y;
                        receivers[receiversCount].radius = receiverRadius;
                        receivers[receiversCount].innerRadius = receiverInnerRadius;

                        receiversCount++;
                    }


                    x = x + dx;
                }

                y = y + dy;
                y_step++;
            }







            ////////////////////////
            //////  Каналы   ///////  
            ////////////////////////

            dx = 0.0429;             //  Шаг по x для гексагональной разметки
            dy = 0.0357;             //  Шаг по y для гексагональной разметки
            x0 = 1.5;                //  Центр шины - x
            y0 = 1.5;                //  Центр шины - y
            double channelRaduis = 0.021;    //  Радиус одного канала


            y_step = 0;
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


        private int Commutation(double breakPercent, double dz, int ExcelCol)
        {

            double d, intens, d_intens, intens_rcv, d_intens_rcv, d_zatyx, receiverIntens;
            int channelsInSourceCount, channelsInReceiver, receiversInLogical;
            int logicalCount = 0;
            bool hasChannels;

            int drawK = 500;

            logicalCount = 0;

            label2.Text = "";
            label3.Text = "";

            int ExcelRow = 1;

            rtbLogChannels.Clear();
            string curLogChannel;




            for (int src_in_logChannel = 1; src_in_logChannel <= 7; src_in_logChannel++)
            {
<<<<<<< HEAD
                channelsInSourceCount = 0;
                channelsInReceiver = 0;
                intens = 0;
                intens_rcv = 0;
                receiverIntens = 0;
                
                


                //  Рисуем для наглядности первый источник
                /*
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
                */
=======
>>>>>>> 875870195bc3f64b06f5373ae18b49e47f8cef76

                int[] mass = new int[9];
                int p;
                string s = "";

                for (int t = 1; t <= src_in_logChannel; t++)  {
                    mass[t] = t;
                }

                p = src_in_logChannel;
                while (p >= 1)
                {
                    s = "";
                    for (int t = 1; t <= src_in_logChannel; t++)  {
                        s = s + mass[t].ToString() + " ";
                    }
                    richTextBox1.AppendText(s + "\n");

                    hasChannels = false;

                    int i;
                    int g = 0;
                    while (g < s.Length)
                    {
                        if (s[g] == ' ')
                        {
                            //
                        }
                        else
                        {
                            i = Convert.ToInt32(s[g].ToString()) - 1;

                            curLogChannel = i.ToString() + " - ";

                            receiversInLogical = 0;
                            channelsInSourceCount = 0;
                            channelsInReceiver = 0;
                            intens = 0;
                            intens_rcv = 0;
                            receiverIntens = 0;



                            for (int j = 0; j < receiversCount; j++)
                            {
                                receivers[j].intens = 0;
                            }

                            //  Рисуем для наглядности первый источник
                            /*
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
                            */



                            //  Для каждого источника перебираем все каналы
                            //  и ищем среди них задействованные этим источником
                            for (int j = 0; j < channelsCount; j++)
                            {

                                //  Если канал не сломан
                                if (channels[j].isOk)
                                {

                                    //  Считаем расстояние от середины источника до середины канала
                                    d = Math.Sqrt((sources[i].x - channels[j].x) * (sources[i].x - channels[j].x) + (sources[i].y - channels[j].y) * (sources[i].y - channels[j].y));


                                    //  Если расстояние от середины источника до середины канала меньше радиуса источника, значит этот канал задейстован
                                    //  dz * Math.Tan(Math.PI/8) -- это поправка на расстояние между каналов и источником по оси Z (чем оно больше, тем рассеяние больше и тем больше каналов задействуется)
                                    if (d < sources[i].radius + dz * Math.Tan(Math.PI / 8))
                                    {
                                        //  Считаем кол-во задейстованных каналов
                                        channelsInSourceCount++;

                                        //  Зависимость интенсивности сигнала от удалённости канала от центра источника
                                        //  Вид - парабола с осями, направленными влево, с вершиной в (0.5; 0)
                                        if (d <= 0.5)
                                        {
                                            d_intens = Math.Sqrt(-200 * (d - 0.5));
                                        }
                                        else
                                        {
                                            d_intens = 0;
                                        }


                                        //  Зависимость коэфа затухания сигнала от удалённости канала от источника по оси Z
                                        //  Вид - парабола с осями, направленными влево, с вершиной в (0.6; 0)
                                        if (dz <= 0.6)
                                        {
                                            d_zatyx = Math.Sqrt(-1.66 * (dz - 0.6));
                                        }
                                        else
                                        {
                                            d_zatyx = 0;
                                        }


                                        //  Суммарная интенсивность всех каналов для текущего источника
                                        intens = intens + d_intens * d_zatyx;


                                        //  Для наглядности рисуем канал
                                        /*
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
                                        */


                                        //  Проверяем попадание сигнала из текущего канала в какой-нибудь приёмник
                                        //  Для этого перебираем все приёмники
                                        for (int k = 0; k < receiversCount; k++)
                                        {

                                            //  Считаем расстояние от середины приёмника до середины канала
                                            d = Math.Sqrt((receivers[k].x - channels[j].x) * (receivers[k].x - channels[j].x) + (receivers[k].y - channels[j].y) * (receivers[k].y - channels[j].y));

                                            //  Если расстояние от середины приёмника до середины канала меньше внешнего и больше внутреннего радиусов приёмника, 
                                            //  значит этот канал задейстован
                                            if ((d < receivers[k].radius) && (d > receivers[k].innerRadius))
                                            {
                                                //  Кол-во задействованных каналов
                                                channelsInReceiver++;


                                                //  Зависимость интенсивности сигнала от удалённости канала от центра источника
                                                //  Вид - парабола с осями, направленными влево, с вершиной в (0.5; 0)
                                                if (d <= 0.5)
                                                {
                                                    d_intens_rcv = Math.Sqrt(-200 * (d - 0.5));
                                                }
                                                else
                                                {
                                                    d_intens_rcv = 0;
                                                }


                                                //  Суммарная интенсивность всех каналов на приёмнике для текущего источника
                                                intens_rcv = intens_rcv + d_intens_rcv;

                                                receivers[k].intens = receivers[k].intens + d_intens_rcv;



                                                //  Рисуем для наглядности приёмники

                                                if (i == 5)
                                                {
                                                    System.Drawing.Pen myPen;
                                                    myPen = new System.Drawing.Pen(System.Drawing.Color.Green, 5);

                                                    System.Drawing.Graphics formGraphics = this.CreateGraphics();
                                                    Rectangle rect = new Rectangle(Convert.ToInt32((receivers[k].x - receivers[k].radius) * drawK),
                                                                                    Convert.ToInt32((receivers[k].y - receivers[k].radius) * drawK),
                                                                                    Convert.ToInt32(receivers[k].radius * 2 * drawK),
                                                                                    Convert.ToInt32(receivers[k].radius * 2 * drawK));
                                                    formGraphics.DrawEllipse(myPen, rect);
                                                    myPen.Dispose();
                                                    formGraphics.Dispose();
                                                }


                                            }


                                        }



                                    }
                                }

                                else
                                {

                                    d = Math.Sqrt((sources[i].x - channels[j].x) * (sources[i].x - channels[j].x) + (sources[i].y - channels[j].y) * (sources[i].y - channels[j].y));

                                    if (d < sources[i].radius)
                                    {


                                        if (i == 5)
                                        {
                                            System.Drawing.Pen myPen;
                                            myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
                                            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.BlueViolet);
                                            System.Drawing.Graphics formGraphics = this.CreateGraphics();
                                            Rectangle rect = new Rectangle(Convert.ToInt32((channels[j].x - channels[j].radius) * drawK),
                                                                            Convert.ToInt32((channels[j].y - channels[j].radius) * drawK),
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
                                receiversInLogical = 0;

                                for (int j = 0; j < receiversCount; j++)
                                {
                                    if (receivers[j].intens >= 100)
                                    {
                                        curLogChannel = curLogChannel + j.ToString() + " (" + receivers[j].intens.ToString("0.00") + "); ";
                                        receiversInLogical++;
                                    }
                                }
                                rtbLogChannels.AppendText(curLogChannel + "\n");
                                Refresh();


                                if (receiversInLogical != 0)
                                {
                                    hasChannels = true;
                                }


                                //                    label2.Text = label2.Text + channelsInSourceCount.ToString() + "/" + Math.Round(intens).ToString() + "; ";
                                //                    label3.Text = label3.Text + channelsInReceiver.ToString() + "/" + Math.Round(receiverIntens).ToString() + "; ";


                                //                    excelcells = excelcells_a1.get_Offset(ExcelRow, ExcelCol);
                                //                    excelcells.Value2 = Math.Round(intens_rcv).ToString();

                                //                    excelcells = excelcells_a1.get_Offset(0, ExcelCol);
                                //                    excelcells.Value2 = receiversInLogical.ToString();

                                //                    ExcelRow++;
                            }

                        }



                        g++;
                    }


                    if (hasChannels)
                    {
                        logicalCount++;
                    }





                    if (mass[src_in_logChannel] == sourcesCount)
                    {
                        p--;
                    }
                    else
                    {
                        p = src_in_logChannel;
                    }

                    if (p >= 1)
                    {
                        for (int j = src_in_logChannel; j >= p; j--)
                        {
                            mass[j] = mass[p] + j - p + 1;
                        }
                    }
                }


            }

            return logicalCount;

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

            int logicalCount;

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
                    logicalCount = Commutation(commonBreakPercent, 0, k);

                    excelcells = excelcells_a1.get_Offset(0, k);
                    excelcells.Value2 = logicalCount.ToString();


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



            if (test == 5)
            {

                int k = 0;
                double a = 0;
                double x0, y0;
                x0 = 1.5;
                y0 = 1.5;

                double drawK = 100;

                while (a <= Math.PI*2)
                {
                    //  Делаем коммутацию, считаем уровень засветки и выводим в Ексель
                    Commutation(0, 0, k);
                    k++;
                    for (int j = 0; j < 8; j++)
                    {

                        sources[j].x = x0 + (sources[j].x - x0) * Math.Cos(0.01) - (sources[j].y - y0) * Math.Sin(0.01);
                        sources[j].y = y0 + (sources[j].y - y0) * Math.Cos(0.01) + (sources[j].x - x0) * Math.Sin(0.01);


                        if (j != 9)
                        {

                            System.Drawing.Pen myPen;
                            myPen = new System.Drawing.Pen(System.Drawing.Color.Blue, 5);

                            System.Drawing.Graphics formGraphics = this.CreateGraphics();
                            Rectangle rect = new Rectangle(Convert.ToInt32((sources[j].x - sources[j].radius) * drawK),
                                                            Convert.ToInt32((sources[j].y - sources[j].radius) * drawK),
                                                            Convert.ToInt32(sources[j].radius * 2 * drawK),
                                                            Convert.ToInt32(sources[j].radius * 2 * drawK));
                            formGraphics.DrawEllipse(myPen, rect);
                            myPen.Dispose();
                            formGraphics.Dispose();
                        }

                    }

                    a = a + 0.01;
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

        private void button3_Click(object sender, EventArgs e)
        {

            /*
              for i := 1 to k do
                A[i] := i; //Первое подмножество
              p := k;
              while p >= 1 do
              begin
                writeln(A[1],..., A[k]); //вывод очередного сочетания
                if A[k] = n then
                  p := p - 1
                else
                  p := k;
                if p >= 1 then
                  for i := k downto p do
                    A[i] := A[p] + i - p + 1;
              end;
            */


            int[] mass = new int[9];
            int n = 8;
            int k = 2;
            int p;
            string s = "";

            for (int i = 1; i <= k; i++)
            {
                mass[i] = i;
            }

            p = k;
            while (p >= 1)
            {
                s = "";
                for (int i = 1; i <= k; i++)
                {
                    s = s + mass[i].ToString() + " ";
                }
                richTextBox1.AppendText(s + "\n");

                if (mass[k] == n)
                {
                    p--;
                }
                else
                {
                    p = k;
                }

                if (p >= 1)
                {
                    for (int i = k; i >= p; i--)
                    {
                        mass[i] = mass[p] + i - p + 1;
                    }
                }
            }





        }
    }
}
