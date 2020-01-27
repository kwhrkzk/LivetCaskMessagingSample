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

namespace LivetCaskMessagingSample
{
    public class MainWindowViewModel
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

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

        public InteractionMessenger Messenger { get; } = new InteractionMessenger();

        public AsyncReactiveCommand CloseCanceledCallbackCommand { get; } = new AsyncReactiveCommand();
        public ReactiveProperty<bool> CanClose { get; } = new ReactiveProperty<bool>();

        public MainWindowViewModel()
        {
            CloseCanceledCallbackCommand.Subscribe(async () => await CloseCanceledCallbackMethodNameAsync()).AddTo(DisposeCollection);
        }

        public async Task CloseCanceledCallbackMethodNameAsync()
        {
            var message = new ConfirmationMessage("Close Canceled. CanClose toggle?", "title", "CloseCanceledCallbackMethodName")
            {
                Button = MessageBoxButton.YesNo,
            };

            await Messenger.RaiseAsync(message);

            CanClose.Value = message.Response ?? false;
        }
    }
}
