using DxBlazorApplication7.Data;
using DxBlazorApplication7.Pages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using CommonLibrary.AuthPack;

namespace DxBlazorApplication7.Data
{
    public partial class DSDBContext : DbContext
    {
        public DSDBContext()
        {
        }

        public DSDBContext(DbContextOptions<DSDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<OperatorInformation> OperatorInformations { get; set; }
        public virtual DbSet<TimePeriod> TimePeriods { get; set; }
        public virtual DbSet<WorkingData> WorkingDatas { get; set; }
        public virtual DbSet<OvertimeSchedule> OvertimeSchedules { get; set; }
        public virtual DbSet<WorkingList> WorkingLists { get; set; }
        public virtual DbSet<WorkingType> WorkingTypes { get; set; }
        public virtual DbSet<WorkingTypeGroup> WorkingTypeGroups { get; set; }
        public virtual DbSet<WorkingTimeDetail> WorkingTimeDetails { get; set; }
        public virtual DbSet<WorkingStepTimeList> WorkingStepTimeLists { get; set; }
        public virtual DbSet<WorkingHoursStatistic> WorkingHoursStatistics { get; set; }
        public virtual DbSet<OP_Report> OP_Reports { get; set; }
        public virtual DbSet<OP_Report_Day> OP_Report_Days { get; set; }
        public virtual DbSet<WO_SN_Report> WO_SN_Reports { get; set; }
        public virtual DbSet<SerialNumberByWorkOrder> SerialNumberByWorkOrders { get; set; }
        public virtual DbSet<ManufacturingProcessName> ManufacturingProcessNames { get; set; }
        public virtual DbSet<ReasonType> ReasonTypes { get; set; }
        public virtual DbSet<AdditionalTime> AdditionalTimes { get; set; }
        public virtual DbSet<StationStatus> StationStatuss { get; set; }
        public virtual DbSet<ProcedureStatus> ProcedureStatuss { get; set; }
        public virtual DbSet<ModelStatus> ModelStatuss { get; set; }
        public virtual DbSet<ESOPStatus> ESOPStatuss { get; set; }
        public virtual DbSet<ManufactureStatus> ManufactureStatuss { get; set; }
        public virtual DbSet<WorkingOrderList> WorkingOrderLists { get; set; }
        public virtual DbSet<WorkingOrderStepList> WorkingOrderStepLists { get; set; }
        public virtual DbSet<UserDetails> UsersDetails { get; set; }

        public virtual DbSet<ComparisonTable> ComparisonTables { get; set; }

        public virtual DbSet<JointModuleTables> JointModuleTables { get; set; }

        public virtual DbSet<QuantityByWorkOrder_ERP> QuantityByWorkOrders { get; set; }

        public virtual DbSet<TypeByWorkOrder> TypeByWorkOrders { get; set; }

        public virtual DbSet<ProcessTypeTable> ProcessTypeTables { get; set; }

        public virtual DbSet<DirectoryReferenceTable> DirectoryReferenceTables { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<OperatorInformation>(entity => {

                entity.HasKey(e => new { e.Department, e.OperatorName, e.Mail, e.OPNO, e.CARDNO });

                entity.Property(e => e.Department).HasMaxLength(50);

                entity.Property(e => e.OperatorName).HasMaxLength(50);

                entity.Property(e => e.OperatorEnName).HasMaxLength(50);

                entity.Property(e => e.Mail).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.OPNO).HasMaxLength(50);

                entity.Property(e => e.CARDNO).HasMaxLength(50);

                entity.Property(e => e.IsDel);

                entity.Property(e => e.IsManager);

                entity.Property(e => e.IsSPDGroup);

                entity.Property(e => e.Ability).HasMaxLength(200);

                entity.Property(e => e.CreateTime).HasMaxLength(30);

                entity.Property(e => e.UpdateTime).HasMaxLength(30);

                entity.Property(e => e.CreateUser).HasMaxLength(50);
            });

            modelBuilder.Entity<TimePeriod>(entity => {

                entity.HasKey(e => new { e.PeriodType, e.PeriodName });

                entity.Property(e => e.PeriodType).HasMaxLength(2);

                entity.Property(e => e.IsDel);

                entity.Property(e => e.PeriodName).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.CreateUser).HasMaxLength(50);
            });

            modelBuilder.Entity<OvertimeSchedule>(entity => {

                entity.HasKey(e => new { e.OverTimeDate, e.OverTimeType, e.OPNO });

                entity.Property(e => e.OverTimeDate).HasMaxLength(30);

                entity.Property(e => e.OverTimeType).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.OPNO).HasMaxLength(50);

                entity.Property(e => e.IsDel);

                entity.Property(e => e.CreateUser).HasMaxLength(50);
            });

            modelBuilder.Entity<WorkingData>(entity => {

                entity.HasKey(e => new { e.WorkingDate, e.WorkingType, e.WorkingID, e.OPNO });

                entity.Property(e => e.WorkingDate).HasMaxLength(30);

                entity.Property(e => e.WorkingType).HasMaxLength(10);

                entity.Property(e => e.WorkingID).HasMaxLength(10);

                entity.Property(e => e.WorkingDescription).HasMaxLength(250);

                entity.Property(e => e.WorkingGroup).HasMaxLength(10);

                entity.Property(e => e.OPNO).HasMaxLength(50);

                entity.Property(e => e.QTY).HasMaxLength(100);

                entity.Property(e => e.IsDel);

                entity.Property(e => e.CreateTime).HasMaxLength(30);

                entity.Property(e => e.UpdateTime).HasMaxLength(30);

                entity.Property(e => e.CreateUser).HasMaxLength(50);
            });

            modelBuilder.Entity<WorkingList>(entity => {

                entity.HasKey(e => new { e.WorkingTime, e.WorkingID, e.WorkOrder, e.SeriaNumber, e.OPNO});

                entity.Property(e => e.WorkingTime).HasMaxLength(30);

                entity.Property(e => e.WorkingID).HasMaxLength(10);

                entity.Property(e => e.WorkingPeriod).HasMaxLength(50);

                entity.Property(e => e.WorkingName).HasMaxLength(100);

                entity.Property(e => e.WorkingDescription).HasMaxLength(300);

                entity.Property(e => e.ProcessName).HasMaxLength(100);

                entity.Property(e => e.ProcessDescription).HasMaxLength(300);

                entity.Property(e => e.WorkOrder).HasMaxLength(100);

                entity.Property(e => e.PartNo).HasMaxLength(300);

                entity.Property(e => e.PartName).HasMaxLength(1200);

                entity.Property(e => e.SeriaNumber).HasMaxLength(100);

                entity.Property(e => e.IsDel);

                entity.Property(e => e.OPNO).HasMaxLength(50);

                entity.Property(e => e.OPName).HasMaxLength(50);

                entity.Property(e => e.AssignTime);

                entity.Property(e => e.IsAssign);

                entity.Property(e => e.ReceiveTime).HasMaxLength(30);

                entity.Property(e => e.IsReceive);

                entity.Property(e => e.PauseTime).HasMaxLength(30);

                entity.Property(e => e.IsPause);

                entity.Property(e => e.ResponseTime).HasMaxLength(30);

                entity.Property(e => e.IsResponse);

                entity.Property(e => e.CreateUser).HasMaxLength(50);
            });

            modelBuilder.Entity<WorkingStepTimeList>(entity => {

                entity.HasKey(e => new { e.WorkingTime, e.WorkOrder, e.SeriaNumber, e.OPNO, e.StatusTime });

                entity.Property(e => e.WorkingTime).HasMaxLength(30);

                entity.Property(e => e.WorkingName).HasMaxLength(100);

                entity.Property(e => e.WorkingDescription).HasMaxLength(300);

                entity.Property(e => e.ProcessName).HasMaxLength(100);

                entity.Property(e => e.ProcessDescription).HasMaxLength(300);

                entity.Property(e => e.WorkOrder).HasMaxLength(100);

                entity.Property(e => e.SeriaNumber).HasMaxLength(100);

                entity.Property(e => e.OPNO).HasMaxLength(50);

                entity.Property(e => e.OPName).HasMaxLength(50);

                entity.Property(e => e.WorkingStatus).HasMaxLength(50);

                entity.Property(e => e.StatusTime).HasMaxLength(30);

                entity.Property(e => e.ReasonType).HasMaxLength(30);

                entity.Property(e => e.IsDel);

                entity.Property(e => e.CreateUser).HasMaxLength(50);
            });

            modelBuilder.Entity<WorkingType>(entity => {

                entity.HasKey(e => new { e.TypeID, e.TypeName });

                entity.Property(e => e.TypeID).HasMaxLength(10);

                entity.Property(e => e.TypeName).HasMaxLength(100);

                entity.Property(e => e.TypeGroup).HasMaxLength(10);

                entity.Property(e => e.IsDel);

                entity.Property(e => e.CreateUser).HasMaxLength(50);
            });

            modelBuilder.Entity<WorkingTypeGroup>(entity => {

                entity.HasKey(e => new { e.TypeGroup, e.TypeGroupName });

                entity.Property(e => e.TypeGroup).HasMaxLength(10);

                entity.Property(e => e.TypeGroupName).HasMaxLength(100);

                entity.Property(e => e.ManufacturingProcess).HasMaxLength(100);

                entity.Property(e => e.IsDel);

                entity.Property(e => e.CreateUser).HasMaxLength(50);
            });

            modelBuilder.Entity<OP_Report>(entity =>
            {

                entity.HasNoKey();

                entity.ToFunction("sp_OP_Report");

                entity.Property(e => e.ROWID).HasColumnName("ROWID");

                entity.Property(e => e.WO).HasColumnName("WO");

                entity.Property(e => e.SN).HasColumnName("SN");

                entity.Property(e => e.ReceiveTime).HasColumnName("ReceiveTime");

                entity.Property(e => e.ResponseTime).HasColumnName("ResponseTime");

                entity.Property(e => e.OP1).HasColumnName("OP1");

                entity.Property(e => e.OP2).HasColumnName("OP2");

                entity.Property(e => e.OP3).HasColumnName("OP3");

                entity.Property(e => e.OP4).HasColumnName("OP4");

                entity.Property(e => e.OP5).HasColumnName("OP5");

                entity.Property(e => e.OP6).HasColumnName("OP6");

                entity.Property(e => e.OP7).HasColumnName("OP7");

                entity.Property(e => e.TotalTimes).HasColumnName("Total");
            });

            modelBuilder.Entity<OP_Report_Day>(entity =>
            {

                entity.HasNoKey();

                entity.ToFunction("sp_OP_ReportByDay");

                entity.Property(e => e.ROWID).HasColumnName("ROWID");

                entity.Property(e => e.DayTime).HasColumnName("DayTime");

                entity.Property(e => e.OP1).HasColumnName("OP1");

                entity.Property(e => e.OP2).HasColumnName("OP2");

                entity.Property(e => e.OP3).HasColumnName("OP3");

                entity.Property(e => e.OP4).HasColumnName("OP4");

                entity.Property(e => e.OP5).HasColumnName("OP5");

                entity.Property(e => e.OP6).HasColumnName("OP6");

                entity.Property(e => e.OP7).HasColumnName("OP7");

                entity.Property(e => e.TotalTimes).HasColumnName("Total");
            });

            modelBuilder.Entity<SerialNumberByWorkOrder>(entity =>
            {

                entity.HasNoKey();

                entity.ToFunction("sp_SerialNumberByWorkOrder");

                entity.Property(e => e.OrderNo).HasColumnName("OrderNo");

                entity.Property(e => e.SerialNO).HasColumnName("SerialNO");

                entity.Property(e => e.LineName).HasColumnName("LineName");

                entity.Property(e => e.PP_Name).HasColumnName("PP_Name");

                entity.Property(e => e.PartNo).HasColumnName("PartNo");

                entity.Property(e => e.PartName).HasColumnName("PartName");

                entity.Property(e => e.Quantity).HasColumnName("Quantity");

                entity.Property(e => e.StartTime).HasColumnName("StartTime");
            });

            modelBuilder.Entity<ManufacturingProcessName>(entity =>
            {

                entity.HasNoKey();

                entity.ToView("ManufacturingProcessNameView");

                entity.Property(e => e.ProcessName).HasColumnName("ManufacturingProcessName");

                //entity.Property(e => e.ProcessCName).HasColumnName("ManufacturingProcessCName");
            });

            modelBuilder.Entity<WorkingHoursStatistic>(entity =>
            {

                entity.HasNoKey();

                entity.ToView("OperatingWorkingStatisticView");

                entity.Property(e => e.WorkOrder).HasColumnName("WorkOrder");

                entity.Property(e => e.SeriaNumber).HasColumnName("SeriaNumber");

                entity.Property(e => e.OPNO).HasColumnName("OPNO");

                entity.Property(e => e.ReceiveTime).HasColumnName("ReceiveTime");

                entity.Property(e => e.ResponseTime).HasColumnName("ResponseTime");

                entity.Property(e => e.DiffDay).HasColumnName("DiffDay");

                entity.Property(e => e.DiffTime).HasColumnName("DiffTime");
            });

            modelBuilder.Entity<WorkingTimeDetail>(entity => {

                entity.HasKey(e => new { e.Index});

                entity.Property(e => e.Index).HasMaxLength(50);

                entity.Property(e => e.WorkOrder).HasMaxLength(50);

                entity.Property(e => e.SeriaNumber).HasMaxLength(50);

                entity.Property(e => e.OPNO).HasMaxLength(50);

                entity.Property(e => e.ReceiveTime).HasMaxLength(50);

                entity.Property(e => e.ResponseTime).HasMaxLength(50);

                entity.Property(e => e.WorkTime).HasMaxLength(50);
            });

            modelBuilder.Entity<WO_SN_Report>(entity =>
            {

                entity.HasNoKey();

                entity.ToView("WorkOrderStatisticView");

                entity.Property(e => e.WO).HasColumnName("WO");

                entity.Property(e => e.SN).HasColumnName("SN");

                entity.Property(e => e.FirstReceiveTime).HasColumnName("FirstReceiveTime");

                entity.Property(e => e.LastResponseTime).HasColumnName("LastResponseTime");

                entity.Property(e => e.WorkTime).HasColumnName("WorkTime");

                entity.Property(e => e.MaxWorkTime).HasColumnName("MaxWorkTime");

                entity.Property(e => e.MinWorkTime).HasColumnName("MinWorkTime");

                entity.Property(e => e.AvgWorkTime).HasColumnName("AvgWorkTime");

                entity.Property(e => e.OPGroup).HasColumnName("OPGroup");

                entity.Property(e => e.WorkCount).HasColumnName("WorkCount");

            });

            modelBuilder.Entity<ReasonType>(entity => {

                entity.HasKey(e => new { e.ReasonCode });

                entity.Property(e => e.ReasonCode).HasMaxLength(30);

                entity.Property(e => e.ReasonName).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(300);

            });

            modelBuilder.Entity<AdditionalTime>(entity => {

                entity.HasKey(e => new { e.AddTimeDate, e.OPNO, e.AddStartTime, e.AddEndTime });

                entity.Property(e => e.AddTimeDate).HasMaxLength(30);

                entity.Property(e => e.AddTimeType).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.OPNO).HasMaxLength(50);

                entity.Property(e => e.AddStartTime).HasMaxLength(45);

                entity.Property(e => e.AddEndTime).HasMaxLength(45);

                entity.Property(e => e.AddTime).HasMaxLength(30);

                entity.Property(e => e.ReasonType).HasMaxLength(30);

                entity.Property(e => e.IsApprove);

                entity.Property(e => e.IsDel);

                entity.Property(e => e.CreateUser).HasMaxLength(50);
            });

            modelBuilder.Entity<StationStatus>(entity => {

                entity.HasKey(e => new { e.StationKey, e.StationID });

                entity.Property(e => e.IndexKey).HasMaxLength(10);

                entity.Property(e => e.StationKey).HasMaxLength(10);

                entity.Property(e => e.StationID).HasMaxLength(30);

                entity.Property(e => e.StationName).HasMaxLength(100);

                entity.Property(e => e.Manufacturing).HasMaxLength(30);

                entity.Property(e => e.IsDel);

                entity.Property(e => e.CreateUser).HasMaxLength(50);
            });

            modelBuilder.Entity<ProcedureStatus>(entity => {

                entity.HasKey(e => new { e.ProcedureKey, e.ProcedureID });

                entity.Property(e => e.IndexKey).HasMaxLength(10);

                entity.Property(e => e.ProcedureKey).HasMaxLength(10);

                entity.Property(e => e.ProcedureID).HasMaxLength(30);

                entity.Property(e => e.ProcedureName).HasMaxLength(300);

                entity.Property(e => e.StationID).HasMaxLength(30);

                entity.Property(e => e.IsDel);

                entity.Property(e => e.CreateUser).HasMaxLength(50);
            });

            modelBuilder.Entity<ModelStatus>(entity => {

                entity.HasKey(e => new { e.ModelKey, e.ModelID });

                entity.Property(e => e.ModelKey).HasMaxLength(10);

                entity.Property(e => e.ModelID).HasMaxLength(30);

                entity.Property(e => e.ModelName).HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(30);

                entity.Property(e => e.IsDel);

                entity.Property(e => e.CreateUser).HasMaxLength(50);
            });

            modelBuilder.Entity<ESOPStatus>(entity => {

                entity.HasKey(e => new { e.StationID, e.ProcedureID, e.ModelID });

                entity.Property(e => e.StationID).HasMaxLength(30);

                entity.Property(e => e.ProcedureID).HasMaxLength(30);

                entity.Property(e => e.ModelID).HasMaxLength(30);

                entity.Property(e => e.TypeGroup).HasMaxLength(30);

                entity.Property(e => e.ESOPCode).HasMaxLength(100);

                entity.Property(e => e.FileSite).HasMaxLength(300);

                entity.Property(e => e.IsDel);

                entity.Property(e => e.CreateUser).HasMaxLength(50);
            });

            modelBuilder.Entity<ManufactureStatus>(entity => {

                entity.HasKey(e => new { e.ManufactureID, e.ManufactureName});

                entity.Property(e => e.ManufactureID).HasMaxLength(30);

                entity.Property(e => e.ManufactureName).HasMaxLength(30);
            });

            modelBuilder.Entity<UserDetails>(entity =>
            {
                //entity.ToTable("UserDetailTable");
                entity.HasKey(e => e.UserName);

                //.ToTable("UserDetail");

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.Property(e => e.Department).HasMaxLength(50);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.LastLoginTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<WorkingOrderList>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.Property(e => e.WorkingTime).HasColumnType("datetime");

                entity.Property(e => e.ID).HasMaxLength(50);

                entity.Property(e => e.StationName).HasMaxLength(50);

                entity.Property(e => e.WorkOrder).HasMaxLength(50);

                entity.Property(e => e.SerialNumber).HasMaxLength(50);

                entity.Property(e => e.WorkOrderCount).HasMaxLength(50);

                entity.Property(e => e.OPNO).HasMaxLength(50);

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.ReceiveTime).HasColumnType("datetime");

                entity.Property(e => e.IsReceive).HasMaxLength(50);

                entity.Property(e => e.PauseTime).HasColumnType("datetime");

                entity.Property(e => e.IsPause).HasMaxLength(50);

                entity.Property(e => e.ResponseTime).HasColumnType("datetime");

                entity.Property(e => e.IsResponse).HasMaxLength(50);

                entity.Property(e => e.CreateUser).HasMaxLength(50);

                entity.Property(e => e.PassCount).HasMaxLength(50);

                entity.Property(e => e.FailCount).HasMaxLength(50);

                entity.Property(e => e.ProcessType).HasMaxLength(50);

            });

            modelBuilder.Entity<WorkingOrderStepList>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.Property(e => e.WorkingTime).HasMaxLength(50);

                entity.Property(e => e.ID).HasMaxLength(50);

                entity.Property(e => e.WOID).HasMaxLength(50);

                entity.Property(e => e.StationName).HasMaxLength(50);

                entity.Property(e => e.WorkOrder).HasMaxLength(50);

                entity.Property(e => e.SerialNumber).HasMaxLength(50);

                entity.Property(e => e.OPNO).HasMaxLength(50);

                entity.Property(e => e.WorkingStatus).HasMaxLength(50);

                entity.Property(e => e.StatusTime).HasColumnType("datetime");

                entity.Property(e => e.ReasonType).HasMaxLength(50);

                entity.Property(e => e.CreateUser).HasMaxLength(50);
            });

            modelBuilder.Entity<ComparisonTable>(entity => {

                entity.HasKey(e => new { e.ID });

                entity.Property(e => e.ID).HasMaxLength(10);

                entity.Property(e => e.ProcessName).HasMaxLength(100);

                entity.Property(e => e.StationName).HasMaxLength(10);
            });

            modelBuilder.Entity<JointModuleTables>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Process).HasMaxLength(100);

                entity.Property(e => e.ProcessType).HasMaxLength(200);

                entity.Property(e => e.StationName).HasMaxLength(100);

                entity.Property(e => e.StationNumber).HasMaxLength(100);
            });

            modelBuilder.Entity<QuantityByWorkOrder_ERP>(entity =>
            {

                entity.HasNoKey();

                entity.ToFunction("sp_QuantityByWorkOrder");

                entity.Property(e => e.Quantity).HasColumnName("Quantity");
            });

            modelBuilder.Entity<TypeByWorkOrder>(entity =>
            {
                entity.HasNoKey();

                entity.ToFunction("sp_TypeByWorkOrder");

                entity.Property(e => e.ProcessType).HasColumnName("ModelName");
            });

            modelBuilder.Entity<ProcessTypeTable>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ProcessType).HasMaxLength(100);

                entity.Property(e => e.Sorting);

                entity.Property(e => e.StationName).HasMaxLength(100);
            });

            modelBuilder.Entity<DirectoryReferenceTable>(entity =>
            {
                entity.HasKey(e => e.DirectoryIndex);

                entity.Property(e => e.DirectoryName).HasMaxLength(100);

                entity.Property(e => e.DisplayName).HasMaxLength(100); ;
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
