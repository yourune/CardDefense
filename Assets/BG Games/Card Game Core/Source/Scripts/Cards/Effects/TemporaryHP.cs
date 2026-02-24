namespace BG_Games.Card_Game_Core.Cards.Effects
{
    public class TemporaryHP:TemporaryEffect
    {
        private int _amount;
        private int _hpBeforeChange;
        private int _temporaryHP;


        public TemporaryHP(int amount)
        {
            _amount = amount;
        }

        protected override void ApplyEffect()
        {
            Target.GainHP(_amount);

            _temporaryHP = _amount;
            _hpBeforeChange = Target.HP;

            Target.OnHPChanged += CheckAdditionalHPSpent;
        }

        private void CheckAdditionalHPSpent(int hp)
        {
            int delta = _hpBeforeChange - hp;

            if (delta > 0)
            {
                _temporaryHP -= delta;
            }

            _hpBeforeChange = hp;
        }

        public override void Remove()
        {
            if (_temporaryHP > 0)
            {
                Target.RemoveHP(_amount);
            }

            base.Remove();
        }
    }
}
