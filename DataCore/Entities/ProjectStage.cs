namespace DataCore.Entities
{
    public class ProjectStage : EntityBase
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? ParentStageId { get; set; }
        public int OrderNumber { get; set; }
        public virtual Project? Project { get; set; }
        public virtual ProjectStage? ParentStage { get; set; }
        public virtual ICollection<ProjectStage> ChildStages { get; set; } = new List<ProjectStage>();        
        public virtual ICollection<DivisionContractor> NavigationDivisionContractors { get; set; } = new List<DivisionContractor>();
        /// <summary>
        /// Ответственные лица
        /// </summary>
        public virtual ICollection<StageManager> StageManagers { get; set; } = new List<StageManager>();
        /// <summary>
        /// Отчёты
        /// </summary>
        public virtual ICollection<StageReport> StageReports { get; set; } = new List<StageReport>();
        /// <summary>
        /// Исполнители
        /// </summary>
        public virtual ICollection<Division> Contractors { get; set; } = new List<Division>();

        
        public double CurrentProgress
        {
            get
            {                
                double total = GetTotalStages();
                double completedTotal = GetTotalCompletedStages();
                return completedTotal / total;
            }
        }

        public virtual double GetTotalStages()
        {
            if (ChildStages.Count < 1) return 1;
            var total = 1d;
            foreach ( var stage in ChildStages)
            {
                total += stage.GetTotalStages();
            }
            return total;
        }

        public virtual double GetTotalCompletedStages()
        {
            if (ChildStages.Count < 1) return IsCompleted ? 1d : 0d;
            var total = IsCompleted ? 1d : 0d;
            foreach (var stage in ChildStages)
            {
                total += stage.GetTotalCompletedStages();
            }
            return total;
        }

    }
}
