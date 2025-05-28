namespace Modulio.Domain.Base
{
    public interface IAuditableEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the user who created the entity.
        /// </summary>
        int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was created.
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who last modified the entity.
        /// </summary>
        int? LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was last modified.
        /// </summary>
        DateTime? LastModifiedAt { get; set; }
    }
}
