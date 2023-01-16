// Write your JavaScript code.
var spAPI = {


    async loadData(url, callback) {
        await $.getJSON(url, function (result) {
            callback(result);
            console.log(result);
            return result;
        });
    },

    async getNewUsers(month, callback) {
        return await this.loadData("/Dashboard/GetUsers?month=" + month, callback);
    },

    async getTrans(month, callback) {
        return await this.loadData("/Dashboard/GetTrans?month=" + month, callback);
    },

    async getRevenues(month, callback) {
        return await this.loadData("/Dashboard/GetRevenues?month=" + month, callback);
    },

    async getOccupancy(month, callback) {
        return await this.loadData("/Dashboard/GetOccupancy?month=" + month, callback);
    },

    async getParkingLotSummary(callback) {
        return await this.loadData("/Dashboard/GetParkingLotSummary", callback);
    },

    async getFinesAndSettlements(month, callback) {
        return await this.loadData("/Dashboard/GetFinesAndSettlements?month=" + month, callback);
    },

    async getDailyDurationTrend(date, callback) {
        return await this.loadData("/Dashboard/GetDailyDurationTrend?date=" + date, callback);
    },


    async getOccupancyPercentage(date, callback) {
        return await this.loadData("/Dashboard/GetOccupancyPercentage?date=" + date, callback);
    },

    async getOccupancyTrend(date, callback) {
        return await this.loadData("/Dashboard/GetOccupancyTrend?date=" + date, callback);
    },


    async getCustomerReviews(callback) {
        return await this.loadData("/Dashboard/GetCustomerReviews", callback);
    },

    async getParkingHistory(urlParms, callback) {
        return await this.loadData("/Report/GetParkingHistory?" + urlParms, callback);
    },

    async getViolationHistory(urlParms, callback) {
        return await this.loadData("/Report/GetViolationHistory?" + urlParms, callback);
    },

    async getParkingInfo(callback) {
        return await this.loadData("/Report/GetParkingInfo", callback);
    },

    async getZones(callback) {
        return await this.loadData("/Report/GetZones", callback);
    },

}