using System;
using System.Timers;
using Foundation;
using UIKit;

namespace LocationConnection
{
    public class CustomTap : UIGestureRecognizer
    {
        ChatListActivity context;
        UITableView table;
        ChatUserListCell selectedCell;
        public Timer startTimer;
        Timer endTimer;
        int timerMs= 100;
        public bool pressed;

        public CustomTap(ChatListActivity context, UITableView table) // (ChatUserListCell cell)
        {
            this.context = context;
            this.table = table;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt) //when table is still scrolling, only TouchesBegan is called, thus row will remain highlighted.
        {
            base.TouchesBegan(touches, evt);
            
            Console.WriteLine("TouchesBegan " + ChatListActivity.chatListScrolling);

            var location = LocationInView(table);
            var indexPath = table.IndexPathForRowAtPoint(location);

            if (!ChatListActivity.chatListScrolling)
            {
                selectedCell = (ChatUserListCell)table.CellAt(indexPath);
                startTimer = new Timer();
                startTimer.Interval = timerMs;
                startTimer.Elapsed += StartTimer_Elapsed;
                startTimer.Start();
            }
        }

        private void StartTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            startTimer.Stop();
            InvokeOnMainThread(() => { SetHighlighted(); });
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);
            Console.WriteLine("TouchesMoved " + ChatListActivity.chatListScrolling);
            //table.ScrollEnabled = true;

            if (startTimer != null && startTimer.Enabled)
            {
                Console.WriteLine("TouchesMoved stopping timer");
                startTimer.Stop();
            }
            else
            {
                Console.WriteLine("TouchesMoved restoring");
                SetNormal();
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            Console.WriteLine("TouchesEnded " + ChatListActivity.chatListScrolling);

            if (startTimer != null && startTimer.Enabled || pressed)
            {
                endTimer = new Timer();
                endTimer.Interval = timerMs;
                endTimer.Elapsed += EndTimer_Elapsed;
                endTimer.Start();

                Console.WriteLine("Ended index: " + table.IndexPathForCell(selectedCell).Row);

                Session.CurrentMatch = context.matchList[table.IndexPathForCell(selectedCell).Row];
                //IntentData.chatOnePageType = "chatList";
                CommonMethods.OpenPage("ChatOneActivity", 1);
            }
        }

        private void EndTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            endTimer.Stop();
            InvokeOnMainThread(() => { SetNormal(); });
        }

        private void SetHighlighted ()
        {
            UIView view = selectedCell.ContentView;
            bool active = selectedCell.activeCell;

            if (active)
            {
                view.BackgroundColor = UIColor.FromRGB(179, 146, 107);
            }
            else
            {
                view.BackgroundColor = UIColor.FromRGB(179, 127, 135);
            }
            pressed = true;
        }

        public void SetNormal ()
        {
            UIView view = selectedCell.ContentView;
            bool active = selectedCell.activeCell;

            if (active)
            {
                view.BackgroundColor = UIColor.FromRGB(246, 226, 187);

            }
            else
            {
                view.BackgroundColor = UIColor.FromRGB(255, 179, 190);
            }
            pressed = false;
        }
    }
}
