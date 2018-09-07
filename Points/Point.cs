using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Timers;
using Android.Graphics;

namespace BLE_Tracker.Points
{
    public class Point
    {
        public const int AliveTime = 10000;
        private const int AlfaValue = 255;
        public float X { get; private set; }
        public float Y { get; private set; }
        public Bitmap Bitmap { get; set; }
        public int Alfa { get; private set; }


        private Timer timer;
        public Point(float x, float y)
        {
            X = x;
            Y = y;
            Alfa = AlfaValue;

            timer = new Timer
            { Interval = AliveTime / AlfaValue, Enabled = true };

            timer.Elapsed += (s, e) =>
            {
                Alfa--;

                if(Alfa == 0)
                {
                    var result = PointCollection.Points.FirstOrDefault(p => p == this);

                    if (result != null)
                    {
                        PointCollection.DeletePoint(result);
                    }

                    timer.Stop();
                }

                
            };

            timer.Start();
        }
    }
}