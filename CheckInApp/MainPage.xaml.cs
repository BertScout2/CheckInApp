using CheckInApp.Data;
using CheckInApp.Models;
using Microsoft.Extensions.Logging;
using static CheckInApp.Data.Constants;

namespace CheckInApp
{
    public partial class MainPage : ContentPage
    {
        private readonly CheckInRepository _repository;
        private readonly ILogger _logger;

        public MainPage(CheckInRepository checkInRepository, ILogger<CheckInRepository> logger)
        {
            InitializeComponent();
            _repository = checkInRepository;
            _logger = logger;
            InitDatabasePath();
        }

        private void EntryNameChanged(object? sender, EventArgs e)
        {
            if (EntryName.Text != "")
            {
                LabelResult.Text = "";
            }
        }
        
        private void CheckInClicked(object? sender, EventArgs e)
        {
            try
            {
                var item = new CheckIn
                {
                    Name = EntryName.Text,
                    CheckInTime = DateTimeOffset.Now,
                };
                var result = Task.FromResult(_repository.SaveItemAsync(item));
                LabelResult.Text = $"{item.Name} has checked in.";
                EntryName.Text = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving CheckIn item");
            }
        }

        private void CheckOutClicked(object? sender, EventArgs e)
        {
            try
            {
                var item = new CheckIn
                {
                    Name = EntryName.Text,
                    CheckOutTime = DateTimeOffset.Now,
                };
                var result = Task.FromResult(_repository.SaveItemAsync(item));
                LabelResult.Text = $"{item.Name} has checked out.";
                EntryName.Text = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving CheckIn item");
            }
        }
    }
}
