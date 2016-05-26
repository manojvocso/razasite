var lb_center = false;

/*Generate unique name for Optimost and Site Catalyst*/
var page_url = window.location.href;
var pagename = 'lp_US_';
var parts = page_url.split("\?");
parts = parts[0].split("/");
pagename += parts[3] + '_';

var tracking_array = {};

var qs_params = page_url.toQueryParams();
pagename += qs_params.LPID + '_';
var countryList;
var bucketValue = '';
var rateValue = '';

//Dev
//var domain_name = 'http://10.111.128.54:5081/';
//var url ='http://10.111.128.54:8080/

//preprod
//var url = 'http://10.250.50.126:8080/
//QA
//var url ='http://qabekb.qa.s.vonagenetworks.net/
//http://qabekb.qa.s.vonagenetworks.net -> qaserver-76
//http://qaaeka.qa.s.vonagenetworks.net -> qaserver-276
//static fa1 qaserver-340

//PROD
//var url ='http://www.vonage.com/ajax/lightbox_ajax.php?content_id='+content_id+qs;

//var domain_name = 'http://qabekb.qa.s.vonagenetworks.net/';

var domain_name = 'http://qaserver-76.qa.s.vonagenetworks.net/';
if (window.location.hostname.indexOf('systesta-cmslivesite.qa.vonagenetworks.net') != -1) {
    domain_name = 'http://qaaeka.qa.s.vonagenetworks.net/';
}
if (window.location.hostname.indexOf('systestb-cmslivesite.qa.vonagenetworks.net') != -1) {
    domain_name = 'http://qabekb.qa.s.vonagenetworks.net/';
}
if (window.location.hostname.indexOf('qaserver-400.qa.s.vonagenetworks.net') != -1) {
    domain_name = 'http://qaserver-340.qa.s.vonagenetworks.net/';
}
if (window.location.hostname.indexOf('vonagenetworks.net') == -1) {
    domain_name = 'http://www.vonage.com/';
}
if (window.location.hostname.indexOf('vonagelabs.com') != -1) {
    domain_name = 'http://10.250.50.126:8080/';
}

if (parts.length < 5) {
    pagename += 'index';
} else {
    pagename += parts[4].substr(0, parts[4].indexOf("."));
}
pagename = pagename.toLowerCase();



/*
 //DO NOT UNCOMMENT, domain_name should come from common.js
 //Dev
 //var domain_name = 'http://10.111.128.54:5081/';
 //var url ='http://10.111.128.54:8080/

 //preprod
 //var url = 'http://10.250.50.126:8080/
 //QA
 //var url ='http://qabekb.qa.s.vonagenetworks.net/
 //http://qabekb.qa.s.vonagenetworks.net -> qaserver-76
 //http://qaaeka.qa.s.vonagenetworks.net -> qaserver-276
 //fa1 qaserver-340

 //PROD
 //var url ='http://www.vonage.com/ajax/lightbox_ajax.php?content_id='+content_id+qs;

 //var domain_name = 'http://qabekb.qa.s.vonagenetworks.net/';
 var domain_name = 'http://qaserver-76.qa.s.vonagenetworks.net/';
 //var domain_name = 'http://qaserver-340.qa.s.vonagenetworks.net/';
 if(window.location.hostname.indexOf('vonagenetworks.net') == -1 ){
 domain_name ='http://www.vonage.com/';
 }
 if(window.location.hostname.indexOf('vonagelabs.com') != -1 ){
 domain_name ='http://vonage.vonagelabs.com/';
 }
 */
/*
 //Spanish template
 if(window.location.href.indexOf('/latin') != -1 && (window.location.href.indexOf('/latin_') == -1 && content_id.indexOf('_espanol') == -1)){
 domain_name ='http://espanol.vonage.com/';
 }
 */


/** Start STATIC LIGHTBOX CALL **/


function getStaticLightbox(content_id, lb_width, lb_height, meta_object, skin_type) {
    if (content_id == '') {
        content_id = '/templatedata/vonage_us/lightbox/data/money_back_guarantee.xml';
    }
    //optional parameters to override the width and height specified in the ajax file
    lb_width = (isNaN(parseInt(lb_width))) ? 0 : parseInt(lb_width);
    lb_height = (isNaN(parseInt(lb_height))) ? 0 : parseInt(lb_height);
    if (meta_object == undefined) {
        meta_object = '';
    }
    if (skin_type == undefined) {
        skin_type = '';
    }

    var left_margin = 0;
    if (lb_center == true) {
        left_margin = (document.viewport.getWidth() / 2) - (350 / 2);
    } else {
        left_margin = 325;
    }
    //populate lightbox with "Loading" placeholder
    $('lightbox_title').innerHTML = '&nbsp;';
    $('lightbox_content').innerHTML = '<div style="padding:70px 0 0 115px;"><img src="/resources/vonage-us/images/common/loading.gif" width="31" height="31" alt="" style="vertical-align:middle;"/> <span style="padding-left:10px; color:#333333; font-size:14px;">Loading</span></div>';

    $('lightbox_container').setStyle({width: '350px', left: left_margin + 'px'});
    $('lightbox_inner_container').setStyle({minHeight: '220px'});
    //Show lightbox
    window_resize_event();
    $('lightbox_background').removeClassName('hidden');
    $('lightbox_container').removeClassName('hidden');

    //position top of lightbox 50px from the top of the visible content
    $('lightbox_container').setStyle({top: (getScrollPosition() + 50) + 'px'});


	var completeUrl;
	var querystring=window.location.search.substring(1);
	if (skin_type !='') {
		if(querystring != null && querystring.length !=0){
			completeUrl = lightboxUrl+"?dcr="+content_id+"&skin="+skin_type+ "&" + querystring;
		} else {
			completeUrl = lightboxUrl+"?dcr="+content_id+"&skin="+skin_type;
		}
	} else {
		if(querystring != null && querystring.length !=0){
			completeUrl = lightboxUrl+"?dcr="+content_id+"&"+querystring;
		} else {
			completeUrl = lightboxUrl+"?dcr="+content_id;
		}
	}
	var planTitle ='';
	if(content_id.match(/^\/templatedata\/vonage_us\/lightbox\/data\/world_countries/)){
		planTitle=(meta_object.planName==undefined)?'Vonage World':meta_object.planName;
	}

    new Ajax.Request(completeUrl, {
        method: 'post',
        asynchronous: false,
        onSuccess: function(transport) {
            var content = transport.responseText;
            /*var temporaryElement = document.createElement('div');
            temporaryElement.setAttribute('id', 'tempLightboxContent');
            temporaryElement.setAttribute('style', 'display:none;');
            temporaryElement.innerHTML = content;
            document.getElementsByTagName('body')[0].appendChild(temporaryElement);*/
            var temporaryElement = new Element("div").insert(content),
            lightbox = {
                content : temporaryElement.select("#lightboxcontent")[0],
                title : temporaryElement.select("#lightboxtitle")[0],
                height : temporaryElement.select("#lightboxheight")[0],
                width : temporaryElement.select("#lightboxwidth")[0],
                className: temporaryElement.select("#lightboxclass")[0],
                metaobj : temporaryElement.select("#lightboxmetaobj")[0]
            };


            //document.getElementById('lightboxcontent')
            if (lightbox.content) {
                //document.getElementById('lightboxtitle').innerHTML != ''
                if (lightbox.title) {
                    /*$('lightbox_title').innerHTML = document.getElementById('lightboxtitle').innerHTML;
                    $('lightbox_title').innerHTML += planTitle;*/
                    $('lightbox_title').update(lightbox.title.innerHTML).insert(planTitle);
                }
                //lightbox height
                if (!lightbox.height.innerHTML.empty())  {
                    if (lb_height == 0) {
                        lb_height = lightbox.height.innerHTML;
                    }
                } else {
                    if (lb_height == 0) {

                        lb_height = "900";
                    }
                }
                //lightbox width
                if (!lightbox.width.innerHTML.empty())
                {
                    if (lb_width == 0) {
                        lb_width = lightbox.width.innerHTML;
                    }
                }
                else
                {
                    if (lb_width == 0) {
                        lb_width = "800";
                    }
                }
                //lightbox class
                if (lightbox.className)    {
                    $('lightbox_container').addClassName(lightbox.className.innerHTML);
                }

                if (lightbox.content) {
                    if (typeof meta_object == "object") {
                        var metaObj;
                        if ($('metaObj') == undefined) {
                            metaObj = document.createElement('script');
                            document.getElementsByTagName('body')[0].appendChild(metaObj);
                        } else {
                            metaObj = $('metaObj');
                        }
                        metaObj.update('var metaObject=' + Object.toJSON(meta_object) + ';');
                    }

                    $('lightbox_content').innerHTML = lightbox.content.innerHTML;
                }
                else
                {
                    $('lightbox_content').update("Specified content does not exist");
                    $('lightbox_title').update("ERROR");
                    lb_height = "200";
                    lb_width = "300";
                }
                //rates lightbox code
                if (lightbox.metaobj) {
                    countryList = ['Canada', 'Puerto Rico', 'United States'];
                    var lightboxMetaObj = lightbox.metaobj.innerHTML;
                   //var lightboxJson = JSON.parse(lightboxMetaObj); // does not work in IE7
                   var lightboxJson = lightbox.metaobj.innerHTML.evalJSON();
                    var planCodes = "";
                    bucketValue = '';
                    rateValue = '';
                    planRate = '';
                    alphabet = {};
                    if (lightboxJson.bucket) {
                        cardValue = lightboxJson.plancodes[0];
                        bucketValue = lightboxJson.bucket;
                        if (lightboxJson.rate) {
                            rateValue = lightboxJson.rate;
                        }
                    }
                    for (var i = 0; i < lightboxJson.plancodes.length; i++) {
                        planCodes = lightboxJson.plancodes[i] + "," + planCodes;
                    }
                    new Ajax.Request("/VUSCS/ipdl/data/world", {
                        parameters: {
                            codes: planCodes,
                            format: "json"},
                        method: 'post',
                        onSuccess: function(transport) {
                            var returned = transport.responseText.evalJSON(true);
                            //alert(returned.genericDataList[0].resultArr.length);
                            //alert(returned.genericDataList[0].resultArr[0][1]);
                            var ratesJsonObj = {};
                            ratesJsonObj = plansData(returned, 'Country');
                            var letter = "A";

                            //ooo
                            table = new Element("tbody");

                            for (var countryName in ratesJsonObj)  {
                                /*var row = table.insertRow(-1);
                                var cell1 = row.insertCell(0);
                                var cell2 = row.insertCell(1);
                                var cell3 = row.insertCell(2);*/
                                var row, cell1, cell2, cell3;
                                var trimCountryName = countryName.replace(/[\s\(\)&]/g, '');
                                //row.id = trimCountryName;
                                var clickFn;

                                if (trimCountryName.indexOf(letter) === 0) {
                                    alphabet[letter] = trimCountryName;
                                    letter = letter.succ();
                                    //hack; no countries begin with an 'X'
                                    if (letter === 'X') {
                                        alphabet[letter] = "";
                                        letter = letter.succ();
                                    }
                                }

                                //ooo
                                row = new Element("tr",{id:trimCountryName});

                                if (ratesJsonObj[countryName]['Special'] && !ratesJsonObj[countryName]['Landline'] && !ratesJsonObj[countryName]['Cellular'])
                                {
                                    //cell3.innerHTML = "N/A";
                                    //cell3.className = "column2 rateTableCell";
                                    cell3 = new Element("td",{"class":"column2 rateTableCell"}).insert("N/A");
                                    //cell2.innerHTML = ratesJsonObj[countryName]['Special'][1];
                                    //cell2.className = "column1 rateTableCell";
                                    cell2 = new Element("td",{"class":"column1 rateTableCell"}).insert(ratesJsonObj[countryName]['Special'][1]);
                                    clickFn = "return openCityRates('" + countryName + "','" + trimCountryName + "','" + planCodes + "','" + "" + "');";
                                }
                                else
                                {
                                    //cell3.innerHTML = ratesJsonObj[countryName]['Cellular'][1];
                                    //cell3.className = "column2 rateTableCell";
                                    cell3 = new Element("td",{"class":"column2 rateTableCell"}).insert(ratesJsonObj[countryName]['Cellular'][1]);
                                    //cell2.innerHTML = ratesJsonObj[countryName]['Landline'][1];
                                    //cell2.className = "column1 rateTableCell";
                                    cell2 = new Element("td",{"class":"column1 rateTableCell"}).insert(ratesJsonObj[countryName]['Landline'][1]);
                                    clickFn = "return openCityRates('" + countryName + "','" + trimCountryName + "','" + planCodes + "');";//+ "','" + ratesJsonObj[countryName]['Cellular'][1]
                                }
                                var countryLink = trimCountryName + 'Link';
                                var countryPlusMinus = trimCountryName + '_plusminus';
                                //cell1.innerHTML = '<a onclick="' + clickFn + '" href="#" id="' + countryLink + '"><img style="display:inline;" id="' + countryPlusMinus + '" src="/resources/vonage-us/images/common/vonage_plus.gif"/>' + countryName + '</a>';
                                //cell1.className = "rateTableCountry";
                                cell1 = new Element("td",{"class":"rateTableCountry"});
                                cell1.insert('<a onclick="' + clickFn + '" href="#" id="' + countryLink + '"><img style="display:inline;" id="' + countryPlusMinus + '" src="/resources/vonage-us/images/common/vonage_plus.gif"/>' + countryName + '</a>');

                                row.insert(cell1).insert(cell2).insert(cell3);
                                table.insert(row);
                            }
                            $('rateTableDiv').insert(table.wrap('table',{id:"rateTable"}));
                            //build and insert alphabet table
                            var d = new Element("div");
                            for (var a in alphabet) {
                                var link;
                                if (alphabet[a].empty()) {
                                    //prevent broken link on 'X'
                                    link = a;
                                } else {
                                    link = new Element("a",{href:"#" + alphabet[a]}).update(a);
                                    link.on("click", function (event,element) {
                                       Event.stop(event);
                                       icr.ratesJump(element.getAttribute("href").substr(1));
                                    });
                                }
                                d.insert(link).insert(new Element("br"));
                            }
                            $('rateTableAlphabet').update(d);

                        }
                    });
                }
            }
            else
            {
                $('lightbox_content').update("Specified content does not exist");
                $('lightbox_title').update("ERROR");
                lb_height = "200";
                lb_width = "300";
            }

				//$('tempLightboxContent').remove();

				//Disable lightbox links inside lightbox
					disable_lightbox_links();
			}
		});

    if (is_ie6 == true) {
        //shut off the select dropdown on the page for ie6 only
        $$('select').each(function(element) {
            element.style.visibility = "hidden";
        });
    }

    if (lb_width > 0) {
        var lb_left = (1000 - lb_width) / 2; //Site is 1000px wide. This will center it within the content not the window
        if (lb_center == true) {

            lb_left = Math.floor((document.viewport.getWidth() / 2) - (lb_width / 2));
        }
        $('lightbox_container').setStyle({width: lb_width + 'px', left: lb_left + 'px'});
    }
    $('lightbox_inner_container').setStyle({minHeight: '1px'});
    if (lb_height > 0) {
        $('lightbox_inner_container').setStyle({minHeight: lb_height + 'px'});
    }

    //While lightbox is open listen for window resize to activly update the background width and height
    Event.observe(window, 'resize', window_resize_event);
    window_resize_event();
    if (content_id == 'rates_small_business' || content_id == 'rates_world_compare' || content_id == 'rates_residential_compare') {
        compare_resize();
    }

}

/*		var url = "/VUSCS/lscs/document";
 new Ajax.Request (url, {
 parameters:	{lscsurl:"http://devserver-380.dev.s.vonagenetworks.net/lscs/v1/document$?q=TeamSite/Templating/DCR/Type:vonage_us/lightbox AND LiveSite/LightBox/ID:%23" + content_id + "%23",format:"json",project:projectName,includeContent:"true",siteName:siteName,queryString:queryString
 },
 method: 'post',
 onSuccess: function(transport){
 var returned=transport.responseText.evalJSON(true);
 var content=returned.vonageDocumentList[0].content;
 var xmlDoc = null;

 if (window.DOMParser) {
 parser = new DOMParser();
 xmlDoc = parser.parseFromString(content,"text/xml");
 }
 else // Internet Explorer
 {
 xmlDoc=new ActiveXObject("Microsoft.XMLDOM");
 xmlDoc.async="false";
 xmlDoc.loadXML(content);
 }

 if(xmlDoc.getElementsByTagName("Title")[0].childNodes[0] != null)
 {
 var title = xmlDoc.getElementsByTagName("Title")[0].childNodes[0].nodeValue;
 $('lightbox_title').innerHTML=title;
 }

 if(xmlDoc.getElementsByTagName("Content")[0].childNodes[0] != null)
 {
 //var body = xmlDoc.getElementsByTagName("Content")[0].childNodes[0].nodeValue;
 var body = '';
 var tmp = xmlDoc.getElementsByTagName("Content")[0];
 for (i=0; i<tmp.childNodes.length; i++)
 {
 body += tmp.childNodes[i].nodeValue;
 }

 $('lightbox_content').innerHTML=body;

 }

 if(xmlDoc.getElementsByTagName("ClassName")[0].childNodes[0] != null)
 {
 var className = xmlDoc.getElementsByTagName("ClassName")[0].childNodes[0].nodeValue;
 if (className!='') {
 $('lightbox_container').addClassName(className[2]);
 }
 }
 if(xmlDoc.getElementsByTagName("Width")[0].childNodes[0] != null)
 {
 var width = xmlDoc.getElementsByTagName("Width")[0].childNodes[0].nodeValue;
 //console.log("width value is:" + width);
 //set width and height specified in the ajax file unless width and height were passed to this function
 if (lb_width == 0 && !isNaN(width)) {
 lb_width = width;
 //console.log("lb width value is:" + lb_width);
 }
 }

 if(xmlDoc.getElementsByTagName("Height")[0].childNodes[0] != null)
 {
 var height = xmlDoc.getElementsByTagName("Height")[0].childNodes[0].nodeValue;
 if (lb_height == 0 && !isNaN(height)) {
 lb_height = height;
 }
 }


 if (is_ie6 == true) {
 //shut off the select dropdown on the page for ie6 only
 $$('select').each(function(element) { element.style.visibility = "hidden";});
 }

 if (lb_width>0) {
 var lb_left=(1000-lb_width)/2; //Site is 1000px wide. This will center it within the content not the window
 if (lb_center==true) {

 lb_left=Math.floor((document.viewport.getWidth()/2)-(lb_width/2));
 }
 $('lightbox_container').setStyle({width:lb_width+'px', left:lb_left+'px'});
 }
 $('lightbox_inner_container').setStyle({minHeight:'1px'});
 if (lb_height>0) {
 $('lightbox_inner_container').setStyle({minHeight:lb_height+'px'});
 }

 //While lightbox is open listen for window resize to activly update the background width and height
 Event.observe(window,'resize', window_resize_event);
 window_resize_event();
 if (content_id=='rates_small_business' || content_id=='rates_world_compare' || content_id=='rates_residential_compare') {
 compare_resize();
 }

 }
 });
 } */


function getFileContentByURI(uri, lb_width, lb_height, meta_object) {
    var domain_name = "/";
    var branch = "project=//webdev-cms-team-01.kewr1.s.vonagenetworks.net/default/main/Static/Vonage_US/Dev";
    var format = "&format=json";
    var url = domain_name + 'lscs/v1' + uri + '?' + branch + format;
    //alert('url : ' + url);
    new Ajax.Request(url, {
        parameters: {
        },
        method: 'get',
        onSuccess: function(transport) {
            xscontent = transport.responseXML;
            var title = xscontent.getElementsByTagName('Title')[0].textContent;
            var content = xscontent.getElementsByTagName('Content')[0].textContent;
            var className = xscontent.getElementsByTagName('ClassName')[0].textContent;
            var width = xscontent.getElementsByTagName('Width')[0].textContent;

            if (is_ie6 == true) {
                //shut off the select dropdown on the page for ie6 only
                $$('select').each(function(element) {
                    element.style.visibility = "hidden";
                });
            }
            if (className != '') {
                $('lightbox_container').addClassName(className);
            }

            //replace "loading" placeholder with requested content
            $('lightbox_title').innerHTML = title;
            $('lightbox_content').innerHTML = content;

            //set width and height specified in the ajax file unless width and height were passed to this function
            if (lb_width == 0 && !isNaN(width)) {
                lb_width = width;
            }


            if (lb_width > 0) {
                var lb_left = (1000 - lb_width) / 2; //Site is 1000px wide. This will center it within the content not the window
                if (lb_center == true) {

                    lb_left = Math.floor((document.viewport.getWidth() / 2) - (lb_width / 2));
                }
                $('lightbox_container').setStyle({width: lb_width + 'px', left: lb_left + 'px'});
            }
            $('lightbox_inner_container').setStyle({minHeight: '1px'});

            //While lightbox is open listen for window resize to activly update the background width and height
            Event.observe(window, 'resize', window_resize_event);
            window_resize_event();
            if (content_id == 'rates_small_business' || content_id == 'rates_world_compare' || content_id == 'rates_residential_compare') {
                compare_resize();
            }



        }
    });
}

/** END STATIC LIGHTBOX CALL **/

function getDIVContentByCountry(content_id, div_id, country, country_image) {

    var url = domain_name + 'ajax/lightbox_ajax.php?content_id=' + content_id;
    if (window.location.hostname.indexOf('vonagenetworks.net') == -1
            && window.location.hostname.indexOf('vonagelabs.com') == -1) {
        url = 'http://www.vonage.com/ajax/lightbox_ajax.php?content_id=' + content_id;
    }
    /*
     if(window.location.href.indexOf('sdsystestb-cmslivesite') == -1){
     url ='http://esvonageqaaekb.convertlanguage.com/sdqabekb/sdqa/sds/dvonagenetworks/ajax/lightbox_ajax.php?content_id='+content_id;
     }
     */
    new Ajax.Request('/WVonage/getContent?url=' + url, {
        parameters: {
            content_id: content_id, //corresponds to the appropriate content in the ajax file
            dcc_country: country
        },
        method: 'post',
        onSuccess: function(transport) {
            if (is_ie6 == true) {
                //shut off the select dropdown on the page for ie6 only
                $$('select').each(function(element) {
                    element.style.visibility = "hidden";
                });
            }
            var returned = transport.responseText.evalJSON(true);
            response = returned['content'].evalJSON(true);
            if ($(div_id)) {
                $(div_id).innerHTML = response[0];
            }
            dccRates(country, country_image);
            if (country == 'India' && ($('dccRatesTable') != null && $('dccRatesDropdown') != null)) {
                $('dccRatesTable', 'dccDivider').invoke('hide');
                $('dccRatesDropdown').removeClassName('fixWidth').setStyle({marginBottom: '30px'});
            }
        }
    });
    return false;
}
function getDIVContent(content_id, div_id) {
    var url = domain_name + 'ajax/lightbox_ajax.php?content_id=' + content_id;
    if (window.location.hostname.indexOf('vonagenetworks.net') == -1
            && window.location.hostname.indexOf('vonagelabs.com') == -1) {
        url = 'http://www.vonage.com/ajax/lightbox_ajax.php?content_id=' + content_id;
    }
    /*
     if(window.location.href.indexOf('sdsystestb-cmslivesite') == -1){
     url ='http://esvonageqaaekb.convertlanguage.com/sdqabekb/sdqa/sds/dvonagenetworks/ajax/lightbox_ajax.php?content_id='+content_id;
     }
     */
    new Ajax.Request('/WVonage/getContent?url=' + url, {
        parameters: {
            content_id: content_id //corresponds to the appropriate content in the ajax file
        },
        method: 'post',
        onSuccess: function(transport) {
            if (is_ie6 == true) {
                //shut off the select dropdown on the page for ie6 only
                $$('select').each(function(element) {
                    element.style.visibility = "hidden";
                });
            }
            var returned = transport.responseText.evalJSON(true);
            response = returned['content'].evalJSON(true);
            if ($(div_id)) {
                $(div_id).innerHTML = response[0];
            }
        }
    });
    return false;
}
function popUp(content_id, lb_width, lb_height, meta_object, skin_type) {

    if (content_id.match(/^\/templatedata/)) {
        getStaticLightbox(content_id, lb_width, lb_height, meta_object, skin_type);
    }

    else {

        if (content_id == 'login_lp') {
            dynamic_tracking({'pageName': 'login_intlp', 'prop38': 'login_intlp', 'eVar38': 'login_intlp'});
        }
        //optional parameters to override the width and height specified in the ajax file
        lb_width = (isNaN(parseInt(lb_width))) ? 0 : parseInt(lb_width);
        lb_height = (isNaN(parseInt(lb_height))) ? 0 : parseInt(lb_height);

        if (meta_object == undefined) {
            meta_object = '';
        }

        var left_margin = 0;
        if (lb_center == true) {
            left_margin = (document.viewport.getWidth() / 2) - (350 / 2);
        } else {
            left_margin = 325;
        }
        //populate lightbox with "Loading" placeholder
        $('lightbox_title').innerHTML = '&nbsp;';
        $('lightbox_content').innerHTML = '<div style="padding:70px 0 0 115px;"><img src="/resources/vonage-us/images/common/loading.gif" width="31" height="31" alt="" style="vertical-align:middle;"/> <span style="padding-left:10px; color:#333333; font-size:14px;">Loading</span></div>';

        $('lightbox_container').setStyle({width: '350px', left: left_margin + 'px'});
        $('lightbox_inner_container').setStyle({minHeight: '220px'});

        //Show lightbox
        window_resize_event();
        $('lightbox_background').removeClassName('hidden');
        $('lightbox_container').removeClassName('hidden');

        //position top of lightbox 50px from the top of the visible content
        $('lightbox_container').setStyle({top: (getScrollPosition() + 50) + 'px'});

        //Record opening with SiteCatalyst
//		link_tracking('lightbox_'+content_id);
        if (content_id == 'rates_residential_compare' || content_id == 'rates_small_business' || content_id == 'rates_world_compare') {
            sc_pagename('compare_plans_all');
        }
        if (content_id == 'vw_ec_subscribe' || content_id == 'wpu_ec_subscribe' || content_id == 'uscu_ec_subscribe' || content_id == 'usc300_ec_subscribe' || content_id == 'usc750_ec_subscribe' || content_id == 'vpro_ec_subscribe' || content_id == 'sbpu_ec_subscribe' || content_id == 'sbB1500_ec_subscribe') {
            var hostname = window.location.hostname;
            var param = "plan=" + content_id + "&HTTP_HOST=" + hostname; // pass content id for swapping subcribe url according to the plan name.
            content_id = 'ec_subscribe';
        }

        if (content_id == 'ec_subscribe') {
            sc_pagename('EC_verification_option_lightbox');
        }
        //if available, capture the refer_id querystring parameter and attach to ajax request
        //This is needed to ensure that links within the lightbox retain the refer_id value
        var qs = '';
        if (getQueryVar('refer_id') != '') {
            //qs='?refer_id='+getQueryVar('refer_id');
            qs = '_#_refer_id=' + getQueryVar('refer_id');
        }

        var url = '/resources/php/ajax/lightbox_ajax.php?content_id=' + content_id + qs;
        /*
         if(window.location.hostname.indexOf('vonagenetworks.net') == -1
         && window.location.hostname.indexOf('vonagelabs.com') == -1){
         url ='http://www.vonage.com/ajax/lightbox_ajax.php?content_id='+content_id+qs;
         }

         //Spanish template
         if(window.location.href.indexOf('/latin') != -1 && (window.location.href.indexOf('/latin_') == -1 && content_id.indexOf('_espanol') == -1)){
         url = 'http://espanol.vonage.com/ajax/lightbox_ajax.php?content_id='+content_id+qs;
         }
         */
        var siteName = window.location.pathname.replace(/\/shared\/.*/g, "");

        new Ajax.Request(url, {
            parameters: {
                content_id: content_id, //corresponds to the appropriate content in the ajax file
                hostname: window.location.host + siteName,
                meta_object: Object.toJSON(meta_object) //optional data object to be passed to the backend pages
            },
            method: 'post',
            onSuccess: function(transport) {

                if (is_ie6 == true) {
                    //shut off the select dropdown on the page for ie6 only
                    $$('select').each(function(element) {
                        element.style.visibility = "hidden";
                    });
                }

                var returned = transport.responseText.evalJSON(true);
                //response=returned['content'].evalJSON(true);

                response = eval(transport.responseText);

                if (response[5] != null) {
                    $('lightbox_container').addClassName(response[5]);
                }

                //replace "loading" placeholder with requested content
                $('lightbox_title').innerHTML = response[1];
                $('lightbox_content').innerHTML = response[0];

                //set width and height specified in the ajax file unless width and height were passed to this function
                if (lb_width == 0 && !isNaN(response[2])) {
                    lb_width = response[2];
                }

                if (lb_height == 0 && !isNaN(response[3])) {
                    lb_height = response[3];
                }

                if (lb_width > 0) {
                    var lb_left = (1000 - lb_width) / 2; //Site is 1000px wide. This will center it within the content not the window
                    if (lb_center == true) {

                        lb_left = Math.floor((document.viewport.getWidth() / 2) - (lb_width / 2));
                    }
                    $('lightbox_container').setStyle({width: lb_width + 'px', left: lb_left + 'px'});
                }
                if (lb_height > 0) {
                    $('lightbox_inner_container').setStyle({minHeight: lb_height + 'px'});
                }

                if (typeof dynamic_tracking != 'undefined') {
                    dynamic_tracking(response[4]);
                }

                //While lightbox is open listen for window resize to activly update the background width and height
                Event.observe(window, 'resize', window_resize_event);
                window_resize_event();
                if (content_id == 'rates_small_business' || content_id == 'rates_world_compare' || content_id == 'rates_residential_compare') {
                    compare_resize();
                }
            }
        });

    }

    return false;
}

function find_in_array(arr, obj) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] == obj)
            return true;
    }
}
function close_lightbox() {
    Event.stopObserving(window, 'resize', window_resize_event); //stop listening for window resize.
    $$('select').each(function(element) {
        element.style.visibility = "visible";
    }); //Show any elements that were hidden in ie6

    $w($('lightbox_container').className).each(function(class_name) {
        $('lightbox_container').removeClassName(class_name);
    });

    //hide lightbox
    $('lightbox_background').addClassName('hidden');
    $('lightbox_container').addClassName('hidden');
    $('lightbox_content').innerHTML = '';

    return false;
}

//This was an existing function that probably needs work.
function window_resize_event() {

    if (typeof window.innerWidth != 'undefined') {
        viewportheight = window.innerHeight;
    } else {
        viewportheight = document.documentElement.clientHeight;
    }

    var blanket_height = getDocHeight();

    if (/MSIE/.test(navigator.userAgent)) { //test for MSIE x.x; http://www.javascriptkit.com/javatutors/navigator.shtml
        document.getElementsByTagName('body')[0].scrollHeight;
        document.getElementsByTagName('body')[0].scrollWidth;
        if (document.getElementsByTagName('body')[0].scrollHeight > blanket_height) { //scroll
            $('lightbox_background').style.height = document.getElementsByTagName('body')[0].scrollHeight + 'px';
            if (document.getElementsByTagName('body')[0].scrollWidth > document.documentElement.clientWidth) {
                $('lightbox_background').style.width = document.getElementsByTagName('body')[0].scrollWidth + 'px';
            } else {
                $('lightbox_background').style.width = document.documentElement.clientWidth + 'px';
            }
        } else {
            $('lightbox_background').style.height = blanket_height + 'px';
            $('lightbox_background').style.width = '100%';
        }
    } else {
        $('lightbox_background').style.height = blanket_height + 'px';
    }

    return false;
}

function getScrollPosition() {
    var scrOfY = 0;
    if (typeof(window.pageYOffset) == 'number') {
        //Netscape compliant
        scrOfY = window.pageYOffset;
    } else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
        //DOM compliant
        scrOfY = document.body.scrollTop;
    } else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
        //IE6 standards compliant mode
        scrOfY = document.documentElement.scrollTop;
    }
    return scrOfY;//[ scrOfX, scrOfY ]
}

function getDocHeight() {
    var D = document;
    return Math.max(
            Math.max(D.body.scrollHeight, D.documentElement.scrollHeight),
            Math.max(D.body.offsetHeight, D.documentElement.offsetHeight),
            Math.max(D.body.clientHeight, D.documentElement.clientHeight)
            );
}
//initialize the close button after the page loads
Event.observe(window, 'load', function() {
    if (Object.isElement($('lightbox_close_button'))) {
        Event.observe('lightbox_close_button', 'click', function() {
            close_lightbox();
        });
    }

    if (Object.isElement($('lightbox_background'))) {
        Event.observe('lightbox_background', 'click', function() {
            close_lightbox();
        });
    }
});


function ie_version() {
    var rv = -1; // Return value assumes failure.
    if (navigator.appName == 'Microsoft Internet Explorer') {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null) {
            rv = parseFloat(RegExp.$1);
        }
    }
    return rv;
}

var is_ie6 = (ie_version() > 0 && ie_version() <= 6) ? true : false;




    //specific function
    function printWindow (n) {
        window.open('/resources/php/lightbox/extensions/content.php?print=' + n,'printExtensions','menubar=no,location=no,resizable=no,scrollbars=no,status=no,height=500,width=700');
        return false;
    }

function redirect_user() {
    var radioObj = document.user_form.cust_action;
    var radioLength = radioObj.length;
    for (var i = 0; i < radioLength; i++) {
        if (radioObj[i].checked) {
            var cust_action = radioObj[i].value;
        }
    }
    if (cust_action == 'subscribe')
        cust_action = metaObject.subscribeURL;
    window.location = cust_action;
}
function compare_resize() {
    var compare_body_height = $('plan_compare_body').getHeight();
    var compare_body_table_height = $('plan_compare_body_table').getHeight();
    var lightbox_height = $('lightbox_inner_container').getHeight();
    var viewport_height = document.viewport.getHeight();
    var lightbox_top_margin = parseInt($('lightbox_container').getStyle('top'));
    var lightbox_border_width = parseInt($('lightbox_border').getStyle('paddingTop'));


    var lightbox_no_body = (lightbox_border_width * 2) + (lightbox_top_margin * 2) + lightbox_height - compare_body_height;

    var max_body_height = viewport_height - lightbox_no_body;

    //console.log(max_body_height);

    if (max_body_height > compare_body_table_height) {
        //$('last_col_body').setStyle({'width':($('last_col_head').getWidth()-22)+'px'});
        max_body_height = compare_body_table_height;
    }


    if (max_body_height >= 135) {
        $('plan_compare_body').setStyle({'height': max_body_height + 'px'});
    }
}
//Create lightbox html
document.write('<div id="lightbox_background" class="hidden"></div><div id="lightbox_container" class="hidden"><div id="lightbox_border"><div id="lightbox_inner_container"><div id="lightbox_title_bar"><div id="lightbox_title"></div><span id="lightbox_close_button">&times;</span></div><div id="lightbox_content"></div></div></div></div>');

//Premium Countries Tooltip

/*
 Event.observe(window, 'load', function() {
 var premium_minutes_container_size=$('premium_minutes_countries_container').getDimensions();

 $$('table.premium_minutes_table span').each(function(e) {
 Event.observe(e, 'mouseover', function() {
 $(e).addClassName('bold');
 show_tooltip(e);
 });
 Event.observe(e, 'mouseout', function() {
 $(e).removeClassName('bold');
 tooltip_timer=setTimeout('hide_tooltip()',50);
 });

 });
 });
 */

var tooltip_timer = '';

function hide_tooltip_delayed() {
    $('tooltip_pointer').addClassName('hidden');
    $('tooltip_content').addClassName('hidden');
}

function hide_tooltip(e) {
    $(e).removeClassName('bold');
    tooltip_timer = setTimeout('hide_tooltip_delayed()', 50);
}

function show_tooltip(e) {

    clearTimeout(tooltip_timer);

    $(e).addClassName('bold');

    var tooltip_data = $(e).nextSibling.innerHTML.split(',');

    $('tooltip_country_name').innerHTML = $(e).innerHTML;
    $('tooltip_country_type').innerHTML = '(' + tooltip_data[1] + ')';
    $('tooltip_rate').innerHTML = tooltip_data[0];

    $('tooltip_pointer').removeClassName('hidden');
    $('tooltip_content').removeClassName('hidden');

    var p_dim = $('tooltip_pointer').getDimensions();
    var c_dim = $('tooltip_content').getDimensions();

    var e_pos = $(e).positionedOffset();
    var e_dim = $(e).getDimensions();
    var p_left = e_pos.left + e_dim.width + 5;
    var c_pos = $('tooltip_content').positionedOffset();

    var premium_minutes_container_size = $('premium_minutes_countries_container').getDimensions();
    var available_right = premium_minutes_container_size.width - (e_pos.left + e_dim.width) - 10;
    var available_bottom = premium_minutes_container_size.height - (e_pos.top + Math.floor((e_dim.width / 2))) - 10;

    if (available_bottom > p_dim.height + c_dim.height - 2) {
        pos = 't';
        p_top = e_pos.top + (Math.ceil(e_dim.height / 2));
        c_top = p_top + p_dim.height - 2;
    } else {
        pos = 'b';
        p_top = e_pos.top - p_dim.height + (Math.ceil(e_dim.height / 2));
        c_top = p_top - p_dim.height + 3 - (parseInt($('tooltip_content').getStyle('paddingTop')) * 2);
    }

    if (available_right > c_dim.width) {
        pos += 'l';
        c_left = p_left;
    } else {
        pos += 'r';

        c_left = p_left - (c_dim.width - p_dim.width) + 20;
    }

    if (!$('tooltip_pointer').hasClassName(pos)) {
        $w($('tooltip_pointer').className).each(function(class_name) {
            $('tooltip_pointer').removeClassName(class_name);
        });
        $('tooltip_pointer').addClassName('pointer_' + pos);
    }

    $('tooltip_pointer').setStyle({left: p_left + 'px', top: p_top + 'px'});
    $('tooltip_content').setStyle({left: c_left + 'px', top: c_top + 'px'});
}



/* features lightbox code */
function getFeatures(name, lnk) {
    $$('a.features_links').each(function(element) {
        element.style.color = "#0066CC";
    });
    lnk.style.color = "#ef8f11";

    var url = domain_name + 'ajax/features_ajax_cms.php';

    new Ajax.Request('/WVonage/getContent?url=' + url, {
        parameters: {
            feature: name
        },
        method: 'post',
        onSuccess: function(transport) {
            var returned = transport.responseText.evalJSON(true);
            response = returned['content'].evalJSON(true);
            $('feature_content').innerHTML = response;
        }
    });
    return false;
}

function toggleLayer(howToUse) {
    var elem, vis;
    if (document.getElementById)
        elem = document.getElementById(howToUse);
    else if (document.all)
        elem = document.all[howToUse];
    else if (document.layers)
        elem = document.layers[howToUse];
    vis = elem.style;

    if (vis.display == '' && elem.offsetWidth != undefined && elem.offsetHeight != undefined)
        vis.display = (elem.offsetWidth != 0 && elem.offsetHeight != 0) ? 'block' : 'none';
    vis.display = (vis.display == '' || vis.display == 'block') ? 'none' : 'block';
}

/* .features lightbox code
 function highlight (action, id) {
 if (action=='on') {
 $('country_'+id).addClassName('highlight');
 } else {
 $('country_'+id).removeClassName('highlight');
 }
 }*/

function highlight(div_id) {
    if (current_question != undefined) {
        $('answer_' + current_question).removeClassName('highlight');
    }
    current_question = div_id;
    $('answer_' + div_id).addClassName('highlight');
}

var disable_dropdown = false;
var disable_reset = false;
var open_dropdown = '';
var country;
var currency = 'us';
var country_list = '';

function setCountryExtensions(country_name, flag_name) {
    country = country_name;
    $$('#country_dropdown_content').each(function(e) {
        $(e).innerHTML = '<div><img src="/images/rate_tool/flags/extensions/square_medium/' + flag_name + '" width="26" height="26" style="vertical-align:middle;"/> <span class="selected_country">' + country_name + '</span></div>';
    });
    toggleDropdown('hide_all');
    $('country_text').addClassName('hidden');
    $('search_results_container').addClassName('hidden');
}

function setCountry(id) {
    //disable_rest=true;
    country = id;
    getResults();
    toggleDropdown('hide_all');
}

function disableReset() {
    disable_reset = true;
    setTimeout("enableReset()", 500);
}

function enableReset() {
    disable_reset = false;
}


function currency_highlight(action, id) {
    if (action == 'on') {
        $('currency_' + id).addClassName('mouseover');
    } else {
        $('currency_' + id).removeClassName('mouseover');
    }
}

function clearSearch(all) {
    disable_dropdown = true;
    //toggleDropdown('hide','countries');
    //toggleDropdown('hide','search_results');
    toggleDropdown('hide_all');
    //if (!$('search_results_container').hasClassName('hidden')) {
    if (!disable_reset) {
        if ($('country_text').value == 'Search by Name or Country Code' || all == 'yes' || true) {
            $('country_text').value = '';
            //toggleDropdown('hide', 'search_results');
        }
        $('country_dropdown_content').innerHTML = '';
        $('country_text').removeClassName('hidden');
        $('country_text').select();
        $('country_text').removeClassName('placeholder');
    }
    setTimeout("disable_dropdown=false", 500);
    if (typeof(country) === 'undefined')
        country = '';
    country = '';
}

function toggleDropdown(action, id) {
    //$('search_results_container').addClassName('hidden');
    //console.log("@");
    try {
        if (action == 'hide_all') {
            if ($('countries_container'))
                $('countries_container').addClassName('hidden');
            if ($('currency_container'))
                $('currency_container').addClassName('hidden');
            if ($('search_results_container'))
                $('search_results_container').addClassName('hidden');
            open_dropdown = '';
        } else if (!disable_dropdown) {
            if (!$('search_results_container').hasClassName('hidden') && id == 'countries') {
                var first_letter = $F('country_text').substr(0, 1);
                countryJump(first_letter);
            }

            if (action == 'show') {
                if (open_dropdown != '')
                    $(open_dropdown + '_container').addClassName('hidden');
                $(id + '_container').removeClassName('hidden');
                open_dropdown = id;
            } else if (action == 'hide') {
                $(id + '_container').addClassName('hidden');
                open_dropdown = '';
            } else if ($(id + '_container').hasClassName('hidden')) {
                if (open_dropdown != '')
                    $(open_dropdown + '_container').addClassName('hidden');
                $(id + '_container').removeClassName('hidden');
                open_dropdown = id;
            } else {
                $(id + '_container').addClassName('hidden');
                open_dropdown = '';
            }
        }
    } catch (e) {
    }
}

var search_position = 0;
var search_results = 0;
function search_highlight(id) {
    search_position = id;
    for (var x = 0; x <= search_results; x++) {
        if (x != search_position) {
            $('search_' + x).removeClassName('search_mouseover');
        }
    }
    $('search_' + id).addClassName('search_mouseover');
}

function getPredictive(el, e) {
    if (el.value != '') {
        $('country_dropdown_content').innerHTML = '';
        toggleDropdown('hide', 'countries')
        toggleDropdown('hide', 'currency')
        toggleDropdown('show', 'search_results');
        $('country_text').removeClassName('hidden');

        var key_pressed = e.keyCode ? e.keyCode : e.charCode;

        if (key_pressed == '38') { //Up arrow
            search_position -= 1;
            if (search_position < 0)
                search_position = search_results;
            search_highlight(search_position)

        } else if (key_pressed == '40') { //Down arrow
            search_position += 1;
            if (search_position > search_results)
                search_position = 0;
            search_highlight(search_position)

        } else if (key_pressed == '13') { //Enter
            if (search_results != 'x') {
                setCountryExtensions($F('search_country_' + search_position), $F('search_flag_' + search_position));
                $('btn_find_plans').focus();
            }
        } else {

            var url = domain_name + 'includes/extensions/search_ajax_cms.php';
            new Ajax.Request('/WVonage/getContent?url=' + url, {
                parameters: {string: el.value,
                    countries: country_list},
                method: 'post',
                onSuccess: function(transport) {
                    var returned = transport.responseText.evalJSON(true);
                    response = eval(returned['content'].evalJSON(true));
                    if (response != undefined) {
                        $('search_results').innerHTML = response[1];
                        search_results = response[0];
                        search_position = 0;
                    }
                }
            });
        }
    } else {
        toggleDropdown('hide', 'search_results');
    }
}

function countryJump(letter) {
    letter = letter.toUpperCase();
    if (Object.isElement($('letter_header_' + letter))) {
        var main_top = document.getElementById('countries').offsetTop;
        var sub_top = document.getElementById('letter_header_' + letter).offsetTop;
        var offset = 0;
        var return_value = false;
        var user_agent = navigator.userAgent.toLowerCase();
        if (navigator.appName == 'Microsoft Internet Explorer') {
            offset = 35;
        } else if (user_agent.indexOf('webkit') >= 0) {
            offset = 35;
        } else if (user_agent.indexOf('firefox') >= 0) {
            offset = 35;
        }

        if (return_value == false) {
            document.getElementById('countries').scrollTop = sub_top - main_top + offset;
            return false;
        } else {
            return true;
        }
    }
}

function ie_version() {
    var rv = -1; // Return value assumes failure.
    if (navigator.appName == 'Microsoft Internet Explorer') {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null) {
            rv = parseFloat(RegExp.$1);
        }
    }
    return rv;
}

//document.observe('dom:loaded', function() {

Event.observe(document, 'click', function(e) {
    var hide_dropdown = true,
            t = e.target,
            dropdown_array = ['dropdown_country', 'dropdown_currency', 'planFinderBtn', 'planFinderBtnNav'];
    while (t) {
        if (dropdown_array.indexOf(t.id) >= 0) {
            hide_dropdown = false;
            break;
        }
        t = t.parentNode;
    }
    if (hide_dropdown) {
        toggleDropdown('hide_all');
    }
});
//});
/*
 function navigateDropdown () {
 console.log(event.keyCode);
 }

 Event.observe(document,'onkeypress', navigateDropdown);
 */

function openDotCom() {
    var qs = '';
    if (typeof(country) !== 'undefined' && country != '')
        qs = '?IPRcountry=' + country.replace('&', '%26');
    window.location.href = 'http://www.vonage.com/international-voip-rates/' + qs;
    return false;
}

function trim(stringToTrim) {
    return stringToTrim.replace(/^\s+|\s+$/g, "");
}
function ltrim(stringToTrim) {
    return stringToTrim.replace(/^\s+/, "");
}
function rtrim(stringToTrim) {
    return stringToTrim.replace(/\s+$/, "");
}



/* Vonage Access */
function search_by_form() {
    var temp = $F('location');
    //(temp=='location') ? search_form='location' : search_form='number';

    if (temp == 'location') {
        location_state = '';
        number_state = 'none';
    } else {

        location_state = 'none';
        number_state = '';
    }

    $('location_div').style.display = location_state;
    $('number_div').style.display = number_state;


    $('vaccess_container_div').innerHTML = '<div id="result1_div" class="vaccess_results"></div>	';

}

var div_count = 2;

function vaccess_country() {
    var response;
    var next_div;
    div_count = 1;
    var url = domain_name + 'ajax/vaccess_ajax_cms.php';
    new Ajax.Request('/WVonage/getContent?url=' + url, {
        parameters: {country: $F('country_select')},
        method: 'post',
        onSuccess: function(transport) {
            var returned = transport.responseText.evalJSON(true);
            response = returned['content'].evalJSON(true);
            next_div = '<div id="result2_div" class="vaccess_results"></div>';
            $('result1_div').innerHTML = response + next_div;
            div_count++;
        }
    });
}

function vaccess_states(country) {
    var next_div;
    var url = domain_name + 'ajax/vaccess_ajax_cms.php';
    new Ajax.Request('/WVonage/getContent?url=' + url, {
        parameters: {state: $F('state_select'),
            country: country},
        method: 'post',
        onSuccess: function(transport) {
            var returned = transport.responseText.evalJSON(true);
            response = returned['content'].evalJSON(true);
            next_div = '<div id="result3_div" class="vaccess_results"></div>';
            $('result2_div').innerHTML = response + next_div;
            div_count++;
        }
    });
}

function vaccess_npa(state, country) {
    var next_div;
    var url = domain_name + 'ajax/vaccess_ajax_cms.php';
    new Ajax.Request('/WVonage/getContent?url=' + url, {
        parameters: {npa: $F('npa_select'),
            country: country,
            state: state},
        method: 'post',
        onSuccess: function(transport) {
            var returned = transport.responseText.evalJSON(true);
            response = returned['content'].evalJSON(true);
            next_div = '<div id="result4_div" class="vaccess_results"></div>';
            $('result3_div').innerHTML = response + next_div;
            div_count++;
        }
    });
}

function vaccess_town(country, npa, state) {
    var next_div;
    var url = domain_name + 'ajax/vaccess_ajax_cms.php';
    new Ajax.Request('/WVonage/getContent?url=' + url, {
        parameters: {town: $F('town_select'),
            country: country,
            state: state,
            npa: npa},
        method: 'post',
        onSuccess: function(transport) {
            var returned = transport.responseText.evalJSON(true);
            response = returned['content'].evalJSON(true);
            //next_div='<div id="result4_div"></div>';
            $('result4_div').innerHTML = response;
            div_count++;
        }
    });
}

function vaccess_number() {
    var next_div;
    var url = domain_name + 'ajax/vaccess_ajax_cms.php';
    new Ajax.Request('/WVonage/getContent?url=' + url, {
        parameters: {number: $F('caller_number')},
        method: 'post',
        onSuccess: function(transport) {
            var returned = transport.responseText.evalJSON(true);
            response = returned['content'].evalJSON(true);
            next_div = '<div id="result4_div" class="vaccess_results"></div>';
            $('result1_div').innerHTML = response + next_div;
            div_count++;
        }
    });
}

/* Extensions Number */
function show_extensions_number(e) {
    if ($('extensions_access_number_div').hasClassName('hidden')) {
        $(e).replace($(e).innerHTML);
        $('extensions_access_number_div').removeClassName('hidden');
        var url = domain_name + 'ajax/extensions_access_number_ajax.php';
        new Ajax.Request('/WVonage/getContent?url=' + url, {
            onSuccess: function(transport) {
                var returned = transport.responseText.evalJSON(true);
                response = returned['content'].evalJSON(true);

                $('extensions_access_number_div').update('Your Vonage access number is <strong>' + response[0] + '</strong>');

                if (typeof dynamic_tracking != 'undefined') {
                    dynamic_tracking(response[1]);
                }
            }
        });
    }
}


function getCountryRate(country, plan_id) {
    if (country == '' || country == 'null' || plan_id == '') {
        return;
    }
    country = country.replace(/.*the\s/i, "");

    var url = domain_name + 'ajax/ipr_ajax.php?country=' + country;
    if (window.location.hostname.indexOf('vonagenetworks.net') == -1
            && window.location.hostname.indexOf('vonagelabs.com') == -1) {
        url = 'http://www.vonage.com/ajax/ipr_ajax.php?country=' + country;
    }

    new Ajax.Request('/WVonage/getContent?url=' + url, {
        parameters: {
            country: country, //corresponds to the appropriate content in the ajax file
            escapeFlag: 'true'
        },
        method: 'post',
        onSuccess: function(transport) {
            if (is_ie6 == true) {
                //shut off the select dropdown on the page for ie6 only
                $$('select').each(function(element) {
                    element.style.visibility = "hidden";
                });
            }
            //console.log("onSuccess response");
            var returned = transport.responseText.evalJSON(true);
            var content = returned['content'].evalJSON(true);
            //console.log("content: " + content);
            var Landline_rates = (((content['plans'])[plan_id])['Landline'])['rates'];
            //console.log("Landline_rates: " + Landline_rates);
            if ($('Landline_rates') && Landline_rates) {
                if (Landline_rates == "Included") {
                    Landline_rates = "UNLIMITED<sup>1</sup>";
                } else if (Landline_rates.indexOf('&cent;') != -1) {
                    Landline_rates = Landline_rates.replace(/&cent;/g, '&cent;/min.');
                }
                $('Landline_rates').innerHTML = Landline_rates;
            }
            var Cellular_rates = (((content['plans'])[plan_id])['Cellular'])['rates'];
            //console.log("Cellular_rates: " + Cellular_rates);
            if ($('Cellular_rates') && Cellular_rates) {
                if (Cellular_rates == "Included") {
                    Cellular_rates = "UNLIMITED<sup>1</sup>";
                    if ($('unlimited-mobile') != null) {
                        //console.log("$('unlimited-mobile'): " + $('unlimited-mobile').getAttribute('style'));
                        $('unlimited-mobile').removeAttribute('style');
                    }
                    if ($('unlimited-mobile1') != null) {
                        //console.log("$('unlimited-mobile1'): " + $('unlimited-mobile1').getAttribute('style'));
                        $('unlimited-mobile1').removeAttribute('style');
                    }
                } else if (Cellular_rates.indexOf('&cent;') != -1) {
                    Cellular_rates = Cellular_rates.replace(/&cent;/g, '&cent;/min.');
                }

                $('Cellular_rates').innerHTML = Cellular_rates;
            }
        }
    });
}

//START Features
var tooltip_content = {
    'anonymous_call_block': 'Stop being hassled by unknown callers. With Anonymous Call Block, all incoming calls from unknown callers are automatically transferred to an audio prompt informing them that you no longer receive calls from restricted phone numbers. Activate this feature from your Online Account.',
    'vonage_online_account': 'Your Vonage Online Account allows you to log in from any computer or your smartphone. From there, you can view your call and billing activity, and even set your preferences for the many great features that come with your Vonage calling plan.',
    'number_for_life': 'Most phone numbers can be transferred to Vonage. Our "Keep Your ExistingNumber" tool makes it easy to see if you can! ',
    'in_network_calling': 'Sign up and get unlimited Vonage-to-Vonage calls for free! Call any Vonage customer world wide as much as you want - it\'s included in every plan.',
    'international_and_directory_assistance_block': 'Have the option to block directory assistance and any international call. You\'ll never have to worry about random international charges again! Activate this feature from your Online Account.',
    'ring_lists': 'Direct your incoming calls to multiple Vonage lines. You can choose to have them forwarded one at a time, in random order or all at the same time! Activate this feature from your Online Account.',
    'call_hunt': 'Not at your desk? Call Hunt rings up to 5 Vonage lines in sequence until you answer and will go to the voicemail of your choice. Activate this feature from your Online Account. ',
    'call_return': 'Missed a call? Call Return tells you who called and when, and lets you return calls without even redialing their number. Activate this feature from your Online Account. ',
    'do_not_disturb': 'Need some quiet time? Have all your calls go straight to Vonage Voicemail Plus! Turn this feature on and off whenever you want - it won\'t affect your outgoing calls. Activate this feature from your Online Account. ',
    'repeat_dialing': 'Tired of busy signals? Repeat Dialing conveniently does the dialing for you, calling you back when the line is free. Simply press #5 when you get a busy signal.',
    'bandwidth_saver': 'Sluggish Internet connection? Optimize your voice quality with Bandwidth Saver so you always have great call quality. Activate this feature from your Online Account.',
    'network_availability_number': 'Worried about power or Internet outages? Setup an alternate Network Availability Number! It allows you to forward your calls to a number of your choice so you can always be reached, even in a power outage. Activate this feature from your Online Account.',
    'special_services_number': '<ul><li>211</li><ul><li>From childcare to laundry service, 211 connects you with the community services you\'re looking for.</li></ul><li>311</li><ul><li>Need the garbage pickup schedule? 311 connects you with your local public information center to help with community-related concerns.</li></ul><li>411</li><ul><li>Find restaurants, movie listings and more! 411 connects you with residential and business listings within the U.S., Canada and Puerto Rico.</li></ul><li>511</li><ul><li>Want the latest traffic report? 511 gets you timely travel information provided by the U.S. Department of Transportation.</li></ul><li>711</li><ul><li>Have a hearing or speech disability? 711 connects you to your state\'s Telecommunications Relay Services (TRS) so you can call any phone with a text phone (TTY).</li></ul><li>811</li><ul><li>Planning to install a new pool? Dial 811 before doing any outdoor project to find out where your city\'s underground utility lines are.</li></ul></ul>',
    'vonage_access_number': 'Let anyone call you for the price of a local call, no matter where they are! All they have to do is dial their local Vonage Access Number before your regular Vonage number.',
    'call_transfer': 'Transfer calls to any phone number in the U.S., Puerto Rico and Canada while you\'re on another call. Log in to your Vonage Online Account to find out more.',
    'call_waiting': 'See who\'s calling, even when you\'re on the other line! You can even take the call, or let it go to voicemail. Log in to your Vonage Online Account to find out more.',
    'three_way_calling': 'Add a third person to your phone conversation, anytime. Log in to your Vonage Online Account to find out more.',
    'caller_id_block': 'Call anybody you want while keeping your phone number private. Log in to your Vonage Online Account to find out more.'
}

function showInfo(e) {
    var position = Element.cumulativeOffset($(e));
    var dimensions = Element.getDimensions($(e));

    var posTop = (position['top'] + dimensions['height'] / 2 - 51);
    var posLeft = (position['left'] + dimensions['width'] + 15);

    //console.log($('lightbox_container').getStyle('top').replace('px','');

    if ($('lightbox_container') && !$('lightbox_container').hasClassName('hidden')) {
        posTop -= $('lightbox_container').getStyle('top').replace('px', '');
        posLeft -= 20;
    }

    $('feature_tooltip').setStyle({
        'top': posTop + 'px',
        'left': posLeft + 'px'
    });
    $('tooltip_mid').update(eval('tooltip_content.' + $(e).id));
    $('feature_tooltip').removeClassName('hidden');
}

function hideInfo() {
    $('feature_tooltip').addClassName('hidden');
}

function showFeature(feature, link) {
    $(feature).removeClassName('hidden');
    $$('.feature_main').each(function(e) {
        if (feature != e.id) {
            $(e).addClassName('hidden');
        }
    });

    $$('#feature_overview_controls a').each(function(e) {
        $(e).removeClassName('selected');
    });
    $(link).addClassName('selected');

    return false;
}

function is_touch_device() {
    try {
        document.createEvent("TouchEvent");
        return true;
    } catch (e) {
        return false;
    }
}

if (is_touch_device()) {
    Event.observe(window, 'load', function() {

        Event.observe(document, 'touchstart', function(e) {
            var t = e.target;
            if (!t.hasClassName('more_info')) {
                hideInfo();
            }
        });

        $$('.more_info').each(function(e) {
            try {
                $(e).addEventListener('touchstart', function() {
                    if ($('feature_tooltip').hasClassName('hidden')) {
                        showInfo(e);
                    } else {
                        hideInfo();
                    }
                }, false);
            } catch (err) {
            }
        });
    });
}
//END Feature
function getQueryVar(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) {
            return pair[1];
        }
    }
    return false;
}

function link_tracking(description) {
    //Temporarily removed until SiteCatalyst is implemented.
    /*
     var s=s_gi(account);
     s.linkTrackVars='None';
     s.linkTrackEvents='None';
     s.tl(true,'o',description);
     */
}



//Tools Lightbox - Start

function change_tool(tool_id) {
    $$('#tool_list a').each(function(e) {
        if (e.id == 'tool_link_' + tool_id) {
            e.addClassName('current_tool');
        } else {
            e.removeClassName('current_tool');
        }
    });
    $$('#tool_content > div').each(function(e) {
        if (e.id == 'tool_div_' + tool_id) {
            e.removeClassName('hidden');
        } else {
            e.addClassName('hidden');
        }
    });

    link_tracking('tools_' + tool_id);

    return false;
}

//Tools Lightbox - End

// Start Available Area Codes Tool Functions
function get_npa() {
    new Ajax.Request('/resources/php/ajax/available_area_codes_ajax.php', {
        parameters: {
            state: $F('state_select'),
            action: 'get_npa'
        },
        method: 'post',
        onSuccess: function(transport) {
            var response = transport.responseText;
            document.getElementById('npa_select_div').innerHTML = response;
        }
    });
}

function form_input_tab(this_field, next_field, length) {
    if (isNaN(this_field.value)) {
        this_field.value = this_field.value.substr(0, this_field.value.length - 1);
    } else if (this_field.value.length == length) {
        //alert (next_field);
        //$(next_field).focus();
        //$(next_field).select();
        document.getElementById(next_field).focus();
        try {
            document.getElementById(next_field).select();
        } catch (err) {
        }
    }
}

function sc_pagename(new_page_name) {
    try {
        s.pageName = new_page_name;
        void(s.t());
    } catch (err) {
    }
}

// Start Speed Test
function start_speed_test() {
    setCookie('speedtest', 'true', 20);
    var speedtestURL = "http://speedtest.vonage.com/";
    window.open(speedtestURL, "_blank", "toolbar=yes, location=yes, directories=yes, status=yes, menubar=yes, scrollbars=yes, resizable=yes, copyhistory=yes, width=1000, height=700");
}

function setCookie(c_name, value, expireseconds) {
    var exdate = new Date();
    exdate.setTime(exdate.getTime() + (expireseconds * 1000));

    var cookie_domain = '';
    if (document.location.host.indexOf('.vonage.com') >= 0) {
        cookie_domain = ' domain=.vonage.com;';
    }
    //console.log(cooke_domain);
    document.cookie = c_name + '=' + escape(value) + ((expireseconds == null) ? '' : ';expires=' + exdate.toGMTString()) + '; path=/;' + cookie_domain;
}

function getCookie(c_name) {
    if (document.cookie.length > 0) {
        var c_start = document.cookie.indexOf(c_name + "=");
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1;
            var c_end = document.cookie.indexOf(";", c_start);
            if (c_end == -1)
                c_end = document.cookie.length;
            return unescape(document.cookie.substring(c_start, c_end));
        }
    }
    return "";
}
// End Speed Test

//Rates Popup functions
function openCityRates(country, div, plans, countryCellularRates) {
    //close row
    if ($(div + "_plusminus").hasClassName("minus")) {
        $(div + "_plusminus").removeClassName("minus").addClassName("plus").src = "/resources/vonage-us/images/common/vonage_plus.gif";
        $$('.' + div + 'Rates').each(function(x) {
            x.remove();
        });
        return false;
    }

    $(div).insert({after: '<tr id="' + div + 'Wait" class="rateTableRates"><td colspan="5"><img src="/resources/vonage-us/images/common/ajax_loader_orange.gif" /></td></tr>'});
    for (var j = 0; j < countryList.length; j++)
    {
        if (country == countryList[j])
        {
            $(div + 'Wait').remove();
            var cityContent = '<tr class="' + div + 'Rates rateTableRates"><td class="description"><span>' + country + '</span></td><td>' + planRate + '</td><td>' + planRate + '</td></tr>';
            $(div).insert({after: cityContent});
            $(div + "_plusminus").removeClassName("plus").addClassName("minus").src = "/resources/vonage-us/images/common/vonage_minus.gif";
            return false;
        }
    }

    new Ajax.Request('/VUSCS/ipdl/data/cityplans/' + country, {
        parameters: {codes: plans, format: 'json'},
        method: 'get',
        onSuccess: function(transport) {
            $(div + 'Wait').remove();
            var returned = transport.responseText.evalJSON(true);
            var cityContent = '';
            var ratesJsonObj = {};
            ratesJsonObj = plansData(returned, 'City');


            for (var cityName in ratesJsonObj)
            {
                var landlineRates, cellularRates;
                if (ratesJsonObj[cityName]['Special'])
                {
                    landlineRates = ratesJsonObj[cityName]['Special'][1];
                    if (cityName != '')
                    {
                        cityName = ' - ' + cityName;
                    }

                    cityContent = cityContent + '<tr class="' + div + 'Rates rateTableRates"><td class="description"><span>' + country + cityName + '</span></td><td colspan="2">' + landlineRates + '</td></tr>';
                }
                else
                {
                    if (ratesJsonObj[cityName]['Cellular'])
                    {
                        cellularRates = ratesJsonObj[cityName]['Cellular'][1];
                    }
                    else
                    {
                        if (cityName !=='' && ratesJsonObj['']['Cellular'])
                        {
	                        cellularRates = ratesJsonObj['']['Cellular'][1];
						}
						else
						{
							cellularRates =ratesJsonObj['']['Landline'][1];
						}
                    }
                    if (ratesJsonObj[cityName]['Landline'])
                    {
                        landlineRates = ratesJsonObj[cityName]['Landline'][1];
                    }
                    else
                    {
                        landlineRates = 'N/A';
                    }
                    if (cityName != '')
                    {
                        cityName = ' - ' + cityName;
                    }
                    cityContent = cityContent + '<tr class="' + div + 'Rates rateTableRates"><td class="description"><span>' + country + cityName + '</span></td><td>' + landlineRates + '</td><td>' + cellularRates + '</td></tr>';
                }
            }
            $(div).insert({after: cityContent});
        },
        onFailure: function() {
            $(div + "Wait").remove();
           $(div + "_plusminus").removeClassName("minus").addClassName("plus").src = "/resources/vonage-us/images/common/vonage_plus.gif";
            //throw unavailable alert
            alert('Sorry, the service is unavailable at the moment.  Please try again later.');
        }
    });
     $(div + "_plusminus").removeClassName("plus").addClassName("minus").src = "/resources/vonage-us/images/common/vonage_minus.gif";
    return false;
}


function plansData(returned, type)
{
    var ratesJsonObj = {};
    for (var i = 0; i < returned.genericDataList[0].resultArr.length; i++)
    {
        var cityName = returned.genericDataList[0].resultArr[i][1];
        var planCode = returned.genericDataList[0].resultArr[i][0];
        var planType = returned.genericDataList[0].resultArr[i][2];
        var maxRate = returned.genericDataList[0].resultArr[i][3];
        var minRate = returned.genericDataList[0].resultArr[i][4];
        var planTypeJSON = {};

        if (minRate != maxRate)
        {
            minRate = currencyPrice(minRate) + " - " + currencyPrice(maxRate);
        }
        else if (minRate == 0)
        {
            minRate = "Included";
        }
        else
        {
            minRate = currencyPrice(minRate);
        }

        if (bucketValue != '' && rateValue == '' && planCode == cardValue)
        {
            minRate = bucketRate(minRate);
        }


        if (ratesJsonObj[cityName])
        {
            planTypeJSON = ratesJsonObj[cityName];
            if (!(planTypeJSON[planType]))
            {
                planTypeJSON[planType] = [planCode, minRate];
                ratesJsonObj[cityName] = planTypeJSON;
            }
        }
        else
        {
            planTypeJSON[planType] = [planCode, minRate];
            ratesJsonObj[cityName] = planTypeJSON;
        }
        if (type == 'Country')
        {

            if (planCode == '999' && planType == 'Landline' && !(ratesJsonObj[cityName]['Cellular']))
            {
                ratesJsonObj[cityName]['Cellular'] = ratesJsonObj[cityName]['Landline'];
            }

            if (cityName == 'Cameroon')
            {
				ratesJsonObj = addSpecialCountries(0,ratesJsonObj);
			}
			else if(cityName == 'Portugal')
			{
				ratesJsonObj = addSpecialCountries(1,ratesJsonObj);
			}
			else if(cityName == 'United Kingdom')
			{
				ratesJsonObj = addSpecialCountries(2,ratesJsonObj);
			}
        }

    }

    return ratesJsonObj;
}

function addSpecialCountries(i,ratesJsonObj)
{

		if (bucketValue != '' && rateValue != '')
		{
			planRate = bucketRate(rateValue);
		}
		else
		{
			planRate = 'Included';
		}
		var cityName = countryList[i];
		ratesJsonObj[cityName] = {};
		ratesJsonObj[cityName]['Cellular'] = ['999', planRate];
		ratesJsonObj[cityName]['Landline'] = ['999', planRate];
		return ratesJsonObj;
}

function currencyPrice(price)
{
    price = Number(price);
    if (price < 1)
    {
        price = Math.round((price * 100) * 10) / 10 + '&cent;';
    }
    else
    {
        price = '$' + price.toFixed(2);
    }
    return price;
}

// LNP - Start
	function checkPortability() {
		$('lnp_tool_loading').show();
		$('lnp-message').update('');
		$('lnp_error').update('');
		new Ajax.Request ('/resources/php/ajax/lnp_ajax.php', {
			parameters:	{
				npa: $F('num-1'),
				nxx: $F('num-2'),
				xxxx: $F('num-3'),
				request: 'isPortable'
			},
			method: 'post',
			onComplete: function(){
				$('lnp_tool_loading').hide();
			},
			onSuccess: function(transport){
				obj=transport.responseText.evalJSON();
				if (obj.error!=undefined) {
					$('lnp_error').show('');
					$('lnp_error').update(obj.error);
				} else {
					if (obj.isValidForPortability) {
						$('lnp-message').update('<div style="width:237px;"><div style="float:left; width:30px; margin-right:10px; margin-left:10px;"><img width="30" height="30" alt="Check mark icon" src="/resources/vonage-us/images/toolbox/round_check.png"></div><div style="float:left; width:160px; text-align:left; font-weight:bold; font-size:13px;"><div style="color:#f38401;">Great News:</div><div style="color:#555555;">Your number\'s a keeper.</div><div style="margin:10px 0 0 20px;"></div></div><div class="clear"></div></div>');
					} else {
						$('lnp-message').update('<div style="text-align:center; margin-left:8px;">Unfortunately, we can\'t transfer that number to our service. How about we give you a brand new one?<br><a href="javascript:show_lnp_form(\'accordion\');">Try another number</a></div>');
					}
					$('lnp-message').show();
					$('lnp_accordion_form').hide();
				}

        },
        onFailure: function() {
            $('lnp_error').show('');
            $('lnp_error').update('We encountered a problem. Please try again.');
        }
    });

    return false;
}

function show_lnp_form() {
    $('lnp_accordion_form').show();
    $('lnp-message').update('');
    $('lnp_error').update('');
    $('lnp-message').hide();
    $('lnp_error').hide();
}
// LNP - End

// Area Code Lookup - Start
function getNpa() {
    clearAreaCodeList('#npa_select option');
    $('npa_select').writeAttribute('disabled', 'disabled');
    $('available-message').hide();
    $('city_list_results').hide();
    $('state_select').addClassName('loading');
    $('npa_select').removeClassName('loading');
    new Ajax.Request('/resources/php/ajax/lnp_ajax.php', {
        parameters: {
            request: 'getAvailableAreaCodes',
            state: $F('state_select')
        },
        method: 'post',
        onComplete: function() {
            $('state_select').removeClassName('loading');
        },
        onSuccess: function(transport) {
            obj = transport.responseText.evalJSON();
            if (obj.error != undefined) {
                $('area_code_error').update(obj.error);
                clearAreaCodeList('#npa_select option');
            } else {
                $('area_code_error').update('');
                $('npa_select').writeAttribute('disabled', null);
                $(obj).each(function(e) {
                    $('npa_select').insert('<option value="' + $(e).npa + '">' + $(e).npa + '</option>');
                });
                $('npa_select')[0].selected = true;

            }
        },
        onFailure: function() {
            $('area_code_error').update('We encountered a problem. Please try again.');
        }
    });

    return false;
}

function clearAreaCodeList(e) {
    try {
        $$(e).each(function(s) {
            if ($(s).value != '') {
                $(s).remove();
            }
        });
    } catch (e) {

    }
}

function getLocations() {
    $('state_select').removeClassName('loading');
    $('npa_select').addClassName('loading');
    $('city_list_results').update('');
    new Ajax.Request('/resources/php/ajax/lnp_ajax.php', {
        parameters: {
            request: 'getAvailableAreaCodes',
            state: $F('state_select'),
            npa: $F('npa_select')
        },
        method: 'post',
        onComplete: function() {
            $('npa_select').removeClassName('loading');
        },
        onSuccess: function(transport) {
            obj = transport.responseText.evalJSON();
            $('area_code_error').update('');
            $('city_list_results').update(obj.join('<br/>'));
            $('available-message').update($F('npa_select') + ' is available in the following towns');
            $('city_list_results').show();
            $('available-message').show();
        },
        onFailure: function() {
            $('lnp_error').update('We encountered a problem. Please try again.');
        }
    });

    return false;
}
// Area Code Lookup - End

function bucketRate(currentRate)
{
    currentRate = '<p class="overage">Included <br />up to ' + bucketValue + ' minutes <br />then ' + currentRate + '/min</p>';
    return currentRate;
}

function showInfo (e) {
	var position=Element.cumulativeOffset($(e));
	var dimensions=Element.getDimensions($(e));
	$('feature_tooltip').setStyle({
		'top':(position['top']+dimensions['height']/2-51)+'px',
		'left':(position['left']+dimensions['width']+15)+'px'
	});
	$('tooltip_mid').update($(e).getAttribute('featureDescription'));
	$('feature_tooltip').removeClassName('hidden');
}

function hideInfo () {
	$('feature_tooltip').addClassName('hidden');
}

function showFeature (feature, link) {
	$(feature).removeClassName('hidden');
	$$('.feature_main').each(function(e){
		if (feature!=e.id) {
			$(e).addClassName('hidden');
		}
	});

	$$('#feature_overview_controls > a').each(function(e){
		$(e).removeClassName('selected');
	});
	$(link).addClassName('selected');

	return false;
}

function is_touch_device() {
	try {
		document.createEvent("TouchEvent");
		return true;
	} catch (e) {
		return false;
	}
}

Event.observe(window, 'load', function() {

	var tooltipContainer=document.createElement('div');
	tooltipContainer.setAttribute('id', 'feature_tooltip');
	tooltipContainer.setAttribute('class', 'hidden');

	var tooltipContent;
	tooltipContent ='<div class="tooltip_top">&nbsp;</div>';
	tooltipContent+='<div id="tooltip_mid"></div>';
	tooltipContent+='<div class="tooltip_bottom">&nbsp;</div>';
	tooltipContent+='<div class="tooltip_pointer">&nbsp;</div>';

	document.getElementsByTagName('body')[0].appendChild(tooltipContainer);

	$(tooltipContainer).update(tooltipContent);

	if (is_touch_device()) {

			Event.observe(document, 'touchstart', function(e){
				var t=e.target;
				if (!t.hasClassName('more_info')) {
					hideInfo();
				}
			});

			$$('.more_info').each(function(e) {
				try {
					$(e).addEventListener('touchstart', function(){
						if ($('feature_tooltip').hasClassName('hidden')) {
							showInfo(e);
						} else {
							hideInfo();
						}
					}, false);
				} catch(err){}
			});
	}
});

function disable_lightbox_links(){
	$('lightbox_container').select('a.lightbox-link').each(function(node){
		if(node.innerHTML != 'ORDER NOW')	{
			node.setAttribute("onclick", "return false;");
			if (node.innerHTML.indexOf('Money-Back Guarantee')>=0) {
				node.setAttribute("id", "mbgLink");
				node.setStyle({'color':'inherit !important'});

				create_mbg_lightbox();

				node.on('mouseover', show_mbg_tooltip);
				node.on('mouseout', hide_mbg_tooltip);
			}
		}
	});
}

function position_mbg_tooltip() {
	var linkDim=$('mbgLink').getDimensions();
	var linkPos=$('mbgLink').cumulativeOffset();
	var tooltipHeight=$('tooltip_mbg').getHeight();
	var pointerDim=$('tooltip_pointer').getDimensions();

	$('tooltip_mbg').setStyle({
		'left':(linkPos.left+linkDim.width+pointerDim.width+10)+'px',
		'top':((linkPos.top+(linkDim.height/2))-(tooltipHeight/2))+'px'
	});

	$('tooltip_pointer').setStyle({'top':((tooltipHeight/2)-(pointerDim.height)/2)+'px'});
}

function create_mbg_lightbox() {
	var tooltip=document.createElement('div');
	$(tooltip).setAttribute('id', 'tooltip_mbg');
	$(tooltip).setStyle({'display':'none'});
	$(tooltip).insert('<p class="tooltip_mbg_title">Get peace of mind</p>');
	$(tooltip).insert('<p class="tooltip_mbg_subtitle">with our 30-Day Money-Back Guarantee offer!</p>');
	$(tooltip).insert('<p class="tooltip_mbg_copy">We offer a 30-day Money Back Guarantee ("MBG") if you terminate your service within 30 days from the date of your order. Your Order Date is the date you order service or the date we successfully process your payment, whichever is later. The MBG applies to the first line only and covers the monthly charge, taxes and any shipping or activation fees. Additional lines are not eligible.</p>');
	$(tooltip).insert('<p class="tooltip_mbg_copy">Our MBG does not cover any charges for international calls or usage outside of your service plan, calls to Vonage toll free numbers, any monthly charge for Extensions&trade; and features or services not expressly included in your monthly plan fee. In addition, not all of the taxes that you paid may be refundable.</p>');
	$(tooltip).insert('<p class="tooltip_mbg_copy">If you purchased your adapter from a retail provider, you can return your adapter only to that retailer pursuant to their return policy. Vonage does not refund the price of adapters purchased from retail providers under its Money Back Guarantee.</p>');

	var tooltip_pointer=document.createElement('img');
	$(tooltip_pointer).setAttribute('id','tooltip_pointer');
	$(tooltip_pointer).setAttribute('alt','tooltip_pointer');
	$(tooltip_pointer).setAttribute('src','/resources/vonage-us/images/calling_plans/tooltip_pointer.png');

	tooltip.insert(tooltip_pointer);

	document.getElementsByTagName('body')[0].insert(tooltip);
}

function show_mbg_tooltip() {
	$('tooltip_mbg').show();
	position_mbg_tooltip();
}

function hide_mbg_tooltip() {
	$('tooltip_mbg').hide();
}



icr = {
    ratesDelay : 1,
    currentRateLookup : '',
    trueRateLookup : '',
    load : function () {
        $('icrRateForm').on("submit",icr.ratesSubmit);
        //$('searchBtn').on("click",function(){icr.ratesSearch});
        $('searchCountries').removeAttribute('onfocus');
        $('searchCountries').on('keyup',icr.ratesSearch);
        //bind all events
    },
    openRates: function (country,div,plan) {
        var plusminus = country + "_plusminus",
            plus = "/resources/vonage-us/images/common/vonage_plus.gif",
            minus = "/resources/vonage-us/images/common/vonage_minus.gif";
        if ($(country + "Link").hasClassName('active')) {
            //rate already open
            //update image to close
            $(country + "_plusminus").src = plus;
            $$("." + div + "Rates").each(function (x) {
                x.remove();
            });
        } else {
            //open rates
            var wait = new Element("img",{src:"/resources/vonage-us/images/common/ajax_loader_orange.gif"}),
                    wait_name = div + "Wait";
            wait = wait.wrap("td",{colspan:"5"});
            wait = wait.wrap("tr",{id:wait_name,"class":"rateTableRates"});
            $(div).insert(wait);
            new Ajax.Request('/resources/php/includes/calling_plans/icr/icr.php', {
                parameters:{country:country, plan:plan, div:div},
                method:"post",
                onSuccess: function (t) {
                    $(wait_name).remove();
                    $(plusminus).src = minus;
                    $(div).insert({after:transport.responseText});
                },
                onFailure: function () {
                    $(wait_name).remove();
                    $(plusminus).src = plus;
                    //display alert
                    alert("Sorry, the service is unavailable.");
                }
            });
        }
        return false;
    },
    ratesJump: function (country) {
        //console.log(country);
        if ($(country)) {
            var main_top = $('rateTableDiv').offsetTop,
                sub_top = $(country).offsetTop,
                offset = -1;
            if (Prototype.Browser.WebKit) {
                offset = 0;
            }
            $("rateTableDiv").scrollTop = sub_top - main_top + offset;
        }
        return false;
    },
    ratesSubmit: function(event, element) {
        Event.stop(event);
        $('searchCountries').value = icr.trueRateLookup;
        var div_name = icr.trueRateLookup.replace(/[^a-zA-Z\-]/g, "");
        icr.ratesJump(div_name);
        $(div_name + "Link").click();
        return false;
    },
    ratesSearch : function (event,element) {
        Event.stop(event);
        var keyCode = event.keyCode ? event.keyCode : event.charCode,
        patt = /[^a-zA-Z\s\-\(\)]/g;
        if (Event.KEY_ESC === keyCode) {
            return false;
        } else if (Event.KEY_TAB === keyCode) {
            $('searchCountries').value = $F('searchSuggestion');
            return false;
        }
        $('searchSuggestion').clear();
        $('searchCountries').value = $('searchCountries').value.replace(patt,'');

        var country = $F('searchCountries');
        country = country.capitalize();

        if ((icr.currentRateLookup == country) && (keyCode)) {
            return false;
        } else {
            icr.currentRateLookup = country;
        }

        if (!country.empty()) {
            //search
            countries_list.each(function(x) {
                if (x.startsWith(country)) {
                    $('searchCountries').value = country;
                    $('searchSuggestion').value = x;
                    icr.trueRateLookup = x;
                    throw $break;
                }
            });
        }

        return false;
    }
};

function ratesLightboxEvent() {
    icr.load();
}


//Open a lightbox on page load if lbid is present in the querystring
document.observe("dom:loaded", function() {
	if (lbid=getQueryVar('lbid')) {
		if (lbid.indexOf('.xml')>=0) {
			lbid='/templatedata/vonage_us/lightbox/data/'+lbid;
		} else {
			lbid='/templatedata/vonage_us/lightbox/data/'+lbid+'.xml';
		}

		//override for existing links with the international shipping lbid
		if (lbid=='/templatedata/vonage_us/lightbox/data/world_lp_countries.xml') lbid='/templatedata/vonage_us/lightbox/data/international_shipping.xml';

		popUp(lbid);
	}
});
