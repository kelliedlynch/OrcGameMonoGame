using System.Collections;
using OrcGame.JobSystem;
using OrcGame.OgEntity.OgCreature;

namespace Tests;

public class TestJobAssignment
{
    private Creature _creature;
    private JobManager _jobManager;
    private CreatureManager _creatureManager;
    
    [SetUp]
    public void Setup()
    {
        _jobManager = JobManager.GetJobManager();
        _creatureManager = CreatureManager.GetCreatureManager();
        _creature = new Creature();
        _creatureManager.AddCreatureToWorld(_creature);
    }

    [Test]
    public void Create_New_Job()
    {
        var job = new GetBone();
        _jobManager.ManageJob(job);
        Assert.True(_jobManager.UnassignedJobs.Contains(job));
    }

    [Test]
    public void Assign_And_Perform_Job()
    {
        var job = new GetBone();
        _jobManager.ManageJob(job);
        _jobManager.OnUpdate();
        Assert.True(_jobManager.AssignedJobs.Contains(job));
    }
}