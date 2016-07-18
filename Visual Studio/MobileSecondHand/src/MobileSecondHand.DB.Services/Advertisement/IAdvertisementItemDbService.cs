﻿using System.Collections.Generic;
using MobileSecondHand.COMMON.Models;
using MobileSecondHand.DB.Models.Advertisement;

namespace MobileSecondHand.DB.Services.Advertisement {
	public interface IAdvertisementItemDbService
    {
		void SaveNewAdvertisementItem(AdvertisementItem advertisementItem);
		IEnumerable<AdvertisementItem> GetAdvertisementsFromDeclaredArea(CoordinatesForSearchingAdvertisementsModel coordinatesForSearchModel, int page);
		AdvertisementItem GetAdvertisementDetails(int advertisementId);
	}
}