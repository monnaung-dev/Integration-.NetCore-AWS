var parkingHistory = {
    table: null,
    txtPlateNum: null,
    txtInTime: null,
    txtOutTime: null,
    txtZone: null,
    btnSearch: null,
    btnReset: null,
    loadingOverlay: null,
    
    Init(table, txtPlateNum, txtZone, txtInTime, txtOutTime, btnSearch, btnReset, loadingOverlay) {
        this.table = table;
        this.txtPlateNum = txtPlateNum;
        this.txtZone = txtZone;
        this.txtInTime = txtInTime;
        this.txtOutTime = txtOutTime;
        
        this.btnSearch = btnSearch;
        this.btnReset = btnReset;
        this.loadingOverlay = loadingOverlay;

        $(this.btnSearch).on('click', function () {
            parkingHistory.LoadData(GetFilters());
        });

        $(this.btnReset).on('click', function () {
            parkingHistory.ResetFilters();
            parkingHistory.LoadData(GetFilters());
        });
    },

    ResetFilters() {
        this.txtOutTime = '';
        this.txtInTime = '';
        this.txtPlateNum = '';
        this.txtZone = '';
        
    },

    GetFilters() {
        var filters = 'plateNumber=' + $(this.txtPlateNum).val();
        filters = filters + '&zone' + $(this.txtZone).val();
        filters = filters + '&inTime' + $(this.txtInTime).val();
        filters = filters + '&outTime' + $(this.txtOutTime).val();
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
                dom: 'Bfrtip',
                buttons: [
                    { "extend": 'excel', "text": '<i class="fa fa-file-excel-o"></i>', "className": 'btn btn-success btn-xs' }
                ],
                "columns": [
                    { "data": "index" },
                    { "data": "plateNumber" },
                    { "data": "zone" },
                    { "data": "inTime" },
                    { "data": "outTime" },
                    { "data": "duration" },
                    { "data": "amount" }

                ]
            });
            $(parkingHistory.loadingOverlay).LoadingOverlay("hide", true); 
        });
    }
    
}


