var lowestRateInfoViewModel = new LowestRateInfoViewModel();
$(document).ready(function () {

    //var jsonData = [];
    //var cities = '<a><i class="magic_1"><p>Afghanistan(+93)</p></i></a>,<a><i class="magic_2"><p>Albania (+355)</p></i></a>,<a><i class="magic_3"><p>Algeria (+213)</p></i></a>,<a><i class="magic_4"><p>American Samoa(+684)</p></i></a>,<a><i class="magic_5"><p>Andorra (+376)</p></i></a>,<a><i class="magic_6"><p>Angola (+244)</p></i></a>,<a><i class="magic_7"><p>Anguilla (+1264)</p></i></a>,<a><i class="magic_8"><p>Antarctica (+672)</p></i></a>,<a><i class="magic_9"><p>Antigua &amp; Barbuda(+1-268)</p></i></a>,<a><i class="magic_10"><p>Arab League</p></i></a>,<a><i class="magic_11"><p>argentina (+54)</p></i></a>,<a><i class="magic_12"><p>Armenia (+374)</p></i></a>,<a><i class="magic_13"><p>Aruba (+297)</p></i></a>,<a><i class="magic_14"><p>Australia (+61)</p></i></a>,<a><i class="magic_15"><p>Austria (+43)</p></i></a>,<a><i class="magic_16"><p>Azerbaijan (+994)</p></i></a>,<a><i class="magic_17"><p>Ascension island (+247)</p></i></a>,<a><i class="magic_18"><p>Bahamas (+1-242)</p></i></a>,<a><i class="magic_19"><p>Bahrain (+973)</p></i></a>,<a><i class="magic_20"><p>Bangladesh (+880)</p></i></a>,<a><i class="magic_21"><p>Barbados (+1-246)</p></i></a>,<a><i class="magic_22"><p>Belarus (+375)</p></i></a>,<a><i class="magic_23"><p>Belgium (+32)</p></i></a>,<a><i class="magic_24"><p>Belize (+501)</p></i></a>,<a><i class="magic_25"><p>Benin(+229)</p></i></a>,<a><i class="magic_26"><p>Bermuda (+1441)</p></i></a>,<a><i class="magic_27"><p>Bolivia (+591)</p></i></a>,<a><i class="magic_28"><p>Bosnia & Herzegovina.png (+387)</p></i></a>,<a><i class="magic_29"><p>Botswana (+267)</p></i></a>,<a><i class="magic_30"><p>Brazil (+55)</p></i></a>,<a><i class="magic_31"><p>Brunei (+673)</p></i></a>,<a><i class="magic_32"><p>Bulgaria (+359)</p></i></a>,<a><i class="magic_33"><p>Burkina Faso (+226)</p></i></a>,<a><i class="magic_34"><p>Burundi (+257)</p></i></a>,<a><i class="magic_35"><p>Bhutan (+975)</p></i></a>,<a><i class="magic_36"><p>Cambodja (+855)</p></i></a>,<a><i class="magic_37"><p>Cameroon (+237)</p></i></a>,<a><i class="magic_38"><p>Canada(+1)</p></i></a>,<a><i class="magic_39"><p>Cape Verde(+238)</p></i></a>,<a><i class="magic_40"><p>Cayman Islands(+1-345)</p></i></a>,<a><i class="magic_41"><p>Central African Republic (+236)</p></i></a>,<a><i class="magic_42"><p>Chad (+235)</p></i></a>,<a><i class="magic_43"><p>Chile (+56)</p></i></a>,<a><i class="magic_44"><p>China(+86)</p></i></a>,<a><i class="magic_45"><p>Colombia(+57)</p></i></a>,<a><i class="magic_46"><p>Comoros(+269)</p></i></a>,<a><i class="magic_47"><p>Congo Brazzaville(+242)</p></i></a>,<a><i class="magic_48"><p>Congo-Kinshasa(+237)</p></i></a>,<a><i class="magic_49"><p>Cook Islands(+682)</p></i></a>,<a><i class="magic_50"><p>Costa Rica(+237)</p></i></a>,<a><i class="magic_51"><p>Cote dIvoire</p></i></a>,<a><i class="magic_52"><p>Croatia(+385)</p></i></a>,<a><i class="magic_53"><p>Cuba (+53)</p></i></a>,<a><i class="magic_54"><p>Cyprus (+357)</p></i></a>,<a><i class="magic_55"><p>Czech Republic (+420)</p></i></a>,<a><i class="magic_56"><p>Denmark (+45)</p></i></a>,<a><i class="magic_57"><p>Djibouti (+253)</p></i></a>,<a><i class="magic_58"><p>Dominica(+1-767)</p></i></a>,<a><i class="magic_59"><p>Dominican Republic (+809)</p></i></a>,<a><i class="magic_60"><p>Ecuador (+593)</p></i></a>,<a><i class="magic_61"><p>Egypt (+20)</p></i></a>,<a><i class="magic_62"><p>El Salvador(+503)</p></i></a>,<a><i class="magic_63"><p>England (+44)</p></i></a>,<a><i class="magic_64"><p>Equatorial Guinea (+240)</p></i></a>,<a><i class="magic_65"><p>Eritrea (+291)</p></i></a>,<a><i class="magic_66"><p>Estonia (+372)</p></i></a>,<a><i class="magic_67"><p>Ethiopia (+251)</p></i></a>,<a><i class="magic_68"><p>European Union</p></i></a>,<a><i class="magic_69"><p>Faroes (+298)</p></i></a>,<a><i class="magic_70"><p>Fiji (+679)</p></i></a>,<a><i class="magic_71"><p>Finland (+358)</p></i></a>,<a><i class="magic_72"><p>France (+33)</p></i></a>,<a><i class="magic_73"><p>Gabon (+241)</p></i></a>,<a><i class="magic_74"><p>Gambia (+220)</p></i></a>,<a><i class="magic_75"><p>Georgia (+995)</p></i></a>,<a><i class="magic_76"><p>Germany (+49)</p></i></a>,<a><i class="magic_77"><p>Ghana(+233)</p></i></a>,<a><i class="magic_78"><p>Gibraltar (+350)</p></i></a>,<a><i class="magic_79"><p>Greece (+30)</p></i></a>,<a><i class="magic_80"><p>Greenland (+299)</p></i></a>,<a><i class="magic_81"><p>Grenada(+1-473)</p></i></a>,<a><i class="magic_82"><p>Guadeloupe (+590)</p></i></a>,<a><i class="magic_83"><p>Guademala (+502)</p></i></a>,<a><i class="magic_84"><p>Guam (+1-671)</p></i></a>,<a><i class="magic_85"><p>Guinea (+224)</p></i></a>,<a><i class="magic_86"><p>guinea-bissau (+245)</p></i></a>,<a><i class="magic_87"><p>Guyana (+509)</p></i></a>,<a><i class="magic_88"><p>Haiti (+509)</p></i></a>,<a><i class="magic_89"><p>Honduras (+504)</p></i></a>,<a><i class="magic_90"><p>Hong Kong (+852)</p></i></a>,<a><i class="magic_91"><p>Hungary (+36)</p></i></a>,<a><i class="magic_92"><p>Iceland (+354)</p></i></a>,<a><i class="magic_93"><p>India (+91)</p></i></a>,<a><i class="magic_94"><p>Indonesia (+62)</p></i></a>,<a><i class="magic_95"><p>Iran (+98)</p></i></a>,<a><i class="magic_96"><p>Iraq (+964)</p></i></a>,<a><i class="magic_97"><p>Ireland (+353)</p></i></a>,<a><i class="magic_98"><p>Isle of Man</p></i></a>,<a><i class="magic_99"><p>Israel (+972)</p></i></a>,<a><i class="magic_100"><p>Italy (+39)</p></i></a>,<a><i class="magic_101"><p>Jamaica (+1-876)</p></i></a>,<a><i class="magic_102"><p>Japan (+81)</p></i></a>,<a><i class="magic_103"><p>Jordan (+962)</p></i></a>,<a><i class="magic_104"><p>Kazakhstan (+7)</p></i></a>,<a><i class="magic_105"><p>Kenya (+254)</p></i></a>,<a><i class="magic_106"><p>Kiribati (+686)</p></i></a>,<a><i class="magic_107"><p>Kuwait (+965)</p></i></a>,<a><i class="magic_108"><p>Kyrgyzstan (+996)</p></i></a>,<a><i class="magic_109"><p>Kosovo (+381)</p></i></a>,<a><i class="magic_110"><p>Laos (+856)</p></i></a>,<a><i class="magic_111"><p>Latvia (+371)</p></i></a>,<a><i class="magic_112"><p>Lebanon (+961)</p></i></a>,<a><i class="magic_113"><p>Lesotho (+266)</p></i></a>,<a><i class="magic_114"><p>Liberia (+231)</p></i></a>,<a><i class="magic_115"><p>Libya (+218)</p></i></a>,<a><i class="magic_116"><p>Liechtenstein (+423)</p></i></a>,<a><i class="magic_117"><p>Lithuania (+370)</p></i></a>,<a><i class="magic_118"><p>Luxembourg (+352)</p></i></a>,<a><i class="magic_119"><p>Macao (+853)</p></i></a>,<a><i class="magic_120"><p>Macedonia (+389)</p></i></a>,<a><i class="magic_121"><p>Madagascar (+261)</p></i></a>,<a><i class="magic_122"><p>Malawi (+265)</p></i></a>,<a><i class="magic_123"><p>Malaysia (+60)</p></i></a>,<a><i class="magic_124"><p>Maldives(+960)</p></i></a>,<a><i class="magic_125"><p>Mali (+223)</p></i></a>,<a><i class="magic_126"><p>Malta (+356)</p></i></a>,<a><i class="magic_127"><p>Marshall Islands (+692)</p></i></a>,<a><i class="magic_128"><p>Martinique (+596)</p></i></a>,<a><i class="magic_129"><p>Mauritania (+222)</p></i></a>,<a><i class="magic_130"><p>Mauritius (+230)</p></i></a>,<a><i class="magic_131"><p>Mexico (+52)</p></i></a>,<a><i class="magic_132"><p>Micronesia (+691)</p></i></a>,<a><i class="magic_133"><p>Moldova(+373)</p></i></a>,<a><i class="magic_134"><p>Monaco (+377)</p></i></a>,<a><i class="magic_135"><p>Mongolia (+976)</p></i></a>,<a><i class="magic_136"><p>Montenegro (+382)</p></i></a>,<a><i class="magic_137"><p>Montserrat (+1-664)</p></i></a>,<a><i class="magic_138"><p>Morocco (+212)</p></i></a>,<a><i class="magic_139"><p>Mozambique (+258)</p></i></a>,<a><i class="magic_140"><p>Myanmar(Burma) (+95)</p></i></a>,<a><i class="magic_141"><p>Namibia (+264)</p></i></a>,<a><i class="magic_142"><p>Nauru(+674)</p></i></a>,<a><i class="magic_143"><p>Nepal (+977)</p></i></a>,<a><i class="magic_144"><p>Netherlands Antilles (+599)</p></i></a>,<a><i class="magic_145"><p>Netherlands (+31)</p></i></a>,<a><i class="magic_146"><p>New Caledonia (+687)</p></i></a>,<a><i class="magic_147"><p>New Zealand (+64)</p></i></a>,<a><i class="magic_148"><p>Nicaragua (+505)</p></i></a>,<a><i class="magic_149"><p>Niger (+227)</p></i></a>,<a><i class="magic_150"><p>nigeria (+234)</p></i></a>,<a><i class="magic_151"><p>norway (+47)</p></i></a>,<a><i class="magic_152"><p>North korea (+850)</p></i></a>,<a><i class="magic_153"><p>Oman (+968)</p></i></a>,<a><i class="magic_154"><p>Pakistan (+92)</p></i></a>,<a><i class="magic_155"><p>Palau (+680)</p></i></a>,<a><i class="magic_156"><p>Panama (+507)</p></i></a>,<a><i class="magic_157"><p>papua-new-guinea (+961)</p></i></a>,<a><i class="magic_158"><p>Paraguay (+595)</p></i></a>,<a><i class="magic_159"><p>Peru (+51)</p></i></a>,<a><i class="magic_160"><p>Philippines (+63)</p></i></a>,<a><i class="magic_161"><p>Portugal (+351)</p></i></a>,<a><i class="magic_162"><p>Puerto Rico (+1-787)</p></i></a>,<a><i class="magic_163"><p>Poland (+48)</p></i></a>,<a><i class="magic_164"><p>Qatar (+974)</p></i></a>,<a><i class="magic_165"><p>Reunion (+262)</p></i></a>,<a><i class="magic_166"><p>Romania (+40)</p></i></a>,<a><i class="magic_167"><p>Russia (+7)</p></i></a>,<a><i class="magic_168"><p>Rwanda (+250)</p></i></a>,<a><i class="magic_169"><p>Saint Lucia (+1-758)</p></i></a>,<a><i class="magic_170"><p>Samoa (+684)</p></i></a>,<a><i class="magic_171"><p>San Marino (+378)</p></i></a>,<a><i class="magic_172"><p>Sao Tome &amp; Principe (+239)</p></i></a>,<a><i class="magic_173"><p>Saudi Arabia (+966)</p></i></a>,<a><i class="magic_174"><p>Senegal (+221)</p></i></a>,<a><i class="magic_175"><p>serbia (+961)</p></i></a>,<a><i class="magic_176"><p>Seyshelles (+248)</p></i></a>,<a><i class="magic_177"><p>Sierra Leone (+232)</p></i></a>,<a><i class="magic_178"><p>Singapore (+65)</p></i></a>,<a><i class="magic_179"><p>Slovakia (+421)</p></i></a>,<a><i class="magic_180"><p>Slovenia (+386)</p></i></a>,<a><i class="magic_181"><p>Solomon Islands (+961)</p></i></a>,<a><i class="magic_182"><p>Somalia (+252)</p></i></a>,<a><i class="magic_183"><p>South Afriica (+27)</p></i></a>,<a><i class="magic_184"><p>Spain (+34)</p></i></a>,<a><i class="magic_185"><p>Sri Lanka (+94)</p></i></a>,<a><i class="magic_186"><p>Sudan (+249)</p></i></a>,<a><i class="magic_187"><p>Suriname (+597)</p></i></a>,<a><i class="magic_188"><p>Swaziland (+268)</p></i></a>,<a><i class="magic_189"><p>Sweden (+46)</p></i></a>,<a><i class="magic_190"><p>Switzerland (+41)</p></i></a>,<a><i class="magic_191"><p>Syria (+963)</p></i></a>,<a><i class="magic_192"><p>Taiwan (+886)</p></i></a>,<a><i class="magic_193"><p>Tajikistan (+992)</p></i></a>,<a><i class="magic_194"><p>Tanzania (+255)</p></i></a>,<a><i class="magic_195"><p>Thailand (+66)</p></i></a>,<a><i class="magic_196"><p>Togo (+228)</p></i></a>,<a><i class="magic_197"><p>Tonga (+676)</p></i></a>,<a><i class="magic_198"><p>Trinidad &amp; Tobago (+1-868)</p></i></a>,<a><i class="magic_199"><p>Tunisia (+216)</p></i></a>,<a><i class="magic_200"><p>Turkey (+961)</p></i></a>,<a><i class="magic_201"><p>Turks and Caicos Islands (+1-649)</p></i></a>,<a><i class="magic_202"><p>Tuvalu (+688)</p></i></a>,<a><i class="magic_203"><p>Uganda (+256)</p></i></a>,<a><i class="magic_204"><p>Ukraine (+380)</p></i></a>,<a><i class="magic_205"><p>United Arab Emirates (+971)</p></i></a>,<a><i class="magic_206"><p>Uruguay (+598)</p></i></a>,<a><i class="magic_207"><p>Uzbekistan (+998)</p></i></a>,<a><i class="magic_208"><p>Vanutau (+678)</p></i></a>,<a><i class="magic_209"><p>Vatican City (+39)</p></i></a>,<a><i class="magic_210"><p>Venezuela (+58)</p></i></a>,<a><i class="magic_211"><p>Viet Nam (+84)</p></i></a>,<a><i class="magic_212"><p>Virgin Islands British (+1-284)</p></i></a>,<a><i class="magic_213"><p>Virgin Islands US (+1-340)</p></i></a>,<a><i class="magic_214"><p>Wales (+681)</p></i></a>,<a><i class="magic_215"><p>Yemen (+967)</p></i></a>,<a><i class="magic_216"><p>Zambia (+260)</p></i></a>,<a><i class="magic_217"><p>Zimbabwe (+263)</p></i></a>,<a><i class="magic_218"><p>Alaska (+387)</p></i></a>,<a><i class="magic_219"><p>Ancension Island (+498)</p></i></a>,<a><i class="magic_220"><p>British Virgin Islands (+47)</p></i></a>,<a><i class="magic_221"><p>Christmas Islands (+404)</p></i></a>,<a><i class="magic_222"><p>Cocos Islands (+345)</p></i></a>,<a><i class="magic_223"><p>Czech Republic (+381)</p></i></a>,<a><i class="magic_224"><p>Diego Garcia (+81)</p></i></a>,<a><i class="magic_225"><p>Falkland Islands (+93)</p></i></a>,<a><i class="magic_226"><p>Finland (+95)</p></i></a>,<a><i class="magic_227"><p>Guantanamo Bay (+119)</p></i></a>,<a><i class="magic_228"><p>Hawaii (+411)</p></i></a>,<a><i class="magic_229"><p>Ivory Coast (+175)</p></i></a>,<a><i class="magic_230"><p>Laos (+187)</p></i></a>,<a><i class="magic_231"><p>Latvia (+188)</p></i></a>,<a><i class="magic_232"><p>Mayotte Island (+597)</p></i></a>,<a><i class="magic_233"><p>Niue Island (+235)</p></i></a>,<a><i class="magic_234"><p>Norfolk Island (+427)</p></i></a>,<a><i class="magic_235"><p>Saipan (+250)</p></i></a>,<a><i class="magic_236"><p>South Korea (+277)</p></i></a>,<a><i class="magic_237"><p>St. Helena (+284)</p></i></a>,<a><i class="magic_238"><p>St. Kitts & Nevis (+285)</p></i></a>,<a><i class="magic_239"><p>St. Martin (+287)</p></i></a>,<a><i class="magic_240"><p>St. Pierre & Mequelon(+288)</p></i></a>,<a><i class="magic_241"><p>St. Vincent (+379)</p></i></a>,<a><i class="magic_242"><p>West Samoa (+324)</p></i></a>,<a><i class="magic_243"><p>Yugoslavia(+326)</p></i></a>'.split(',');
    //for (var i = 0; i < cities.length; i++) jsonData.push({ id: i, name: cities[i], status: i % 2 ? 'Already Visited' : 'Planned for visit', coolness: Math.floor(Math.random() * 10) + 1 });

    $("#close-val1").click({ handler: lowestRateInfoViewModel.disablepopupval1 });
    ko.applyBindings(lowestRateInfoViewModel, document.getElementById("search-save"));
    var jsonData = [];

    var predata = [];
    $.ajax({
        url: "/Account/GetCountryToListFlag/",
        type: 'GET',
        success: function (data) {
            predata = data;

            $.each(data, function () {
               jsonData.push({ id: this.Id, cname: this.CountryFlag, name: this.Name, Code: this.CountCode, status: 'Already visited' });
            });

            var lowestrate = $('#ms9').magicSuggest({
                displayField: 'cname',
                allowFreeEntries: false,
                data: jsonData,
                resultAsString: true,
                maxSelection: 1,
                //SortDir: 'asc',
                strictSuggest: true,
                //sortOrder: 'cname',class=magic_1
                renderer: function (data) {
                    return '<div><a><i class="magic_' + data.id + '"><p>' + data.name + '(+' + data.Code + ')</p></i></a></div>';
                },
                //<a><i class="magic_1"><p>Afghanistan(+93)</p></i></a>
                //renderer: function (data) {
                //    return '<div>' +
                //        data.cname + '(+' + data.Code + ')' +
                //        '</div><div style="clear:both;"></div>';
                //},
                maxSelectionRenderer: function () {
                }
            });

            $(lowestrate).on('selectionchange', function (event, combo, selection) {
               // alert(lowestrate.getData());
                var selected = lowestrate.getSelectedItems();
                var countryto = selected[0].id;

                var countrytoname = selected[0].name;
                $("#hcountrytoname").val(countrytoname);

                if (isNaN(countryto)) {
                    return false;
                }
                var countryfrom = $("#stamp_flag").val();
                lowestRateInfoViewModel.LowestRate(countryfrom, countryto);
             
            });

        }
    });

    var countryid = $("#country-byip").val();
 

});





function LowestRateInfoViewModel() {

    var self = this;
    self.CallingFrom = ko.observable("");
    self.CallingTo = ko.observable("");
    self.LandlineRate = ko.observable("");
    self.MobileRate = ko.observable("");
    self.RateperSign = ko.observable("");
    self.CountryListFrom = ko.observableArray([]);
    self.CountryListTo = ko.observableArray([]);
    self.ErrorMessage = ko.observable("");
    self.BuyPlan = function () {
     
        if (self.CallingTo() == "") {
          
            return false;

        }

        var countryfromname = self.CallingFrom().replace("1", "usa").replace("2", "canada").replace("3","uk");
        //var countrytoname = this.Get_CountryTo_Name(self.CallingTo());
        var countrytoname = $("#hcountrytoname").val().replace(" ", "-").replace(" ","-").replace(" ","-");

        if (countrytoname != "")
            window.location.href = "/Rate/search-calling-card?from-" + countryfromname + "-to-" + countrytoname;
        else
            window.location.href = "/Rate/searchrate?countryfrom=" + self.CallingFrom() + "&countryto=" + self.CallingTo() + "";    
    };
    //enable and disable pop up code



    self.Get_CountryTo_Name = function (countryid) {
        if (countryid == "1")
            return "Afghanistan";
        else
            return "";
    }
    


   // get lowest rate.
    self.LowestRate = function (countryfrom, countryto) {
        self.CallingFrom(countryfrom);
        self.CallingTo(countryto);
      
        $.ajax({
            url: '/Account/GetLowestRate/',
            data: {
                CallingFrom: countryfrom,
                CallingTo: countryto
            },
            type: "GET",
            success: function (data) {
                self.LandlineRate(data.LandLineRate);
                self.MobileRate(data.MobileRate);
                self.RateperSign(data.RateperSign);
                var $this = $(this);
                
                $("#flipbox").flip({

                    direction: $this.attr("rel"),
                    color: $this.attr("rev"),
                    endColor: "grey",
                    content: $this.attr("title"),//(new Date()).getTime(),
                    onBefore: function () { $(".revert").show() },
                    onEnd: function () {
                        $("#flipbox").css({
                            color: "black",
                            background: "#e1e1e1"

                        });
                    }

                })
                return false;

            },
            error: function (jqXHR, textStatus, errorThrown) {
                //alert(errorThrown);
            }

        });

    };

};
   








                                                    




