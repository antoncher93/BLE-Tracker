using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AltBeaconOrg.BoundBeacon;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BLE_Tracker.Views;
using SCAppLibrary.Android.Services.Telemetry;
using AlgorithmTest_Sharp;
using BLE_Tracker.Points;
using SCAppLibrary.Android.Services.Regions;

namespace BLE_Tracker.Activities
{
    [Activity(Label = "TraectoryActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class TraectoryActivity : Activity, IRanger
    {
        private SurfaceView view;
        private BeaconScanerService service;
        private AlgorithmTest_Sharp.Programm algoritm;
        private NotificationManager notifManager;

        private List<AlgorithmTest_Sharp.Beacon> found_beacons = new List<AlgorithmTest_Sharp.Beacon>();
        public static List<AlgorithmTest_Sharp.Beacon> Beacons { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActionBar.Hide();
            view = new TrackView(this);
            SetContentView(view);


            // Create your application here

            algoritm = new Programm();
            notifManager = (NotificationManager)GetSystemService(Context.NotificationService);
            service = new BeaconScanerService.Setter()
                .SetRegion(BeaconScanerService.RegionType.Common)
                .SetForegroundScanPeriod(500)
                .SetRangeNotifier(this)
                .Complete();

            StartService(new Intent(this, service.Class));
        }


        public override void OnBackPressed()
        {
            base.OnBackPressed();

            StopService(new Intent(this, service.Class));

            this.Finish();
        }

        public void DidRangeBeaconsInRegion(ICollection<AltBeaconOrg.BoundBeacon.Beacon> beacons, Region region)
        {
            foreach(var beac in beacons)
            {
                var search = found_beacons.FirstOrDefault(s => s.Mac == beac.BluetoothAddress);
                if(search != null)
                {
                    search.Rssi[0] = beac.Rssi; 
                }
                else
                {
                    search = new AlgorithmTest_Sharp.Beacon { Mac = beac.BluetoothAddress, Rssi = new List<double> { beac.Rssi } };
                    found_beacons.Add(search);
                }
            }

            IList<point_xyz> result = null;
            try
            {
                result = algoritm.calc_trajectory(found_beacons);
            }
            catch(Exception e)
            {
                RunOnUiThread(() => Toast.MakeText(this.ApplicationContext, e.Message, ToastLength.Short).Show());

                
            }

            Beacons = found_beacons;

            if(result != null && result.Count > 0)
            {
                PointCollection.AddPoint(new Point((float)result[0].X, (float)result[0].Y));

                var ts = TimeSpan.FromSeconds(DateTime.Now.TimeOfDay.TotalSeconds);
            }
            

            

        }
    }
}