using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Contrib.WiX.MUI
{
  public partial class GetMsiTranPath : Task
  {
    public const string MsiTranFileName = "MsiTran.exe";

    [Output]
    public virtual string Path { get; set; }

    /// <inheritdoc />
    public override bool Execute()
    {
      this.Path = ToolLocationHelper.GetPathToWindowsSdkFile(GetMsiTranPath.MsiTranFileName,
                                                             TargetDotNetFrameworkVersion.VersionLatest,
                                                             VisualStudioVersion.VersionLatest,
                                                             DotNetFrameworkArchitecture.Current);

      var result = !string.IsNullOrEmpty(this.Path);

      return result;
    }
  }
}
