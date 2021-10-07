using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Particle_RoomTemperature.ViewModels;
using System;

namespace Particle_RoomTemperature
{
    public partial class AllMeasurementsPage : ContentPage
    {
        public AllMeasurementsPage()
        {
            InitializeComponent();
            BindingContext= new AllMeasurements_ViewModel();
        }
    }
}
