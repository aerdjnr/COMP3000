using CommunityToolkit.Mvvm.ComponentModel;

namespace Velocity.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        public virtual double? SetWidth => null;
        public virtual double? SetHeight => null;
       // public virtual double? MinimumWidth => null;
       // public virtual double? MinimumHeight => null;
    }
}
