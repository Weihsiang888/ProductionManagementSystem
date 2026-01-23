using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using CommonLibrary.AuthPack;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Schedule;
//using DevExpress.XtraPrinting.Native;
using DxBlazorApplication7.Data;
using DxBlazorApplication7.Services;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using NuGet.ContentModel;
using NuGet.Protocol;
using static NuGet.Packaging.PackagingConstants;
//using static DevExpress.XtraPrinting.Export.Pdf.PdfImageCache;


namespace DxBlazorApplication7.Data
{
    public partial class DataService
    {
        private readonly IServiceProvider services;
        //readonly DSDBContext _DSDBContext = new();

        public static List<DirectoryStatus> _DirectoryItem = new List<DirectoryStatus>();
        public static List<FolderStatus> _FolderItem = new List<FolderStatus>();

        DataLogService _DataLogService = new DataLogService();
        //public DataService(IServiceProvider services, DataLogService dataLogService)
        public DataService(IServiceProvider services)
        {
            this.services = services;
            _DataLogService.WriteEventLog("Code", "Service Start");
            //GetESOPDirectoryStatusAsync();
            //var scope = services.CreateScope();
            //_DataLogService = dataLogService;
            //_DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
        }

        private List<OperatorInformation> _OperatorInformations = new List<OperatorInformation>();
        private List<UserDetails> _UserDetails = new List<UserDetails>();
        public Task<List<OperatorInformation>> GetOperatorInformationsAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _OperatorInformations = _DSDBContext.OperatorInformations.Where(x => x.IsDel == false).ToList();
                return Task.FromResult(_OperatorInformations);
            }
        }
        public Task<List<UserDetails>> GetUserDetailsAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _UserDetails = _DSDBContext.UsersDetails.ToList();
                return Task.FromResult(_UserDetails);
            }
        }
        public Task<List<OperatorInformation>> ChangeOperatorInformationsAsync(OperatorInformation changed)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _OperatorInformations.FirstOrDefault(i => i.OperatorName == changed.OperatorName && i.OPNO == changed.OPNO);
                if (orginalForecast != null)
                {
                    orginalForecast.Department = changed.Department;
                    orginalForecast.OperatorName = changed.OperatorName;
                    //orginalForecast.OperatorEnName = changed.OperatorEnName;
                    //orginalForecast.Mail = changed.Mail;
                    //orginalForecast.Phone = changed.Phone;
                    orginalForecast.OPNO = changed.OPNO;
                    //orginalForecast.IsSPDGroup = changed.IsSPDGroup;
                    orginalForecast.UpdateTime = changed.UpdateTime;
                    _DSDBContext.OperatorInformations.Update(orginalForecast);
                }
                else
                {
                    UserDetails item = new UserDetails();
                    item.UserName = changed.OPNO;
                    item.Department = changed.Department;
                    item.Role = 1;
                    item.HashedPassword = $"10000.SHA1.oqLSOBNNq6wq3MciTlWSIg==.jO3DaBVxAT9LYA3QGnCQxOd8mZkE13hal6O2qWSboRlJ/jTjiMpy5ZVU5ElF6e6e3vfvGP5G5P6kXDyQmLdtng==";
                    item.CreateTime = DateTime.Now;
                    item.LastLoginTime = DateTime.Now;
                    item.Token = "";
                    _DSDBContext.UsersDetails.Add(item);
                    _DSDBContext.OperatorInformations.Add(changed);
                    _OperatorInformations.Add(changed);
                }
                _DSDBContext.SaveChanges();
                return Task.FromResult(_OperatorInformations);
            }
        }
        public Task<List<OperatorInformation>> Remove(OperatorInformation forecast)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var OperatorInformationValue = _OperatorInformations.FirstOrDefault(i => i.OperatorName == forecast.OperatorName && i.OPNO == forecast.OPNO);
                _UserDetails = _DSDBContext.UsersDetails.ToList();
                var UsersDetailValue = _UserDetails.FirstOrDefault(i => i.UserName == forecast.OperatorName);
                //if (orginalForecast != null)
                //{
                //    orginalForecast.Department = forecast.Department;
                //    orginalForecast.OperatorName = forecast.OperatorName;
                //    orginalForecast.OperatorEnName = forecast.OperatorEnName;
                //    orginalForecast.Mail = forecast.Mail;
                //    orginalForecast.Phone = forecast.Phone;
                //    orginalForecast.OPNO = forecast.OPNO;
                //    orginalForecast.IsDel = true;
                //    orginalForecast.IsSPDGroup = forecast.IsSPDGroup;
                //    orginalForecast.UpdateTime = forecast.UpdateTime;
                //    _DSDBContext.OperatorInformations.Update(orginalForecast);
                //}
                if (OperatorInformationValue != null)
                {
                    _OperatorInformations.Remove(forecast);
                    _DSDBContext.OperatorInformations.Remove(forecast);
                }
                if (UsersDetailValue != null)
                {
                    _UserDetails.Remove(UsersDetailValue);
                    _DSDBContext.UsersDetails.Remove(UsersDetailValue);
                }
                _DSDBContext.SaveChanges();
                return Task.FromResult(_OperatorInformations);
            }
        }

        private List<Department> _Departments = new List<Department>();
        public Task<List<Department>> GetDepartmentsAsync()
        {
            using (var scope = services.CreateScope())
            {
                _Departments.Clear();

                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();

                foreach (var item in _DSDBContext.UsersDetails.Where(x => x.UserName != "admin" && x.UserName != "Supervisor").Select(x => x.Department).Distinct().ToList())
                {
                    _Departments.Add(new Department
                    {
                        DepartmentName = item
                    });
                }

                //foreach (var item in _DSDBContext.OperatorInformations.Where(x => x.IsDel == false).Select(x => x.Department).Distinct().ToList())
                //{
                //    _Departments.Add(new Department
                //    {
                //        DepartmentName = item
                //    });
                //}
                return Task.FromResult(_Departments);
            }
        }

        private List<TimePeriod> _TimePeriods = new List<TimePeriod>();
        public Task<List<TimePeriod>> GetTimePeriodsAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _TimePeriods = _DSDBContext.TimePeriods.Where(x => x.IsDel == false).ToList();
                return Task.FromResult(_TimePeriods);
            }
        }
        public Task<List<TimePeriod>> ChangeTimePeriodsAsync(TimePeriod changed)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _TimePeriods.FirstOrDefault(i => i.PeriodType == changed.PeriodType && i.PeriodName == changed.PeriodName);
                if (orginalForecast != null)
                {
                    orginalForecast.PeriodType = changed.PeriodType;
                    orginalForecast.PeriodName = changed.PeriodName;
                    orginalForecast.Description = changed.Description;
                    _DSDBContext.TimePeriods.Update(orginalForecast);
                }
                else
                {
                    _DSDBContext.TimePeriods.Add(changed);
                    _TimePeriods.Add(changed);
                }
                _DSDBContext.SaveChanges();
                return Task.FromResult(_TimePeriods);
            }
        }
        public Task<List<TimePeriod>> Remove(TimePeriod forecast)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _TimePeriods.FirstOrDefault(i => i.PeriodType == forecast.PeriodType && i.PeriodName == forecast.PeriodName);
                if (orginalForecast != null)
                {
                    orginalForecast.PeriodType = forecast.PeriodType;
                    orginalForecast.IsDel = true;
                    orginalForecast.PeriodName = forecast.PeriodName;
                    orginalForecast.Description = forecast.Description;
                    _DSDBContext.TimePeriods.Update(orginalForecast);
                }
                _TimePeriods.Remove(forecast);
                _DSDBContext.SaveChanges();
                return Task.FromResult(_TimePeriods);
            }
        }

        private List<TimeStep> _TimeSteps = new List<TimeStep>();
        public Task<List<TimeStep>> GetTimeStepsAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                if (_TimeSteps.Count != 0)
                {
                    _TimeSteps.Clear();
                }
                foreach (var item in _DSDBContext.TimePeriods.Where(x => x.IsDel == false).ToList())
                {
                    _TimeSteps.Add(new TimeStep
                    {
                        TimeType = item.PeriodType.ToString(),
                        IsSelect = false,
                        Description = item.Description
                    });
                }
                return Task.FromResult(_TimeSteps);
            }
        }

        private List<TimeDay> _TimeDays = new List<TimeDay>();
        public Task<List<TimeDay>> GetTimeDaysAsync()
        {
            if (_TimeDays.Count != 0)
            {
                _TimeDays.Clear();
            }

            for (int day = 0; day < 7; day++)
            {
                string _Day = DateTime.Now.AddDays(day).ToString("yyyy-MM-dd");
                _TimeDays.Add(new TimeDay
                {
                    TimeDayType = day.ToString(),
                    IsSelect = false,
                    Description = _Day
                });
            }
            return Task.FromResult(_TimeDays);
        }

        public List<OvertimeSchedule> _OvertimeSchedules = new List<OvertimeSchedule>();
        public Task<List<OvertimeSchedule>> GetOvertimeSchedulesAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _OvertimeSchedules = _DSDBContext.OvertimeSchedules.Where(x => x.IsDel == false).ToList();
                return Task.FromResult(_OvertimeSchedules);
            }
        }
        public Task<List<OvertimeSchedule>> ChangeOvertimeSchedulesAsync(OvertimeSchedule changed)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _OvertimeSchedules.FirstOrDefault(i => i.OverTimeDate == changed.OverTimeDate && i.OverTimeType == changed.OverTimeType && i.OPNO == changed.OPNO);
                if (orginalForecast != null)
                {
                    orginalForecast.OverTimeDate = changed.OverTimeDate;
                    orginalForecast.OverTimeType = changed.OverTimeType;
                    orginalForecast.OPNO = changed.OPNO;
                    _DSDBContext.OvertimeSchedules.Update(orginalForecast);
                }
                _DSDBContext.SaveChanges();
                return Task.FromResult(_OvertimeSchedules);
            }
        }
        public Task<List<OvertimeSchedule>> ChangeOvertimeSchedulesAsync(List<OvertimeSchedule> changed)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                foreach (OvertimeSchedule item in changed)
                {
                    var orginalForecast = _OvertimeSchedules.FirstOrDefault(i => i.OverTimeDate == item.OverTimeDate && i.OverTimeType == item.OverTimeType && i.OPNO == item.OPNO);
                    if (orginalForecast == null)
                    {
                        _DSDBContext.OvertimeSchedules.Add(item);
                        _OvertimeSchedules.Add(item);
                    }
                }
                _DSDBContext.SaveChanges();
                return Task.FromResult(_OvertimeSchedules);
            }
        }

        public Task<List<OvertimeSchedule>> Remove(OvertimeSchedule forecast)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _OvertimeSchedules.FirstOrDefault(i => i.OverTimeType == forecast.OverTimeType && i.OPNO == forecast.OPNO);
                if (orginalForecast != null)
                {
                    orginalForecast.OverTimeType = forecast.OverTimeType;
                    orginalForecast.IsDel = true;
                    orginalForecast.OPNO = forecast.OPNO;
                    _DSDBContext.OvertimeSchedules.Update(orginalForecast);
                }
                _OvertimeSchedules.Remove(forecast);
                _DSDBContext.SaveChanges();
                return Task.FromResult(_OvertimeSchedules);
            }
        }

        private List<WorkingData> _WorkingDatas = new List<WorkingData>();
        public Task<List<WorkingData>> GetWorkingDatasAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _WorkingDatas = _DSDBContext.WorkingDatas.Where(x => x.IsDel == false).ToList();
                return Task.FromResult(_WorkingDatas);
            }
        }
        public Task<List<WorkingData>> ChangeWorkingDatasAsync(WorkingData changed)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _WorkingDatas.FirstOrDefault(i => i.OPNO == changed.OPNO && i.WorkingDate == changed.WorkingDate && i.WorkingType == changed.WorkingType);
                if (orginalForecast != null)
                {
                    orginalForecast.WorkingDate = changed.WorkingDate;
                    orginalForecast.WorkingType = changed.WorkingType;
                    orginalForecast.WorkingID = changed.WorkingID;
                    orginalForecast.WorkingDescription = changed.WorkingDescription;
                    orginalForecast.WorkingGroup = changed.WorkingGroup;
                    orginalForecast.OPNO = changed.OPNO;
                    orginalForecast.QTY = changed.QTY;
                    //orginalForecast.IsDel = changed.IsDel;
                    orginalForecast.UpdateTime = changed.UpdateTime;
                    _DSDBContext.WorkingDatas.Update(orginalForecast);
                }
                else
                {
                    _DSDBContext.WorkingDatas.Add(changed);
                    _WorkingDatas.Add(changed);
                }
                _DSDBContext.SaveChanges();
                return Task.FromResult(_WorkingDatas);
            }
        }
        public Task<List<WorkingData>> Remove(WorkingData forecast)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _WorkingDatas.FirstOrDefault(i => i.OPNO == forecast.OPNO && i.WorkingDate == forecast.WorkingDate && i.WorkingType == forecast.WorkingType);
                if (orginalForecast != null)
                {
                    orginalForecast.WorkingDate = forecast.WorkingDate;
                    orginalForecast.WorkingType = forecast.WorkingType;
                    orginalForecast.WorkingID = forecast.WorkingID;
                    orginalForecast.WorkingDescription = forecast.WorkingDescription;
                    orginalForecast.WorkingGroup = forecast.WorkingGroup;
                    orginalForecast.OPNO = forecast.OPNO;
                    orginalForecast.QTY = forecast.QTY;
                    orginalForecast.IsDel = true;
                    //orginalForecast.CreateTime = changed.CreateTime;
                    orginalForecast.UpdateTime = forecast.UpdateTime;
                    _DSDBContext.WorkingDatas.Update(orginalForecast);
                }
                _WorkingDatas.Remove(forecast);
                //_DSDBContext.OperatorInformations.Remove(forecast);
                _DSDBContext.SaveChanges();
                return Task.FromResult(_WorkingDatas);
            }
        }

        private List<WorkingType> _WorkingTypes = new List<WorkingType>();

        private List<WorkingTypeDetail> _WorkingTypeDetail = new List<WorkingTypeDetail>();
        public Task<List<WorkingType>> GetWorkingTypesAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _WorkingTypes = _DSDBContext.WorkingTypes.Where(x => x.IsDel == false).ToList();
                return Task.FromResult(_WorkingTypes);
            }
        }
        public Task<List<WorkingTypeDetail>> GetWorkingTypeDetailAsync(string groupname)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                if (_WorkingTypeDetail.Count > 0)
                {
                    _WorkingTypeDetail.Clear();
                }
                foreach (var item in _DSDBContext.WorkingTypes.Where(x => x.IsDel == false && x.TypeGroup == groupname).ToList())
                {
                    _WorkingTypeDetail.Add(new WorkingTypeDetail
                    {
                        TypeID = item.TypeID,
                        TypeName = item.TypeName,
                        IsSelect = false
                    });
                }
                return Task.FromResult(_WorkingTypeDetail);
            }
        }

        public Task<List<WorkingType>> ChangeWorkingTypesAsync(WorkingType changed)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _WorkingTypes.FirstOrDefault(i => i.TypeID == changed.TypeID && i.TypeName == changed.TypeName);
                if (orginalForecast != null)
                {
                    orginalForecast.TypeID = changed.TypeID;
                    orginalForecast.TypeName = changed.TypeName;
                    orginalForecast.TypeGroup = changed.TypeGroup;
                    _DSDBContext.WorkingTypes.Update(orginalForecast);
                }
                else
                {
                    _DSDBContext.WorkingTypes.Add(changed);
                    _WorkingTypes.Add(changed);
                }
                _DSDBContext.SaveChanges();
                return Task.FromResult(_WorkingTypes);
            }
        }
        public Task<List<WorkingType>> Remove(WorkingType forecast)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _WorkingTypes.FirstOrDefault(i => i.TypeID == forecast.TypeID && i.TypeName == forecast.TypeName);
                if (orginalForecast != null)
                {
                    orginalForecast.TypeID = forecast.TypeID;
                    orginalForecast.IsDel = true;
                    orginalForecast.TypeName = forecast.TypeName;
                    orginalForecast.TypeGroup = forecast.TypeGroup;
                    _DSDBContext.WorkingTypes.Update(orginalForecast);
                }
                _WorkingTypes.Remove(forecast);
                _DSDBContext.SaveChanges();
                return Task.FromResult(_WorkingTypes);
            }
        }

        private List<WorkingTypeGroup> _WorkingTypeGroups = new List<WorkingTypeGroup>();
        public Task<List<WorkingTypeGroup>> GetWorkingTypeGroupsAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _WorkingTypeGroups = _DSDBContext.WorkingTypeGroups.Where(x => x.IsDel == false).ToList();
                return Task.FromResult(_WorkingTypeGroups);
            }
        }
        public Task<List<WorkingTypeGroup>> ChangeWorkingTypeGroupsAsync(WorkingTypeGroup changed, string processname)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _WorkingTypeGroups.FirstOrDefault(i => i.TypeGroup == changed.TypeGroup);
                if (orginalForecast != null)
                {
                    orginalForecast.TypeGroup = changed.TypeGroup;
                    orginalForecast.TypeGroupName = changed.TypeGroupName;
                    orginalForecast.ManufacturingProcess = processname;
                    _DSDBContext.WorkingTypeGroups.Update(orginalForecast);
                }
                else
                {
                    changed.ManufacturingProcess = processname;
                    _DSDBContext.WorkingTypeGroups.Add(changed);
                    _WorkingTypeGroups.Add(changed);
                }
                _DSDBContext.SaveChanges();
                return Task.FromResult(_WorkingTypeGroups);
            }
        }
        public Task<List<WorkingTypeGroup>> Remove(WorkingTypeGroup forecast)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _WorkingTypeGroups.FirstOrDefault(i => i.TypeGroup == forecast.TypeGroup);
                if (orginalForecast != null)
                {
                    orginalForecast.TypeGroup = forecast.TypeGroup;
                    orginalForecast.IsDel = true;
                    orginalForecast.TypeGroupName = forecast.TypeGroupName;
                    _DSDBContext.WorkingTypeGroups.Update(orginalForecast);
                }
                _WorkingTypeGroups.Remove(forecast);
                _DSDBContext.SaveChanges();
                return Task.FromResult(_WorkingTypeGroups);
            }
        }

        private List<Stations> _Stations = new List<Stations>();
        public Task<List<Stations>> GetStationsAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                foreach (var item in _DSDBContext.WorkingTypeGroups.Where(x => x.IsDel == false))
                {
                    _Stations.Add(new Stations
                    {
                        StationID = item.TypeGroup,
                        StationName = item.TypeGroupName
                    });
                }
                return Task.FromResult(_Stations);
            }
        }

        public List<WorkingList> _WorkingLists = new List<WorkingList>();
        public Task<List<WorkingList>> GetWorkingListsAsync(string OPID)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                if (OPID == "Detail")
                {
                    _WorkingLists = _DSDBContext.WorkingLists.Where(x => x.IsDel == false).ToList();
                }
                else
                {
                    _WorkingLists = _DSDBContext.WorkingLists.Where(x => x.IsDel == false && x.OPNO == OPID).ToList();
                }
                return Task.FromResult(_WorkingLists);
            }
        }

        public Task<List<WorkingList>> GetWorkingListsTimeAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                //_WorkingLists = _DSDBContext.WorkingLists.ToList();
                _WorkingLists = _DSDBContext.WorkingLists.Where(x => x.IsDel == false && x.IsReceive == true && x.IsResponse == false && x.IsPause == false).ToList();
                return Task.FromResult(_WorkingLists);
            }
        }

        public Task<int> GetWorkingListsCountAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                return Task.FromResult(_DSDBContext.WorkingLists.ToList().Count);
            }
        }

        public Task<List<WorkingList>> ChangeWorkingListsAsync(WorkingList changed, bool IsNew)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _WorkingLists = _DSDBContext.WorkingLists.ToList();
                var orginalForecast = _WorkingLists.FirstOrDefault(i => i.WorkingTime == changed.WorkingTime && i.WorkingID == changed.WorkingID && i.WorkOrder == changed.WorkOrder && i.SeriaNumber == changed.SeriaNumber && i.OPNO == changed.OPNO && i.AssignTime == changed.AssignTime && i.IsDel == false);
                if (orginalForecast != null)
                {
                    if (!IsNew)
                    {
                        //orginalForecast.WorkingTime = changed.WorkingTime;
                        //orginalForecast.WorkingID = orginalForecast.WorkingID;
                        //orginalForecast.WorkingPeriod = changed.WorkingPeriod;
                        //orginalForecast.WorkingName = changed.WorkingName;
                        //orginalForecast.OPNO = changed.OPNO;
                        //orginalForecast.AssignTime = changed.AssignTime;
                        //orginalForecast.IsAssign = false;
                        orginalForecast.ReceiveTime = changed.ReceiveTime;
                        orginalForecast.IsReceive = changed.IsReceive;
                        orginalForecast.ResponseTime = changed.ResponseTime;
                        orginalForecast.IsResponse = changed.IsResponse;
                        orginalForecast.PauseTime = changed.PauseTime;
                        orginalForecast.IsPause = changed.IsPause;
                        _DSDBContext.WorkingLists.Update(orginalForecast);
                    }
                    else
                    {
                        //changed.WorkingID = orginalForecast.WorkingID + 1;
                        changed.WorkingPeriod = "";
                        changed.IsDel = false;
                        changed.IsAssign = true;
                        changed.IsPause = false;
                        changed.IsReceive = false;
                        changed.IsResponse = false;
                        _DSDBContext.WorkingLists.Add(changed);
                        _WorkingLists.Add(changed);
                    }
                }
                else
                {
                    //changed.WorkingID = 1;
                    changed.WorkingPeriod = "";
                    changed.IsDel = false;
                    changed.IsAssign = true;
                    changed.IsReceive = false;
                    changed.IsResponse = false;
                    _DSDBContext.WorkingLists.Add(changed);
                    _WorkingLists.Add(changed);
                }
                _DSDBContext.SaveChanges();
                return Task.FromResult(_WorkingLists);
            }
        }
        public Task<List<WorkingList>> ChangeWorkingListsAsync(List<WorkingList> changed)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                foreach (WorkingList item in changed)
                {
                    _DataLogService.WriteEventLog("DB", "Add WorkingList" + "|" + item.WorkingTime + "|" + item.OPNO + "|" + item.WorkOrder + "|" + item.SeriaNumber + "|" + "Assign :" + item.AssignTime);
                    _DSDBContext.WorkingLists.Add(item);
                    _WorkingLists.Add(item);
                }
                _DSDBContext.SaveChanges();
                return Task.FromResult(_WorkingLists);
            }
        }
        public Task<List<WorkingList>> UpdateWorkingListsAsync(WorkingList change)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _DSDBContext.WorkingLists.Update(change);
                _DSDBContext.SaveChanges();
                return Task.FromResult(_WorkingLists);
            }
        }
        public Task<List<WorkingList>> Remove(WorkingList forecast)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _WorkingLists.FirstOrDefault(i => i.WorkingTime == forecast.WorkingTime && i.WorkOrder == forecast.WorkOrder && i.SeriaNumber == forecast.SeriaNumber && i.OPNO == forecast.OPNO && i.AssignTime == forecast.AssignTime);
                if (orginalForecast != null)
                {
                    orginalForecast.WorkingTime = forecast.WorkingTime;
                    orginalForecast.IsDel = true;
                    orginalForecast.WorkingID = forecast.WorkingID;
                    //orginalForecast.WorkingPeriod = forecast.WorkingPeriod;
                    orginalForecast.WorkingName = forecast.WorkingName;
                    orginalForecast.OPNO = forecast.OPNO;
                    orginalForecast.AssignTime = forecast.AssignTime;
                    orginalForecast.ReceiveTime = forecast.ReceiveTime;
                    orginalForecast.ResponseTime = forecast.ResponseTime;
                    _DSDBContext.WorkingLists.Update(orginalForecast);
                }
                _DataLogService.WriteEventLog("DB", "Remove WorkingList" + "|" + orginalForecast.WorkingTime + "|" + orginalForecast.OPNO + "|" + orginalForecast.WorkOrder + "|" + orginalForecast.SeriaNumber + "|" + "Remove :" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
                _WorkingLists.Remove(forecast);
                _DSDBContext.SaveChanges();
                return Task.FromResult(_WorkingLists);
            }
        }
        public Task<bool> CheckWorkingListsAsync(WorkingList forecast)
        {
            bool result = false;
            var orginalForecast = _WorkingLists.FirstOrDefault(i => i.WorkingTime == forecast.WorkingTime && i.WorkOrder == forecast.WorkOrder && i.SeriaNumber == forecast.SeriaNumber && i.OPNO == forecast.OPNO && i.AssignTime == forecast.AssignTime);
            if (orginalForecast != null)
            {
                result = true;
            }
            return Task.FromResult(result);
        }

        private List<OperatorWorkTime> _OperatorWorkTimes = new List<OperatorWorkTime>();
        public Task<List<OperatorWorkTime>> GetOperatorWorkTimesAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                foreach (var item in _DSDBContext.TimePeriods.Where(x => x.IsDel == false))
                {
                    _OperatorWorkTimes.Add(new OperatorWorkTime
                    {
                        OperatorWorkTimeID = item.PeriodType,
                        OperatorWorkTimeName = item.Description
                    });
                }
                return Task.FromResult(_OperatorWorkTimes);
            }
        }

        private List<Operator> _Operatorss = new List<Operator>();
        public Task<List<Operator>> GetOperatorsAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                if (_Operatorss.Count != 0)
                {
                    _Operatorss.Clear();
                }
                foreach (var item in _DSDBContext.OperatorInformations.Where(x => x.IsDel == false))
                {
                    _Operatorss.Add(new Operator
                    {
                        OperatorID = item.OPNO,
                        OperatorName = item.OperatorName,
                        IsSelect = false
                    });
                }
                return Task.FromResult(_Operatorss);
            }
        }
        public Task<List<Operator>> GetOperatorsAsync(string StationName)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                if (_Operatorss.Count != 0)
                {
                    _Operatorss.Clear();
                }
                foreach (var item in _DSDBContext.OperatorInformations.Where(x => x.IsDel == false && x.Ability.Contains(StationName)).ToList())
                {
                    _Operatorss.Add(new Operator
                    {
                        OperatorID = item.OPNO,
                        OperatorName = item.OperatorName,
                        IsSelect = false
                    });
                }
                return Task.FromResult(_Operatorss);
            }
        }

        private List<OperatorProcedure> _OperatorProcedures = new List<OperatorProcedure>();
        public Task<List<OperatorProcedure>> GetOperatorProceduresAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                foreach (var item in _DSDBContext.WorkingTypeGroups.Where(x => x.IsDel == false))
                {
                    _OperatorProcedures.Add(new OperatorProcedure
                    {
                        OperatorProcedureID = item.TypeGroup,
                        OperatorProcedureName = item.TypeGroupName,
                        OperatorProcedureSFC = item.ManufacturingProcess
                    });
                }
                return Task.FromResult(_OperatorProcedures);
            }
        }

        private List<WorkOrderSelection> _WorkOrderSelections = new List<WorkOrderSelection>();
        private List<WorkOrderSelection> _WorkOrders = new List<WorkOrderSelection>();

        public List<WorkOrderSelection> GetWorkOrdersAsync(string param1)
        {
            _WorkOrders.Clear();

            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                foreach (var item in _DSDBContext.SerialNumberByWorkOrders.FromSqlInterpolated($"sp_SerialNumberByWorkOrder {param1}").ToList())
                {
                    _WorkOrders.Add(new WorkOrderSelection
                    {
                        WorkOrder = item.OrderNo,
                        PartNo = item.PartNo,
                        PartName = item.PartName,
                        SerialNO = item.SerialNO,
                        IsSelect = false,
                        WorkOrderCount = 0
                    });
                }
                return _WorkOrders;
            }
        }

        //public Task<List<WorkOrderSelection>> GetWorkOrderSelectionAsync(string param1, string param2)
        public Task<List<WorkOrderSelection>> GetWorkOrderSelectionAsync(string param1)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                if (_WorkOrderSelections.Count != 0)
                {
                    _WorkOrderSelections.Clear();
                }

                List<WorkOrderSelection> _WorkOrderItems = GetWorkOrdersAsync(param1);

                //foreach (var item in _DSDBContext.SerialNumberByWorkOrders.FromSqlInterpolated($"sp_SerialNumberByWorkOrder {param1},{param2}").ToList().Select(x => x.OrderNo).Distinct().ToList())
                //foreach (var item in _DSDBContext.SerialNumberByWorkOrders.FromSqlInterpolated($"sp_SerialNumberByWorkOrder {param1}").ToList().Select(x => x.OrderNo).Distinct().ToList())
                //foreach (var item in _DSDBContext.SerialNumberByWorkOrders.FromSqlInterpolated($"sp_SerialNumberByWorkOrder {param1}").ToList().Where(x => x.StartTime == "").Select(x => x.OrderNo).Distinct().ToList())
                foreach (var item in _WorkOrderItems.Select(x => x.WorkOrder).Distinct().ToList())
                {
                    _WorkOrderSelections.Add(new WorkOrderSelection
                    {
                        WorkOrder = item,
                        PartNo = _WorkOrderItems.FirstOrDefault(x => x.WorkOrder == item).PartNo,
                        PartName = _WorkOrderItems.FirstOrDefault(x => x.WorkOrder == item).PartName,
                        IsSelect = false,
                        WorkOrderCount = 0
                    });
                }
                return Task.FromResult(_WorkOrderSelections);
            }
        }

        private List<SerialNumberSelection> _SerialNumberSelections = new List<SerialNumberSelection>();
        //public Task<List<SerialNumberSelection>> GetSerialNumberSelectionAsync(string param1, string param2)
        public Task<List<SerialNumberSelection>> GetSerialNumberSelectionAsync(string param1)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                if (_SerialNumberSelections.Count != 0)
                {
                    _SerialNumberSelections.Clear();
                }

                List<WorkOrderSelection> _WorkOrderItems = GetWorkOrdersAsync(param1);
                //foreach (var item in _DSDBContext.SerialNumberByWorkOrders.FromSqlInterpolated($"sp_SerialNumberByWorkOrder {param1},{param2}").ToList())
                //foreach (var item in _DSDBContext.SerialNumberByWorkOrders.FromSqlInterpolated($"sp_SerialNumberByWorkOrder {param1}").ToList())
                foreach (var item in _WorkOrderItems)
                {
                    _SerialNumberSelections.Add(new SerialNumberSelection
                    {
                        WorkOrder = item.WorkOrder,
                        //PartNo = _WorkOrderItems.FirstOrDefault(x => x.WorkOrder == item.WorkOrder).PartNo,
                        //PartName = _WorkOrderItems.FirstOrDefault(x => x.WorkOrder == item.WorkOrder).PartName,
                        IsWorkOrderSelect = false,
                        SerialNumber = item.SerialNO,
                        IsSerialNumberSelect = false
                    });
                }
                return Task.FromResult(_SerialNumberSelections);
            }
        }

        private List<OperatorAbility> _OperatorAbilitys = new List<OperatorAbility>();
        public Task<List<OperatorAbility>> GetOperatorAbilityAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                if (_OperatorAbilitys.Count > 0)
                {
                    _OperatorAbilitys.Clear();
                }
                foreach (var item in _DSDBContext.OperatorInformations.Where(x => x.IsDel == false).ToList())
                {
                    List<WorkAbility> _WorkAbilitys = new List<WorkAbility>();

                    foreach (var items in _DSDBContext.WorkingTypeGroups.Where(x => x.IsDel == false).ToList())
                    {
                        _WorkAbilitys.Add(new WorkAbility
                        {
                            WorkNumber = items.TypeGroup,
                            WorkName = items.TypeGroupName,
                            IsSelect = false
                        });
                    }

                    if (item.Ability != null && item.Ability != "")
                    {
                        foreach (var Abilitys in item.Ability.Split(','))
                        {
                            _WorkAbilitys.Single(x => x.WorkNumber == Abilitys).IsSelect = true;
                        }
                    }

                    _OperatorAbilitys.Add(new OperatorAbility
                    {
                        OperatorID = item.OPNO,
                        OperatorList = _WorkAbilitys
                    });
                }
                return Task.FromResult(_OperatorAbilitys);
            }
        }
        public Task<List<OperatorAbility>> ChangeOperatorAbilityAsync(List<OperatorAbility> changed)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                foreach (var item in changed)
                {
                    string _Ability = "";
                    foreach (var items in item.OperatorList)
                    {
                        if (items.IsSelect == true)
                        {
                            _Ability = _Ability + items.WorkNumber + ",";
                        }
                    }
                    if (_Ability != "")
                    {
                        _Ability = _Ability.Substring(0, _Ability.Length - 1);
                    }
                    _OperatorInformations.Single(x => x.OPNO == item.OperatorID).Ability = _Ability;
                    _DSDBContext.OperatorInformations.UpdateRange(_OperatorInformations);
                    _DSDBContext.SaveChanges();
                }
                return Task.FromResult(changed);
            }
        }

        private List<WorkingTypeGroupBySFC> _WorkingTypeGroupBySFCs = new List<WorkingTypeGroupBySFC>();
        public Task<List<WorkingTypeGroupBySFC>> GetWorkingTypeGroupBySFCAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                foreach (var item in _DSDBContext.WorkingTypeGroups.Where(x => x.IsDel == false).ToList())
                {
                    List<WorkingTypeGroupBySFCDetail> _WorkingTypeGroupBySFCDetails = new List<WorkingTypeGroupBySFCDetail>();

                    foreach (var items in _DSDBContext.ManufacturingProcessNames.ToList())
                    {
                        _WorkingTypeGroupBySFCDetails.Add(new WorkingTypeGroupBySFCDetail
                        {
                            ProcessName = items.ProcessName,
                            //ProcessCName = items.ProcessCName,
                            IsSelect = false
                        });
                    }

                    if (item.ManufacturingProcess != null && item.ManufacturingProcess != "")
                    {
                        if (item.ManufacturingProcess == "RobotAssembly")
                        {
                            _WorkingTypeGroupBySFCDetails.Single(x => x.ProcessName == item.ManufacturingProcess).IsSelect = true;
                        }
                    }

                    _WorkingTypeGroupBySFCs.Add(new WorkingTypeGroupBySFC
                    {
                        TypeGroup = item.TypeGroup,
                        DetailList = _WorkingTypeGroupBySFCDetails
                    });
                }
                return Task.FromResult(_WorkingTypeGroupBySFCs);
            }
        }

        private List<WorkingTypeGroupBySFCDetail> _WorkingTypeGroupBySFCsList = new List<WorkingTypeGroupBySFCDetail>();
        public Task<List<WorkingTypeGroupBySFCDetail>> GetWorkingTypeGroupBySFCListAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                foreach (var items in _DSDBContext.ManufacturingProcessNames.ToList())
                {
                    _WorkingTypeGroupBySFCsList.Add(new WorkingTypeGroupBySFCDetail
                    {
                        ProcessName = items.ProcessName,
                        //ProcessCName = items.ProcessCName,
                        IsSelect = false
                    });
                }
                return Task.FromResult(_WorkingTypeGroupBySFCsList);
            }
        }

        private List<WorkingHoursStatistic> _WorkingHoursStatistics = new List<WorkingHoursStatistic>();

        public Task<List<WorkingHoursStatistic>> GetWorkingHoursStatisticsAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                List<WorkingTimeDetail> _WorkingTimeDetails = new List<WorkingTimeDetail>();
                _WorkingHoursStatistics = _DSDBContext.WorkingHoursStatistics.ToList();

                _WorkingTimeDetails = _DSDBContext.WorkingTimeDetails.ToList();
                //_DSDBContext.WorkingTimeDetails.ExecuteDelete();
                //_DSDBContext.SaveChanges();
                _DSDBContext.WorkingTimeDetails.RemoveRange(_WorkingTimeDetails);
                _DSDBContext.SaveChanges();
                //foreach (var item in _WorkingTimeDetails)
                //{
                //    _DSDBContext.WorkingTimeDetails.Remove(item);
                //    _DSDBContext.SaveChanges();
                //}
                _WorkingTimeDetails = _DSDBContext.WorkingTimeDetails.ToList();

                foreach (var item in _WorkingHoursStatistics)
                {
                    int WorkTime = 0;

                    WorkTime = item.DiffTime;

                    if (item.DiffDay > 1 && item.ReceiveTime.DayOfWeek != DayOfWeek.Friday && item.ResponseTime.DayOfWeek != DayOfWeek.Monday)
                    {
                        WorkTime = item.DiffTime + (item.DiffDay - 1) * 27000;
                    }

                    DateTime _ReceiveTime12 = Convert.ToDateTime(item.ReceiveTime.ToString("yyyy/MM/dd") + " 12:00:00");
                    DateTime _ResponseTime12 = Convert.ToDateTime(item.ResponseTime.ToString("yyyy/MM/dd") + " 12:00:00");

                    int _ReceiveTimeCompare = DateTime.Compare(item.ReceiveTime, _ReceiveTime12);
                    int _ResponseTimeCompare = DateTime.Compare(item.ResponseTime, _ResponseTime12);

                    if (item.DiffDay == 0 && _ReceiveTimeCompare == -1 && _ResponseTimeCompare == 1)
                    {
                        WorkTime = WorkTime - 3600;
                    }
                    else if (item.DiffDay >= 1 && _ReceiveTimeCompare == -1 && _ResponseTimeCompare == -1)
                    {
                        WorkTime = WorkTime - 3600;
                    }
                    else if (item.DiffDay >= 1 && _ReceiveTimeCompare == 1 && _ResponseTimeCompare == 1)
                    {
                        WorkTime = WorkTime - 3600;
                    }
                    else if (item.DiffDay >= 1 && _ReceiveTimeCompare == -1 && _ResponseTimeCompare == 1)
                    {
                        WorkTime = WorkTime - 7200;
                    }

                    _WorkingTimeDetails.Add(new WorkingTimeDetail
                    {
                        Index = item.Index,
                        WorkOrder = item.WorkOrder,
                        SeriaNumber = item.SeriaNumber,
                        OPNO = item.OPNO,
                        ReceiveTime = item.ReceiveTime,
                        ResponseTime = item.ResponseTime,
                        WorkTime = WorkTime
                    });

                }
                _DSDBContext.WorkingTimeDetails.AddRange(_WorkingTimeDetails);
                _DSDBContext.SaveChanges();
                return Task.FromResult(_WorkingHoursStatistics);
            }
        }

        private List<OP_Report> _OP_Reports = new List<OP_Report>();

        public Task<List<OP_Report>> GetOP_ReportsAsync(DateTime param1, DateTime param2)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _OP_Reports = _DSDBContext.OP_Reports.FromSqlInterpolated($"sp_OP_Report {param1},{param2}").ToList();
                return Task.FromResult(_OP_Reports);
            }
        }

        private List<OP_Report_Day> _OP_Report_Days = new List<OP_Report_Day>();

        public Task<List<OP_Report_Day>> GetOP_Report_DaysAsync(DateTime param1, DateTime param2)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _OP_Report_Days = _DSDBContext.OP_Report_Days.FromSqlInterpolated($"sp_OP_ReportByDay {param1},{param2}").ToList();
                return Task.FromResult(_OP_Report_Days);
            }
        }

        private List<WO_SN_Report> _WO_SN_Reports = new List<WO_SN_Report>();

        public Task<List<WO_SN_Report>> GetWO_SN_ReportsAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _WO_SN_Reports = _DSDBContext.WO_SN_Reports.ToList();
                return Task.FromResult(_WO_SN_Reports);
            }
        }

        private List<WorkingStepTimeList> _WorkingStepTimeLists = new List<WorkingStepTimeList>();

        public Task<List<WorkingStepTimeList>> GetWorkingPauseTimeListsAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _WorkingStepTimeLists = _DSDBContext.WorkingStepTimeLists.ToList();
                return Task.FromResult(_WorkingStepTimeLists);
            }
        }

        public Task<List<WorkingStepTimeList>> ChangeWorkingPauseTimeListsAsync(WorkingList changed, string WorkType, bool? IsReceive, bool? IsPause, bool? IsResponse, ReasonType reasontype)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                //var orginalForecast = _WorkingPauseTimeLists.FirstOrDefault(i => i.WorkingTime == changed.WorkingTime && i.WorkOrder == changed.WorkOrder && i.SeriaNumber == changed.SeriaNumber && i.OPNO == changed.OPNO && i.PauseTime == changed.PauseTime);
                //if (orginalForecast != null)
                //{
                //    orginalForecast.IsDel = changed.IsDel;
                //    if (orginalForecast.IsPause == true)
                //    {
                //        orginalForecast.IsPause = false;
                //        orginalForecast.StartTime = DateTime.Now;
                //    }
                //    orginalForecast.StartTime = DateTime.Now;
                //    _DSDBContext.WorkingPauseTimeLists.Update(orginalForecast);
                //}
                //else
                //{
                var orginalForecast = new WorkingStepTimeList();
                orginalForecast.WorkingTime = changed.WorkingTime;
                orginalForecast.WorkingName = changed.WorkingName;
                orginalForecast.WorkingDescription = changed.WorkingDescription;
                orginalForecast.ProcessName = changed.ProcessName;
                orginalForecast.ProcessDescription = changed.ProcessDescription;
                orginalForecast.WorkOrder = changed.WorkOrder;
                orginalForecast.SeriaNumber = changed.SeriaNumber;
                orginalForecast.OPNO = changed.OPNO;
                orginalForecast.OPName = changed.OPName; 
                orginalForecast.WorkingStatus = "";
                if (WorkType == "Receive")
                {
                    if (IsReceive == true)
                    {
                        orginalForecast.WorkingStatus = "Received";
                    }
                }
                if (WorkType == "Pause")
                {
                    if (IsPause == false)
                    {
                        orginalForecast.WorkingStatus = "Working";
                    }
                    else
                    {
                        orginalForecast.WorkingStatus = "Pause";
                    }
                }
                if (WorkType == "Response")
                {
                    if (IsResponse == true)
                    {
                        orginalForecast.WorkingStatus = "Responsed";
                    }
                }
                if (reasontype != null)
                {
                    orginalForecast.ReasonType = reasontype.ReasonCode;
                }
                else
                {
                    orginalForecast.ReasonType = "";
                }
                orginalForecast.StatusTime = DateTime.Now;
                orginalForecast.IsDel = false;
                //orginalForecast.IsPause = IsPause;
                _DSDBContext.WorkingStepTimeLists.Add(orginalForecast);
                _WorkingStepTimeLists.Add(orginalForecast);
                //}
                _DSDBContext.SaveChanges();
                return Task.FromResult(_WorkingStepTimeLists);
            }
        }


        public Task<bool> CheckReceiveWork(string OPID)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                bool Result = false;
                foreach (var item in _DSDBContext.WorkingLists.Where(x => x.IsDel == false && x.OPNO == OPID && x.IsAssign == true && x.IsResponse == false).ToList())
                {
                    if (item.IsReceive == true)
                    {
                        Result = true;
                    }
                }
                return Task.FromResult(Result);
            }
        }

        public Task<bool> CheckPauseWork(string OPID)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                bool Result = false;
                foreach (var item in _DSDBContext.WorkingLists.Where(x => x.IsDel == false && x.OPNO == OPID && x.IsAssign == true && x.IsResponse == false).ToList())
                {
                    if (item.IsPause == true)
                    {
                        Result = true;
                    }
                }
                return Task.FromResult(Result);
            }
        }

        public Task<bool> CheckResponseWork(string OPID)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                bool Result = false;

                return Task.FromResult(Result);
            }
        }

        private List<ReasonType> _ReasonTypes = new List<ReasonType>();

        public Task<List<ReasonType>> GetReasonTypesAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _ReasonTypes = _DSDBContext.ReasonTypes.ToList();
                return Task.FromResult(_ReasonTypes);
            }
        }

        public Task<List<ReasonType>> Remove(ReasonType forecast)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _ReasonTypes.Remove(forecast);
                _DSDBContext.SaveChanges();
                return Task.FromResult(_ReasonTypes);
            }
        }

        private List<AdditionalTime> _AdditionalTimes = new List<AdditionalTime>();

        public Task<List<AdditionalTime>> GetAdditionalTimesAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _AdditionalTimes = _DSDBContext.AdditionalTimes.Where(x => x.IsDel == false).ToList();
                return Task.FromResult(_AdditionalTimes);
            }
        }

        public Task<List<AdditionalTime>> ChangeAdditionalTimeAsync(AdditionalTime changed)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _AdditionalTimes.FirstOrDefault(i => i.AddTimeDate == changed.AddTimeDate && i.OPNO == changed.OPNO && i.AddStartTime == changed.AddStartTime && i.AddEndTime == changed.AddEndTime);
                if (orginalForecast != null)
                {
                    orginalForecast.IsApprove = changed.IsApprove;
                    _DSDBContext.AdditionalTimes.Update(orginalForecast);
                }
                else
                {
                    _DSDBContext.AdditionalTimes.Add(changed);
                    _AdditionalTimes.Add(changed);
                }
                _DSDBContext.SaveChanges();
                return Task.FromResult(_AdditionalTimes);
                //DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                //_DSDBContext.AdditionalTimes.Add(changed);
                //_AdditionalTimes.Add(changed);
                //_DSDBContext.SaveChanges();
                //_AdditionalTimes = _DSDBContext.AdditionalTimes.Where(x => x.IsDel == false).ToList();
                //return Task.FromResult(_AdditionalTimes);
            }
        }

        public Task<List<AdditionalTime>> Remove(AdditionalTime forecast)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _AdditionalTimes.FirstOrDefault(i => i.AddTimeDate == forecast.AddTimeDate && i.OPNO == forecast.OPNO && i.AddStartTime == forecast.AddStartTime && i.AddEndTime == forecast.AddEndTime);
                if (orginalForecast != null)
                {
                    orginalForecast.IsDel = true;
                    _DSDBContext.AdditionalTimes.Update(orginalForecast);
                }
                _DSDBContext.SaveChanges();
                return Task.FromResult(_AdditionalTimes);
            }
        }

        private UserDataBase _UserDataBase;

        public void SetUserDataAsync(UserDataBase item)
        {
            _UserDataBase = item;
        }

        public Task<UserDataBase> GetUserDataAsync()
        {
            return Task.FromResult(_UserDataBase);
        }

        public Task<List<WorkOrderSelection>> GetERPWorkOrderList()
        {
            WorkOrderList.ResponseContent _WorkOrderList = new WorkOrderList.ResponseContent();

            string json = "{ \"wo_no\": \"GMO0-\", \"wo_date_start\": \"\", \"wo_date_end\": \"\", \"wo_modify_date_start\": \"\", \"wo_modify_date_end\": \"\", \"responseType\": \"xml\" }";

            string rawXml = PostRequest("http://172.25.210.61/TMMConnector/api/v1.0/GetAllWO", json);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(rawXml);
            XmlReader xmlReader = new XmlNodeReader(xmlDoc.LastChild.LastChild);
            XmlSerializer serializer = new XmlSerializer(typeof(WorkOrderList.ResponseContent));

            _WorkOrderList = (WorkOrderList.ResponseContent)serializer.Deserialize(xmlReader);

            var aaa = JsonSerializer.Serialize(_WorkOrderList);

            //string _WorkOrder = "";
            //string _PartName = "";
            //float _WorkOrderCount = 0;
            //bool IsIOModule = false;
            _WorkOrderSelections.Clear();

            foreach (var item in _WorkOrderList.Document.RecordSet)
            {
                if (item.Master.Name == "GetAllWO")
                {
                    string _WorkOrder = "";
                    string _PartNo = "";
                    string _PartName = "";
                    float _WorkOrderCount = 0;
                    bool IsIOModule = false;

                    foreach (var items in item.Master.Record.Field)
                    {
                        if (items.Name == "wo_no")
                        {
                            _WorkOrder = items.Value;
                        }
                        if (items.Value.Contains("SUB ASSY, IO MODULE"))
                        {
                            IsIOModule = true;
                        }
                        if (items.Name == "wo_qty")
                        {
                            _WorkOrderCount = float.Parse(items.Value);
                        }
                        if (items.Name == "wo_item")
                        {
                            _PartNo = items.Value;
                        }
                        if (items.Name == "item_name")
                        {
                            _PartName = items.Value;
                        }
                    }

                    if (IsIOModule)
                    {
                        //IsIOModule = false;
                        _WorkOrderSelections.Add(new WorkOrderSelection
                        {
                            WorkOrder = _WorkOrder,
                            PartNo= _PartNo,
                            PartName = _PartName,
                            IsSelect = false,
                            WorkOrderCount = _WorkOrderCount
                        });
                    }
                }
            }

            return Task.FromResult(_WorkOrderSelections);
        }

        public string PostRequest(string url, string json)
        {
            string result = "";
            WorkOrderList _WorkOrderList = new WorkOrderList();
            XmlSerializer serializer = new XmlSerializer(typeof(WorkOrderList));

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                //json = "[  { \"ReferenceId\": \"a123\"  } ]";
                Debug.Write(json);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            {
                using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    if (httpWebRequest.HaveResponse && response != null)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)e.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            result = reader.ReadToEnd();
                        }
                    }

                }
            }

            return result;

        }

        private List<StationStatus> _StationStatus = new List<StationStatus>();
        public Task<List<StationStatus>> GetStationStatusAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _StationStatus = _DSDBContext.StationStatuss.Where(x => x.IsDel == false).ToList();
                return Task.FromResult(_StationStatus);
            }
        }

        public Task<List<StationStatus>> UpdateStationStatusAsync(StationStatus _StationItem)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();

                var orginalForecast = _DSDBContext.StationStatuss.SingleOrDefault(x => x.StationKey == _StationItem.StationKey && x.StationID == _StationItem.StationID);

                if (orginalForecast == null)
                {
                    int Count;

                    if (_DSDBContext.StationStatuss.Count() == 0)
                    {
                        Count = 1;
                    }
                    else
                    {
                        Count = (int)_DSDBContext.StationStatuss.Select(x => x.StationID).Max() + 1;
                    }

                    orginalForecast = new StationStatus();
                    orginalForecast.IndexKey = 0;
                    orginalForecast.StationKey = "S";
                    orginalForecast.StationID = Count;
                    orginalForecast.StationName = _StationItem.StationName;
                    orginalForecast.Manufacturing = _StationItem.Manufacturing;
                    orginalForecast.IsDel = false;
                    orginalForecast.CreateUser = "";

                    _DSDBContext.StationStatuss.Add(orginalForecast);
                    _StationStatus.Add(orginalForecast);
                }
                else
                {
                    orginalForecast.StationKey = _StationItem.StationKey;
                    orginalForecast.StationID = _StationItem.StationID;
                    orginalForecast.StationName = _StationItem.StationName;
                    orginalForecast.Manufacturing = _StationItem.Manufacturing;
                    orginalForecast.IsDel = false;
                    orginalForecast.CreateUser = "";
                    _DSDBContext.StationStatuss.Update(orginalForecast);
                }

                _DSDBContext.SaveChanges();
                return Task.FromResult(_StationStatus);
            }
        }

        private List<StationSelection> _StationSelection = new List<StationSelection>();
        public Task<List<StationSelection>> GetStationSelectionAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _StationStatus = _DSDBContext.StationStatuss.Where(x => x.IsDel == false).ToList();
                _StationSelection.Clear();
                foreach (var item in _StationStatus)
                {
                    _StationSelection.Add(new StationSelection
                    {
                        StationID = item.StationKey + item.StationID.ToString(),
                        StationName = item.StationName,
                        Manufacturing = item.Manufacturing,
                        IsSelect = false,
                    });
                }
                return Task.FromResult(_StationSelection);
            }
        }

        public Task<List<StationStatus>> Remove(StationStatus forecast)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _StationStatus.FirstOrDefault(i => i.StationID == forecast.StationID && i.StationName == forecast.StationName);
                if (orginalForecast != null)
                {
                    orginalForecast.IsDel = true;
                    _DSDBContext.StationStatuss.Update(orginalForecast);
                }
                _StationStatus.Remove(forecast);
                _DSDBContext.SaveChanges();
                return Task.FromResult(_StationStatus);
            }
        }

        private List<ProcedureStatus> _ProcedureStatus = new List<ProcedureStatus>();
        public Task<List<ProcedureStatus>> GetProcedureStatusAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _ProcedureStatus = _DSDBContext.ProcedureStatuss.Where(x => x.IsDel == false).ToList();
                return Task.FromResult(_ProcedureStatus);
            }
        }

        public Task<List<ProcedureStatus>> UpdateProcedureStatusAsync(ProcedureStatus _ProcedureItem)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();

                var orginalForecast = _DSDBContext.ProcedureStatuss.SingleOrDefault(x => x.ProcedureKey == _ProcedureItem.ProcedureKey && x.ProcedureID == _ProcedureItem.ProcedureID);

                if (orginalForecast == null)
                {
                    int Count;

                    if (_DSDBContext.ProcedureStatuss.Count() == 0)
                    {
                        Count = 1;
                    }
                    else
                    {
                        Count = (int)_DSDBContext.ProcedureStatuss.Select(x => x.ProcedureID).Max() + 1;
                    }

                    orginalForecast = new ProcedureStatus();
                    orginalForecast.IndexKey = 0;
                    orginalForecast.ProcedureKey = "P";
                    orginalForecast.ProcedureID = Count;
                    orginalForecast.ProcedureName = _ProcedureItem.ProcedureName;
                    orginalForecast.StationID = _ProcedureItem.StationID;
                    orginalForecast.IsDel = false;
                    orginalForecast.CreateUser = "";

                    _DSDBContext.ProcedureStatuss.Add(orginalForecast);
                    _ProcedureStatus.Add(orginalForecast);
                }
                else
                {
                    orginalForecast.ProcedureKey = _ProcedureItem.ProcedureKey;
                    orginalForecast.ProcedureID = _ProcedureItem.ProcedureID;
                    orginalForecast.ProcedureName = _ProcedureItem.ProcedureName;
                    orginalForecast.StationID = _ProcedureItem.StationID;
                    orginalForecast.IsDel = false;
                    orginalForecast.CreateUser = "";
                    _DSDBContext.ProcedureStatuss.Update(orginalForecast);
                }

                _DSDBContext.SaveChanges();
                return Task.FromResult(_ProcedureStatus);
            }
        }

        private List<ProcedureSelection> _ProcedureSelection = new List<ProcedureSelection>();
        public Task<List<ProcedureSelection>> GetProcedureSelectionAsync(string id)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _ProcedureStatus = _DSDBContext.ProcedureStatuss.Where(x => x.IsDel == false).ToList();
                _ProcedureSelection.Clear();
                foreach (var item in _ProcedureStatus.Where(x => x.StationID == id).OrderBy(x => x.IndexKey))
                {
                    _ProcedureSelection.Add(new ProcedureSelection
                    {
                        ProcedureID = item.ProcedureKey + item.ProcedureID.ToString(),
                        ProcedureName = item.ProcedureName,
                        StationID = item.StationID,
                        IsSelect = false,
                    });
                }
                return Task.FromResult(_ProcedureSelection);
            }
        }

        public Task<List<ProcedureSelection>> GetProcedureSelectionAsync(List<StationStatus> StationStatusItem)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _ProcedureStatus = _DSDBContext.ProcedureStatuss.Where(x => x.IsDel == false).ToList();
                _ProcedureSelection.Clear();
                foreach (var item in StationStatusItem)
                {
                    foreach (var items in _ProcedureStatus.Where(x => x.StationID == (item.StationKey + item.StationID.ToString())))
                    {
                        _ProcedureSelection.Add(new ProcedureSelection
                        {
                            ProcedureID = items.ProcedureKey + items.ProcedureID.ToString(),
                            ProcedureName = items.ProcedureName,
                            StationID = items.StationID,
                            IsSelect = false,
                        });
                    }
                }
                return Task.FromResult(_ProcedureSelection);
            }
        }

        public Task<List<ProcedureStatus>> Remove(ProcedureStatus forecast)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _ProcedureStatus.FirstOrDefault(i => i.ProcedureID == forecast.ProcedureID && i.ProcedureName == forecast.ProcedureName);
                if (orginalForecast != null)
                {
                    orginalForecast.IsDel = true;
                    _DSDBContext.ProcedureStatuss.Update(orginalForecast);
                }
                _ProcedureStatus.Remove(forecast);
                _DSDBContext.SaveChanges();
                return Task.FromResult(_ProcedureStatus);
            }
        }

        private List<ModelStatus> _ModelStatus = new List<ModelStatus>();
        public Task<List<ModelStatus>> GetModelStatusAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _ModelStatus = _DSDBContext.ModelStatuss.Where(x => x.IsDel == false).ToList();
                return Task.FromResult(_ModelStatus);
            }
        }

        public Task<List<ModelStatus>> UpdateModelStatusStatusAsync(ModelStatus _ModelItem)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();

                var orginalForecast = _DSDBContext.ModelStatuss.SingleOrDefault(x => x.ModelKey == _ModelItem.ModelKey && x.ModelID == _ModelItem.ModelID);

                if (orginalForecast == null)
                {
                    int Count;

                    if (_DSDBContext.ModelStatuss.Count() == 0)
                    {
                        Count = 1;
                    }
                    else
                    {
                        Count = (int)_DSDBContext.ModelStatuss.Select(x => x.ModelID).Max() + 1;
                    }

                    orginalForecast = new ModelStatus();

                    orginalForecast.ModelKey = "M";
                    orginalForecast.ModelID = Count;
                    orginalForecast.ModelName = _ModelItem.ModelName;
                    orginalForecast.Description = _ModelItem.Description;
                    orginalForecast.IsDel = false;
                    orginalForecast.CreateUser = "";

                    _DSDBContext.ModelStatuss.Add(orginalForecast);
                    _ModelStatus.Add(orginalForecast);
                }
                else
                {
                    orginalForecast.ModelKey = _ModelItem.ModelKey;
                    orginalForecast.ModelID = _ModelItem.ModelID;
                    orginalForecast.ModelName = _ModelItem.ModelName;
                    orginalForecast.Description = _ModelItem.Description;
                    orginalForecast.IsDel = false;
                    orginalForecast.CreateUser = "";
                    _DSDBContext.ModelStatuss.Update(orginalForecast);
                }

                _DSDBContext.SaveChanges();
                return Task.FromResult(_ModelStatus);
            }
        }

        public Task<List<ModelStatus>> Remove(ModelStatus forecast)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _ModelStatus.FirstOrDefault(i => i.ModelID == forecast.ModelID && i.ModelName == forecast.ModelName);
                if (orginalForecast != null)
                {
                    orginalForecast.IsDel = true;
                    _DSDBContext.ModelStatuss.Update(orginalForecast);
                }
                _ModelStatus.Remove(forecast);
                _DSDBContext.SaveChanges();
                return Task.FromResult(_ModelStatus);
            }
        }

        private List<ModelSelection> _ModelSelection = new List<ModelSelection>();
        public Task<List<ModelSelection>> GetModelSelectionAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _ModelStatus = _DSDBContext.ModelStatuss.Where(x => x.IsDel == false).ToList();
                _ModelSelection.Clear();
                foreach (var item in _ModelStatus)
                {
                    _ModelSelection.Add(new ModelSelection
                    {
                        ModelKey = item.ModelKey,
                        ModelID = item.ModelID,
                        ModelName = item.ModelName,
                        Description = item.Description,
                        IsSelect = false,
                    });
                }
                return Task.FromResult(_ModelSelection);
            }
        }

        private List<ESOPStatus> _ESOPStatus = new List<ESOPStatus>();
        public Task<List<ESOPStatus>> GetESOPStatusAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _ESOPStatus = _DSDBContext.ESOPStatuss.Where(x => x.IsDel == false).ToList();
                return Task.FromResult(_ESOPStatus);
            }
        }
        public Task<List<ESOPStatus>> ChangeESOPStatusAsync(ESOPStatus changed)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _ESOPStatus = _DSDBContext.ESOPStatuss.ToList();
                var orginalForecast = _ESOPStatus.FirstOrDefault(i => i.StationID == changed.StationID && i.ProcedureID == changed.ProcedureID && i.ModelID == changed.ModelID);
                if (orginalForecast != null)
                {
                    //orginalForecast.IsDel = false;
                    orginalForecast.ESOPCode = changed.ESOPCode;
                    orginalForecast.FileSite = changed.FileSite;
                    _DSDBContext.ESOPStatuss.Update(orginalForecast);
                }
                else
                {
                    _DSDBContext.ESOPStatuss.Add(changed);
                    _ESOPStatus.Add(changed);
                }
                _DSDBContext.SaveChanges();
                return Task.FromResult(_ESOPStatus);
            }
        }
        public Task<List<ESOPStatus>> Remove(ESOPStatus forecast)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var orginalForecast = _ESOPStatus.FirstOrDefault(i => i.StationID == forecast.StationID && i.ProcedureID == forecast.ProcedureID && i.ModelID == forecast.ModelID);
                if (orginalForecast != null)
                {
                    //orginalForecast.IsDel = true;
                    //_DSDBContext.ESOPStatuss.Update(orginalForecast);
                    _DSDBContext.ESOPStatuss.Remove(orginalForecast);
                }
                _ESOPStatus.Remove(forecast);
                _DSDBContext.SaveChanges();
                return Task.FromResult(_ESOPStatus);
            }
        }

        private List<ESOPFileStatus> _ESOPFileStatus = new List<ESOPFileStatus>();

        public Task<List<ESOPFileStatus>> GetESOPFileStatusAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                string _Directory = System.IO.Directory.GetCurrentDirectory() + @"\wwwroot\Uploads\SOP\";

                if (Directory.Exists(_Directory))
                {
                    foreach (var item in Directory.GetFiles(_Directory, "*.pdf"))
                    {
                        _ESOPFileStatus.Add(new ESOPFileStatus
                        {
                            StationID = "",
                            ProcedureID = "",
                            ModelID = "",
                            ESOPCode = item.Replace(_Directory, ""),
                            FileSite = item,
                            IsDel = false,
                            CreateUser = ""
                        });
                    }
                }

                return Task.FromResult(_ESOPFileStatus);
            }
        }

        private List<DirectoryStatus> _DirectoryStatus = new List<DirectoryStatus>();
        private List<FolderStatus> _FolderStatus = new List<FolderStatus>();

        public Task<Tuple<List<DirectoryStatus>, List<FolderStatus>>> GetESOPDirectoryStatusAsync(string DirectoryName)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                //string _Directory = System.IO.Directory.GetCurrentDirectory() + @"\wwwroot\esop\";
                //string _Directory = @"Z:\411 Standard Operation Procedure";
                string _Directory = @DirectoryName;
                //List<DirectoryStatus> _DirectoryStatus = new List<DirectoryStatus>();
                string _Folder = "";
                _DirectoryStatus = new List<DirectoryStatus>();
                _FolderStatus = new List<FolderStatus>();
                if (Directory.Exists(_Directory))
                {
                    DirectoryInfo dir = new DirectoryInfo(_Directory);
                    foreach (DirectoryInfo item in dir.GetDirectories())
                    {
                        if (!item.Name.Contains("_INA"))
                        {
                            if (item.Name.Contains("-") && item.Name.Split("-").Count() == 3)
                            {
                                if (_Folder != item.Name.Split("-")[0])
                                {
                                    _FolderStatus.Add(new FolderStatus
                                    {
                                        FolderName = item.Name.Split("-")[0],
                                        IsSelect = false
                                    });
                                    _Folder = item.Name.Split("-")[0];
                                }
                                //DirectoryInfo dirs = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory() + @"\wwwroot\esop\" + item.Name);
                                //DirectoryInfo dirs = new DirectoryInfo(Path.Combine(@"Z:\411 Standard Operation Procedure", item.Name));
                                DirectoryInfo dirs = new DirectoryInfo(Path.Combine(@DirectoryName, item.Name));
                                List<string> strings = new List<string>();
                                List<int> ints = new List<int>();
                                string MaxDirectory = "";
                                string FileName = "";
                                foreach (DirectoryInfo items in dirs.GetDirectories())
                                {
                                    strings.Add(items.Name);
                                    if (items.Name.Contains("."))
                                    {
                                        int number;

                                        if (int.TryParse(items.Name.Split(".")[1], out number))
                                        {
                                            ints.Add(int.Parse(items.Name.Split(".")[1]));
                                            MaxDirectory = items.Name.Split(".")[0];
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(MaxDirectory))
                                {
                                    MaxDirectory = MaxDirectory + "." + ints.OrderByDescending(x => x).FirstOrDefault();

                                    //foreach (var items in Directory.GetFiles(_Directory + item.Name + "\\" + MaxDirectory, "*.pdf"))
                                    foreach (var items in Directory.GetFiles(_Directory + "\\" + item.Name + "\\" + MaxDirectory, "*.pdf"))
                                    {
                                        //if (items.Contains(" "))
                                        //{
                                        //    FileName = items.Replace(_Directory + item.Name + "\\" + MaxDirectory + "\\", "");
                                        //}

                                        var GetFileName = items.Replace(_Directory + "\\" + item.Name + "\\" + MaxDirectory + "\\", "");
                                        GetFileName = GetFileName.Replace("EN_", "");
                                        if (GetFileName.Contains(" "))
                                        {
                                            FileName = GetFileName;
                                        }
                                    }

                                    _DirectoryStatus.Add(new DirectoryStatus
                                    {
                                        DirectoryName = item.Name,
                                        DirectoryMax = MaxDirectory,
                                        FileName = FileName,
                                        Directorys = strings,
                                        IsSelect = false
                                    });
                                }
                            }
                        }
                        //List<string> dirs = Directory.GetDirectories(System.IO.Directory.GetCurrentDirectory() + @"\wwwroot\esop\" + item.Name).ToList().OrderByDescending(x => x).ToList();
                    }
                }

                if (DirectoryName == @"\\172.25.210.8\01.product$\411 Standard Operation Procedure")
                {
                    _DirectoryItem = _DirectoryStatus;
                    _FolderItem = _FolderStatus;
                }

                return Task.FromResult(new Tuple<List<DirectoryStatus>, List<FolderStatus>>(_DirectoryStatus, _FolderStatus));
            }
        }

        public Task<List<ESOPFolderItem>> GetESOPFolderStatusAsync(string DirectoryName)
        {
            List<ESOPFolderItem> _ESOPFolder = new List<ESOPFolderItem>();
            string _Directory = @DirectoryName;

            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var _DirectoryReferenceTable = _DSDBContext.DirectoryReferenceTables.ToList();

                if (Directory.Exists(_Directory))
                {
                    DirectoryInfo dir = new DirectoryInfo(_Directory);
                    foreach (DirectoryInfo item in dir.GetDirectories())
                    {
                        if (!item.Name.Contains("_INA"))
                        {
                            if (_DirectoryReferenceTable.Where(x => x.DirectoryName == item.Name).Count() == 0)
                            {
                                _ESOPFolder.Add(new ESOPFolderItem
                                {
                                    ESOPFolderName = item.Name
                                });
                            }
                        }
                    }
                }
            }
            
            return Task.FromResult(_ESOPFolder);
        }

        private List<ManufactureStatus> _ManufactureStatus = new List<ManufactureStatus>();
        public Task<List<ManufactureStatus>> GetManufactureStatusAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _ManufactureStatus = _DSDBContext.ManufactureStatuss.ToList();
                return Task.FromResult(_ManufactureStatus);
            }
        }

        private List<WorkingOrderList> _WorkingOrderLists = new List<WorkingOrderList>();

        public Task<List<WorkingOrderList>> GetWorkingOrderListAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _WorkingOrderLists = _DSDBContext.WorkingOrderLists.ToList();
                return Task.FromResult(_WorkingOrderLists);
            }
        }

        public Task<List<WorkingOrderList>> GetWorkingOrderListAsync(string OPID)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _WorkingOrderLists = _DSDBContext.WorkingOrderLists.Where(x => x.OPNO == OPID).ToList();
                return Task.FromResult(_WorkingOrderLists);
            }
        }

        #region 20241129 找出WorkingOrderList裡面WorkerOrder不是15碼的資料

        public async Task<List<WorkingOrderList>> GetWorkingOrderListWrongLengthOfWorkOrderAsync()
        {
            try
            {
                using (var scope = services.CreateScope())
                {
                    DSDBContext context = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                    return await context.WorkingOrderLists.Where(x => x.WorkOrder != null && x.WorkOrder.Length != 15).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                return new List<WorkingOrderList>();
            }
        }

        public async Task UpdateWorkingOrderListAndStepListAsync(List<WorkingOrderList> lists)
        {
            foreach(WorkingOrderList list in lists)
            {
                Task<List<WorkingOrderList>> _WorkingOrderListTask = _getWorkingOrderListByIdAsync((int)list.ID);
                Task<List<WorkingOrderStepList>> _workingOrderStepListTask = _getWorkingOrderStepListByIdAsync((int)list.ID);

                List<WorkingOrderList> _workingOrderList = await _WorkingOrderListTask;
                List<WorkingOrderStepList> _workingOrderStepList = await _workingOrderStepListTask;

                if (_workingOrderList.Count != 0)
                {
                    await _setWorkingOrderListAsync(_workingOrderList, list.WorkOrder);
                }

                if(_workingOrderStepList.Count != 0)
                {
                    await _setWorkingOrderStepListAsync(_workingOrderStepList, list.WorkOrder);
                }
            }
        }

        async Task<List<WorkingOrderList>> _getWorkingOrderListByIdAsync(int id)
        {
            try
            {
                using (var scope = services.CreateScope())
                {
                    DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                    return await _DSDBContext.WorkingOrderLists.Where(x => x.ID == id).ToListAsync();
                }
            }
            catch(Exception ex)
            {
                return new List<WorkingOrderList>();
            }
        }

        async Task _setWorkingOrderListAsync(List<WorkingOrderList> lists, string WorkOrder = "")
        {
            try
            {
                using (var scope = services.CreateScope())
                {
                    DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();

                    foreach (WorkingOrderList list in lists)
                    {
                        list.WorkOrder = WorkOrder;
                        _DSDBContext.Update(list);
                    }

                    await _DSDBContext.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                return;
            }
        }

        async Task<List<WorkingOrderStepList>> _getWorkingOrderStepListByIdAsync(int id)
        {
            try
            {
                using (var scope = services.CreateScope())
                {
                    DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                    return await _DSDBContext.WorkingOrderStepLists.Where(x => x.WOID == id).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                return new List<WorkingOrderStepList>();
            }
        }

        async Task _setWorkingOrderStepListAsync(List<WorkingOrderStepList> lists, string WorkOrder = "")
        {
            try
            {
                using (var scope = services.CreateScope())
                {
                    DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();

                    foreach (WorkingOrderStepList list in lists)
                    {
                        list.WorkOrder = WorkOrder;
                        _DSDBContext.Update(list);
                    }

                    await _DSDBContext.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                return;
            }
        }

        #endregion

        #region 20241203 修改在指定的WorkOrder中的WorkOrderCount

        public async Task<List<WorkingOrderList>> GetWorkingOrderListByWorkOrderAsync(string WorkOrder)
        {
            try
            {
                using (var scope = services.CreateScope())
                {
                    DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                    return await _DSDBContext.WorkingOrderLists.Where(x => x.WorkOrder == WorkOrder).ToListAsync();
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task UpdateWorkingOrderCountListAsync(List<WorkingOrderList> lists)
        {
            foreach(WorkingOrderList list in lists)
            {
                List<WorkingOrderList> _workingOrderList = await _getWorkingOrderListByIdAsync((int)list.ID);

                if (_workingOrderList.Count != 0)
                {
                    await _setWorkingOrderCountListAsync(_workingOrderList, (double)list.PassCount, (double)list.FailCount);
                }
            }
        }

        async Task _setWorkingOrderCountListAsync(List<WorkingOrderList> Lists, double passCount, double failCount)
        {
            try
            {
                using (var scope = services.CreateScope())
                {
                    DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();

                    foreach(WorkingOrderList list in Lists)
                    {
                        list.PassCount = passCount;
                        list.FailCount = failCount;
                        _DSDBContext.Update(list);
                    }

                    await _DSDBContext.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                return;
            }
        }

        #endregion

        #region 20241217 找出WorkingOrderList裡面有工作接收的資料

        public Task<(List<WorkingOrderList> list, string status)> GetWorkingReceiveListFromWorkingOrderlistAsync()
        {
            try
            {
                using (var scope = services.CreateScope())
                {
                    DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                    List<WorkingOrderList> result = _DSDBContext.WorkingOrderLists.AsNoTracking().Where(x => x.IsReceive == true && x.ReceiveTime != null).ToList();
                    return Task.FromResult((result, "抓取WorkingOrderList的時候是正常狀態"));
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult((new List<WorkingOrderList>(), ex.ToString()));
            }
        }

        #endregion

        #region 20250306 ProcessTypeTables資料表
        public async Task<ProcessTypeTable> getProcessTypeTableAsync(string processType, string stationName)
        {
            try
            {
                using (var scope = services.CreateScope())
                {
                    DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();

                    List<ProcessTypeTable> list = await _DSDBContext.ProcessTypeTables.AsNoTracking()
                                                                                      .Where(x => x.StationName == stationName)
                                                                                      .ToListAsync();

                    foreach(ProcessTypeTable type in list)
                    {
                        if (type.ProcessType.Split(',').Contains(processType))
                        {
                            return type;
                        }
                    }

                    return null;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<ProcessTypeTable> getProcessTypeTableAsync_v2(string processType, int sorting)
        {
            try
            {
                using (var scope = services.CreateScope())
                {
                    DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();

                    List<ProcessTypeTable> list = await _DSDBContext.ProcessTypeTables.AsNoTracking()
                                                                                      .Where(x => x.Sorting == sorting)
                                                                                      .ToListAsync();

                    foreach (ProcessTypeTable type in list)
                    {
                        if (type.ProcessType.Split(',').Contains(processType))
                        {
                            return type;
                        }
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 20241216 JointModuleTables資料表

        public async Task<List<JointModuleTables>> GetJointModuleTablesList()
        {
            try
            {
                using (var scope = services.CreateScope())
                {
                    DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();

                    List<JointModuleTables> list = await _DSDBContext.JointModuleTables.ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region 20241225 QuantityByWorkOrder 利用Stored Procedure去撈TMMainDB的PP_WorkOrder資料表

        public async Task<QuantityByWorkOrder_ERP> GetQuantityByWorkerOrder(string param1)
        {
            try
            {
                using (var scope = services.CreateScope())
                {
                    DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                    List<QuantityByWorkOrder_ERP> list = await _DSDBContext.QuantityByWorkOrders.FromSqlInterpolated($"sp_QuantityByWorkOrder {param1}").ToListAsync();

                    if (list.Count != 0)
                        return list.First();
                    else
                        return null;

                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<TypeByWorkOrder> GetTypeByWorkOrder(string workOrder)
        {
            try
            {
                using (var scope = services.CreateScope())
                {
                    DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();

                    List<TypeByWorkOrder> list = await _DSDBContext.TypeByWorkOrders.FromSqlInterpolated($"sp_TypeByWorkOrder {workOrder}").ToListAsync();

                    if(list.Count != 0 && list.First().ProcessType != null)
                    {
                        return list.First();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        #endregion

        private List<WorkingOrderStepList> _WorkingOrderStepLists = new List<WorkingOrderStepList>();

        public Task<List<WorkingOrderStepList>> GetWorkingOrderStepListAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _WorkingOrderStepLists = _DSDBContext.WorkingOrderStepLists.ToList();
                return Task.FromResult(_WorkingOrderStepLists);
            }
        }

        public Task<List<WorkingOrderStepList>> GetWorkingOrderStepListAsync(string OPID)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _WorkingOrderStepLists = _DSDBContext.WorkingOrderStepLists.Where(x => x.OPNO == OPID).ToList();
                return Task.FromResult(_WorkingOrderStepLists);
            }
        }

        public Task<List<WorkingOrderList>> UpdateWorkingOrderListAsync(WorkingOrderList WorkingOrderListItem)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                WorkingOrderList _WorkingOrderList = _DSDBContext.WorkingOrderLists.SingleOrDefault(x => x.ID == WorkingOrderListItem.ID);
                if (_WorkingOrderList == null)
                {
                    _DSDBContext.WorkingOrderLists.Add(WorkingOrderListItem);
                }
                else
                {
                    _WorkingOrderList.WorkOrderCount = WorkingOrderListItem.WorkOrderCount;
                    _WorkingOrderList.PassCount = WorkingOrderListItem.PassCount;
                    _WorkingOrderList.FailCount = WorkingOrderListItem.FailCount;
                    _WorkingOrderList.IsReceive = WorkingOrderListItem.IsReceive;
                    _WorkingOrderList.ReceiveTime = WorkingOrderListItem.ReceiveTime;
                    _WorkingOrderList.IsPause = WorkingOrderListItem.IsPause;
                    _WorkingOrderList.PauseTime = WorkingOrderListItem.PauseTime;
                    _WorkingOrderList.IsResponse = WorkingOrderListItem.IsResponse;
                    _WorkingOrderList.ResponseTime = WorkingOrderListItem.ResponseTime;
                    _DSDBContext.WorkingOrderLists.Update(_WorkingOrderList);
                }
                _DSDBContext.SaveChanges();
                _WorkingOrderLists = _DSDBContext.WorkingOrderLists.ToList();
                return Task.FromResult(_WorkingOrderLists);
            }
        }

        public Task<List<WorkingOrderStepList>> UpdateWorkingOrderStepListAsync(WorkingOrderList WorkingOrderListItem)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();

                WorkingOrderStepList WorkingOrderStepListItem = new WorkingOrderStepList();
                WorkingOrderStepListItem.WorkingTime = DateTime.Now;
                WorkingOrderStepListItem.StationName = WorkingOrderListItem.StationName;
                WorkingOrderStepListItem.WorkOrder = WorkingOrderListItem.WorkOrder;
                WorkingOrderStepListItem.SerialNumber = WorkingOrderListItem.SerialNumber;
                WorkingOrderStepListItem.OPNO = WorkingOrderListItem.OPNO;
                if (WorkingOrderListItem.IsReceive == false && WorkingOrderListItem.IsPause == false && WorkingOrderListItem.IsResponse == false)
                {
                    WorkingOrderStepListItem.WorkingStatus = "作業已新增";
                    WorkingOrderList _WorkingOrderList = _DSDBContext.WorkingOrderLists.SingleOrDefault(x => x.AddTime == WorkingOrderListItem.AddTime);
                    if (_WorkingOrderList != null)
                    {
                        WorkingOrderStepListItem.WOID = _WorkingOrderList.ID;
                    }
                    else
                    {
                        WorkingOrderStepListItem.WOID = 0;
                    }
                    WorkingOrderStepListItem.StatusTime = WorkingOrderListItem.AddTime;
                }
                if (WorkingOrderListItem.IsReceive == true)
                {
                    WorkingOrderStepListItem.WorkingStatus = "作業已接收";
                    WorkingOrderStepListItem.WOID = WorkingOrderListItem.ID;
                    WorkingOrderStepListItem.StatusTime = DateTime.Now;
                }
                if (WorkingOrderListItem.IsPause == true)
                {
                    WorkingOrderStepListItem.WorkingStatus = "作業已暫停";
                    WorkingOrderStepListItem.WOID = WorkingOrderListItem.ID;
                    WorkingOrderStepListItem.StatusTime = DateTime.Now;
                }
                if (WorkingOrderListItem.IsResponse == true)
                {
                    WorkingOrderStepListItem.WorkingStatus = "作業已結束";
                    WorkingOrderStepListItem.WOID = WorkingOrderListItem.ID;
                    WorkingOrderStepListItem.StatusTime = WorkingOrderListItem.ResponseTime;
                }
                WorkingOrderStepListItem.ReasonType = "";
                WorkingOrderStepListItem.CreateUser = WorkingOrderListItem.CreateUser;

                //WorkingOrderStepList _WorkingOrderStepList = _DSDBContext.WorkingOrderStepLists.SingleOrDefault(x => x.ID == WorkingOrderStepListItem.ID);
                //if (_WorkingOrderStepList == null)
                //{
                _DSDBContext.WorkingOrderStepLists.Add(WorkingOrderStepListItem);
                //}
                //else
                //{
                //    _DSDBContext.WorkingOrderStepLists.Update(_WorkingOrderStepList);
                //}
                _DSDBContext.SaveChanges();
                _WorkingOrderStepLists = _DSDBContext.WorkingOrderStepLists.ToList();
                return Task.FromResult(_WorkingOrderStepLists);
            }
        }

        public Task RemoveWorkingOrderListAsync(WorkingOrderList item)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                WorkingOrderList _WorkingOrderList = _DSDBContext.WorkingOrderLists.SingleOrDefault(x => x.ID == item.ID);
                if (_WorkingOrderList != null)
                {
                    _DSDBContext.WorkingOrderLists.Remove(_WorkingOrderList);
                }
                List<WorkingOrderStepList> _WorkingOrderStepLists = _DSDBContext.WorkingOrderStepLists.Where(x => x.WOID == item.ID).ToList();
                if (_WorkingOrderStepLists != null)
                {
                    _DSDBContext.WorkingOrderStepLists.RemoveRange(_WorkingOrderStepLists);
                }
                _DSDBContext.SaveChanges();
            }
            return Task.FromResult(true);
        }

        private List<ComparisonTable> _ComparisonTables = new List<ComparisonTable>();

        public Task<List<ComparisonTable>> GetComparisonTableAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _ComparisonTables = _DSDBContext.ComparisonTables.ToList();
                return Task.FromResult(_ComparisonTables);
            }
        }

        public Task<List<ComparisonTable>> ChangeComparisonTableAsync(ComparisonTable changed)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _ComparisonTables = _DSDBContext.ComparisonTables.ToList();
                var orginalForecast = _ComparisonTables.FirstOrDefault(i => i.ID == changed.ID);
                if (orginalForecast != null)
                {
                    orginalForecast.ProcessName = changed.ProcessName;
                    orginalForecast.StationName = changed.StationName;
                    _DSDBContext.ComparisonTables.Update(orginalForecast);
                }
                else
                {
                    _DSDBContext.ComparisonTables.Add(changed);
                    _ComparisonTables.Add(changed);
                }
                _DSDBContext.SaveChanges();
                return Task.FromResult(_ComparisonTables);
            }
        }

        public Task<List<ComparisonTable>> Remove(ComparisonTable forecast)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                ComparisonTable table = _DSDBContext.ComparisonTables.FirstOrDefault(x => x == forecast);
                ComparisonTable localTable = _ComparisonTables.FirstOrDefault(x => x == forecast);

                if (table != null)
                {
                    _DSDBContext.ComparisonTables.Remove(table);
                    _DSDBContext.SaveChanges();
                }

                if(localTable != null)
                {
                    _ComparisonTables.Remove(forecast);
                }

                return Task.FromResult(_ComparisonTables);
            }
        }

        public Task<List<DirectoryReferenceTable>> GetDirectoryReferenceTablesAsync()
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                var item = _DSDBContext.DirectoryReferenceTables.ToList();
                return Task.FromResult(item);
            }
        }

        public Task<List<DirectoryReferenceTable>> SetDirectoryReferenceTablesAsync(DirectoryReferenceTable Item)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                DirectoryReferenceTable DirectoryReferenceTableItem = _DSDBContext.DirectoryReferenceTables.SingleOrDefault(x => x.DirectoryName == Item.DirectoryName);
                if (DirectoryReferenceTableItem == null)
                {
                    var NewDirectoryReferenceTableItem = new DirectoryReferenceTable();
                    NewDirectoryReferenceTableItem.DirectoryName = Item.DirectoryName;
                    NewDirectoryReferenceTableItem.DisplayName = Item.DisplayName;
                    _DSDBContext.DirectoryReferenceTables.Add(NewDirectoryReferenceTableItem);
                }
                else
                {
                    DirectoryReferenceTableItem.DisplayName = Item.DisplayName;
                    _DSDBContext.DirectoryReferenceTables.Update(DirectoryReferenceTableItem);
                }
                _DSDBContext.SaveChanges();
                var item = _DSDBContext.DirectoryReferenceTables.ToList();
                return Task.FromResult(item);
            }
        }

        public Task<List<DirectoryReferenceTable>> Remove(DirectoryReferenceTable forecast)
        {
            using (var scope = services.CreateScope())
            {
                DSDBContext _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                _DSDBContext.DirectoryReferenceTables.Remove(forecast);
                _DSDBContext.SaveChanges();
                var item = _DSDBContext.DirectoryReferenceTables.ToList();
                return Task.FromResult(item);
            }
        }
    }
}
