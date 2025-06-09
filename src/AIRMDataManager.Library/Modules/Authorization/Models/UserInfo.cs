using System.Collections.Generic;

namespace AIRMDataManager.Library.Modules.Authorization.Models;

// Original source: https://github.com/berhir/ReactAspnetCoreWebAssemblyCookieAuth.
public class UserInfo
{
    public static readonly UserInfo Anonymous = new UserInfo();

    public bool IsAuthenticated { get; set; }

    public string NameClaimType { get; set; }

    public string RoleClaimType { get; set; }

    public ICollection<ClaimValue> Claims { get; set; }
}
