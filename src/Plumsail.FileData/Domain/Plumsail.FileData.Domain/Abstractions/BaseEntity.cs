using System.ComponentModel.DataAnnotations;

namespace Plumsail.FileData.Domain.Abstractions;

/// <summary>
/// Represents a base entity with a unique identifier.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the entity.
    /// </summary>
    [Key]
    public Guid Id { get; set; }
}
