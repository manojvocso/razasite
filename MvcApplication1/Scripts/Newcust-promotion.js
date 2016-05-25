$(document).ready(function () {

    var promotionmodel = new PromotionalPlan();

    var jsonData = [];
    var cities = '<a><img src="/images/india.png" alt="">India (+91)</a>,<a><img src="/images/pakistan.png" alt="">Pakistan (+92)</a>,<a><img src="/images/nepal.png" alt="">Nepal (+977)</a>,<a><img src="/images/bangladesh.png" alt="">Bangladesh (+880)</a>,<a><img src="/images/sri-lanka.png" alt="">Sri Lanka (+94)</a>,'.split(',');
    for (var i = 0; i < cities.length; i++) jsonData.push({ id: i, name: cities[i], status: i % 2 ? 'Already Visited' : 'Planned for visit', coolness: Math.floor(Math.random() * 10) + 1 });


    var ms7 = $('#ms-new-pr').magicSuggest({
        data: jsonData,
        resultAsString: true,
        maxSelection: 1,
        value: [0],
        maxSelectionRenderer: function () { }
    });
    var countryfrom = $('#countries_searchrates_footer').val();
    var id = 130;
    $(ms7).on('selectionchange', function (event, selection) {
        var selected = ms7.getSelectedItems();
       var countryto = selected[0].id;
        
        if (countryto == 0) {
            id = 130;
            $("#hidden-countryto").val(id);
        }
        if (countryto == 1) {
            id = 238;
            $("#hidden-countryto").val(id);
        }
        if (countryto == 2) {
            id = 224;
            $("#hidden-countryto").val(id);
        }
        if (countryto == 3) {
            id = 27;
            $("#hidden-countryto").val(id);
        }
        if (countryto == 4) {
            id = 281;
            $("#hidden-countryto").val(id);
        }
         countryfrom = $('#countries_searchrates_footer').val();
         promotionmodel.GetPromotionRate(countryfrom, id);
        

    });
  


    ko.applyBindings(promotionmodel, document.getElementById("promotion-plan-bind"));
    promotionmodel.GetPromotionRate(countryfrom, id);
});




function PromotionalPlan() {

    var self = this;

    self.CountryFrom = ko.observable("");
    self.CountryTo = ko.observable("");
    self.PromotionPlans = ko.observableArray([]);

    self.GetPromotionRate = function(countryfrom, countryto) {
        //self.status("");
     
        $.ajax({
            url: '/Account/GetNewCustomerPromotionRate',
            data: {
                countryfrom: countryfrom,
                countryto: countryto,
                planname: "One Touch Dial"
            },
            type: "GET",
            success: function(resp) {

                self.PromotionPlans(resp);
               
                //$("#retr_email_valmsg").text("Please enter a valid email address.").hide();

            },
            error: function(jqXHR, textStatus, errorThrown) {
                self.ErrorMsg(JSON.parse(jqXHR.responseText).ValidationMessage);
            }
        });
    };

    self.BuyPromotion = function(plandata) {
        var countryfrom = $("#countries_searchrates_newCustomer").val();
      
        $.ajax({
            url: '/Cart/BuyCustomerPromotion',
            data: {
                CountryFrom: countryfrom,
                CountryTo: plandata.CountryTo,
                Denomination: plandata.Denomination,
                CurrencyCode: plandata.CurrencyCode,
                PlanName: plandata.PlanName,
                ServiceFee: plandata.ServiceFee,
                PlanId: plandata.PlanId,
                CouponCode: plandata.CouponCode
            },
            type: "POST",
            success: function (resp) {
                window.location.href = resp.newurl;

            },
            error: function (jqXHR, textStatus, errorThrown) {
               // self.ErrorMsg(JSON.parse(jqXHR.responseText).ValidationMessage);
            }
        });

    };

}