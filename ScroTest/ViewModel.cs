using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ScroTest
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ViewModel()
        {
        }

        private double pointY;

        public double PointY
        {
            get
            {
                return pointY;
            }

            set
            {
                pointY = value;
                OnPropertyChanged();
            }
        }
    }
}