using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.Notifications;
using Timebox.Core;
using Timebox.MVVM.ViewModel;
using Timebox.Services;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;

namespace Timebox
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<LoadingScreenViewModel>();
            services.AddSingleton<AlarmsViewModel>();
            services.AddSingleton<HourglassViewModel>();

            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<Func<Type, ViewModel>>(serviceProvider => viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //ToastNotificationManagerCompat.OnActivated += ToastNotificationManagerCompat_OnActivated;

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        internal static void ToastNotificationManagerCompat_OnActivated(ToastNotificationActivatedEventArgsCompat toastArgs)
        {
            // Obtain the arguments from the notification
            ToastArguments args = ToastArguments.Parse(toastArgs.Argument);

            // Obtain any user input (text boxes, menu selections) from the notification
            ValueSet userInput = toastArgs.UserInput;

            // Need to dispatch to UI thread if performing UI operations

            //Application.Current.Dispatcher.Invoke(delegate
            //{
                //switch (args["action"])
                //{
                //    case "ALARM_OK": Alarm.StopSound(); break;
                //    case "ALARM_NOTIFICATION_CLICK": Alarm.StopSound(); break;
                //    case "HOURGLASS_OK": Hourglass.StopSound(); break;
                //    case "HOURGLASS_NOTIFICATION_CLICK": Hourglass.StopSound(); break;
                //}

                //string id = args["conversationId"];
                //MessageBox.Show(id);
            //});
        }
    }
}
