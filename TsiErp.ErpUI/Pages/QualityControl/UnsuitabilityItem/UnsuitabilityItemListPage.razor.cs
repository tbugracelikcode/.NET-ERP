using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.UnsuitabilityItem
{
    partial class UnsuitabilityItemListPage
    {
        private List<SelectionList> SelectionLists = new List<SelectionList>() { new SelectionList { Text = "Evet", Id=1 }, new SelectionList { Text = "Hayır", Id=2 } };
    

    protected override void OnInitialized()
        {
            BaseCrudService = UnsuitabilityItemsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectUnsuitabilityItemsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        public void LifeThreateningOnChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, SelectionList> args)
        {
        }

        public void LossOfPrestigeOnChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, SelectionList> args)
        {
        }

        public void ExtraCostOnChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, SelectionList> args)
        {
        }

        public void ProductLifeShorteningOnChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, SelectionList> args)
        {
        }

        public void DetectabilityOnChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, SelectionList> args)
        {
        }

        public void ToBeUsedAsOnChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, SelectionList> args)
        {
        }
    }


    public class SelectionList
    {
        public int Id { get; set; }

        public string Text { get; set; }
    }
}
