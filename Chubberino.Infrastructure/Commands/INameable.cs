using Chubberino.Common.ValueObjects;

namespace Chubberino.Infrastructure.Commands;

/// <summary>
/// Has a <see cref="Name"/>.
/// </summary>
public interface INameable
{
    /// <summary>
    /// Identifying name.
    /// </summary>
    Name Name { get; }
}
