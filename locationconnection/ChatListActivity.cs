using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace LocationConnection
{
    public partial class ChatListActivity : BaseActivity, IUITableViewDelegate
    {
        public List<MatchItem> matchList;
        ChatUserListAdapter adapter;
        CustomTap tap;

        public static bool chatListScrolling;

        public ChatListActivity (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                NoMatch.Text = LangEnglish.NoMatch;
                NoMatch.Hidden = true;

                c.AddViews(Snackbar, Snackbar.SnackText, Snackbar.SnackButton);

                MenuList.TouchUpInside += MenuList_Click;
                MenuList.TouchDown += MenuList_Touch;

                tap = new CustomTap(this, ChatUserList);
                ChatUserList.AddGestureRecognizer(tap);

                RoundBottom_Base = RoundBottom;
                Snackbar_Base = Snackbar;
                BottomConstraint_Base = BottomConstraint;
                SnackBottomConstraint_Base = SnackBottomConstraint;
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public override async void ViewWillAppear(bool animated)
        {
            try
            {
                base.ViewWillAppear(animated);                

                string responseString = await c.MakeRequest("action=loadmessagelist&ID=" + Session.ID + "&SessionID=" + Session.SessionID);
                if (responseString.Substring(0, 2) == "OK")
                {
                    responseString = responseString.Substring(3);
                    if (responseString != "")
                    {
                        NoMatch.Hidden = true;
                        ServerParser<MatchItem> parser = new ServerParser<MatchItem>(responseString);
                        matchList = parser.returnCollection;
                        adapter = new ChatUserListAdapter(matchList);
                        ChatUserList.Source = adapter;
                        NoofMatches.Text = (matchList.Count == 1) ? "1 " + LangEnglish.ChatListMatch : matchList.Count + " " + LangEnglish.ChatListMatches;
                    }
                    else
                    {
                        adapter = new ChatUserListAdapter(new List<MatchItem>());
                        ChatUserList.Source = adapter;
                        NoMatch.Hidden = false;
                        NoofMatches.Text = "";
                    }
                    ChatUserList.ReloadData();
                }

                ChatUserList.RowHeight = 101; //If the delegate is set before setting the Source, row height is gotten from the adapter allright. Otherwise the first row is about 20, and the subsequent rows are about 70.
                ChatUserList.Delegate = this; //must be called after setting the Source, otherwise the Scrolled event is not fired.
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public void InsertMessage(string meta, string body)
        {
            long unixTimestamp = c.Now();

            int sep1Pos = meta.IndexOf('|');
            int sep2Pos = meta.IndexOf('|', sep1Pos + 1);
            int sep3Pos = meta.IndexOf('|', sep2Pos + 1);

            int messageID = int.Parse(meta.Substring(0, sep1Pos));
            int senderID = int.Parse(meta.Substring(sep1Pos + 1, sep2Pos - sep1Pos - 1));
            long sentTime = long.Parse(meta.Substring(sep2Pos + 1, sep3Pos - sep2Pos - 1));
            long seenTime = unixTimestamp;
            long readTime = 0;

            for (int i = 0; i < matchList.Count; i++)
            {
                if (matchList[i].TargetID == senderID)
                {
                    c.LogActivity("InsertMessage i " + i + " matchList.Count" + matchList.Count);
                    if (matchList[i].Chat.Length == 3)
                    {
                        matchList[i].Chat[0] = matchList[i].Chat[1];
                        matchList[i].Chat[1] = matchList[i].Chat[2];
                        matchList[i].Chat[2] = messageID + "|" + senderID + "|" + sentTime + "|" + seenTime + "|" + readTime + "|" + body;
                    }
                    else
                    {
                        List<string> chatList = new List<string>(matchList[i].Chat);
                        chatList.Add(messageID + "|" + senderID + "|" + sentTime + "|" + seenTime + "|" + readTime + "|" + body);
                        matchList[i].Chat = chatList.ToArray();
                    }

                    ChatUserListAdapter adapter = new ChatUserListAdapter(matchList);
                    ChatUserList.Source = adapter;
                    ChatUserList.ReloadData();
                    c.MakeRequest("action=messagedelivered&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&MatchID=" + matchList[i].MatchID + "&MessageID=" + messageID + "&Status=Seen");
                }
            }
        }

        public void AddMatchItem(MatchItem item)
        {
            matchList.Insert(0, item);
            ChatUserListAdapter adapter = new ChatUserListAdapter(matchList);
            ChatUserList.Source = adapter;
            ChatUserList.ReloadData();
            ChatUserList.RowHeight = 101;
            ChatUserList.Delegate = this;
        }

        public void UpdateMatchItem(int matchID, bool active, long? unmatchDate)
        {
            for (int i = 0; i < matchList.Count; i++)
            {
                if (matchList[i].MatchID == matchID)
                {
                    matchList[i].Active = active;
                    matchList[i].UnmatchDate = unmatchDate;
                }
            }
            ChatUserListAdapter adapter = new ChatUserListAdapter(matchList);
            ChatUserList.Source = adapter;
            ChatUserList.ReloadData();
        }

        [Export("scrollViewDidScroll:")]
        public void Scrolled(UIScrollView scrollView) //A TouchesBegan alone can start a scroll movement.
        {
            chatListScrolling = true;

            if (tap.startTimer != null && tap.startTimer.Enabled)
            {
                tap.startTimer.Stop();
            }

            if (tap.pressed)
            {
                tap.SetNormal();
            }
        }

        [Export("scrollViewDidEndDragging:willDecelerate:")]
        public void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            if (tap.startTimer != null && tap.startTimer.Enabled)
            {
                tap.startTimer.Stop();
            }

            if (!willDecelerate)
            {
                chatListScrolling = false;
            }
        }

        [Export("scrollViewDidEndDecelerating:")]
        public void DecelerationEnded(UIScrollView scrollView)
        {
            chatListScrolling = false;

            if (tap.startTimer != null && tap.startTimer.Enabled)
            {
                tap.startTimer.Stop();
            }            
        }

        private void MenuList_Touch(object sender, EventArgs e)
        {
            c.AnimateRipple(RippleChatList, 3);
        }

        private void MenuList_Click(object sender, EventArgs e)
        {
            CommonMethods.OpenPage("ListActivity", 3);
        }
    }
}