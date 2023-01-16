
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using SmartParking.Admin.Models.JsonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartParking.Admin.Helpers;
using SmartParking.Admin.Models;

namespace SmartParking.Admin.Data
{
    public class CustomerReviewsInfo
    {
        IDynamoDBContext db;

        public CustomerReviewsInfo(IDynamoDBContext db)
        {
            this.db = db;
        }


        public async Task<List<customerReviewsSummary>> GetReviews()
        {
            List<customerReviewsSummary> summary = new List<customerReviewsSummary>();
            try
            {
                customerReviewsSummary ratingOne = new customerReviewsSummary();
                ratingOne.rating = "1*";
                ratingOne.count = "4";
                summary.Add(ratingOne);

                customerReviewsSummary ratingTwo = new customerReviewsSummary();
                ratingTwo.rating = "2*";
                ratingTwo.count = "10";
                summary.Add(ratingTwo);

                customerReviewsSummary ratingThree = new customerReviewsSummary();
                ratingThree.rating = "3*";
                ratingThree.count = "50";
                summary.Add(ratingThree);

                customerReviewsSummary ratingFour = new customerReviewsSummary();
                ratingFour.rating = "4*";
                ratingFour.count = "750";
                summary.Add(ratingFour);

                customerReviewsSummary ratingFive = new customerReviewsSummary();
                ratingFive.rating = "5*";
                ratingFive.count = "985";
                summary.Add(ratingFive);
            }
            catch(Exception ex)
            {

            }
            

            return summary;
        }

    }
}
