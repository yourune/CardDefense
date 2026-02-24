using System.Collections.Generic;
using BG_Games.Card_Game_Core.Cards.Descriptors;

namespace BG_Games.Card_Game_Core.Cards
{
    public interface ILogicWithDescriptors
    {
        public List<IDescriptor> Descriptors { get; }

        public void RemoveDescriptor<T>() where T : IDescriptor
        {
            IDescriptor descriptor = Descriptors.Find(descriptor => descriptor is T);
            Descriptors.Remove(descriptor);
        }

        public bool ContainsDescriptorFromList(List<IDescriptor> descriptors)
        {
            foreach (var descriptor in descriptors)
            {
                if (Descriptors.Contains(descriptor))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
