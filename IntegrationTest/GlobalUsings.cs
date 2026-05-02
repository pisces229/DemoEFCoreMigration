global using Microsoft.EntityFrameworkCore;
global using Model;
global using Model.Definitions;
global using Model.Entities;
global using Model.Extensions;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.ClassLevel)]
