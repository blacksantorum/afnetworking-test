using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using AFNetworking;

namespace AFNetworkingTest
{
	public partial class AFNetworkingTestViewController : UIViewController
	{
		public AFNetworkingTestViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var parameters = new NSDictionary ("auth_token", "md4CwfEL24FGJo4FFqcY");

			var url = new NSString ("upload_policies/new");

			AFHTTPRequestOperationManager manager = new AFHTTPRequestOperationManager(new NSUrl("http://fandings-staging.herokuapp.com/api/v1/"));
			manager.GET(url,parameters, (
			(AFHTTPRequestOperation operation, NSObject postData) => 
				{
					Console.WriteLine(postData.ToString());
					var postUrl = (NSString)postData.ValueForKey(new NSString ("post_url"));
					var fields = (NSDictionary)postData.ValueForKey(new NSString("fields"));

					var mutableFields = new NSMutableDictionary(fields);
				
					mutableFields["AWSAccessKeyId"] = mutableFields["aws_access_key_id"];
					mutableFields.Remove(new NSString("aws_access_key_id"));

					mutableFields["Content-Type"] = new NSString ("image/jpeg");

					UIImageView imageView = new UIImageView(UIImage.FromFile("voodoo.jpg"));
					imageView.Frame = View.Frame;
					Add(imageView);

					manager.POST(postUrl,mutableFields, (
						(AFStreamingMultipartFormData formData) => {
							formData.AppendPartWithFileData(imageView.Image.AsJPEG(),new NSString ("file"), new NSString("photo.jpg"), new NSString("image/jpeg"));
						}
					), (
						(AFHTTPRequestOperation uploadOperation, NSObject uploadPostData) => {
							Console.WriteLine(uploadOperation.ResponseObject.ToString());
							Console.WriteLine(uploadPostData.ToString());
						}
					), (
						(AFHTTPRequestOperation uploadOperation, NSError error) => {
							Console.WriteLine(uploadOperation.ResponseObject.ToString());
							Console.WriteLine(error.ToString());
						}
					)
					);
				}
			), 
			((AFHTTPRequestOperation operation, NSError error) => 
				{
				Console.WriteLine(error.ToString());
				}
			)
			);
		}
	

























		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}
}

