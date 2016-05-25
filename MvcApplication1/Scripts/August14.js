$(document).ready(function () {

    var ctx = $("#toPromotion").val();
    var VAL = $("#toPromotion").val();
    ctx = 0;

    var promotionmodel = new PromotionalPlan();

    var jsonData = [];
    var cities = '<a><img src="/images/pakistan.png" alt="">Pakistan</a>,'.split(',');
    for (var i = 0; i < cities.length; i++) jsonData.push({ id: i, name: cities[i], status: i % 2 ? 'Already Visited' : 'Planned for visit', coolness: Math.floor(Math.random() * 10) + 1 });


    var ms7 = $('#ms-new-pr').magicSuggest({
        data: jsonData,
        resultAsString: true,
        maxSelection: 1,
        value: [ctx],
        maxSelectionRenderer: function () { }
    });

    var countryfrom = $('#countries_searchrates_footer').val();
    var id = $("#toPromotion").val();

    $(ms7).on('selectionchange', function (event, selection) {
        var selected = ms7.getSelectedItems();
        var countryto = selected[0].id;

        id = 238;
        $("#hidden-countryto").val(id);

        countryfrom = $('#countries_searchrates_footer').val();
        promotionmodel.GetPromotionRate(countryfrom, id);


    });



    ko.applyBindings(promotionmodel, document.getElementById("promotion-plan-bind"));
    promotionmodel.GetPromotionRate(countryfrom, id);
});




function PromotionalPlan() {
    var UserType = $("#hid-usertype").val();
    if (UserType == "new") {

        var self = this;

        self.CountryFrom = ko.observable("");
        self.CountryTo = ko.observable("");
        self.PromotionPlans = ko.observableArray([]);

        self.GetPromotionRate = function (countryfrom, countryto) {
            //self.status("");


            $.ajax({
                url: '/Promotion/GetUpTo_PromotionRate',
                data: {
                    countryfrom: countryfrom,
                    countryto: countryto,
                    planname: "One Touch Dial",
                    usertype: "new",
                    promocode: "FREEDOM15"
                },
                type: "GET",
                success: function (resp) {

                    self.PromotionPlans(resp);
                    //$("#retr_email_valmsg").text("Please enter a valid email address.").hide();

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    self.ErrorMsg(JSON.parse(jqXHR.responseText).ValidationMessage);
                }
            });
        };

        self.BuyPromotion = function (plandata) {
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
    else if (UserType == "old") {
        var self = this;

        self.CountryFrom = ko.observable("");
        self.CountryTo = ko.observable("");
        self.PromotionPlans = ko.observableArray([]);
        var orderid = $("#premium-orderid").val();

        var balncAmount = $("#Balnc-amount").val();
        var currency = $("#Curr-code").val();
        self.GetPromotionRate = function (countryfrom, countryto) {
            //self.status("");
            $.ajax({
                url: '/Promotion/GetUpTo_PromotionRate',
                data: {
                    countryfrom: countryfrom,
                    countryto: countryto,
                    planname: "One Touch Dial",
                    usertype: "old",
                    promocode: "FREEDOM15"
                },
                type: "GET",
                success: function (resp) {
                    self.PromotionPlans(resp);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    self.ErrorMsg(JSON.parse(jqXHR.responseText).ValidationMessage);
                }
            });
        };

        self.BuyPromotion = function (plandata) {

            window.location.href = "/Recharge?orderid=" + orderid + "&RecBal=" + balncAmount + "&currencycode=" + currency + "&servicefee="
                + plandata.ServiceFee + "&plan_id=" + plandata.PlanId + "&RechAmount=" + plandata.Denomination + "&CouponCode="
                + plandata.CouponCode;
        };

    }

}