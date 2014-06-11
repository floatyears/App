/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace Soomla {

	/// <summary>
	/// <c>StoreController</c> for Android. 
	/// This class holds the basic assets needed to operate the Store.
	/// You can use it to purchase products from the mobile store.
	/// This is the only class you need to initialize in order to use the SOOMLA SDK.
	/// </summary>
	public class StoreControllerAndroid : StoreController {

#if UNITY_ANDROID && !UNITY_EDITOR
		private static AndroidJavaObject jniStoreController = null;

		/// <summary>
		/// Initializes the SOOMLA SDK.
		/// </summary>
		/// <param name="storeAssets">Your game's economy.</param>
		/// <exception cref="ExitGUIException">Thrown if customSecret or soomSec is missing or has not been changed.
		/// </exception>
		protected override void _initialize(IStoreAssets storeAssets) {
			if (SoomSettings.GPlayBP && 
			    (string.IsNullOrEmpty(SoomSettings.AndroidPublicKey) ||
			 		SoomSettings.AndroidPublicKey==SoomSettings.AND_PUB_KEY_DEFAULT)) {
				Utils.LogError(TAG, "SOOMLA/UNITY You chose Google Play billing service but publicKey is not set!! Stopping here!!");
				throw new ExitGUIException();
			}

			StoreInfo.Initialize(storeAssets);

			AndroidJNI.PushLocalFrame(100);
			//init EventHandler
			using(AndroidJavaClass jniEventHandler = new AndroidJavaClass("com.soomla.unity.EventHandler")) {
				jniEventHandler.CallStatic("initialize");
			}
			using(AndroidJavaObject jniStoreAssetsInstance = new AndroidJavaObject("com.soomla.unity.StoreAssets")) {
				using(AndroidJavaClass jniStoreControllerClass = new AndroidJavaClass("com.soomla.store.StoreController")) {
					jniStoreController = jniStoreControllerClass.CallStatic<AndroidJavaObject>("getInstance");
					jniStoreController.Call<bool>("initialize", jniStoreAssetsInstance, SoomSettings.CustomSecret);
				}
			}

			using(AndroidJavaClass jniStoreConfigClass = new AndroidJavaClass("com.soomla.store.StoreConfig")) {
				jniStoreConfigClass.SetStatic("logDebug", SoomSettings.DebugMessages);
			}

			if (SoomSettings.GPlayBP) {
				using(AndroidJavaClass jniGooglePlayIabServiceClass = new AndroidJavaClass("com.soomla.store.billing.google.GooglePlayIabService")) {
					AndroidJavaObject jniGooglePlayIabService = jniGooglePlayIabServiceClass.CallStatic<AndroidJavaObject>("getInstance");
					jniGooglePlayIabService.Call("setPublicKey", SoomSettings.AndroidPublicKey);

					jniGooglePlayIabServiceClass.SetStatic("AllowAndroidTestPurchases", SoomSettings.AndroidTestPurchases);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		/// <summary>
		/// Sets up SoomSec.
		/// </summary>
		protected override void _setupSoomSec() {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStoreAssets = new AndroidJavaClass("com.soomla.unity.StoreAssets")) {
				jniStoreAssets.CallStatic("setSoomSec", SoomSettings.SoomSecret);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		/// <summary>
		/// Starts a purchase process in the market.
		/// </summary>
		/// <param name="productId">id of the item to buy.</param>
		protected override void _buyMarketItem(string productId, string payload) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaObject jniPurchasableItem = AndroidJNIHandler.CallStatic<AndroidJavaObject>(
				new AndroidJavaClass("com.soomla.store.data.StoreInfo"),"getPurchasableItem", productId)) {
				AndroidJNIHandler.CallVoid(jniStoreController, "buyWithMarket", 
				                           jniPurchasableItem.Call<AndroidJavaObject>("getPurchaseType").Call<AndroidJavaObject>("getMarketItem"), 
				                           payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		/// <summary>
		/// Creates a list of all metadata stored in the Market (the items that have been purchased).
		/// The metadata includes the item's name, description, price, product id, etc...
		/// </summary>
		protected override void _refreshInventory() {
			AndroidJNI.PushLocalFrame(100);
			jniStoreController.Call("refreshInventory");
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		/// <summary>
		/// Initiates the restore transactions process.
		/// </summary>
		protected override void _restoreTransactions() {
			AndroidJNI.PushLocalFrame(100);
			jniStoreController.Call("restoreTransactions");
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		/// <summary>
		/// Starts in-app billing service in background.
		/// </summary>
		protected override void _startIabServiceInBg() {
			AndroidJNI.PushLocalFrame(100);
			jniStoreController.Call("startIabServiceInBg");
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		/// <summary>
		/// Stops in-app billing service in background.
		/// </summary>
		protected override void _stopIabServiceInBg() {
			AndroidJNI.PushLocalFrame(100);
			jniStoreController.Call("stopIabServiceInBg");
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
#endif
	}
}
