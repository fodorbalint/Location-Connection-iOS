using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MapKit;
using UIKit;

namespace LocationConnection
{
	class CustomAnnotationView : MKMapViewDelegate
	{
        string pId = "PinAnnotation";
        string iId = "ImageAnnotation";
        BaseActivity context;


        public CustomAnnotationView(BaseActivity context)
        {
            this.context = context;
        }

        public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
		{
            if (annotation is ProfileAnnotation) //profile image
            {
                MKAnnotationView imageView = mapView.DequeueReusableAnnotation(iId);
                if (imageView == null)
                {
                    imageView = new MKAnnotationView(annotation, iId);
                }
                
                ImageCache im = new ImageCache(context);
                im.LoadImage(imageView, ((ProfileAnnotation)annotation).UserID.ToString(), ((ProfileAnnotation)annotation).image);

                imageView.Frame = new CoreGraphics.CGRect(0, 0, (double)Settings.MapIconSize, (double)Settings.MapIconSize);

                return imageView;
            }
            else if (annotation is MKPointAnnotation) //list view circle center / profile view position marker
            {
                if (context is LocationActivity)
                {
                    MKAnnotationView imageView = mapView.DequeueReusableAnnotation(iId);
                    if (imageView == null)
                    {
                        imageView = new MKAnnotationView(annotation, iId);
                    }

                    imageView.Image = UIImage.FromBundle("ic_mapmarker.png");
                    imageView.Frame = new CoreGraphics.CGRect(0, 0, 30, 30);

                    return imageView;
                }
                else
                {
                    MKPinAnnotationView pinView = (MKPinAnnotationView)mapView.DequeueReusableAnnotation(pId);
                    if (pinView == null)
                    {
                        pinView = new MKPinAnnotationView(annotation, pId);
                    }

                    pinView.PinColor = MKPinAnnotationColor.Red;
                    pinView.CanShowCallout = false;

                    return pinView;
                }
                
            }
            else //user location. Class = MapKit.MKAnnotationWrapper, title = My Location
            {
                return null;
            }
                
        }

        public override void DidSelectAnnotationView(MKMapView mapView, MKAnnotationView view)
        {
            if (view.Annotation is ProfileAnnotation)
            {
                int ID = ((ProfileAnnotation)view.Annotation).UserID;

                int index;
                for (index = 0; index < ListActivity.listProfiles.Count; index++)
                {
                    if (ListActivity.listProfiles[index].ID == ID)
                    {
                        break;
                    }
                }

                IntentData.profileViewPageType = "list";
                ListActivity.viewIndex = index;
                ListActivity.absoluteIndex = index + (int)Session.ResultsFrom - 1;
                ListActivity.absoluteStartIndex = (int)Session.ResultsFrom - 1;

                CommonMethods.OpenPage("ProfileViewActivity", 1);
            }
        }

        public override MKOverlayView GetViewForOverlay(MKMapView mapView, IMKOverlay overlay)
        {
            if (context is LocationActivity)
            {
                var lineOverlay = overlay as MKPolyline;

                try
                { //null exception error on lineOverlay, when stepping back from location history
                    var lineView = new MKPolylineView(lineOverlay);

                    string title = lineOverlay.GetTitle();

                    if (!(title is null))
                    {
                        string[] colors = title.Split("|");
                        int red = int.Parse(colors[0]);
                        int green = int.Parse(colors[1]);
                        int blue = int.Parse(colors[2]);


                        lineView.StrokeColor = UIColor.FromRGB(red, green, blue);
                        lineView.LineWidth = 2f;
                    }
                    else
                    {
                        lineView.StrokeColor = UIColor.Black;
                        lineView.LineWidth = 3f;
                    }
                    return lineView;
                }
                catch
                {
                    context.c.ReportErrorSilent("Error at GetViewForOverlay lineOverlay: " + lineOverlay);
                    return new MKPolylineView();
                }                 
            }
            else
            {
                var circleOverlay = overlay as MKCircle;
                var circleView = new MKCircleView(circleOverlay);
                circleView.FillColor = UIColor.FromRGBA(0, 205, 0, 18);
                circleView.LineWidth = 0.5f;
                circleView.StrokeColor = UIColor.Black;
                return circleView;
            }            
        }
    }
}