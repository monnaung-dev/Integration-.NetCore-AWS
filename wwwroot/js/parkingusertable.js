var parkingUser = {
    table: null,
    txtName: null,
    txtEmail: null,
    txtPhone: null,
    txtFromDate: null,
    txtToDate: null,
    btnSearch: null,
    btnReset: null,
    loadingOverlay: null,

    Init(table, txtName, txtEmail, txtPhone, txtFromDate, txtToDate,
        btnSearch, btnReset, loadingOverlay) {
        this.table = table;
        this.txtName = txtName;
        this.txtEmail = txtEmail;
        this.txtPhone = txtPhone;
        this.txtFromDate = txtFromDate;
        this.txtToDate = txtToDate;
        this.btnSearch = btnSearch;
        this.btnReset = btnReset;
        this.loadingOverlay = loadingOverlay;

        $(this.btnSearch).on('click', function () {
            parkingUser.LoadData(parkingUser.GetFilters());
        });
    },

    GetFilters() {
        var filters = 'name=' + $(this.txtName).val();
        filters = filters + "&email=" + $(this.txtEmail).val();
        filters = filters + "&phone=" + $(this.txtPhone).val();
        filters = filters + "&fromDate=" + $(this.txtFromDate).val();
        filters = filters + "&toDate=" + $(this.txtToDate).val();
        return filters;
    },

    ResetFilters() {
        this.txtFromDate = '';
        this.txtToDate = '';
        this.txtName = '';
        this.txtEmail = '';
        this.txtPhone = '';
    },


    LoadData(filters) {
        $(this.loadingOverlay).LoadingOverlay("show");
        spAPI.getParkingUsers(filters, function (data) {
            $(parkingUser.table).dataTable({
                "aaData": data,
                "bDestroy": true,
                "bFilter": false,
                "bLengthChange": false,
                "order": [[4, "desc"]],
                dom: 'Bfrtip',
                buttons: [
                    { "extend": 'excel', "text": '<i class="fa fa-file-excel-o"></i>', "className": 'btn btn-success btn-xs-6' }
                ],
                "columns": [
                    { "data": "index" },
                    { "data": "name" },
                    { "data": "email" },
                    { "data": "phone" },
                    { "data": "date" },
                ],
                "sSearch": filters
            });
            $(parkingUser.loadingOverlay).LoadingOverlay("hide", true);
        })
    }
}