using DataCore.Entities;


namespace Services.Models._BaseModels
{
    public class SuccessfullCreateModel : ModelBase
    {
        public SuccessfullCreateModel(EntityBase resultModel)
        {
            Id = resultModel.Id;
            CreatedByUser = resultModel.CreatedByUser;
            UpdatedByUser = resultModel.UpdatedByUser;
            CreatedOn = resultModel.CreatedOn;
            UpdatedOn = resultModel.UpdatedOn;
        }
        public SuccessfullCreateModel(DataCore.Entities.AvetonUser resultModel)
        {
            Id = resultModel.Id;
            CreatedOn = resultModel.CreatedOn;
            UpdatedOn = resultModel.UpdatedOn;
        }
    }
}
