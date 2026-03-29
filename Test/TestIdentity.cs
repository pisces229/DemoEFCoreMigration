using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Test;

[TestClass]
public class TestIdentity : BaseTest
{
    public override async Task TestCleanup()
    {
        var cancellationToken = default(CancellationToken);
        await _dbContext.Set<AspRoleClaim>().ExecuteDeleteAsync(cancellationToken);
        await _dbContext.Set<AspUserClaim>().ExecuteDeleteAsync(cancellationToken);
        await _dbContext.Set<AspUserLogin>().ExecuteDeleteAsync(cancellationToken);
        await _dbContext.Set<AspUserToken>().ExecuteDeleteAsync(cancellationToken);
        await _dbContext.Set<AspUserRole>().ExecuteDeleteAsync(cancellationToken);
        await _dbContext.Set<AspRole>().ExecuteDeleteAsync(cancellationToken);
        await _dbContext.Set<AspUser>().ExecuteDeleteAsync(cancellationToken);
        await base.TestCleanup();
    }

    #region User

    [TestMethod(DisplayName = "Identity_Write_User")]
    public async Task Write_User()
    {
        var cancellationToken = default(CancellationToken);

        var user = new AspUser
        {
            UserName = "testuser",
            NormalizedUserName = "TESTUSER",
            Email = "test@example.com",
            NormalizedEmail = "TEST@EXAMPLE.COM",
        };

        await _dbContext.Set<AspUser>().AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        Assert.AreNotEqual(Guid.Empty, user.Id);
        Console.WriteLine($"User Id={user.Id}, UserName={user.UserName}, Email={user.Email}");
    }

    [TestMethod(DisplayName = "Identity_Update_User")]
    public async Task Update_User()
    {
        var cancellationToken = default(CancellationToken);

        var user = new AspUser
        {
            UserName = "updateuser",
            NormalizedUserName = "UPDATEUSER",
            Email = "update@example.com",
            NormalizedEmail = "UPDATE@EXAMPLE.COM",
        };
        await _dbContext.Set<AspUser>().AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        user.PhoneNumber = "0912345678";
        user.EmailConfirmed = true;
        await _dbContext.SaveChangesAsync(cancellationToken);

        var found = await _dbContext.Set<AspUser>()
            .Where(u => u.Id == user.Id)
            .FirstOrDefaultAsync(cancellationToken);

        Assert.IsNotNull(found);
        Assert.AreEqual("0912345678", found.PhoneNumber);
        Assert.IsTrue(found.EmailConfirmed);
        Console.WriteLine($"Updated User Id={found.Id}, PhoneNumber={found.PhoneNumber}, EmailConfirmed={found.EmailConfirmed}");
    }

    [TestMethod(DisplayName = "Identity_Delete_User")]
    public async Task Delete_User()
    {
        var cancellationToken = default(CancellationToken);

        var user = new AspUser
        {
            UserName = "deleteuser",
            NormalizedUserName = "DELETEUSER",
            Email = "delete@example.com",
            NormalizedEmail = "DELETE@EXAMPLE.COM",
        };
        await _dbContext.Set<AspUser>().AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var deleted = await _dbContext.Set<AspUser>()
            .Where(u => u.Id == user.Id)
            .ExecuteDeleteAsync(cancellationToken);

        Assert.AreEqual(1, deleted);
        Console.WriteLine($"Deleted {deleted} user(s)");
    }

    #endregion

    #region Role

    [TestMethod(DisplayName = "Identity_Write_Role")]
    public async Task Write_Role()
    {
        var cancellationToken = default(CancellationToken);

        var role = new AspRole
        {
            Name = "Admin",
            NormalizedName = "ADMIN",
        };

        await _dbContext.Set<AspRole>().AddAsync(role, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        Assert.AreNotEqual(Guid.Empty, role.Id);
        Console.WriteLine($"Role Id={role.Id}, Name={role.Name}");
    }

    [TestMethod(DisplayName = "Identity_Update_Role")]
    public async Task Update_Role()
    {
        var cancellationToken = default(CancellationToken);

        var role = new AspRole
        {
            Name = "Editor",
            NormalizedName = "EDITOR",
        };
        await _dbContext.Set<AspRole>().AddAsync(role, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        role.Name = "SuperEditor";
        role.NormalizedName = "SUPEREDITOR";
        await _dbContext.SaveChangesAsync(cancellationToken);

        var found = await _dbContext.Set<AspRole>()
            .Where(r => r.Id == role.Id)
            .FirstOrDefaultAsync(cancellationToken);

        Assert.IsNotNull(found);
        Assert.AreEqual("SuperEditor", found.Name);
        Console.WriteLine($"Updated Role Id={found.Id}, Name={found.Name}");
    }

    [TestMethod(DisplayName = "Identity_Delete_Role")]
    public async Task Delete_Role()
    {
        var cancellationToken = default(CancellationToken);

        var role = new AspRole
        {
            Name = "TempRole",
            NormalizedName = "TEMPROLE",
        };
        await _dbContext.Set<AspRole>().AddAsync(role, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var deleted = await _dbContext.Set<AspRole>()
            .Where(r => r.Id == role.Id)
            .ExecuteDeleteAsync(cancellationToken);

        Assert.AreEqual(1, deleted);
        Console.WriteLine($"Deleted {deleted} role(s)");
    }

    #endregion

    #region UserRole

    [TestMethod(DisplayName = "Identity_Write_UserRole")]
    public async Task Write_UserRole()
    {
        var cancellationToken = default(CancellationToken);

        var user = new AspUser
        {
            UserName = "roleuser",
            NormalizedUserName = "ROLEUSER",
            Email = "roleuser@example.com",
            NormalizedEmail = "ROLEUSER@EXAMPLE.COM",
        };
        var role = new AspRole
        {
            Name = "Manager",
            NormalizedName = "MANAGER",
        };
        await _dbContext.Set<AspUser>().AddAsync(user, cancellationToken);
        await _dbContext.Set<AspRole>().AddAsync(role, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var userRole = new AspUserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
        };
        await _dbContext.Set<AspUserRole>().AddAsync(userRole, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        Console.WriteLine($"UserRole UserId={userRole.UserId}, RoleId={userRole.RoleId}");
    }

    [TestMethod(DisplayName = "Identity_Delete_UserRole")]
    public async Task Delete_UserRole()
    {
        var cancellationToken = default(CancellationToken);

        var user = new AspUser
        {
            UserName = "delroleuser",
            NormalizedUserName = "DELROLEUSER",
            Email = "delroleuser@example.com",
            NormalizedEmail = "DELROLEUSER@EXAMPLE.COM",
        };
        var role = new AspRole
        {
            Name = "TempManager",
            NormalizedName = "TEMPMANAGER",
        };
        await _dbContext.Set<AspUser>().AddAsync(user, cancellationToken);
        await _dbContext.Set<AspRole>().AddAsync(role, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var userRole = new AspUserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
        };
        await _dbContext.Set<AspUserRole>().AddAsync(userRole, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var deleted = await _dbContext.Set<AspUserRole>()
            .Where(ur => ur.UserId == user.Id && ur.RoleId == role.Id)
            .ExecuteDeleteAsync(cancellationToken);

        Assert.AreEqual(1, deleted);
        Console.WriteLine($"Deleted {deleted} user-role(s)");
    }

    #endregion

    #region UserClaim

    [TestMethod(DisplayName = "Identity_Write_UserClaim")]
    public async Task Write_UserClaim()
    {
        var cancellationToken = default(CancellationToken);

        var user = new AspUser
        {
            UserName = "claimuser",
            NormalizedUserName = "CLAIMUSER",
            Email = "claimuser@example.com",
            NormalizedEmail = "CLAIMUSER@EXAMPLE.COM",
        };
        await _dbContext.Set<AspUser>().AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var claim = new AspUserClaim
        {
            UserId = user.Id,
            ClaimType = "Department",
            ClaimValue = "Engineering",
        };
        await _dbContext.Set<AspUserClaim>().AddAsync(claim, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        Assert.AreNotEqual(0, claim.Id);
        Console.WriteLine($"UserClaim Id={claim.Id}, UserId={claim.UserId}, ClaimType={claim.ClaimType}, ClaimValue={claim.ClaimValue}");
    }

    [TestMethod(DisplayName = "Identity_Update_UserClaim")]
    public async Task Update_UserClaim()
    {
        var cancellationToken = default(CancellationToken);

        var user = new AspUser
        {
            UserName = "updclaimuser",
            NormalizedUserName = "UPDCLAIMUSER",
            Email = "updclaimuser@example.com",
            NormalizedEmail = "UPDCLAIMUSER@EXAMPLE.COM",
        };
        await _dbContext.Set<AspUser>().AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var claim = new AspUserClaim
        {
            UserId = user.Id,
            ClaimType = "Department",
            ClaimValue = "Engineering",
        };
        await _dbContext.Set<AspUserClaim>().AddAsync(claim, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        claim.ClaimValue = "Management";
        await _dbContext.SaveChangesAsync(cancellationToken);

        var found = await _dbContext.Set<AspUserClaim>()
            .Where(c => c.Id == claim.Id)
            .FirstOrDefaultAsync(cancellationToken);

        Assert.IsNotNull(found);
        Assert.AreEqual("Management", found.ClaimValue);
        Console.WriteLine($"Updated UserClaim Id={found.Id}, ClaimValue={found.ClaimValue}");
    }

    [TestMethod(DisplayName = "Identity_Delete_UserClaim")]
    public async Task Delete_UserClaim()
    {
        var cancellationToken = default(CancellationToken);

        var user = new AspUser
        {
            UserName = "delclaimuser",
            NormalizedUserName = "DELCLAIMUSER",
            Email = "delclaimuser@example.com",
            NormalizedEmail = "DELCLAIMUSER@EXAMPLE.COM",
        };
        await _dbContext.Set<AspUser>().AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var claim = new AspUserClaim
        {
            UserId = user.Id,
            ClaimType = "TempClaim",
            ClaimValue = "TempValue",
        };
        await _dbContext.Set<AspUserClaim>().AddAsync(claim, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var deleted = await _dbContext.Set<AspUserClaim>()
            .Where(c => c.Id == claim.Id)
            .ExecuteDeleteAsync(cancellationToken);

        Assert.AreEqual(1, deleted);
        Console.WriteLine($"Deleted {deleted} user-claim(s)");
    }

    #endregion

    #region RoleClaim

    [TestMethod(DisplayName = "Identity_Write_RoleClaim")]
    public async Task Write_RoleClaim()
    {
        var cancellationToken = default(CancellationToken);

        var role = new AspRole
        {
            Name = "ClaimRole",
            NormalizedName = "CLAIMROLE",
        };
        await _dbContext.Set<AspRole>().AddAsync(role, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var roleClaim = new AspRoleClaim
        {
            RoleId = role.Id,
            ClaimType = "Permission",
            ClaimValue = "CanEdit",
        };
        await _dbContext.Set<AspRoleClaim>().AddAsync(roleClaim, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        Assert.AreNotEqual(0, roleClaim.Id);
        Console.WriteLine($"RoleClaim Id={roleClaim.Id}, RoleId={roleClaim.RoleId}, ClaimType={roleClaim.ClaimType}, ClaimValue={roleClaim.ClaimValue}");
    }

    [TestMethod(DisplayName = "Identity_Update_RoleClaim")]
    public async Task Update_RoleClaim()
    {
        var cancellationToken = default(CancellationToken);

        var role = new AspRole
        {
            Name = "UpdClaimRole",
            NormalizedName = "UPDCLAIMROLE",
        };
        await _dbContext.Set<AspRole>().AddAsync(role, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var roleClaim = new AspRoleClaim
        {
            RoleId = role.Id,
            ClaimType = "Permission",
            ClaimValue = "CanEdit",
        };
        await _dbContext.Set<AspRoleClaim>().AddAsync(roleClaim, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        roleClaim.ClaimValue = "CanDelete";
        await _dbContext.SaveChangesAsync(cancellationToken);

        var found = await _dbContext.Set<AspRoleClaim>()
            .Where(rc => rc.Id == roleClaim.Id)
            .FirstOrDefaultAsync(cancellationToken);

        Assert.IsNotNull(found);
        Assert.AreEqual("CanDelete", found.ClaimValue);
        Console.WriteLine($"Updated RoleClaim Id={found.Id}, ClaimValue={found.ClaimValue}");
    }

    [TestMethod(DisplayName = "Identity_Delete_RoleClaim")]
    public async Task Delete_RoleClaim()
    {
        var cancellationToken = default(CancellationToken);

        var role = new AspRole
        {
            Name = "DelClaimRole",
            NormalizedName = "DELCLAIMROLE",
        };
        await _dbContext.Set<AspRole>().AddAsync(role, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var roleClaim = new AspRoleClaim
        {
            RoleId = role.Id,
            ClaimType = "TempPermission",
            ClaimValue = "TempValue",
        };
        await _dbContext.Set<AspRoleClaim>().AddAsync(roleClaim, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var deleted = await _dbContext.Set<AspRoleClaim>()
            .Where(rc => rc.Id == roleClaim.Id)
            .ExecuteDeleteAsync(cancellationToken);

        Assert.AreEqual(1, deleted);
        Console.WriteLine($"Deleted {deleted} role-claim(s)");
    }

    #endregion

    #region UserLogin

    [TestMethod(DisplayName = "Identity_Write_UserLogin")]
    public async Task Write_UserLogin()
    {
        var cancellationToken = default(CancellationToken);

        var user = new AspUser
        {
            UserName = "loginuser",
            NormalizedUserName = "LOGINUSER",
            Email = "loginuser@example.com",
            NormalizedEmail = "LOGINUSER@EXAMPLE.COM",
        };
        await _dbContext.Set<AspUser>().AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var login = new AspUserLogin
        {
            UserId = user.Id,
            LoginProvider = "Google",
            ProviderKey = "google-key-12345",
            ProviderDisplayName = "Google",
        };
        await _dbContext.Set<AspUserLogin>().AddAsync(login, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        Console.WriteLine($"UserLogin UserId={login.UserId}, LoginProvider={login.LoginProvider}, ProviderKey={login.ProviderKey}");
    }

    [TestMethod(DisplayName = "Identity_Delete_UserLogin")]
    public async Task Delete_UserLogin()
    {
        var cancellationToken = default(CancellationToken);

        var user = new AspUser
        {
            UserName = "delloginuser",
            NormalizedUserName = "DELLOGINUSER",
            Email = "delloginuser@example.com",
            NormalizedEmail = "DELLOGINUSER@EXAMPLE.COM",
        };
        await _dbContext.Set<AspUser>().AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var login = new AspUserLogin
        {
            UserId = user.Id,
            LoginProvider = "Facebook",
            ProviderKey = "fb-key-99999",
            ProviderDisplayName = "Facebook",
        };
        await _dbContext.Set<AspUserLogin>().AddAsync(login, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var deleted = await _dbContext.Set<AspUserLogin>()
            .Where(l => l.LoginProvider == "Facebook" && l.ProviderKey == "fb-key-99999")
            .ExecuteDeleteAsync(cancellationToken);

        Assert.AreEqual(1, deleted);
        Console.WriteLine($"Deleted {deleted} user-login(s)");
    }

    #endregion

    #region UserToken

    [TestMethod(DisplayName = "Identity_Write_UserToken")]
    public async Task Write_UserToken()
    {
        var cancellationToken = default(CancellationToken);

        var user = new AspUser
        {
            UserName = "tokenuser",
            NormalizedUserName = "TOKENUSER",
            Email = "tokenuser@example.com",
            NormalizedEmail = "TOKENUSER@EXAMPLE.COM",
        };
        await _dbContext.Set<AspUser>().AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var token = new AspUserToken
        {
            UserId = user.Id,
            LoginProvider = "MyApp",
            Name = "RefreshToken",
            Value = "abc123xyz",
        };
        await _dbContext.Set<AspUserToken>().AddAsync(token, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        Console.WriteLine($"UserToken UserId={token.UserId}, LoginProvider={token.LoginProvider}, Name={token.Name}");
    }

    [TestMethod(DisplayName = "Identity_Update_UserToken")]
    public async Task Update_UserToken()
    {
        var cancellationToken = default(CancellationToken);

        var user = new AspUser
        {
            UserName = "updtokenuser",
            NormalizedUserName = "UPDTOKENUSER",
            Email = "updtokenuser@example.com",
            NormalizedEmail = "UPDTOKENUSER@EXAMPLE.COM",
        };
        await _dbContext.Set<AspUser>().AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var token = new AspUserToken
        {
            UserId = user.Id,
            LoginProvider = "MyApp",
            Name = "RefreshToken",
            Value = "old-token-value",
        };
        await _dbContext.Set<AspUserToken>().AddAsync(token, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        token.Value = "new-token-value";
        await _dbContext.SaveChangesAsync(cancellationToken);

        var found = await _dbContext.Set<AspUserToken>()
            .Where(t => t.UserId == user.Id && t.LoginProvider == "MyApp" && t.Name == "RefreshToken")
            .FirstOrDefaultAsync(cancellationToken);

        Assert.IsNotNull(found);
        Assert.AreEqual("new-token-value", found.Value);
        Console.WriteLine($"Updated UserToken Value={found.Value}");
    }

    [TestMethod(DisplayName = "Identity_Delete_UserToken")]
    public async Task Delete_UserToken()
    {
        var cancellationToken = default(CancellationToken);

        var user = new AspUser
        {
            UserName = "deltokenuser",
            NormalizedUserName = "DELTOKENUSER",
            Email = "deltokenuser@example.com",
            NormalizedEmail = "DELTOKENUSER@EXAMPLE.COM",
        };
        await _dbContext.Set<AspUser>().AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var token = new AspUserToken
        {
            UserId = user.Id,
            LoginProvider = "MyApp",
            Name = "TempToken",
            Value = "temp-value",
        };
        await _dbContext.Set<AspUserToken>().AddAsync(token, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var deleted = await _dbContext.Set<AspUserToken>()
            .Where(t => t.UserId == user.Id && t.LoginProvider == "MyApp" && t.Name == "TempToken")
            .ExecuteDeleteAsync(cancellationToken);

        Assert.AreEqual(1, deleted);
        Console.WriteLine($"Deleted {deleted} user-token(s)");
    }

    #endregion
}
