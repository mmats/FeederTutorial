using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.V7.Widget;
using FeederTutorial.Model;
using System;
using FeederTutorial.Common;
using Newtonsoft.Json;
using FeederTutorial.Adapter;
using Java.Lang;
using Android.Views;


/*
 https://www.youtube.com/watch?v=xGPjk2Mv5po
 29min
*/

namespace FeederTutorial
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        Android.Support.V7.Widget.Toolbar toolbar;
        public RecyclerView recyclerView;

        //private const string RSS_link = "https://www.heise.de/rss/heise-atom.xml";
        private const string RSS_link = "https://rss.golem.de/rss.php?feed=RSS2.0";
        private const string RSS_to_JSON = "https://api.rss2json.com/v1/api.json?rss_url=";

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Main_Menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menu_refresh)
                LoadData();
            return true;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            SetContentView(Resource.Layout.activity_main);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "News";
            SetSupportActionBar(toolbar);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            recyclerView.SetLayoutManager(linearLayoutManager);

            LoadData();
        }

        private void LoadData()
        {
            StringBuilder sb = new StringBuilder(RSS_to_JSON);
            sb.Append(RSS_link);

            new LoadDataAsync(this).Execute(sb.ToString());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    class LoadDataAsync : AsyncTask<string, string, string>
    {
        MainActivity mainActivity;

        //ProgressBar progressBar;

        //[Obsolete]
        //ProgressDialog mDialog;

        public LoadDataAsync(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
            //mDialog = new ProgressDialog(mainActivity);
        }

        protected override void OnPreExecute()
        {
            //mDialog.Window.SetType(Android.Views.WindowManagerTypes.SystemAlert);
            //mDialog.SetMessage("Please wait...");
            //mDialog.Show();
        }

        protected override string RunInBackground(params string[] @params)
        {
            string result = new HttpDataHandler().GetHttpData(@params[0]);
            return result;
        }

        protected override void OnPostExecute(string result)
        {
            RssObject data = JsonConvert.DeserializeObject<RssObject>(result);
            //mDialog.Dismiss();
            FeedAdapter adapter = new FeedAdapter(data, mainActivity);
            mainActivity.recyclerView.SetAdapter(adapter);
            adapter.NotifyDataSetChanged();
        }
    }
}