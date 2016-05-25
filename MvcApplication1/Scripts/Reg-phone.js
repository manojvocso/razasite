function loading() {
    $("div.loader").show();
}
function loadPopup() {
    if (popupStatus == 0) { // if value is 0, show popup
        closeloading(); // fadeout loading
        $("#errorValidation").fadeIn(0500); // fadein popup div
        $("#backgroundPopup").css("opacity", "0.7"); // css opacity, supports IE7, IE8
        $("#backgroundPopup").fadeIn(0001);
        popupStatus = 1; // and set value to 1
    }
}


$(this).keyup(function (event) {
    if (event.which == 27) { // 27 is 'Ecs' in the keyboard
        disablePopup(); // function close pop up
    }
});

function closeloading() {
    $("div.loader").fadeOut('normal');
}


var popupStatus = 0; // set value
function disablePopup() {
    if (popupStatus == 1) { // if value is 1, close popup
        $("#errorValidation").fadeOut("normal");
        $("#backgroundPopup").fadeOut("normal");
        popupStatus = 0; // and set value to 0
    }
}
// end for function for show popup div.


var model = new Pinlessmodel();
$(document).ready(function () {
    $("#close-val").click({ handler: model.Hidepopup });
    $("#backgroundPopup").click({ handler: model.Hidepopup });
    $("#close-val2").click({ handler: model.NoDelete });
    $("#YesDelete").click({ handler: model.DeletePinlessNumber });
    $("#NoDelete").click({ handler: model.NoDelete });
    $("#submit-regnumbers").click({ handler: model.RegisterPhoneNumbers });
    
    ko.applyBindings(model, document.getElementById("inner_body_container"));

    model.GetNumbers();
    

});

function Pinlessmodel() {

    var self = this;

    self.PinlessNumberList = ko.observableArray([]);
    self.SelectedNumbersList = ko.observableArray([]);

    self.SelectedListTemp = ko.observableArray([]);
    self.MainPinlessList = ko.observableArray([]);

    self.AniNumber = ko.observable("");


    self.PinlessNumber1_1 = ko.observable("");
    self.PinlessNumber1_2 = ko.observable("");
    self.PinlessNumber1_3 = ko.observable("");
    self.CountryCode1 = ko.observable("");

    self.PinlessNumber2_1 = ko.observable("");
    self.PinlessNumber2_2 = ko.observable("");
    self.PinlessNumber2_3 = ko.observable("");
    self.CountryCode2 = ko.observable("");

    self.PinlessNumber3_1 = ko.observable("");
    self.PinlessNumber3_2 = ko.observable("");
    self.PinlessNumber3_3 = ko.observable("");
    self.CountryCode3 = ko.observable("");

    self.PinlessNumber4_1 = ko.observable("");
    self.PinlessNumber4_2 = ko.observable("");
    self.PinlessNumber4_3 = ko.observable("");
    self.CountryCode4 = ko.observable("");

    self.PinlessNumber5_1 = ko.observable("");
    self.PinlessNumber5_2 = ko.observable("");
    self.PinlessNumber5_3 = ko.observable("");
    self.CountryCode5 = ko.observable("");

    self.validationmessages = ko.observableArray([]);

    self.AniNumber1 = ko.computed(function() {
        return self.AniNumber().substring(0, 3);
    }, this);

    self.AniNumber2 = ko.computed(function () {
        return self.AniNumber().substring(3, 6);
    }, this);

    self.AniNumber3 = ko.computed(function () {
        return self.AniNumber().substring(6);
    }, this);


    self.GetNumbers = function () {
        $.ajax({
            url: '/Recharge/GetAllPinlessNumber',
            type: "GET",
            cache: false,
            success: function (data) {
                self.PinlessNumberList(data.PinlessNumberList);
                self.AniNumber(data.AniNumber);
                
                //self.OrderId = data.OrderID;
            }

        });
    };


    // ko.utils.addOrRemoveItem isn't included until version 2.3.0
    if (!ko.utils.addOrRemoveItem) {
        ko.utils.addOrRemoveItem = function (array, value, included) {
            
            var existingEntryIndex = array.indexOf ? array.indexOf(value) : ko.utils.arrayIndexOf(array, value);
            if (existingEntryIndex < 0) {
                if (included)
                    array.push(value);
                self.SetTextboxes();

            } else {
                if (!included){}
                    array.splice(existingEntryIndex, 1);
                self.SetTextboxes();
            }
        };
    }

    ko.bindingHandlers.checkedInArray = {
        init: function (element, valueAccessor) {
            ko.utils.registerEventHandler(element, "click", function () {
                if (self.SelectedNumbersList().length == 5 && element.checked == true) {
                    element.checked = false;

                    return false;
                }
                var options = ko.utils.unwrapObservable(valueAccessor()),
                    array = options.array, // don't unwrap array because we want to update the observable array itself
                    value = ko.utils.unwrapObservable(options.value),
                    checked = element.checked;
                
                ko.utils.addOrRemoveItem(array, value, checked);
            });
        },
        update: function (element, valueAccessor) {
            var options = ko.utils.unwrapObservable(valueAccessor()),
                array = ko.utils.unwrapObservable(options.array),
                value = ko.utils.unwrapObservable(options.value);
            
            element.checked = ko.utils.arrayIndexOf(array, value) >= 0;
        }
    };

    self.Hidepopup = function() {
        disablePopup();
    };

    self.Delete = function (data) {
        self.ForDelPinnumber(data.PinlessNumber);
        self.ForDelcountry(data.CountryCode);
        self.ForDelorderid(data.OrderId);
        loadPopup1();
    };
    self.NoDelete = function () {

        disablePopup1();
    };

    self.ForDelPinnumber = ko.observable("");
    self.ForDelcountry = ko.observable("");
    self.ForDelorderid = ko.observable("");

    self.DeletePinlessNumber = function () {
        $.ajax({
            url: '/Account/DeletePinLessSetup',
            data: {
                pn: self.ForDelPinnumber,
                cd: self.ForDelcountry,
                oid: self.ForDelorderid
            },
            type: "POST",
            cache: false,
            success: function (resp) {

                if (resp.status == true) {
                    self.GetNumbers();
                } else {
                    self.GetNumbers();
                }
                self.NoDelete();
            }

        });
    };

    self.RegisterPhoneNumbers = function () {
        //PostRegphone();
        self.validationmessages([]);
        
        //if ((self.PinlessNumberOne().length == 0 || self.PinlessNumberOne().length < 10) || (self.PinlessNumberTwo().length == 0 || self.PinlessNumberTwo().length < 10)
        //    (self.PinlessNumberThree().length == 0 || self.PinlessNumberThree().length < 10) || (self.PinlessNumberFour().length == 0 || self.PinlessNumberFour().length < 10)
        //    || (self.PinlessNumberFive().length == 0 || self.PinlessNumberFive().length < 10))
        if (self.PinlessNumberOne().length != 10 || self.PinlessNumberTwo().length != 10 || self.PinlessNumberThree().length != 10 ||
            self.PinlessNumberFour().length != 10 || self.PinlessNumberFive().length != 10)
        {
            if (self.PinlessNumberOne().length == 0 && self.PinlessNumberTwo().length == 0 && self.PinlessNumberThree().length == 0 &&
                self.PinlessNumberFour().length == 0 && self.PinlessNumberFive().length == 0) {
                model.validationmessages.push("There should be a Phone number assigned under option 2 to purchase a new plan or transfer your existing number from option 1.");
                loadPopup();
                return false;

            } else if ((self.PinlessNumberOne().length > 0 && self.PinlessNumberOne().length < 10) || (self.PinlessNumberTwo().length > 0 && self.PinlessNumberTwo().length < 10) ||
                (self.PinlessNumberThree().length > 0 && self.PinlessNumberThree().length < 10) || (self.PinlessNumberFour().length > 0 && self.PinlessNumberFour().length < 10) ||
                (self.PinlessNumberFive().length > 0 && self.PinlessNumberFive().length < 10)
                )
            {
                model.validationmessages.push("Please enter a valid Phone number.");
                loadPopup();
                return false;
            }
            
        }

        $.ajax({
            url: '/Recharge/Regphone',
            data: {
                PinlessNumberOne:self.PinlessNumberOne,
                PinlessNumberTwo: self.PinlessNumberTwo,
                PinlessNumberThree: self.PinlessNumberThree,
                PinlessNumberFour: self.PinlessNumberFour,
                PinlessNumberFive: self.PinlessNumberFive,

            },
            type: "POST",
            cache: false,
            success: function (resp) {
                if (resp.status == true) {
                    window.location.href = "/Recharge/PurchasePlan";
                } else {
                    self.validationmessages.push(resp.statuserror);
                    loadPopup();
                }
                
            }

        });
    };



    var i = 1;
    self.SetTextboxes = function () {
        i = 1;
        self.SelectedListTemp([]);
        self.SelectedListTemp(self.SelectedNumbersList());
        if (self.SelectedNumbersList().length == 0) {
            self.PinlessNumber1_1("");
            self.PinlessNumber1_2("");
            self.PinlessNumber1_3("");
            self.CountryCode1("");
        }
        ko.utils.arrayForEach(self.SelectedListTemp(), function (item) {
           if (self.SelectedNumbersList().length == 0) {
               self.PinlessNumber1_1("");
               self.PinlessNumber1_2("");
               self.PinlessNumber1_3("");
               self.CountryCode1("");
           }
           else if (self.SelectedNumbersList().length == 1) {
               if (i == 1) {
                   var num1 = item.UnmaskPinlessNumber.substring(0, 3);
                   var num2 = item.UnmaskPinlessNumber.substring(3, 6);
                   var num3 = item.UnmaskPinlessNumber.substring(6);

                   self.PinlessNumber1_1(num1);
                   self.PinlessNumber1_2(num2);
                   self.PinlessNumber1_3(num3);
                   self.CountryCode1(item.CountryCode);

                   i = 2;
                   self.PinlessNumber2_1("");
                   self.PinlessNumber2_2("");
                   self.PinlessNumber2_3("");
                   self.CountryCode2("");

                   self.PinlessNumber3_1("");
                   self.PinlessNumber3_2("");
                   self.PinlessNumber3_3("");
                   self.CountryCode3("");

                   self.PinlessNumber4_1("");
                   self.PinlessNumber4_2("");
                   self.PinlessNumber4_3("");
                   self.CountryCode4("");

               }
              
           }
           else if (self.SelectedNumbersList().length == 2) {
              if (i == 1) {
                  num1 = item.UnmaskPinlessNumber.substring(0, 3);
                  num2 = item.UnmaskPinlessNumber.substring(3, 6);
                  num3 = item.UnmaskPinlessNumber.substring(6);

                  self.PinlessNumber1_1(num1);
                  self.PinlessNumber1_2(num2);
                  self.PinlessNumber1_3(num3);
                  self.CountryCode1(item.CountryCode);
                  i = 2;
              }
             else if (i == 2) {
                 num1 = item.UnmaskPinlessNumber.substring(0, 3);
                 num2 = item.UnmaskPinlessNumber.substring(3, 6);
                 num3 = item.UnmaskPinlessNumber.substring(6);

                  self.PinlessNumber2_1(num1);
                  self.PinlessNumber2_2(num2);
                  self.PinlessNumber2_3(num3);
                  self.CountryCode2(item.CountryCode);
                  i = 3;

                  self.PinlessNumber3_1("");
                  self.PinlessNumber3_2("");
                  self.PinlessNumber3_3("");
                  self.CountryCode3("");

                  self.PinlessNumber4_1("");
                  self.PinlessNumber4_2("");
                  self.PinlessNumber4_3("");
                  self.CountryCode4("");

             }
               
           }
           else if (self.SelectedNumbersList().length == 3) {
              if (i == 1) {
                  num1 = item.UnmaskPinlessNumber.substring(0, 3);
                  num2 = item.UnmaskPinlessNumber.substring(3, 6);
                  num3 = item.UnmaskPinlessNumber.substring(6);

                  self.PinlessNumber1_1(num1);
                  self.PinlessNumber1_2(num2);
                  self.PinlessNumber1_3(num3);
                  self.CountryCode1(item.CountryCode);
                  i = 2;
              }
              else if (i == 2) {
                  num1 = item.UnmaskPinlessNumber.substring(0, 3);
                  num2 = item.UnmaskPinlessNumber.substring(3, 6);
                  num3 = item.UnmaskPinlessNumber.substring(6);

                  self.PinlessNumber2_1(num1);
                  self.PinlessNumber2_2(num2);
                  self.PinlessNumber2_3(num3);
                  self.CountryCode2(item.CountryCode);
                  i = 3;
              }
              else if (i == 3) {
                  num1 = item.UnmaskPinlessNumber.substring(0, 3);
                  num2 = item.UnmaskPinlessNumber.substring(3, 6);
                  num3 = item.UnmaskPinlessNumber.substring(6);

                  self.PinlessNumber3_1(num1);
                  self.PinlessNumber3_2(num2);
                  self.PinlessNumber3_3(num3);
                  self.CountryCode2(item.CountryCode);
                  i = 4;

                  self.PinlessNumber4_1("");
                  self.PinlessNumber4_2("");
                  self.PinlessNumber4_3("");
                  self.CountryCode4("");
              }
              
          }
           else if (self.SelectedNumbersList().length == 4) {
              if (i == 1) {
                  num1 = item.UnmaskPinlessNumber.substring(0, 3);
                  num2 = item.UnmaskPinlessNumber.substring(3, 6);
                  num3 = item.UnmaskPinlessNumber.substring(6);

                  self.PinlessNumber1_1(num1);
                  self.PinlessNumber1_2(num2);
                  self.PinlessNumber1_3(num3);
                  self.CountryCode1(item.CountryCode);
                  i = 2;
              }
              else if (i == 2) {
                  num1 = item.UnmaskPinlessNumber.substring(0, 3);
                  num2 = item.UnmaskPinlessNumber.substring(3, 6);
                  num3 = item.UnmaskPinlessNumber.substring(6);

                  self.PinlessNumber2_1(num1);
                  self.PinlessNumber2_2(num2);
                  self.PinlessNumber2_3(num3);
                  self.CountryCode2(item.CountryCode);
                  i = 3;
              }
              else if (i == 3) {
                  num1 = item.UnmaskPinlessNumber.substring(0, 3);
                  num2 = item.UnmaskPinlessNumber.substring(3, 6);
                  num3 = item.UnmaskPinlessNumber.substring(6);

                  self.PinlessNumber3_1(num1);
                  self.PinlessNumber3_2(num2);
                  self.PinlessNumber3_3(num3);
                  self.CountryCode3(item.CountryCode);
                  i = 4;
              }
              else if (i == 4) {
                  num1 = item.UnmaskPinlessNumber.substring(0, 3);
                  num2 = item.UnmaskPinlessNumber.substring(3, 6);
                  num3 = item.UnmaskPinlessNumber.substring(6);

                  self.PinlessNumber4_1(num1);
                  self.PinlessNumber4_2(num2);
                  self.PinlessNumber4_3(num3);
                  self.CountryCode4(item.CountryCode);
              }

              
           }


           else if (self.SelectedNumbersList().length == 5) {
               if (i == 1) {
                   num1 = item.UnmaskPinlessNumber.substring(0, 3);
                   num2 = item.UnmaskPinlessNumber.substring(3, 6);
                   num3 = item.UnmaskPinlessNumber.substring(6);

                   self.PinlessNumber1_1(num1);
                   self.PinlessNumber1_2(num2);
                   self.PinlessNumber1_3(num3);
                   self.CountryCode1(item.CountryCode);
                   i = 2;
               }
               else if (i == 2) {
                   num1 = item.UnmaskPinlessNumber.substring(0, 3);
                   num2 = item.UnmaskPinlessNumber.substring(3, 6);
                   num3 = item.UnmaskPinlessNumber.substring(6);

                   self.PinlessNumber2_1(num1);
                   self.PinlessNumber2_2(num2);
                   self.PinlessNumber2_3(num3);
                   self.CountryCode2(item.CountryCode);
                   i = 3;
               }
               else if (i == 3) {
                   num1 = item.UnmaskPinlessNumber.substring(0, 3);
                   num2 = item.UnmaskPinlessNumber.substring(3, 6);
                   num3 = item.UnmaskPinlessNumber.substring(6);

                   self.PinlessNumber3_1(num1);
                   self.PinlessNumber3_2(num2);
                   self.PinlessNumber3_3(num3);
                   self.CountryCode3(item.CountryCode);
                   i = 4;
               }
               else if (i == 4) {
                   num1 = item.UnmaskPinlessNumber.substring(0, 3);
                   num2 = item.UnmaskPinlessNumber.substring(3, 6);
                   num3 = item.UnmaskPinlessNumber.substring(6);

                   self.PinlessNumber4_1(num1);
                   self.PinlessNumber4_2(num2);
                   self.PinlessNumber4_3(num3);
                   self.CountryCode4(item.CountryCode);
                   i = 5;
               }
               else if (i == 5) {
                   num1 = item.UnmaskPinlessNumber.substring(0, 3);
                   num2 = item.UnmaskPinlessNumber.substring(3, 6);
                   num3 = item.UnmaskPinlessNumber.substring(6);

                   self.PinlessNumber5_1(num1);
                   self.PinlessNumber5_2(num2);
                   self.PinlessNumber5_3(num3);
                   self.CountryCode5(item.CountryCode);
               }

           }

           
        });
    };

    self.PinlessNumberOne = ko.computed(function () {
        return self.PinlessNumber1_1() + self.PinlessNumber1_2() + self.PinlessNumber1_3();
    }, this);

    self.PinlessNumberTwo = ko.computed(function () {
        return self.PinlessNumber2_1() + self.PinlessNumber2_2() + self.PinlessNumber2_3();
    }, this);

    self.PinlessNumberThree = ko.computed(function () {
        return self.PinlessNumber3_1() + self.PinlessNumber3_2() + self.PinlessNumber3_3();
    }, this);

    self.PinlessNumberFour = ko.computed(function () {
        return self.PinlessNumber4_1() + self.PinlessNumber4_2() + self.PinlessNumber4_3();
    }, this);
    self.PinlessNumberFive = ko.computed(function () {
        return self.PinlessNumber5_1() + self.PinlessNumber5_2() + self.PinlessNumber5_3();
    }, this);

    self.GetCountryListFrom = function () {
        var url = '/Account/GetCountryToCountryFromList';
        $.getJSON(url, function (data) {
            self.CountryListFrom(data);
        });
    };

    self.SubmitNumbers = function () {
      
        //if (self.PinlessNumberOne().length < 10 || self.PinlessNumberTwo().length < 10 || self.PinlessNumberThree().length < 10 || self.PinlessNumberFour().length < 10) {
        //    if (self.PinlessNumberOne().length == 0 && self.PinlessNumberTwo().length == 0 && self.PinlessNumberThree().length == 0 && self.PinlessNumberFour().length == 0) {
        //        self.validationmessages.push("IMPORTANT!!! In order to use Raza Calling Plans, Each Raza Calling Plan must have a number attached to it so that dialing your PIN is not needed. You can either assign a new number or re-assign an existing number to the new plan.");
        //        loadPopup();
        //        return false;
        //    }
        //    else if ((self.PinlessNumberOne().length > 0 && self.PinlessNumberOne().length < 10) || (self.PinlessNumberTwo().length > 0 && self.PinlessNumberTwo().length < 10) ||
        //        (self.PinlessNumberThree().length > 0 && self.PinlessNumberThree().length < 10) || (self.PinlessNumberFour().length > 0 && self.PinlessNumberFour().length < 10)) {
        //        self.validationmessages.push("Please enter a ten digit valid valid number.");
        //        loadPopup();
        //        return false;
        //    }
        //}
        
        if (self.AniNumber().length < 10) {
            self.validationmessages([]);
                self.validationmessages.push("IMPORTANT!!! In order to use Raza Calling Plans, Each Raza Calling Plan must have a number attached to it so that dialing your PIN is not needed. You can either assign a new number or re-assign an existing number to the new plan.");
                loadPopup();
                return false;
            }
            else if ((self.PinlessNumberOne().length > 0 && self.PinlessNumberOne().length < 10) || (self.PinlessNumberTwo().length > 0 && self.PinlessNumberTwo().length < 10) ||
                (self.PinlessNumberThree().length > 0 && self.PinlessNumberThree().length < 10) || (self.PinlessNumberFour().length > 0 && self.PinlessNumberFour().length < 10)) {
                self.validationmessages([]);
                self.validationmessages.push("Please enter a valid 10 digit number.");
                loadPopup();
                return false;
            }
    };

};

function Dat(dt) {
      
    var limit = 4,
        //_check = elems.filter(':checked').length;
        _check = model.SelectedNumbersList().length;
    
      if (model.PinlessNumberOne().length > 0 && model.PinlessNumberTwo().length > 0 && model.PinlessNumberThree().length > 0
        && model.PinlessNumberFour().length > 0 && model.PinlessNumberFive().length > 0 && dt.checked == true) {
          model.validationmessages([]);
          model.validationmessages.push("A Maximum of 5 numbers can be registered only.");
          loadPopup();
        dt.checked = false;
        return false;
    } else {
          return true;
      }
}

function PostRegphone() {
    model.validationmessages([]);
    if ((model.PinlessNumberOne().length == 0 || model.PinlessNumberOne().length < 10) || (model.PinlessNumberTwo().length == 0 || model.PinlessNumberTwo().length < 10)
        (model.PinlessNumberThree().length == 0 || model.PinlessNumberThree().length < 10) || (model.PinlessNumberFour().length == 0 || model.PinlessNumberFour().length < 10) ||
        (model.PinlessNumberFive().length == 0 || model.PinlessNumberFive().length < 10)
    ) {
        model.validationmessages.push("There should be a Phone number assigned under option 2 to purchase a new plan or transfer your existing number from option 1.");
        loadPopup();
        return false;
    }
}

function loadPopup1() {
    if (popupStatus == 0) { // if value is 0, show popup
        closeloading(); // fadeout loading
        $("#toPopupforward").fadeIn(0500); // fadein popup div
        $("#backgroundValidation").css("opacity", "0.7"); // css opacity, supports IE7, IE8
        $("#backgroundValidation").fadeIn(0001);
        popupStatus = 1; // and set value to 1
    }
}

$(this).keyup(function (event) {
    if (event.which == 27) { // 27 is 'Ecs' in the keyboard
        disablePopup1(); // function close pop up
    }
});

function closeloading() {
    $("div.loader").fadeOut('normal');
}

var popupStatus = 0; // set value

function disablePopup1() {
    if (popupStatus == 1) { // if value is 1, close popup
        $("#toPopupforward").fadeOut("normal");
        $("#backgroundValidation").fadeOut("normal");
        popupStatus = 0; // and set value to 0
    }
}



