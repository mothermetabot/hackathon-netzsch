using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace ChatLab.ViewModels
{
    using R = ChatLab.Win.Properties.Resources;

    public enum ItemType
    {
        Response = 0,
        Message,
    }

    public class ChatMessageModel : ReactiveObject
    {
        [Reactive] public int Id { get; set; }
        [Reactive] public string Message { get; set; }
        
    }

    /// <summary>
    /// For visual representation of
    /// the chat.
    /// </summary>
    public class ChatItem : ReactiveObject
    {
        [Reactive] public int Id { get; set; }
        [Reactive] public ItemType ItemType { get; set; }
        [Reactive] public string Text { get; set; }
    }

    public class ChatResponseModel : ReactiveObject
    {
        [Reactive] public string MessageId { get; set; }
        [Reactive] public object Response { get; set; }
    }

    public class MainViewModel : ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        #region Commands
        public ReactiveCommand<Unit, Unit> StartCommand { get; }
        public ReactiveCommand<Unit, Unit> StopCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenChatCommand { get; }
        public ReactiveCommand<Unit, Unit> CloseChatCommand { get; }

        public ReactiveCommand<Unit, Unit> SendMessageCommand { get; }

        #endregion


        #region ChatRelated
        [Reactive] public bool IsChatVisible { get; set; }

        [Reactive] public ObservableCollection<ChatMessageModel> ChatMessages { get; set; }
        [Reactive] public ObservableCollection<ChatResponseModel> ChatResponses { get; set; }

        [Reactive] public ObservableCollection<ChatItem> ChatItems { get; set; }

        #endregion


        [Reactive] public ChatMessageModel CurrentMessage { get; set; }

        [Reactive] public bool IsActivated { get; private set; }

        /// TODO: maybe later we implement language switching
        [Reactive] public CultureInfo CurrentCulture { get; private set; } = CultureInfo.CurrentCulture;

        public MainViewModel()
        {
            StartCommand = ReactiveCommand.Create(StartMeasurement);
            StopCommand = ReactiveCommand.Create(StopMeasurement);
            OpenChatCommand = ReactiveCommand.Create(OpenChat);
            CloseChatCommand = ReactiveCommand.Create(CloseChat);
            SendMessageCommand = ReactiveCommand.Create(SendMessage);

            this.WhenActivated((CompositeDisposable disposables) =>
            {

            });
        }

        private void SendMessage()
        {
            Debug.WriteLine("Sending Message");
        }

        #region 

        #endregion

        private void OpenChat()
        {
            Debug.WriteLine("Opening Chat");

            IsChatVisible = true;
            // establish connection here?
        }

        private void CloseChat()
        {
            Debug.WriteLine("Closing Chat");

            IsChatVisible = false;
            // close connection here?
        }

        private void StopMeasurement()
        {
            Debug.WriteLine("Starting Measurement");

        }

        private void StartMeasurement()
        {
            Debug.WriteLine("Stopping Measurement");

        }

    }
}
