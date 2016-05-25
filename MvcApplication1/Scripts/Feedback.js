$(document).ready(function () {

    var feedbackviewmodel = new FeedBackViewModel();

   
    $("#feedBack-btn").click({ handler: feedbackviewmodel.CustFeedBack });

    ko.applyBindings(feedbackviewmodel, document.getElementById("FeedBack"));
   
});

function FeedBackViewModel() {
    var self = this;
    self.firstname = ko.observable();
    self.lastname = ko.observable();
    self.phonenumber = ko.observable();
    self.email = ko.observable();
    self.feedback = ko.observable();
    self.feed = ko.observable();
    self.Israzacust = ko.observable();
    self.validationmessage = ko.observableArray([]);
    self.result = ko.observable();
    
    self.CustFeedBack = function () {
        self.validationmessage([]);
        self.result("");
        self.Israzacust($("#israzacustomer").val());
        var res = $("#email").val();
        var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
        self.validationmessage([]);
        if (reg.test(res) == false) {
            self.validationmessage.push("Invalid Email Address");
            return false;
        }
        if (self.firstname() == undefined || self.lastname() == undefined || self.phonenumber() == undefined ||  self.feedback() == undefined || self.feed() == undefined) {
            self.validationmessage([]);
            self.validationmessage.push("Please insert complete information");
            return false;
        } else if (self.phonenumber().length < 10) {
            self.validationmessage.push("Please put correct phone number");
            return false;
        }


        $.ajax({
            url: "/Support/CustomerFeedback/",
            type: 'POST',
            data: {
                IsRazaCustomer: self.Israzacust,
                FirstName: self.firstname,
                LastName: self.lastname,
                EmailAddress: res,
                FeedbackType: self.feedback,
                Feedback: self.feed,
                PhoneNumber: self.phonenumber
            },
            success: function(response) {
                self.result(response.message);
                self.validationmessage([]);
                self.firstname("");
                self.lastname("");
                self.email("");
                self.feed("");
                $("#email").val("");
                $('.com_checkbox').attr('checked', false);
                $('.com_checkbox_1').attr('checked', false);
                self.phonenumber("");

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