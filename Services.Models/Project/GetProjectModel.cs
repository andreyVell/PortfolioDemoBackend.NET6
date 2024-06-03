using Services.Models._BaseModels;
using Services.Models.Client;
using Services.Models.Division;
using Services.Models.Employee;

namespace Services.Models.Project
{
    public class GetProjectModel : ModelBase
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? ManagerId { get; set; }
        public double CurrentProgress { get; set; }
        /// <summary>
        /// Менеджер
        /// </summary>
        public virtual GetEmployeeShortModel? Manager { get; set; }
        /// <summary>
        /// Заказчики
        /// </summary>
        public virtual List<GetClientModel>? Clients { get; set; }
        /// <summary>
        /// Исполнители
        /// </summary>
        public virtual List<GetDivisionModel>? Contractors { get; set; }
    }
}
