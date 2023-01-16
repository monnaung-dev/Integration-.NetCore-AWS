var parkingHistory = {
    table: null,
    txtPlateNum: null,
    txtInTime: null,
    txtOutTime: null,
    txtZone:null,
    btnSearch: null,
    btnReset: null,
    loadingOverlay: null,
    
    Init(table, txtPlateNum, txtZone,txtInTime, txtOutTime, btnSearch, btnReset, loadingOverlay) {
        this.table = table;
        this.txtPlateNum = txtPlateNum;
        this.txtZone = txtZone;
        this.txtInTime = txtInTime;
        this.txtOutTime = txtOutTime;
        
        this.btnSearch = btnSearch;
        this.btnReset = btnReset;
        this.loadingOverlay = loadingOverlay;

        $(this.btnSearch).on('click', function () {
            parkingHistory.LoadData(parkingHistory.GetFilters());
        });

        //$(this.btnReset).on('click', function () {
        //    parkingHistory.ResetFilters();
        //    parkingHistory.LoadData(parkingHistory.GetFilters());
        //});
    },

    ResetFilters() {
        $(this.txtInTime).datepicker('setDate', 'now');
        $(this.txtOutTime).datepicker('setDate', 'now');
        $(this.txtPlateNum).value = "";
        $(this.txtZone).selectedIndex = "-1";
    },

    GetFilters() {
        var filters = 'plateNumber=' + $(this.txtPlateNum).val();
        filters = filters + '&zone=' + $(this.txtZone).val();
        filters = filters + '&inTime=' + $(this.txtInTime).val();
        filters = filters + '&outTime=' + $(this.txtOutTime).val();
        return filters;
    },

    LoadData(filters) {
        $(this.loadingOverlay).LoadingOverlay("show"); 
        spAPI.getParkingHistory(filters, function (data) {
                        
            $(parkingHistory.table).dataTable({
                "aaData": data,
                "bDestroy": true,
                "bFilter": false,
                "bLengthChange": false,
                "order": [[3, "desc"]],
                dom: 'Bfrtip',
                buttons: [
                    { "extend": 'excel', "text": '<i class="fa fa-file-excel-o"></i>', "className": 'btn btn-success btn-xs-6' }
                ],
                "columns": [
                    { "data": "index" },
                    { "data": "plateNumber" },
                    { "data": "zone" },
                    { "data": "inTime" },
                    { "data": "outTime" },
                    { "data": "duration" },
                    { "data": "amount" }

                ],
                "sSearch": filters
            });
            $(parkingHistory.loadingOverlay).LoadingOverlay("hide", true); 
        });
    }
    
}


