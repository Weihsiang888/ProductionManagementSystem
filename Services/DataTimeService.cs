using DevExpress.Utils.Drawing;
using DxBlazorApplication7.Data;
using System.Diagnostics.Eventing.Reader;
using System.IO;

namespace DxBlazorApplication7.Services
{
    public partial class DataTimeService : BackgroundService
    {
        private readonly IServiceProvider _ServiceProvider;
        private readonly ILogger<DataTimeService> _logger;
        private readonly PeriodicTimer _timer;
        //DataService _DataService;
        DataLogService _DataLogService = new DataLogService();

        //public DataTimeService(DataService dataService, DataLogService dataLogService, ILogger<DataTimeService> logger)
        public DataTimeService(IServiceProvider serviceProvider, ILogger<DataTimeService> logger)
        {
            _ServiceProvider = serviceProvider;
            //_DataService = new DataService(_ServiceProvider);
            //_DataService = _ServiceProvider.GetRequiredService<DataService>();
            //_DataLogService = dataLogService;
            //_timer = new(TimeSpan.FromMinutes(15));
            _DataLogService.WriteEventLog("Code", "Program Start");
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _ServiceProvider.CreateScope();
            var _DataService = scope.ServiceProvider.GetRequiredService<DataService>();

            //_DataLogService.WriteEventLog("Code", "Program Start");
            _DataLogService.WriteEventLog("Code", "Check OvertimeSchedules" + "|" + stoppingToken.IsCancellationRequested.ToString());
            _DataLogService.WriteEventLog("Code", "Check PausetimeSchedules" + "|" + stoppingToken.IsCancellationRequested.ToString());
            // && !stoppingToken.IsCancellationRequested
            while (!stoppingToken.IsCancellationRequested)
            {
                int NowHour = DateTime.Now.Hour;
                DateTime NowDateTime = DateTime.Now;

                _DataLogService.WriteEventLog("Code", "Check OvertimeSchedules" + "|" + NowHour.ToString());

                try
                {
                    if (18 <= NowHour && NowHour < 19)
                    {
                        _DataLogService.WriteEventLog("Code", "Check OvertimeSchedules" + "|" + "Now Hour" + NowHour.ToString());
                        List<WorkingList> _WorkingLists = await _DataService.GetWorkingListsTimeAsync();
                        List<OvertimeSchedule> _OvertimeSchedules = await _DataService.GetOvertimeSchedulesAsync();

                        foreach (var item in _WorkingLists)
                        {
                            int _Day = Convert.ToInt32((DateTime.Now.Date - Convert.ToDateTime(item.ReceiveTime).Date).TotalDays);
                            //if (Convert.ToInt32((DateTime.Now.Date - Convert.ToDateTime(item.ReceiveTime).Date).TotalDays) >= 0)
                            //{
                            var OvertimeSchedulesItem = _OvertimeSchedules.FirstOrDefault(x => x.OPNO == item.OPNO && x.OverTimeDate == DateTime.Today);

                            if (OvertimeSchedulesItem == null)
                            {
                                item.IsPause = true;
                                item.PauseTime = DateTime.Now;
                                item.IsReceive = false;
                                _DataService.UpdateWorkingListsAsync(item);
                                ReasonType ReasonTypeValue = new ReasonType();
                                ReasonTypeValue.ReasonCode = "SystemAutoPause";
                                ReasonTypeValue.ReasonName = "SystemAutoPause";
                                await _DataService.ChangeWorkingPauseTimeListsAsync((WorkingList)item, "Pause", item.IsReceive, item.IsPause, item.IsResponse, ReasonTypeValue);
                                _DataLogService.WriteEventLog("Code", "Check OvertimeSchedules" + "|" + NowHour + "|" + item.OPNO + "|" + "IsPause" + "," + item.IsPause);
                            }
                            //foreach (var items in _DataService._OvertimeSchedules)
                            //{
                            //    //if (Convert.ToInt32((DateTime.Now.Date - Convert.ToDateTime(items.OverTimeDate).Date).TotalDays) == 0)
                            //    //{
                            //    if (item.OPNO != items.OPNO)
                            //    {
                            //        item.IsPause = true;
                            //        item.PauseTime = DateTime.Now;
                            //        item.IsReceive = false;
                            //    }
                            //    //}
                            //}
                            //}
                        }

                        //_DataService.UpdateWorkingListsAsync();
                    }
                }
                catch (Exception ex)
                {
                    _DataLogService.WriteEventLog("Code", "Check OvertimeSchedules" + "|" + ex);
                }

                try
                {
                    if (22 <= NowHour && NowHour < 23)
                    {
                        _DataLogService.WriteEventLog("Code", "Check OvertimeSchedules" + "|" + "Now Hour" + NowHour.ToString());
                        List<WorkingList> _WorkingLists = await _DataService.GetWorkingListsTimeAsync();
                        List<OvertimeSchedule> _OvertimeSchedules = await _DataService.GetOvertimeSchedulesAsync();

                        foreach (var item in _WorkingLists)
                        {
                            int _Day = Convert.ToInt32((DateTime.Now.Date - Convert.ToDateTime(item.ReceiveTime).Date).TotalDays);
                            //if (Convert.ToInt32((DateTime.Now.Date - Convert.ToDateTime(item.ReceiveTime).Date).TotalDays) >= 0)
                            //{
                            foreach (var items in _OvertimeSchedules.Where(x => (x.OverTimeType == "8" || x.OverTimeType == "9" || x.OverTimeType == "10" || x.OverTimeType == "11") && x.OverTimeDate == DateTime.Today))
                            {
                                //if (Convert.ToInt32((DateTime.Now.Date - Convert.ToDateTime(items.OverTimeDate).Date).TotalDays) == 0)
                                //{
                                if (item.OPNO == items.OPNO)
                                {
                                    item.IsPause = true;
                                    item.PauseTime = DateTime.Now;
                                    item.IsReceive = false;
                                    _DataService.UpdateWorkingListsAsync(item);
                                    ReasonType ReasonTypeValue = new ReasonType();
                                    ReasonTypeValue.ReasonCode = "SystemAutoPause";
                                    ReasonTypeValue.ReasonName = "SystemAutoPause";
                                    await _DataService.ChangeWorkingPauseTimeListsAsync((WorkingList)item, "Pause", item.IsReceive, item.IsPause, item.IsResponse, ReasonTypeValue);
                                    _DataLogService.WriteEventLog("Code", "Check OvertimeSchedules" + "|" + NowHour + "|" + item.OPNO + "|" + "IsPause" + "," + item.IsPause);
                                }
                                //}
                            }
                            //}
                        }

                        //_DataService.UpdateWorkingListsAsync();
                    }
                }
                catch (Exception ex)
                {
                    _DataLogService.WriteEventLog("Code", "Check OvertimeSchedules" + "|" + ex);
                }

                try
                {
                    DateTime pauseTime;

                    _DataLogService.WriteEventLog("Code", "Check PausetimeSchedules" +
                                                  " | " + NowDateTime.Hour.ToString() +
                                                  ":" + NowDateTime.Minute.ToString());

                    if (IsRequestTimeRange(NowDateTime, out pauseTime))
                    {
                        _DataLogService.WriteEventLog("Code", "確認已到達暫停時間");
                        await WorkingReceivePause(pauseTime);
                        _CheckIsUpdatePause = true;
                    }
                }
                catch (Exception ex)
                {
                    _DataLogService.WriteEventLog("Code", "Check PausetimeSchedules" + "|" + ex.ToString());
                }

                if(IsIntensiveCheckTimeRange(NowDateTime))
                {
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                else
                {
                    await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken); //要改回15分鐘
                    _CheckIsUpdatePause = false;
                }
            }
        }
    }

    public partial class DataTimeService
    {
        enum ETimePoint
        {
            Range1PreStart, Range1Start, Range1End,
            Range2PreStart, Range2Start, Range2End,
            Range3PreStart, Range3Start, Range3End
        }

        static Dictionary<ETimePoint, TimeSpan> dcTimeSpan = new Dictionary<ETimePoint, TimeSpan>()
        {
            { ETimePoint.Range1PreStart, new TimeSpan(12, 10, 0) }, // 12:10
            { ETimePoint.Range1Start, new TimeSpan(12, 30, 0) }, //12:30
            { ETimePoint.Range1End, new TimeSpan(12, 40, 0) }, //12:40
            { ETimePoint.Range2PreStart, new TimeSpan(17, 10, 0) },
            { ETimePoint.Range2Start, new TimeSpan(17, 30, 0) },
            { ETimePoint.Range2End, new TimeSpan(17, 40, 0) },
            { ETimePoint.Range3PreStart, new TimeSpan(19, 40, 0) },
            { ETimePoint.Range3Start, new TimeSpan(20, 0, 0) },
            { ETimePoint.Range3End, new TimeSpan(20, 10, 0) },
        };

        bool _CheckIsUpdatePause { get; set; } = false;

        bool IsIntensiveCheckTimeRange(DateTime now)
        {
            DateTime today = now.Date;

            if (_CheckIsUpdatePause)
            {
                return false;
            }
            else if(today.Add(dcTimeSpan[ETimePoint.Range1PreStart]) <= now && now <= today.Add(dcTimeSpan[ETimePoint.Range1End]))
            {
                return true;
            }
            else if(today.Add(dcTimeSpan[ETimePoint.Range2PreStart]) <= now && now <= today.Add(dcTimeSpan[ETimePoint.Range2End]))
            {
                return true;
            }
            else if(today.Add(dcTimeSpan[ETimePoint.Range3PreStart]) <= now && now <= today.Add(dcTimeSpan[ETimePoint.Range3End]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool IsRequestTimeRange(DateTime now, out DateTime pause)
        {
            DateTime today = now.Date;

            if (today.Add(dcTimeSpan[ETimePoint.Range1Start]) <= now && now <= today.Add(dcTimeSpan[ETimePoint.Range1End]))
            {
                pause = today.Add(dcTimeSpan[ETimePoint.Range1Start]);
                return true;
            }
            else if (today.Add(dcTimeSpan[ETimePoint.Range2Start]) <= now && now <= today.Add(dcTimeSpan[ETimePoint.Range2End]))
            {
                pause = today.Add(dcTimeSpan[ETimePoint.Range2Start]);
                return true;
            }
            else if (today.Add(dcTimeSpan[ETimePoint.Range3Start]) <= now && now <= today.Add(dcTimeSpan[ETimePoint.Range3End]))
            {
                pause = today.Add(dcTimeSpan[ETimePoint.Range3Start]);
                return true;
            }
            else
            {
                pause = DateTime.Now;
                return false;
            }
        }

        public delegate Task dgUpdateStatus();
        public static dgUpdateStatus dgUpdateWorkingReceivePauseStatus;

        async Task WorkingReceivePause(DateTime pauseTime)
        {
            try
            {
                using var scope = _ServiceProvider.CreateScope();
                var _DataService = scope.ServiceProvider.GetRequiredService<DataService>();

                var result = await _DataService.GetWorkingReceiveListFromWorkingOrderlistAsync();

                List<WorkingOrderList> list = result.list;
                _DataLogService.WriteEventLog("Code", result.status);

                if (list.Count != 0)
                {
                    _DataLogService.WriteEventLog("Code", "有抓到需要暫停的資料");

                    foreach (WorkingOrderList item in list)
                    {
                        if (item.IsReceive == true)
                        {
                            item.IsReceive = false;
                            item.ReceiveTime = null;
                            item.IsPause = true;
                            item.PauseTime = pauseTime;
                            await Task.Delay(100);
                            await _DataService.UpdateWorkingOrderListAsync(item);
                            _DataLogService.WriteEventLog("Code", "更新WorkingOrderList");
                            await Task.Delay(100);
                            await _DataService.UpdateWorkingOrderStepListAsync(item);
                            _DataLogService.WriteEventLog("Code", "更新WorkingOrderStepList");
                        }
                    }

                    dgUpdateWorkingReceivePauseStatus?.Invoke();

                    _DataLogService.WriteEventLog("Code", "更新UI");
                }
                else
                {
                    _DataLogService.WriteEventLog("Code", "沒有抓到需要暫停的資料");
                }
            }
            catch (Exception ex)
            {
                _DataLogService.WriteEventLog("Code", ex.ToString());
            }

        }
    }
}
