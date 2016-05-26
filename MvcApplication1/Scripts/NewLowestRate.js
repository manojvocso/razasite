var newlowestRateModel = new NewLowestRateModel();

$(document).ready(function () {

    ko.applyBindings(newlowestRateModel, document.getElementById("NewcustRate"));

});
var jsonData12 = [];

var predata12 = [];
$.ajax({
    url: "/Account/GetCountryToListFlag/",
    type: 'GET',
    success: function (data) {
        predata12 = data;

        $.each(data, function () {
            jsonData12.push({ id: this.Id, name: this.CountryFlag, status: 'Already visited' });
        });

        var newlowestrate = $('#ms-Newcustlowestrate').magicSuggest({
            data: jsonData12,
            resultAsString: true,
            maxSelection: 1,
            maxSelectionRenderer: function () {
            }
        });

    
        $(newlowestrate).on('selectionchange', function (event, selection) {
            var selected = newlowestrate.getSelectedItems();
            var countryto = selected[0].id;
            var countryfrom = $("#stamp_flag1").val();
            newlowestRateModel.NewLowestRate(countryfrom, countryto);
        });

    }
});

function NewLowestRateModel() {
    var self = this;
    self.CallingFrom = ko.observable("");
    self.CallingTo = ko.observable("");
    self.Landline = ko.observable("");
    self.Mobile= ko.observable("");
  

    self.BuySearchPlan = function () {

        window.location.href = "/Rate/searchrate?countryfrom=" + self.CallingFrom() + "&countryto=" + self.CallingTo() + "";
    };
    self.NewLowestRate = function (countryfrom, countryto) {
      
        self.CallingFrom(countryfrom);
        self.CallingTo(countryto);

        $.ajax({
            url: '/Account/NewCustomerLowestRate/',
            data: {
                CallingFrom: countryfrom,
                CallingTo: countryto
            },
            type: "GET",
            success: function (data) {
              
                self.Landline(data.LandLineRate);
                self.Mobile(data.MobileRate);
            
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //alert(errorThrown);
            }

        });

    };

 
}
