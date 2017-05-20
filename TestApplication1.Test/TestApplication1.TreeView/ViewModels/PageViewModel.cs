using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TestApplication1.Test.Annotations;

namespace TestApplication1.Test.ViewModels
{
    public class PageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DataItem> _items;

        public ObservableCollection<DataItem> Items
        {
            get { return _items; }
            set
            {
                if (Equals(value, _items)) return;
                _items = value;
                OnPropertyChanged();
            }
        }

        public PageViewModel()
        {
            var rndm = new Random();
            _items = new ObservableCollection<DataItem>();
            var defaultPlaceHolder = "Lorem ipsum";
            for (int i = 0; i < 22; i++)
            {
                var textLength = rndm.Next(15) + 1;
                var text = i.ToString()+"\n";
                for (int j = 0; j < textLength; j++)
                {
                    text += defaultPlaceHolder + "\n";
                }
                Items.Add(new DataItem() {Text = text});
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}