using System.Collections.Generic;
using System.Linq;
using OrcGame.OgEntity.OgCreature;
using OrcGame.OgEntity.OgItem;

namespace OrcGame.JobSystem;

public class GetBone : Job
{
    private Item _bone = null;
    public bool JobIsDone()
    {
        return HasBone(Worker);
    }

    public override void FindWorker()
    {
        var man = CreatureManager.GetCreatureManager();
        Worker = man.IdleCreatures.First();
    }

    public override void DoNext()
    {
        if (JobIsDone())
        {
            _jobManager.CancelJob(this);
            return;
        }
        // find bone
        if (_bone == null)
        {
            var props = new Dictionary<string, object>
            {
                ["Material"] = MaterialType.Bone
            };
            var item = _itemManager.FindNearestItemWithProps(props);
            if (item != null)
            {
                _bone = item;
            }

            return;
        }
        
        // tag bone
        if (!_bone.IsTagged)
        {
            Worker.AddToTagged(_bone);
            // this return may not be necessary; however, threading/update cycle may cause conflicts if next step happens immediately
            return;
        }
        
        // move to bone
        if (Worker.Location != _bone.Location)
        {
            Worker.WalkTo(_bone.Location);
            return;
        }

        // pick up bone
        if (Worker.Location == _bone.Location)
        {
            Worker.AddToCarried(_bone);
            return;
        }
    }

    public bool HasBone(Creature creature)
    {
        return creature.Carried.Any(x => x.Material == MaterialType.Bone);
    }
    
}