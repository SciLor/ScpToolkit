using ScpControl.Shared.Core;

namespace ScpProfiler.ViewModels.MappingTargets
{
    internal class TargetGamepadButtonViewModel : MappingTargetViewModelBase
    {
        private IMappingTarget mappingTarget;

        public TargetGamepadButtonViewModel(IMappingTarget mappingTarget)
        {
            this.mappingTarget = mappingTarget;
        }
    }
}