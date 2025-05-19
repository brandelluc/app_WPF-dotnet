using System.Windows;
using BibliothequeWPF.ViewModels;

namespace BibliothequeWPF
{
    public partial class EmpruntsWindow : Window
    {
        private readonly EmpruntsViewModel _viewModel;

        public EmpruntsWindow()
        {
            InitializeComponent();
            _viewModel = new EmpruntsViewModel();
            DataContext = _viewModel;
        }
    }
} 