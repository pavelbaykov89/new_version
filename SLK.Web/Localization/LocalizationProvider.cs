using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using Knoema.Localization;
using SLK.DataLayer;
using SLK.Web.Localization;

namespace SLK.Web.Localization
{ 
	public class LocalizationProvider : ILocalizationProvider
	{
        private readonly LocalizationContext _context;

        public LocalizationProvider(LocalizationContext context)
        {
            _context = context;
        }

        public IEnumerable<ILocalizedObject> GetAll(CultureInfo culture)
		{
			var result = LocalizationCache.Get<IEnumerable<ILocalizedObject>>(culture.Name);

			if (result == null || !result.Any())
			{
				
				result = _context.Objects.Where(obj => obj.LocaleId == culture.LCID && obj.Scope.StartsWith("~/")).ToList();				

				LocalizationCache.Set(culture.Name, result);
			}
		
			return result;
		}

		public ILocalizedObject Get(int key)
		{
			var obj = LocalizationCache.Get<ILocalizedObject>(key.ToString());
			
			if (obj == null)
			{				
				obj = _context.Objects.SingleOrDefault(o => o.Key == key);
				
				LocalizationCache.Set(key.ToString(), obj);
			}

			return obj;
		}

		public ILocalizedObject Get(CultureInfo culture, string scope, string text)
		{
			return GetAll(culture).FirstOrDefault(x => x.Hash == GetHash(scope, text));
		}

		public void Delete(params ILocalizedObject[] list)
		{
			
			foreach (var obj in list)
			{
				var stored = _context.Objects.Where(x => x.Key == obj.Key).FirstOrDefault();
				if (stored != null)
					_context.Objects.Remove(stored as LocalizedObject);
			}

			_context.SaveChanges();
			
			LocalizationCache.Clear();
		}

		public void Save(params ILocalizedObject[] list)
		{			
			_context.Objects.AddOrUpdate(list.Cast<LocalizedObject>().ToArray());
			_context.SaveChanges();
			
			LocalizationCache.Clear();
		}

		public IEnumerable<CultureInfo> GetCultures()
		{	
			return _context.Objects.Select(x => x.LocaleId).ToList().Distinct().Select(x => new CultureInfo(x)).ToList();		
		}

		public ILocalizedObject Create(CultureInfo culture, string scope, string text)
		{
			return new LocalizedObject
			{
				Hash = GetHash(scope, text),
				LocaleId = culture.LCID,
				Scope = scope,
				Text = text
			};
		}

		private string GetHash(string scope, string text)
		{
			var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(scope.ToLowerInvariant() + text));
			var stringBuilder = new StringBuilder();

			for (var i = 0; i < hash.Length; i++)
				stringBuilder.Append(hash[i].ToString("x2"));

			return stringBuilder.ToString();
		}
		
		public string GetRoot()
		{
			return null;
		}
	}
}
