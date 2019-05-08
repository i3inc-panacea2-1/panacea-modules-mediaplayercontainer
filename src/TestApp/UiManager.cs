using Panacea.Modularity.UiManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TestApp
{
    class UiManager : IUiManagerPlugin
    {
        private readonly ContentControl _content;

        public UiManager(ContentControl window)
        {
            _content = window;
        }

        public IReadOnlyList<FrameworkElement> History => throw new NotImplementedException();

        public FrameworkElement CurrentPage => throw new NotImplementedException();

        public bool IsNavigationDisabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsHomeTheCurrentPage => throw new NotImplementedException();

        public bool IsPaused => throw new NotImplementedException();

        public event EventHandler<BeforeNavigateEventArgs> BeforeNavigate;
        public event EventHandler<BeforeNavigateEventArgs> Back;
        public event EventHandler AfterNavigate;
        public event EventHandler Paused;
        public event EventHandler Resumed;

        public void AddToolButton(string text, string namesp, string iconUrl, Action action)
        {
            throw new NotImplementedException();
        }

        public Task BeginInit()
        {
            throw new NotImplementedException();
        }

        public void DisableFullscreen()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task DoWhileBusy(Func<Task> action)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> DoWhileBusy<TResult>(Func<Task<TResult>> action)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> DownloadDataAsync(string url)
        {
            throw new NotImplementedException();
        }

        public void EnableFullscreen()
        {
            throw new NotImplementedException();
        }

        public Task EndInit()
        {
            throw new NotImplementedException();
        }

        public void GoBack(int count = 1)
        {
            throw new NotImplementedException();
        }

        public void GoHome()
        {
            throw new NotImplementedException();
        }

        public void HideAllPopups()
        {
            throw new NotImplementedException();
        }

        public void HideKeyboard()
        {
            throw new NotImplementedException();
        }

        public void HideNotifications()
        {
            throw new NotImplementedException();
        }

        public void HidePopup(IPopup element)
        {
            throw new NotImplementedException();
        }

        public void HidePopup(object element)
        {
            throw new NotImplementedException();
        }

        public void Navigate(FrameworkElement page, bool cache = true)
        {
            _content.Content = page;
        }

        public FrameworkElement Notify(string message, Action del)
        {
            throw new NotImplementedException();
        }

        public void Notify(FrameworkElement c)
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Refrain(FrameworkElement c)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RequestLogin(string text)
        {
            throw new NotImplementedException();
        }

        public void RequestMagicPin()
        {
            throw new NotImplementedException();
        }

        public void Restart(string message, Exception exception = null)
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void SetNavigationBarControl(FrameworkElement c)
        {
            throw new NotImplementedException();
        }

        public void ShowKeyboard(KeyboardTypes keyboardType = KeyboardTypes.Normal)
        {
            throw new NotImplementedException();
        }

        public void ShowNotifications()
        {
            throw new NotImplementedException();
        }

        public IPopup ShowPopup(FrameworkElement element, string title = null, PopupType popupType = PopupType.None, bool closable = true, bool trasnparent = true)
        {
            throw new NotImplementedException();
        }

        public Task Shutdown()
        {
            throw new NotImplementedException();
        }

        public void Toast(string text, int timeout = 5000)
        {
            throw new NotImplementedException();
        }
    }
}
