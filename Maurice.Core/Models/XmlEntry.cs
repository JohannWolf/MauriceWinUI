using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maurice.Core.Models
{
    public partial class XmlEntry : ObservableObject
    {
        [ObservableProperty]
        private string _key;

        [ObservableProperty]
        private string _value;
    }
}
