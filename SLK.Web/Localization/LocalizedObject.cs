﻿using Knoema.Localization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLK.Web.Localization
{
	public class LocalizedObject: ILocalizedObject
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Key { get; set; }

		public int LocaleId {get; set;}

		public string Scope {get; set;}

		public string Text {get; set;}

		public string Hash {get; set;}

		public string Translation {get; set;}
	}
}
