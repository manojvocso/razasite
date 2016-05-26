$(document).ready(function () {

    var topupmobile = new TopupMobile();

    ko.applyBindings(topupmobile, document.getElementById("left_panel"));

    topupmobile.GetCountryList();

});

function TopupMobile() {

    var self = this;
    self.CountryListTopup = ko.observableArray([]);
    self.MobileNumber = ko.observable("");
   // self.SelectedCountry = ko.observable("");

    self.GetCountryList = function() {
        $.ajax(function() {
            $.getJSON('/Account/GetCountryToList/', null, function (data) {
                self.CountryListTopup(data);
            });
        });
    };
}
