   
var jsonData1 = [];
var predata1 = [];

$.ajax({
    url: "/Account/GetCountryToListFlag/",
    type: 'GET',
    success: function (data) {
        predata1 = data;

        $.each(data, function () {
            jsonData1.push({ id: this.Id, cname: this.CountryFlag, name: this.Name, Code: this.CountCode, status: 'Already visited' });
        });

        //asia tab
        var msasia = $('#ms-asia').magicSuggest({
            displayField: 'cname',
            allowFreeEntries: false,
            strictSuggest: true,
            data: jsonData1,
            resultAsString: true,
            maxSelection: 1,
            

            renderer: function (data) {
                return '<div>' +
                     data.cname + '(+' + data.Code + ')' +
                    '</div><div style="clear:both;"></div>'
            },
            maxSelectionRenderer: function () {
            }
        });

        $(msasia).on('selectionchange', function (event, selection) {
            var selected = msasia.getSelectedItems();
            var countryto = selected[0].id;
            var countryfrom = $("#from-asia").val();                         

          window.location.href = "/Rate/SearchRate?countryfrom=" + countryfrom + "&countryto=" + countryto + "";
        });

        // africa tab search
        var msafrica = $('#ms-Africa ').magicSuggest({
            displayField: 'cname',
            allowFreeEntries: false,
            strictSuggest: true,
            data: jsonData1,
            resultAsString: true,
            maxSelection: 1,

            renderer: function (data) {
                return '<div>' +
                    data.cname + '(+' + data.Code + ')' +
                    '</div><div style="clear:both;"></div>';
            },
            maxSelectionRenderer: function () {
            }
        });

        $(msafrica).on('selectionchange', function (event, selection) {
            var selected = msafrica.getSelectedItems();
            var countryto = selected[0].id;
            var countryfrom = $("#from-africa").val();                               // var countrycode = selected[0].Code;

            window.location.href = "/Rate/SearchRate?countryfrom=" + countryfrom + "&countryto=" + countryto + "";
        });

        //Europe tab search
        var msEurope = $('#ms-Europe ').magicSuggest({
            displayField: 'cname',
            allowFreeEntries: false,
            strictSuggest: true,
            data: jsonData1,
            resultAsString: true,
            maxSelection: 1,

            renderer: function (data) {
                return '<div>' +
                    data.cname + '(+' + data.Code + ')' +
                    '</div><div style="clear:both;"></div>';
            },
            maxSelectionRenderer: function () {
            }
        });

        $(msEurope).on('selectionchange', function (event, selection) {
            var selected = msEurope.getSelectedItems();
            var countryto = selected[0].id;
            var countryfrom = $("#from-Europe").val();                               // var countrycode = selected[0].Code;

            window.location.href = "/Rate/SearchRate?countryfrom=" + countryfrom + "&countryto=" + countryto + "";
        });

        // middle map search
        var msMiddle = $('#ms-middle ').magicSuggest({
            displayField: 'cname',
            allowFreeEntries: false,
            strictSuggest: true,
            data: jsonData1,
            resultAsString: true,
            maxSelection: 1,

            renderer: function (data) {
                return '<div>' +
                     data.cname + '(+' + data.Code + ')' +
                    '</div><div style="clear:both;"></div>'
            },
            maxSelectionRenderer: function () {
            }
        });

        $(msMiddle).on('selectionchange', function (event, selection) {
            var selected = msMiddle.getSelectedItems();
            var countryto = selected[0].id;
            var countryfrom = $("#from-middleeast").val();                               // var countrycode = selected[0].Code;

            window.location.href = "/Rate/SearchRate?countryfrom=" + countryfrom + "&countryto=" + countryto + "";
        });

        //SouthAmerica map seacrh
        var msSouth = $('#mssouth ').magicSuggest({
            displayField: 'cname',
            allowFreeEntries: false,
            strictSuggest: true,
            data: jsonData1,
            resultAsString: true,
            maxSelection: 1,

            renderer: function (data) {
                return '<div>' +
                     data.cname + '(+' + data.Code + ')' +
                    '</div><div style="clear:both;"></div>'
            },
            maxSelectionRenderer: function () {
            }
        });

        $(msSouth).on('selectionchange', function (event, selection) {
            var selected = msSouth.getSelectedItems();
            var countryto = selected[0].id;
            var countryfrom = $("#from-southamerica").val();                               // var countrycode = selected[0].Code;

            window.location.href = "/Rate/SearchRate?countryfrom=" + countryfrom + "&countryto=" + countryto + "";
        });

        //North America 
        var msNorth = $('#ms-North ').magicSuggest({
            displayField: 'cname',
            allowFreeEntries: false,
            strictSuggest: true,
            data: jsonData1,
            resultAsString: true,
            maxSelection: 1,

            renderer: function (data) {
                return '<div>' +
                    data.cname + '(+' + data.Code + ')' +
                    '</div><div style="clear:both;"></div>';
            },
            maxSelectionRenderer: function () {
            }
        });

        $(msNorth).on('selectionchange', function (event, selection) {
            var selected = msNorth.getSelectedItems();
            var countryto = selected[0].id;
            var countryfrom = $("#from-North").val();                               // var countrycode = selected[0].Code;

            window.location.href = "/Rate/SearchRate?countryfrom=" + countryfrom + "&countryto=" + countryto + "";
        });

    }
});


//var cid = $("#country-byip").val();
//function setSelectedIndex(s, valsearch) {
//    // Loop through all the items in drop down list
//    for (var i = 0; i < s.options.length; i++) {
//        if (s.options[i].value == valsearch) {
//            // Item is found. Set its property and exit
//            s.options[i].selected = true;
//            break;
//        }
//    }
//    return;
//}

//setSelectedIndex(document.getElementById("from-asia"), cid);
//setSelectedIndex(document.getElementById("from-africa"), cid);
//setSelectedIndex(document.getElementById("from-Europe"), cid);
//setSelectedIndex(document.getElementById("from-middleeast"), cid);
//setSelectedIndex(document.getElementById("from-southamerica"), cid);
//setSelectedIndex(document.getElementById("from-North"), cid);
