using FluentValidation;

namespace WebApi.DTOs.StageReportAttachedFile
{
    public class CreateStageReportAttachedFileRequestValidation : AbstractValidator<CreateStageReportAttachedFileRequest>
    {
        public CreateStageReportAttachedFileRequestValidation()
        {
            RuleFor(e => e.StageReportId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(e => e.File).NotEmpty();
            RuleFor(e => e.File!.FileName).NotEmpty();
            RuleFor(e => e.File!.FileContent).NotEmpty();
        }
    }
}
