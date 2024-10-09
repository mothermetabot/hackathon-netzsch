using OllamaConnector;
using OllamaConnector.Responses;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace ChatLab.ViewModels
{
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
        [Reactive] public string ItemType { get; set; }
        [Reactive] public string Text { get; set; }
    }

    public class ChatResponseModel : ReactiveObject
    {
        [Reactive] public string MessageId { get; set; }
        [Reactive] public object Response { get; set; }
    }

    public class MainViewModel : ReactiveObject, IActivatableViewModel
    {
        private const string MessageString = "Message";
        private const string ResponseString = "Response";

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
        [Reactive] public bool IsImageVisible { get; set; }

        [Reactive] public ObservableCollection<ChatMessageModel> ChatMessages { get; set; } = [];
        [Reactive] public ObservableCollection<ChatResponseModel> ChatResponses { get; set; } = [];

        [Reactive] public ObservableCollection<ChatItem> ChatItems { get; set; } = [];

        [Reactive] public double ChatWidth { get; set; }

        [Reactive] public ChatMessageModel CurrentChatMessage { get; set; } = new();

        #endregion

        [Reactive] public bool IsBusy { get; set; }

        [Reactive] public ChatMessageModel CurrentMessage { get; set; }

        [Reactive] public bool IsActivated { get; private set; }

        /// TODO: maybe later we implement language switching
        [Reactive] public CultureInfo CurrentCulture { get; private set; } = CultureInfo.CurrentCulture;

        public MainViewModel()
        {
            var canSend = this.WhenAnyValue(x => x.IsBusy)
                .Select(busy =>
                {
                    return !busy;
                })
                .DistinctUntilChanged();

            StartCommand = ReactiveCommand.Create(StartMeasurement);
            StopCommand = ReactiveCommand.Create(StopMeasurement);
            OpenChatCommand = ReactiveCommand.CreateFromTask(OpenChat);
            CloseChatCommand = ReactiveCommand.Create(CloseChat);
            SendMessageCommand = ReactiveCommand.CreateFromTask(SendMessage, canSend);

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                //StartCommand.Execute().Subscribe().DisposeWith(disposables);
                this.WhenAnyValue(x => x.IsChatVisible,
                    isVisible => isVisible ? 240 : 0)
                .BindTo(this, x => x.ChatWidth);
            });
        }

        private async Task SendMessage()
        {
            Debug.WriteLine("Sending Message");
            IsBusy = true;
            ChatItems.Add(
                        new ChatItem()
                        {
                            Id = ChatItems.Count,
                            ItemType = MessageString,
                            Text = CurrentChatMessage.Message,
                        });
            CurrentChatMessage.Message = string.Empty;
            var response = await Prompt.Send(CurrentChatMessage.Message);
            IsBusy = false;
            ParseResponse(response);
        }

        private void ParseResponse(IResponse response)
        {
            switch (response.Type)
            {
                case ResponseType.DEFINE when response is IDefineResponse defineResponse:

                    var stepStrings = defineResponse.Steps.Select(step => $@"- temperature: {step.TargetTemperature} °C
- duration: {step.DurationInSeconds} seconds
- heatingRate: {step.HeatingRate} K/min
- type: {step.Type}");

                    var defString = string.Join("\n", stepStrings);
                    ChatItems.Add(
                        new ChatItem()
                        {
                            Id = ChatItems.Count,
                            ItemType = ResponseString,
                            Text = defString
                        });
                    break;
                case ResponseType.START when response is IStartResponse startResponse:
                    StartCommand.Execute().Subscribe();
                    break;
                case ResponseType.STOP when response is IStopResponse stopResponse:
                    StopCommand.Execute().Subscribe();
                    break;
                case ResponseType.INFORMATION when response is IInfoResponse infoResponse:
                    ChatItems.Add(
                        new ChatItem()
                        {
                            Id = ChatItems.Count,
                            ItemType = ResponseString,
                            Text = infoResponse.Params
                        });
                    break;
                default:
                    ChatItems.Add(
                        new ChatItem()
                        {
                            Id = ChatItems.Count,
                            ItemType = ResponseString,
                            Text = "Please try again, something went wrong :("
                        });
                    break;
            }
        }

        #region 

        #endregion
        bool isInitialized;

        private async Task OpenChat()
        {
            Debug.WriteLine("Opening Chat");
            IsChatVisible = true;
            if (!isInitialized)
            {
                IsBusy = true;
                var firstMessage = await Prompt.Initialize();
                IsBusy = false;
                isInitialized = true;
                ChatItems.Add(new()
                {
                    Id = ChatItems.Count,
                    Text = firstMessage,
                    ItemType = ResponseString
                });
            }
        }

        private void CloseChat()
        {
            Debug.WriteLine("Closing Chat");

            IsChatVisible = false;
        }

        private void StopMeasurement()
        {
            Debug.WriteLine("Stopping Measurement");
            IsImageVisible = false;
        }

        private void StartMeasurement()
        {
            Debug.WriteLine("Starting Measurement");
            IsImageVisible = true;
        }

    }
}
