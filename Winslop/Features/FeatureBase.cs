using System.Threading.Tasks;

namespace Winslop
{
    public abstract class FeatureBase
    {
        public abstract string ID();
        public abstract string GetFeatureDetails();
        public abstract Task<bool> CheckFeature();  // async
        public abstract Task<bool> DoFeature();    // async
        public abstract bool UndoFeature();

        public virtual string HelpAnchorId()
        {
            return ID();
        }
    }

}
