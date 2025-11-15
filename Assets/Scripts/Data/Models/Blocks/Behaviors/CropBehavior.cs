using Core;
using Core.Context;
using Core.Context.Registry;
using Core.Events;
using Generated.Ids;
using Systems.SaveSystem.SaveData.BlockBehavior;
using Utils;

namespace Data.Models.Blocks.Behaviors
{
    public class CropBehavior : BaseBlockBehavior
    {
        public override string BehaviorId => BlockBehaviorIds.Crop;
        
        private readonly float _totalTime;
        private float _remainingTime;
        
        public GrowthStage GrowthStage;
        private bool _isGrowing;
        
        public CropBehavior(BlockBehaviorContext ctx) : base(ctx.Position)
        {
            _totalTime = ctx.BlockData.Crop.GrowthTime;
            _remainingTime = _totalTime;
            _isGrowing = true;
            GrowthStage = GrowthStage.Seed;
            RecalculateGrowthStage();
        }

        public CropBehavior(BlockBehaviorContext ctx, CropBehaviorSaveData saveData) : base(ctx.Position)
        {
            _totalTime = saveData.TotalTime;
            _remainingTime = saveData.RemainingTime;
            _isGrowing = _totalTime > _remainingTime;
            RecalculateGrowthStage();
        }
        
        public override void Tick(float timeInterval, TickContext ctx)
        {
            if (!_isGrowing)
                return;
            
            _remainingTime -= timeInterval;
            if (_remainingTime <= 0)
            {
                _remainingTime = 0;
                _isGrowing = false;
            }
            RecalculateGrowthStage();
        }
        private void RecalculateGrowthStage()
        { 
            var newGrowthStage = MathUtils.ProgressToEnum<GrowthStage>(_totalTime, _totalTime - _remainingTime);

            if (newGrowthStage == GrowthStage) return;
            
            var oldStage = GrowthStage;
            GrowthStage = newGrowthStage;
            
            GameEventBus.Publish(new CropStateChangedEvent(Position, oldStage, GrowthStage));
        }

        public override BlockBehaviorSaveData ToSaveData()
        {
            return new CropBehaviorSaveData
            {
                BehaviorId = BehaviorId,
                TotalTime = _totalTime,
                RemainingTime = _remainingTime,
            };
        }

        
    }
    
    public enum GrowthStage : byte
    {
        Seed,
        Growing,
        Mature,
    }

}