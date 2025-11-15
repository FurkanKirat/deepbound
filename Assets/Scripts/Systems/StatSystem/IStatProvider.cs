using System.Collections.Generic;

namespace Systems.StatSystem
{
    public interface IStatProvider
    {
        IEnumerable<StatModifier> GetStatModifiers();
    }
}