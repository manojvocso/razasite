var model = new LocalAccessViewModel();
function FetchStates() {
   
    var countryCode = $("#dlLocalAcessCountry").val();
    var countrytext = $("#dlLocalAcessCountry option:selected").text();
    
    /*if (countrytext == "Afghanistan") {
        countryCode = 4;
    }
    if (countrytext == "Alaska") {
        countryCode = 5;
    }
    if (countrytext == "Albania") {
        countryCode = 6;
    }*/
  
    $.getJSON('/Support/GetStates/' + countryCode, null, function (data) {
       
        model.states(data);

    });
}
$(document).ready(function () {

    $("#defaultAccessNumbers").css("display", 'none');
    $("#AccessNumbersList").css("display", 'none');
    $("#StateLessAccessNumbersList").css("display", "none");
  

//    $("tr:even").css("background-color", "#f9f9f9");
//    $("tr:odd").css("background-color", "#ffffff");
    
    ko.applyBindings(model, document.getElementById("divlocalaccessnumber"));
});

function LocalAccessViewModel() {
    var self = this;
    self.AccessNumbers = ko.observableArray([]);
    self.CountryId = ko.observable("");
    self.StatusMessage = ko.observable("");

   
    self.states = ko.observableArray([]);
    self.Error = ko.observable();

    //self.gridViewModel = new ko.simpleGrid.viewModel({
    //   // data: self.AccessNumbers,
    //    columns: [
    //        { headerText: "SNo.", rowText: "SNo" },
    //        { headerText: "State", rowText: "State" },
    //        { headerText: "PinLess Local Access No.", rowText: "PinLessLocalNumber" },
    //        { headerText: "Local Access No. Using Pin", rowText: "PinLocalNumber" },
    //        { headerText: "City", rowText: "City" }
    //    ],
    //    pageSize: 4
    //});


   
};




          