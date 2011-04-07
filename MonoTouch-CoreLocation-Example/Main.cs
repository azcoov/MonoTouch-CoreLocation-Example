using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.CoreLocation;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MonoTouchCoreLocationExample
{
	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args);
		}
	}

	public partial class AppDelegate : UIApplicationDelegate
	{
		const string footer = "This is an example using MonoTouch.Dialog and MonoTouch.CoreLocation";
		RootElement demoRoot;
		DialogViewController controller;
		
		bool Busy {
			get {
				return UIApplication.SharedApplication.NetworkActivityIndicatorVisible;
			}
			set {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = value;
			}
		}

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window.AddSubview (navigationController.View);

			demoRoot = CreateRoot(String.Empty, String.Empty);
			controller = new DialogViewController (demoRoot) {
				Autorotate = true
			};
			navigationController.PushViewController (controller, true);				

			window.MakeKeyAndVisible ();

			return true;
		}

		public void GetLocation () 
		{
			if (Busy)
				return;			
			Busy = true;
			
			Util.RequestLocation (newLocation => {
				controller.Root = CreateRoot(newLocation.Coordinate.Latitude.ToString(), newLocation.Coordinate.Longitude.ToString());
				controller.ReloadData();
				Busy = false;
			});
		}
		
		public void ClearLocation () 
		{
			demoRoot = CreateRoot(String.Empty, String.Empty);
			controller.Root = demoRoot;
			controller.ReloadData();
		}
		
		RootElement CreateRoot (String lat, String lng)
		{
		    var rootElement = new RootElement ("Location Example"){
				new Section ("Latitude/Longitude data", footer){
					new StyledStringElement("Latitude", lat),
					new StyledStringElement ("Longitude", lng)
				},
				new Section() {
					new StringElement ("Get location", GetLocation),
					new StringElement ("Clear location", ClearLocation)
				}
			};
		    return rootElement;
		}
	}
}