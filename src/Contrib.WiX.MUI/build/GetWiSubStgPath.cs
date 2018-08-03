using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Contrib.WiX.MUI
{
  public partial class GetWiSubStgPath : Task
  {
    public const string WisubstgFileName = "WiSubStg.vbs";

    [Output]
    public virtual string Path { get; set; }

    /// <inheritdoc />
    public override bool Execute()
    {
      this.Path = ToolLocationHelper.GetPathToWindowsSdkFile(GetWiSubStgPath.WisubstgFileName,
                                                             TargetDotNetFrameworkVersion.VersionLatest,
                                                             VisualStudioVersion.VersionLatest,
                                                             DotNetFrameworkArchitecture.Current);

      var result = !string.IsNullOrEmpty(this.Path);

      return result;
    }
  }
}
