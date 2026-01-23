using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DxBlazorApplication7.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalTimes",
                columns: table => new
                {
                    AddTimeDate = table.Column<DateTime>(type: "datetime2", maxLength: 30, nullable: false),
                    OPNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AddStartTime = table.Column<DateTime>(type: "datetime2", maxLength: 45, nullable: false),
                    AddEndTime = table.Column<DateTime>(type: "datetime2", maxLength: 45, nullable: false),
                    AddTimeType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AddTime = table.Column<double>(type: "float", maxLength: 30, nullable: false),
                    ReasonType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsApprove = table.Column<bool>(type: "bit", nullable: true),
                    IsDel = table.Column<bool>(type: "bit", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalTimes", x => new { x.AddTimeDate, x.OPNO, x.AddStartTime, x.AddEndTime });
                });

            migrationBuilder.CreateTable(
                name: "ComparisonTables",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", maxLength: 10, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StationName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComparisonTables", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ESOPStatuss",
                columns: table => new
                {
                    StationID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ProcedureID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ModelID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TypeGroup = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ESOPCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FileSite = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsDel = table.Column<bool>(type: "bit", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESOPStatuss", x => new { x.StationID, x.ProcedureID, x.ModelID });
                });

            migrationBuilder.CreateTable(
                name: "JointModuleTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Process = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsMultipleJobs = table.Column<bool>(type: "bit", nullable: false),
                    ProcessType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true),
                    IsFinal = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JointModuleTables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManufactureStatuss",
                columns: table => new
                {
                    ManufactureID = table.Column<int>(type: "int", maxLength: 30, nullable: false),
                    ManufactureName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManufactureStatuss", x => new { x.ManufactureID, x.ManufactureName });
                });

            migrationBuilder.CreateTable(
                name: "ModelStatuss",
                columns: table => new
                {
                    ModelKey = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ModelID = table.Column<int>(type: "int", maxLength: 30, nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsDel = table.Column<bool>(type: "bit", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelStatuss", x => new { x.ModelKey, x.ModelID });
                });

            migrationBuilder.CreateTable(
                name: "OP_Report_Days",
                columns: table => new
                {
                    ROWID = table.Column<int>(type: "int", nullable: false),
                    DayTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OP1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OP2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OP3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OP4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OP5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OP6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OP7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "OP_Reports",
                columns: table => new
                {
                    ROWID = table.Column<int>(type: "int", nullable: false),
                    WO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiveTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponseTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OP1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OP2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OP3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OP4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OP5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OP6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OP7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "OperatorInformations",
                columns: table => new
                {
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OperatorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OPNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CARDNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OperatorEnName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDel = table.Column<bool>(type: "bit", nullable: true),
                    IsManager = table.Column<bool>(type: "bit", nullable: true),
                    IsSPDGroup = table.Column<bool>(type: "bit", nullable: true),
                    Ability = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", maxLength: 30, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", maxLength: 30, nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatorInformations", x => new { x.Department, x.OperatorName, x.Mail, x.OPNO, x.CARDNO });
                });

            migrationBuilder.CreateTable(
                name: "OvertimeSchedules",
                columns: table => new
                {
                    OverTimeDate = table.Column<DateTime>(type: "datetime2", maxLength: 30, nullable: false),
                    OverTimeType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OPNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsDel = table.Column<bool>(type: "bit", nullable: true),
                    CreateUser = table.Column<DateTime>(type: "datetime2", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OvertimeSchedules", x => new { x.OverTimeDate, x.OverTimeType, x.OPNO });
                });

            migrationBuilder.CreateTable(
                name: "ProcedureStatuss",
                columns: table => new
                {
                    ProcedureKey = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ProcedureID = table.Column<int>(type: "int", maxLength: 30, nullable: false),
                    IndexKey = table.Column<int>(type: "int", maxLength: 10, nullable: true),
                    ProcedureName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    StationID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsDel = table.Column<bool>(type: "bit", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureStatuss", x => new { x.ProcedureKey, x.ProcedureID });
                });

            migrationBuilder.CreateTable(
                name: "ProcessTypeTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Sorting = table.Column<int>(type: "int", nullable: true),
                    StationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessTypeTables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuantityByWorkOrders",
                columns: table => new
                {
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ReasonTypes",
                columns: table => new
                {
                    ReasonCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ReasonName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReasonTypes", x => x.ReasonCode);
                });

            migrationBuilder.CreateTable(
                name: "SerialNumberByWorkOrders",
                columns: table => new
                {
                    OrderNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PP_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "StationStatuss",
                columns: table => new
                {
                    StationKey = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StationID = table.Column<int>(type: "int", maxLength: 30, nullable: false),
                    IndexKey = table.Column<int>(type: "int", maxLength: 10, nullable: true),
                    StationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Manufacturing = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsDel = table.Column<bool>(type: "bit", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationStatuss", x => new { x.StationKey, x.StationID });
                });

            migrationBuilder.CreateTable(
                name: "TimePeriods",
                columns: table => new
                {
                    PeriodType = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    PeriodName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDel = table.Column<bool>(type: "bit", nullable: true),
                    IsOT = table.Column<bool>(type: "bit", nullable: true),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimePeriods", x => new { x.PeriodType, x.PeriodName });
                });

            migrationBuilder.CreateTable(
                name: "TypeByWorkOrders",
                columns: table => new
                {
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "UsersDetails",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastLoginTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDetails", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "WorkingDatas",
                columns: table => new
                {
                    WorkingDate = table.Column<DateTime>(type: "datetime2", maxLength: 30, nullable: false),
                    WorkingType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WorkingID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OPNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WorkingDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    WorkingGroup = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    QTY = table.Column<int>(type: "int", maxLength: 100, nullable: true),
                    IsDel = table.Column<bool>(type: "bit", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", maxLength: 30, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", maxLength: 30, nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingDatas", x => new { x.WorkingDate, x.WorkingType, x.WorkingID, x.OPNO });
                });

            migrationBuilder.CreateTable(
                name: "WorkingLists",
                columns: table => new
                {
                    WorkingTime = table.Column<DateTime>(type: "datetime2", maxLength: 30, nullable: false),
                    WorkingID = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    WorkOrder = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SeriaNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OPNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WorkingPeriod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkingName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WorkingDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ProcessName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProcessDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PartNo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PartName = table.Column<string>(type: "nvarchar(1200)", maxLength: 1200, nullable: true),
                    WorkOrderCount = table.Column<double>(type: "float", nullable: true),
                    OPName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDel = table.Column<bool>(type: "bit", nullable: true),
                    AssignTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAssign = table.Column<bool>(type: "bit", nullable: true),
                    PauseTime = table.Column<DateTime>(type: "datetime2", maxLength: 30, nullable: true),
                    IsPause = table.Column<bool>(type: "bit", nullable: true),
                    ReceiveTime = table.Column<DateTime>(type: "datetime2", maxLength: 30, nullable: true),
                    IsReceive = table.Column<bool>(type: "bit", nullable: true),
                    ResponseTime = table.Column<DateTime>(type: "datetime2", maxLength: 30, nullable: true),
                    IsResponse = table.Column<bool>(type: "bit", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingLists", x => new { x.WorkingTime, x.WorkingID, x.WorkOrder, x.SeriaNumber, x.OPNO });
                });

            migrationBuilder.CreateTable(
                name: "WorkingOrderLists",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkingTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    StationName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkOrder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkOrderCount = table.Column<double>(type: "float", maxLength: 50, nullable: true),
                    OPNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ReceiveTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsReceive = table.Column<bool>(type: "bit", maxLength: 50, nullable: true),
                    PauseTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsPause = table.Column<bool>(type: "bit", maxLength: 50, nullable: true),
                    ResponseTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsResponse = table.Column<bool>(type: "bit", maxLength: 50, nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PassCount = table.Column<double>(type: "float", maxLength: 50, nullable: true),
                    FailCount = table.Column<double>(type: "float", maxLength: 50, nullable: true),
                    ProcessType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingOrderLists", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WorkingOrderStepLists",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkingTime = table.Column<DateTime>(type: "datetime2", maxLength: 50, nullable: true),
                    WOID = table.Column<int>(type: "int", maxLength: 50, nullable: true),
                    StationName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkOrder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OPNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StatusTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ReasonType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingOrderStepLists", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WorkingStepTimeLists",
                columns: table => new
                {
                    WorkingTime = table.Column<DateTime>(type: "datetime2", maxLength: 30, nullable: false),
                    WorkOrder = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SeriaNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OPNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusTime = table.Column<DateTime>(type: "datetime2", maxLength: 30, nullable: false),
                    WorkingName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WorkingDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ProcessName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProcessDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    OPName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReasonType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsDel = table.Column<bool>(type: "bit", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingStepTimeLists", x => new { x.WorkingTime, x.WorkOrder, x.SeriaNumber, x.OPNO, x.StatusTime });
                });

            migrationBuilder.CreateTable(
                name: "WorkingTimeDetails",
                columns: table => new
                {
                    Index = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkOrder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SeriaNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OPNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReceiveTime = table.Column<DateTime>(type: "datetime2", maxLength: 50, nullable: false),
                    ResponseTime = table.Column<DateTime>(type: "datetime2", maxLength: 50, nullable: false),
                    WorkTime = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingTimeDetails", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "WorkingTypeGroups",
                columns: table => new
                {
                    TypeGroup = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TypeGroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ManufacturingProcess = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDel = table.Column<bool>(type: "bit", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingTypeGroups", x => new { x.TypeGroup, x.TypeGroupName });
                });

            migrationBuilder.CreateTable(
                name: "WorkingTypes",
                columns: table => new
                {
                    TypeID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TypeGroup = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IsDel = table.Column<bool>(type: "bit", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingTypes", x => new { x.TypeID, x.TypeName });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalTimes");

            migrationBuilder.DropTable(
                name: "ComparisonTables");

            migrationBuilder.DropTable(
                name: "ESOPStatuss");

            migrationBuilder.DropTable(
                name: "JointModuleTables");

            migrationBuilder.DropTable(
                name: "ManufactureStatuss");

            migrationBuilder.DropTable(
                name: "ModelStatuss");

            migrationBuilder.DropTable(
                name: "OP_Report_Days");

            migrationBuilder.DropTable(
                name: "OP_Reports");

            migrationBuilder.DropTable(
                name: "OperatorInformations");

            migrationBuilder.DropTable(
                name: "OvertimeSchedules");

            migrationBuilder.DropTable(
                name: "ProcedureStatuss");

            migrationBuilder.DropTable(
                name: "ProcessTypeTables");

            migrationBuilder.DropTable(
                name: "QuantityByWorkOrders");

            migrationBuilder.DropTable(
                name: "ReasonTypes");

            migrationBuilder.DropTable(
                name: "SerialNumberByWorkOrders");

            migrationBuilder.DropTable(
                name: "StationStatuss");

            migrationBuilder.DropTable(
                name: "TimePeriods");

            migrationBuilder.DropTable(
                name: "TypeByWorkOrders");

            migrationBuilder.DropTable(
                name: "UsersDetails");

            migrationBuilder.DropTable(
                name: "WorkingDatas");

            migrationBuilder.DropTable(
                name: "WorkingLists");

            migrationBuilder.DropTable(
                name: "WorkingOrderLists");

            migrationBuilder.DropTable(
                name: "WorkingOrderStepLists");

            migrationBuilder.DropTable(
                name: "WorkingStepTimeLists");

            migrationBuilder.DropTable(
                name: "WorkingTimeDetails");

            migrationBuilder.DropTable(
                name: "WorkingTypeGroups");

            migrationBuilder.DropTable(
                name: "WorkingTypes");
        }
    }
}
