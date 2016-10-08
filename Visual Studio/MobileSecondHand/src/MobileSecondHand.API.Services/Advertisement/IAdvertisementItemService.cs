﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MobileSecondHand.API.Models.Shared.Advertisements;
using MobileSecondHand.API.Models.Shared.Location;

namespace MobileSecondHand.API.Services.Advertisement
{
	public interface IAdvertisementItemService {
		void CreateNewAdvertisementItem(NewAdvertisementItem newAdvertisementModel, string userId);
		Task<IEnumerable<AdvertisementItemShort>> GetAdvertisements(AdvertisementsSearchModel searchModel, string userId);
		Task<AdvertisementItemDetails> GetAdvertisementDetails(int advertisementId, string userId);
		Task<IEnumerable<AdvertisementItemShort>> GetUserAdvertisements(string userId, int pageNumber);
		bool CheckForNewAdvertisementsSinceLastCheck(string userId, CoordinatesForAdvertisementsModel coordinatesModel, bool currentLocation);
		bool DeleteAdvertisement(int advertisementId, string userId);
		bool AddToUserFavourites(string userId, int advertisementId);
		Task<IEnumerable<AdvertisementItemShort>> GetUserFavouritesAdvertisements(string userId, int pageNumber);
		bool DeleteAdvertisementFromFavourites(int advertisementId, string userId);
	}
}