using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IO.Hashing;
using System.Text;
using System.Text.Json;

namespace Model;

/// <summary>
/// SqlServer, PostgreSQL, Sqlite
/// </summary>
public class DbContextUtil
{
    /// <summary>
    /// Max Name Length
    /// Max Name: 63
    /// </summary>
    public const int MaxNameLength = 63;
    /// <summary>
    /// Max TableName Length
    /// Max (c/p/i/f/d) Name: 63 (PostgreSQL), Max TableName Length: 55 (63 - 2 - 6)
    /// </summary>
    public const int MaxTableNameLength = MaxNameLength - 2 - HashNameLength;
    /// <summary>
    /// Hash Name Length
    /// </summary>
    public const int HashNameLength = 6;

    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };

    public static string NamingConvention(string name) =>
        string.Concat(name.Select((c, i) => i > 0 && char.IsUpper(c) ? "_" + c : c.ToString())).ToLower();

    public static void HashKeyName(IMutableKey mutableKey)
    {
        if (string.IsNullOrWhiteSpace(mutableKey.GetName())) return;

        var depTable = mutableKey.DeclaringEntityType.GetTableName();
        if (mutableKey.IsPrimaryKey())
        {
            mutableKey.SetName(CreatePrimaryKeyName(depTable!));
        }
        else
        {
            var colNames = string.Join("|", mutableKey.Properties.Select(p => p.Name));
            var hashName = ToHashName(colNames);
            mutableKey.SetName(CreateAlternateKeyName(depTable!, hashName));
        }
    }

    public static void HashDatabaseName(IMutableIndex mutableIndex)
    {
        if (string.IsNullOrWhiteSpace(mutableIndex.GetDatabaseName())) return;

        var depTable = mutableIndex.DeclaringEntityType.GetTableName();
        var colNames = string.Join("|", mutableIndex.Properties.Select(p => p.Name));
        var hashName = ToHashName(colNames);
        mutableIndex.SetDatabaseName(CreateDatabaseName(depTable!, hashName));
    }

    public static void HashConstraintName(IMutableForeignKey mutableForeignKey)
    {
        if (string.IsNullOrWhiteSpace(mutableForeignKey.GetConstraintName())) return;
        if (mutableForeignKey.GetConstraintName()!.StartsWith("d_")) return;

        var depTable = mutableForeignKey.DeclaringEntityType.GetTableName();
        var colNames = string.Join("|", mutableForeignKey.Properties.Select(p => p.Name));
        var hashName = ToHashName(colNames);
        mutableForeignKey.SetConstraintName(CreateConstraintName(depTable!, hashName));
    }

    public static string ToHashName(string name)
    {
        var inputBytes = Encoding.UTF8.GetBytes(name);
        var hashBytes = XxHash64.Hash(inputBytes);
        Array.Reverse(hashBytes);
        return Convert.ToHexString(hashBytes).ToLowerInvariant()[..HashNameLength];
    }

    public static string CreateCheckConstraint(string table, string name)
    {
        var result = $"c_{NamingConvention(table)}_{ToHashName(name)}";
        ValidateNameMaxLength(result);
        return result;
    }

    public static string CreatePrimaryKeyName(string table)
    {
        var result = $"p_{NamingConvention(table)}";
        ValidateNameMaxLength(result);
        return result;
    }

    public static string CreateAlternateKeyName(string table, string name)
    {
        var result = $"a_{NamingConvention(table)}_{NamingConvention(name)}";
        return result;
    }

    public static string CreateDatabaseName(string table, string name)
    {
        var result = $"i_{NamingConvention(table)}_{NamingConvention(name)}";
        ValidateNameMaxLength(result);
        return result;
    }

    public static string CreateConstraintName(string table, string name)
    {
        var result = $"f_{NamingConvention(table)}_{NamingConvention(name)}";
        ValidateNameMaxLength(result);
        return result;
    }

    public static string DropConstraintName(string table, string name)
    {
        var result = $"d_{NamingConvention(table)}_{ToHashName(NamingConvention(name))}";
        ValidateNameMaxLength(result);
        return result;
    }

    private static void ValidateNameMaxLength(string name)
    {
        if (name.Length > MaxNameLength)
        {
            throw new InvalidOperationException($"Name '{name}' exceeds maximum length of {MaxNameLength} characters.");
        }
    }
}
