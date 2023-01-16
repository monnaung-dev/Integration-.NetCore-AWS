var registerVehicle = {
    table: null,
    txtplateNumber: null,
    txtmodel: null,
    txtuserName: null,
    txtFromDate: null,
    txtToDate: null,
    btnSearch: null,
    btnReset: null,
    loadingOverlay: null,

    Init(table, txtplateNumber, txtmodel, txtuserName, txtFromDate, txtToDate,
        btnSearch, btnReset, loadingOverlay) {
        this.table = table;
        this.txtplateNumber = txtplateNumber;
        this.txtmodel = txtmodel;
        this.txtuserName = txtuserName;
        this.txtFromDate = txtFromDate;
        this.txtToDate = txtToDate;
        this.btnSearch = btnSearch;
        this.btnReset = btnReset;
        this.loadingOverlay = loadingOverlay;

        $(this.btnSearch).on('click', function () {
            console.log("Search");
            registerVehicle.LoadData(registerVehicle.GetFilters());
        });
    },

    GetFilters() {
        var filters = 'plateNumber=' + $(this.txtplateNumber).val();
        filters = filters + "&model=" + $(this.txtmodel).val();
        filters = filters + "&userName=" + $(this.txtuserName).val();
        filters = filters + "&fromDate=" + $(this.txtFromDate).val();
        filters = filters + "&toDate=" + $(this.txtToDate).val();
        return filters;
    },

    ResetFilters() {
        this.txtFromDate = '';
        this.txtToDate = '';
        this.txtplateNumber = '';
        this.txtmodel = '';
        this.txtuserName = '';
    },


    LoadData(filters) {
        $(this.loadingOverlay).LoadingOverlay("show");
        spAPI.getRegisterVehicle(filters, function (data) {
            $(registerVehicle.table).dataTable({
                "aaData": data,
                "bDestroy": true,
                "bFilter": false,
                "bLengthChange": false,
                "order": [[0, "desc"]],
                dom: 'Bfrtip',
                buttons: [
                    { "extend": 'excel', "text": '<i class="fa fa-file-excel-o"></i>', "className": 'btn btn-success btn-xs-6' }
                ],
                "columns": [
                    { "data": "index" },
                    { "data": "plateNumber" },
                    { "data": "model" },
                    { "data": "userName" },
                ],
                "sSearch": filters
            });
            $(registerVehicle.loadingOverlay).LoadingOverlay("hide", true);
        })
    }
}