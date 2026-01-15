using System.ComponentModel;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace ManagementHouseFee.Views
{
    public partial class CustomTooltipView : UserControl, IChartTooltip, INotifyPropertyChanged
    {
        private TooltipData _data;

        public CustomTooltipView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public TooltipData Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        
        public TooltipSelectionMode? SelectionMode { get; set; }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}