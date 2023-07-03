using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.SimpleLocalization
{
	/// <summary>
	/// Downloads spritesheets from Google Spreadsheet and saves them to Resources. My laziness made me to create it.
	/// </summary>
	[ExecuteInEditMode]
	public class LocalizationSync : MonoBehaviour
	{
		/// <summary>
		/// Table id on Google Spreadsheet.
		/// Let's say your table has the following url https://docs.google.com/spreadsheets/d/1RvKY3VE_y5FPhEECCa5dv4F7REJ7rBtGzQg9Z_B_DE4/edit#gid=331980525
		/// So your table id will be "1RvKY3VE_y5FPhEECCa5dv4F7REJ7rBtGzQg9Z_B_DE4" and sheet id will be "331980525" (gid parameter)
		/// </summary>
		public string TableId;

		/// <summary>
		/// Table sheet contains sheet name and id. First sheet has always zero id. Sheet name is used when saving.
		/// </summary>
		public Sheet[] Sheets;

		/// <summary>
		/// Folder to save spreadsheets. Must be inside Resources folder.
		/// </summary>
		public UnityEngine.Object SaveFolder;

		private const string UrlPattern = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}";

		#if UNITY_EDITOR

		/// <summary>
		/// Sync spreadsheets.
		/// </summary>
		public void Sync()
		{
			StopAllCoroutines();
			StartCoroutine(SyncCoroutine());
		}

		private IEnumerator SyncCoroutine()
		{
			var folder = UnityEditor.AssetDatabase.GetAssetPath(SaveFolder);

			Debug.Log("<color=yellow>Localization sync started...</color>");

			var dict = new Dictionary<string, UnityWebRequest>();

			foreach (var sheet in Sheets)
			{
				var url = string.Format(UrlPattern, TableId, sheet.Id);

				Debug.Log($"Downloading: {url}...");

				dict.Add(url, UnityWebRequest.Get(url));
			}

			foreach (var entry in dict)
            {
                var url = entry.Key;
                var request = entry.Value;

				if (!request.isDone)
				{
					yield return request.SendWebRequest();
				}

				if (request.error == null)
				{
					var sheet = Sheets.Single(i => url == string.Format(UrlPattern, TableId, i.Id));
					var path = System.IO.Path.Combine(folder, sheet.Name + ".csv");

					System.IO.File.WriteAllBytes(path, request.downloadHandler.data);
					Debug.LogFormat("Sheet {0} downloaded to <color=grey>{1}</color>", sheet.Id, path);
				}
				else
				{
					throw new Exception(request.error);
				}
			}

            UnityEditor.AssetDatabase.Refresh();

			Debug.Log("<color=yellow>Localization sync completed!</color>");
		}

		#endif
	}
}