using Microsoft.AspNetCore.SignalR;
using RandevuPlus.API.Infrastructure.Sockets;
using RandevuPlus.API.Infrastructure.UnitOfWork;
using RandevuPlus.API.Shared.Constants;
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

                if (await unitOfWork.Appointments.CheckEndedAppointmentsAsync())
                {
                    var endedAppointments = await unitOfWork.Appointments.GetEndedAppointmentsAsync();
                    foreach (var endedAppointment in endedAppointments)
                    {
                        endedAppointment.Status = AppointmentStatus.Completed;
                        await unitOfWork.Appointments.UpdateAsync(endedAppointment);

                        Notification userNotification = new()
                        {
                            ReceiverId = endedAppointment.UserId,
                            NotificationText = NotificationTexts.AppointmentCompleteUser(endedAppointment.Instructor.User.FullName,
                                endedAppointment.Instructor.Title ?? string.Empty, endedAppointment.Course.Name, endedAppointment.Date, endedAppointment.SlotStartIndex, endedAppointment.SlotEndIndex)
                        };
                        await unitOfWork.Notifications.AddAsync(userNotification);
                        Notification instructorNotification = new()
                        {
                            ReceiverId = endedAppointment.Instructor.UserId,
                            NotificationText = NotificationTexts.AppointmentCompleteInstructor(endedAppointment.Course.Name, endedAppointment.Date, endedAppointment.SlotStartIndex, endedAppointment.SlotEndIndex)
                        };
                        await unitOfWork.Notifications.AddAsync(instructorNotification);
                        await unitOfWork.CommitAsync();

                        var onlineUsers = userService.GetOnlineUsers();
                        if (onlineUsers.Contains(endedAppointment.UserId.ToString()))
                        {
                            await _hubContext.Clients.User(endedAppointment.UserId.ToString()).SendAsync("AppointmentEnded", endedAppointment.Id);
                            await _hubContext.Clients.User(endedAppointment.UserId.ToString()).SendAsync("NotificationReceived");
                        }
                        if (onlineUsers.Contains(endedAppointment.Instructor.UserId.ToString()))
                        {
                            await _hubContext.Clients.User(endedAppointment.Instructor.UserId.ToString()).SendAsync("AppointmentEnded", endedAppointment.Id);
                            await _hubContext.Clients.User(endedAppointment.Instructor.UserId.ToString()).SendAsync("NotificationReceived");
                        }
                    }

                    if (await unitOfWork.Appointments.CheckApproachingAppointmentsAsync())
                    {
                        var approachingAppointments = await unitOfWork.Appointments.GetApproachingAppointmentsAsync();
                        foreach (var approachingAppointment in approachingAppointments)
                        {
                            Notification userNotification = new()
                            {
                                ReceiverId = approachingAppointment.UserId,
                                NotificationText = NotificationTexts.AppointmentReminderUser(approachingAppointment.Instructor.User.FullName,
                                    approachingAppointment.Instructor.Title ?? string.Empty, approachingAppointment.Course.Name, approachingAppointment.Date, approachingAppointment.SlotStartIndex, approachingAppointment.SlotEndIndex)
                            };
                            await unitOfWork.Notifications.AddAsync(userNotification);
                            Notification instructorNotification = new()
                            {
                                ReceiverId = approachingAppointment.Instructor.UserId,
                                NotificationText = NotificationTexts.AppointmentReminderInstructor(approachingAppointment.Course.Name, approachingAppointment.Date, approachingAppointment.SlotStartIndex, approachingAppointment.SlotEndIndex)
                            };
                            await unitOfWork.Notifications.AddAsync(instructorNotification);
                            await unitOfWork.CommitAsync();

                            var onlineUsers = userService.GetOnlineUsers();
                            if (onlineUsers.Contains(approachingAppointment.UserId.ToString()))
                            {
                                await _hubContext.Clients.User(approachingAppointment.UserId.ToString()).SendAsync("NotificationReceived");
                            }
                            if (onlineUsers.Contains(approachingAppointment.Instructor.UserId.ToString()))
                            {
                                await _hubContext.Clients.User(approachingAppointment.Instructor.UserId.ToString()).SendAsync("NotificationReceived");
                            }
                        }
                    }
                }

                await Task.Delay(1000);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return base.StopAsync(cancellationToken);
        }
    }
}
