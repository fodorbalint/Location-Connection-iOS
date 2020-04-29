using System;
using System.Timers;
using Foundation;
using UIKit;

/* sequence for iPad
 020-04-28 23:36:34.728 locationconnection[1315:14103] 
-------------------------------------------------------------- ChatList TouchesBegan False {X=138, Y=51}
2020-04-28 23:36:34.728 locationconnection[1315:14103] 
Thread started:  #14
2020-04-28 23:36:34.828 locationconnection[1315:14103] 
-------------------------------------------------------------- TouchesEnded False {X=138, Y=51}
2020-04-28 23:36:34.828 locationconnection[1315:14103] 
2020-04-28 23:36:34.828 locationconnection[1315:14103] 
-------------------------------------------------------------- Ended index: 0
2020-04-28 23:36:34.828 locationconnection[1315:14103] 
2020-04-28 23:36:34.828 locationconnection[1315:14103] OpenPage currentContext <ChatListActivity: 0x109b65ba0> target ChatOneActivity anim 1 transitionRunning False
2020-04-28 23:36:34.844 locationconnection[1315:14103] 
-------------------------------------------------------------- ChatOneActivity ViewDidLoad
2020-04-28 23:36:34.844 locationconnection[1315:14103] 
2020-04-28 23:36:34.849 locationconnection[1315:14130] Notification authorization granted: True
2020-04-28 23:36:34.851 locationconnection[1315:14103] 
-------------------------------------------------------------- SetHighlighted
2020-04-28 23:36:34.851 locationconnection[1315:14103] 
2020-04-28 23:36:34.852 locationconnection[1315:14103] RegisteredForRemoteNotifications
2020-04-28 23:36:34.854 locationconnection[1315:14103] 
-------------------------------------------------------------- ChatListActivity ViewWillDisappear
2020-04-28 23:36:34.854 locationconnection[1315:14103] 
2020-04-28 23:36:34.855 locationconnection[1315:14103] 
-------------------------------------------------------------- ChatOneActivity ViewWillAppear
2020-04-28 23:36:34.855 locationconnection[1315:14103] 
2020-04-28 23:36:34.869 locationconnection[1315:14103] RegisteredForRemoteNotifications
2020-04-28 23:36:34.929 locationconnection[1315:14103] 
-------------------------------------------------------------- SetNormal

for iPhone X
2020-04-28 23:43:24.138 locationconnection[5314:93877] 
-------------------------------------------------------------- ChatList TouchesBegan False {X=145, Y=134}
2020-04-28 23:43:24.138 locationconnection[5314:93877] 
2020-04-28 23:43:24.145 locationconnection[5314:93877] 
-------------------------------------------------------------- ChatList TouchesMoved False {X=145, Y=134}
2020-04-28 23:43:24.145 locationconnection[5314:93877] 
2020-04-28 23:43:24.145 locationconnection[5314:93877] 
-------------------------------------------------------------- TouchesMoved stopping timer
2020-04-28 23:43:24.145 locationconnection[5314:93877] 
2020-04-28 23:43:24.151 locationconnection[5314:93877] 
-------------------------------------------------------------- ChatList TouchesMoved False {X=145, Y=134}
2020-04-28 23:43:24.151 locationconnection[5314:93877] 
2020-04-28 23:43:24.151 locationconnection[5314:93877] 
-------------------------------------------------------------- TouchesMoved restoring
2020-04-28 23:43:24.151 locationconnection[5314:93877] 
2020-04-28 23:43:24.152 locationconnection[5314:93877] 
-------------------------------------------------------------- SetNormal
2020-04-28 23:43:24.152 locationconnection[5314:93877] 
2020-04-28 23:43:24.160 locationconnection[5314:93877] 
-------------------------------------------------------------- ChatList TouchesMoved False {X=145, Y=134}
2020-04-28 23:43:24.160 locationconnection[5314:93877] 
2020-04-28 23:43:24.160 locationconnection[5314:93877] 
-------------------------------------------------------------- TouchesMoved restoring
2020-04-28 23:43:24.160 locationconnection[5314:93877] 
2020-04-28 23:43:24.160 locationconnection[5314:93877] 
-------------------------------------------------------------- SetNormal
2020-04-28 23:43:24.160 locationconnection[5314:93877] 
2020-04-28 23:43:24.167 locationconnection[5314:93877] 
-------------------------------------------------------------- ChatList TouchesMoved False {X=145, Y=134}
2020-04-28 23:43:24.167 locationconnection[5314:93877] 
2020-04-28 23:43:24.167 locationconnection[5314:93877] 
-------------------------------------------------------------- TouchesMoved restoring
2020-04-28 23:43:24.167 locationconnection[5314:93877] 
2020-04-28 23:43:24.168 locationconnection[5314:93877] 
-------------------------------------------------------------- SetNormal
2020-04-28 23:43:24.168 locationconnection[5314:93877] 
2020-04-28 23:43:24.240 locationconnection[5314:93877] 
-------------------------------------------------------------- TouchesEnded False {X=145, Y=134}*/

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
        private nfloat startX, startY;

        public CustomTap(ChatListActivity context, UITableView table) // (ChatUserListCell cell)
        {
            this.context = context;
            this.table = table;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt) //when table is still scrolling, only TouchesBegan is called, thus row will remain highlighted.
        {
            base.TouchesBegan(touches, evt);
            
            context.c.CW("ChatList TouchesBegan " + ChatListActivity.chatListScrolling + " " + LocationInView(table));

            var location = LocationInView(table);
            var indexPath = table.IndexPathForRowAtPoint(location);
            startX = location.X;
            startY = location.Y;

            if (indexPath != null && !ChatListActivity.chatListScrolling)
            {
                selectedCell = (ChatUserListCell)table.CellAt(indexPath);
                startTimer = new Timer();
                startTimer.Interval = timerMs;
                startTimer.Elapsed += StartTimer_Elapsed;
                startTimer.Start();
            }
            else
            {
                selectedCell = null;
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
            context.c.CW("ChatList TouchesMoved " + ChatListActivity.chatListScrolling + " " + LocationInView(table));

            var location = LocationInView(table);

            //threshold not needed for now
            if (location.X != startX || location.Y != startY)
            {
                if (startTimer != null && startTimer.Enabled)
                {
                    context.c.CW("TouchesMoved stopping timer");
                    startTimer.Stop();
                }
                else
                {
                    context.c.CW("TouchesMoved restoring");
                    SetNormal();
                }
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            context.c.CW("TouchesEnded " + ChatListActivity.chatListScrolling + " " + LocationInView(table));

            if (startTimer != null && startTimer.Enabled || pressed)
            {
                endTimer = new Timer();
                endTimer.Interval = timerMs;
                endTimer.Elapsed += EndTimer_Elapsed;
                endTimer.Start();

                context.c.CW("Ended index: " + table.IndexPathForCell(selectedCell).Row);

                Session.CurrentMatch = context.matchList[table.IndexPathForCell(selectedCell).Row];
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
            context.c.CW("SetHighlighted");
            if (selectedCell != null)
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
        }

        public void SetNormal ()
        {
            context.c.CW("SetNormal");
            if (selectedCell != null)
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
}
