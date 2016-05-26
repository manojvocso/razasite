$(document).ajaxSend(function (event, request, settings) {
    $('#load-blk').css('display', '');

});

$(document).ajaxComplete(function (event, request, settings) {
    $('#load-blk').css('display', 'none');
   
});


var model = new RateModel();
var cid = $("#country-byip").val();
$(document).ready(function () {
    var list = [];
    var predata1 = [];

    $.ajax({

        url: "/Account/CountryToListwithFlag/",
        type: 'GET',
        success: function (data) {
            predata1 = data;
            model.ToCountryLoaded(true);

            if (model.ToCountryLoaded() == true) {
                if (location.search.length > 1) {
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
                   // $('#rate-main').block({ message: null });
                }
            }
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
                //SortDir: 'asc',
               
                value: [ctx],
                //renderer: function (v) {
                //    return '<div>' +
                //        '<div style="float:left;">' +
                //        v.cname + '(+' + v.Code + ')' +
                //        '</div><div style="clear:both;"></div>';
                //},
                renderer: function (data) {
                    return '<div><a><i class="magic_' + data.id + '"><p>' + data.name + '(+' + data.Code + ')'  + '</p></i></a></div>';
                },
                maxSelectionRenderer: function () {
                }

            });

            $(searchrate).on('selectionchange', function (event, selection) {
                var selected = searchrate.getSelectedItems();
                var countryto = selected[0].id;
                model.CallingTo(countryto);

                // var countrycode = selected[0].Code;


            });
        }
    });
});
// document ready function
$(document).ready(function () {


    $(document).ajaxSend(function (event, request, settings) {
        $('#load-blk').css('display', '');
    });

    $(document).ajaxComplete(function (event, request, settings) {
       $('#load-blk').css('display', 'none');
    });

    $("#clickk").hover(function () {
        $("#monster").slideToggle("fast");
    });
    
    $("#tabAndroid").trigger("click");
     
    $("#close-val").click({ handler: model.Disablepopup });
    $("#backgroundPopup").click({ handler: model.Disablepopup });

    $("#Search-button").click({ handler: model.GetRate });
    //$("#one-touch-autorefill").change({ handler: model.one });
   // $("#onetouchbuy").click({ handler: model.BuyOnetouch });

    ko.applyBindings(model, document.getElementById("rate-bind-search"));

    //  model.GetCountryListFrom();
    // model.FetchCountryListTo();
    // model.GetDealsSearch();
    // model.autorefill();
    model.HideCityDirectPlan();


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
    self.CityDirectCard = ko.observableArray([]);
    self.OnetouchDialCard = ko.observableArray([]);

    self.NewCust_MobileDirect = ko.observableArray([]);
    self.NewCust_CityDirect = ko.observableArray([]);
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
    self.CountryCityList = ko.observableArray([]);
    
    self.PreList = ko.observableArray([]);
    self.PreList1 = ko.observableArray([]);
    self.PreList2 = ko.observableArray([]);

    self.OneTouchCardAutoRefill = ko.observable(true);
    self.MobileDirectAutoRefill = ko.observable(true);
    self.CityDirectAutoRefill = ko.observable(true);

    self.FromCountryLoaded = ko.observable(false);
    self.ToCountryLoaded = ko.observable(false);

    self.RatePerMinMobileDirect = ko.observable(0);
    self.RatePerMinCityDirect = ko.observable(0);
    self.RatePerMinOneTouch = ko.observable(0);

    self.Newcust_RatePerMinMobileDirect = ko.observable(0);
    self.NewCust_RatePerMinCityDirect = ko.observable(0);
    self.NewCust_RatePerMinGlobal = ko.observable(0);
    //Indiacentplan variables
    self.IndiaCentPlan = ko.observableArray([]);
    self.Minuteval1 = ko.observable();
    self.Minuteval2 = ko.observable();
    self.Minuteval3 = ko.observable();
    self.ServiceFee1 = ko.observable();
    self.ServiceFee2 = ko.observable();
    self.ServiceFee3 = ko.observable();
    self.TotalCharge1 = ko.observable();
    self.TotalCharge2 = ko.observable();
    self.TotalCharge3 = ko.observable();
    self.CurrencyCountryCode = ko.observable();
    //ends here variable of indiacentplan
  
    //Unlimited india plan
    self.UnlimitedIndiaPlan = ko.observableArray([]);
    self.UnlimitedPlanFee1 = ko.observable();
    self.UnlimitedPlanFee2 = ko.observable();
    self.UnlimitedPlanFee3 = ko.observable();
    self.UnlimitedPlanMinute1 = ko.observable();
    self.UnlimitedPlanMinute2 = ko.observable();
    self.UnlimitedPlanMinute3 = ko.observable();
    self.UnlimtedPlanCurrency=ko.observable();


    //ends here

    self.RatePerMinSign = ko.observable("");
   
    //calculation for the new customer global plan.
    self.Calc_onetouchautorefill = ko.computed(function() {
        var i = 0;
        ko.utils.arrayForEach(self.NewCust_GlobalPlan(), function(item) {
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
            i++;
        });
    });

    // new customer mobile direct plan calculation.
    self.Calc_MobileDirectautorefill = ko.computed(function() {
        var i = 0;
        ko.utils.arrayForEach(self.NewCust_MobileDirect(), function(item) {
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
            
            i++;
        });

    });

    // new customer city direct plan calculation.
    self.Calc_CityDirectautorefill = ko.computed(function() {
        var i = 0;

        ko.utils.arrayForEach(self.NewCust_CityDirect(), function(item) {

            var min2 = Math.round(item.TotalMinutes + (item.TotalMinutes * item.Discount / 100));

            if (i == 0) {

                self.NewCust_RatePerMinCityDirect(Math.round(((item.PlanAmount / min2) * 100) * item.Discount) / item.Discount);
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
    });


    // calculation for onetouch plan
    self.IsautorefillOnetouch = ko.computed(function() {
        var t = self.OneTouchCardAutoRefill();
        if (t == 1) { // calculation for autorefill plan.
            var i = 0;
            self.PreList([]);
            self.PreList(self.OnetouchDialCard());

            ko.utils.arrayForEach(self.OnetouchDialCard(), function(item) {

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

            ko.utils.arrayForEach(self.PreList(), function(item) {

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
    self.IsautorefillMobileDirect = ko.computed(function() {
        var t = self.MobileDirectAutoRefill();
        if (t == 1) { // for autorefill plan.
            var i = 0;
            self.PreList1([]);
            self.PreList1(self.MobileDirectCard());
            var ratepermin;
            ko.utils.arrayForEach(self.PreList1(), function(item) {
                
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
            ko.utils.arrayForEach(self.PreList1(), function(item) {
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
    // calculation end for mobile direct plan.
    // calculation start for city direct plan.
    self.IsautorefillCityDirect = ko.computed(function() {
        var t = self.CityDirectAutoRefill();
        if (t == 1) { // Autorefil plan
            var i = 0;
            self.PreList2([]);
            self.PreList2(self.CityDirectCard());
            
            ko.utils.arrayForEach(self.PreList2(), function(item) {
                var  min2 = Math.round(item.TotalMinutes + (item.TotalMinutes * item.Discount / 100));
                if (i == 0) {
                    self.RatePerMinCityDirect(Math.round(((item.PlanAmount / min2) * 100) * 10) / 10);
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
            self.CityDirectCard([]);
            self.CityDirectCard(self.PreList2());
        } else {
            var j = 0;
            self.PreList2([]);
            self.PreList2(self.CityDirectCard());
            
            ko.utils.arrayForEach(self.PreList2(), function(item) {

                if (j == 0) {
                    self.RatePerMinCityDirect(item.RatePerMin);
                    if (self.CallingFrom() == 3 || self.CallingFrom() == "3") {
                        item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(0));
                    } else {
                        item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(100));
                    }
                    // item.StrikeoutAmount = parseFloat(ko.utils.unwrapObservable(100));
                    item.Minute = Math.round(parseFloat(ko.utils.unwrapObservable(item.TotalMinutes)));
                } else {
                   item.Minute = Math.round(parseFloat(ko.utils.unwrapObservable(item.TotalMinutes)));
                }

                item.IsAutoRefill = false;
                j++;
            });
            self.CityDirectCard([]);
            self.CityDirectCard(self.PreList2());
          }
    });

    self.CityDirectAutoRefill.subscribe(self.IsautorefillCityDirect); // subscribe radiobutton for mobiledirect in recommended plans.
    // calculation end for city direct plan.


    self.GetCountryListFrom = function() {
        var url = '/Account/GetCountryToCountryFromList';
        $.getJSON(url, function(data) {
            self.CountryListFrom(data);
            self.FromCountryLoaded(true);

            if (self.FromCountryLoaded() == true && self.ToCountryLoaded() == true) {
                if (location.search.length > 1) {
                    var qs = getQueryStrings();
                    

                    var fromCountry = qs["countryfrom"];
                    var toCountry = qs["countryto"];
                    var globalcall = qs["globalcall"];
                    var callforwarding = qs["callForwarding"];

                    self.CallingFrom(fromCountry);
                    self.CallingTo(toCountry);
                    self.IsCallForward(callforwarding);
                    self.IsGlobalCall(globalcall);

                    self.GetRate();
                }
            }


        });
    };

    self.BuyPlan = function (data) {

        $.ajax({
            url: '/Cart/BuyPlan',
            data: {
                PlanId: data.PlanId,
                FromToMapping: data.FromToMapping,
                FromCountry: data.CountryFrom,
                ToCountry: data.CountryTo,
                AutoRefill: data.IsAutoRefill,
                IsfromSerchrate: true
    },
            type: "POST",
            success: function(returndata) {
                if (returndata.ok) {
                    window.location = returndata.newurl;
                }
              
            },
        });
    };

    self.FetchCountryListTo = function() {
        $.getJSON('/Account/GetCountryListFull/', null, function(data) {
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

                    self.GetRate();
                }
            }
        });
    };
   
    self.GetRate = function () {
        //alert("hi");
        //if (self.CallingFrom() == "") {
        //    var res = $("#countryfromid").val();
        //    self.CallingFrom(res);

        //}
        //else {
        //    self.CallingFrom();
        //}
     
        if ( self.CallingFrom() == undefined || self.CallingTo() == undefined ) {
            return false;
        }
        
        $('#load-blk').css('display', '');

        var random = document.getElementById('banner-india');
        if (self.CallingTo() == 130 || self.CallingTo() == 132 || self.CallingTo() == 672 || self.CallingTo() == 159 || self.CallingTo() == 574 || self.CallingTo() == 161
           || self.CallingTo() == 163 || self.CallingTo() == 131 || self.CallingTo() == 162 || self.CallingTo() == 145 || self.CallingTo() == 146
            || self.CallingTo() == 133 || self.CallingTo() == 844 || self.CallingTo() == 160 || self.CallingTo() == 164 || self.CallingTo() == 575
            || self.CallingTo() == 355 || self.CallingTo() == 673) {
            if (document.images) {
                random.style.background = 'url(../images/india1.jpg)';

            }
        }
        else if (self.CallingTo() == 1 || self.CallingTo() == 347) {
            if (document.images) {
                random.style.background = 'url(../images/afganisthan.jpg)';

            }
        }
        else if (self.CallingTo() == 27 || self.CallingTo() == 28 || self.CallingTo() == 29 || self.CallingTo() == 30 || self.CallingTo() == 31 || self.CallingTo() == 501 || self.CallingTo() == 702) {
            if (document.images) {
                random.style.background = 'url(../images/bangladesh1.jpg)';

            }
        }
        else if (self.CallingTo() == 57 || self.CallingTo() == 727 || self.CallingTo() == 728) {
            if (document.images) {
                random.style.background = 'url(../images/canada1.jpg)';

            }
        }
        else if (self.CallingTo() == 224 || self.CallingTo() == 225 || self.CallingTo() == 330 || self.CallingTo() == 1149 || self.CallingTo() == 1150) {
            if (document.images) {
                random.style.background = 'url(../images/nepal1.jpg)';

            }
        }
        else if (self.CallingTo() == 238 || self.CallingTo() == 612 || self.CallingTo() == 366 || self.CallingTo() == 239 || self.CallingTo() == 240) {
            if (document.images) {
                random.style.background = 'url(../images/Pakistan1.jpg)';

            }
        }
        else if (self.CallingTo() == 248 || self.CallingTo() == 616 || self.CallingTo() == 617 || self.CallingTo() == 241 || self.CallingTo() == 1184 || self.CallingTo() == 1185 || self.CallingTo() == 1186 || self.CallingTo() == 1187) {
            if (document.images) {
                random.style.background = 'url(../images/philippines1.jpg)';

            }
        }
        else if (self.CallingTo() == 262 || self.CallingTo() == 264 || self.CallingTo() == 265 || self.CallingTo() == 362) {
            if (document.images) {
                random.style.background = 'url(../images/saudiarabia1.jpg)';

            }
        }
        else if (self.CallingTo() == 1221 || self.CallingTo() == 1222 || self.CallingTo() == 281 || self.CallingTo() == 283 || self.CallingTo() == 360) {
            if (document.images) {
                random.style.background = 'url(../images/srilanka1.jpg)';

            }
        }
        else if (self.CallingTo() == 312 || self.CallingTo() == 1262 || self.CallingTo() == 1263 || self.CallingTo() == 1264 || self.CallingTo() == 1265 || self.CallingTo() == 1266 || self.CallingTo() == 1267 || self.CallingTo() == 1268
             || self.CallingTo() == 1269 || self.CallingTo() == 310 || self.CallingTo() == 356) {
            if (document.images) {
                random.style.background = 'url(../images/UK1.jpg)';

            }
        }
        else if (self.CallingTo() == 309 || self.CallingTo() == 376) {
            if (document.images) {
                random.style.background = 'url(../images/UAE2.jpg)';

            }
        }
        else if (self.CallingTo() == 91 || self.CallingTo() == 783 || self.CallingTo() == 784 || self.CallingTo() == 408) {
            if (document.images) {
                random.style.background = 'url(../images/ethiopia1.jpg)';

            }
        }
        else if (self.CallingTo() == 87 || self.CallingTo() == 550 || self.CallingTo() == 551 || self.CallingTo() == 454) {
            if (document.images) {
                random.style.background = 'url(../images/egypt1.jpg)';

            }
        }
        else if (self.CallingTo() == 888 || self.CallingTo() == 889 || self.CallingTo() == 890 || self.CallingTo() == 891 || self.CallingTo() == 892 || self.CallingTo() == 893 || self.CallingTo() == 180
              || self.CallingTo() == 181 || self.CallingTo() == 182) {
            if (document.images) {
                random.style.background = 'url(../images/kenya1.jpg)';

            }
        }
        else if (self.CallingTo() == 805 || self.CallingTo() == 806 || self.CallingTo() == 807 || self.CallingTo() == 350 || self.CallingTo() == 563 || self.CallingTo() == 110) {
            if (document.images) {
                random.style.background = 'url(../images/ghana1.jpg)';

            }
        }
        else if (self.CallingTo() == 456 || self.CallingTo() == 457 || self.CallingTo() == 877 || self.CallingTo() == 878 || self.CallingTo() == 879 || self.CallingTo() == 178) {
            if (document.images) {
                random.style.background = 'url(../images/jordan1.jpg)';

            }
        }
        else if (self.CallingTo() == 1166 || self.CallingTo() == 1167 || self.CallingTo() == 1168 || self.CallingTo() == 1169 || self.CallingTo() == 1170 || self.CallingTo() == 232 || self.CallingTo() == 233 || self.CallingTo() == 365) {
            if (document.images) {
                random.style.background = 'url(../images/nigeria1.jpg)';

            }
        }
        else if (self.CallingTo() == 930 || self.CallingTo() == 931 || self.CallingTo() == 932 || self.CallingTo() == 933 || self.CallingTo() == 934 || self.CallingTo() == 209 || self.CallingTo() == 935 || self.CallingTo() == 936 ||
            self.CallingTo() == 937 || self.CallingTo() == 938 || self.CallingTo() == 939 || self.CallingTo() == 940 || self.CallingTo() == 941 || self.CallingTo() == 942 || self.CallingTo() == 943 || self.CallingTo() == 944 ||
            self.CallingTo() == 945 || self.CallingTo() == 946 || self.CallingTo() ==947 || self.CallingTo() == 948 || self.CallingTo() == 949 || self.CallingTo() == 950 || self.CallingTo() == 951 || self.CallingTo() == 952 ||
            self.CallingTo() == 953 || self.CallingTo() == 954 || self.CallingTo() == 955 || self.CallingTo() == 956 || self.CallingTo() == 957 || self.CallingTo() == 958 || self.CallingTo() == 959 || self.CallingTo() == 960 ||
            self.CallingTo() == 961 || self.CallingTo() == 962 || self.CallingTo() == 963 || self.CallingTo() == 964 || self.CallingTo() == 965 || self.CallingTo() == 966 || self.CallingTo() ==967 || self.CallingTo() == 968 ||
            self.CallingTo() == 969 || self.CallingTo() == 970 || self.CallingTo() == 971 || self.CallingTo() == 972 || self.CallingTo() == 973 || self.CallingTo() == 974 || self.CallingTo() == 975 || self.CallingTo() == 976 ||
            self.CallingTo() == 977 || self.CallingTo() == 978 || self.CallingTo() == 979 || self.CallingTo() == 980 || self.CallingTo() == 981 || self.CallingTo() == 982 || self.CallingTo() == 983 || self.CallingTo() == 983 ||
            self.CallingTo() == 985 || self.CallingTo() == 986 || self.CallingTo() == 987 || self.CallingTo() == 988 || self.CallingTo() == 989 || self.CallingTo() == 990 || self.CallingTo() == 991 || self.CallingTo() == 992||
            self.CallingTo() == 993 || self.CallingTo() == 994 || self.CallingTo() == 995 || self.CallingTo() == 996 || self.CallingTo() == 997 || self.CallingTo() == 998 || self.CallingTo() == 999 || self.CallingTo() == 1000 ||
            self.CallingTo() == 1001 || self.CallingTo() == 1002 || self.CallingTo() == 1003 || self.CallingTo() == 1004 || self.CallingTo() == 1005 || self.CallingTo() == 1006 || self.CallingTo() == 1007 || self.CallingTo() == 1008 ||
            self.CallingTo() == 1009 || self.CallingTo() == 1010 || self.CallingTo() == 1011 || self.CallingTo() == 1012 || self.CallingTo() == 1013 || self.CallingTo() == 1014 || self.CallingTo() == 1015 || self.CallingTo() == 1016||
            self.CallingTo() == 1017 || self.CallingTo() == 1018 || self.CallingTo() == 1019 || self.CallingTo() == 1020 || self.CallingTo() == 1021 || self.CallingTo() == 1022 || self.CallingTo() == 1023 || self.CallingTo() == 1024||
            self.CallingTo() == 1025 || self.CallingTo() == 1026 || self.CallingTo() == 1027 || self.CallingTo() == 1028 || self.CallingTo() == 1029 || self.CallingTo() == 1030 || self.CallingTo() == 1031 || self.CallingTo() == 1032 ||
            self.CallingTo() == 1033 || self.CallingTo() == 1034 || self.CallingTo() == 1035 || self.CallingTo() == 1036 || self.CallingTo() == 1037 || self.CallingTo() ==1038 || self.CallingTo() == 1039 || self.CallingTo() == 1040 ||
            self.CallingTo() == 1041 || self.CallingTo() == 1042 || self.CallingTo() == 1043 || self.CallingTo() == 1044 || self.CallingTo() == 1045 || self.CallingTo() == 1046 || self.CallingTo() == 1047 || self.CallingTo() == 1048 ||
            self.CallingTo() == 1049 || self.CallingTo() == 1050 || self.CallingTo() == 1051 || self.CallingTo() == 1052 || self.CallingTo() == 1053 || self.CallingTo() ==1054 || self.CallingTo() == 1055 || self.CallingTo() == 1056 ||
            self.CallingTo() == 1057 || self.CallingTo() == 1058 || self.CallingTo() == 1059 || self.CallingTo() == 1060 || self.CallingTo() == 1061 || self.CallingTo() ==1062 || self.CallingTo() == 1063 || self.CallingTo() == 1064 ||
            self.CallingTo() == 1065 || self.CallingTo() == 1066 || self.CallingTo() == 1067 || self.CallingTo() == 1068 || self.CallingTo() == 1069 || self.CallingTo() ==1070 || self.CallingTo() == 1071 || self.CallingTo() == 1072 ||
            self.CallingTo() == 1073 || self.CallingTo() == 1074 || self.CallingTo() == 1075 || self.CallingTo() == 1076 || self.CallingTo() == 1077 || self.CallingTo() ==1078 || self.CallingTo() == 1079 || self.CallingTo() == 1080 ||
            self.CallingTo() == 1081 || self.CallingTo() == 1082 || self.CallingTo() == 1083 || self.CallingTo() == 1084 || self.CallingTo() == 1085 || self.CallingTo() ==1086 || self.CallingTo() == 1087 || self.CallingTo() == 1088 ||
            self.CallingTo() == 1089 || self.CallingTo() == 1090 || self.CallingTo() == 1091 || self.CallingTo() == 1092 || self.CallingTo() == 1093 || self.CallingTo() ==1094 || self.CallingTo() == 1095 || self.CallingTo() == 1096 ||
            self.CallingTo() == 1097 || self.CallingTo() == 1098 || self.CallingTo() == 1099 || self.CallingTo() == 1100 || self.CallingTo() == 1101 || self.CallingTo() ==1102 || self.CallingTo() == 1103 || self.CallingTo() == 1104 ||
            self.CallingTo() == 1105 || self.CallingTo() == 1106 || self.CallingTo() == 1107 || self.CallingTo() == 1108 || self.CallingTo() == 1109 || self.CallingTo() ==1110 || self.CallingTo() == 1111 || self.CallingTo() == 1112 ||
            self.CallingTo() == 1113 || self.CallingTo() == 1114 || self.CallingTo() == 1115 || self.CallingTo() == 1116 || self.CallingTo() == 1117 || self.CallingTo() ==1118 || self.CallingTo() == 1119 || self.CallingTo() == 1120 ||
            self.CallingTo() == 1121 || self.CallingTo() == 1122 || self.CallingTo() == 1123 || self.CallingTo() == 1124 || self.CallingTo() == 1125 || self.CallingTo() ==1126 || self.CallingTo() == 1127 || self.CallingTo() == 1128 ||
            self.CallingTo() == 1129 || self.CallingTo() == 1130 || self.CallingTo() == 211|| self.CallingTo() == 212 || self.CallingTo() == 666 ) {
            if (document.images) {
                random.style.background = 'url(../images/mexico1.jpg)';

            }
        }
        else {
            if (document.images) {
                random.style.background = 'url(../images/rate-save-big-banner.jpg)';

            }
        }
        var response = $("#MobileorGlobal").val();
        
   
        $.ajax({
            url: '/Rate/SearchMethod',
            data: {
                countryfrom: self.CallingFrom,
                countryto: self.CallingTo,
                callForwarding: self.IsCallForward,
                globalcall: self.IsGlobalCall,
                mobileorGlobal:response
            },
            type: "GET",
            cache: false,
            success: function (data) {
                
                $("#MobileorGlobal").val("");
                
                self.RatePerMinMobileDirect(0);
                self.RatePerMinCityDirect(0);
                self.RatePerMinOneTouch(0);
                self.MobileDirectCard(data.MobileDirectRateCards);
                self.CityDirectCard(data.CityDirectRateCards);
                self.OnetouchDialCard(data.OneTouchDialRateCards);
               
                
                //list for country city.
                self.MobileCityList(data.MobileCityList);
                self.CountryMobileList(data.CountryMobileList);
                self.CountryCityList(data.CountryCityList);
                //indiaCentPlan dynamic data starts here
                
                if (data.CountryCode == 91) {
                    
                    $("#ind-plan").css("display", "");

                    if (self.CallingFrom() == 1) {
                        //$("#Unlimited_planid").val(175);
                        //$("#Unlimited14.99").val("177");
                        //$("#Unlimited19.99").val("179");

                        $(".add_countryfrom").val(1);
                        $(".add_countryto").val(130);
                        $(".add_currencycode").val(self.CurrencyCountryCode());

                    } else if (self.CallingFrom() == 2) {
                        //$("#Unlimited9.99").val("176");
                        //$("#Unlimited14.99").val("178");
                        //$("#Unlimited19.99").val("180");
                    
                        $(".add_countryfrom").val(2);
                        $(".add_countryto").val(130);
                        $(".add_currencycode").val(self.CurrencyCountryCode());
                    }
                    //  <input type="hidden" id="Unlimited19.99" name="planid" />
                    //  <input type="hidden"  id="planname19.99" value="" name="planname" />
                    //  <input type="hidden" class="add_countryfrom" name="countryfrom" />
                    //  <input type="hidden" class="add_countryto" name="countryto" />
                    //  <input type="hidden" class="add_currencycode" name="currencycode" />


                    //self.IndiaCentPlan(data.IndiaCentPlan);
                    //self.Minuteval1(data.IndiaCentPlan[0].TotalMinute);
                    //self.Minuteval2(data.IndiaCentPlan[1].TotalMinute);
                    //self.Minuteval3(data.IndiaCentPlan[2].TotalMinute);
                    //self.ServiceFee1(data.IndiaCentPlan[0].ServiceFee);
                    //self.ServiceFee2(data.IndiaCentPlan[1].ServiceFee);
                    //self.ServiceFee3(data.IndiaCentPlan[2].ServiceFee);
                    //self.TotalCharge1(data.IndiaCentPlan[0].TotalCharge);
                    //self.TotalCharge2(data.IndiaCentPlan[1].TotalCharge);
                    //self.TotalCharge3(data.IndiaCentPlan[2].TotalCharge);
                    //self.CurrencyCountryCode(data.IndiaCentPlan[0].CurrencyCode);

                    ////Indiacent plan dynamic data ends here
                    ////unlimited india plan
                    //self.UnlimitedIndiaPlan(data.UnlimitedIndiaPlan);
                    //self.UnlimitedPlanFee1(data.UnlimitedIndiaPlan[0].ServiceFee);
                    //self.UnlimitedPlanFee2(data.UnlimitedIndiaPlan[1].ServiceFee);
                    //self.UnlimitedPlanFee3(data.UnlimitedIndiaPlan[2].ServiceFee);
                    //self.UnlimitedPlanMinute1(data.UnlimitedIndiaPlan[0].TotalMinute);
                    //self.UnlimitedPlanMinute2(data.UnlimitedIndiaPlan[1].TotalMinute);
                    //self.UnlimitedPlanMinute3(data.UnlimitedIndiaPlan[2].TotalMinute);

                    //unlimited india plans data ends here
                } else {
                    $("#ind-plan").css("display", "none");
                }
                
                self.NewCust_MobileDirect(data.MobileDirectRateCards);
                self.NewCust_CityDirect(data.CityDirectRateCards);
                self.NewCust_GlobalPlan(data.OneTouchDialRateCards);

                self.RatePerMinSign(data.RateperMinSign);
                
                $("#container-main").css("display", "");
                $("#container-main-second").css("display", "");
                //$( "#tabAndroid" ).trigger("click");
              
                    $("div.ramadan-div2").css("display", "");

                self.HideCityDirectPlan();
                 // $("#new-cust-global").after($('#city-direct-new')).after($('#new-cust-mobile'));
                //$("#recomnd-global").after($('#city-direct-recmnd')).after($('#recomnd-mobile'));
                 
                var detach1, detach2, detach3, detach4;
               
                if (data.SearchType == "Mobile") {
                  
                  // for new customer
                    if ($("#container-plan-3-New").find('#open-citydirect').length == 1) {
                        //place citydirect in container plan.
                        //no change
                    } else {
                        // place citydirect after detach
                        detach1 = $("#open-citydirect").detach();
                        detach2 = $("#new-cust-mobile").detach();
                        detach1.appendTo("#container-plan-3-New");
                        detach2.appendTo("#container-plan-2-New");
                    }
                    $("#container-plan-2-New").after($('#container-main')).after($('#new-cust-global'));

                    // for recmnd plan
                    if ($("#container-plan-3-recmnd").find('#city-direct-recmnd').length == 1) {
                        //place citydirect in container plan.
                        //no change
                    } else {
                        // place citydirect after detach
                        detach1 = $("#city-direct-recmnd").detach();
                        detach2 = $("#recomnd-mobile").detach();
                        detach1.appendTo("#container-plan-3-recmnd");
                        detach2.appendTo("#container-plan-2-recmnd");
                    }
                    $("#new-link").text("LandLine Special");
                    $("#recmnd-link").text("LandLine Special");
                    $("#container-plan-2-recmnd").after($('#container-main-second')).after($('#recomnd-global'));
                }

                if (data.SearchType=="City") {
                     //for new customer
                    if ($("#container-plan-3-New").find('#new-cust-mobile').length == 1) {
                    // no change.
                    } else {

                        detach1 = $("#open-citydirect").detach();
                        detach2 = $("#new-cust-mobile").detach();
                        detach1.appendTo("#container-plan-2-New");
                        detach2.appendTo("#container-plan-3-New");
                    }
                    
                    //for recmnd
                     if ($("#container-plan-3-recmnd").find('#recomnd-mobile').length == 1) {
                    // no change.
                    } else {

                        detach1 = $("#city-direct-recmnd").detach();
                        detach2 = $("#recomnd-mobile").detach();
                        detach1.appendTo("#container-plan-2-recmnd");
                        detach2.appendTo("#container-plan-3-recmnd");
                    }
                     $("#new-link").text("Mobile Direct Plan");
                     $("#recmnd-link").text("Mobile Direct Plan");
                     $("#container-plan-2-recmnd").after($('#container-main-second')).after($('#recomnd-global'));
                     $("#container-plan-2-New").after($('#container-main')).after($('#new-cust-global'));

                }

                if(data.SearchType=="Country"){
                    
                    // for new customer
                    if ($("#container-plan-3-New").find('#open-citydirect').length == 1) {
                        //place citydirect in container plan.
                        //no change
                    } else {
                        // place citydirect after detach
                        detach1 = $("#open-citydirect").detach();
                        detach2 = $("#new-cust-mobile").detach();
                        detach1.appendTo("#container-plan-3-New");
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
                        detach1.appendTo("#container-plan-3-recmnd");
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
            },
            error: function (jqXHR, textStatus, errorThrown) {
              //  alert(jqXHR);
            }
            
        });
    };

    self.GetPlanRate = function(planname, countryId) {
     
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
            success: function(data) {
                    self.MobileDirectCard(data.MobileDirectRateCards);
                    self.NewCust_MobileDirect(data.MobileDirectRateCards);
                    self.CityDirectCard(data.CityDirectRateCards);
                    self.NewCust_CityDirect(data.CityDirectRateCards);
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
                        detach1 = $("#open-citydirect").detach();
                        detach2 = $("#new-cust-mobile").detach();
                        detach1.appendTo("#container-plan-3-New");
                        detach2.appendTo("#container-plan-2-New");
                    }
                    $("#container-plan-2-New").after($('#container-main')).after($('#new-cust-global'));

                    // for recmnd plan
                    if ($("#container-plan-3-recmnd").find('#city-direct-recmnd').length == 1) {
                        //place citydirect in container plan.
                        //no change
                    } else {
                        // place citydirect after detach
                        detach1 = $("#city-direct-recmnd").detach();
                        detach2 = $("#recomnd-mobile").detach();
                        detach1.appendTo("#container-plan-3-recmnd");
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

                        detach1 = $("#open-citydirect").detach();
                        detach2 = $("#new-cust-mobile").detach();
                        detach1.appendTo("#container-plan-2-New");
                        detach2.appendTo("#container-plan-3-New");
                    }
                    
                    //for recmnd
                     if ($("#container-plan-3-recmnd").find('#recomnd-mobile').length == 1) {
                    // no change.
                    } else {

                        detach1 = $("#city-direct-recmnd").detach();
                        detach2 = $("#recomnd-mobile").detach();
                        detach1.appendTo("#container-plan-2-recmnd");
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
                        detach1 = $("#open-citydirect").detach();
                        detach2 = $("#new-cust-mobile").detach();
                        detach1.appendTo("#container-plan-3-New");
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
                        detach1.appendTo("#container-plan-3-recmnd");
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
                //  self.HideCityDirectPlan();
                
            }
        });
    };

    $("#recmnd-link").click(function() {
        if (self.iscollapseRecmnd() == true) {
            self.iscollapseRecmnd(false);
        } else {
            self.iscollapseRecmnd(true);
        }
        
       // alert(iscollapse);
    });
    $("#new-link").click(function() {
        if (self.iscollapseNew() == true) {
            self.iscollapseNew(false);
        } else {
            self.iscollapseNew(true);
        }
       // alert(iscollapse);
    });

    self.CollapseContainerNew = function() {
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

    self.HideCityDirectPlan = function() {
        if (self.MobileCityList().length > 0) {
            $("#city-direct-new").css("display", "");
            $("#city-direct-recmnd").css("display", "");
        } else {
            $("#city-direct-new").css("display", "none");
            $("#city-direct-recmnd").css("display", "none");
        }
    };

    self.GetMobileRate = function(data) {
        self.GetPlanRate(data.RateType, data.Id);

    };

    self.GetCityRate = function(data) {
        self.GetPlanRate(data.RateType, data.Id);
       
    };

}


function getQueryStrings() {
        var assoc = {};
        var decode = function(s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
        var queryString = location.search.substring(1);
        var keyValues = queryString.split('&');

        for (var i in keyValues) {
            var key = keyValues[i].split('=');
            if (key.length > 1) {
                assoc[decode(key[0])] = decode(key[1]);
            }
        }

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
  
  

