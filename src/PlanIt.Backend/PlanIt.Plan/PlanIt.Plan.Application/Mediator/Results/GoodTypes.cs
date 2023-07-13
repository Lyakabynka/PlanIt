using System.Runtime.InteropServices;

namespace PlanIt.Plan.Application.Mediator.Results;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Success { }

public struct Success<T>
{
    public Success(T value) => this.Value = value;

    public T Value { get; }
}
