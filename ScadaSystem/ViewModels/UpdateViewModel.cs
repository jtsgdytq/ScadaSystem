using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

namespace ScadaSystem.ViewModels
{
    public class UpdateViewModel<T> : ObservableObject where T : class
    {
        private T _entity;

        public T Entity
        {
            get => _entity;
            set => SetProperty(ref _entity, value);
        }

        public UpdateViewModel(T entity)
        {
            _entity = entity;
        }
    }
}
