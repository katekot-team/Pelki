using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.SimpleLocalization
{
	/// <summary>
	/// Localization manager.
	/// </summary>
    public static class LocalizationManager
    {
		/// <summary>
		/// Fired when localization changed.
		/// </summary>
        public static event Action LocalizationChanged = () => { }; 

        public static readonly Dictionary<string, Dictionary<string, string>> Dictionary = new Dictionary<string, Dictionary<string, string>>();
        private static string _language = "English";

		/// <summary>
		/// Get or set language.
		/// </summary>
        public static string Language
        {
            get => _language;
            set { _language = value; LocalizationChanged(); }
        }

		/// <summary>
		/// Set default language.
		/// </summary>
        public static void AutoLanguage()
        {
            Language = "English";
        }

		/// <summary>
		/// Read localization spreadsheets.
		/// </summary>
		public static void Read(string path = "Localization")
        {
            if (Dictionary.Count > 0) return;

            var textAssets = Resources.LoadAll<TextAsset>(path);

            foreach (var textAsset in textAssets)
            {
                var text = textAsset.text.Replace("\r\n", "\n").Replace("\"\"", "[_quote_]");
                var matches = Regex.Matches(text, "\"[\\s\\S]+?\"");

                foreach (Match match in matches)
                {
					text = text.Replace(match.Value, match.Value.Replace("\"", null).Replace(",", "[_comma_]").Replace("\n", "[_newline_]"));
                }

                // Making uGUI line breaks to work in asian texts.
                text = text.Replace("。", "。 ").Replace("、", "、 ").Replace("：", "： ").Replace("！", "！ ").Replace("（", " （").Replace("）", "） ").Trim();

                var lines = text.Split('\n').Where(i => i != "").ToList();
				var languages = lines[0].Split(',').Select(i => i.Trim()).ToList();

				for (var i = 1; i < languages.Count; i++)
                {
                    if (!Dictionary.ContainsKey(languages[i]))
                    {
                        Dictionary.Add(languages[i], new Dictionary<string, string>());
                    }
                }
				
                for (var i = 1; i < lines.Count; i++)
                {
					var columns = lines[i].Split(',').Select(j => j.Trim()).Select(j => j.Replace("[_quote_]", "\"").Replace("[_comma_]", ",").Replace("[_newline_]", "\n")).ToList();
					var key = columns[0];

                    if (key == "") continue;

                    for (var j = 1; j < languages.Count; j++)
                    {
                        Dictionary[languages[j]].Add(key, columns[j]);
                    }
                }
            }

            AutoLanguage();
        }

        /// <summary>
        /// Check if a key exists in localization.
        /// </summary>
        public static bool HasKey(string localizationKey)
        {
            return Dictionary[Language].ContainsKey(localizationKey);
        }

        /// <summary>
        /// Get localized value by localization key.
        /// </summary>
        public static string Localize(string localizationKey)
        {
            if (Dictionary.Count == 0)
            {
                Read();
            }

            if (!Dictionary.ContainsKey(Language)) throw new KeyNotFoundException("Language not found: " + Language);

            var missed = !Dictionary[Language].ContainsKey(localizationKey) || Dictionary[Language][localizationKey] == "";

            if (missed)
            {
                Debug.LogWarning($"Translation not found: {localizationKey} ({Language}).");

                return Dictionary["English"].ContainsKey(localizationKey) ? Dictionary["English"][localizationKey] : localizationKey;
            }

            return Dictionary[Language][localizationKey];
        }

	    /// <summary>
	    /// Get localized value by localization key.
	    /// </summary>
		public static string Localize(string localizationKey, params object[] args)
        {
            var pattern = Localize(localizationKey);

            return string.Format(pattern, args);
        }
    }
}