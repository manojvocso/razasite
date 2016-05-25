$(document).ready(function () {

    var masterViewModel = new MasterViewModel();

    ko.applyBindings(masterViewModel, document.getElementById("Headersearch"));

    // Always fill From list
    masterViewModel.GetCountryListFrom();
    masterViewModel.FetchCountryListTo();
    masterViewModel.GetCountryListTo();
});

function MasterViewModel() {
    var self = this;

 
    self.CountryListFrom = ko.observableArray([]);
    self.CountryListTo = ko.observableArray([]);
    self.CountryToList = ko.observableArray([]);
 


    self.GetCountryListFrom = function () {
        var url = '/Account/GetCountryToCountryFromList';
        $.getJSON(url, function (data) {
            self.CountryListFrom(data);
        });
    };


    self.GetCountryListTo = function () {
        var url = '/Account/GetCountryToList/';
        $.getJSON(url, function (data) {
            self.CountryToList(data);
          
        });
    };
  

    self.FetchCountryListTo = function () {
        $.getJSON('/Account/GetCountryToList/', null, function (data) {
            self.CountryListTo(data);
      
        });
    };




}

$('#ddlCountryListFr').change(function () {

    if ($(this).find('option:selected').text() == 'Choose...') {

        $('#Email').attr('disabled', 'disabled');
    } else {

        $('#Email').removeAttr('disabled', '');

    }
});



$('#combobox').change(function () {

    if ($(this).find('option:selected').text() == 'Choose...') {

        $('#TypePassword').attr('disabled', 'disabled');
    } else {

        $('#TypePassword').removeAttr('disabled', '');

    }
});





