using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Livet.Messaging;
using Livet.Messaging.IO;
using Prism.Interactivity.InteractionRequest;

namespace LivetCaskMessagingSample.ViewModels
{
    public class ContentViewModel : IConfirmNavigationRequest, IRegionMemberLifetime
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067
        public bool KeepAlive => false;

        private IRegionNavigationService RegionNavigationService { get; set; }

        private CompositeDisposable DisposeCollection = new CompositeDisposable();
        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed")]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeCollection.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose() => Dispose(true);
        #endregion

        public ReactiveProperty<string> OutputMessage { get; } = new ReactiveProperty<string>();
        public InteractionMessenger Messenger { get; } = new InteractionMessenger();
        public ReactiveCommand ViewModelToViewMessagerCommand { get; } = new ReactiveCommand();

        public ReactiveProperty<bool> ButtonMouseOver { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> Password { get; } = new ReactiveProperty<string>();

        public ContentViewModel()
        {
            ViewModelToViewMessagerCommand.Subscribe(async _ => await ViewModelToViewMessagerAsync()).AddTo(DisposeCollection);
            ButtonMouseOver.Subscribe(b => OutputMessage.Value = $"{DateTime.Now}: ButtonMouseOver: {b}").AddTo(DisposeCollection);
            Password.Subscribe(p => OutputMessage.Value = $"{DateTime.Now}: Password: {p}").AddTo(DisposeCollection);
        }

        private async Task ViewModelToViewMessagerAsync()
        {
            var message = new ConfirmationMessage("ViewModel Raise", "title", "ViewModelToViewMessager")
            {
                Button = MessageBoxButton.YesNo,
            };

            await Messenger.RaiseAsync(message);

            OutputMessage.Value = $"{DateTime.Now}: ViewModelToViewMessagerAsync: {message.Response ?? false}";
        }

        public void ViewMessager(ConfirmationMessage message)
        {
            OutputMessage.Value = $"{DateTime.Now}: ViewMessager: {message.Response ?? false}";
        }

        public void FolderSelected(FolderSelectionMessage message)
        {
            OutputMessage.Value = $"{DateTime.Now}: FolderSelected: {message.Response}";
        }

        public void FileSelected(OpeningFileSelectionMessage message)
        {
            OutputMessage.Value = $"{DateTime.Now}: FileSelected: {message.Response[0]}";
        }

        public void InformationMessager(InformationMessage message)
        {
            OutputMessage.Value = $"{DateTime.Now}: InformationMessager";
        }

        public void SaveFile(SavingFileSelectionMessage message)
        {
            OutputMessage.Value = $"{DateTime.Now}: SaveFile: {message.Response[0]}";
        }

        public void SubWindowOpened(TransitionMessage message)
        {
            OutputMessage.Value = $"{DateTime.Now}: SaveFile: {message.Response}";
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback) => continuationCallback(true);

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}