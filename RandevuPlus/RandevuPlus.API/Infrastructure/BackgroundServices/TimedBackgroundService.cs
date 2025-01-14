using Microsoft.AspNetCore.SignalR;
using RandevuPlus.API.Infrastructure.Sockets;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Enums;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.Infrastructure.BackgroundServices
{
    public class TimedBackgroundService : BackgroundService
    {
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);
        private Timer _timer;
        private readonly IHubContext<UserHub> _hubContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TimedBackgroundService(IHubContext<UserHub> hubContext, IServiceScopeFactory serviceScopeFactory)
        {
            _hubContext = hubContext;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoWorkAsync(stoppingToken);
            _timer = new Timer(async _ => await DoWorkAsync(stoppingToken), null, _interval, _interval);
            await Task.CompletedTask;
        }

        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                _timer?.Change(Timeout.Infinite, 0);
                return;
            }
            Console.WriteLine($"Background service is working at {DateTime.Now}");

            using (var scope = _serviceScopeFactory.CreateScope())
            {

                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                var endedAppointments = await unitOfWork.Appointments.GetEndedAppointmentsAsync();
                foreach (var endedAppointment in endedAppointments)
                {
                    endedAppointment.Status = AppointmentStatus.Completed;
                    await unitOfWork.Appointments.UpdateAsync(endedAppointment);
                    await unitOfWork.CommitAsync();
                    var onlineUsers = userService.GetOnlineUsers();
                    if (onlineUsers != null && onlineUsers.Contains(endedAppointment.UserId.ToString()))
                    {
                        await _hubContext.Clients.User(endedAppointment.UserId.ToString()).SendAsync("AppointmentEnded", endedAppointment.Id);
                    }
                    if (onlineUsers != null && onlineUsers.Contains(endedAppointment.Instructor.UserId.ToString()))
                    {
                        await _hubContext.Clients.User(endedAppointment.Instructor.UserId.ToString()).SendAsync("AppointmentEnded", endedAppointment.Id);
                    }
                }
            }

            await Task.Delay(1000);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return base.StopAsync(cancellationToken);
        }
    }
}
