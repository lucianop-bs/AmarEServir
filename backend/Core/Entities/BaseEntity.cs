namespace AmarEServir.Core.Entities
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; protected set; }
        public bool IsActive { get; protected set; }

        protected BaseEntity()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            IsActive = true;
        }

        public virtual void Activate()
        {
            IsActive = true;
        }

        public virtual void Desactivate()
        {
            IsActive = false;
        }

        public virtual void SetUpdatedAtDate(DateTime updatedAtDate)
        {
            UpdatedAt = updatedAtDate;
        }
    }

    public abstract class BaseEntity<TId> : BaseEntity
    {
        public TId? Id { get; protected set; }
    }
}
