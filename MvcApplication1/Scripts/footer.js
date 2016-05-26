
$(document).ready(function () {
    alert("hi");
    var footerViewModel = new FooterViewModel();

    ko.applyBindings(footerViewModel, document.getElementById("ftrtabs-1"));
//    $("#get-freetrial").click({ handler: footerViewModel.GetFreeTrail });
    
    // Always fill From list
//    footerViewModel.GetCountryListFrom();
    footerViewModel.GetFreeTrialCountryList();
    //    footerViewModel.CountryListTofooter();
    footerViewModel.FetchCountryList();

});

function FooterViewModel() {
    var self = this;
//    self.CountryListFromTryUsFree = ko.observableArray([]);
    self.TrialCountries = ko.observableArray([]);
//    self.CountryListTofooter = ko.observableArray([]);
//    self.Trialtocountry = ko.observable("");
//    self.Trialfromcountry = ko.observable("");

//    self.GetFreeTrail = function () {
//        $.ajax({
//            url: '/Cart/GetFreeTrialPlan',
//            data: {
//                trialcountryfrom: self.Trialfromcountry,
//                trialcountryto: self.Trialtocountry
//            },
//            type: "POST",
//            success: function (returndata) {
//                if (returndata.ok) {
//                     window.location = returndata.newurl;
//                }
//                //window.location.replace('/Cart/Index');
//                
//            },
////            error: function (jqXHR, textStatus, errorThrown) {
////                //alert(errorThrown);
////            }

//        });
//    };

//    self.FetchCountryList = function () {
//    
//        $.getJSON('/Account/GetCountryToList/', null, function (data) {
//            self.CountryListTofooter(data);
//        });
//    };


//    self.GetCountryListFrom = function() {
//        var url = '/Account/GetCountryToCountryFromList';
//        $.getJSON(url, function(data) {
//            self.CountryListFromTryUsFree(data);
//        });
//    };

////    self.flagsStyle = ko.computed(function () {
////            return "flag fnone";
////    });

    self.GetFreeTrialCountryList = function () {
     
        $.getJSON('/Account/GetFreeTrialCountryList/', null, function (data) {
            self.TrialCountries(data);
        });
    };
}

//                                  var jsonData = [];
//                                  var predata = [];
//                                                    $.ajax({
//                                                        url: "/Account/GetCountryToListFlag/",
//                                                        type: 'GET',
//                                                        success: function(data) {
//                                                            predata = data;

//                                                            $.each(data, function() {
//                                                                jsonData.push({ id: this.Id, name: this.CountryFlag, status: 'Already visited' });
//                                                            });

//                                                            var msfooter = $('#ms-footer').magicSuggest({
//                                                                data: jsonData,
//                                                                resultAsString: true,
//                                                                maxSelection: 1,
//                                                                maxSelectionRenderer: function() {
//                                                                }
//                                                            });
//                                                            

//                                                            $(msfooter).on('selectionchange', function(event, selection) {
//                                                                var selected = msfooter.getSelectedItems();
//                                                                var countryto = selected[0].id;
//                                                                var countryfromsearch = $('#countries_searchrates_footer').val();
//                                                                
//                                                                window.location.href = "/Rate/SearchRate?countryfrom=" + countryfromsearch + "&countryto=" + countryto + "";
//                                                            });

//                                                            $('.ms-res-item').click(function() {
//                                                                alert("hii");
//                                                               var selected = msfooter.getSelectedItems();
//                                                                var countryto = selected[0].id;
//                                                                var countryfromsearch = $('#countries_searchrates_footer').val();
//                                                                
//                                                                window.location.href = "/Rate/SearchRate?countryfrom=" + countryfromsearch + "&countryto=" + countryto + "";

//                                                            });

//                                                        }
//                                                    });    