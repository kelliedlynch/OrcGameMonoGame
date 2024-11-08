using OrcGame.OgEntity.OgCreature;
using OrcGame.OgEntity.OgItem;

namespace OrcGame.JobSystem;

public class Job
{
    protected readonly JobManager _jobManager = JobManager.GetJobManager();
    protected readonly ItemManager _itemManager = ItemManager.GetItemManager();
    
    public string Name = "Generic Job";
    public Creature Worker;

    public virtual void DoNext(){}

    public virtual void FindWorker(){}
}