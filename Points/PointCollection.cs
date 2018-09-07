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

namespace BLE_Tracker.Points
{
    public static class PointCollection
    {
        private static List<Point> points = new List<Point>();
        public static List<Point> Points => points;
        public static void AddPoint(Point point)
        {
            points.Add(point);
        }
        public static void DeletePoint(Point point)
        {
            if(points.Remove(point)) System.Diagnostics.Debug.WriteLine("Delete Point: " + point.X + " " + point.Y);
        }

    }
}