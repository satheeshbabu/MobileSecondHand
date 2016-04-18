﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MobileSecondHand.Common.Keywords;
using MobileSecondHand.Db.Models.Advertisement.Keywords;
using MobileSecondHand.Db.Services.Advertisement.Keywords;

namespace MobileSecondHand.Api.Services.Advertisement.Keywords {
	public class KeywordsService : IKeywordsService {
		KeywordsHelper keywordsHelper;
		IKeywordsDbService keywordsService;

		public KeywordsService(IKeywordsDbService keywordsService) {
			keywordsHelper = new KeywordsHelper();
			this.keywordsService = keywordsService;
		}
		public ICollection<T> RecognizeAndGetKeywords<T>(string textToRecognize) where T : IKeyword {
			IEnumerable<string> keywordsStringList = RecognizeAndGetStringCollectionKeywords<T>(textToRecognize);
			var keywordsFromDb = this.keywordsService.GetKeywordsByNames<T>(keywordsStringList).ToList();

			if (keywordsFromDb.Count > keywordsStringList.Count()) {
				keywordsFromDb = SaveNewKeywordsToDb(keywordsStringList, keywordsFromDb);
			}

			return keywordsFromDb;
		}

		private List<T> SaveNewKeywordsToDb<T>(IEnumerable<string> keywordsStringList, List<T> keywordsFromDb) where T : IKeyword {
			var keywordsFromDbNames = keywordsFromDb.Select(k => k.Name);
			var nonStoredKeywordsNames = keywordsStringList.Except(keywordsFromDbNames);
			foreach (var newKeywordName in nonStoredKeywordsNames) {
				if (typeof(T) == typeof(CategoryKeyword)) {
					var categoryKeyword = new CategoryKeyword { Name = newKeywordName };
					this.keywordsService.AddCategoryKeywordToContext(categoryKeyword);
				}
				else if (typeof(T) == typeof(ColorKeyword)) {
					var colorKeyword = new ColorKeyword { Name = newKeywordName };
					this.keywordsService.AddColorKeywordToContext(colorKeyword);
				}
			}
			this.keywordsService.SaveChanges();
			keywordsFromDb = this.keywordsService.GetKeywordsByNames<T>(keywordsStringList).ToList();
			return keywordsFromDb;
		}

		private IEnumerable<string> RecognizeAndGetStringCollectionKeywords<T>(string textToRecognize) {
			var keywords = new List<string>();
			textToRecognize = textToRecognize.ToLower();

			if (typeof(T) == typeof(CategoryKeyword)) {
				foreach (var key in keywordsHelper.CategoryKeywords.Keys) {
					if (textToRecognize.Contains(key.ToLower())) {
						foreach (var sample in keywordsHelper.CategoryKeywords[key]) {
							if (textToRecognize.Contains(sample)) {
								keywords.Add(keywordsHelper.CategoryKeywords[key][0]);
							}
						}
					}
				}
			}
			else if (typeof(T) == typeof(ColorKeyword)) {
				foreach (var key in keywordsHelper.ColorKeywords.Keys) {
					if (textToRecognize.Contains(key.ToLower())) {
						foreach (var sample in keywordsHelper.ColorKeywords[key]) {
							if (textToRecognize.Contains(sample)) {
								keywords.Add(keywordsHelper.ColorKeywords[key][0]);
							}
						}
					}
				}
			}

			return keywords;
		}
	}
}
