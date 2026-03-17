global using Model.Extensions;

[assembly: Parallelize(Workers = 1, Scope = ExecutionScope.MethodLevel)]
