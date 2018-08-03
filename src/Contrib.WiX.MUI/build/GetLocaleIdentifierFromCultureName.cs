using System;
using System.Globalization;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Contrib.WiX.MUI
{
  public partial class GetLocaleIdentifierFromCultureName : Task
  {
    [Required]
    public virtual string CultureName { get; set; }

    [Output]
    public virtual int LocaleIdentifier { get; set; }

    /// <inheritdoc />
    public override bool Execute()
    {
      var cultureName = this.CultureName;
      if (string.IsNullOrEmpty(cultureName))
      {
        this.LocaleIdentifier = 0;
      }
      else if (string.Equals(cultureName,
                             "neutral",
                             StringComparison.OrdinalIgnoreCase))
      {
        this.LocaleIdentifier = 0;
      }
      else
      {
        var cultureInfo = CultureInfo.GetCultureInfo(cultureName);
        this.LocaleIdentifier = cultureInfo.LCID;
      }

      var result = this.LocaleIdentifier >= 0;

      return result;
    }
  }
}
