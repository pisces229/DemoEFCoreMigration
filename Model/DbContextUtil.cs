using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IO.Hashing;
using System.Text;
using System.Text.Json;

namespace Model;

/// <summary>
/// SqlServer, PostgreSQL, Sqlite
/// </summary>
internal class DbContextUtil
{
    /// <summary>
    /// Schema
    /// </summary>
    public const string Schema = "public";
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
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static string NamingConvention(string name) => ToSnakeCase(name);

    public static string ToSnakeCase(string name) =>
        string.Concat(name.Select((c, i) => i > 0 && char.IsUpper(c) ? "_" + c : c.ToString())).ToLower();

    public static void HashKeyName(IMutableKey mutableKey)
    {
        //Console.WriteLine($"IMutableKey.DeclaringEntityType.GetTableName(): {mutableKey.DeclaringEntityType.GetTableName()}");
        //Console.WriteLine($"IMutableKey.IsPrimaryKey(): {mutableKey.IsPrimaryKey()}");
        //Console.WriteLine($"IMutableKey.GetName(): {mutableKey.GetName()}");
        //Console.WriteLine($"IMutableKey.GetDefaultName(): {mutableKey.GetDefaultName()}");

        if (string.IsNullOrWhiteSpace(mutableKey.GetName())) return;

        var depTable = mutableKey.DeclaringEntityType.GetTableName();
        if (mutableKey.IsPrimaryKey())
        {
            mutableKey.SetName(CreatePrimaryKeyName(depTable!));
        }
        else
        {
            var colNames = string.Join("|", mutableKey.Properties.Select(p => p.Name));
            var hashName = ToHashName($"{colNames}");
            mutableKey.SetName(CreateAlternateKeyName(depTable!, hashName));
        }
    }

    //public static void HashCheckConstraintName(IMutableCheckConstraint mutableCheckConstraint)
    //{
    //    //Console.WriteLine($"IMutableCheckConstraint.Name: {mutableCheckConstraint.Name}");
    //    //Console.WriteLine($"IMutableCheckConstraint.Sql: {mutableCheckConstraint.Sql}");
    //    if (string.IsNullOrWhiteSpace(mutableCheckConstraint.Name)) return;
    //}

    public static void HashDatabaseName(IMutableIndex mutableIndex)
    {
        //Console.WriteLine($"IMutableIndex.DeclaringEntityType.GetTableName(): {mutableIndex.DeclaringEntityType.GetTableName()}");
        //Console.WriteLine($"IMutableIndex.GetDefaultDatabaseName(): {mutableIndex.GetDefaultDatabaseName()}");
        //Console.WriteLine($"IMutableIndex.GetDatabaseName(): {mutableIndex.GetDatabaseName()}");
        //Console.WriteLine($"IMutableIndex.Properties.Count: {mutableIndex.Properties.Count}");
        //mutableIndex.Properties.ToList().ForEach(p => Console.WriteLine($"IMutableIndex.Properties: {p.Name}"));

        if (string.IsNullOrWhiteSpace(mutableIndex.GetDatabaseName())) return;

        var depTable = mutableIndex.DeclaringEntityType.GetTableName();
        var colNames = string.Join("|", mutableIndex.Properties.Select(p => p.Name));
        var hashName = ToHashName($"{colNames}");
        mutableIndex.SetDatabaseName(CreateDatabaseName(depTable!, hashName));
    }

    public static void HashConstraintName(IMutableForeignKey mutableForeignKey)
    {
        //Console.WriteLine($"IMutableForeignKey.DeclaringEntityType.GetTableName: {mutableForeignKey.DeclaringEntityType.GetTableName()}");
        //Console.WriteLine($"IMutableForeignKey.PrincipalEntityType.GetTableName: {mutableForeignKey.PrincipalEntityType.GetTableName()}");
        //Console.WriteLine($"IMutableForeignKey.GetDefaultName(): {mutableForeignKey.GetDefaultName()}");
        //Console.WriteLine($"IMutableForeignKey.GetConstraintName(): {mutableForeignKey.GetConstraintName()}");
        //Console.WriteLine($"IMutableForeignKey.Properties.Count: {mutableForeignKey.Properties.Count}");
        //mutableForeignKey.Properties.ToList().ForEach(p => Console.WriteLine($"IMutableForeignKey.Properties: {p.Name}"));

        if (string.IsNullOrWhiteSpace(mutableForeignKey.GetConstraintName())) return;
        if (mutableForeignKey.GetConstraintName()!.StartsWith($"d_")) return;

        var depTable = mutableForeignKey.DeclaringEntityType.GetTableName();
        //var priTable = mutableForeignKey.PrincipalEntityType.GetTableName();
        var colNames = string.Join("|", mutableForeignKey.Properties.Select(p => p.Name));
        var hashName = ToHashName($"{colNames}");
        mutableForeignKey.SetConstraintName(CreateConstraintName(depTable!, hashName));
    }

    private static string ToHashName(string name)
    {
        var inputBytes = Encoding.UTF8.GetBytes(name);
        var hashBytes = XxHash64.Hash(inputBytes);
        Array.Reverse(hashBytes);
        return Convert.ToHexString(hashBytes).ToLowerInvariant()[..HashNameLength];
    }

    public static string CreateCheckConstraint(string table, string name)
    {
        var result = $"c_{ToSnakeCase(table)}_{ToHashName(name)}";
        ValidateNameMaxLength(result);
        return result;
    }

    public static string CreatePrimaryKeyName(string table)
    {
        var result = $"p_{ToSnakeCase(table)}";
        ValidateNameMaxLength(result);
        return result;
    }

    public static string CreateAlternateKeyName(string table, string name)
    {
        var result = $"a_{ToSnakeCase(table)}_{ToSnakeCase(name)}";
        return result;
    }

    public static string CreateDatabaseName(string table, string name)
    {
        var result = $"i_{ToSnakeCase(table)}_{ToSnakeCase(name)}";
        ValidateNameMaxLength(result);
        return result;
    }

    public static string CreateConstraintName(string table, string name)
    {
        var result = $"f_{ToSnakeCase(table)}_{ToSnakeCase(name)}";
        ValidateNameMaxLength(result);
        return result;
    }

    public static string DropConstraintName(string table, string name)
    {
        var result = $"d_{ToSnakeCase(table)}_{ToSnakeCase(name)}";
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
