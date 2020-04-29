﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CoreAnimation;
using CoreGraphics;
using CoreLocation;
using Foundation;
using UIKit;

namespace LocationConnection
{
    public class RegisterCommonMethods
    {
        public static string regSessionFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "regsession.txt");
        private string regSaveFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "regsave.txt");

        private BaseActivity context;

		public ImageFrameLayout ImagesUploaded;
		public UITextField Email, Username, Name;
        public UITextView DescriptionText;
		public UIButton CheckUsername, Images;
		public UILabel ImagesProgressText;
		public UIImageView LoaderCircle;
		public UIProgressView ImagesProgress;
		public UISwitch UseLocationSwitch, LocationShareAll, LocationShareLike, LocationShareMatch, LocationShareFriend, LocationShareNone;
		public UISwitch DistanceShareAll, DistanceShareLike, DistanceShareMatch, DistanceShareFriend, DistanceShareNone;
		public UIView ImageEditorControls;
		public UIView ImageEditorStatus;
		public UIButton ImageEditorCancel;
		public UIButton ImageEditorOK;
		public UIImageView ImageEditor;
		public UIView ImageEditorFrame, ImageEditorFrameBorder;
		//private CustomMove move;
        
		public CommonMethods c;

		public List<string> uploadedImages;
		public bool imagesUploading;
		public bool imagesDeleting;
		private nint loaderAnimTime = 1300;
        private WebClient client;

        UIImagePickerController imagePicker;
		private static string selectedImage, selectedImageName;

		private nfloat lastScale;
		private nfloat touchStartX;
		private nfloat touchStartY;
		private nfloat startCenterX;
		private nfloat startCenterY;
		private nfloat xDist;
		private nfloat yDist;
		private nfloat prevTouchX;
		private nfloat prevTouchY;
		private bool outOfFrameX;
		private bool outOfFrameY;

		public RegisterCommonMethods(BaseActivity context, CommonMethods c, ImageFrameLayout ImagesUploaded, UITextField Email, UITextField Username, UITextField Name, UITextView DescriptionText, UIButton CheckUsername, UIButton Images,
            UILabel ImagesProgressText, UIImageView LoaderCircle, UIProgressView ImagesProgress, UISwitch UseLocationSwitch, UISwitch LocationShareAll, UISwitch LocationShareLike, UISwitch LocationShareMatch, UISwitch LocationShareFriend, UISwitch LocationShareNone,
            UISwitch DistanceShareAll, UISwitch DistanceShareLike, UISwitch DistanceShareMatch, UISwitch DistanceShareFriend, UISwitch DistanceShareNone, UIView ImageEditorControls, UIView ImageEditorStatus, UIButton ImageEditorCancel, UIButton ImageEditorOK, UIImageView ImageEditor,UIView ImageEditorFrame, UIView ImageEditorFrameBorder)
        {
            this.context = context;
            this.c = c;

			this.ImagesUploaded = ImagesUploaded;
			this.Email = Email;
			this.Username = Username;
			this.Name = Name;
			this.DescriptionText = DescriptionText;
			this.CheckUsername = CheckUsername;
			this.Images = Images;
			this.ImagesProgressText = ImagesProgressText;
			this.LoaderCircle = LoaderCircle;
			this.ImagesProgress = ImagesProgress;
			this.UseLocationSwitch = UseLocationSwitch;
			this.LocationShareAll = LocationShareAll;
			this.LocationShareLike = LocationShareLike;
			this.LocationShareMatch = LocationShareMatch;
			this.LocationShareFriend = LocationShareFriend;
			this.LocationShareNone = LocationShareNone;
			this.DistanceShareAll = DistanceShareAll;
			this.DistanceShareLike = DistanceShareLike;
			this.DistanceShareMatch = DistanceShareMatch;
			this.DistanceShareFriend = DistanceShareFriend;
			this.DistanceShareNone = DistanceShareNone;
			this.ImageEditorControls = ImageEditorControls;
			this.ImageEditorStatus = ImageEditorStatus;
			this.ImageEditorCancel = ImageEditorCancel;
			this.ImageEditorOK = ImageEditorOK;
			this.ImageEditor = ImageEditor;
			this.ImageEditorFrame = ImageEditorFrame;
			this.ImageEditorFrameBorder = ImageEditorFrameBorder;

			UIPanGestureRecognizer move = new UIPanGestureRecognizer();
			move.AddTarget(() => MoveImage(move));
			ImageEditor.AddGestureRecognizer(move);

			UIPinchGestureRecognizer zoom = new UIPinchGestureRecognizer();
			zoom.AddTarget(() => ZoomImage(zoom));
			ImageEditor.AddGestureRecognizer(zoom);

            client = new WebClient();
            client.UploadProgressChanged += Client_UploadProgressChanged;
            client.UploadFileCompleted += Client_UploadFileCompleted;
            client.Headers.Add("Content-Type", "image/jpeg");            
        }

		public async void CheckUsername_Click(object sender, EventArgs e)
		{
			if (Username.Text.Trim() == "")
			{
				Username.BecomeFirstResponder();
				c.Snack(LangEnglish.UsernameEmpty);
				return;
			}
			
            if (Username.Text.Trim() == Session.Username)
            {
                c.Snack(LangEnglish.UsernameSame);
                return;
            }

			context.View.EndEditing(true);

			CheckUsername.Enabled = false;
			CheckUsername.Alpha = 0.5f;

			string responseString = await c.MakeRequest("action=usercheck&Username=" + Username.Text.Trim());
			if (responseString == "OK")
			{
				c.Snack(LangEnglish.UsernameAvailable);
			}
			else if (responseString.Substring(0, 6) == "ERROR_")
			{
				c.Snack(c.GetLang(responseString.Substring(6)));
			}
			else
			{
				c.ReportError(responseString);
			}

			CheckUsername.Enabled = true;
			CheckUsername.Alpha = 1;
		}

        public void Images_Click(object sender, EventArgs e)
        {
			context.View.EndEditing(true);

            if (uploadedImages.Count < Constants.MaxNumPictures)
            {
                if (!imagesUploading && !imagesDeleting)
                {
                    ImagesProgressText.Text = "";
                    ImagesProgress.Progress = 0;

                    if (imagePicker == null)
                    {
                        imagePicker = new UIImagePickerController();

                        imagePicker.Canceled += (object sender1, EventArgs e1) =>
                        {
                            imagePicker.DismissViewController(true, null);
                        };

                        imagePicker.FinishedPickingMedia += async (object sender1, UIImagePickerMediaPickedEventArgs e1) =>
                        {
                            imagePicker.DismissViewController(true, null);

							selectedImage = e1.ImageUrl.Path;
							selectedImageName = selectedImage.Substring(selectedImage.LastIndexOf("/") + 1);

                            Console.WriteLine("imageName: " + selectedImageName);
                            if (uploadedImages.IndexOf(selectedImageName) != -1) //virtually impossible, since uploaded images are tagged with timestamp
                            {
                                context.c.Snack(LangEnglish.ImageExists);
                                return;
                            }

							UIImage image = UIImage.FromFile(e1.ImageUrl.Path);
							nfloat sizeRatio = image.Size.Width / image.Size.Height;

                            if (sizeRatio == 1)
                            {
								await UploadFile(selectedImage, RegisterActivity.regsessionid);
							}
                            else
                            {
								ImageEditor.Image = image;
								ImageEditor.Transform = CGAffineTransform.MakeScale(1, 1);
								lastScale = 1;
								ImageEditorControls.Hidden = false;
								ImageEditorStatus.Hidden = false;
								ImageEditor.Hidden = false;
								ImageEditorFrame.Hidden = false;
								ImageEditorFrameBorder.Hidden = false;

								if (sizeRatio > 1)
								{
									context.c.SetHeight(ImageEditor, ImageEditorFrameBorder.Frame.Width);
									context.c.SetWidth(ImageEditor, ImageEditorFrameBorder.Frame.Width * sizeRatio);
								}
								else
								{
									context.c.SetHeight(ImageEditor, ImageEditorFrameBorder.Frame.Width / sizeRatio);
									context.c.SetWidth(ImageEditor, ImageEditorFrameBorder.Frame.Width);
								}
							}							
						};
                    }

                    imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
                    if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                    {
						imagePicker.ModalPresentationStyle = UIModalPresentationStyle.Popover;
                        imagePicker.PopoverPresentationController.SourceView = Images;
                    }
                    else
                    {
                        imagePicker.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
                    }

                    context.PresentViewController(imagePicker, true, () => { });
                }
                else
                {
                    if (imagesUploading)
                    {
						c.Snack(LangEnglish.ImagesUploading);
					}
                    else
                    {
						c.Snack(LangEnglish.ImagesDeleting);
					}                    
                }
            }
            else
            {
                c.Snack(LangEnglish.MaxNumImages + " " + Constants.MaxNumPictures + ".");
            }
        }

        private bool IsOutOfFrameY(nfloat yDist)
        {
			if (yDist <= 0 && (-yDist + ImageEditorFrameBorder.Frame.Height / 2) > ImageEditor.Frame.Height / 2 || yDist > 0 && (yDist + ImageEditorFrameBorder.Frame.Height / 2) > ImageEditor.Frame.Height / 2)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private bool IsOutOfFrameX(nfloat xDist)
		{
			if (xDist <= 0 && (-xDist + ImageEditorFrameBorder.Frame.Width / 2) > ImageEditor.Frame.Width / 2 || xDist > 0 && (xDist + ImageEditorFrameBorder.Frame.Width / 2) > ImageEditor.Frame.Width / 2)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		//out of frame image is allowed to come closer. Image in frame is not allowed to go out, only by pinching action.
		public void MoveImage(UIPanGestureRecognizer recognizer)
		{
			var location = recognizer.LocationInView(ImageEditorFrame);
			if (recognizer.State == UIGestureRecognizerState.Began)
			{
				prevTouchX = touchStartX = location.X;
				prevTouchY = touchStartY = location.Y;
				startCenterX = ImageEditor.Center.X;
				startCenterY = ImageEditor.Center.Y;

				xDist = startCenterX - ImageEditorFrameBorder.Center.X;
				yDist = startCenterY - ImageEditorFrameBorder.Center.Y;

				outOfFrameY = IsOutOfFrameY(yDist);
				outOfFrameX = IsOutOfFrameX(xDist);
			}
            else
            {
				nfloat newxDist = startCenterX + location.X - touchStartX - ImageEditorFrameBorder.Center.X;
				nfloat newyDist = startCenterY + location.Y - touchStartY - ImageEditorFrameBorder.Center.Y;

				if (outOfFrameY && (yDist <= 0 && newyDist < yDist || yDist > 0 && newyDist > yDist)) //out of frame, new distance is greater than previous
				{
					touchStartY += location.Y - prevTouchY;
				}
                else if (outOfFrameY) //new distance is smaller
                {
					if (yDist <= 0 && newyDist > (ImageEditor.Frame.Height - ImageEditorFrameBorder.Frame.Height) / 2) //making sure not to go out of frame the opposite end. (when the image is scaled back to 1:1, and moved fast, it can happen)
                    {
						yDist = (ImageEditor.Frame.Height - ImageEditorFrameBorder.Frame.Height) / 2;
						touchStartY += newyDist - (ImageEditor.Frame.Height - ImageEditorFrameBorder.Frame.Height) / 2; //moving start touch position, so an opposite move will react immediately
					}
                    else if (yDist > 0 && newyDist < -(ImageEditor.Frame.Height - ImageEditorFrameBorder.Frame.Height) / 2)
					{
						yDist = -(ImageEditor.Frame.Height - ImageEditorFrameBorder.Frame.Height) / 2;
						touchStartY += newyDist - -(ImageEditor.Frame.Height - ImageEditorFrameBorder.Frame.Height) / 2;
					}
					else
					{
						yDist = newyDist;
					}
					ImageEditor.Center = new CoreGraphics.CGPoint(ImageEditor.Center.X, ImageEditorFrameBorder.Center.Y + yDist);

					outOfFrameY = IsOutOfFrameY(yDist);
				}
                else
                {
					yDist = newyDist;

					if (yDist <= 0 && (-yDist + ImageEditorFrameBorder.Frame.Height / 2) > ImageEditor.Frame.Height / 2) //going out of frame too high
					{
						yDist = -(ImageEditor.Frame.Height - ImageEditorFrameBorder.Frame.Height) / 2;
						touchStartY += newyDist - -(ImageEditor.Frame.Height - ImageEditorFrameBorder.Frame.Height) / 2;
					}
                    else if (yDist > 0 && (yDist + ImageEditorFrameBorder.Frame.Height / 2) > ImageEditor.Frame.Height / 2) //going out of frame too low
                    {
						yDist = (ImageEditor.Frame.Height - ImageEditorFrameBorder.Frame.Height) / 2;
						touchStartY += newyDist - (ImageEditor.Frame.Height - ImageEditorFrameBorder.Frame.Height) / 2;
					}
					// else in frame 
					ImageEditor.Center = new CoreGraphics.CGPoint(ImageEditor.Center.X, ImageEditorFrameBorder.Center.Y + yDist);
				}				


				if (outOfFrameX && (xDist <= 0 && newxDist < xDist || xDist > 0 && newxDist > xDist)) //out of frame, new is distance greater than previous
				{
					touchStartX += location.X - prevTouchX;
				}
                else if (outOfFrameX)
                {
					if (xDist <= 0 && newxDist > (ImageEditor.Frame.Width - ImageEditorFrameBorder.Frame.Width) / 2) //making sure not to go out of frame the opposite end. (when the image is scaled back to 1:1, and moved fast, it can happen)
					{
						xDist = (ImageEditor.Frame.Width - ImageEditorFrameBorder.Frame.Width) / 2;
						touchStartX += newxDist - (ImageEditor.Frame.Width - ImageEditorFrameBorder.Frame.Width) / 2; //moving start touch position, so an opposite move will react immediately
					}
					else if (xDist > 0 && newxDist < -(ImageEditor.Frame.Width - ImageEditorFrameBorder.Frame.Width) / 2)
					{
						xDist = -(ImageEditor.Frame.Width - ImageEditorFrameBorder.Frame.Width) / 2;
						touchStartX += newxDist - -(ImageEditor.Frame.Width - ImageEditorFrameBorder.Frame.Width) / 2; //moving start touch position, so an opposite move will react immediately
					}
					else
					{
						xDist = newxDist;
					}
					ImageEditor.Center = new CoreGraphics.CGPoint(ImageEditorFrameBorder.Center.X + xDist, ImageEditor.Center.Y);

					outOfFrameX = IsOutOfFrameX(xDist);
				}
                else
                {
					xDist = newxDist;

					if (xDist <= 0 && (-xDist + ImageEditorFrameBorder.Frame.Width / 2) > ImageEditor.Frame.Width / 2) //going out of frame too left
					{
						xDist = -(ImageEditor.Frame.Width - ImageEditorFrameBorder.Frame.Width) / 2;
						touchStartX += newxDist - -(ImageEditor.Frame.Width - ImageEditorFrameBorder.Frame.Width) / 2; //moving start touch position, so an opposite move will react immediately
					}
                    else if (xDist > 0 && (xDist + ImageEditorFrameBorder.Frame.Width / 2) > ImageEditor.Frame.Width / 2) //going out of frame too right
                    {
						xDist = (ImageEditor.Frame.Width - ImageEditorFrameBorder.Frame.Width) / 2;
						touchStartX += newxDist - (ImageEditor.Frame.Width - ImageEditorFrameBorder.Frame.Width) / 2;
					}
					// else in frame
					ImageEditor.Center = new CoreGraphics.CGPoint(ImageEditorFrameBorder.Center.X + xDist, ImageEditor.Center.Y);
				}

				prevTouchX = location.X;
				prevTouchY = location.Y;
			}
		}

		public void ZoomImage(UIPinchGestureRecognizer recognizer)
		{

            if (recognizer.State == UIGestureRecognizerState.Ended)
            {
				lastScale = recognizer.Scale * lastScale;
                if (lastScale < 1)
                {
					lastScale = 1;
                }
                else if (lastScale > 3)
                {
					lastScale = 3;
                }
				return;
            }
            if (lastScale * recognizer.Scale >= 1 && lastScale * recognizer.Scale <= 3)
            {
				recognizer.View.Transform = CGAffineTransform.MakeScale(recognizer.Scale * lastScale, recognizer.Scale * lastScale);
			}
            else if (lastScale * recognizer.Scale < 1)
            {
				recognizer.View.Transform = CGAffineTransform.MakeScale(1, 1);
			}
            else
            {
				recognizer.View.Transform = CGAffineTransform.MakeScale(3, 3);
			}
			
			//context.c.CW("Image pinched, state: " + recognizer.State + " " + recognizer.Scale + " " + lastScale);
		}

		public void CancelImageEditing(object sender, EventArgs e)
		{
			ImageEditorControls.Hidden = true;
			ImageEditorStatus.Hidden = true;
			ImageEditor.Hidden = true;
			ImageEditorFrame.Hidden = true;
			ImageEditorFrameBorder.Hidden = true;
			ImageEditor.Image = null;
		}

		public async void OKImageEditing(object sender, EventArgs e)
		{
			
			xDist = ImageEditor.Center.X - ImageEditorFrameBorder.Center.X;
			yDist = ImageEditor.Center.Y - ImageEditorFrameBorder.Center.Y;
			if (IsOutOfFrameX(xDist) || IsOutOfFrameY(yDist))
            {
				context.c.Alert(LangEnglish.ImageEditorAlert);
				return;
            }

			nfloat w = ImageEditor.Image.Size.Width;
			nfloat h = ImageEditor.Image.Size.Height;

			nfloat x = (ImageEditorFrameBorder.Frame.Left - ImageEditor.Frame.Left) / ImageEditor.Frame.Width * w;
			nfloat y = (ImageEditorFrameBorder.Frame.Top - ImageEditor.Frame.Top) / ImageEditor.Frame.Height * h;
			nfloat cropW = ImageEditorFrameBorder.Frame.Width / ImageEditor.Frame.Width * w;
			nfloat cropH = cropW;

			//context.c.CW("OKImageEditing " + ImageEditorFrameBorder.Frame + " --- " + ImageEditor.Frame + " " + w + " " + h + " " + x + " " + y + " " + cropW + " " + cropH);

			UIImage im = CropImage(ImageEditor.Image, (int)Math.Round(x), (int)Math.Round(y), (int)Math.Round(cropW), (int)Math.Round(cropH));

			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			string cacheDir = Path.Combine(documents, "..", "Library/Caches");
			string fileName = Path.Combine(cacheDir, selectedImageName);
			string ext = selectedImageName.Substring(selectedImageName.LastIndexOf(".") + 1).ToLower();

			NSData data;
			context.c.CW(selectedImageName);

			if (ext == "jpg" || ext == "jpeg")
			{
				data = im.AsJPEG(); //default compression quality is 1. File size example: 0.99: 1710751, 1: 3502822
			}
			else
			{
				data = im.AsPNG();
			}			

			if (!data.Save(fileName, false, out NSError error))
			{
				context.c.ReportError("Error while cropping image: " + error.LocalizedDescription);
			}
            else
            {
				ImageEditorControls.Hidden = true;
				ImageEditorStatus.Hidden = true;
				ImageEditor.Hidden = true;
				ImageEditorFrame.Hidden = true;
				ImageEditorFrameBorder.Hidden = true;
				ImageEditor.Image = null;

				await UploadFile(fileName, RegisterActivity.regsessionid); //works for profile edit too
			}
		}

		private UIImage CropImage(UIImage sourceImage, float crop_x, int crop_y, int width, int height)
		{
			var imgSize = sourceImage.Size;
			UIGraphics.BeginImageContext(new SizeF(width, height));
			var context = UIGraphics.GetCurrentContext();
			var clippedRect = new RectangleF(0, 0, width, height);
			context.ClipToRect(clippedRect);
			var drawRect = new RectangleF(-crop_x, -crop_y, (float)imgSize.Width, (float)imgSize.Height);
			sourceImage.Draw(drawRect);
			var modifiedImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return modifiedImage;
		}

		public async Task UploadFile(string fileName, string regsessionid) //use Task<int> for return value
        {
			imagesUploading = true;
			context.InvokeOnMainThread(() => { StartAnim(); });

			try
            {
                string url;
                if (c.IsLoggedIn())
                {
                    url = Constants.HostName + "?action=uploadtouser&ID=" + Session.ID + "&SessionID=" + Session.SessionID;
					if (Constants.isTestDB)
					{
						url += Constants.TestDB;
					}
                }
                else
                {
                    url = (regsessionid == "") ? Constants.HostName + "?action=uploadtotemp" : Constants.HostName + "?action=uploadtotemp&regsessionid=" + regsessionid;
					if (Constants.isTestDB)
					{
						url += Constants.TestDB;
					}
                }

                await client.UploadFileTaskAsync(url, fileName);
            }
            catch (Exception ex)
            {
				context.InvokeOnMainThread(() => { StopAnim(); });

				imagesUploading = false;
                context.InvokeOnMainThread(() => {
                    c.ReportError(ex.Message + Environment.NewLine + ex.StackTrace);
                });
            }
        }

        private void Client_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                ImagesProgressText.Text = LangEnglish.ImagesProgressText;
            }
            else
            {
                ImagesProgressText.Text = LangEnglish.ImagesProgressTextPercent + " " + e.ProgressPercentage + "%";
            }
            ImagesProgress.Progress = e.ProgressPercentage;
        }

        private void Client_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            Console.WriteLine("Client_UploadFileCompleted " + imagesUploading);

			imagesUploading = false;
			StopAnim();

            try
            {
                string responseString = System.Text.Encoding.UTF8.GetString(e.Result);
                if (responseString.Substring(0, 2) == "OK")
                {
                    responseString = responseString.Substring(3);

                    string[] arr = responseString.Split(";");
                    string imgName = arr[0];
                    uploadedImages.Add(imgName);
                    if (c.IsLoggedIn())
                    {
                        Session.Pictures = uploadedImages.ToArray();
                    }
                    else
                    {
                        Console.WriteLine("--------upload finished-----");

                        RegisterActivity.regsessionid = arr[1];
                        if (!File.Exists(regSessionFile))
                        {
                            File.WriteAllText(regSessionFile, RegisterActivity.regsessionid);
                        }
                        ((RegisterActivity)context).SaveRegData();
                    }
                    ImagesUploaded.AddPicture(imgName, uploadedImages.Count - 1);

                }
                else if (responseString.Substring(0, 6) == "ERROR_")
                {
				    c.Snack(c.GetLang(responseString.Substring(6)));
			    }
                else
                {
                    c.ReportError(responseString);
                }                

                ImagesProgress.Progress = 0;
                if (uploadedImages.Count > 1)
                {
                    ImagesProgressText.Text = LangEnglish.ImagesRearrange;
                }
                else
                {
                    ImagesProgressText.Text = "";
                }
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }

        public void StartAnim()
        {
			Console.WriteLine("Anim started.");
            CABasicAnimation rotationAnimation = CABasicAnimation.FromKeyPath("transform.rotation");
            rotationAnimation.To = NSNumber.FromDouble(Math.PI * 2);
            rotationAnimation.RepeatCount = int.MaxValue;
            rotationAnimation.Duration = loaderAnimTime / 1000;

            LoaderCircle.Hidden = false;
            LoaderCircle.Layer.AddAnimation(rotationAnimation, "rotationAnimation");
        }

        public void StopAnim()
        {
            LoaderCircle.Hidden = true;
            LoaderCircle.Layer.RemoveAllAnimations();
        }

		public void UseLocationSwitch_Click(object sender, EventArgs e)
		{
			if (UseLocationSwitch.On)
			{
                if (!context.c.IsLocationEnabled())
                {
					UseLocationSwitch.On = false;
					RequestPermissions();
                }
				else
				{
					EnableLocationSwitches(true);
				}
			}
			else
			{
				EnableLocationSwitches(false);
			}
		}

		private void RequestPermissions()
		{
			CLLocationManager locationManager = new CLLocationManager();
			locationManager.AuthorizationChanged += LocationManager_AuthorizationChanged;

			//Location seems to be working in the background with this
			locationManager.RequestWhenInUseAuthorization();
            //locationManager.RequestAlwaysAuthorization();
		}

		private void LocationManager_AuthorizationChanged(object sender, CLAuthorizationChangedEventArgs e)
		{
			c.CW("LocationManager_AuthorizationChanged " + e.Status);
			if (e.Status == CLAuthorizationStatus.AuthorizedAlways || e.Status == CLAuthorizationStatus.AuthorizedWhenInUse)
			{
				UseLocationSwitch.On = true;
				EnableLocationSwitches(true);
			}
			else if (e.Status != CLAuthorizationStatus.NotDetermined)
			{
				context.c.Snack(LangEnglish.LocationNotGranted);
			}
		}

		public void EnableLocationSwitches(bool val)
        {
            LocationShareAll.Enabled = val;
            LocationShareLike.Enabled = val;
            LocationShareMatch.Enabled = val;
            LocationShareFriend.Enabled = val;
            LocationShareNone.Enabled = val;

            DistanceShareAll.Enabled = val;
            DistanceShareLike.Enabled = val;
            DistanceShareMatch.Enabled = val;
            DistanceShareFriend.Enabled = val;
            DistanceShareNone.Enabled = val;
        }

		public void LocationShareAll_Click(object sender, EventArgs e)
		{
			if (LocationShareAll.On)
			{
				LocationShareLike.On = true;
				LocationShareMatch.On = true;
				LocationShareFriend.On = true;
				LocationShareNone.On = false;
			}
		}

		public void LocationShareLike_Click(object sender, EventArgs e)
		{
			if (LocationShareLike.On)
			{
				LocationShareMatch.On = true;
				LocationShareFriend.On = true;
				LocationShareNone.On = false;
			}
			else
			{
				LocationShareAll.On = false;
			}
		}

		public void LocationShareMatch_Click(object sender, EventArgs e)
		{
			if (LocationShareMatch.On)
			{
				LocationShareFriend.On = true;
				LocationShareNone.On = false;
			}
			else
			{
				LocationShareAll.On = false;
				LocationShareLike.On = false;
			}
		}

		public void LocationShareFriend_Click(object sender, EventArgs e)
		{
			if (LocationShareFriend.On)
			{
				LocationShareNone.On = false;
			}
			else
			{
				LocationShareAll.On = false;
				LocationShareLike.On = false;
				LocationShareMatch.On = false;
			}
		}

		public void LocationShareNone_Click(object sender, EventArgs e)
		{
			if (LocationShareNone.On)
			{
				LocationShareAll.On = false;
				LocationShareLike.On = false;
				LocationShareMatch.On = false;
				LocationShareFriend.On = false;
			}
			else
			{
				LocationShareFriend.On = true;
			}
		}

		public void DistanceShareAll_Click(object sender, EventArgs e)
		{
			if (DistanceShareAll.On)
			{
				DistanceShareLike.On = true;
				DistanceShareMatch.On = true;
				DistanceShareFriend.On = true;
				DistanceShareNone.On = false;
			}
		}

		public void DistanceShareLike_Click(object sender, EventArgs e)
		{
			if (DistanceShareLike.On)
			{
				DistanceShareMatch.On = true;
				DistanceShareFriend.On = true;
				DistanceShareNone.On = false;
			}
			else
			{
				DistanceShareAll.On = false;
			}
		}

		public void DistanceShareMatch_Click(object sender, EventArgs e)
		{
			if (DistanceShareMatch.On)
			{
				DistanceShareFriend.On = true;
				DistanceShareNone.On = false;
			}
			else
			{
				DistanceShareAll.On = false;
				DistanceShareLike.On = false;
			}
		}

		public void DistanceShareFriend_Click(object sender, EventArgs e)
		{
			if (DistanceShareFriend.On)
			{
				DistanceShareNone.On = false;
			}
			else
			{
				DistanceShareAll.On = false;
				DistanceShareLike.On = false;
				DistanceShareMatch.On = false;
			}
		}

		public void DistanceShareNone_Click(object sender, EventArgs e)
		{
			if (DistanceShareNone.On)
			{
				DistanceShareAll.On = false;
				DistanceShareLike.On = false;
				DistanceShareMatch.On = false;
				DistanceShareFriend.On = false;
			}
			else
			{
				DistanceShareFriend.On = true;
			}
		}

		public int GetLocationShareLevel()
		{
			if (LocationShareAll.On)
			{
				return 4;
			}
			else if (LocationShareLike.On)
			{
				return 3;
			}
			else if (LocationShareMatch.On)
			{
				return 2;
			}
			else if (LocationShareFriend.On)
			{
				return 1;
			}
			else
			{
				return 0;
			}
		}

		public int GetDistanceShareLevel()
		{
			if (DistanceShareAll.On)
			{
				return 4;
			}
			else if (DistanceShareLike.On)
			{
				return 3;
			}
			else if (DistanceShareMatch.On)
			{
				return 2;
			}
			else if (DistanceShareFriend.On)
			{
				return 1;
			}
			else
			{
				return 0;
			}
		}

		public void SetLocationShareLevel(int level)
		{
			switch (level)
			{
				case 0:
					LocationShareNone.On = true;
					LocationShareFriend.On = false;
					LocationShareMatch.On = false;
					LocationShareLike.On = false;
					LocationShareAll.On = false;
					break;
				case 1:
					LocationShareNone.On = false;
					LocationShareFriend.On = true;
					LocationShareMatch.On = false;
					LocationShareLike.On = false;
					LocationShareAll.On = false;
					break;
				case 2:
					LocationShareNone.On = false;
					LocationShareFriend.On = true;
					LocationShareMatch.On = true;
					LocationShareLike.On = false;
					LocationShareAll.On = false;
					break;
				case 3:
					LocationShareNone.On = false;
					LocationShareFriend.On = true;
					LocationShareMatch.On = true;
					LocationShareLike.On = true;
					LocationShareAll.On = false;
					break;
				case 4:
					LocationShareNone.On = false;
					LocationShareFriend.On = true;
					LocationShareMatch.On = true;
					LocationShareLike.On = true;
					LocationShareAll.On = true;
					break;
			}
		}

		public void SetDistanceShareLevel(int level)
		{
			switch (level)
			{
				case 0:
					DistanceShareNone.On = true;
					DistanceShareFriend.On = false;
					DistanceShareMatch.On = false;
					DistanceShareLike.On = false;
					DistanceShareAll.On = false;
					break;
				case 1:
					DistanceShareNone.On = false;
					DistanceShareFriend.On = true;
					DistanceShareMatch.On = false;
					DistanceShareLike.On = false;
					DistanceShareAll.On = false;
					break;
				case 2:
					DistanceShareNone.On = false;
					DistanceShareFriend.On = true;
					DistanceShareMatch.On = true;
					DistanceShareLike.On = false;
					DistanceShareAll.On = false;
					break;
				case 3:
					DistanceShareNone.On = false;
					DistanceShareFriend.On = true;
					DistanceShareMatch.On = true;
					DistanceShareLike.On = true;
					DistanceShareAll.On = false;
					break;
				case 4:
					DistanceShareNone.On = false;
					DistanceShareFriend.On = true;
					DistanceShareMatch.On = true;
					DistanceShareLike.On = true;
					DistanceShareAll.On = true;
					break;
			}
		}
	}
}
