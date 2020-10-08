using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GPS_Reader
{
    
    delegate void ReceiveString( string str );

    public partial class Form1 : Form
    {
        NmeaInterpreter ni = new NmeaInterpreter();
        string nmeaReceivedString=string.Empty;
        event ReceiveString nmeaStringEvent = null;
        GPSToDatabase gdb = new GPSToDatabase();

        double gSpeed = 0;
        double gBearing=0;

        public Form1()
        {
            InitializeComponent();

            nmeaStringEvent += new ReceiveString(Form1_nmeaStringEvent);
            ni.PositionReceived += new NmeaInterpreter.PositionReceivedEventHandler(ni_PositionReceived);
            ni.SatelliteReceived += new NmeaInterpreter.SatelliteReceivedEventHandler(ni_SatelliteReceived);
            ni.DateTimeChanged += new NmeaInterpreter.DateTimeChangedEventHandler(ni_DateTimeChanged);
            ni.SpeedReceived += new NmeaInterpreter.SpeedReceivedEventHandler(ni_SpeedReceived);
            ni.BearingReceived += new NmeaInterpreter.BearingReceivedEventHandler(ni_BearingReceived);
        }

        void ni_BearingReceived(double bearing)
        {
            gBearing = bearing;
        }

        void ni_SpeedReceived(double speed)
        {
            gSpeed = speed;
        }

        void ni_DateTimeChanged(DateTime dateTime)
        {
            textBoxTime.Text = dateTime.ToLongDateString() + " " + dateTime.ToLongTimeString();

           gdb.WriteNavToDB(dateTime, ni.latitude, ni.longitude , gSpeed, gBearing);
        }

        void ni_SatelliteReceived(int pseudoRandomCode, int azimuth, int elevation, int signalToNoiseRatio)
        {
            //throw new NotImplementedException();
            Console.WriteLine("Sats {0} , {1} , {2} , {3} " , pseudoRandomCode , azimuth , elevation , signalToNoiseRatio);
        }

        void ni_PositionReceived(string latitude, string longitude)
        {
            string tmp = longitude;
            //gLatitude =  Convert.ToDouble(latitude);
            //gLongitude = Convert.ToDouble(longitude);

            textBoxLatitude.Text = latitude;
            textBoxLongitude.Text = longitude;


        }

        void Form1_nmeaStringEvent(string str)
        {
            textBoxNMEA.Text += str;
            lock (ni)
            {
                ni.Parse(str.Substring(0, str.Length - 2));
            }
        }


        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string st = serialPort1.ReadExisting();

            for (int x = 0; x < st.Length; x++)
            {
                nmeaReceivedString += st[x];

                if (st[x] == '\n')
                {
                    //Console.Write("Str= {0}" , nmeaReceivedString);
                   
                    if (nmeaStringEvent != null)
                    {
                        string newstr = nmeaReceivedString;
                        try
                        {
                            Invoke(nmeaStringEvent, newstr);
                        }
                        catch (Exception ex )
                        {
                            Console.WriteLine("failed {0}" , ex.Message);
                        }
                    }

                    nmeaReceivedString = string.Empty;
                }
            }
  
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            serialPort1.Open();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort1.Close();
        }
    }
}
