var parkinginfo = {
    table: null,
    loadingOverlay: null,
   

    Init(table, loadingOverlay) {
        this.table = table;
        this.loadingOverlay = loadingOverlay;
    },

   
    LoadData() {
        $(this.loadingOverlay).LoadingOverlay("show"); 
        spAPI.getParkingInfo(function (data) {
                        
            $(parkinginfo.table).dataTable({
                "aaData": data,
                "bDestroy": true,
                "bFilter": false,
                "bPaginate": false,
                "bInfo": false,
                "bLengthChange": false,
                dom: 'Bfrtip',
                buttons: [
                    { "extend": 'excel', "text": '<i class="fa fa-file-excel-o"></i>', "className": 'btn btn-success btn-xs-6' }
                ],
                "columns": [
                    { "data": "index" },
                    { "data": "zone" },
                    { "data": "total" },
                    { "data": "free" },
                    { "data": "occupied" },
                    { "data": "paid" }

                ]
            });
            $(parkinginfo.loadingOverlay).LoadingOverlay("hide", true); 

            //setTimeout(parkinginfo.LoadData(), 10000);
        });
    }
    
}


