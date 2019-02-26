using Foundation;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using UIKit;

namespace test_interpreter
{
	public partial class ViewController : UIViewController
	{
		Assembly mazeAssembly;

		public ViewController (IntPtr handle) : base (handle)
		{
		}


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		string DownloadAssembly()
		{
			string assemblyURL = @"https://github.com/codefoco/MazeCreator/blob/test_download/MazeCreator.dll?raw=true";
			var temp = Path.GetTempFileName ();
			try {
				var client = new WebClient ();
				client.DownloadFile (assemblyURL, temp);

				return temp;
			} catch (Exception e) {
				Fail ($"Could not download '{assemblyURL}' to '{temp}'.{Environment.NewLine}{e}");
				return null;
			}
		}

		void Fail(string message)
		{
			outputView.TextColor = UIColor.Red;
			outputView.Text += message;
		}

		void EnsureAssemblyLoaded()
		{
			if (mazeAssembly != null)
				return;

			string assemblyPath = DownloadAssembly ();
			if (assemblyPath == null)
				return;

			mazeAssembly = Assembly.LoadFile (assemblyPath);
		}

		void TryCreateMaze (int rows, int columns)
		{
			EnsureAssemblyLoaded ();

			if (mazeAssembly == null) {
				Fail ("Faill to load Assembly");
				return;
			}

			Type icreatorType = mazeAssembly.GetType ("MazeCreator.Core.ICreator");
			Type creatorType = mazeAssembly.GetType ("MazeCreator.Core.Creator");
			Type algorithmEnum = mazeAssembly.GetType ("MazeCreator.Core.Algorithm");

			object dfsValue = Enum.ToObject (algorithmEnum, 0);
			Type extensions = mazeAssembly.GetType ("MazeCreator.Extensions.MazeExtensions");

			MethodInfo[] members = creatorType.GetMethods (BindingFlags.Static | BindingFlags.Public);
			MethodInfo methodCreator = members[0];

#if true // TODO: Fix dynamic with --interpreter
			dynamic creator = methodCreator.Invoke (null, BindingFlags.Public | BindingFlags.Static, null, new object[] { dfsValue, null }, null);
			object maze = creator.Create (rows, columns);
#else
			object creator = methodCreator.Invoke (null, BindingFlags.Public | BindingFlags.Static, null, new object[] { dfsValue, null }, null);
			object maze = icreatorType.InvokeMember ("Create", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance , null, creator, new object[] { rows, columns });
#endif
			object sMaze = extensions.InvokeMember ("ToBoxString", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, new object [] { maze });

			outputView.Text = (string)sMaze;
		}

		partial void OnButtonClick (UIButton sender)
		{
			int rows = string.IsNullOrEmpty (Rows.Text) ? 10 : int.Parse (Rows.Text);
			int cols = string.IsNullOrEmpty (Columns.Text) ? 10 : int.Parse (Columns.Text);

			try {
				TryCreateMaze (rows, cols);
			}
			catch(Exception e) {
				Fail ("FAIL" + e.Message);
			}
		}

	}
}