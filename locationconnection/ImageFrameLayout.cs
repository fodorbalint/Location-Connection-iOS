using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Foundation;
using UIKit;

namespace LocationConnection
{
	[Register("ImageFrameLayout"), DesignTimeVisible(true)]
	public class ImageFrameLayout : UIView
	{
		private BaseActivity context;

		public int numColumns;
		public int tileSpacing;
		public nfloat tileSize;
		private float tweenTime = 0.36f;
		//public List<int> drawOrder;

		int removeIndex;

		UIView currentImage;
		Timer timer;
		float scaleRatio = 1.1F;
		bool imageMovable;
		public bool touchStarted;
		nfloat touchCurrentX;
		nfloat touchCurrentY;
		nfloat dragStartX;
		nfloat dragStartY;
		float picStartX;
		float picStartY;
		int startIndexPos;
		int endIndexPos;
		NSLayoutConstraint currentImageLeft;
		NSLayoutConstraint currentImageTop;
		NSLayoutConstraint currentImageWidth;
		NSLayoutConstraint currentImageHeight;

		RegisterCommonMethods rc;

		public ImageFrameLayout(IntPtr p) : base(p)
		{
			//drawOrder = new List<int>();
		}

		public void SetContext(BaseActivity context)
		{
			this.context = context;
			if (context is RegisterActivity)
			{
				rc = ((RegisterActivity)context).rc;
			}
            else
            {
				rc = ((ProfileEditActivity)context).rc;
            }
		}

		public void SetTileSize()
		{
			nfloat actualWidth = BaseActivity.dpWidth;
            if (BaseActivity.dpWidth / BaseActivity.dpHeight > 1) // in landscape
			{
				actualWidth -= BaseActivity.safeAreaLeft + BaseActivity.safeAreaRight;
			}
			
			tileSize = (actualWidth - 20 - tileSpacing * (numColumns - 1)) / numColumns;
			//context.c.CW("SetTileSize dpWidth " + BaseActivity.dpWidth + " left " + BaseActivity.safeAreaLeft + " right " + BaseActivity.safeAreaRight + " actualWidth " + actualWidth +  " tileSize "  + tileSize);
		}

        public void Reposition()
		{
			for (int i = 0; i < Subviews.Length; i++)
			{
				float top = GetPosY(i);
				float left = GetPosX(i);

				foreach (NSLayoutConstraint constraint in Constraints)
				{
					if (constraint.FirstItem == Subviews[i]) {
						UIView view = Subviews[i];

						//Console.WriteLine("reposition constraint " + constraint.FirstItem + " --- " + constraint.FirstAttribute + " --- " + constraint.SecondItem + " --- " + constraint.SecondAttribute + " --- " + constraint.Constant);
						if (constraint.FirstAttribute == NSLayoutAttribute.Top && constraint.Constant != top)
                        {
							constraint.Constant = top;
						}
						if (constraint.FirstAttribute == NSLayoutAttribute.Left && constraint.Constant != left)
						{
							constraint.Constant = left;
						}

                        foreach (NSLayoutConstraint constraint1 in view.Constraints)
                        {
							if (constraint1.FirstAttribute == NSLayoutAttribute.Width && constraint1.SecondItem is null)
                            {
								constraint1.Constant = tileSize;
							}
                            if (constraint1.FirstAttribute == NSLayoutAttribute.Height && constraint1.SecondItem is null)
                            {
								constraint1.Constant = tileSize;
                            }
						}
					}                    
				}				
			}
		}

		public void AddPicture(string picture, int pos)
        {
			UploadedItem uploadedItem = new UploadedItem();

			AddSubview(uploadedItem);

			//uploadedItem.Layer.ZPosition = pos;

			uploadedItem.TranslatesAutoresizingMaskIntoConstraints = false;

			uploadedItem.WidthAnchor.ConstraintEqualTo(tileSize).Active = true;
			uploadedItem.HeightAnchor.ConstraintEqualTo(tileSize).Active = true;

			uploadedItem.TopAnchor.ConstraintEqualTo(TopAnchor, GetPosY(pos)).Active = true;
			uploadedItem.LeftAnchor.ConstraintEqualTo(LeftAnchor, GetPosX(pos)).Active = true;

			uploadedItem.LayoutIfNeeded();

			if (context is ProfileEditActivity)
			{
				uploadedItem.SetImage(context, Session.ID.ToString(), picture);
			}
			else
			{
				uploadedItem.SetImage(context, RegisterActivity.regsessionid, picture, true);
			}
			
			UIButton deleteUploadedImage = (UIButton)uploadedItem.Subviews[0].Subviews[1];

            deleteUploadedImage.TouchUpInside += DeleteUploadedImage_Click;            

			foreach (NSLayoutConstraint constraint in Constraints)
			{
				if (constraint.FirstAttribute == NSLayoutAttribute.Height)
				{
					constraint.Constant = (pos - pos % numColumns) / numColumns * (tileSize + tileSpacing) + tileSize;
				}
			}

			//drawOrder.Add(pos);
		}

        private async void DeleteUploadedImage_Click(object sender, EventArgs e)
        {
			if (rc.imagesUploading)
            {
				context.c.Snack(LangEnglish.ImagesUploading);
				return;
            }
            else if (rc.imagesDeleting)
            {
				context.c.Snack(LangEnglish.ImagesDeleting);
				return;
            }

			if (context is ProfileEditActivity)
			{
				if (rc.uploadedImages.Count == 1)
				{
					context.c.Snack(LangEnglish.LastImageToDelete);
					return;
				}
			}
			else if (rc.uploadedImages.Count == 1)
			{
				rc.ImagesProgressText.Text = "";
			}

			rc.imagesDeleting = true;
			rc.StartAnim();

			UploadedItem item = (UploadedItem)((UIButton)sender).Superview.Superview;
			int index = Array.IndexOf(Subviews, item); //GetDrawPosFromChildIndex(Array.IndexOf(Subviews, item));

			if (context is ProfileEditActivity)
			{
				string responseString = await context.c.MakeRequest("action=deleteexisting&imageName=" + CommonMethods.UrlEncode(rc.uploadedImages[index]) + "&ID=" + Session.ID + "&SessionID=" + Session.SessionID);
				if (responseString.Substring(0, 2) == "OK")
				{
					rc.uploadedImages.RemoveAt(index);
					Session.Pictures = rc.uploadedImages.ToArray();

					RemovePicture(index);
					if (rc.uploadedImages.Count == 1)
					{
						rc.ImagesProgressText.Text = "";
					}
				}
				else
				{
					context.c.ReportError(responseString);
					rc.imagesDeleting = false;
				}
			}
			else
			{
				context.c.LogActivity("DeleteUploadedImage_Click index: " + index + " count " + rc.uploadedImages.Count);
				Console.WriteLine("DeleteUploadedImage_Click index: " + index + " count " + rc.uploadedImages.Count);

				string responseString = await context.c.MakeRequest("action=deletetemp&imageName=" + CommonMethods.UrlEncode(rc.uploadedImages[index]) + "&regsessionid=" + RegisterActivity.regsessionid);
				if (responseString.Substring(0, 2) == "OK")
				{
					rc.uploadedImages.RemoveAt(index);

					RemovePicture(index);
					if (rc.uploadedImages.Count == 1)
					{
						rc.ImagesProgressText.Text = "";
					}
					else if (rc.uploadedImages.Count == 0)
					{
						File.Delete(RegisterActivity.regSessionFile);
					}
				}
				else
				{
					context.c.ReportError(responseString);
					rc.imagesDeleting = false;
				}
			}
			
			rc.StopAnim();

		}

        private void RemovePicture(int index)
        {
			removeIndex = index;
			UIView view = Subviews[index]; // GetChildFromDrawOrder(index);
            Animate(duration: tweenTime, delay: 0, options: UIViewAnimationOptions.CurveLinear, animation: () =>
			{
				view.Alpha = 0;
			}, completion: () => { Animator_AnimationEnd(); });
		}

		private void Animator_AnimationEnd()
		{
			if (removeIndex < rc.uploadedImages.Count) //not the last one (uploadedImages have already been updated)
			{
				for (int i = removeIndex + 1; i <= rc.uploadedImages.Count; i++)
				{
					MovePictureTo(i, i - 1);
				}
				Timer t = new Timer();
				t.Interval = tweenTime * 1000;
				t.Elapsed += T_Elapsed;
				t.Start();
				Subviews[removeIndex].RemoveFromSuperview(); //RemoveViewFromDrawOrder(removeIndex);
			}
			else
			{
				Subviews[removeIndex].RemoveFromSuperview(); //RemoveViewFromDrawOrder(removeIndex);
				RefitImagesContainer();
				rc.imagesDeleting = false;
			}			
		}

		private void MovePictureTo(int from, int to)
		{
			Animate(duration: tweenTime, delay: 0, options: UIViewAnimationOptions.CurveLinear, animation: () =>
			{
				UIView view = Subviews[from]; //GetChildFromDrawOrder(from);
                foreach (NSLayoutConstraint constraint in Constraints)
                {
                    if (constraint.FirstItem == view)
                    {
						if (constraint.FirstAttribute == NSLayoutAttribute.Top && constraint.Constant != GetPosY(to))
						{
							constraint.Constant = GetPosY(to);
						}
						if (constraint.FirstAttribute == NSLayoutAttribute.Left)
						{
							constraint.Constant = GetPosX(to);
						}
					}
				}
				LayoutIfNeeded();

			}, completion: () => { rc.imagesDeleting = false; });
		}

		private void T_Elapsed(object sender, ElapsedEventArgs e)
		{
			((Timer)sender).Stop();
			context.InvokeOnMainThread(() => {
				RefitImagesContainer();
			});
		}

		public void RefitImagesContainer()
		{
			if (Subviews.Length > 0)
			{
				int indexCount = Subviews.Length - 1;

				foreach (NSLayoutConstraint constraint in Constraints)
				{
                    if (constraint.FirstAttribute == NSLayoutAttribute.Height)
                    {
						constraint.Constant = (indexCount - indexCount % numColumns) / numColumns * (tileSize + tileSpacing) + tileSize;
					}
                }
			}
			else
			{
				foreach (NSLayoutConstraint constraint in Constraints)
				{
					if (constraint.FirstAttribute == NSLayoutAttribute.Height)
					{
						constraint.Constant = 0;
					}
				}
			}
		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			context.c.CW("TouchesBegan");
			base.TouchesBegan(touches, evt);
			if (!touchStarted)
            {
				Down(touches);
			}
		}

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
			base.TouchesMoved(touches, evt);
			Move(touches);
		}

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
			context.c.CW("TouchesEnded");
			base.TouchesEnded(touches, evt);
			Up();
		}

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
			context.c.CW("TouchCancelled");
			base.TouchesCancelled(touches, evt);
			Up();
		}

        public void Down(NSSet touches) //there is a system delay for detecting press, no need to define a pressTime value.
		{
            if (Subviews.Length <= 1)
            {
				return;
            }

			UITouch touch = touches.AnyObject as UITouch;

			touchCurrentX = touch.LocationInView(this).X;
			touchCurrentY = touch.LocationInView(this).Y;
			startIndexPos = GetIndexFromPos((float)touchCurrentX, (float)touchCurrentY);

			context.c.CW("Down startindexpos " + startIndexPos);

			if (startIndexPos < Subviews.Length)
			{
				currentImage = Subviews[startIndexPos];

				BringSubviewToFront(currentImage); //changes subview order. view.Layer.ZPosition is unreliable, it wouldn't always bring a view to top even if its value is set to float.MaxValue.
                
				foreach (NSLayoutConstraint constraint in Constraints)
				{
					if (constraint.FirstItem == currentImage)
					{
						if (constraint.FirstAttribute == NSLayoutAttribute.Top)
						{
							currentImageTop = constraint;
						}
						if (constraint.FirstAttribute == NSLayoutAttribute.Left)
						{
							currentImageLeft = constraint;
						}

						foreach (NSLayoutConstraint constraint1 in currentImage.Constraints)
						{
							if (constraint1.FirstAttribute == NSLayoutAttribute.Width && constraint1.SecondItem is null)
							{
								currentImageWidth = constraint1;
							}
							if (constraint1.FirstAttribute == NSLayoutAttribute.Height && constraint1.SecondItem is null)
							{
								currentImageHeight = constraint1;
							}
						}
					}
				}

				Animate(tweenTime / 2, () => {

					currentImageTop.Constant -= tileSize * (scaleRatio - 1) / 2;
                    currentImageLeft.Constant -= tileSize * (scaleRatio - 1) / 2;
					currentImageWidth.Constant = tileSize * scaleRatio;
					currentImageHeight.Constant = tileSize * scaleRatio;

					LayoutIfNeeded();
				}, () => {
					Animate(tweenTime / 2, () => {

						currentImageTop.Constant += tileSize * (scaleRatio - 1) / 2;
						currentImageLeft.Constant += tileSize * (scaleRatio - 1) / 2;
						currentImageWidth.Constant = tileSize;
						currentImageHeight.Constant = tileSize;

						LayoutIfNeeded();
					}, () => {
						dragStartX = touchCurrentX;
						dragStartY = touchCurrentY;
						Console.WriteLine("dragstart: x " + dragStartX + " y " + dragStartY + " " + picStartX + " " + picStartY);
                        imageMovable = true;
                    });
				});

				picStartX = GetPosX(startIndexPos);
				picStartY = GetPosY(startIndexPos);

				imageMovable = false;
				touchStarted = true;
			}
		}

        public void Move(NSSet touches)
        {
			UITouch touch = touches.AnyObject as UITouch;

			touchCurrentX = touch.LocationInView(this).X;
			touchCurrentY = touch.LocationInView(this).Y;

            if (imageMovable)
            {
				nfloat distX = touchCurrentX - dragStartX;
				nfloat distY = touchCurrentY - dragStartY;

				currentImageTop.Constant = picStartY + distY;
                currentImageLeft.Constant = picStartX + distX;
			}
		}

		public void Up()
		{
			context.c.CW("Up imageMovable " + imageMovable);

            if (imageMovable)
            {
				imageMovable = false;

				nfloat centerX = currentImageLeft.Constant + tileSize / 2;
				nfloat centerY = currentImageTop.Constant + tileSize / 2;

				endIndexPos = GetIndexFromPos(centerX, centerY);		

				if (endIndexPos < Subviews.Length && endIndexPos >= 0)
				{
					float endX = GetPosX(endIndexPos);
					float endY = GetPosY(endIndexPos);

					Animate(tweenTime, () =>
					{
						currentImageTop.Constant = endY;
						currentImageLeft.Constant = endX;
						LayoutIfNeeded();
					}, () => { });

					if (endIndexPos < startIndexPos)
					{
						string moveImage = rc.uploadedImages[startIndexPos];

						for (int i = startIndexPos; i > endIndexPos; i--)
						{
							UIView.Animate(tweenTime, () => {
								UIView view = Subviews[i - 1];

								foreach (NSLayoutConstraint constraint in Constraints)
								{
									if (constraint.FirstItem == view)
									{

										if (constraint.FirstAttribute == NSLayoutAttribute.Top)
										{
											constraint.Constant = GetPosY(i);
										}
										if (constraint.FirstAttribute == NSLayoutAttribute.Left)
										{
											constraint.Constant = GetPosX(i);
										}
									}
								}
								LayoutIfNeeded();
							});

							rc.uploadedImages[i] = rc.uploadedImages[i - 1];
						}
						rc.uploadedImages[endIndexPos] = moveImage;

						if (context is ProfileEditActivity)
						{
							Task.Run(async () =>
							{
								string responseString = await context.c.MakeRequest("action=updatepictures&Pictures=" + CommonMethods.UrlEncode(string.Join("|", ((ProfileEditActivity)context).rc.uploadedImages)) + "&ID=" + Session.ID + "&SessionID=" + Session.SessionID);
								if (responseString.Substring(0, 2) == "OK")
								{
									Session.Pictures = ((ProfileEditActivity)context).rc.uploadedImages.ToArray();
								}
								else
								{
									context.c.ReportError(responseString);
								}
							});
						}
					}
					else if (endIndexPos > startIndexPos)
					{
						string moveImage = rc.uploadedImages[startIndexPos];
						for (int i = startIndexPos; i < endIndexPos; i++)
						{
							UIView.Animate(tweenTime, () => {
								UIView view = Subviews[i];

								foreach (NSLayoutConstraint constraint in Constraints)
								{
									if (constraint.FirstItem == view)
									{
										if (constraint.FirstAttribute == NSLayoutAttribute.Top)
										{
											constraint.Constant = GetPosY(i);
										}
										if (constraint.FirstAttribute == NSLayoutAttribute.Left)
										{
											constraint.Constant = GetPosX(i);
										}
									}
								}
								LayoutIfNeeded();
							});

							rc.uploadedImages[i] = rc.uploadedImages[i + 1];
							//drawOrder[i] = drawOrder[i + 1];
						}
						rc.uploadedImages[endIndexPos] = moveImage;
						//drawOrder[endIndexPos] = moveIndex;

						if (context is ProfileEditActivity)
						{
							Task.Run(async () =>
							{
								string responseString = await context.c.MakeRequest("action=updatepictures&Pictures=" + CommonMethods.UrlEncode(string.Join("|", ((ProfileEditActivity)context).rc.uploadedImages)) + "&ID=" + Session.ID + "&SessionID=" + Session.SessionID);
								if (responseString.Substring(0, 2) == "OK")
								{
									Session.Pictures = ((ProfileEditActivity)context).rc.uploadedImages.ToArray();
								}
								else
								{
									context.c.ReportError(responseString);
								}
							});
						}
					}
					//else remained at the same place
				}
				else //out of range, restore position
				{
					float endX = GetPosX(startIndexPos);
					float endY = GetPosY(startIndexPos);

					UIView.Animate(tweenTime, () =>
					{
						currentImageTop.Constant = endY;
						currentImageLeft.Constant = endX;

						LayoutIfNeeded();
					});
					endIndexPos = startIndexPos; //WaitToEnd will put it into place in the subview hierarchy
				}
				WaitToEnd();
			}
            else if (touchStarted)
            {
				InsertSubview(currentImage, startIndexPos);
				touchStarted = false;
			}//click position out of range
		}

		private void WaitToEnd()
		{
			timer = new Timer();
			timer.Interval = tweenTime * 1000;
			timer.Elapsed += Timer_Elapsed;
			timer.Start();
		}

		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			timer.Stop();
			InvokeOnMainThread(() => { InsertSubview(currentImage, endIndexPos); });			
			touchStarted = false;
		}

		private float GetPosX(int pos)
		{
			return (float)(pos % numColumns * (tileSize + tileSpacing));
		}

		private float GetPosY(int pos)
		{
			return (float)((pos - pos % numColumns) / numColumns * (tileSize + tileSpacing));
		}

		private int GetIndexFromPos(nfloat x, nfloat y)
		{
			int posX = (int)((x - x % (tileSize + tileSpacing)) / (tileSize + tileSpacing));
			int posY = (int)((y - y % (tileSize + tileSpacing)) / (tileSize + tileSpacing));
			return posY * numColumns + posX;
		}
	}
}