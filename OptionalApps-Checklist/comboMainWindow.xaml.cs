using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace MultiSelectComboBoxDemo
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<ComboBoxItemViewModel> _items;
        public ObservableCollection<ComboBoxItemViewModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            // Sample data
            Items = new ObservableCollection<ComboBoxItemViewModel>
            {
                new ComboBoxItemViewModel("Item 1"),
                new ComboBoxItemViewModel("Item 2"),
                new ComboBoxItemViewModel("Item 3"),
                new ComboBoxItemViewModel("Item 4"),
                new ComboBoxItemViewModel("Item 5")
            };
        }

        private void MultiSelectComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Handle selection change here if needed
        }
    }

    public class ComboBoxItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        public string Item { get; set; }

        public ComboBoxItemViewModel(string item)
        {
            Item = item;
        }
    }
}
