using Game.Code.Common;

namespace Game.Code.Logic.Sitizens
{
    public class SitizenSpawnDefaultConfig
    {
        private SitizenType _type;
        private readonly int _defaultQuantity;

        public SitizenType Type => _type;
        public int DefaultQuantity => _defaultQuantity;

        public SitizenSpawnDefaultConfig(SitizenType type, int defaultQuantity)
        {
            _type = type;
            _defaultQuantity = defaultQuantity;
        }
    }
}