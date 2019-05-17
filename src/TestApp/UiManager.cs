using Panacea.Modularity.UiManager;
using Panacea.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TestApp
{
    class UiManagerPlugin : IUiManagerPlugin
    {
        private readonly ContentControl _content;

        public UiManagerPlugin(ContentControl window)
        {
            _content = window;
        }

        public Task BeginInit()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            
        }

        public Task EndInit()
        {
            return Task.CompletedTask;
        }

        public IUiManager GetUiManager()
        {
            return new UiManager(_content);
        }

        public Task Shutdown()
        {
            return Task.CompletedTask;
        }
    }

    class UiManager : IUiManager
    {
        public UiManager(ContentControl window)
        {
            _content = window;
        }
        private readonly ContentControl _content;

        public IReadOnlyList<ViewModelBase> History => throw new NotImplementedException();

        public ViewModelBase CurrentPage => throw new NotImplementedException();

        public bool IsNavigationDisabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsHomeTheCurrentPage => throw new NotImplementedException();

        public bool IsPaused => throw new NotImplementedException();

        public event EventHandler<BeforeNavigateEventArgs> BeforeNavigate;
        public event EventHandler<BeforeNavigateEventArgs> Back;
        public event EventHandler AfterNavigate;
        public event EventHandler Paused;
        public event EventHandler Resumed;

   

        public void DisableFullscreen()
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

        public void HidePopup(ViewModelBase element)
        {
            throw new NotImplementedException();
        }

        

        public void Navigate(ViewModelBase page, bool cache = true)
        {
            _content.Content = page.View;
        }

        public void Notify(string message, Action del)
        {
            throw new NotImplementedException();
        }

        public void Notify(ViewModelBase c)
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Refrain(ViewModelBase c)
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

        public void SetNavigationBarControl(ViewModelBase c)
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

        public void ShowPopup(ViewModelBase element, string title = null, PopupType popupType = PopupType.None, bool closable = true, bool trasnparent = true)
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

        public void AddMainPageControl(ViewModelBase c)
        {
            throw new NotImplementedException();
        }

        public void RemoveMainPageControl(ViewModelBase c)
        {
            throw new NotImplementedException();
        }

        public void AddNavigationBarControl(ViewModelBase c)
        {
            throw new NotImplementedException();
        }

        public void RemoveNavigationBarControl(ViewModelBase c)
        {
            throw new NotImplementedException();
        }

        public void AddSettingsControl(ViewModelBase c)
        {
            throw new NotImplementedException();
        }

        public void RemoveSettingsControl(ViewModelBase c)
        {
            throw new NotImplementedException();
        }
    }
}
