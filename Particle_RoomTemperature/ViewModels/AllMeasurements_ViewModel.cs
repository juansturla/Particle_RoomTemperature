
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
 using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Particle_RoomTemperature.Models;
using Particle_RoomTemperature.Services;

namespace Particle_RoomTemperature.ViewModels
{
    public class AllMeasurements_ViewModel : BaseViewModel
    {
        public ObservableCollection<Measurement> Measurements {  get; set; }
        public AllMeasurements_ViewModel()
        {
            Measurements = (ObservableCollection<Measurement>)Particle_Service.GetRoomMeasurements();
        }
    }
}
