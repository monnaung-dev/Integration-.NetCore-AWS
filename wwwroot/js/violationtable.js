var violationHistory = {
    table: null,
    txtPlateNum: null,
    txtInTime: null,
    txtOutTime: null,
    txtZone: null,
    txtViolationType: null,
    btnSearch: null,
    btnReset: null,
    loadingOverlay: null,
    
    Init(table, txtZone, txtViolationType,
        txtInTime, txtOutTime, txtPlateNum,
        txtOffenseLevel, txtRefCode, btnSearch, btnReset, loadingOverlay) {
        this.table = table;
        this.txtZone = txtZone;
        this.txtViolationType = txtViolationType;
        this.txtInTime = txtInTime;
        this.txtOutTime = txtOutTime;
        this.txtPlateNum = txtPlateNum;
        this.txtOffenseLevel = txtOffenseLevel;
        this.txtRefCode = txtRefCode;
        this.btnSearch = btnSearch;
        this.btnReset = btnReset;
        this.loadingOverlay = loadingOverlay;

       
        $(this.btnSearch).on('click', function () {
            violationHistory.LoadData(violationHistory.GetFilters());
        });

        //$(this.btnReset).on('click', function () {
        //    violationHistory.ResetFilters();
        //    violationHistory.LoadData(violationHistory.GetFilters());
        //});
    },

    ResetFilters() {
        $(this.txtInTime).datepicker('setDate', 'now');
        $(this.txtOutTime).datepicker('setDate', 'now');
        this.txtPlateNum ='';
        this.txtRefCode = '';
        this.txtOffenseLevel = '';
        this.txtZone = '';
        this.txtViolationType = '';        
    },


    GetFilters() {
        var filters = 'plateNumber=' + $(this.txtPlateNum).val();
        filters = filters + '&zone=' + $(this.txtZone).val();
        filters = filters + '&inTime=' + $(this.txtInTime).val();
        filters = filters + '&outTime=' + $(this.txtOutTime).val();
        filters = filters + '&violationType=' + $(this.txtViolationType).val(); 
        filters = filters + '&refCode=' + $(this.txtRefCode).val(); 
        filters = filters + '&offenseLevel=' + $(this.txtOffenseLevel).val();
        return filters;

    },

    LoadData(filters) {
        $(this.loadingOverlay).LoadingOverlay("show"); 
        spAPI.getViolationHistory(filters, function (data) {
                        
            $(violationHistory.table).dataTable({
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
                    { "data": "date" },
                    { "data": "dueDate" },
                    { "data": "violationType" },
                    { "data": "offenseLevel" },
                    { "data": "reference" },
                    { "data": "fine" },
                    { "data": "paid" },
                    { "data": "paymentDate" },
                ],
                "sSearch": filters
            });
            $(violationHistory.loadingOverlay).LoadingOverlay("hide", true); 
        });
    }
    
}


