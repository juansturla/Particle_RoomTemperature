using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Particle_RoomTemperature.Models;
using Particle_RoomTemperature.Services;
using Particle_RoomTemperature.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace Particle_RoomTemperature
{
    public partial class AllMeasurementsPage : ContentPage
    {
        public AllMeasurementsPage()
        {
            InitializeComponent();
            BindingContext = new AllMeasurements_ViewModel();
        }
    }
}
