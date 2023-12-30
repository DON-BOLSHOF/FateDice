using Cysharp.Threading.Tasks;

namespace BKA.Units
{
    public class Militia : Unit
    {
        protected override UnitDefinition _definition { get; set; }
        
        public override void Execute()
        {
            
        }

        public Militia()
        {
            LoadBaseData().Forget();
        }

        private async UniTask LoadBaseData()
        {
            _definition = await _definitionProvider.Load("MilitiaDefinition");
        }
    }
}