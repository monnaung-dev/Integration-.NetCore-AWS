var dashboard = {
    monthNames: ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ],

    monthlyFilter: 1,
    monthLabel: null,
    prevMonthBtn: null,
    nextMonthBtn: null,
    monthLoadingOverlay: null,

    newUsers: {
        countLabel: null,
        percentLabel: null
    },

    newTrans: {
        countLabel: null,
        percentLabel: null
    },

    newRevenues: {
        countLabel: null,
        percentLabel: null
    },

    newOccu: {
        countLabel: null,
        percentLabel: null
    },

    fineSettlements: {
        fineLabel: null,
        settlementLabel: null
    },

    lotSummaryChart: '',
    customerReviewChart: '',

    durationTrend: {
        chart: '',
        today: true,
        btnToday: null,
        btnYesterday: null,
        loadingOverlay: null
    },

    occuPercentage: {
        chart: '',
        today: true,
        btnToday: null,
        btnYesterday: null,
        loadingOverlay: null
    },
    
    occuTrend: {
        chart: '',
        today: true,
        btnToday: null,
        btnYesterday: null,
        loadingOverlay: null
    },

    getDate(today) {
        var datestr = '';
        if (today) {
            var todayD = new Date();
            var dd = String(todayD.getDate()).padStart(2, '0');
            var mm = String(todayD.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = todayD.getFullYear();

            datestr = dd + '-' + mm + '-' + yyyy;

        } else {
            var yesterday = new Date();
            yesterday.setDate(yesterday.getDate() - 1);
            var dd = String(yesterday.getDate()).padStart(2, '0');
            var mm = String(yesterday.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = yesterday.getFullYear();

            datestr = dd + '-' + mm + '-' + yyyy;

        }
        return datestr;
    },

    initMonthButton(btnPrev, BntNext, monthLabel, monthLoadingOverlay) {
        this.prevMonthBtn = btnPrev;
        this.nextMonthBtn = BntNext;
        this.monthLabel = monthLabel;
        this.monthLoadingOverlay = monthLoadingOverlay;

        const d = new Date();
        this.monthlyFilter = d.getMonth();
        
        $(this.monthLabel).text('Monthly Summary (' + this.monthNames[this.monthlyFilter] + ')');

        $(this.prevMonthBtn).on('click', function () {
            if (dashboard.monthlyFilter >= 1) {
                $(dashboard.monthLoadingOverlay).LoadingOverlay("show");
                dashboard.monthlyFilter = dashboard.monthlyFilter - 1;

                var serverMonth = dashboard.monthlyFilter + 1;

                dashboard.LoadNewUsers(serverMonth);
                dashboard.LoadNewTrans(serverMonth);
                dashboard.LoadNewRevenues(serverMonth);
                dashboard.LoadNewOccu(serverMonth);
                dashboard.LoadFineAndSettlements(serverMonth);

                dashboard.LoadLotSummary();
                dashboard.LoadCustomerReviewSummary();

                $(dashboard.monthLoadingOverlay).LoadingOverlay("hide", true);
                $(dashboard.monthLabel).text('Monthly Summary (' + dashboard.monthNames[dashboard.monthlyFilter] + ')');
            }
            
        });


        $(this.nextMonthBtn).on('click', function () {
            if (dashboard.monthlyFilter < 11) {
                $(dashboard.monthLoadingOverlay).LoadingOverlay("show");

                dashboard.monthlyFilter = dashboard.monthlyFilter + 1;

                var serverMonth = dashboard.monthlyFilter + 1;

                dashboard.LoadNewUsers(serverMonth);
                dashboard.LoadNewTrans(serverMonth);
                dashboard.LoadNewRevenues(serverMonth);
                dashboard.LoadNewOccu(serverMonth);
                dashboard.LoadFineAndSettlements(serverMonth);

                dashboard.LoadLotSummary();
                dashboard.LoadCustomerReviewSummary();
                $(dashboard.monthLoadingOverlay).LoadingOverlay("hide", true);
                $(dashboard.monthLabel).text('Monthly Summary (' + dashboard.monthNames[dashboard.monthlyFilter] + ')');
            }

        });
    },

    initNewUsers(countLabel, percentLabel) {
        this.newUsers = { countLabel: countLabel, percentLabel: percentLabel };
        $(this.newUsers.countLabel).text('0');
        $(this.newUsers.percentLabel).text('0% from prior month');
    },

    initNewTrans(countLabel, percentLabel) {
        this.newTrans = { countLabel: countLabel, percentLabel: percentLabel };
        $(this.newTrans.countLabel).text('0');
        $(this.newTrans.percentLabel).text('0% from prior month');
    },

    initNewRevenues(countLabel, percentLabel) {
        this.newRevenues = { countLabel: countLabel, percentLabel: percentLabel };
        $(this.newRevenues.countLabel).text('0');
        $(this.newRevenues.percentLabel).text('0% from prior month');
    },

    initNewOccu(countLabel, percentLabel) {
        this.newOccu = { countLabel: countLabel, percentLabel: percentLabel };
        $(this.newOccu.countLabel).text('0');
        $(this.newOccu.percentLabel).text('0% from prior month');
    },

    initFineAndSettlement(fineLabel, settlementLabel) {
        this.fineSettlements = { fineLabel: fineLabel, settlementLabel: settlementLabel };
        $(this.fineSettlements.fineLabel).text('0');
        $(this.fineSettlements.fineLabel).text('0');
    },

    initDurationTrend(chart, btnToday, btnYesterday, loadingOverlay) {
        this.durationTrend.chart = chart;
        this.durationTrend.btnToday = btnToday;
        this.durationTrend.btnYesterday = btnYesterday;
        this.durationTrend.loadingOverlay = loadingOverlay;

        $(this.durationTrend.btnToday).on('click', function () {
            dashboard.durationTrend.today = true;
            $(dashboard.durationTrend.loadingOverlay).LoadingOverlay("show"); 
            dashboard.LoadDurationTrend(dashboard.getDate(dashboard.durationTrend.today));
            $(dashboard.durationTrend.loadingOverlay).LoadingOverlay("hide", true);
        });

        $(this.durationTrend.btnYesterday).on('click', function () {
            dashboard.durationTrend.today = false;
            $(dashboard.durationTrend.loadingOverlay).LoadingOverlay("show");
            dashboard.LoadDurationTrend(dashboard.getDate(dashboard.durationTrend.today));
            $(dashboard.durationTrend.loadingOverlay).LoadingOverlay("hide", true);
        });
    },

    initOccuPercentage(chart, btnToday, btnYesterday, loadingOverlay) {
        this.occuPercentage.chart = chart;
        this.occuPercentage.btnToday = btnToday;
        this.occuPercentage.btnYesterday = btnYesterday;
        this.loadingOverlay = loadingOverlay;

        $(this.occuPercentage.btnToday).on('click', function () {
            dashboard.occuPercentage.today = true;
            $(dashboard.occuPercentage.loadingOverlay).LoadingOverlay("show");
            dashboard.LoadOccuPercentage(dashboard.getDate(dashboard.occuPercentage.today));

            $(dashboard.occuPercentage.loadingOverlay).LoadingOverlay("hide", true);
        });

        $(this.occuPercentage.btnYesterday).on('click', function () {
            dashboard.occuPercentage.today = false;
            $(dashboard.occuPercentage.loadingOverlay).LoadingOverlay("show");
            dashboard.LoadOccuPercentage(dashboard.getDate(dashboard.occuPercentage.today));
            $(dashboard.occuPercentage.loadingOverlay).LoadingOverlay("hide", true);
        });
    },

    initOccuTrend(chart, btnToday, btnYesterday, loadingOverlay) {
        this.occuTrend.chart = chart;
        this.occuTrend.btnToday = btnToday;
        this.occuTrend.btnYesterday = btnYesterday;
        this.occuTrend.loadingOverlay = loadingOverlay;

        $(this.occuTrend.btnToday).on('click', function () {
            dashboard.occuTrend.today = true;
            $(dashboard.occuTrend.loadingOverlay).LoadingOverlay("show");
            dashboard.LoadOccuTrend(dashboard.getDate(dashboard.occuTrend.today));
            $(dashboard.occuTrend.loadingOverlay).LoadingOverlay("hide", true);
        });

        $(this.occuTrend.btnYesterday).on('click', function () {
            dashboard.occuTrend.today = false;
            $(dashboard.occuTrend.loadingOverlay).LoadingOverlay("show");
            dashboard.LoadOccuTrend(dashboard.getDate(dashboard.occuTrend.today));
            $(dashboard.occuTrend.loadingOverlay).LoadingOverlay("hide", true);
        });
    },

    initLotSummary(chart) {
        this.lotSummaryChart = chart;
    },

    initCustomerReviews(chart) {
        this.customerReviewChart = chart;
    },

    LoadNewUsers(month) {
        spAPI.getNewUsers(month, function (result) {
            $(dashboard.newUsers.countLabel).text(result.count);
            $(dashboard.newUsers.percentLabel).text(result.percent + ' % from prior month');
        });
    },

    LoadNewTrans(month) {
        spAPI.getTrans(month, function (result) {
            $(dashboard.newTrans.countLabel).text(result.count);
            $(dashboard.newTrans.percentLabel).text(result.percent + ' % from prior month');
        });
    },

    LoadNewRevenues(month) {
        spAPI.getRevenues(month, function (result) {
            $(dashboard.newRevenues.countLabel).text(result.count);
            $(dashboard.newRevenues.percentLabel).text(result.percent + ' % from prior month');
        });
    },

    LoadNewOccu(month) {
        spAPI.getOccupancy(month, function (result) {
            $(dashboard.newOccu.countLabel).text(result.count);
            $(dashboard.newOccu.percentLabel).text(result.percent + ' % from prior month');
        });
    },

    LoadFineAndSettlements(month) {
        spAPI.getFinesAndSettlements(month, function (result) {
            $(dashboard.fineSettlements.fineLabel).text(result.fine);
            $(dashboard.fineSettlements.settlementLabel).text(result.settlement);
        });
    },

    

    LoadDurationTrend(date) {
        spAPI.getDailyDurationTrend(date, function (result) {
            
            var zones = [];

            var oneHours = [];
            var twoHours = [];
            var twoToFiveHours = [];
            var fiveHours = [];

            result.zones.forEach(function (zoneitem, index) {
                zones.push(zoneitem.name);
                zoneitem.hours.forEach(function (houritem, idx) {
                    if (houritem.name === '1 Hour') {
                        oneHours.push(parseInt(houritem.count));
                    } else if (houritem.name === '1-2 Hour') {
                        twoHours.push(parseInt(houritem.count));
                    } else if (houritem.name === '2-5 Hour') {
                        twoToFiveHours.push(parseInt(houritem.count));
                    } else if (houritem.name === '>5 Hour') {
                        fiveHours.push(parseInt(houritem.count));
                    }
                }); 
            });

            console.log(zones);

            ZC.LICENSE = ["569d52cefae586f634c54f86dc99e6a9", "b55b025e438fa8a98e32482b5f768ff5"];
            var myConfig = {
                type: "bar",
                plot: {
                    "bar-width": "8px",
                    "animation": {
                        "effect": "ANIMATION_SLIDE_BOTTOM"
                    },
                    "hover-state": {
                        "alpha": 1,
                        "visible": true
                    }
                },
                legend: {
                    margin: 'auto auto 1 auto',
                    backgroundColor: 'none',
                    borderWidth: '0px',
                    item: {
                        margin: '0px',
                        padding: '0px',
                        fontColor: '#000',
                        fontFamily: 'Arial',
                        fontSize: '14px',

                    },
                    layout: 'x4',
                    position: 'top',
                    shadow: false
                },
                scaleX: {
                    values: zones,
                    guide: {
                        visible: false
                    },
                    item: {
                        paddingTop: '2px',
                        fontColor: '#8391a5',
                        fontFamily: 'Arial',
                        fontSize: '11px',
                        "font-angle": -45,
                    },
                    itemsOverlap: true,
                    lineColor: '#d2dae2',
                    maxItems: 9999,
                    offsetY: '1px',
                    tick: {
                        lineColor: '#d2dae2',
                        visible: false
                    }
                },
                "series": [{
                    "values": oneHours,
                    backgroundColor: "#0066ff",
                    legendText: "1 Hour"
                },
                {
                    "values": twoHours,
                    backgroundColor: "#ff3300",
                    legendText: "1-2 Hours"
                },
                {
                    "values": twoToFiveHours,
                    backgroundColor: "#ff9900",
                    legendText: "2-5 Hours"
                },
                {
                    "values": fiveHours,
                    backgroundColor: "#00b33c",
                    legendText: ">5 Hours"
                }
                ],
            };
            zingchart.render({
                id: dashboard.durationTrend.chart,
                data: myConfig,
                height: 300,
                width: "100%"
            });
        });
    },

    LoadOccuTrend(date) {
        spAPI.getOccupancyTrend(date, function (result) {

            var times = [];
            var counts = [];
            
            result.parkingLots.forEach(function (timeItem, index) {
                times.push(timeItem.time);
                counts.push(timeItem.count);
            });

            console.log(times);

            ZC.LICENSE = ["569d52cefae586f634c54f86dc99e6a9", "b55b025e438fa8a98e32482b5f768ff5"];
            var myConfig = {
                graphset: [{
                    type: "bar",
                    height: "100%",
                    width: "100%",
                    x: "0%",
                    y: "0%",

                    plot: {
                        "alpha": 0.9,
                        "border-width": 1,
                        "stacked": true,
                        "border-color": "black",
                        "cursor": "pointer",
                        "border-radius-top-left": 4,
                        "border-radius-top-right": 4,
                        "bar-width": "18px",
                        "line-width": 2,
                        "bar-space": "20px",
                        "background-fit": "x",
                        "animation": {
                            "effect": "ANIMATION_SLIDE_BOTTOM"
                        },
                        "hover-state": {
                            "alpha": 1,
                            "visible": true
                        }
                    },

                    scaleX: {
                        values: times,
                        guide: {
                            visible: false
                        },
                        item: {
                            paddingTop: '2px',
                            fontColor: '#8391a5',
                            fontFamily: 'Arial',
                            fontSize: '11px',
                            "font-angle": -45,
                        },
                        itemsOverlap: true,
                        lineColor: '#d2dae2',
                        maxItems: 9999,
                        offsetY: '1px',
                        tick: {
                            lineColor: '#d2dae2',
                            visible: false
                        }
                    },
                    scaleY: {
                        "format": "%v%",
                        guide: {
                            rules: [{
                                lineWidth: '0px',
                                rule: '%i == 0'
                            },
                            {
                                alpha: 0.4,
                                lineColor: '#d2dae2',
                                lineStyle: 'solid',
                                lineWidth: '1px',
                                rule: '%i > 0'
                            }
                            ]
                        },
                        item: {
                            paddingRight: '5px',
                            fontColor: '#8391a5',
                            fontFamily: 'Arial',
                            fontSize: '10px',
                            "font-angle": -45,
                        },
                        lineColor: 'none',
                        maxItems: 4,
                        maxTicks: 4,
                        tick: {
                            visible: false
                        }
                    },
                    series: [{
                        values: counts,
                        backgroundColor: '#00b33c'
                    }]
                },
                ]
            };
            zingchart.render({
                id: dashboard.occuTrend.chart,
                data: myConfig,
                height: "300",
                width: "100%"
            });
        });
    },

    LoadOccuPercentage(date) {
        spAPI.getOccupancyPercentage(date, function (result) {

            var zones = [];
            var counts = [];
            
            result.parkingLots.forEach(function (zoneitem, index) {
                zones.push(zoneitem.name);
                counts.push(zoneitem.count);
            });

            console.log(zones);

            ZC.LICENSE = ["569d52cefae586f634c54f86dc99e6a9", "b55b025e438fa8a98e32482b5f768ff5"];
            var myConfig = {
                type: "hbar",
                plot: {
                    "alpha": 0.9,
                    "border-width": 1,
                    "stacked": true,
                    "border-color": "black",
                    "cursor": "pointer",
                    "border-radius-top-left": 4,
                    "border-radius-top-right": 4,
                    "bar-width": "30px",
                    "line-width": 2,
                    "bar-space": "20px",
                    "background-fit": "x",
                    "animation": {
                        "effect": "ANIMATION_SLIDE_BOTTOM"
                    },
                    "hover-state": {
                        "alpha": 1,
                        "visible": true
                    }
                },
                plotarea: {
                    adjustLayout: true,
                },
                scaleX: {
                    values: zones
                },
                scaleY: {
                    "format": "%v%",
                },
                series: [{
                    values: counts
                },
                ]
            };
            zingchart.render({
                id: dashboard.occuPercentage.chart,
                data: myConfig,
                height: 300,
                width: "100%"
            });
        });
    },

    LoadLotSummary() {
        spAPI.getParkingLotSummary(function (result) {

            var data = [];
            result.lots.forEach(function (item, index) {
                data.push({
                    "values": [parseInt(item.count)],
                    "backgroundColor": item.color,
                    "text": item.pricePerHour,
                    "slice": '70%'
                });
            });

            console.log(data);
            ZC.LICENSE = ["569d52cefae586f634c54f86dc99e6a9", "b55b025e438fa8a98e32482b5f768ff5"];
            var myConfig = {
                "type": "ring",

                "plot": {
                    "bar-width": "4px",
                    "animation": {
                        "effect": "ANIMATION_SLIDE_BOTTOM"
                    },
                    "hover-state": {
                        "alpha": 1,
                        "visible": true
                    },

                    "value-box": {
                        "value": "%v",
                        "text": "%pie-total-value",
                        "placement": "center",
                        "font-color": "black",
                        "font-size": 20,
                        "font-family": "Georgia",
                        "font-weight": "normal",
                        "rules": [{
                            "rule": "%p != 0",
                            "visible": false
                        }]
                    },

                    "tooltip": {
                        "text": "%t: %v (%npv%)",
                        "font-color": "black",
                        "font-family": "Georgia",
                        "text-alpha": 1,
                        "background-color": "white",
                        "alpha": 0.7,
                        "border-width": 1,
                        "border-color": "#cccccc",
                        "line-style": "dotted",
                        "border-radius": "10px",
                        "padding": "10%",
                        "placement": ""
                    },
                },


                legend: {
                    margin: 'auto auto 1 auto',
                    backgroundColor: 'none',
                    borderWidth: '0px',
                    item: {
                        margin: '0px',
                        padding: '0px',
                        fontColor: '#000',
                        fontFamily: 'Arial',
                        fontSize: '13px',
                    },
                    layout: 'x4',
                    position: 'top',
                    shadow: false
                },

                "series": data
            };
            zingchart.render({
                id: dashboard.lotSummaryChart,
                data: myConfig,
                height: 300,
                width: "100%"
            });
        });
    },

    LoadCustomerReviewSummary() {
        spAPI.getCustomerReviews(function (result) {

            var ratings = [];
            var counts = [];

            result.forEach(function (item, index) {
                ratings.push(item.rating);
                counts.push(parseInt(item.count));
            });

            console.log(counts);

            ZC.LICENSE = ["569d52cefae586f634c54f86dc99e6a9", "b55b025e438fa8a98e32482b5f768ff5"];
            var myConfig = {
                type: "hbar",
                plot: {
                    "alpha": 0.9,
                    "border-width": 1,
                    "stacked": true,
                    "border-color": "black",
                    "cursor": "pointer",
                    "border-radius-top-left": 4,
                    "border-radius-top-right": 4,
                    "bar-width": "30px",
                    "line-width": 2,
                    "bar-space": "20px",
                    "background-fit": "x",
                    "animation": {
                        "effect": "ANIMATION_SLIDE_BOTTOM"
                    },
                    "hover-state": {
                        "alpha": 1,
                        "visible": true
                    }
                },
                plotarea: {
                    adjustLayout: true
                },
                scaleX: {
                    values: ratings
                },
                series: [{
                    valueBox: {
                        placement: 'top',
                        color: 'black',
                    },
                    values: counts,
                    backgroundColor: "#ff9900",
                },

                ]
            };
            zingchart.render({
                id: dashboard.customerReviewChart,
                data: myConfig,
                height: 300,
                width: "100%"
            });
        });
    },

    LoadAll() {
        const d = new Date();
        this.monthlyFilter = d.getMonth();
        $(this.monthLabel).text('Monthly Summary (' + this.monthNames[this.monthlyFilter] + ')');

        this.LoadNewUsers(this.monthlyFilter);
        this.LoadNewTrans(this.monthlyFilter);
        this.LoadNewRevenues(this.monthlyFilter);
        this.LoadNewOccu(this.monthlyFilter);
        this.LoadFineAndSettlements(this.monthlyFilter);


        this.LoadLotSummary();
        this.LoadCustomerReviewSummary();
        
        $(this.durationTrend.btnToday).trigger('click');
        $(this.occuPercentage.btnToday).trigger('click');
        $(this.occuTrend.btnToday).trigger('click');
        
    }
}