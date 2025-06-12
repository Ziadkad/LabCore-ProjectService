namespace ProjectService.Domain.Common;

public class BaseModel
{
    public DateTime CreatedOn { get; private set; }
    public Guid CreatedBy { get; private set; }

    public DateTime? ModifiedOn { get; private set; }
    
    public Guid? ModifiedBy { get; private set; }
    
    public bool IsArchived { get; private set; }
    
    public void UpdateAudit(Guid modifiedBy)
    {
        ModifiedOn = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
    }
    
    public void CreateAudit(Guid createdBy)
    {
        CreatedOn = DateTime.UtcNow;
        CreatedBy = createdBy;
    }
    
    public void SetArchived()
    {
        IsArchived = true;
    }
}