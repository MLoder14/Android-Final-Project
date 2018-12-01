using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Timers;
using Android.Graphics;

namespace TheTimer
{
    /// <summary>
    /// different alarm headers
    /// </summary>
    public enum TimerType
    {
        WORKTIME,
        SHORTBREAK,
        LONGBREAK
    }

    /// <summary>
    /// Timer is either running or not running, keep track of which
    /// </summary>
    public enum TimerState
    {
        RUNNING,
        STOPPED
    }

    [Activity(Label = "Timer", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        /// <summary>
        /// instantance variables we will need
        /// </summary>
        private int shortBreak = 300;

        private int longBreak = 900;

        private int quarterHour = 1500;

        private int countseconds = 1500;

        private int longBreakInterval = 3;

        private int currentPomodoro = 1;

        private int totalBreak = 1;

        private TimerType currentTimer = TimerType.WORKTIME;
        private TimerState timerState = TimerState.STOPPED;

        private Timer timer = new Timer();
        //private Bundle bundle;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner1);

            //spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(ItemSelected);
            //var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.colors, Android.Resource.Layout.SimpleSpinnerItem);

            //adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            //spinner.Adapter = adapter;

            //var rl = new RelativeLayout(this);
            //var ll = new LinearLayout(this);

            //var layoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            //ll.LayoutParameters = layoutParams;

            //var surfaceOrientation = WindowManager.DefaultDisplay.Rotation;


            //Sets the timer for 1 Second
            timer.Interval = 1000;
            timer.Elapsed += TimerElapsedEvent;
            SetContentView(Resource.Layout.activity_main);

            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner1);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.colors, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            Button button = FindViewById<Button>(Resource.Id.StartButton);
            TextView timerText = FindViewById<TextView>(Resource.Id.TimerTextView);
            timerText.Text = secondsToCountdown();
            button.Click += delegate { TimerButtonClicked(); };
        }

        /*
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutAll(outState);
        }
        */

        private void ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            TextView timerText = FindViewById<TextView>(Resource.Id.TimerTextView);
            Spinner spinner = (Spinner)sender;
            string toast = string.Format("The Color is {0}", spinner.GetItemAtPosition(e.Position));
            if (toast.Contains("yellow"))
            {
                timerText.SetTextColor(Color.Yellow);
            }
            if (toast.Contains("blue"))
            {
                timerText.SetTextColor(Color.CornflowerBlue);
            }
            if (toast.Contains("green"))
            {
                timerText.SetTextColor(Color.ForestGreen);
            }
            if (toast.Contains("white"))
            {
                timerText.SetTextColor(Color.White);
            }
            if (toast.Contains("orange"))
            {
                timerText.SetTextColor(Color.Orange);
            }
            if (toast.Contains("pink"))
            {
                timerText.SetTextColor(Color.HotPink);
            }
            if (toast.Contains("red"))
            {
                timerText.SetTextColor(Color.OrangeRed);
            }

            Toast.MakeText(this, toast, ToastLength.Long).Show();
            //timerText.SetTextColor(Color.Blue);
            //Toast.MakeText(this, "Your choose : " + spinner.GetItemAtPosition(e.Position), ToastLength.Short).Show();
        }
        

        private void TimerButtonClicked()
        {
            if (timerState == TimerState.STOPPED)
            {
                timer.Start();
                timerState = TimerState.RUNNING;
            }
            else
            {
                timer.Stop();
                timerState = TimerState.STOPPED;
                GetNextTimer();
            }
            RunOnUiThread(SetTimerButtonText);
        }
        private void TimerElapsedEvent(object sender, ElapsedEventArgs e)
        {
            countseconds--;
            RunOnUiThread(DisplaySeconds);
            if (countseconds == 0)
            {
                timer.Stop();
                GetNextTimer();
                timer.Start();
            }
        }

        /// <summary>
        /// Sorts out what timer should be next.
        /// </summary>
        private void GetNextTimer()
        {
            TextView timerText = FindViewById<TextView>(Resource.Id.TimerTextView);
           //LinearLayout linearlayout = FindViewById<LinearLayout>(Resource.Id.LinearLayout1);
            //RelativeLayout relativelayout = FindViewById<RelativeLayout>(Resource.Id.RelativeLayout1);

            switch (currentTimer)
            {
                case TimerType.WORKTIME:
                    currentPomodoro++;

                    if ((totalBreak % longBreakInterval) == 0)
                    {
                        currentTimer = TimerType.LONGBREAK;
                        countseconds = longBreak;
                    }
                    else
                    {
                        currentTimer = TimerType.SHORTBREAK;
                        countseconds = shortBreak;
                    }
                    break;
                case TimerType.SHORTBREAK:
                    totalBreak++;
                    currentTimer = TimerType.WORKTIME;
                    countseconds = quarterHour;
                    timerText.SetBackgroundColor(Color.Blue);
                    //linearlayout.SetBackgroundColor(Color.MidnightBlue);
                    //relativelayout.SetBackgroundColor(Color.MidnightBlue);

                    break;
                case TimerType.LONGBREAK:
                     totalBreak++;
                    currentTimer = TimerType.WORKTIME;
                    countseconds = quarterHour;
                    timerText.SetBackgroundColor(Color.Blue);
                    //linearlayout.SetBackgroundColor(Color.MidnightBlue);
                    //relativelayout.SetBackgroundColor(Color.MidnightBlue);
                    break;
            }
            RunOnUiThread(SetTimerBackground);
        }
        private string secondsToCountdown()
        {
            int mins = countseconds / 60;
            int seconds = countseconds - mins * 60;
            return string.Format("{0}:{1}", mins.ToString("00"), seconds.ToString("00"));
        }

        private void DisplaySeconds()
        {
            TextView timerText = FindViewById<TextView>(Resource.Id.TimerTextView);
            timerText.Text = secondsToCountdown();
        }

        private void SetTimerBackground()
        {
            //LinearLayout linearlayout = FindViewById<LinearLayout>(Resource.Id.LinearLayout1);
            //RelativeLayout relativelayout = FindViewById<RelativeLayout>(Resource.Id.RelativeLayout1);
            TextView timerStatus = FindViewById<TextView>(Resource.Id.TimerStatus);
            timerStatus.Text = currentTimer.ToString();
            TextView timerText = FindViewById<TextView>(Resource.Id.TimerTextView);

            if (currentTimer == TimerType.WORKTIME)
            {
                timerText.SetBackgroundColor(Color.Red);

            }
            else
            {
                //relativelayout.SetBackgroundColor(Color.MidnightBlue);
                //linearlayout.SetBackgroundColor(Color.MidnightBlue);
                timerText.SetBackgroundColor(Color.Blue);

            }

        }

        private void SetTimerButtonText()
        {
            DisplaySeconds();
            Button timerButton = FindViewById<Button>(Resource.Id.StartButton);
            if (timerState == TimerState.STOPPED)
            {
                timerButton.Text = "Start Timer";
            }
            else
            {
                timerButton.Text = "Stop Timer";
            }
        }
    }
}

