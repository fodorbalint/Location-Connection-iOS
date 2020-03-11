using System;
using System.IO;
using Foundation;
using MapKit;
using UIKit;

namespace LocationConnection
{
    public class ImageCache
    {
        NSObject context;
        string cacheDir;

        public ImageCache(NSObject context)
        {
            this.context = context;

            var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
            cacheDir = Path.Combine (documents, "..", "Library/Caches");
        }

        public void LoadImage(UIView imageView, string userID, string picture, bool temp = false)
        {
            string url;
            if (!temp)
            {
                url = Constants.HostName + Constants.UploadFolder + "/" + userID + "/" + Constants.SmallImageSize + "/" + picture;
            }
            else
            {
                url = Constants.HostName + Constants.TempUploadFolder + "/" + userID + "/" + Constants.SmallImageSize + "/" + picture;
            }

            string saveName = userID + "_" + picture;

            if (Exists(saveName))
            {
                if (imageView is UIImageView)
                {
                    ((UIImageView)imageView).Image = Load(saveName);
                }
                else if (imageView is UIButton)
                {
                    ((UIButton)imageView).SetBackgroundImage(Load(saveName), UIControlState.Normal);
                }
                else if (imageView is MKAnnotationView)
                {
                    ((MKAnnotationView)imageView).Image = Load(saveName);
                }

            }
            else
            {
                if (imageView is UIImageView)
                {
                    ((UIImageView)imageView).Image = UIImage.FromBundle(Constants.loadingImage);
                }
                else if (imageView is UIButton)
                {
                    ((UIButton)imageView).SetBackgroundImage(UIImage.FromBundle(Constants.loadingImage), UIControlState.Normal);
                }
                else if (imageView is MKAnnotationView)
                {
                    ((MKAnnotationView)imageView).Image = UIImage.FromBundle(Constants.loadingImage);
                }

                CommonMethods.LoadFromUrlAsyncData(url).ContinueWith((task) => {
                    if (task.Result != null)
                    {
                        Save(saveName, task.Result);
                        context.InvokeOnMainThread(() => {
                            if (imageView is UIImageView)
                            {
                                ((UIImageView)imageView).Image = UIImage.LoadFromData(task.Result);
                            }
                            else if (imageView is UIButton)
                            {
                                ((UIButton)imageView).SetBackgroundImage(UIImage.LoadFromData(task.Result), UIControlState.Normal);
                            }
                            else if (imageView is MKAnnotationView)
                            {
                                ((MKAnnotationView)imageView).Image = UIImage.LoadFromData(task.Result);
                            }
                        });
                    }
                    else
                    {
                        context.InvokeOnMainThread(() => {
                            if (imageView is UIImageView)
                            {
                                ((UIImageView)imageView).Image = UIImage.FromBundle(Constants.noImage);
                            }
                            else if (imageView is UIButton)
                            {
                                ((UIButton)imageView).SetBackgroundImage(UIImage.FromBundle(Constants.noImage), UIControlState.Normal);
                            }
                            else if (imageView is MKAnnotationView)
                            {
                                ((MKAnnotationView)imageView).Image = UIImage.FromBundle(Constants.noImage);
                            }
                        });
                    }
                    
                });
            }
        }

        public void Save(string imageName, NSData data)
        {
            string fileName = Path.Combine(cacheDir, imageName);
            if (!data.Save(fileName, false, out NSError error))
            {
                Console.WriteLine("Image save error: " + error.LocalizedDescription);
            }
        }

        public UIImage Load(string imageName)
        {
            string fileName = Path.Combine(cacheDir, imageName);
            return UIImage.FromFile(fileName);
        }

        public bool Exists(string imageName)
        {
            string fileName = Path.Combine(cacheDir, imageName);
            return File.Exists(fileName);
        }
    }
}
