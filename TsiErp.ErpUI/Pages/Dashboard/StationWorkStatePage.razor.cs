﻿using DevExpress.Blazor.Internal;
using Syncfusion.Blazor.DropDowns;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.ErpUI.Pages.Dashboard
{
    public partial class StationWorkStatePage
    {
        List<StationCardDto> Stations = new List<StationCardDto>();

        protected override async Task OnInitializedAsync()
        {
            GetStations();

            Floors.Clear();

            Floors.Add(L["Combobox1.Floor"]);
            Floors.Add(L["Combobox0.Floor"]);
            Floors.Add(L["Combobox-1.Floor"]);
        }

        #region İstasyon Metotları
        private async void GetStations()
        {

            Stations.Clear();

            var erpStations = (await StationsService.GetListAsync(new ListStationsParameterDto())).Data.Where(t => t.IsIotStation == true).ToList();

            var assetpath = "images/Maintenance/";

            foreach (var station in erpStations)
            {
                Stations.Add(new StationCardDto
                {
                    Code = station.Code,
                    StationGroupName = station.StationGroup,
                    ImageUrl = station.StationWorkStateEnum == Entities.Enums.StationWorkStateEnum.Operasyonda
                    ? assetpath + station.Code.Trim() + "on.gif"
                    : assetpath + station.Code.Trim() + "off.png",
                    Floor = station.StationFloor,
                    StationWorkStateEnum = station.StationWorkStateEnum
                });
            }
        }

        private async void GetStations(string[] floors)
        {
            Stations.Clear();

            if (floors != null)
            {

                var erpStations = (await StationsService.GetListAsync(new ListStationsParameterDto())).Data.Where(t => t.IsIotStation == true && floors.Contains(t.StationFloor)).ToList();

                var assetpath = "images/Maintenance/";

                foreach (var station in erpStations)
                {
                    Stations.Add(new StationCardDto
                    {
                        Code = station.Code,
                        StationGroupName = station.StationGroup,
                        ImageUrl = station.StationWorkStateEnum == Entities.Enums.StationWorkStateEnum.Operasyonda
                        ? assetpath + station.Code.Trim() + "on.gif"
                        : assetpath + station.Code.Trim() + "off.png",
                        Floor = station.StationFloor,
                        StationWorkStateEnum = station.StationWorkStateEnum
                    });
                }
            }
            else
            {

                var erpStations = (await StationsService.GetListAsync(new ListStationsParameterDto())).Data.Where(t => t.IsIotStation == true).ToList();

                var assetpath = "images/Maintenance/";

                foreach (var station in erpStations)
                {
                    Stations.Add(new StationCardDto
                    {
                        Code = station.Code,
                        StationGroupName = station.StationGroup,
                        ImageUrl = station.StationWorkStateEnum == Entities.Enums.StationWorkStateEnum.Operasyonda
                        ? assetpath + station.Code.Trim() + "on.gif"
                        : assetpath + station.Code.Trim() + "off.png",
                        Floor = station.StationFloor,
                        StationWorkStateEnum = station.StationWorkStateEnum
                    });
                }
            }

        }

        #endregion


        #region Filtre İşlemleri

        public string[] SelectedFloor { get; set; }
        public string[] SelectedWorkState { get; set; }

        public List<string> Floors = new List<string>() {};

        public List<StationCardDto> WorkStates = new List<StationCardDto>();

        #endregion

        public void FilterButtonClicked()
        {
            GetStations(SelectedFloor);
        }

        public void RefreshButtonClicked()
        {
            GetStations(SelectedWorkState);
            SelectedFloor = null;
        }

        public class StationCardDto
        {
            public string Code { get; set; }

            public string StationGroupName { get; set; }

            public string ImageUrl { get; set; }

            public string Floor { get; set; }

            public StationWorkStateEnum StationWorkStateEnum { get; set; }
        }
    }
}
