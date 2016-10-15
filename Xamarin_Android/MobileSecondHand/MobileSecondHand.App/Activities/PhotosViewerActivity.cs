using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MobileSecondHand.App.Adapters;
using MobileSecondHand.App.Infrastructure.ActivityState;
using Newtonsoft.Json;

namespace MobileSecondHand.App.Activities
{
	[Activity(Label = "PhotosViewerActivity")]
	public class PhotosViewerActivity : AppCompatActivity
	{
		private List<byte[]> photosList;
		private RecyclerView photosRecycler;
		private int selectedPhotoIndex;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			this.Window.RequestFeature(WindowFeatures.NoTitle);
			this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
			SetContentView(Resource.Layout.PhotosViewerActivity);
			GetData(savedInstanceState);

			SetupViews();
		}

		protected override void OnSaveInstanceState(Bundle outState)
		{
			base.OnSaveInstanceState(outState);
			outState.PutInt(ActivityStateConsts.SELECTED_PHOTO_INDEX_TO_START_ON_PHOTOS_VIEWER, selectedPhotoIndex);
		}

		private void GetData(Bundle savedInstanceState)
		{
			if (savedInstanceState != null)
			{
				selectedPhotoIndex = savedInstanceState.GetInt(ActivityStateConsts.SELECTED_PHOTO_INDEX_TO_START_ON_PHOTOS_VIEWER, 0);
			}
			else
			{
				selectedPhotoIndex = Intent.GetIntExtra(ActivityStateConsts.SELECTED_PHOTO_INDEX_TO_START_ON_PHOTOS_VIEWER, 0);
			}

			this.photosList = (List<byte[]>)SharedObject.Data;
		}

		private void SetupViews()
		{
			var adpater = new AdvertisementPhotosListAdapter(this.photosList, zoomable: true);

			this.photosRecycler = FindViewById<RecyclerView>(Resource.Id.photosRecyclerViewOnPhotosViewer);
			var photoRecyclerLayoutManager = new LinearLayoutManager(this);
			photoRecyclerLayoutManager.Orientation = LinearLayoutManager.Horizontal;
			photosRecycler.SetLayoutManager(photoRecyclerLayoutManager);
			photosRecycler.SetAdapter(adpater);
			photosRecycler.ScrollToPosition(selectedPhotoIndex);
		}
	}
}