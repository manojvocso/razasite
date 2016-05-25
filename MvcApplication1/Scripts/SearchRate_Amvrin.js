$(document).ajaxSend(function (event, request, settings) {
    $('#load-blk').css('display', '');

});

$(document).ajaxComplete(function (event, request, settings) {
    $('#load-blk').css('display', 'none');
});


var model = new RateModel();
var cid = $("#country-byip").val();


//var CountryTooID = "0";
//var PageName = window.location.pathname;
//PageName = PageName.substring(PageName.lastIndexOf("/") + 1);

//var abc = "sabal";
//$.ajax({
//    url: '/Rates/GetCountryID_By_CountryName/',
//    data: {
//        PageName: PageName
//    },
//    type: "GET",
//    cache: false,
//    success: function (returndata) {
//        if (returndata.ok) {
//            var abc = returndata.CountryToID;
//            Test_Data(abc);
//        }
//    },
//    error: function (jqXHR, textStatus, errorThrown) {
//        CountryTooID = "314";
//    }

//});

//function Test_Data(abc) {
//    CountryTooID = abc;
//}









// starting of page 
$(document).ready(function () {


    var list = [];
    var qs = getQueryStrings();
    var fromCountry = qs["countryfrom"];
    var toCountry = qs["countryto"];
    var globalcall = qs["globalcall"];
    var callforwarding = qs["callForwarding"];
    model.CallingTo(toCountry);
    model.CallingFrom(fromCountry);
    if (globalcall == "true") {
        model.IsGlobalCall(true);
    }

    if (callforwarding == "true") {
        model.IsCallForward(true);
    }

    model.GetRate();














    $.ajax({
        url: "/Account/CountryToListwithFlag/",
        type: 'GET',
        success: function (data) {
            //predata1 = data;
            model.ToCountryLoaded(true);

            $.each(data, function () {
                list.push({ id: this.Id, cname: this.CountryFlag, name: this.Name, Code: this.CountCode, dt: this.Name, status: 'Already visited' });
            });

            var ctx = model.CallingTo();

            var searchrate = $('#ms19').magicSuggest({
                displayField: 'cname',
                allowFreeEntries: false,
                data: list,
                resultAsString: true,
                strictSuggest: true,
                maxSelection: 1,
                value: [ctx],

                renderer: function (data) {
                    return '<div><a><i class="magic_' + data.id + '"><p>' + data.name + '(+' + data.Code + ')' + '</p></i></a></div>';
                },
                maxSelectionRenderer: function () {
                }

            });

            $(searchrate).on('selectionchange', function (event, selection) {
                var selected = searchrate.getSelectedItems();
                var countryto = selected[0].id;
                model.CallingTo(countryto);
            });


        }
    });


    $("#clickk").hover(function () {
        $("#monster").slideToggle("fast");
    });

    $("#tabAndroid").trigger("click");

    $("#close-val").click({ handler: model.Disablepopup });

    $("#backgroundPopup").click({ handler: model.Disablepopup });

    $("#Search-button").click({ handler: model.GetRate1 });

    ko.applyBindings(model, document.getElementById("rate-bind-search"));


});

function sleep(delay) {
    var start = new Date().getTime();
    while (new Date().getTime() < start + delay);
}

function RateModel() {

    var self = this;

    self.iscollapseRecmnd = ko.observable(true);
    self.iscollapseNew = ko.observable(true);

    self.MobileDirectCard = ko.observableArray([]);
    self.OnetouchDialCard = ko.observableArray([]);

    self.NewCust_MobileDirect = ko.observableArray([]);
    self.NewCust_GlobalPlan = ko.observableArray([]);

    self.CallingFrom = ko.observable("");
    self.CallingTo = ko.observable("");
    self.IsGlobalCall = ko.observable(false);
    self.IsCallForward = ko.observable(false);

    // self.amnt = ko.observable("");
    self.CountryListFrom = ko.observableArray([]);
    self.CountryListTo = ko.observableArray([]);

    // List for country or Mobile.
    self.MobileCityList = ko.observableArray([]);
    self.CountryMobileList = ko.observableArray([]);

    self.PreList = ko.observableArray([]);
    self.PreList1 = ko.observableArray([]);
    self.PreList2 = ko.observableArray([]);

    self.OneTouchCardAutoRefill = ko.observable(true);
    self.MobileDirectAutoRefill = ko.observable(true);

    self.FromCountryLoaded = ko.observable(false);
    self.ToCountryLoaded = ko.observable(false);

    self.RatePerMinMobileDirect = ko.observable(0);
    self.RatePerMinOneTouch = ko.observable(0);

    self.Newcust_RatePerMinMobileDirect = ko.observable(0);
    self.NewCust_RatePerMinGlobal = ko.observable(0);

    //ends here

    self.RatePerMinSign = ko.observable("");

    //calculation for the new customer global plan.
    self.Calc_onetouchautorefill = ko.computed(function () {
        var i = 0;
        ko.utils.arrayForEach(self.NewCust_GlobalPlan(), function (item) {
            var min2 = Math.round(item.TotalMinutes + (item.TotalMinutes * item.Discount / 100));
            if (i == 0) {
                self.NewCust_RatePerMinGlobal(Math.round(((item.PlanAmount / min2) * 100) * 10) / 10);
                if (self.CallingFrom() == 3 || self.CallingFrom() == "3") {
                    item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(0));
                } else {
                    item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(100));
                }

                item.Minute = Math.round(parseFloat(ko.utils.unwrapObservable(min2)));
            } else {
                item.Minute = Math.round(parseFloat(ko.utils.unwrapObservable(min2)));
            }
            item.IsAutoRefill = true;
            item.IsEnrollToExtraMinute = true;
            i++;
        });
    });

    // new customer mobile direct plan calculation.
    self.Calc_MobileDirectautorefill = ko.computed(function () {
        var i = 0;
        ko.utils.arrayForEach(self.NewCust_MobileDirect(), function (item) {
            var min2 = Math.round(item.TotalMinutes + (item.TotalMinutes * item.Discount / 100));

            if (i == 0) {
                self.Newcust_RatePerMinMobileDirect(Math.round(((item.PlanAmount / min2) * 100) * 10) / 10);
                if (self.CallingFrom() == 3 || self.CallingFrom() == "3") {
                    item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(0));
                } else {
                    item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(100));
                }

                item.Minute = Math.round(parseFloat(ko.utils.unwrapObservable(min2)));

            } else {

                item.Minute = Math.round(parseFloat(ko.utils.unwrapObservable(min2)));
            }

            item.IsAutoRefill = true;
            item.IsEnrollToExtraMinute = true;
            i++;
        });

    });



    // calculation for onetouch plan
    self.IsautorefillOnetouch = ko.computed(function () {
        var t = self.OneTouchCardAutoRefill();
        if (t == 1) { // calculation for autorefill plan.
            var i = 0;
            self.PreList([]);
            self.PreList(self.OnetouchDialCard());

            ko.utils.arrayForEach(self.OnetouchDialCard(), function (item) {

                var min2 = Math.round(item.TotalMinutes + (item.TotalMinutes * (item.Discount / 100)));

                if (i == 0) {
                    self.RatePerMinOneTouch(Math.round(((item.PlanAmount / min2) * 100) * 10) / 10);
                    if (self.CallingFrom() == 3 || self.CallingFrom() == "3") {
                        item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(0));
                    } else {
                        item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(100));
                    }
                    // item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(100));
                    item.Minute = Math.round(parseFloat(ko.utils.unwrapObservable(min2)));


                } else {

                    item.Minute = Math.round(parseFloat(ko.utils.unwrapObservable(min2)));
                }
                item.IsAutoRefill = true;

                i++;
            });

            self.OnetouchDialCard([]);
            self.OnetouchDialCard(self.PreList());
        } else { // without AutoRefill
            var ratewithoutauto;
            var j = 0;
            self.PreList([]);
            self.PreList(self.OnetouchDialCard());

            ko.utils.arrayForEach(self.PreList(), function (item) {

                if (j == 0) {
                    self.RatePerMinOneTouch(item.RatePerMin);
                    if (self.CallingFrom() == 3 || self.CallingFrom() == "3") {
                        item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(0));
                    } else {
                        item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(100));
                    }
                    //item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(100));
                    item.Minute = Math.round(parseFloat(ko.utils.unwrapObservable(item.TotalMinutes)));
                } else {

                    item.Minute = Math.round(parseFloat(ko.utils.unwrapObservable(item.TotalMinutes)));
                }
                item.IsAutoRefill = false;
                j++;
            });
            self.OnetouchDialCard([]);
            self.OnetouchDialCard(self.PreList());

        }
    });

    self.OneTouchCardAutoRefill.subscribe(self.IsautorefillOnetouch);

    // end calculation for one touch
    // calculation start for mobile direct plan.
    self.IsautorefillMobileDirect = ko.computed(function () {
        var t = self.MobileDirectAutoRefill();
        if (t == 1) { // for autorefill plan.
            var i = 0;
            self.PreList1([]);
            self.PreList1(self.MobileDirectCard());
            var ratepermin;
            ko.utils.arrayForEach(self.PreList1(), function (item) {

                var min2 = Math.round(item.TotalMinutes + (item.TotalMinutes * item.Discount / 100));
                if (i == 0) {

                    self.RatePerMinMobileDirect(Math.round(((item.PlanAmount / min2) * 100) * 10) / 10);
                    if (self.CallingFrom() == 3 || self.CallingFrom() == "3") {
                        item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(0));
                    } else {
                        item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(100));
                    }
                    //  item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(100));
                    item.Minute = Math.round(parseFloat(ko.utils.unwrapObservable(min2)));


                } else {

                    item.Minute = Math.round(parseFloat(ko.utils.unwrapObservable(min2)));
                }

                item.IsAutoRefill = true;
                i++;
            });
            self.MobileDirectCard([]);
            self.MobileDirectCard(self.PreList1());
        } else { // calc for without autorefill.
            var j = 0;
            self.PreList1([]);
            self.PreList1(self.MobileDirectCard());
            var ratewithoutauto;
            ko.utils.arrayForEach(self.PreList1(), function (item) {
                if (j == 0) {

                    self.RatePerMinMobileDirect(item.RatePerMin);
                    if (self.CallingFrom() == 3 || self.CallingFrom() == "3") {
                        item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(0));
                    } else {
                        item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(100));
                    }
                    // item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(100));
                    item.Minute = Math.round(parseFloat(ko.utils.unwrapObservable(item.TotalMinutes)));
                } else {
                    //min1 = Math.round(item.PlanAmount * 100 / ratewithoutauto);
                    item.Minute = Math.round(parseFloat(ko.utils.unwrapObservable(item.TotalMinutes)));
                }
                item.IsAutoRefill = false;

                j++;
            });
            self.MobileDirectCard([]);
            self.MobileDirectCard(self.PreList1());

        }
    });

    self.MobileDirectAutoRefill.subscribe(self.IsautorefillMobileDirect); // subscribe radiobutton for mobiledirect in recommended plans.


    self.BuyPlan = function (data) {

        $.ajax({
            url: '/Cart/BuyPlan',
            data: {
                PlanId: data.PlanId,
                FromToMapping: data.FromToMapping,
                FromCountry: data.CountryFrom,
                ToCountry: data.CountryTo,
                AutoRefill: data.IsAutoRefill,
                IsfromSerchrate: true,
                isEnrollToExtraMinute: data.IsEnrollToExtraMinute
            },
            type: "POST",
            success: function (returndata) {
                if (returndata.ok) {
                    window.location = returndata.newurl;
                }
            },
        });
    };

    self.FetchCountryListTo = function () {
        $.getJSON('/Account/GetCountryListFull/', null, function (data) {
            self.CountryListTo(data);
            self.ToCountryLoaded(true);

            if (self.FromCountryLoaded() == true && self.ToCountryLoaded() == true) {
                if (location.search.length > 1) {
                    var qs = getQueryStrings();
                    var fromCountry = qs["countryfrom"];
                    var toCountry = qs["countryto"];
                    var globalcall = qs["globalcall"];
                    var callforwarding = qs["callForwarding"];

                    self.CallingFrom(fromCountry);
                    self.CallingTo(toCountry);

                    if (globalcall == "true") {
                        self.IsGlobalCall(true);
                    }
                    if (callforwarding == "true") {
                        self.IsCallForward(true);
                    }

                    //    self.GetRate();
                }
            }
        });
    };

    self.SetServerValues = function (data) {

        var random = document.getElementById('banner-india');

        if (data.CountryBannerPath == "") {
            random.style.background = 'url(../images/rate-save-big-banner.jpg)';
        }
        else {
            random.style.background = 'url(../images/' + data.CountryBannerPath + '.jpg)';
        }

        $("#MobileorGlobal").val("");

        self.RatePerMinMobileDirect(0);
        self.RatePerMinOneTouch(0);
        self.MobileDirectCard(data.MobileDirectRateCards);
        self.OnetouchDialCard(data.OneTouchDialRateCards);

        //list for country city.
        self.MobileCityList(data.MobileCityList);
        self.CountryMobileList(data.CountryMobileList);
        //indiaCentPlan dynamic data starts here

        self.NewCust_MobileDirect(data.MobileDirectRateCards);
        self.NewCust_GlobalPlan(data.OneTouchDialRateCards);

        self.RatePerMinSign(data.RateperMinSign);

        $("#container-main").css("display", "");
        $("#container-main-second").css("display", "");
        $("div.ramadan-div2").css("display", "");

        var detach1, detach2, detach3, detach4;

        if (data.SearchType == "Mobile") {

            // for new customer
            if ($("#container-plan-3-New").find('#open-citydirect').length == 1) {
                //place citydirect in container plan.
                //no change
            } else {
                // place citydirect after detach
                //detach1 = $("#open-citydirect").detach();
                detach2 = $("#new-cust-mobile").detach();
                //detach1.appendTo("#container-plan-3-New");
                detach2.appendTo("#container-plan-2-New");
            }
            $("#container-plan-2-New").after($('#container-main')).after($('#new-cust-global'));

            // for recmnd plan
            if ($("#container-plan-3-recmnd").find('#city-direct-recmnd').length == 1) {
                //place citydirect in container plan.
                //no change
            } else {
                // place citydirect after detach
                //detach1 = $("#city-direct-recmnd").detach();
                detach2 = $("#recomnd-mobile").detach();
                //detach1.appendTo("#container-plan-3-recmnd");
                detach2.appendTo("#container-plan-2-recmnd");
            }
            $("#new-link").text("LandLine Special");
            $("#recmnd-link").text("LandLine Special");
            $("#container-plan-2-recmnd").after($('#container-main-second')).after($('#recomnd-global'));
        }

        if (data.SearchType == "City") {
            //for new customer
            if ($("#container-plan-3-New").find('#new-cust-mobile').length == 1) {
                // no change.
            } else {

                //detach1 = $("#open-citydirect").detach();
                detach2 = $("#new-cust-mobile").detach();
                //detach1.appendTo("#container-plan-2-New");
                detach2.appendTo("#container-plan-3-New");
            }

            //for recmnd
            if ($("#container-plan-3-recmnd").find('#recomnd-mobile').length == 1) {
                // no change.
            } else {

                //detach1 = $("#city-direct-recmnd").detach();
                detach2 = $("#recomnd-mobile").detach();
                //detach1.appendTo("#container-plan-2-recmnd");
                detach2.appendTo("#container-plan-3-recmnd");
            }
            $("#new-link").text("Mobile Direct Plan");
            $("#recmnd-link").text("Mobile Direct Plan");
            $("#container-plan-2-recmnd").after($('#container-main-second')).after($('#recomnd-global'));
            $("#container-plan-2-New").after($('#container-main')).after($('#new-cust-global'));
        }

        if (data.SearchType == "Country") {

            // for new customer
            if ($("#container-plan-3-New").find('#open-citydirect').length == 1) {
                //place citydirect in container plan.
                //no change
            } else {
                // place citydirect after detach
                //detach1 = $("#open-citydirect").detach();
                detach2 = $("#new-cust-mobile").detach();
                //detach1.appendTo("#container-plan-3-New");
                detach2.appendTo("#container-plan-2-New");
            }
            $("#new-cust-global").after($('#container-main')).after($('#container-plan-2-New'));

            // for recmnd plan
            if ($("#container-plan-3-recmnd").find('#city-direct-recmnd').length == 1) {
                //place citydirect in container plan.
                //no change
            } else {
                // place citydirect after detach
                detach1 = $("#city-direct-recmnd").detach();
                detach2 = $("#recomnd-mobile").detach();
                //detach1.appendTo("#container-plan-3-recmnd");
                detach2.appendTo("#container-plan-2-recmnd");
            }
            $("#new-link").text("LandLine Special");
            $("#recmnd-link").text("LandLine Special");
            $("#recomnd-global").after($('#container-main-second')).after($('#container-plan-2-recmnd'));

        }

        self.CollapseContainerNew();
        self.CollapseContainerRecmnd();
        $('#load-blk').css('display', 'none');
        $('#recomnd-global').css('display', '');
        $('#container-plan-2-recmnd').css('display', '');
        $('#container-main-second').css('display', '');
        //$('#rate-main').unblock({ message: null });
        $("#gradientBG").css('display', '');
        $("#gradientBG2").css('display', 'none');
    }

    self.GetRate = function () {

        if (self.CallingFrom() == undefined || self.CallingTo() == undefined) {
            return false;
        }

        //    $('#load-blk').css('display', ''); // show progress bar

        //     var random = document.getElementById('banner-india');

        var response = $("#MobileorGlobal").val();

        $.ajax({
            url: '/Rate/SearchMethod',
            data: {
                countryfrom: self.CallingFrom,
                //countryfrom: 1,
                countryto: self.CallingTo,
                //countryto: 130,
                //callForwarding: self.IsCallForward,
                callForwarding: false,
                //globalcall: self.IsGlobalCall,
                globalcall: false,

                mobileorGlobal: response
            },
            type: "GET",
            cache: false,
            success: function (data) {
                self.SetServerValues(data);
                //set server values
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //  alert(jqXHR);
            }

        });
    };




    //This GetRate1 Function was Created By Sabal on 11/13/14 to display rates wihout JSON so that meta tags and keywords will display properly on the rate page.
    self.GetRate1 = function () {

        if (self.CallingFrom() == undefined || self.CallingTo() == undefined) {
            return false;
        }
        window.location.href = "searchrate?countryfrom=" + self.CallingFrom() + "&countryto=" + self.CallingTo();
    };



    self.GetPlanRate = function (planname, countryId) {

        $.ajax({
            url: '/Rate/SearchMethod',
            data: {
                countryfrom: self.CallingFrom,
                countryto: countryId,
                // cardTypeName: planname
                callForwarding: self.IsCallForward,
                globalcall: self.IsGlobalCall

            },
            type: "GET",
            cache: false,
            success: function (data) {
                self.MobileDirectCard(data.MobileDirectRateCards);
                self.NewCust_MobileDirect(data.MobileDirectRateCards);
                self.OnetouchDialCard(data.OneTouchDialRateCards);
                self.NewCust_GlobalPlan(data.OneTouchDialRateCards);
                var detach1, detach2, detach3, detach4;

                //when rates secrh for country mobile
                if (data.SearchType == "Mobile") {

                    // for new customer
                    if ($("#container-plan-3-New").find('#open-citydirect').length == 1) {
                        //place citydirect in container plan.
                        //no change
                    } else {
                        // place citydirect after detach
                        //detach1 = $("#open-citydirect").detach();
                        detach2 = $("#new-cust-mobile").detach();
                        //detach1.appendTo("#container-plan-3-New");
                        detach2.appendTo("#container-plan-2-New");
                    }
                    $("#container-plan-2-New").after($('#container-main')).after($('#new-cust-global'));

                    // for recmnd plan
                    if ($("#container-plan-3-recmnd").find('#city-direct-recmnd').length == 1) {
                        //place citydirect in container plan.
                        //no change
                    } else {
                        // place citydirect after detach
                        //detach1 = $("#city-direct-recmnd").detach();
                        detach2 = $("#recomnd-mobile").detach();
                        //detach1.appendTo("#container-plan-3-recmnd");
                        detach2.appendTo("#container-plan-2-recmnd");
                    }
                    $("#new-link").text("LandLine Special");
                    $("#recmnd-link").text("Call Landlines");
                    $("#container-plan-2-recmnd").after($('#container-main-second')).after($('#recomnd-global'));

                }

                // when City type country search
                if (data.SearchType == "City") {
                    //for new customer
                    if ($("#container-plan-3-New").find('#new-cust-mobile').length == 1) {
                        // no change.
                    } else {

                        //detach1 = $("#open-citydirect").detach();
                        detach2 = $("#new-cust-mobile").detach();
                        //detach1.appendTo("#container-plan-2-New");
                        detach2.appendTo("#container-plan-3-New");
                    }

                    //for recmnd
                    if ($("#container-plan-3-recmnd").find('#recomnd-mobile').length == 1) {
                        // no change.
                    } else {

                        //detach1 = $("#city-direct-recmnd").detach();
                        detach2 = $("#recomnd-mobile").detach();
                        //detach1.appendTo("#container-plan-2-recmnd");
                        detach2.appendTo("#container-plan-3-recmnd");
                    }
                    $("#new-link").text("Mobile Direct Plan");
                    $("#recmnd-link").text("Mobile Direct Plan");
                    $("#container-plan-2-recmnd").after($('#container-main-second')).after($('#recomnd-global'));
                    $("#container-plan-2-New").after($('#container-main')).after($('#new-cust-global'));
                }
                //when serach country type is country
                if (data.SearchType == "Country") {
                    // for new customer
                    if ($("#container-plan-3-New").find('#open-citydirect').length == 1) {
                        //place citydirect in container plan.
                        //no change
                    } else {
                        // place citydirect after detach
                        //detach1 = $("#open-citydirect").detach();
                        detach2 = $("#new-cust-mobile").detach();
                        //detach1.appendTo("#container-plan-3-New");
                        detach2.appendTo("#container-plan-2-New");
                    }
                    $("#new-cust-global").after($('#container-main')).after($('#container-plan-2-New'));

                    // for recmnd plan
                    if ($("#container-plan-3-recmnd").find('#city-direct-recmnd').length == 1) {
                        //place citydirect in container plan.
                        //no change
                    } else {
                        // place citydirect after detach
                        //detach1 = $("#city-direct-recmnd").detach();
                        detach2 = $("#recomnd-mobile").detach();
                        //detach1.appendTo("#container-plan-3-recmnd");
                        detach2.appendTo("#container-plan-2-recmnd");
                    }
                    $("#new-link").text("LandLine Special");
                    $("#recmnd-link").text("LandLine Special");
                    $("#recomnd-global").after($('#container-main-second')).after($('#container-plan-2-recmnd'));

                }

                $('html,body').animate({ scrollTop: 0 }, 'slow', function () {
                });
                self.CollapseContainerNew();
                self.CollapseContainerRecmnd();

                $("div.ramadan-div2").css("display", "");

            }
        });
    };

    $("#recmnd-link").click(function () {
        if (self.iscollapseRecmnd() == true) {
            self.iscollapseRecmnd(false);
        } else {
            self.iscollapseRecmnd(true);
        }

        // alert(iscollapse);
    });
    $("#new-link").click(function () {
        if (self.iscollapseNew() == true) {
            self.iscollapseNew(false);
        } else {
            self.iscollapseNew(true);
        }
        // alert(iscollapse);
    });

    self.CollapseContainerNew = function () {
        if (self.iscollapseNew() == true) {
            $("#new-link").trigger("click");
            //  iscollapseRecmnd = false;
        }
    };
    self.CollapseContainerRecmnd = function () {
        if (self.iscollapseRecmnd() == true) {
            $("#recmnd-link").trigger("click");
            //  iscollapseNew = false;
        }
    };








    //self.GetMobileRate = function (data) {
    //    self.GetPlanRate(data.RateType, data.Id);
    //};





    self.GetMobileRate = function (data) {

        //var qs = getQueryStrings();
        //if (qs["countryfrom"] != undefined && qs["countryfrom"] != null) {
        //    var fromCountry = qs["countryfrom"];
        //    var toCountry = qs["countryto"];
        //    window.location = "SearchRate?countryfrom=" + fromCountry + "&countryto=" + data.Id;
        //}
        //else {
        //    self.GetPlanRate(data.RateType, data.Id);
        //}


        var qs = getQueryStrings();
        if (qs["countryfrom"] != undefined && qs["countryfrom"] != null) {
            var fromCountry = "1";
            var toCountry = data.Id;
            window.location = "SearchRate?countryfrom=" + fromCountry + "&countryto=" + data.Id;
        }
        else {
            self.GetPlanRate(data.RateType, data.Id);
        }

    };

}


function getQueryStrings() {
    //var assoc = {};
    //var decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
    //var queryString = location.search.substring(1);
    //var keyValues = queryString.split('&');

    //for (var i in keyValues) {
    //    var key = keyValues[i].split('=');
    //    if (key.length > 1) {
    //        assoc[decode(key[0])] = decode(key[1]);
    //    }
    //}

    //return assoc;


    var assoc = {};

    assoc["countryfrom"] = $("#countryfromid").val();

    //var PathName = window.location.pathname;
    //if (PathName.toLowerCase().indexOf("india") > -1)
    //    assoc["countryto"] = "130";
    //else if (PathName.toLowerCase().indexOf("nepal") > -1)
    //    assoc["countryto"] = "224";
    //else if (PathName.toLowerCase().indexOf("pakistan") > -1)
    //    assoc["countryto"] = "238";
    //else if (PathName.toLowerCase().indexOf("bangladesh") > -1)
    //    assoc["countryto"] = "27";
    //else
    //    assoc["countryto"] = "314";

    //assoc["countryto"] = countryto;
    //assoc["countryto"] = CountryTooID;
    assoc["countryto"] = $("#hcountryfrom_raza").val()

    assoc["globalcall"] = "false";
    assoc["callForwarding"] = "false";
    return assoc;
}


function setSelectedIndex(s, valsearch) {
    // Loop through all the items in drop down list
    for (var i = 0; i < s.options.length; i++) {
        if (s.options[i].value == valsearch) {
            // Item is found. Set its property and exit
            s.options[i].selected = true;
            break;
        }
    }
    return;
}



