﻿using System.Collections.Generic;
using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Controls.Internals;

#if UITEST
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using NUnit.Framework;
#endif

namespace Microsoft.Maui.Controls.ControlGallery.Issues
{
#if UITEST
	[NUnit.Framework.Category(Compatibility.UITests.UITestCategories.Github5000)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 2499, "Binding Context set to Null in Picker", PlatformAffected.All)]
	public class Issue2499 : TestContentPage
	{
		protected override void Init()
		{
			var _picker = new Picker()
			{
				ItemsSource = new List<string> { "cat", "mouse", "rabbit" },
				AutomationId = "picker",
			};
			_picker.SelectedIndexChanged += (_, __) => _picker.ItemsSource = null;

			Content = new StackLayout()
			{
				Children =
				{
					_picker
				}
			};
		}

#if UITEST
		[Test]
		public void Issue2499Test()
		{
			RunningApp.WaitForElement("picker");
			RunningApp.Tap("picker");
			RunningApp.WaitForElement("cat");

			AppResult[] items = RunningApp.Query("cat");
			Assert.AreNotEqual(items.Length, 0);
			RunningApp.WaitForElement(q => q.Marked("mouse"));
			RunningApp.Tap("mouse");
#if __IOS__
			System.Threading.Tasks.Task.Delay(500).Wait();
			var cancelButtonText = "Done";
			RunningApp.WaitForElement(q => q.Marked(cancelButtonText));
			RunningApp.Tap(q => q.Marked(cancelButtonText));
			System.Threading.Tasks.Task.Delay(1000).Wait();
#endif
			items = RunningApp.Query("cat");
			Assert.AreEqual(items.Length, 0);
		}
#endif
	}
}