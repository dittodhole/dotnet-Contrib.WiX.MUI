using System;
using System.Globalization;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Contrib.WiX.MUI
{
  public partial class AddLocaleIdentifierToItemMetadata : Task
  {
    public const string CultureMetadataName = "Culture";
    public const string LocaleIdentifierMetadataItemName = "LocaleIdentifier";

    [Required]
    public virtual ITaskItem[] Items { get; set; }

    /// <inheritdoc />
    public override bool Execute()
    {
      var result = true;
      foreach (var item in this.Items)
      {
        this.Log.LogMessage(item.ItemSpec);

        int localeIdentifier;

        var culture = item.GetMetadata(AddLocaleIdentifierToItemMetadata.CultureMetadataName);
        if (string.IsNullOrEmpty(culture))
        {
          localeIdentifier = 0;
        }
        else if (string.Equals(culture,
                               "neutral",
                               StringComparison.OrdinalIgnoreCase))
        {
          localeIdentifier = 0;
        }
        else
        {
          try
          {
            var cultureInfo = CultureInfo.GetCultureInfo(culture);
            localeIdentifier = cultureInfo.LCID;
          }
          catch (CultureNotFoundException)
          {
            localeIdentifier = 0;
            result = false;
          }
        }

        item.SetMetadata(AddLocaleIdentifierToItemMetadata.LocaleIdentifierMetadataItemName,
                         localeIdentifier.ToString());

        this.Log.LogMessage("Culture '{0}' --> LCID '{1}'",
                            culture,
                            localeIdentifier);
      }
      return result;
    }
  }
}
