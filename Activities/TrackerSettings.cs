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
using BLE_Tracker.Adapters;

namespace BLE_Tracker.Activities
{
    [Activity(Label = "Settings")]
    public class TrackerSettings : Activity, AdapterView.IOnItemSelectedListener
    {
        public const string SettingsName = "AppSettings";
        public const string ScaleSize = "ScaleSize";
        private Spinner spinner;
        ScaleItemAdapter adapter;
        private float[] data = { 10, 20, 30, 40, 50 };

        private ISharedPreferences mSettings;
        float scale = 50;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.settings);
            // Create your application here
            mSettings = GetSharedPreferences(SettingsName, FileCreationMode.Private);
            if(mSettings.Contains(ScaleSize))
            {
                scale = mSettings.GetFloat(ScaleSize, float.MaxValue);
            }
            
            spinner = FindViewById<Spinner>(Resource.Id.spinner_scale);
            spinner.OnItemSelectedListener = this;
            adapter = new ScaleItemAdapter(this, Resource.Layout.ScaleItem, data); 
            //ScaleAdapter adapter = new ScaleAdapter(this, data);
            spinner.Adapter = adapter;
            spinner.SetSelection(FindDataId(scale));
            
        }

        public override void OnBackPressed()
        {
            

            base.OnBackPressed();



            this.Finish();
        }

        public void OnItemSelected(AdapterView parent, View view, int position, long id)
        {
            scale = adapter[position];
            var editor = mSettings.Edit();
            editor.PutFloat(ScaleSize, scale);
            editor.Apply();
        }

        public void OnNothingSelected(AdapterView parent)
        {
            
        }

        private int FindDataId(double selection)
        {
            int id = 0;
            for(int i = 0; i < data.Length; i++)
            {
                if(data[i] == selection)
                {
                    id = i;
                    break;
                }
            }
            return id;
        }
    }
}