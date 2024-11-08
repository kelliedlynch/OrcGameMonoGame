using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Collections;

namespace OrcGame.JobSystem;

public sealed class JobManager
{
    private static readonly Lazy<JobManager> Instance = new (() => new JobManager());
    public static JobManager GetJobManager() { return Instance.Value; }
    
    
    public HashSet<Job> AssignedJobs { get; } = new();
    public HashSet<Job> UnassignedJobs { get; } = new();

    public void ManageJob(Job job)
    {
        UnassignedJobs.Add(job);
    }

    private void AssignJob(Job job)
    {
        job.FindWorker();
        if (job.Worker != null)
        {
            AssignedJobs.Add(job);
            return;
        }
        UnassignedJobs.Add(job);
    }

    private void AssignUnassignedJobs()
    {
        foreach (var job in UnassignedJobs)
        {
            AssignJob(job);
        }
    }

    private void UnassignJob(Job job)
    {
        job.Worker = null;
        AssignedJobs.Remove(job);
        UnassignedJobs.Add(job);
    }

    public void CancelJob(Job job)
    {
        AssignedJobs.Remove(job);
        UnassignedJobs.Remove(job);
    }

    public void OnUpdate()
    {
        AssignUnassignedJobs();
        foreach (var job in AssignedJobs)
        {
            job.DoNext();
        }

    }
}