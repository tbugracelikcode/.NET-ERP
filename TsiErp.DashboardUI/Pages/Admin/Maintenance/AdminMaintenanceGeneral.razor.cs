using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using System.Timers;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services;

namespace TsiErp.DashboardUI.Pages.Admin.Maintenance
{
    public partial class AdminMaintenanceGeneral
    {
        public List<int> topluBakimIDList = new();
        List<BakimIhtiyacListesi> ihtiyacList = new();
        List<BakimIhtiyacListesi> topluIhtiyacList = new();
        List<BakimTalimatlar> talimatList = new();
        public List<SatinAlmaDetaylari> satinAlmaList = new();
        private List<StationCards> _cardDatas;
        public List<StationCards> CardDatas

        {
            get { return _cardDatas; }
            set { _cardDatas = value; }
        }

        #region Değişkenler

        [Inject] public IJSRuntime JsRuntime { get; set; }
        public string SearchValue { get; set; }
        public string SortingValue { get; set; }
        public string[] FilteringValue { get; set; }
        string TitleStation = string.Empty;
        private System.Timers.Timer _timer;

        bool PopupVisible { get; set; } = false;
        bool PopupVisible2 { get; set; } = false;
        bool PopupVisibleAll { get; set; } = false;
        private int? selectedTimeIndex { get; set; } = 61;

        #endregion

        protected override void OnInitialized()
        {
            _cardDatas = this.GetCardDatas();
            base.OnInitialized();

            var list = DBHelper.GetMaintenanceRecords();
            _timer = new();
            _timer.Interval = 6000000;
            _timer.Elapsed += async (object? sender, ElapsedEventArgs e) =>
            {
                timerEvent();
                await InvokeAsync(StateHasChanged);
            };
            _timer.Enabled = true;
        }

        void MaintenancePopupClosing2(PopupClosingEventArgs args)
        {
            PopupVisible = false;
            PopupVisibleAll = false;
            PopupVisible2 = false;
        }

        void MaintenancePopupClosing(PopupClosingEventArgs args)
        {
            PopupVisible = false;
            PopupVisibleAll = false;
        }

        private void OnPurchaseReceiptClicked()
        {
            var satinAlmaListVT = DBHelper.GetPurchaseDetails().OrderByDescending(t => t.TARIH).ToList();
            if (PopupVisibleAll)
            {
                foreach (var item in topluIhtiyacList.Where(t => t.IHTIYACMIKTARI > 0))
                {
                    int stokID = item.STOKID;
                    SatinAlmaDetaylari satir = new SatinAlmaDetaylari
                    {
                        STOKID = stokID,
                        STOKKODU = item.ESKISTOKKODU,
                        CARIUNVAN = satinAlmaListVT.Where(t => t.STOKID == stokID).Select(t => t.CARIUNVAN).FirstOrDefault(),
                        BIRIMFIYATDECIMAL = Convert.ToDecimal(satinAlmaListVT.Where(t => t.STOKID == stokID).Select(t => t.BIRIMFIYAT).FirstOrDefault()),
                        BIRIMSETKOD = item.BIRIMSETKOD,
                        IHTIYACMIKTARI = item.IHTIYACMIKTARI,
                        PARABIRIMI = satinAlmaListVT.Where(t => t.STOKID == stokID).Select(t => t.PARABIRIMI).FirstOrDefault(),
                        PARABIRIMIID = satinAlmaListVT.Where(t => t.STOKID == stokID).Select(t => t.PARABIRIMIID).FirstOrDefault(),
                        TOPLAMFIYAT = Convert.ToDecimal(satinAlmaListVT.Where(t => t.STOKID == stokID).Select(t => t.BIRIMFIYAT).FirstOrDefault()) * item.IHTIYACMIKTARI
                    };
                    satinAlmaList.Add(satir);
                }
                PopupVisible = false;
                PopupVisibleAll = false;
                PopupVisible2 = true;
            }
            else if (PopupVisible)

            {
                foreach (var item in ihtiyacList.Where(t => t.IHTIYACMIKTARI > 0))
                {
                    int stokID = item.STOKID;
                    SatinAlmaDetaylari satir = new SatinAlmaDetaylari
                    {
                        STOKID = stokID,
                        STOKKODU = item.ESKISTOKKODU,
                        CARIUNVAN = satinAlmaListVT.Where(t => t.STOKID == stokID).Select(t => t.CARIUNVAN).FirstOrDefault(),
                        BIRIMFIYATDECIMAL = Convert.ToDecimal(satinAlmaListVT.Where(t => t.STOKID == stokID).Select(t => t.BIRIMFIYAT).FirstOrDefault()),
                        BIRIMSETKOD = item.BIRIMSETKOD,
                        IHTIYACMIKTARI = item.IHTIYACMIKTARI,
                        PARABIRIMI = satinAlmaListVT.Where(t => t.STOKID == stokID).Select(t => t.PARABIRIMI).FirstOrDefault(),
                        PARABIRIMIID = satinAlmaListVT.Where(t => t.STOKID == stokID).Select(t => t.PARABIRIMIID).FirstOrDefault(),
                        TOPLAMFIYAT = Convert.ToDecimal(satinAlmaListVT.Where(t => t.STOKID == stokID).Select(t => t.BIRIMFIYAT).FirstOrDefault()) * item.IHTIYACMIKTARI
                    };
                    satinAlmaList.Add(satir);
                }
                PopupVisible = false;
                PopupVisibleAll = false;
                PopupVisible2 = true;
            }
        }

        private void ValueChangeHandler(ChangeEventArgs<int?, ComboboxTimePeriods> args)
        {
            if (args.Value != null)
            {
                selectedTimeIndex = args.Value;
                StateHasChanged();
            }
            else
            {
                selectedTimeIndex = 61;
                StateHasChanged();
            }
        }

        public void CellInfoHandler(QueryCellInfoEventArgs<BakimIhtiyacListesi>
        Args)
        {
            var value = Args.Data.IHTIYACMIKTARI;
            //if (Args.Column.Field == "IHTIYACMIKTARI")
            //{
            if (value > 0)
            {
                Args.Cell.AddStyle(new string[] { "background-color: #DF2C2C; color: white; " });
            }
            //}
            StateHasChanged();
        }


        public void timerEvent()
        {
            if (SearchValue == null && SortingValue == null && FilteringValue == null)
            {
                _cardDatas = this.GetCardDatas();
            }
        }

        public void OnClickMaintenance(string stationCode, DateTime maintenanceDate, int maintenanceID)
        {
            ihtiyacList.Clear();
            talimatList.Clear();
            PopupVisible = true;
            TitleStation = stationCode + " - " + maintenanceDate.ToString("dd.MM.yyyy") + " Tarihli Planlanan Bakım Detayları";
            var list = DBHelper.GetMaintenanceLineRecords().Where(t => t.BAKIMID == maintenanceID).ToList();

            foreach (var item in list.Where(t => t.IHTIYACMIKTARI > 0).ToList())
            {
                BakimIhtiyacListesi satir = new BakimIhtiyacListesi
                {
                    STOKID = item.STOKID,
                    ESKISTOKKODU = item.ESKISTOKKODU,
                    STOKACIKLAMASI = item.STOKACIKLAMASI,
                    BIRIMSETKOD = item.BIRIMSETKOD,
                    BAKIMMIKTARI = item.IHTIYACMIKTARI,
                    STOKMIKTARI = item.STOKMIKTARI,
                    IHTIYACMIKTARI = item.IHTIYACMIKTARI - item.STOKMIKTARI < 0 ? 0 : item.IHTIYACMIKTARI - item.STOKMIKTARI
                };
                ihtiyacList.Add(satir);
            }

            foreach (var item in list)
            {
                BakimTalimatlar talimat = new BakimTalimatlar
                {
                    BAKIMID = item.BAKIMID,
                    TALIMAT = item.BAKIMTALIMATI
                };
                talimatList.Add(talimat);
            }

        }
        public void OnClickAllMaintenance()
        {
            _timer.Enabled = false;
            topluIhtiyacList.Clear();
            PopupVisibleAll = true;
            List<BakimSatirlari> list = new();
            foreach (var bakimID in topluBakimIDList.Distinct())
            {
                list.AddRange(DBHelper.GetMaintenanceLineRecords().Where(t => t.BAKIMID == bakimID).ToList());
            }

            foreach (var item in list.Where(t => t.IHTIYACMIKTARI > 0).ToList())
            {
                if (topluIhtiyacList.Any(x => x.STOKID == item.STOKID))
                {
                    var satir = topluIhtiyacList.Find(t => t.STOKID == item.STOKID);

                    if (satir != null)
                    {
                        satir.BAKIMMIKTARI = satir.BAKIMMIKTARI + item.IHTIYACMIKTARI;
                        satir.IHTIYACMIKTARI = satir.BAKIMMIKTARI - item.STOKMIKTARI < 0 ? 0 : satir.BAKIMMIKTARI - item.STOKMIKTARI;
                    }
                }
                else
                {
                    BakimIhtiyacListesi satir = new BakimIhtiyacListesi
                    {
                        STOKID = item.STOKID,
                        ESKISTOKKODU = item.ESKISTOKKODU,
                        STOKACIKLAMASI = item.STOKACIKLAMASI,
                        BIRIMSETKOD = item.BIRIMSETKOD,
                        BAKIMMIKTARI = item.IHTIYACMIKTARI,
                        STOKMIKTARI = item.STOKMIKTARI,
                        IHTIYACMIKTARI = item.IHTIYACMIKTARI - item.STOKMIKTARI < 0 ? 0 : item.IHTIYACMIKTARI - item.STOKMIKTARI
                    };
                    topluIhtiyacList.Add(satir);
                }

            }
            _timer.Enabled = true;


        }

        public class StationCards
        {
            public string ImageUrl { get; set; }
            public string StationCode { get; set; }
            public int TimeLeft { get; set; }
            public DateTime PlannedMaintenanceDate { get; set; }
            public string Period { get; set; }
            public string StationDepartment { get; set; }
            public string MaintenanceType { get; set; }
            public string Floor { get; set; }
            public int MaintenanceID { get; set; }
        }


        private string GetFloor(int floor)
        {
            switch (floor)
            {
                case 1:
                    return "-1. Kat";
                    break;
                case 2:
                    return "Zemin Kat";
                    break;
                case 3:
                    return "1.Kat";
                    break;
                default:
                    return "Zemin Kat";
                    break;
            }


        }

        #region Listeler

        public List<StationCards> GetCardDatas()
        {
            topluBakimIDList.Clear();
            var assetpath = "images/Maintenance/";
            List<StationCards>
                CardData = new List<StationCards>
                    ();

            var stations = DBHelper.GetStations().Where(t => t.VERITOPLAMA == true).ToList();
            var canli = DBHelper.GetMaintenanceRecordsView().OrderByDescending(t => t.TARIH).ToList();
            if (stations != null)
            {
                foreach (var item in stations)
                {
                    var tempCanli = canli.Where(t => t.ISTID == item.ID).ToList();
                    int kalanSure = tempCanli.Where(t => t.DURUM == "Yapılmadı").OrderBy(t => t.KALANSURE).Select(t => t.KALANSURE).FirstOrDefault();
                    StationCards stationCards = new StationCards
                    {
                        ImageUrl = tempCanli.Select(t => t.CALISMABILGISI).FirstOrDefault() != 0 && tempCanli.Select(t => t.CALISMABILGISI).FirstOrDefault() != 8 ? assetpath + item.MAKINEKODU.Trim() + "on.gif" : assetpath + item.MAKINEKODU.Trim() + "off.png",
                        StationCode = item.MAKINEKODU,
                        StationDepartment = tempCanli.Select(t => t.BOLUM).FirstOrDefault(),
                        Period = tempCanli.Where(t => t.KALANSURE <= kalanSure && t.DURUM == "Yapılmadı").Select(t => t.PERIYOT).FirstOrDefault().ToString(),
                        PlannedMaintenanceDate = tempCanli.Where(t => t.KALANSURE <= kalanSure && t.DURUM == "Yapılmadı").Select(t => t.PLANLANANTARIH).FirstOrDefault(),
                        TimeLeft = kalanSure / 22500,
                        MaintenanceType = tempCanli.Where(t => t.KALANSURE <= kalanSure && t.DURUM == "Yapılmadı").Select(t => t.BAKIMTURU).FirstOrDefault(),
                        Floor = GetFloor(tempCanli.Select(t => t.KAT).FirstOrDefault()),
                        MaintenanceID = tempCanli.Where(t => t.KALANSURE <= kalanSure && t.DURUM == "Yapılmadı").Select(t => t.ID).FirstOrDefault()
                    };
                    if (stationCards.PlannedMaintenanceDate != DateTime.MinValue)
                    {
                        CardData.Add(stationCards);
                        topluBakimIDList.Add(stationCards.MaintenanceID);
                    }
                }
            }
            return CardData.OrderBy(t => t.TimeLeft).ToList();
        }

        public List
       <StationCards>
           GetSortedCardDatas(List<StationCards>
               sortingCards)
        {
            if (this.SortingValue != null)
            {
                sortingCards.Sort((x, y) => x.TimeLeft.CompareTo(y.TimeLeft));
                if (this.SortingValue == "Azalan")
                {
                    sortingCards.Reverse();
                }
            }
            return sortingCards;
        }

        public List<StationCards>
          GetCardDatas(string[] values)
        {
            var cardDatas = GetCardDatas();
            if (values == null)
            {
                return this.GetSortedCardDatas(cardDatas);
            }
            var filterCards = new List<StationCards>
                ();
            foreach (string value in values)
            {
                var cards = cardDatas.Where(e => e.Floor == value);
                foreach (var card in cards)
                {
                    filterCards.Add(card);
                }
            }
            return this.GetSortedCardDatas(filterCards);
        }

        public List<string>
            SortingType = new List<string>
                () { "Artan", "Azalan" };
        public List<string>
            FilteringType = new List<string>
                () { "1.Kat", "Zemin Kat", "-1. Kat" };

        #endregion
        public async Task NavigateToUrlAsync(string url)
        {
            await JsRuntime.InvokeAsync
            <object>
                ("open", url, "_blank");
        }

        public void OnSearch(Syncfusion.Blazor.Inputs.ChangedEventArgs args)
        {
            if (args.Value != null)
            {
                string value = args.Value.ToUpper();
                this.CardDatas = this.CardDatas.FindAll(e => e.StationCode.Contains(value) || e.StationDepartment.Contains(value) || e.MaintenanceType.Contains(value));
            }
            else
            {
                this.CardDatas = this.GetCardDatas(this.FilteringValue);
            }
        }

        public void OnSorting(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, string>
            args)
        {
            this.CardDatas = this.GetSortedCardDatas(this.CardDatas);
        }

        public void OnFiltering(MultiSelectChangeEventArgs<string[]>
            args)
        {
            this.CardDatas = this.GetCardDatas(args.Value);
        }

        public void OnReset()
        {
            this.SearchValue = null;
            this.SortingValue = null;
            this.FilteringValue = null;
            this.CardDatas = this.GetCardDatas();
            this.selectedTimeIndex = 61;
        }

        #region Combobox

        private List<ComboboxTimePeriods> timeperiods = new List<ComboboxTimePeriods>() {
        new ComboboxTimePeriods(){ TimeID= 121, TimeText= "Son 4 Ay" },
        new ComboboxTimePeriods(){ TimeID= 91, TimeText= "Son 3 Ay" },
        new ComboboxTimePeriods(){ TimeID= 61, TimeText= "Son 2 Ay" },
        new ComboboxTimePeriods(){ TimeID= 31, TimeText= "Son 1 Ay" },
        new ComboboxTimePeriods(){ TimeID= 8, TimeText= "Son 1 Hafta" }
    };

        #endregion
    }
}
