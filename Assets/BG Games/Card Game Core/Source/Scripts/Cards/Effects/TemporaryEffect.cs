using System;

namespace BG_Games.Card_Game_Core.Cards.Effects
{
    public abstract class TemporaryEffect:ITemporaryEffect
    {
        protected bool Applied;
        protected ITroopLogic Target;

        protected IEffectSource Source;

        protected int Duration;
        protected DurationMode DurationMode;

        public void Apply(ITroopLogic target, int duration, DurationMode durationMode)
        {
            if (Applied)
                throw new Exception($"Effect allready applied to target: {target.ToString()}");

            target.AddTemporaryEffect(this);

            Target = target;
            Duration = duration;
            DurationMode = durationMode;

            ApplyEffect();
            Applied = true;
        }

        public void Apply(ITroopLogic target, IEffectSource source)
        {
            if (Applied)
                throw new Exception($"Effect allready applied to target: {target.ToString()}");

            DurationMode = DurationMode.InvokeRemove;

            target.AddTemporaryEffect(this);
            source.OnRemoveEffect += Remove;

            Target = target;
            Source = source;

            ApplyEffect();
            Applied = true;
        }

        protected abstract void ApplyEffect();

        public virtual void Remove()
        {
            if (Target != null)
            {
                Target.RemoveTemporaryEffect(this);
            }

            if (Source != null)
            {
                Source.OnRemoveEffect -= Remove;
            }
        }

        public virtual void NextTurn()
        {
            if (DurationMode == DurationMode.TurnStart)
            {
                Duration--;
                if (Duration <= 0)
                {
                    Remove();
                }
            }
        }

        public virtual void EndTurn()
        {
            if (DurationMode == DurationMode.TurnEnd)
            {
                Duration--;
                if (Duration <= 0)
                {
                    Remove();
                }
            }
        }
    }
}
