/*
 * requires Prototype
 * assumed object
 * JS to support form function
 */

if (Prototype) {

    countries_list = ["Afghanistan", "Albania", "Algeria", "American Samoa", "Andorra", "Angola", "Anguilla", "Antarctica", "Antarctica - Casey", "Antarctica - Christmas Island", "Antarctica - Cocos Island", "Antarctica - Davis", "Antarctica - Maquarie Station", "Antarctica - Norfolk Island", "Antarctica - Scott", "Antigua & Barbuda", "Argentina", "Armenia", "Aruba", "Ascension Island", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia - Herzegovina", "Botswana", "Brazil", "British Virgin Islands", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Central African Republic", "Chad", "Chile", "China", "Colombia", "Comoros", "Congo", "Cook Islands", "Costa Rica", "Croatia", "Cuba", "Cyprus", "Czech Republic", "Democratic Republic of the Congo (Zaire)", "Denmark", "Diego Garcia", "Djibouti", "Dominica", "Dominican Republic", "East Timor (Timor-Leste)", "Ecuador", "Egypt", "El Salvador", "Ellipso Satellite System", "EMSAT", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France", "French Guiana", "French Polynesia", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Global MobilSatServ", "Globalstar - Avrasya", "Greece", "Greenland", "Grenada", "Guadeloupe", "Guam", "Guatemala", "Guinea", "Guinea - Bissau", "Guyana", "Haiti", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Inmarsat (AOR)", "Inmarsat (IOR)", "Inmarsat (POR)", "Inmarsat (SNAC)", "Inmarsat (WAT)", "Inmarsat East Atlantic - Aero", "Inmarsat East Atlantic - B HSD", "Inmarsat East Atlantic - B Voice/Data", "Inmarsat East Atlantic - M", "Inmarsat East Atlantic - M4 HSD", "Inmarsat East Atlantic - Mini M", "Inmarsat India Ocean - Aero", "Inmarsat India Ocean - B HSD", "Inmarsat India Ocean - B Voice/Data", "Inmarsat India Ocean - M", "Inmarsat India Ocean - M4 HSD", "Inmarsat India Ocean - Mini M", "Inmarsat Pacific Ocean - Aero", "Inmarsat Pacific Ocean - B HSD", "Inmarsat Pacific Ocean - B Voice/Data", "Inmarsat Pacific Ocean - M", "Inmarsat Pacific Ocean - M4 HSD", "Inmarsat Pacific Ocean - Mini M", "Inmarsat SNAC - Aero", "Inmarsat SNAC - B", "Inmarsat SNAC - B HSD", "Inmarsat SNAC - M", "Inmarsat SNAC - M4 HSD", "Inmarsat SNAC - Mini M", "Inmarsat West Atlantic - Aero", "Inmarsat West Atlantic - B HSD", "Inmarsat West Atlantic - B Voice/data", "Inmarsat West Atlantic - M", "Inmarsat West Atlantic - M4 HSD", "Inmarsat West Atlantic - Mini M", "International Networks", "Iran", "Iraq", "Ireland", "Iridium - 6", "Iridium - 7", "Israel", "Italy", "Ivory Coast", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Martinique", "Mauritania", "Mauritius", "Mayotte & Reunion Islands", "MCP Network", "Mexico", "Micronesia", "Moldova", "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Myanmar (Burma)", "Namibia", "Nauru", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Niue", "North Korea", "Norway", "Oman", "Oration Technologies Network", "Pakistan", "Palau", "Palestinian Territory", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Puerto Rico", "Qatar", "Romania", "Russia", "Rwanda", "Saint Kitts and Nevis", "Saint Lucia", "Saint Vincent and the Grenadines", "Saipan (Northern Mariana)", "Samoa", "San Marino", "Sao Tome and Principe", "Satellite Network", "Satellite Network - Global Networks", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Korea", "South Sudan","Spain", "Sri Lanka", "St. Helena", "St. Pierre & Miquelon", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Togo", "Tokelau", "Tonga", "Trinidad & Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks & Caicos Islands", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States", "UPT", "Uruguay", "US Virgin Islands", "Uzbekistan", "Vanuatu", "Vatican City", "Venezuela", "Vietnam", "Wake Island", "Wallis & Futuna Islands", "Yemen", "Zambia", "Zanzibar", "Zimbabwe"];

    sac = {
        included_countries: ["United States", "Canada", "Puerto Rico"],
        image_blacklist: ["AntarcticaChristmasIsland", "AntarcticaCocosIsland", "AntarcticaCasey", "AntarcticaDavis", "AntarcticaMaquarieStation", "AntarcticaNorfolkIsland", "AntarcticaScott", "EllipsoSatelliteSystem", "EastTimorTimorLeste", "EMSAT", "GlobalstarAvrasya", "GlobalMobilSatServ", "Honduras", "InmarsatAOR", "InmarsatIOR", "InmarsatPOR", "InmarsatWAT", "InmarsatEastAtlanticAero", "InmarsatSNAC", "InmarsatEastAtlanticBVoiceData", "InmarsatEastAtlanticM", "InmarsatEastAtlanticBHSD", "InmarsatEastAtlanticMiniM", "InmarsatIndiaOceanAero", "InmarsatEastAtlanticMHSD", "InmarsatIndiaOceanBVoiceData", "InmarsatIndiaOceanBHSD", "InmarsatIndiaOceanM", "InmarsatIndiaOceanMHSD", "InmarsatIndiaOceanMiniM", "InmarsatPacificOceanAero", "InmarsatPacificOceanBVoiceData", "InmarsatPacificOceanBHSD", "InmarsatPacificOceanMHSD", "InmarsatPacificOceanMiniM", "InmarsatPacificOceanM", "InmarsatSNACAero", "InmarsatSNACB", "InmarsatSNACBHSD", "InmarsatSNACMHSD", "InmarsatSNACMiniM", "InmarsatSNACM", "InmarsatWestAtlanticAero", "InmarsatWestAtlanticBHSD", "InmarsatWestAtlanticBVoicedata", "InmarsatWestAtlanticM", "InmarsatWestAtlanticMHSD", "InmarsatWestAtlanticMiniM", "InternationalNetworks", "Iridium", "MCPNetwork", "OrationTechnologiesNetwork", "SatelliteNetwork", "SatelliteNetworkGlobalNetworks", "UPT", "SouthSudan"],     
        location : "/plan-finder?country=",  // "/plan-finder/"
        country_list: {},
        init: function() {
            sac.country_list = sac.letterCount(countries_list);
            sac.dd = sac.dropdown();
            
        },
        load: function (container,input,button) {
    
        },
        attachDropdown : function (div) {
            $(div).insert({after: sac.dd});
            //.insert({after:new Element("br",{clear:"all",id:"dropdown_break"})})
            return true;
        },
        letterCount: function(c) {
            var current_letter = "", temp = {};
            c.each(function(x) {
                var letter = x.charAt(0);
                if (current_letter !== letter) {
                    current_letter = letter;
                    temp[current_letter] = [];
                }
                temp[current_letter].push(x);
            });
            return temp;
        },
        countryURL: function (country) {
            //return encodeURIComponent(country.strip().replace(/\s/g,"-"));
            return encodeURIComponent((country.strip()));
        },
        countryLookup: function(search) {
            //return false if empty 
       
            if (search.empty()) {
                return false;
            }
            //default value
            var predictions = [];
            //only capitalize first letter; may bug multi-word countries (ie. Puerto Rico)       
            search = search.capitalize();
            countries_list.each(function(x) {
                //if partial is in array and first character matches
                if (x.startsWith(search)) {
                    predictions.push(x);
                    //throw $break; //break .each loop
                }               
            });            
            return predictions;
        },
        countryJump: function(letter) {
      
            letter = letter.toUpperCase();
            if ($('letter_header_' + letter)) {
            
                var main_top = $('countries').offsetTop,
                    sup_top = $('letter_header_' + letter).offsetTop,
                    offset = 0;
                if (Prototype.Browser.IE || Prototype.Browser.WebKit || Prototype.Browser.Gecko) {
                    offset = 35;
                }
                $('countries').scrollTop = sup_top - main_top + offset;
            }
            return false;
        },
        search_position : 0,
        search_results : 0,
        search_highlight : function () {
            //sac.search_position = id;
            for (var i = 0; i <= sac.search_results; i++) {
                if (i != sac.search_position) {
                    //console.log('search_' + i);
                    $('search_' + i).removeClassName('search_mouseover');
                }
            }
            $('search_' + sac.search_position).addClassName('search_mouseover');
            //console.log('search_' + sac.search_position);
        },
        getPredictive: function (event,element) {            
            if (element.value.empty()) {
                //hide search
                toggleDropdown("hide","search_results");                
            } else {
                var key_pressed = event.keyCode ? event.keyCode : event.charCode;
                //console.log(key_pressed);
                
                switch (key_pressed) {
                    case 38:    //up arrow Event.KEY_UP
                        sac.search_position -= 1;
                        if (sac.search_position < 0) {
                            sac.search_position = sac.search_results;                            
                        }
                        sac.search_highlight();
                        break;
                    case 40:    //down arrow Event.KEY_DOWN
                        sac.search_position += 1;
                        if (sac.search_position > sac.search_results) {
                            sac.search_position = 0;
                        }
                        sac.search_highlight();
                        break;
                    case 13:    //Enter   Event.KEY_RETURN                                             
                        Event.stop(event);
                        var country = $$(".search_mouseover").first();                        
                        if (country) {
                            suppressSaveOffer = true; //supress popup
                            document.location = sac.location + sac.countryURL(country.innerHTML);
                            return false;                        
                        }
                        break;
                    default:
                        //search and display matches
                        var cap = 8, //display limit
                        predictions = sac.countryLookup(element.value);
                        if (predictions.length <= 0) {
                            //zero predictions, country not found
                            $('search_results').update(new Element("div",{"class":"search_not_found"}).insert("Country Not Found"));
                            sac.search_results = 0;
                        } else if (Object.isArray(predictions) && (predictions.length > 0)) {
                        
                            var len = predictions.length, 
                                count = 0,
                                div_holder = new Element("div"),
                                div;
                            predictions.each(function(x){
                                //build display?
                                div = new Element("div",{id:"search_" + count,"class":"results" + ((count === 0) ? " search_mouseover" :"")}).insert(x);
                                div.on("click", function() {
                                        //alert("click");
                                        //global variable set to suppress Save Offer
                                        suppressSaveOffer = true;
                                        document.location = sac.location + sac.countryURL(x);
                                });
                                div.on("mouseover", function (event,element) {
                                        //alert("mouseover");
                                        $$(".results").each(function(y) {
                                                    y.removeClassName('search_mouseover');
                                        });
                                        element.addClassName('search_mouseover');
                                });
                                div_holder.insert(div);
                                count++;
                                if (count >= cap) {
                                    //create 'more countries' row
                                    div_holder.insert(new Element("div").insert((len - cap) + " more countries"));
                                    throw $break;
                                }                                
                            });
                            sac.search_results = --count;
                            //throw new elements in div
                            $('search_results').update(div_holder);
                            toggleDropdown("show","search_results");
                        } else {
                            toggleDropdown("hide","search_results");
                        }
                }
                
            }
        },        
        dropdown: function() {
            //container DIV
            //will add event at later time
            var dropdown_country = new Element("div", {id: "dropdown_country"}),
            search_results_container = new Element("div", {id: "search_results_container", "class": "hidden"})
                    .insert(new Element("div", {id: "search_results_top"}).insert("&nbsp;"))
                    .insert(new Element("div", {id: "search_results"}))
                    .insert(new Element("div", {id: "search_results_bottom"}).insert("&nbsp;")),
            countries_container = new Element("div", {id: "countries_container", "class": "hidden"}),
            countries = sac.country_list;

            //build alphabet DIV    
            var alphabet = new Element("div", {id: "alphabet"}),
            a = "A", tempSpan;
            while ($R("A", "Z").include(a)) { 
            //alert("Hello");
                //create <span>
                
                tempSpan = new Element("span", {"class": "letter"}).insert(a); //.store("letter", a);
               
                tempSpan.on("click", function (e,el) {
                
                   sac.countryJump(el.innerHTML); 
                });
                //insert new <span> into temp storage
                alphabet.insert({bottom: tempSpan});
                //increment letter
                a = a.succ();
            }
            //build countries DIV
            var countries_div = new Element("div", {id: "countries"}),
            //<tbody> to fix IE7 issue
            countries_table = new Element("tbody"),
            count = 0, row, cell, obj, s;

            for (obj in countries) {
                //build letter row
                row = new Element("tr", {"class": "letter_header", id: "letter_header_" + obj})
                        .insert(new Element("td", {colspan: 2}).insert(obj));
                countries_table.insert(row);
                //create cell element
                row = new Element("tr");
                cell = new Element("td");
                //find the split point
                s = Math.ceil(countries[obj].length / 2);
                countries[obj].each(function(x, i) {
                    if ((i + 1) > s) {
                        //insert current cell
                        row.insert(cell);
                        //create a new cell element
                        cell = new Element("td");
                        s = 9999;
                    }
                    //create country DIV
                    var flag = x.replace(/[^a-zA-Z]/g,''),
                    flag_class = (sac.image_blacklist.indexOf(flag) >= 0) ?  "" : " sprites " + flag.toLowerCase();
                    
                    var link = new Element("a",{href:sac.location + sac.countryURL(x)}).insert(x);
                    link.on("click",function() {
                        suppressSaveOffer = true;
                    });
                    
                    //flag = (sac.image_blacklist.indexOf(flag) >= 0) ? "/resources/vonage-us/images/common/spacer.gif"  : flag_path + flag + ".png";
                    
                    var div = new Element("div", {id: "country_" + count, "class": "country"})
                            .insert(new Element("img", {"class":"pftFlag" + flag_class,width: 14, height: 14, src: "/resources/vonage-us/images/common/spacer.gif"}))
                            .insert(new Element("div", {"class": "country_name"}).insert(new Element("span")
                                                                .insert(link)))
                            .insert(new Element("div", {style: "clear:both"}));
                    cell.insert(div);
                    count++;
                });
                row.insert(cell);
                countries_table.insert(row);                
            }
            sac.country_count = count;
            countries_table = new Element("table",{cellspacing:"0"}).insert(countries_table);
            countries_div.insert(countries_table);
            countries_container.insert(alphabet).insert(countries_div);
            dropdown_country.insert(countries_container).insert(search_results_container);
            return dropdown_country;
        },
        
        dropdown_location : function (div) {
            var container = $(div).retrieve("container"), //from element
            next = $(container).next(); //check if dropdown is next            
            if (!next) {
                //find dropdown and destroy
                if ($('dropdown_country')) {
                    $('dropdown_country').remove();
                }
                sac.attachDropdown(container);
            }
        }
    };
    //end of sac Object    
    sacLoad = function (container,input,button) {         
        $(input).store("copy",$(input).value); //store input txt in element
        $(input,button).invoke("store","container",container);                     
        $(button).on("click", function () {   
            sac.dropdown_location(this);
            toggleDropdown("toggle","countries");                                
        });
        //prevent form from submitting
        $(input).up().on("submit",function(e,el) {
            Event.stop(e);
        });        
        
        $(input).on("focus", function (e,el) {       
            sac.dropdown_location(this);
            if (countries_list.indexOf(el.value) === -1) {
                el.value = "";
            }
        }); 
        
                  
        $(input).on("blur", function(e,el) {
            if (countries_list.indexOf(el.value) === -1) {
                //el.value = sac.inputCopy;
                el.value = $(el).retrieve("copy");
            }            
        });
        $(input).on("keyup", sac.getPredictive);                                                
    };  
    
    Event.observe(window,"load", function() {         
        sac.init();         
        if ($('plan-finder-navigation-select')) {
            sacLoad("plan-finder-navigation-select","pftCountryNav","planFinderBtnNav");
        }       
        if ($('planFinderSelect')) {
            sacLoad("planFinderSelect","pftCountry","planFinderBtn");
        }
    });    

} //end Prototype