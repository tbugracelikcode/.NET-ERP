using Blazored.Modal;
using Blazored.Modal.Services;
using TsiErp.ErpUI.Utilities.ModalUtilities.ModalComponents;

namespace TsiErp.ErpUI.Utilities.ModalUtilities
{
    public class ModalManager
    {
        private readonly IModalService modalService;

        public ModalManager(IModalService modalService)
        {
            this.modalService = modalService;
        }

        public async Task<bool> ConfirmationAsync(String Title, String Message)
        {
            ModalParameters mParams = new ModalParameters();
            mParams.Add("Message", Message);

            var modalRef = modalService.Show<ConfirmationPopupComponent>(Title, mParams);
            var modalResult = await modalRef.Result;

            return !modalResult.Cancelled;
        }
    }
}
