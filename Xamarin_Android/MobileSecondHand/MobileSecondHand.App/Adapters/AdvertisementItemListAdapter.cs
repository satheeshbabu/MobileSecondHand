using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MobileSecondHand.App.Holders;
using MobileSecondHand.App.Infrastructure;
using MobileSecondHand.Common.Enumerations;
using MobileSecondHand.Models.Advertisement;
using MobileSecondHand.Models.EventArgs;

namespace MobileSecondHand.App.Adapters {
	public class AdvertisementItemListAdapter : RecyclerView.Adapter {
		Activity context;
		BitmapOperationService bitmapOperationService;
		IInfiniteScrollListener infiniteScrollListener;
		private int photoImageViewWitdth;
		private int photoImageViewHeight;
		private MainActivity mainActivity1;
		private List<AdvertisementItemShort> advertisements;
		private AdvertisementsKind advertisementsKind;
		private MainActivity mainActivity2;

		public event EventHandler<ShowAdvertisementDetailsEventArgs> AdvertisementItemClick;
		public bool InfiniteScrollDisabled { get; set; }
		public List<AdvertisementItemShort> AdvertisementItems { get; private set; }

		public AdvertisementItemListAdapter(Activity context, List<AdvertisementItemShort> advertisementItems, AdvertisementsKind advertisementsKind, IInfiniteScrollListener infiniteScrollListener) {
			this.AdvertisementItems = advertisementItems;
			this.context = context;
			this.bitmapOperationService = new BitmapOperationService();
			this.infiniteScrollListener = infiniteScrollListener;
			this.advertisementsKind = advertisementsKind;
			CalculateSizeForPhotoImageView();
		}


		public override int ItemCount
		{
			get { return this.AdvertisementItems.Count; }
		}

		public void AddAdvertisements(List<AdvertisementItemShort> advertisements) {
			CalculateSizeForPhotoImageView();
			this.AdvertisementItems.AddRange(advertisements);
			this.NotifyDataSetChanged();
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			var currentItem = this.AdvertisementItems[position];
			AdvertisementItemViewHolder vh = holder as AdvertisementItemViewHolder;
			if (advertisementsKind == AdvertisementsKind.AdvertisementsCreatedByUser)
			{
				vh.DeleteAdvertisementFab.Visibility = ViewStates.Visible;
			}
			else
			{
				vh.DeleteAdvertisementFab.Visibility = ViewStates.Invisible;
			}

			if (currentItem.IsSellerOnline)
			{
				vh.SellerChatStateImageView.SetBackgroundResource(Resource.Drawable.rounded_chat_state_online);
			}
			else
			{
				vh.SellerChatStateImageView.SetBackgroundResource(Resource.Drawable.rounded_chat_state_offline);
			}
			if (currentItem.IsOnlyForSell)
			{
				vh.AdvertisementKindTextView.Text = "tylko sprzeda�";
			}
			vh.DistanceTextView.Text = String.Format("{0} km", currentItem.Distance);
			vh.TitleTextView.Text = currentItem.AdvertisementTitle;
			vh.PriceTextView.Text = String.Format("{0} z�", currentItem.AdvertisementPrice);
			vh.PhotoImageView.SetImageBitmap(bitmapOperationService.ResizeImage(currentItem.MainPhoto, vh.PhotoImageView.Width > 0 ? vh.PhotoImageView.Width : photoImageViewWitdth, vh.PhotoImageView.Height > 0 ? vh.PhotoImageView.Height : photoImageViewHeight));
			RaiseOnInfiniteScrollWhenItemIsLastInList(currentItem, vh);
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.AdvertisementItemRowView, parent, false);
			AdvertisementItemViewHolder vh = new AdvertisementItemViewHolder(itemView, OnAdvertisementItemClick);
			return vh;
		}

		private void CalculateSizeForPhotoImageView() {
			var metrics = context.Resources.DisplayMetrics;
			var width = metrics.WidthPixels - 20;
			this.photoImageViewWitdth = width;
			this.photoImageViewHeight = (int)(width * 0.8);
		}

		private int ConvertPixelsToDp(float pixelValue) {
			var dp = (int)((pixelValue) / context.Resources.DisplayMetrics.Density);
			return dp;
		}

		private void OnAdvertisementItemClick(int positionId) {
			if (AdvertisementItemClick != null) {
				AdvertisementItemClick(this, new ShowAdvertisementDetailsEventArgs { Id = this.AdvertisementItems[positionId].Id, Distance = this.AdvertisementItems[positionId].Distance });
			}
		}

		private void RaiseOnInfiniteScrollWhenItemIsLastInList(AdvertisementItemShort currentItem, AdvertisementItemViewHolder viewHolder) {
			if (this.AdvertisementItems.IndexOf(currentItem) == (this.AdvertisementItems.Count - 1) && !InfiniteScrollDisabled) {
				this.infiniteScrollListener.OnInfiniteScroll();
			}
		}

	}
}