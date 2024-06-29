using _Stat_System.Runtime.Common;
using UnityEngine;
using Zenject;

namespace _Stat_System.Runtime.Base
{
    public interface IDamageable
    {
        public void TakeDamage(int amount);
        public int CurrentValue { get; }
        public int BaseValue { get; }
        public string ItemName { get; }
    }

    public interface IBreakable
    {
        public void Break();
    }

    public enum StatType
    {
        Health,
        Stamina,
        Thirst,
        Hunger,
        Radiation,
        Experience
    }

    public enum StatGroup
    {
        Player,
        Enemy
    }

    public interface IStat
    {
        public int BaseValue { get; }
        public int Value { get; }
        public int ChangeValue { get; }
        public abstract StatType Type { get; }
        public StatGroup Group { get; }
    }

    public abstract class Stat : IStat
    {
        public StatGroup Group { get; private set; }
        public abstract StatType Type { get; }
        public int BaseValue { get; protected set; }
        public int Value { get; protected set; }
        public int ChangeValue { get; protected set; }

        protected StatObserverManager _subject;

        public virtual int Modify
        {
            get => Value;
            set
            {
                int previousValue = Value;
                int newValue = Mathf.Clamp(value, 0, BaseValue);

                if (newValue != Value)
                {
                    Value = newValue;
                    ChangeValue = newValue - previousValue;

                    _subject.Notify(this);
                }
            }
        }

        public Stat(StatObserverManager subject, StatGroup statGroup)
        {
            _subject = subject;
            Group = statGroup;
        }

        public void ModifyBaseValue(int amount)
        {
            BaseValue += amount;
            Value = BaseValue;
        }
    }
}