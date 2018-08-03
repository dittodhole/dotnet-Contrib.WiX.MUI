using System;
using System.Collections.Generic;
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

    [Output]
    public virtual ITaskItem[] EnrichedItems { get; set; }

    /// <inheritdoc />
    public override bool Execute()
    {
      var result = new List<ITaskItem>(this.Items.Length);

      foreach (var item in this.Items)
      {
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
          catch (CultureNotFoundException cultureNotFoundException)
          {
            this.Log.LogErrorFromException(cultureNotFoundException);

            return false;
          }
        }

        item.SetMetadata(AddLocaleIdentifierToItemMetadata.LocaleIdentifierMetadataItemName,
                         localeIdentifier.ToString());

        this.Log.LogMessage("Assigned LCID:        {0} --> {1}",
                            item.ItemSpec,
                            localeIdentifier);

        result.Add(item);
      }

      this.EnrichedItems = result.ToArray();

      return true;
    }
  }
}
