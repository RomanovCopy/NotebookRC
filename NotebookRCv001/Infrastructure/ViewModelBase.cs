using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NotebookRCv001.Infrastructure
{
    [Serializable]
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            bool propertyChanged = false;
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(name);
                propertyChanged = true;
            }
            return propertyChanged;
        }

        protected virtual bool SetProperty<T>(ref T field, T value, string[] names)
        {
            bool propertyChanged = false;
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(names);
                propertyChanged = true;
            }
            return propertyChanged;
        }

        [field: NonSerialized]
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }

        public virtual void OnPropertyChanged(string[] names)
        {
            foreach (string name in names)
                OnPropertyChanged(name);
        }
    }
}
