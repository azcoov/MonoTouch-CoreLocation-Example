// Map code borrowed from concept dev https://github.com/conceptdev/Monospace09
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;

namespace MonoTouchCoreLocationExample
{
	public class MapLocation
	{
		public string Title {get;set;}
		public CLLocationCoordinate2D Location {get;set;}
	}
	public class MapLocationViewController : UITableViewController
	{
		private UITableView tableView;
		private UINavigationBar navBar;
		
		private List<MapLocation> _locations;
		public MapFlipViewController FlipController = null;
		public MapLocationViewController (MapFlipViewController mfvc) : base()
		{
			FlipController = mfvc;
			
			_locations = new List<MapLocation>();
			_locations.Add(new MapLocation{Title="Hero's", Location = new CLLocationCoordinate2D(33.4683,-111.682974)});//Location of my favorite watering hole
			_locations.Add(new MapLocation{Title="Your location", Location = new CLLocationCoordinate2D(0,0)});
		}
		
	 	public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
		
			// no XIB !
			navBar = new UINavigationBar();
			navBar.PushNavigationItem (new UINavigationItem("Choose Location"), false);
			navBar.BarStyle = UIBarStyle.Black;
			navBar.Frame = new RectangleF(0,0,this.View.Frame.Width,45);
			navBar.TopItem.RightBarButtonItem = new UIBarButtonItem("Done",UIBarButtonItemStyle.Bordered, delegate {FlipController.Flip();});
			tableView = new UITableView()
			{
				Delegate = new TableViewDelegate(this, _locations),
			    DataSource = new TableViewDataSource(this, _locations),
			    AutoresizingMask = UIViewAutoresizing.FlexibleHeight|
			                       UIViewAutoresizing.FlexibleWidth,
				Frame = new RectangleF (0, 45, this.View.Frame.Width, this.View.Frame.Height-44)
				, TableHeaderView = navBar, 
				BackgroundColor = UIColor.Black,
			};
			
			//quick hack to make cell text white
			foreach (var item in tableView.VisibleCells) {
				item.TextLabel.TextColor = UIColor.White;
			}
			
			// Set the table view to fit the width of the app.
			tableView.SizeToFit();
			
			// Reposition and resize the receiver
			tableView.Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height);
			
			// Add the table view as a subview
			this.View.AddSubview(tableView);
		}
		
		
        private class TableViewDelegate : UITableViewDelegate
        {
			private MapLocationViewController _dvc;
			private List<MapLocation> _locations;
            public TableViewDelegate(MapLocationViewController controller, List<MapLocation> locations)
            {
				_dvc = controller;
				_locations = locations;
            }

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
            {
				var loc = _locations[indexPath.Row];
				
                Console.WriteLine("RowSelected: Label=" + loc.Title);
				
				_dvc.FlipController.Flip(_locations[indexPath.Row].Location);
			}
        }

        private class TableViewDataSource : UITableViewDataSource
        {
            static NSString kCellIdentifier = new NSString ("MyLocationIdentifier");

			private MapLocationViewController _dvc;
			private List<MapLocation> _locations;
            public TableViewDataSource (MapLocationViewController controller, List<MapLocation> dates)
            {
				_dvc = controller;
				_locations = dates;
            }

            public override int RowsInSection (UITableView tableview, int section)
            {
                return _locations.Count;
            }

            public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
                UITableViewCell cell = tableView.DequeueReusableCell (kCellIdentifier);
                if (cell == null)
                {
                    cell = new UITableViewCell (UITableViewCellStyle.Default, kCellIdentifier);
                }
				
                cell.TextLabel.Text = _locations[indexPath.Row].Title;
				cell.Accessory = UITableViewCellAccessory.None;
                return cell;
            }
        }
	}
}
