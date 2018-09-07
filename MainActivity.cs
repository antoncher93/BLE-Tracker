using Android.App;
using Android.Widget;
using Android.OS;
using BLE_Tracker.Activities;
using BLE_Tracker.Points;
using AlgorithmTest_Sharp;
using Android.Views;

namespace BLE_Tracker
{
    [Activity(Label = "BLE_Tracker", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private Button button;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            button = FindViewById<Button>(Resource.Id.button1);

            button.Click += (s, e) =>
            {
                StartActivity(new Android.Content.Intent(this, new TraectoryActivity().Class));
            };

            

            //System.Timers.Timer timer = new System.Timers.Timer
            //{
            //    Interval = 1000,
            //    Enabled = true
            //};
            //int count = 0;
            //timer.Elapsed += (s, e) =>
            //{

            //    PointCollection.AddPoint(new Point(count, count));

            //    count++;
            //};

        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(0, 1, 0, "Settings");
            return base.OnCreateOptionsMenu(menu);
        }

        
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch(item.ItemId)
            {
                case 1:
                    StartActivity(new Android.Content.Intent(this, new TrackerSettings().Class));
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}

