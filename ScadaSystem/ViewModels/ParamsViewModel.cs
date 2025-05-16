using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Options;
using ScadaSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem.ViewModels
{
    public partial class ParamsViewModel: ObservableObject
    {
        [ObservableProperty]
        RootParam _rootParamProp;
        public ParamsViewModel(IOptionsSnapshot<RootParam> optionsSnapshot)
        {
            RootParamProp = optionsSnapshot.Value;
        }
    }
}
