using System;
using System.Diagnostics;
using System.Windows;
using ESFT.GroundStation.GUI.View;
using ESFT.GroundStation.Helpers;

namespace ESFT.GroundStation.GUI {
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : IDisposable {
        private void App_Startup(object sender, StartupEventArgs e) {
            var wndMain = new MainWindowView();
            wndMain.Show();
        }

        protected override void OnStartup(StartupEventArgs e) {
            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Listeners.Add(new DebugTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning | SourceLevels.Error;
            base.OnStartup(e);
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (_disposedValue) return;
            if (disposing) SerialPortWatcherService.CleanUp();

            _disposedValue = true;
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion
    }

    public class DebugTraceListener : TraceListener {
        public override void Write(string message) {
        }

        public override void WriteLine(string message) {
            Debugger.Break();
        }
    }
}
