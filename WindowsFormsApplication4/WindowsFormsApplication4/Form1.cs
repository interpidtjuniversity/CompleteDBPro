using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlSugar;
using Newtonsoft.Json;
using Sugar.Enties;
using System.Threading;

namespace WindowsFormsApplication4
{

    public partial class Form1 : Form
    {
        public static bool locker = true;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            textBox1.ScrollToCaret();
            Actions a = new Actions();
            try
            {
                a.BeginTran();
                label1.Text = a.SystemTime.ToString();
                a.CommitTran();
            }
            catch (Exception ex)
            {
                a.RollbackTran();
                Console.WriteLine("school C");
                //输出0 ,没错虽然不在同一个dal里面但是 studentDal 成功回滚了 schoolDal的插入操作
            }


        }

        private void Start_button_Click(object sender, EventArgs e)
        {
            locker = true;
            Thread s = new Thread(new ThreadStart(simulater));
            s.Start();

        }
        private void Stop_button_Click(object sender, EventArgs e)
        {
            locker = false;
        }
        public void simulater()
        {

            do {
                
                dropcheck();
                customerActions();
                FlightDepart();
                
                Thread.Sleep(5000);
            } while (locker);
        }
         

         public void dropcheck()
        {

            Actions a = new Actions();

            try
            {
                a.BeginTran();

                List<FLIGHTIME> dropFlightList = a.searchDropFlights();
                foreach (var dropFlightime in dropFlightList)    //降落飞机
                {
                    string d_ID = dropFlightime.F_ID;       //降落飞机ID
                    char[] str = d_ID.ToCharArray();
                    DateTime systime = a.SystemTime;
                    TimeSpan rest = TimeSpan.FromHours(5);
                    DateTime ex_takeoftime_re = systime + rest;    //返回起飞时间
                    var time_length = dropFlightime.EX_LAND_TIME - dropFlightime.EX_TAKEOFF_TIME;
                    DateTime ex_landtime_re = ex_takeoftime_re + time_length;                                              //航班长度
                    string d_ID_re;   //返回航班ID
                    int temp = int.Parse(str[5].ToString());
                    if (temp % 2 == 0)
                    {
                        d_ID_re = d_ID.Substring(0, d_ID.Length - 1) + (temp + 1).ToString();
                    }
                    else
                    {
                        d_ID_re = d_ID.Substring(0, d_ID.Length - 1) + (temp - 1).ToString();
                    }
                    //现航班
                    try
                    {
                        a.BeginTran();
                        a.dealDropFlightime(dropFlightime);
                        FLIGHT dropFlight = a.getFlight(dropFlightime.F_ID);
                        dropFlight.CHECKED = 0;
                        dropFlight.BOARD = 0;
                        dropFlight.RESERVE = 0;
                        a.updateFlight(dropFlight);
                        a.delSeat(dropFlight.F_ID);
                        a.delLuggage(dropFlight.F_ID);
                        textBox1.Text += (dropFlight.F_ID + "  已降落\r\n");
                       
                        a.CommitTran();
                    }
                    catch
                    {
                        a.RollbackTran();
                        Console.WriteLine("dealDropFlight");
                    }

                    //返回航班
                    try
                    {
                        a.BeginTran();
                        a.dealDropFlight_return(ex_takeoftime_re, ex_landtime_re, d_ID_re);
                        textBox1.Text += (d_ID_re + "  返程航班已修改\r\n"); 
                        a.CommitTran();
                    }
                    catch
                    {
                        a.RollbackTran();
                        Console.WriteLine("dealDropFlight_return" + "  " +
                            d_ID_re + " " + systime.ToString("yyyy-MM-dd-hh-mm") + " " + ex_takeoftime_re.GetDateTimeFormats('t')[0].ToString() + " " + ex_landtime_re.GetDateTimeFormats('t')[0].ToString());
                    }
                    //乘客下机  行李下机 飞机初始化修改航班
                    try
                    {
                        a.BeginTran();
                        a.dropFlightUpdate(dropFlightime.F_ID, d_ID_re);
                        a.CommitTran();
                    }
                    catch
                    {
                        a.RollbackTran();
                        Console.WriteLine("乘客下机  行李下机 飞机初始化修改航班");
                    }
                }
                List<FLIGHTIME> newFlightimes = a.searchnewFlightimes();
                foreach(FLIGHTIME newFlightime in newFlightimes)
                {
                    try
                    {
                        a.BeginTran();
                        a.departFstate( newFlightime.F_ID);
                        textBox1.Text += (newFlightime.F_ID + "  待起飞\r\n");
                        a.CommitTran();
                    }
                    catch
                    {
                        a.RollbackTran();
                        Console.WriteLine("departFstate" + "  " +
                            newFlightime.F_ID + " " + newFlightime.EX_TAKEOFF_TIME.ToString("yyyy-MM-dd-hh-mm"));
                    }
                    //选出10名乘客购票

                    var CustomerList = a.selectFreeCustomers(10);
                    Console.WriteLine(CustomerList.Count);
                    foreach (var customer in CustomerList)
                    {
                        SEAT insertSeat = new SEAT();
                        insertSeat.F_ID = newFlightime.F_ID;
                        insertSeat.C_ID = customer.ID;
                        insertSeat.SEAT_NUMBER = null;
                        insertSeat.STATE = "已购票";
                        try
                        {
                            a.BeginTran();
                            a.InsertSeat(insertSeat);
                            textBox1.Text += (customer.ID.ToString() + "  已购票\r\n");
                            a.CommitTran();
                        }
                        catch
                        {
                            a.RollbackTran();
                            Console.WriteLine("insertSeat");
                        }
                       
                    }
                    FLIGHT newFlight = a.getFlight(newFlightime.F_ID);
                        newFlight.RESERVE = (short)CustomerList.Count;
                        a.updateFlight(newFlight);

                }
                a.CommitTran();
            }catch
            {
                a.RollbackTran();
            }
        }


        public void customerActions()
        {
            Actions a = new Actions();
            //办理登机牌

            var systime = a.SystemTime;
            var preCardtime = systime.AddHours(3);   //3小时前办理登机牌
            var preBoardtime = systime.AddMinutes(30); //30分钟前登机
            List<FLIGHTIME> preCardFlightimeList = a.searchPreCardFlights(preCardtime);
            foreach (var preCardFlightime in preCardFlightimeList) //办理登机的飞机
            {
                short count = 0;
                var preCardSeatList = a.searchPreCardSeats(preCardFlightime.F_ID);

                foreach (SEAT preCardSeat in preCardSeatList)
                {
                    bool find = false;
                    do
                    {
                        Random ran = new Random(DateTime.Now.Millisecond);
                        int RandKey = ran.Next(14) + 1;

                        //**********此处有问题 SEAT_NUMBER是string类型***********

                        //找座位是否被占用
                        bool found = a.judgeFoundSeat(preCardSeat.F_ID, RandKey.ToString());
                        if (!found)
                        {
                            preCardSeat.STATE = "已办理";
                            preCardSeat.SEAT_NUMBER = RandKey.ToString();
                            try
                            {
                                a.BeginTran();
                                a.updateSeat(preCardSeat);
                                string c_name = a.getCustomer(preCardSeat.C_ID).NAME;
                                textBox1.Text += (preCardSeat.F_ID + " " + c_name + " 已办理\r\n");
                                a.CommitTran();
                            }
                            catch
                            {
                                a.RollbackTran();
                                Console.WriteLine("updateSeat");
                            }


                            CUSTOMER customer = a.getCustomer(preCardSeat.C_ID);
                            if (customer.LUG_WEIGHT != null)
                            {     //这个顾客有行李

                                LUGGAGE luggage = new LUGGAGE();
                                luggage.F_ID = preCardSeat.F_ID;
                                luggage.L_ID = preCardSeat.C_ID;
                                luggage.STATE = "已办理";
                                luggage.WEGHT = (short)customer.LUG_WEIGHT;
                                a.InsertLuggage(luggage);
                                string c_name = a.getCustomer(luggage.L_ID).NAME;
                                textBox1.Text += (c_name + " 行李已办理\r\n");
                                
                            }
                            else    //没有行李，不用处理
                            { }

                            find = true;
                        }
                        else { }//座位已被占
                    } while (find != true);
                    count++;
                }           //修改飞机已办理信息
                var preCardFlight = a.getFlight(preCardFlightime.F_ID);
                preCardFlight.CHECKED += count;
                try
                {
                    a.BeginTran();
                    a.updateFlight(preCardFlight);
                    textBox1.Text += (preCardFlight.F_ID + " 信息已更新\r\n");
                    a.CommitTran();
                }
                catch
                {
                    a.RollbackTran();
                    Console.WriteLine("updateFlight");
                }


            }

            //要登机的航班
            var preBoardFlightimeList = a.GetpreBoardFlightimeList(preBoardtime);
            foreach (var preBoardFlightime in preBoardFlightimeList) //办理登机的飞机
            {
                short count = 0;
                var preBoardSeatList = a.GetpreBoardSeats(preBoardFlightime.F_ID);
                foreach (SEAT preBoardSeat in preBoardSeatList)
                {
                    preBoardSeat.STATE = "已登机";
                    string c_name = a.getCustomer(preBoardSeat.C_ID).NAME;
                    textBox1.Text += (c_name+ " 已登机\r\n");
                    count++;
                }
                if (preBoardSeatList.ToArray().Length != 0)
                {
                    a.updateSeat(preBoardSeatList);

                }

                var preBoardFlight = a.getFlight(preBoardFlightime.F_ID);
                preBoardFlight.BOARD += count;
                a.updateFlight(preBoardFlight);
                a.UpdateBoardLuggage(preBoardFlight.F_ID);

            }
        }
        public void FlightDepart()
        {

            Actions a = new Actions();
            var systime = a.SystemTime;
            var DepartFlightimeList = a.searchDepartFlightimes();
            List<FSTATE> DepartFlightsState = new List<FSTATE>();
            foreach (var DepartFlightime in DepartFlightimeList)//起飞航班
            {
                DepartFlightime.RE_TAKEOFF_TIME = systime;  //实际起飞时间
                FSTATE DepartFlightState = a.GetFstate(DepartFlightime.F_ID);
                DepartFlightState.STATE = "在空中";   //状态改为在空中
                textBox1.Text += (DepartFlightState.F_ID + " 起飞!\r\n");
                DepartFlightsState.Add(DepartFlightState);

            }
            if (DepartFlightimeList.ToArray().Length != 0)
            {
                a.updateFlightime(DepartFlightimeList);
                a.updateFstate(DepartFlightsState);

            }

            a.SystemTime = a.SystemTime.AddMinutes(30);
            label1.Text = a.SystemTime.ToString();
        }
    }
    
}