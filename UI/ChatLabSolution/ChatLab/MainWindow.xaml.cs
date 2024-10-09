using ChatLab.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

namespace ChatLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainViewModel();
            DataContext = ViewModel;

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                this.BindCommand(ViewModel,
                    vm => vm.StartCommand,
                    v => v.StartButton).
                    DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.StopCommand,
                    v => v.StopButton).
                    DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.OpenChatCommand,
                    v => v.OpenChatButton).
                    DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.CloseChatCommand,
                    v => v.CloseChatButton).
                    DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.SendMessageCommand,
                    v => v.SendMessageButton).
                    DisposeWith(disposables);
            });
        }

        public MainViewModel ViewModel
        {
            get => (MainViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(MainViewModel), typeof(MainWindow), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MainViewModel)value;
        }
    }
}