$(document).ready(function () {

    var opencomplaintviewmodel = new OpenComplaintViewModel();

    $("#Complaint-btn").click({ handler: opencomplaintviewmodel.Update });
    $("#Reset-btn").click({ handler: opencomplaintviewmodel.Reset });
  
    ko.applyBindings(opencomplaintviewmodel, document.getElementById("OpenComplaint"));
    opencomplaintviewmodel.GetData();
});

function OpenComplaintViewModel() {
    var self = this;

    self.CountryFrom = ko.observableArray([]);
    self.PhoneNumber = ko.observable();
    self.FirstName = ko.observable();
    self.LastName = ko.observable();
    self.PlanName = ko.observable([]);
    self.EmailAddress = ko.observable();
    self.DestinationNumber = ko.observable();
    self.CountryTo = ko.observableArray([]);
    self.Description = ko.observable();
    self.Comment = ko.observableArray([]);
    self.countryTo = ko.observable();
    self.countryFrom = ko.observable("");
    self.order_id = ko.observable();
    self.Response = ko.observable();
  
    
    self.GetData = function() {
   
        $.ajax({
            url: '/Support/GetComplaintData',
            type: 'GET',
            success: function (resp) {

                self.CountryFrom(resp.CountryFromList);
                self.CountryTo(resp.CountryToList);
                self.PlanName(resp.PlansList);
               
                self.FirstName(resp.FirstName);
               
                self.LastName(resp.LastName);
                self.EmailAddress(resp.Email);
                self.PhoneNumber(resp.ContactPhone);
            }
             
        });

    };
   
    self.Reset = function () {
       
        self.PhoneNumber("");
        self.LastName("");
        self.EmailAddress("");
        self.FirstName("");
        self.DestinationNumber("");
        self.Comment("");
        self.countryFrom(null);
        self.countryTo(null);
        self.order_id(null);
        self.Response("");
    };
    self.Update = function () {

        self.Response("");
       
        if (self.PhoneNumber().length < 10 || self.FirstName().length == 0 ||
            self.order_id() == undefined || self.LastName().length == 0 || self.EmailAddress().length == 0
            | self.DestinationNumber() ==undefined || self.countryFrom().length == undefined || self.countryTo().length == undefined
            || self.Description().length == 0 || self.Comment().length == 0) {
           
            self.Response("Please enter required fields.");
            return false;
        }
        $.ajax({
            url: '/support/ComplaintSubmit',
            data: {
                ContactPhone: self.PhoneNumber,
                Destination_Number: self.DestinationNumber,
                OrderId: self.order_id,
                CountryFrom: self.countryFrom,
                CountryTo: self.countryTo,
                Description: self.Description,
                Notes: self.Comment
            },
            type: 'POST',
            success: function (result) {
                self.DestinationNumber("");
                self.Comment("");
                self.countryFrom(null);
                self.countryTo(null);
                if (result.status == true) {
                   
                 
                    self.Response("We have received your complaint , You will receive our response within 24 hrs. <br/> Complaint No. is: " + result.message);
                } else {
                    self.Response(result.message);
                }
              },

            error: function (jqXHR, textStatus, errorThrown) {

                //                 self.ErrorMsg(JSON.parse(jqXHR.responseText).Message);

            }
        });

    };

   
}

function numberOnly() {
    // 48 = 0
    // 57 = 9
    if ((event.keyCode < 48) || (event.keyCode > 57)) {
        return false;
    }
    return true;
}



